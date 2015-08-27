sToken = 'svzsnwdpnvyzy3y6v9j6tb2k'
sAddress = 'http://www.jigsaw.com/rest/searchCompany.json'
lNames = 'Name AreaCode ZipCode Country WebSiteType FortuneRank'.split()
lValues = []
for sName in lNames: sValue = ReadValue('Address' + sName); lValues.append(sValue)
lResults = IniFormDialogMultiInput('Multi Input', lNames, lValues)
if not lResults: Exit()
dData = {'token' : sToken}
for i, sName in enumerate(lNames): 
	sResult = lResults[i].strip()
	WriteValue('Address' + sName, sResult)
	if sResult: dData[sName[0:1].lower() + sName[1:]] = sResult

SayLine('Please wait')
sResponse = WebRequestGetToString(sAddress, dData)
dResponse = JsToPyObject(sResponse)
# print dResponse
lCompanies = dResponse['companies']
if not lCompanies: Exit('No information available!')
if len(lCompanies) == 1: sText = str(len(lCompanies)) + ' Organization Found via jigsaw.com\r\n\r\n'
else: sText = str(len(lCompanies)) + ' Organizations Found via jigsaw.com\r\n\r\n'
lKeys = 'name address city state zip country'.split()
for dCompany in lCompanies: sText += GetDictionaryText(dCompany, lKeys) + '\r\n\r\n'
sText = sText.strip()
SayLine('Loading results')
StringToFile(sText, sOutputFile)



