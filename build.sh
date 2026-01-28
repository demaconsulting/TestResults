#!/bin/bash
# Build script for TestResults project

set -e

echo "Building TestResults..."
dotnet build DemaConsulting.TestResults.sln --configuration Release

echo ""
echo "Running tests..."
dotnet test DemaConsulting.TestResults.sln --configuration Release --no-build

echo ""
echo "Build completed successfully!"
