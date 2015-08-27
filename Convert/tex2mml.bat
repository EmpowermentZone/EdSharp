rem %1 is SourceDir, %2 is SourceRoot, and %3 is Target
@echo off
cls
set LATEX=-quiet -interaction=batchmode -aux-directory=%TEMP%
cd %1
mzlatex.exe %2.tex "html,mathplayer"
if exist %1\%2.xml move /y %1\%2.xml %3
