#!/usr/bin/env python3
# coding: utf8

import os

projName = "CryptotrackerTests"

commands = [
    "cd " + projName,
    "dotnet test",
]

os.system(" && ".join(commands))

print("Wykonywanie testów zakończone")
input("Naciśnij dowolny klawisz aby kontynuować ...")

os.system("dotnet clean")