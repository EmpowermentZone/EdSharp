sDomainName = ReadValue('DomainName')
sDomainName = IniFormDialogInput('Input', 'Domain Name', sDomainName)
if not sDomainName: Exit()
WriteValue('DomainName', sDomainName)
SayLine('Please wait')
sExe = PathCombine(sAppDir, 'whoiscl.exe')
sCommand = 'cmd.exe /c ' + StringQuote(sExe) + ' -r ' + sDomainName + ' >' + sOutputFile
ShellRun(sCommand, 0, True)
SayLine('Loading results')


