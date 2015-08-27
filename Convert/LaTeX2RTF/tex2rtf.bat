@echo off
cls
if exist %2.rtf del %2.rtf
%1\latex2rt.exe -P %1\cfg %2.tex
if exist %2.rtf move /y %2.rtf %3
