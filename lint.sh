#!/bin/bash
# Lint script for TestResults project

set -e

echo "Checking code formatting..."
if ! dotnet format --verify-no-changes DemaConsulting.TestResults.sln; then
    echo ""
    echo "Code formatting issues found. Run 'dotnet format' to fix."
    exit 1
fi

echo ""
echo "Checking spelling..."
cspell "**/*.{md,cs}" --no-progress

echo ""
echo "Checking markdown..."
markdownlint "**/*.md" --ignore node_modules

echo ""
echo "All lint checks passed!"
