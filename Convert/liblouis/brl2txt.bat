@echo off
cls
cd %1
xml2brl.exe -b %2 %3
reformat.exe %3
