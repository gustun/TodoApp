FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY TodoApp.Api/TodoApp.Api.csproj TodoApp.Api/
COPY TodoApp.DataAccess/TodoApp.DataAccess.csproj TodoApp.DataAccess/
COPY TodoApp.Common/TodoApp.Common.csproj TodoApp.Common/
RUN dotnet restore TodoApp.Api/TodoApp.Api.csproj
COPY . .
WORKDIR /src/TodoApp.Api
RUN dotnet build TodoApp.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TodoApp.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TodoApp.Api.dll"]
