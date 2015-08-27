lNames = 'FirstName LastName HouseNumber Street City State Zip AreaCode'.split()
lValues = []
for sName in lNames: sValue = ReadValue('Virtual' + sName); lValues.append(sValue)
lResults = IniFormDialogMultiInput('Multi Input', lNames, lValues)
if not lResults: Exit()

sAddress = 'http://api.whitepages.com/find_person/1.0/'
sApiKey = '809fe67b9341f44573d4dd441ef4e95f'
dData = {'api_key' : sApiKey, 'outputtype' : 'JSON', 'metro' : '1'}
for i, sName in enumerate(lNames): 
	sResult = lResults[i].strip()
	WriteValue('Virtual' + sName, sResult)
	if sResult: dData[sName.lower()] = sResult

SayLine('Please wait')
sResponse = WebRequestGetToString(sAddress, dData)
# print sResponse
dResponse = JsToPyObject(sResponse)
# for sKey in dResponse.keys(): print sKey
lListings = dResponse['listings']
sText = Pluralize('Result', len(lListings)) + ' from WhitePages.com\r\n\r\n'
for dListing in lListings:
	# for sKey in dListing.keys(): print sKey
	lPeople = dListing.get('people', None)
	lPhoneNumbers = dListing.get('phonenumbers', None)
	dAddress = dListing.get('address', None)
	if lPeople: 
		for dPerson in lPeople: sText += dPerson['firstname'] + ' ' + dPerson['lastname'] + '\r\n' 
	if dAddress: sText += dAddress['fullstreet'] + '\r\n' + dAddress['city'] + ', ' + dAddress['state'] + '  ' + dAddress['zip'] + '-' + dAddress['zip4'] + '\r\n' + dAddress['country'] + '\r\n'
	if lPhoneNumbers: 
		for dPhone in lPhoneNumbers: sText += dPhone['fullphone'] + '\r\n'
	sText += '\r\n'

sText = sText.strip()
SayLine('Loading results')
StringToFile(sText, sOutputFile)

