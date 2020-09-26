#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

EXPOSE 8001
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "BlazorCommunity.Api.dll","urls=http://*:8001"]
