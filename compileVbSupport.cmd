@echo off
cls
set netDir=c:\windows\Microsoft.NET\Framework\v4.0.30319
if exist VbSupport.dll del VbSupport.dll
if exist errors.txt del errors.txt
echo Compiling
%netDir%\vbc.exe /nologo /t:library /r:Microsoft.VisualBasic.dll VbSupport.vb >errors.txt
if errorlevel 1 goto End
if exist VbSupport.dll echo Done
:End
