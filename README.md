# Example Product Codes

## Overview

A simple **Product Code Management System** built for Example.com. The single MVC page uses a green title area, search controls, and a responsive table. No authentication or dashboard is included.

## Technology

- .NET 8 / C#
- ASP.NET Core MVC
- Entity Framework Core 8
- SQL Server
- Bootstrap 5 (local assets)
- JavaScript and QRCode.js (local assets, MIT license)
- xUnit

## Features

- Generate exactly 30 alphanumeric characters using RandomNumberGenerator.GetInt32.
- Format product codes as six groups of five characters separated by five hyphens.
- Save codes and UTC creation timestamps to SQL Server.
- Search by literal substring using GET /ProductCode?search=ABCDE; SQL Server uses an explicit case-insensitive collation.
- Display the exact formatted row code in a centered QR modal.
- Confirm deletion in a modal; Cancel only closes it. Confirm uses POST with anti-forgery protection.
- Reject invalid IDs with HTTP 400 and missing records with HTTP 404.
- Show friendly empty states and generic production error pages.

## Prerequisites

- .NET 8 SDK (which includes the runtime)
- SQL Server or SQL Server LocalDB on Windows
- EF Core CLI, restored from the committed local tool manifest

### Windows: missing .NET 8 runtime

If `dotnet test` reports that Microsoft.NETCore.App 8.0.0 is missing, having a .NET 10 SDK alone is insufficient. From PowerShell in this project directory, run:

~~~powershell
.\Use-DotNet8.ps1
dotnet restore
dotnet build
dotnet test
~~~

The setup script selects a .NET 8 SDK in `work/dotnet` for the current terminal. If absent, it downloads it using Microsoft's official installer. It requires no administrator access and does not change system settings. Run it again in each new terminal, or install the .NET 8 SDK system-wide. The same terminal can then run the EF and application commands below.

## Configuration

The safe development example in Example.Web/appsettings.json uses Windows integrated authentication with the `(localdb)\\Example` named instance and the `Example` database.

For another SQL Server instance, override ConnectionStrings:DefaultConnection using an environment variable named ConnectionStrings__DefaultConnection or development user secrets. Use a trusted server certificate in production. Do not commit real connection credentials.

PowerShell example with integrated authentication:

~~~powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost;Database=Example;Integrated Security=True;TrustServerCertificate=True"
~~~

## Database Setup

Run from the solution directory:

~~~sh
dotnet restore
dotnet tool restore
dotnet ef database update --project Example.Web
~~~

The committed InitialCreate migration creates ProductCodeItems with Id, Code (varchar(35)), and CreatedAt (datetime2). The application does not apply migrations automatically.

## Run

~~~sh
dotnet run --project Example.Web --launch-profile http
~~~

Open http://localhost:5165/ProductCode. The root URL opens the same page.

For local HTTPS, use dotnet dev-certs https --trust and the https launch profile.

## Test

~~~sh
dotnet test
~~~

Tests cover the 30-character payload, allowed alphabet, full format, separators, group sizes, and differences across 1,000 generated values.

~~~sh
dotnet build
~~~

## Project Structure

- Example.Web/Controllers/ProductCodeController.cs: async list/search, Add, and Delete.
- Example.Web/Models/ProductCodeItem.cs: persisted product code entity.
- Example.Web/Data/AppDbContext.cs: SQL Server model configuration.
- Example.Web/Data/Migrations/: initial EF Core migration and snapshot.
- Example.Web/Services/: injectable product code generator.
- Example.Web/Views/ProductCode/Index.cshtml: table and Bootstrap modals.
- Example.Web/wwwroot/: local Bootstrap, QR library, CSS, and JavaScript.
- Example.ProductCodes.Tests/ProductCodeGeneratorTests.cs: xUnit tests.
- .config/dotnet-tools.json: pinned EF CLI.
- Dockerfile: optional multi-stage application image.

## Optional Docker

~~~sh
docker build -t example-product-codes .
docker run --rm -p 8080:8080 --env ConnectionStrings__DefaultConnection example-product-codes
~~~


