FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV TZ=America/Sao_Paulo

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ./src .
RUN dotnet restore "Chat/Chat.UI.csproj"
RUN dotnet build "Chat/Chat.UI.csproj" --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish "Chat/Chat.UI.csproj" -c Release -o /app

FROM base AS final
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 80
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Chat.UI.dll"]