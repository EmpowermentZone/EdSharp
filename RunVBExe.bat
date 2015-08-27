@echo off
cls
if exist VB.dll del VB.dll
vbc.exe /nologo /t:exe /r:Microsoft.VisualBasic.dll VB.vb
