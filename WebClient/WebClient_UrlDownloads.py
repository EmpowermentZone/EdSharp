# sText = GetClipboardText()
sText = ReadValue('UrlDownloads').replace('|', '\r\n').strip()
sText = IniFormDialogMemo('Multi Line Input', sText)
if not sText: Exit()

WriteValue('UrlDownloads', sText)
sText = sText.replace('|', '\r\n')
sMatch = '\\w+\\:\\/\\/\\S*'
lLines = RegExpExtract(sText, sMatch, False)
# for sLine in lLines: print sLine
iCount = lLines.Count
if not iCount: Exit('No urls found on clipboard!')

SayLine(StringPlural('url', iCount))

for i in range(iCount):
	sLine = lLines(i)
	sLine += ' = ' + WebUrlDownloadFileName(sLine)
	lLines[i] = sLine

sLines = VtListToString(lLines, '\n')
sLines = StringConvertToUnixLineBreak(sLines)
sLines = RegExpReplace(sLines, '\n+', '\n')
sLines = StringTrim(sLines)
# SetClipboardText(sLines)
lLines = StringToList(sLines, '\n')
sMatch = '\\w+\\:\\/\\/\\S+'
lUrls = RegExpExtract(sLines, sMatch, False)
sUrls = VtListToString(lUrls, '\n')

lExtensions = VtListGetExtensions(lLines)
sExtensions = VtListToString(lExtensions, ' ')
sExtensions = IniFormDialogInput('Input', 'Extensions', sExtensions)
lExtensions = StringToList(sExtensions, ' ')
lLines = VtListFilterByExtension(lLines, lExtensions)
sLines = VtListToString(lLines, '\r\n')
# SetClipboardText("lines\r\n" + sLines)
iCount = lLines.Count
if  not iCount: Exit('No matches!')
elif iCount == 1: SayLine('1 match')
else: SayLine(PyString(iCount) + ' matches')

sItems = VtListToString(lLines, '\007')
sTitle = 'Pick Files'
lResults = IniFormDialogMultiPick(sTitle, lLines, False)
if  not lResults: Exit()

sResults = '\n'.join(lResults)
sMatch = '\\w+\\:\\/\\/\\S+'
# lUrls.Clear()
lUrls = RegExpExtract(sResults, sMatch, False)
sUrls = VtListToString(lUrls, '\r\n')
sDir = ReadValue('Directory', '')
if not FolderExists(sDir): sDir = PathGetSpecialFolder('Personal')
print 'dir', sDir
sDir = IniFormDialogBrowseForFolder(sDir)
if not sDir: Exit()

WriteValue('Directory', sDir)

iCount = lUrls.Count
iDownloads = 0
sText = ''
for i in range(iCount):
	sUrl = lUrls(i)
	sFile = WebUrlDownloadPath(sUrl, sDir, False)
	sName = PathGetName(sFile)
	SayLine(sName)
	if WebUrlToFile(sUrl, sFile): sText += sUrl + ' = ' + sName + '\r\n'; iDownloads += 1
	else: SayLine('Error!')

# DialogShow('Results', 'Downloaded ' + StringPlural('file', iDownloads))
sText = 'Downloaded ' + StringPlural('file', iDownloads) + '\r\n\r\n' + sText
sText = sText.strip() + '\r\n'
StringToFile(sText, sOutputFile)
SayLine('Loading results')
