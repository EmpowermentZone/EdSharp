@echo off
cls
rem parameters are ProgDir, SourceDir, SourceRoot, TargetDir, TargetRoot
rem set CommandLine=%1\Convert\NFBTrans\nfbtrans.exe so=0 ow=1 qm=1 tm=13 on=2 od=%2% %3%
rem echo %CommandLine% >c:\temp\temp.txt
rem Pause %1
rem Pause%2
rem Pause %3
rem Pause %4
%1\Convert\NFBTrans\nfbtrans.exe so=0 ow=1 qm=1 tm=13 on=2 od=%4 %2\%3.brf
move /y %4\%3.txt %4\%5.txt
