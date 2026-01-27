FROM registry.suse.com/bci/dotnet-aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM registry.suse.com/bci/dotnet-sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/", "./"]
RUN dotnet restore "BlazorApp/BlazorApp.csproj"
COPY . .
WORKDIR "/src/BlazorApp"
RUN dotnet build "BlazorApp.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG ADDITIONAL_FILE_NAME=""
ARG ADDITIONAL_FILE_CONTENT=""
RUN dotnet publish "BlazorApp.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false
RUN if [ -n "$ADDITIONAL_FILE_NAME" ] && [ -n "$ADDITIONAL_FILE_CONTENT" ]; then echo "$ADDITIONAL_FILE_CONTENT" > "/app/publish/${ADDITIONAL_FILE_NAME}"; fi

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Devpro.TodoList.BlazorApp.dll"]
