FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY TodoApp.Api/TodoApp.Api.csproj TodoApp.Api/
COPY TodoApp.DataAccess/TodoApp.DataAccess.csproj TodoApp.DataAccess/
COPY TodoApp.Common/TodoApp.Common.csproj TodoApp.Common/
RUN dotnet restore TodoApp.Api/TodoApp.Api.csproj

COPY . .
WORKDIR /app/TodoApp.Api
FROM build AS publish
RUN dotnet publish TodoApp.Api.csproj -c Release -o "../out" --no-restore

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS final
WORKDIR /app
COPY --from=publish /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "TodoApp.Api.dll"]
