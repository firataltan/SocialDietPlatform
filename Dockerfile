FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SocialDietPlatform.sln", "./"]
COPY ["src/*/*.csproj", "./"]
RUN for file in $(ls *.csproj); do mkdir -p src/$(basename $file .csproj) && mv $file src/$(basename $file .csproj)/; done
RUN dotnet restore
COPY . .
WORKDIR "/src"
RUN dotnet build "SocialDietPlatform.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SocialDietPlatform.sln" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SocialDietPlatform.Web.dll"] 