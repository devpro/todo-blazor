# Todo Blazor (Web Application)

[![CI](https://github.com/devpro/todo-blazor/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/devpro/todo-blazor/actions/workflows/ci.yml)
[![PKG](https://github.com/devpro/todo-blazor/actions/workflows/pkg.yml/badge.svg?branch=main)](https://github.com/devpro/todo-blazor/actions/workflows/pkg.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=devpro_todo-blazor&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=devpro_todo-blazor)
[![Docker Image Version](https://img.shields.io/docker/v/devprofr/todoblazor?label=Image&logo=docker)](https://hub.docker.com/r/devprofr/todoblazor)
[![FOSSA Status](https://app.fossa.com/api/projects/custom%2B60068%2Fgithub.com%2Fdevpro%2Ftodo-blazor.svg?type=shield&issueType=license)](https://app.fossa.com/projects/custom%2B60068%2Fgithub.com%2Fdevpro%2Ftodo-blazor?ref=badge_shield&issueType=license)
[![FOSSA Status](https://app.fossa.com/api/projects/custom%2B60068%2Fgithub.com%2Fdevpro%2Ftodo-blazor.svg?type=shield&issueType=security)](https://app.fossa.com/projects/custom%2B60068%2Fgithub.com%2Fdevpro%2Ftodo-blazor?ref=badge_shield&issueType=security)

A simple todo list that runs as a web application (written with Blazor / C# / .NET).

For development details, see [CONTRIBUTING.md](CONTRIBUTING.md) - contributions are more than welcome 🙂

## Artifacts

### Container images

All images pushed to DockerHub are signed using Cosign (keyless mode via GitHub OIDC).

```bash
# predicts signature location
docker run --rm ghcr.io/sigstore/cosign/cosign:latest \
  triangulate docker.io/devprofr/todoblazor:1.0.21564931504

# verifies signature (recommended)
docker run --rm ghcr.io/sigstore/cosign/cosign:latest \
  verify \
    --certificate-oidc-issuer=https://token.actions.githubusercontent.com \
    --certificate-identity-regexp="https://github.com/devpro/todo-blazor/.github/workflows/pkg.yml@.*" \
    docker.io/devprofr/todoblazor:1.0.21564931504
```
