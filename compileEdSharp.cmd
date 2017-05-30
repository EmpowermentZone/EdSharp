@echo off
cls
set netDir=c:\windows\Microsoft.NET\Framework\v4.0.30319
if exist EdSharp.exe del EdSharp.exe
if exist errors.txt del errors.txt
echo Compiling
%netDir%\csc.exe /platform:x86 /out:EdSharp.exe /nologo /t:winexe /r:JsSupport.dll /r:Tektosyne.dll /r:VbSupport.dll /r:Microsoft.JScript.dll /r:Microsoft.VisualBasic.Compatibility.dll /r:Microsoft.VisualBasic.dll EdSharp.cs >errors.txt
if errorlevel 1 goto end
if exist EdSharp.Exe echo Done
:end
