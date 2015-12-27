@echo off
cls
if exist EdSharp.exe del EdSharp.exe
if exist EdSharp64.exe del EdSharp64.exe
rem csc.exe /nologo /t:winexe /r:Tektosyne.dll /r:IronCOM.dll /r:Microsoft.JScript.dll /r:eval.dll /r:VB.dll /r:Microsoft.VisualBasic.dll
rem Make EdSharp.exe Win32 and EdSharp64.exe Win64
rem c:\windows\Microsoft.NET\Framework\v2.0.50727\csc.exe /nologo /t:winexe /r:Tektosyne.dll /r:IronCOM.dll /r:Microsoft.JScript.dll /r:eval.dll /r:VB.dll /r:Microsoft.VisualBasic.dll /r:Microsoft.VisualBasic.Compatibility.dll EdSharp.cs 
c:\windows\Microsoft.NET\Framework\v2.0.50727\csc.exe /platform:x86 /out:EdSharp.exe /nologo /t:winexe /r:Tektosyne.dll /r:IronCOM.dll /r:Microsoft.JScript.dll /r:eval.dll /r:VB.dll /r:Microsoft.VisualBasic.dll /r:Microsoft.VisualBasic.Compatibility.dll EdSharp.cs  
if errorlevel 1 goto end
c:\windows\Microsoft.NET\Framework\v2.0.50727\csc.exe /platform:x64 /out:EdSharp64.exe /nologo /t:winexe /r:Tektosyne.dll /r:IronCOM.dll /r:Microsoft.JScript.dll /r:eval.dll /r:VB.dll /r:Microsoft.VisualBasic.dll /r:Microsoft.VisualBasic.Compatibility.dll EdSharp.cs  
if exist EdSharp.Exe EdSharp.exe
:end
