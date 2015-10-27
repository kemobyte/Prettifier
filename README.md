# Prettifier
Prettifier App - ASP.NET MVC

# Get Started
This is an ASP.NET MVC 5 Web application project with Entity Framework 6 and developed using a Visual Studio 2015 Community Edition. 
This project targets .NET Framework 4.6

# Prerequisites:  
1- Install Visual Studio 2015 Community Edition. Download from https://go.microsoft.com/fwlink/?LinkId=532606&clcid=0x409   
 - Ensure Visual Studio Web Development Tools is checked to get installed during Visual Studio 2015 installation   
 - Ensure SQL Server Data Tools (SSDT) is checked to get installed during Visual Studio 2015 installation  
 
2- Install NuGet latest (v3.2.0) Package Manager, Download latest version from https://dist.nuget.org/win-x86-commandline/latest/nuget.exe  

# Database
Prittifer Databse will be created automatically as localDB stored in the App_Data folder the first time you run the project in the browser.

# How to Build the Prettifier
1- Inside the Source Folder, Open the solution ".sln" file in Visual Studio.  
2- Build the solution, which automatically installs the missing NuGet packages.   
3- Run the application in the browser from Visual Studio in F5/CTRL+F5    

# How To Test? 
Copy/paste the following text to test prettifier app: 
[I've sold my house for 10000000 and bought another house for 2000000000 although I wanted the other one but it was too much expensive costs around 1000000000000.] 

# Source Code Overview
The Prettifier porject folder includes the following folders and files 
- App_Data folder - Holds the SQL Server localDB database file. 
- Content - Holds CSS files. 
- Controllers - Holds controller classes. 
- Migrations folder - Holds EF Code First migrations code 
- Models folder - Holds model classes. 
- Properties or MyProject folder - Project properties. 
- Scripts folder - Script files. 
- Views folder - Holds view classes. 
- Visual Studio project file (.csproj or .vbproj). 
- packages.config - Specifies NuGet packages included in the project. 
- Global.asax file - Includes database initializer code. 
- Web.config file - Includes the connection string to the database. 

# Known Issues
- A known issue with NuGet restore is it doesn't copy content of the packages to the applications folders. For large size content (Rotativa),
 I had to create MSBuild post-build event to copy the files to the applications folder.

 xcopy /Q /Y "$(SolutionDir)\packages\Rotativa.1.6.4\content\Rotativa\wkhtmltopdf.exe" "$(ProjectDir)\Rotativa"
