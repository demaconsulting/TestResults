# Installation

## Prerequisites

### Required Tools

- **.NET SDK**: Version 8.0, 9.0, or 10.0
- **Development Environment**: Visual Studio 2022, Visual Studio Code, or JetBrains Rider

### Required Knowledge

- Basic understanding of C# programming
- Familiarity with .NET project structure

### Compatibility

The TestResults library targets .NET Standard 2.0, .NET 8.0, .NET 9.0, and .NET 10.0. It can be used in any
.NET project type: console applications, class libraries, ASP.NET Core applications, worker services, and more.
The library has zero runtime dependencies, minimizing integration friction.

## Installing via .NET CLI

The recommended way to install the TestResults library is using the .NET CLI:

```bash
dotnet add package DemaConsulting.TestResults
```

This command adds the latest stable version to your project file (`.csproj`). To install a specific version:

```bash
dotnet add package DemaConsulting.TestResults --version 1.0.0
```

## Installing via Visual Studio Package Manager Console

If you are using Visual Studio, install the package via the Package Manager Console:

```powershell
Install-Package DemaConsulting.TestResults
```

For a specific version:

```powershell
Install-Package DemaConsulting.TestResults -Version 1.0.0
```

## Installing via Visual Studio NuGet Package Manager GUI

1. Right-click your project in Solution Explorer
2. Select **Manage NuGet Packages...**
3. Click the **Browse** tab
4. Search for `DemaConsulting.TestResults`
5. Select the package and click **Install**

## Manual Package Reference

Add the package reference directly to your `.csproj` file:

```xml
<ItemGroup>
  <PackageReference Include="DemaConsulting.TestResults" Version="1.0.0" />
</ItemGroup>
```

After adding the reference manually, restore packages:

```bash
dotnet restore
```
