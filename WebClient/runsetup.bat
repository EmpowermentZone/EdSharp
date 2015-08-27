@echo off
cls
echo Compiling
if exist build rd /s /q build
if exist dist rd /s /q dist
copy InPy.py InPyC.py >nul
c:\python25\python.exe setup.py py2exe>log.txt
rem if exist dist\MSVCR71.dll del dist\MSVCR71.dll
rem if exist dist\w9xpopen.exe del dist\w9xpopen.exe
copy SayLine.exe dist >nul
copy SayFile.exe dist >nul
copy saapi32.dll dist >nul
copy msvcp71.dll dist >nul
copy dist\InPy.exe > null
echo Done!
