@echo off
cls
set netDir=c:\windows\Microsoft.NET\Framework\v4.0.30319
if exist JsSupport.dll del JsSupport.dll
if exist errors.txt del errors.txt
echo Compiling
%netDir%\jsc.exe /nologo /fast- /t:library JsSupport.js >errors.txt
if errorlevel 1 goto End
if exist JsSupport.dll echo Done
:End

