# ------------------------------------------
# Create MyMovies Applications Solutions
# ------------------------------------------
pause
mkdir MyMovies
cd MyMovies
# ------------------------------------------
# Create Persons Web API
# ------------------------------------------
mkdir MyMovies.Persons
cd MyMovies.Persons
dotnet new sln
dotnet new classlib --name=MyMovies.PersonsLibrary.Data
dotnet new classlib --name=MyMovies.PersonsLibrary.Business
dotnet new classlib --name=MyMovies.PersonsLibrary.Domain
dotnet new mstest --name=MyMovies.PersonsTest.UnitTests
dotnet new webapi --name=MyMovies.PersonsApi.WebApp

dotnet sln add ./MyMovies.PersonsLibrary.Data/MyMovies.PersonsLibrary.Data.csproj --solution-folder Core
dotnet sln add ./MyMovies.PersonsLibrary.Business/MyMovies.PersonsLibrary.Business.csproj --solution-folder Core
dotnet sln add ./MyMovies.PersonsLibrary.Domain/MyMovies.PersonsLibrary.Domain.csproj --solution-folder Core
dotnet sln add ./MyMovies.PersonsTest.UnitTests/MyMovies.PersonsTest.UnitTests.csproj --solution-folder Test
dotnet sln add ./MyMovies.PersonsApi.WebApp/MyMovies.PersonsApi.WebApp.csproj --solution-folder Web

cd MyMovies.PersonsLibrary.Data
dotnet add reference ../MyMovies.PersonsLibrary.Domain/MyMovies.PersonsLibrary.Domain.csproj 
cd..
cd MyMovies.PersonsLibrary.Business
dotnet add reference ../MyMovies.PersonsLibrary.Data/MyMovies.PersonsLibrary.Data.csproj 
cd..
cd MyMovies.PersonsTest.UnitTests
dotnet add reference ../MyMovies.PersonsLibrary.Business/MyMovies.PersonsLibrary.Business.csproj 
cd..
cd MyMovies.PersonsApi.WebApp
dotnet add reference ../MyMovies.PersonsLibrary.Business/MyMovies.PersonsLibrary.Business.csproj 
cd ..
# ------------------------------------------
# Create Movies Web API
# ------------------------------------------
cd ..
mkdir MyMovies.Movies
cd MyMovies.Movies
dotnet new sln
dotnet new classlib --name=MyMovies.MoviesLibrary.Data
dotnet new classlib --name=MyMovies.MoviesLibrary.Business
dotnet new classlib --name=MyMovies.MoviesLibrary.Domain
dotnet new mstest --name=MyMovies.MoviesTest.UnitTests
dotnet new webapi --name=MyMovies.MoviesApi.WebApp

dotnet sln add ./MyMovies.MoviesLibrary.Data/MyMovies.MoviesLibrary.Data.csproj --solution-folder Core
dotnet sln add ./MyMovies.MoviesLibrary.Business/MyMovies.MoviesLibrary.Business.csproj --solution-folder Core
dotnet sln add ./MyMovies.MoviesLibrary.Domain/MyMovies.MoviesLibrary.Domain.csproj --solution-folder Core
dotnet sln add ./MyMovies.MoviesTest.UnitTests/MyMovies.MoviesTest.UnitTests.csproj --solution-folder Test
dotnet sln add ./MyMovies.MoviesApi.WebApp/MyMovies.MoviesApi.WebApp.csproj --solution-folder Web
cd MyMovies.MoviesLibrary.Data
dotnet add reference ../MyMovies.MoviesLibrary.Domain/MyMovies.MoviesLibrary.Domain.csproj
cd..
cd MyMovies.MoviesLibrary.Business
dotnet add reference ../MyMovies.MoviesLibrary.Data/MyMovies.MoviesLibrary.Data.csproj
cd..
cd MyMovies.MoviesTest.UnitTests
dotnet add reference ../MyMovies.MoviesLibrary.Business/MyMovies.MoviesLibrary.Business.csproj 
cd..
cd MyMovies.MoviesApi.WebApp
dotnet add reference ../MyMovies.MoviesLibrary.Business/MyMovies.MoviesLibrary.Business.csproj 
cd ..
# ------------------------------------------
# Create Web Page Application
# ------------------------------------------
cd ..
mkdir MyMovies.WebApp
cd MyMovies.WebApp
dotnet new sln
dotnet new mvc --name=MyMovies.WebApp
dotnet sln add ./MyMovies.WebApp/MyMovies.WebApp.csproj
cd ..

