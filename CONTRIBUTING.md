# Contribution guide

## Debug the application

Start MongoDB in a container (Docker or another container engine must be running):

```bash
docker run --name mongodb8 -d -p 27017:27017 mongo:8.2
```

Run the web API ([.NET 10](https://dotnet.microsoft.com/download) must be installed):

```bash
dotnet run --project src/BlazorApp
```
