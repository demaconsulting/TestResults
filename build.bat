@echo off
REM Build script for TestResults project

echo Building TestResults...
dotnet build DemaConsulting.TestResults.sln --configuration Release
if %errorlevel% neq 0 exit /b %errorlevel%

echo.
echo Running tests...
dotnet test DemaConsulting.TestResults.sln --configuration Release --no-build
if %errorlevel% neq 0 exit /b %errorlevel%

echo.
echo Build completed successfully!
