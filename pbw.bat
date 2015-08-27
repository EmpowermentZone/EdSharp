@echo off
cls
C:\PBWin10\bin\pbwin.exe /iC:\PbWin10\WinAPI /l /q %1
rem echo %1 >c:\temp\temp.lst
rem echo %2 >>c:\temp\temp.lst
rem echo %3 >>c:\temp\temp.lst
copy %2 %3
rem type %2
