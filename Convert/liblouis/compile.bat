@echo off
cls
if exist reformat.exe del reformat.exe
c:\windows\Microsoft.NET\Framework\v2.0.50727\csc.exe /nologo /t:exe reformat.cs
if exist reformat.exe echo Done!
