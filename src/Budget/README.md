# Budget

Simple budget tracking system

## Getting Started

The application has Docker support. It can be launched both in a container & in Kestrel.

### Debugging in Container

There is `docker-compose` support and that's how the project should be launched in a container.

1. In VS Code right click on `docker-compose.debug.yml` -> `Compose Up`
2. Attach to the contianer:
    1. Go to `Run and Debug`
    2. Click on `Docker .NET Core Attach`
    3. Select the container that was started in the compose step

### Launching 

The application can be launched also with Kestrel using the `.NET Core Launch (web)` option in VS Code

### Dependencies

* .NET 6
