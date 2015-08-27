@echo off
echo This batch file uses nfbtrans.fmt to create nfbtrans.txt formatted
echo with a line length of 80 columns and 56 lines per page.
pause
if exist nfbtrans.fmt goto got_file
echo nfbtrans.fmt not found...
goto exit
:got_file

rem Create nfbprint.fmt identical to nfbtrans.fmt except for the line of
rem formatting commands at the beginning.

echo ~0 ~-pw=80 ~-ls=99 ~-pl=56 ~-if=12 ~-fc=. ~o5 ~p- ~-ip=0 ~-oc=1 >temp.$$$
copy temp.$$$ + nfbtrans.fmt nfbprint.fmt >nul
del temp.$$$ >nul
nfbtrans ob=oc=15 nfbprint.fmt >nfbtrans.txt
del nfbprint.fmt >nul
:exit
