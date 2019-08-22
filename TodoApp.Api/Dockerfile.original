FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["TodoApp.Api/TodoApp.Api.csproj", "TodoApp.Api/"]
COPY ["TodoApp.DataAccess/TodoApp.DataAccess.csproj", "TodoApp.DataAccess/"]
COPY ["TodoApp.Common/TodoApp.Common.csproj", "TodoApp.Common/"]
RUN dotnet restore "TodoApp.Api/TodoApp.Api.csproj"
COPY . .
WORKDIR "/src/TodoApp.Api"
RUN dotnet build "TodoApp.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TodoApp.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TodoApp.Api.dll"]