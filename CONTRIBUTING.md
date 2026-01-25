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

    - **Option 2**: Run in a container (assuming Docker is present)

      - Build the container

      ```bash
      docker build . -t devprofr/todoblazor:local -f src/BlazorApp/Dockerfile
      ```
      
      - Run the container

      ```bash
      docker run -it --rm --link "mongodb8" \
        -p 9001:8080 -e ASPNETCORE_ENVIRONMENT=Development \
        -e DatabaseSettings__ConnectionString=mongodb://mongodb8:27017 \
        devprofr/todoblazor:local
      ```

      - Open [localhost:9001](http://localhost:9001)

    - **Option 3**: Debug in an IDE (Visual Studio 2022/2026 or Rider)

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
