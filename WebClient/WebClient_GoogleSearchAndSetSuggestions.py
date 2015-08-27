sProposedSearch = ReadValue('ProposedSearch')
sProposedSearch = IniFormDialogInput('Input', 'Proposed Search', sProposedSearch)
if not sProposedSearch: Exit()

WriteValue('ProposedSearch', sProposedSearch)
sAddress = 'http://labs.google.com/sets'
dData = {'hl' : 'en', 'btn' : 'Large Set'}
lTerms = sProposedSearch.split(',')
lTerms = [sTerm.strip() for sTerm in lTerms]
for i in range(len(lTerms)):
	sKey = 'q' + str(i)
	dData[sKey] = lTerms[i]
sResponse = PyWebRequestGetToString(sAddress, dData)
sText = HtmlToText(sResponse)
sMatch = r'Predicted Items(.|\n)+labs\.google\.com'
sPrefix = 'Predicted Items'
iPrefix = len(sPrefix)
sSuffix = 'labs.google.com'
iSuffix = len(sSuffix)
iIndex = sText.find(sPrefix)
sText = sText[iIndex + iPrefix:]
iIndex = sText.find(sSuffix)
sText = sText[0:iIndex]
sText = sText.strip()
lItems = sText.split('\n')
lItems = [sItem.strip() for sItem in lItems]
lItems.sort()
sText = '\r\n'.join(lItems)
# lItems = RegExpExtract(sText, sMatch)
# sText = lItems[0]
sSetText = Pluralize('Google Set Suggestion', len(lItems)) + ' for ' + sProposedSearch + '\r\n\r\n' + sText
# memo.SetValue(sText)
# memo.SetFocus()
# memo.SetIndex(0)

sAddress = 'http://google.com/complete/search'
dData = {'output' : 'toolbar', 'q' : sProposedSearch}
sXml = PyWebRequestGetToString(sAddress, dData)
sMatch = 'suggestion data\=\".*?\"' 
lSuggestions = RegExpExtract(sXml, sMatch)
sText = Pluralize('Google Search Suggestion', len(lSuggestions)) + ' for ' + sProposedSearch + '\r\n\r\n'
for sSuggestion in lSuggestions: sText += sSuggestion[17:-1] + '\r\n'
sText = sText.strip()
sText += '\r\n\r\n' + sSetText
SayLine('Loading results')
StringToFile(sText, sOutputFile)


