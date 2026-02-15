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

<!--
using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
var page = await browser.NewPageAsync();
await page.GotoAsync("https://playwright.dev/dotnet");
await page.ScreenshotAsync(new() { Path = "screenshot.png" });
-->

## IDE

### Rider

Install [Reqnroll extension](https://docs.reqnroll.net/latest/installation/setup-ide.html#setup-rider) 

The file `TodoBlazor.sln.DotSettings` is versioned and contains the following fix:

- Remove duplication in the test view:

  - **File** > **Settings** > **Build, Execution, Deployment** > **Unit Testing** > **VSTest**: `Enable VSTest adapters support` must be unchecked
    (see [xunit/visualstudio.xunit/issues/436](https://github.com/xunit/visualstudio.xunit/issues/436#issuecomment-2687240662))

### Visual Studio 2022/2026

Install [Reqnroll extension](https://docs.reqnroll.net/latest/installation/setup-ide.html#setup-visual-studio)

## Limitations

NuGet packages:

- xunit.v3 3.2.2 doesn't work with Microsoft.Testing.Platform 2 (and as a consequence with JunitXml.TestLogger 8)
- FIXED ~~Keep version 9 of ASP.NET EF (Entity Framework) for now, as version 10 introduces breaking changes for MongoDB EF Provider 9~~

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
