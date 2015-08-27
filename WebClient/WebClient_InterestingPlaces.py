dNames = {'Location' : 'address', 'Kilometers' : 'rad', 'Query' : 'q'}
# lNames = 'Address Rad Q'.split()
lNames = 'Location Kilometers Query'.split()
lValues = []
for sName in lNames: sValue = ReadValue('Interesting' + sName); lValues.append(sValue)
lResults = IniFormDialogMultiInput('Multi Input', lNames, lValues)
if not lResults: Exit()

sAddress = 'http://api.nextstop.com/search/'
sApiKey = '1LfiUaiDqYdpUFpor'
dData = {'key' : sApiKey, 'result_type' : 'places'}
for i, sName in enumerate(lNames): 
	sResult = lResults[i].strip()
	WriteValue('Interesting' + sName, sResult)
	# if sResult: dData[sName.lower()] = sResult
	if sResult: dData[dNames[sName]] = sResult

SayLine('Please wait')
sResponse = WebRequestGetToString(sAddress, dData)
WriteBinaryFile('temp.txt', sResponse)

dResponse = JsToPyObject(sResponse)
lPlaces = dResponse['data']['resultsList']
if not lPlaces: Exit('No information available!')
sText = Pluralize('Place', len(lPlaces)) + ' from NextStop.com\r\n\r\n'
for dPlace in lPlaces:
	sText += AddLine(dPlace.get('category', ''))
	sText += AddLine(dPlace.get('name', ''))
	sText += AddLine(dPlace.get('address', ''))
	sText += AddLine(dPlace.get('locationName', ''))
	sText += AddLine(dPlace.get('phone', ''))
	sText += AddLine(dPlace.get('website', ''))
	try: s = dPlace['recommendationsInfo']['bestRecommendation']['shortText']
	except: s = ''
	if s: sText += '"' + s + '"' + '\r\n'
	sText += '\r\n'

# for sKey in dResponse.keys(): print sKey
sText = sText.strip()
SayLine('Loading results')
StringToFile(sText, sOutputFile)

