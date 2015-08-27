def IniFormDialogPick(sTitle, lValues, bSort=False, iIndex=1):
# Get choices from a single selection list box using IniForm.exe
	
	sTempDir = PathCreateTempFolder()
	sInputIni = PathCombine(sTempDir, 'Input.ini')
	sOutputIni = PathCombine(sTempDir, 'Output.ini')
	sInputTxt = PathCombine(sTempDir, 'Input.txt')
	sOutputTxt = PathCombine(sTempDir, 'Output.txt')
	sCommand = sIniFormExe + " " + sTempDir + "\\"
	
	sValues = '\r\n'.join(lValues)
	sValues = "[[List]]\r\n" + sValues
	StringToFile(sValues, sInputTxt)

	sBody = "[" + sTitle + "]\r\nListWidth=600\r\nListHeight=600\r\nMisc=NoStatus\r\n"
	sBody = sBody + "[List]\r\nControl=List\r\n"
	sBody = sBody + 'Selection=' + str(iIndex) + '\r\n'
	if bSort: sBody = sBody + "Misc=NoLabel|Sort\r\n"
	else: sBody = sBody + "Misc=NoLabel\r\n"

	sBody = sBody + "[OK]\r\nControl=Button\r\n"
	sBody = sBody + "[Cancel]\r\nControl=Button\r\n"
	StringToFile(sBody, sInputIni)
	ShellRun(sCommand, 1, True)
	FileDelete(sInputIni)

	sReturn = ''
	if FileExists(sOutputIni):
		sReturn = win32api.GetProfileVal('Results', 'List', '', sOutputIni)
		FileDelete(sOutputIni)
	
		FileDelete(sOutputIni)

	FolderDelete(sTempDir)
	return sReturn
	
	

	
	


sSports = """
AFL
Boxing
College-Football
Golf
Horse-Racing
Lacrosse
Mens-College-Basketball
MLB
MMA
NASCAR
NBA
NCAA
NFL
NHL
Olympics
Outdoors
Poker
Racing
Soccer
Sports
Tennis
Womens-Basketball
"""

sAddress = 'http://espn.go.com/'
lSports = sSports.strip().split('\n')
iIndex = 1
sSport = ReadValue('Sport')
if sSport in lSports: iIndex = lSports.index(sSport) + 1
sSport = IniFormDialogPick('Pick', lSports, False, iIndex)
if not sSport: Exit()

WriteValue('Sport', sSport)
sAddress += sSport.lower() + '/'
SayLine('Opening web page')
os.startfile(sAddress)
lSports
