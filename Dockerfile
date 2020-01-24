FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

COPY ./ ./
RUN dotnet restore TodoApp.sln
RUN dotnet publish TodoApp.Api/TodoApp.Api.csproj -c Release -o "../out" --no-restore

FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 7002
ENTRYPOINT ["dotnet", "TodoApp.Api.dll"]