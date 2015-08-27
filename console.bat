@echo off
cls
if exist EdSharp.exe del EdSharp.exe
csc.exe /nologo /t:exe /r:Microsoft.JScript.dll /r:eval.dll /r:VB.dll /r:Microsoft.VisualBasic.dll /r:Microsoft.VisualBasic.Compatibility.dll EdSharp.cs 
if exist EdSharp.Exe EdSharp.exe
