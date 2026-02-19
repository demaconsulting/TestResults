#!/usr/bin/env bash
# Build and test TestResults

set -e  # Exit on error

echo "🔧 Building TestResults..."
dotnet build --configuration Release

echo "🧪 Running unit tests..."
dotnet test --configuration Release

echo "✨ Build and tests completed successfully!"
