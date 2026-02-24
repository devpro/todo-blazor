# Contribution guide

## Run locally

### Individual components

1. Database (MongoDB)

    - Run in a container (assuming Docker is present):

      ```bash
      docker run --name mongodb8 -d -p 27017:27017 mongo:8.2
      ```

2. Web application (Blazor / .NET / C#)

    - **Option 1**: Run from the command line (assuming [.NET 10](https://dotnet.microsoft.com/download) is installed)

      - Use .NET CLI:

        ```bash
        dotnet run --project src/BlazorApp
        ```

      - Open [localhost:5147](http://localhost:5147)

    - **Option 2**: Run in a container

      - Build the container

      ```bash
      docker build . -t devprofr/todoblazor:local -f src/BlazorApp/Dockerfile
      ```

      <!--
      other commands that may be good to know in some edge cases:
      docker build . -t devprofr/todoblazor:local -f src/BlazorApp/Dockerfile --build-arg ADDITIONAL_FILE_NAME=titi.txt --build-arg ADDITIONAL_FILE_CONTENT="toto"
      docker run -it --rm --name todoblazorlocal devprofr/todoblazor:local
      docker exec -it todoblazorlocal bash
      docker login
      docker build . -t devprofr/todoblazor:latest -f src/BlazorApp/Dockerfile
      docker push devprofr/todoblazor:latest
      docker compose build --no-cache
      DOCKER_BUILDKIT=1 docker compose build --no-cache --progress=plain
      -->

      - Run the container

      ```bash
      docker run -it --rm --link "mongodb8" \
        -p 9001:8080 -e ASPNETCORE_ENVIRONMENT=Development \
        -e DatabaseSettings__ConnectionString=mongodb://mongodb8:27017 \
        devprofr/todoblazor:local
      ```

      - Open [localhost:9001](http://localhost:9001)

    - **Option 3**: Debug in an IDE

3. Clean-up

```bash
docker stop mongodb8
docker rm mongodb8
```

### All-in-one

- Start all containers (assuming Docker Compose is present):

```bash
docker compose up --build
```

- Open [localhost:9001](http://localhost:9001)

- Clean-up:

```bash
docker compose down
```

If there is an issue with OpenTelemetry auto-instrumentation:

- Look in `/logs/otel`

- Switch to console exporter (instead of otlp) and set log level to debug

```bash
services:
  webapp:
    image: devprofr/todoblazor:latest
    environment:
      # ...
      - OTEL_TRACES_EXPORTER=console
      - OTEL_METRICS_EXPORTER=console
      - OTEL_LOGS_EXPORTER=console
      - OTEL_LOG_LEVEL=debug
```

### Test setup

Make sure PowerShell 7+ is installed:

```dos
winget install --id Microsoft.PowerShell --source winget
```

Install required browsers:

```dos
pwsh test/BlazorApp.PlaywrightTests/bin/Debug/net8.0/playwright.ps1 install
```

Generate tests with:

```dos
pwsh test/BlazorApp.PlaywrightTests/bin/Debug/net10.0/playwright.ps1 codegen https://localhost:7099/
```

## IDE

### Rider

Install [Reqnroll extension](https://docs.reqnroll.net/latest/installation/setup-ide.html#setup-rider)

To disable Playwright headless browser testing (see what's really happening):

- Open "File | Settings | Build, Execution, Deployment | Unit Testing | Test Runner"
- Add an environment variable: "HEADED" = 1

The file `TodoBlazor.sln.DotSettings` is versioned and contains the following fix:

- Remove duplication in the test view:

  - **File** > **Settings** > **Build, Execution, Deployment** > **Unit Testing** > **VSTest**: `Enable VSTest adapters support` must be unchecked
    (see [xunit/visualstudio.xunit/issues/436](https://github.com/xunit/visualstudio.xunit/issues/436#issuecomment-2687240662))

Shortcuts (assuming **Visual Studio** mapping):

- `Ctrl`+`Alt`+`Enter` formats the current file
- `Ctrl`+`Alt`+`/` (num) comments/uncomments the selected lines

### Visual Studio 2022/2026

Install [Reqnroll extension](https://docs.reqnroll.net/latest/installation/setup-ide.html#setup-visual-studio)

## Limitations

### NuGet packages

- xunit.v3 3.2.2 doesn't work with Microsoft.Testing.Platform 2 (and as a consequence of JunitXml.TestLogger 8)
- FIXED ~~Keep version 9 of ASP.NET EF (Entity Framework) for now, as version 10 introduces breaking changes for MongoDB EF Provider 9~~

### OpenTelemetry auto-instrumentation

Specific instrumentation can be disabled from the configuration:

```yaml
services:
  webapp:
    environment:
      # ...
      # - OTEL_DOTNET_AUTO_TRACES_ENTITYFRAMEWORKCORE_INSTRUMENTATION_ENABLED=false
```

MongoDB instrumentation had to be disabled (supposed to work but maybe an issue with ASP.NET Identity/Entity Framework provider):

```yaml
services:
  webapp:
    environment:
      # ...
      # - OTEL_DOTNET_AUTO_INSTRUMENTATIONS=AspNetCore,HttpClient,MongoDB
      - OTEL_DOTNET_AUTO_INSTRUMENTATIONS=AspNetCore,HttpClient
```

Because of the error seen in otel logs:

> The property or field 'EndPoint' for the proxy property 'EndPoint' was not found in the instance of type 'MongoDB.Driver.OperationContext'

<!--
[2026-02-24T00:14:10.3837534Z] [Error] The type initializer for 'OpenTelemetry.AutoInstrumentation.CallTarget.Handlers.BeginMethodHandler`4' threw an exception.
Exception: The type initializer for 'OpenTelemetry.AutoInstrumentation.CallTarget.Handlers.BeginMethodHandler`4' threw an exception.
System.TypeInitializationException: The type initializer for 'OpenTelemetry.AutoInstrumentation.CallTarget.Handlers.BeginMethodHandler`4' threw an exception.
 ---> OpenTelemetry.AutoInstrumentation.CallTarget.CallTargetInvokerException: The property or field 'EndPoint' for the proxy property 'EndPoint' was not found in the instance of type 'MongoDB.Driver.OperationContext'.
---> OpenTelemetry.AutoInstrumentation.DuckTyping.DuckTypePropertyOrFieldNotFoundException: The property or field 'EndPoint' for the proxy property 'EndPoint' was not found in the instance of type 'MongoDB.Driver.OperationContext'.
at OpenTelemetry.AutoInstrumentation.DuckTyping.DuckTypePropertyOrFieldNotFoundException.Throw(String name, String duckAttributeName, Type type)
at OpenTelemetry.AutoInstrumentation.DuckTyping.DuckType.CreateProperties(TypeBuilder proxyTypeBuilder, Type proxyDefinitionType, Type targetType, FieldInfo instanceField)
at OpenTelemetry.AutoInstrumentation.DuckTyping.DuckType.CreateProxyType(Type proxyDefinitionType, Type targetType, Boolean dryRun)
--- End of stack trace from previous location ---
at OpenTelemetry.AutoInstrumentation.CallTarget.Handlers.IntegrationMapper.CreateBeginMethodDelegate(Type integrationType, Type targetType, Type[] argumentsTypes)
at OpenTelemetry.AutoInstrumentation.CallTarget.Handlers.BeginMethodHandler`4..cctor()
   --- End of inner exception stack trace ---
   at OpenTelemetry.AutoInstrumentation.CallTarget.Handlers.BeginMethodHandler`4..cctor()
--- End of inner exception stack trace ---
at MongoDB.Driver.Core.WireProtocol.CommandUsingCommandMessageWireProtocol`1.Execute(OperationContext operationContext, IConnection connection)
-->

Using [MongoDB.Driver.Core.Extensions.DiagnosticSources](https://github.com/jbogard/MongoDB.Driver.Core.Extensions.DiagnosticSources) means referencing OpenTelemetry + config in the code.

## Quality gates

### yamllint

```bash
docker run --rm -v "$(pwd)":/data cytopia/yamllint .
```

## Operations

### Checking image signature

<!--All images pushed to DockerHub are signed using Cosign (keyless mode via GitHub OIDC).-->

```bash
# predicts signature location
docker run --rm ghcr.io/sigstore/cosign/cosign:latest \
  triangulate docker.io/devprofr/todoblazor:1.0.21711317943

# verifies signature (recommended)
docker run --rm ghcr.io/sigstore/cosign/cosign:latest \
  verify \
    --certificate-oidc-issuer=https://token.actions.githubusercontent.com \
    --certificate-identity-regexp="https://github.com/devpro/todo-blazor/.github/workflows/pkg.yml@.*" \
    docker.io/devprofr/todoblazor:1.0.21711317943
```

## References

.NET documentation:

- [Introduction to Identity on ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [ASP.NET Core Blazor authentication and authorization](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/)

Thanks to Miiro Ashirafu for his example on Blazor user identity with MongoDB: [ashirafumiiro/MongoDBEFCoreWeatherApp](https://github.com/ashirafumiiro/MongoDBEFCoreWeatherApp).

Blazor samples ([Blazor movie database app](https://learn.microsoft.com/en-us/aspnet/core/blazor/tutorials/movie-database-app/)):

```bash
git clone https://github.com/dotnet/blazor-samples.git
cd blazor-samples/10.0/BlazorWebAppMovies
dotnet tool restore
dotnet ef database update
dotnet run
```
