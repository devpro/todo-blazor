# syntax=docker/dockerfile:1

FROM registry.suse.com/bci/dotnet-aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM registry.suse.com/bci/dotnet-sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ./Directory.*.props .
COPY ./src ./src
RUN dotnet restore "src/BlazorApp/BlazorApp.csproj"
RUN dotnet build "src/BlazorApp/BlazorApp.csproj" --no-restore -c "$BUILD_CONFIGURATION"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN dotnet publish "src/BlazorApp/BlazorApp.csproj" --no-build -c "$BUILD_CONFIGURATION" \
    -o /app/publish /p:UseAppHost=false
RUN --mount=type=secret,id=additional_file_name,env=ADDITIONAL_FILE_NAME \
    --mount=type=secret,id=additional_file_content,env=ADDITIONAL_FILE_CONTENT \
    if [ -n "$ADDITIONAL_FILE_NAME" ] && [ -n "$ADDITIONAL_FILE_CONTENT" ]; then \
        echo "$ADDITIONAL_FILE_CONTENT" > "/app/publish/${ADDITIONAL_FILE_NAME}"; \
    fi

FROM registry.suse.com/bci/python:3.13 AS otel-fetch
ARG OTEL_VERSION=1.14.1
WORKDIR /tmp
RUN curl --proto '=https' --fail-with-body -L -o checksums.txt \
    "https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/download/v${OTEL_VERSION}/checksums.txt" && \
    curl --proto '=https' --fail-with-body -L -o otel.zip \
    "https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/download/v${OTEL_VERSION}/opentelemetry-dotnet-instrumentation-linux-glibc-x64.zip" && \
    awk '/opentelemetry-dotnet-instrumentation-linux-glibc-x64\.zip/ { gsub(/\*/, ""); print $1 "  otel.zip" }' checksums.txt | \
    sha256sum -c - && \
    python3 -c 'import zipfile; zipfile.ZipFile("otel.zip").extractall("/otel-ai")' && \
    rm -f otel.zip checksums.txt

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=otel-fetch /otel-ai/AdditionalDeps/shared/Microsoft.NETCore.App/10.0.0 /otel-ai/AdditionalDeps/shared/Microsoft.NETCore.App/10.0.0/
COPY --from=otel-fetch /otel-ai/linux-x64/OpenTelemetry.AutoInstrumentation.Native.so /otel-ai/linux-x64/
COPY --from=otel-fetch /otel-ai/net/*.* /otel-ai/net/
COPY --from=otel-fetch /otel-ai/store/x64/net10.0 /otel-ai/store/x64/net10.0/
USER root
RUN chown -R $APP_UID:$APP_UID /otel-ai && \
    chmod -R 755 /otel-ai
USER $APP_UID
ENV OTEL_DOTNET_AUTO_HOME=/otel-ai \
    CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={918728DD-259F-4A6A-AC2B-B85E1B658318} \
    CORECLR_PROFILER_PATH=/otel-ai/linux-x64/OpenTelemetry.AutoInstrumentation.Native.so \
    DOTNET_ADDITIONAL_DEPS=/otel-ai/AdditionalDeps \
    DOTNET_SHARED_STORE=/otel-ai/store \
    DOTNET_STARTUP_HOOKS=/otel-ai/net/OpenTelemetry.AutoInstrumentation.StartupHook.dll \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_NOLOGO=1
ENTRYPOINT ["dotnet", "Devpro.TodoList.BlazorApp.dll"]
