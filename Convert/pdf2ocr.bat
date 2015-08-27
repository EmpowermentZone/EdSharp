rem parameters are ProgDir, SourceRoot, TargetRoot
@echo off
set path=%1;%1\gsdata;%path%
call %1\pdf2ocr.bat %2
if exist %2.txt move /y %2.txt %3.ocr
