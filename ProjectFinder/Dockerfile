# aluskiht
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
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
COPY Base.Helpers/*.csproj ./Base.Helpers/
COPY Base.Resources/*.csproj ./Base.Resources/

COPY App.BLL/*.csproj ./App.BLL/
COPY App.BLL.Contracts/*.csproj ./App.BLL.Contracts/
COPY App.BLL.DTO/*.csproj ./App.BLL.DTO/
COPY App.DAL.Contracts/*.csproj ./App.DAL.Contracts/
COPY App.DAL.DTO/*.csproj ./App.DAL.DTO/
COPY App.DAL.EF/*.csproj ./App.DAL.EF/
COPY App.DTO/*.csproj ./App.DTO/
COPY App.Domain/*.csproj ./App.Domain/
COPY App.Resources/*.csproj ./App.Resources/
COPY App.Tests/*.csproj ./App.Tests/

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
COPY Base.Helpers/. ./Base.Helpers/
COPY Base.Resources/. ./Base.Resources/

COPY BLL/. ./BLL/
COPY BLL.Contracts/. ./BLL.Contracts/
COPY BLL.DTO/. ./BLL.DTO/
COPY DAL.Contracts/. ./DAL.Contracts/
COPY DAL.DTO/. ./DAL.DTO/
COPY DAL.EF/. ./DAL.EF/
COPY DTO/. ./DTO/
COPY Domain/. ./Domain/
COPY Resources/. ./Resources/
COPY Tests/. ./Tests/

COPY WebApp/. ./WebApp/

RUN dotnet publish -c Release -o out

# jooksuta testid, peatub vea korral
RUN dotnet test --filter FullyQualifiedName~Tests.Integration.Api.IdentityTests
#RUN dotnet test --filter FullyQualifiedName~Tests.Integration.Api.TaskTests
#RUN dotnet test --filter FullyQualifiedName~Tests.Integration.Mvc.IdentityTests
#RUN dotnet test --filter FullyQualifiedName~Tests.Integration.Mvc.TaskTests
RUN dotnet test --filter FullyQualifiedName~Tests.Unit.Base.BaseRepositoryTest
RUN dotnet test --filter FullyQualifiedName~Tests.Unit.Base.BaseServiceTest
#RUN dotnet test --filter FullyQualifiedName~Tests.Unit.App.TaskRepositoryTest
#RUN dotnet test --filter FullyQualifiedName~Tests.Unit.App.TaskServiceTest

# tekita uus image
# intel cpu image - aspnet:9.0-bookworm-slim-amd64 AS runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim-amd64 AS runtime
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/out ./
ENV ConnectionStrings:DefaultConnection="Host=host.docker.internal;Port=5432;Database=taskbase;Username=postgres;Password=postgres"
ENTRYPOINT ["dotnet", "WebApp.dll"]
