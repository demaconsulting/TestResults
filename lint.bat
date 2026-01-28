@echo off
REM Lint script for TestResults project

echo Checking code formatting...
dotnet format --verify-no-changes DemaConsulting.TestResults.sln
if %errorlevel% neq 0 (
    echo.
    echo Code formatting issues found. Run 'dotnet format' to fix.
    exit /b 1
)

echo.
echo Checking spelling...
cspell "**/*.{md,cs}" --no-progress
if %errorlevel% neq 0 exit /b %errorlevel%

echo.
echo Checking markdown...
markdownlint "**/*.md" --ignore node_modules
if %errorlevel% neq 0 exit /b %errorlevel%

echo.
echo All lint checks passed!
