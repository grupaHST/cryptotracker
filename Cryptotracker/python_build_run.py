#!/usr/bin/env python3
# coding: utf8

import os

projName = "Cryptotracker"

commands = [
    "cd " + projName,
    "dotnet build",
    "dotnet run",
    "dotnet clean"
]

os.system(" && ".join(commands))