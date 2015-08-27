@echo off
cls
if exist Eval.dll del Eval.dll
rem jsc.exe /nologo /r:System.Drawing.dll /r:Accessibility.dll /r:Microsoft.VisualBasic.dll /r:Microsoft.VisualBasic.Compatibility.dll /t:library Eval.js
jsc.exe /nologo /fast- /t:library eval.js
