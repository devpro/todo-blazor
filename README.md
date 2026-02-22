# Todo Blazor (Web Application)

[![CI](https://github.com/devpro/todo-blazor/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/devpro/todo-blazor/actions/workflows/ci.yml)
[![PKG](https://github.com/devpro/todo-blazor/actions/workflows/pkg.yml/badge.svg?branch=main)](https://github.com/devpro/todo-blazor/actions/workflows/pkg.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=devpro_todo-blazor&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=devpro_todo-blazor)
[![Docker Image Version](https://img.shields.io/docker/v/devprofr/todoblazor?label=Image&logo=docker)](https://hub.docker.com/r/devprofr/todoblazor)
[![FOSSA Status](https://app.fossa.com/api/projects/custom%2B60068%2Fgithub.com%2Fdevpro%2Ftodo-blazor.svg?type=shield&issueType=license)](https://app.fossa.com/projects/custom%2B60068%2Fgithub.com%2Fdevpro%2Ftodo-blazor?ref=badge_shield&issueType=license)
[![FOSSA Status](https://app.fossa.com/api/projects/custom%2B60068%2Fgithub.com%2Fdevpro%2Ftodo-blazor.svg?type=shield&issueType=security)](https://app.fossa.com/projects/custom%2B60068%2Fgithub.com%2Fdevpro%2Ftodo-blazor?ref=badge_shield&issueType=security)

A simple todo list that runs in a web application powered Blazor and written in C# / .NET.

For development details, see [CONTRIBUTING.md](CONTRIBUTING.md) - contributions are more than welcome 🙂

## Quick demo (containers/Docker)

Start the database:

```bash
docker run --name mongodb8 -d -p 27017:27017 mongo:8.2
```

Run the web application:

```bash
docker run --rm -p 9001:8080 --link "mongodb8" -e DatabaseSettings__ConnectionString=mongodb://mongodb8:27017 docker.io/devprofr/todoblazor:latest
```

Open [localhost:9001](http://localhost:9001)

## Deployment

Use the [Helm chart](https://github.com/devpro/helm-charts/tree/main/charts/todoblazor).

## Backlog

- [x] Auth with users in MongoDB
- [x] Todo list page
- [x] Dockerfile
- [x] Docker compose
- [x] Helm chart
- [x] Integration tests (xUnit v3)
- [x] Code scan (linters, Sonar)
- [x] CI/CD
- [x] Badges in README
- [x] Secret check (GitGuardian)
- [x] License check (FOSSA)
- [x] Health check (with db check)
- [x] End-to-end tests (Playwright)
- [x] BDD/Gherkin (Reqnroll)
- [ ] Observability (OpenTelemetry SDK/Instrumentation)
