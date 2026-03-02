#!/usr/bin/env bash
set -e

echo "Clearing tiles collection..."
mongosh mongodb://localhost/tiles --eval "db.tiles.deleteMany({})"

echo ""
echo "Running console app for zoom levels 0–5..."
time dotnet run -- 5
