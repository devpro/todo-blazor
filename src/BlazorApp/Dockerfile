# syntax=docker/dockerfile:1

FROM registry.suse.com/bci/dotnet-aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM registry.suse.com/bci/dotnet-sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
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

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Devpro.TodoList.BlazorApp.dll"]
