@echo off
cls
if exist %2.tex del %2.tex
%1\rtf2latex2e.exe %2.rtf
if exist %2.tex move /y %2.tex %3.tex
