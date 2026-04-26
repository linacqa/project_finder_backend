# aluskiht
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
# tee kataloog /app vmis ja vaheta aktiivne kataloog sinna
WORKDIR /app
# kopeeri solution hetke hosti kataloogist konteineri aktiivsesse kataloogi
# kustkohast-> kuhu
COPY *.sln .

# kopeeri kõik projektid solutionist

COPY Base.BLL/*.csproj ./Base.BLL/
COPY Base.BLL.Contracts/*.csproj ./Base.BLL.Contracts/
COPY Base.Contracts/*.csproj ./Base.Contracts/
COPY Base.DAL.Contracts/*.csproj ./Base.DAL.Contracts/
COPY Base.DAL.EF/*.csproj ./Base.DAL.EF/
COPY Base.Domain/*.csproj ./Base.Domain/
COPY Base.DTO/*.csproj ./Base.DTO/
COPY Base.Helpers/*.csproj ./Base.Helpers/

COPY BLL/*.csproj ./BLL/
COPY BLL.Contracts/*.csproj ./BLL.Contracts/
COPY BLL.DTO/*.csproj ./BLL.DTO/
COPY DAL.Contracts/*.csproj ./DAL.Contracts/
COPY DAL.DTO/*.csproj ./DAL.DTO/
COPY DAL.EF/*.csproj ./DAL.EF/
COPY DTO/*.csproj ./DTO/
COPY Domain/*.csproj ./Domain/
#COPY App.Tests/*.csproj ./App.Tests/

COPY WebApp/*.csproj ./WebApp/

# taasta nuget paketid konteineris
RUN dotnet restore


# kopeeri kogu lähtekood ja ehita rakendus
COPY Base.BLL/. ./Base.BLL/
COPY Base.BLL.Contracts/. ./Base.BLL.Contracts/
COPY Base.Contracts/. ./Base.Contracts/
COPY Base.DAL.Contracts/. ./Base.DAL.Contracts/
COPY Base.DAL.EF/. ./Base.DAL.EF/
COPY Base.Domain/. ./Base.Domain/
COPY Base.DTO/. ./Base.DTO/
COPY Base.Helpers/. ./Base.Helpers/

COPY BLL/. ./BLL/
COPY BLL.Contracts/. ./BLL.Contracts/
COPY BLL.DTO/. ./BLL.DTO/
COPY DAL.Contracts/. ./DAL.Contracts/
COPY DAL.DTO/. ./DAL.DTO/
COPY DAL.EF/. ./DAL.EF/
COPY DTO/. ./DTO/
COPY Domain/. ./Domain/
#COPY Tests/. ./Tests/

COPY WebApp/. ./WebApp/

WORKDIR /app/WebApp
RUN dotnet publish -c Release -o out

# tekita uus image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/WebApp/out ./
ENTRYPOINT ["dotnet", "WebApp.dll"]
