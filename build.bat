@echo off
REM Build and test TestResults (Windows)

echo Building TestResults...
dotnet build --configuration Release
if %errorlevel% neq 0 exit /b %errorlevel%

echo Running unit tests...
dotnet test --configuration Release
if %errorlevel% neq 0 exit /b %errorlevel%

echo Build and tests completed successfully!
