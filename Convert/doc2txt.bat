@echo off
cls
if exist %3 del %3
%1\Convert\OfficeConvert\WdVert.exe %2 %3
if exist %3 goto end
%1\Convert\GetText\GetText.exe %2 %3
:end
