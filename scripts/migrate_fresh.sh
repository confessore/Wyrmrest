#!/bin/sh
cd ../src/Wyrmrest.Web
dotnet ef database drop
dotnet ef migrations remove
dotnet ef migrations add init
dotnet ef database update
