#! /bin/bash

dotnet clean
dotnet restore
dotnet jb cleanupcode Kern.sln --verbosity=INFO --exclude=lib/**/*