@echo off
cls
set path=C:\MinGW\bin;%path%
if exist %1.o del %1.o
g++.exe -c %1.cpp
if not exist %1.o goto end
g++.exe -o %1 %1.o
:end
