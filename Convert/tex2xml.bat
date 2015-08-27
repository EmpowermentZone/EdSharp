rem parameters are SourceDir, SourceRoot, Target
@echo off
cls
set path =C:\Program Files\MiKTeX 2.8\scripts\tex4ht\bat;%path%
set LATEX=-quiet -interaction=batchmode -aux-directory=%TEMP%
cd %1
call mzlatex.exe %2.tex "html,mathplayer"
if exist %1\%2.xml move /y %1\%2.xml %3
