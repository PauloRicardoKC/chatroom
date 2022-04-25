#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["Chat.UI/Chat.UI.csproj", "Chat.UI/"]
#COPY ["Chat.Application/Chat.Application.csproj", "Chat.Application/"]
#COPY ["Chat.Domain/Chat.Domain.csproj", "Chat.Domain/"]
#COPY ["Chat.Infra.Data/Chat.Infra.Data.csproj", "Chat.Infra.Data/"]
#RUN dotnet restore "Chat.UI/Chat.UI.csproj"
#COPY . .
#WORKDIR "/src/Chat.UI"
#RUN dotnet build "Chat.UI.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Chat.UI.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Chat.UI.dll"]

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