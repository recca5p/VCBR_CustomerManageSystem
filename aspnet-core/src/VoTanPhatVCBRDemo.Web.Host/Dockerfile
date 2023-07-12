FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/VoTanPhatVCBRDemo.Web.Host/VoTanPhatVCBRDemo.Web.Host.csproj", "src/VoTanPhatVCBRDemo.Web.Host/"]
COPY ["src/VoTanPhatVCBRDemo.Web.Core/VoTanPhatVCBRDemo.Web.Core.csproj", "src/VoTanPhatVCBRDemo.Web.Core/"]
COPY ["src/VoTanPhatVCBRDemo.Application/VoTanPhatVCBRDemo.Application.csproj", "src/VoTanPhatVCBRDemo.Application/"]
COPY ["src/VoTanPhatVCBRDemo.Core/VoTanPhatVCBRDemo.Core.csproj", "src/VoTanPhatVCBRDemo.Core/"]
COPY ["src/VoTanPhatVCBRDemo.EntityFrameworkCore/VoTanPhatVCBRDemo.EntityFrameworkCore.csproj", "src/VoTanPhatVCBRDemo.EntityFrameworkCore/"]
WORKDIR "/src/src/VoTanPhatVCBRDemo.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/VoTanPhatVCBRDemo.Web.Host", "src/VoTanPhatVCBRDemo.Web.Host"]
COPY ["src/VoTanPhatVCBRDemo.Web.Core", "src/VoTanPhatVCBRDemo.Web.Core"]
COPY ["src/VoTanPhatVCBRDemo.Application", "src/VoTanPhatVCBRDemo.Application"]
COPY ["src/VoTanPhatVCBRDemo.Core", "src/VoTanPhatVCBRDemo.Core"]
COPY ["src/VoTanPhatVCBRDemo.EntityFrameworkCore", "src/VoTanPhatVCBRDemo.EntityFrameworkCore"]
WORKDIR "/src/src/VoTanPhatVCBRDemo.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "VoTanPhatVCBRDemo.Web.Host.dll"]
