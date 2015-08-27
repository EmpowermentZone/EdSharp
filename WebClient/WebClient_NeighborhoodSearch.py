def GetGoogleLocalSearch(sKeywords, sAddress):
	oMaps = googlemaps.GoogleMaps()
	oSearch = oMaps.local_search(sKeywords + ' ' + sAddress)
	aPlaces = oSearch['responseData']['results']
	sText = ''
	for dPlace in aPlaces: 
		sText += dPlace['titleNoFormatting'] + '\r\n'
		# sText += dPlace['streetAddress'] + '\r\n'
		# sText += dPlace['city'] + '\r\n'
		if len(dPlace.get('addressLines', [])): sText += '\r\n'.join(dPlace['addressLines']) + '\r\n'
		if len(dPlace.get('phoneNumbers', [])): sText += dPlace['phoneNumbers'][0]['number'] + '\r\n'
		# for dPhone in dPlace['phoneNumbers']: sText += dPhone['number'] + ' ' + dPhone['type'] + '\r\n'
		sText += '\r\n'

	# for dPlace in aPlaces:
		# for sKey in dPlace: print sKey + ': ' + type(dPlace[sKey]).__name__
	# sText = sText.strip()
	return (len(aPlaces), sText)
	
aLabels = ['Address', 'Keywords']
sNeighborhoodAddress = ReadValue('NeighborhoodAddress')
if not sNeighborhoodAddress: sNeighborhoodAddress = ReadValue('StartAddress')
sNeighborhoodKeywords = ReadValue('NeighborhoodKeywords')
aValues = [sNeighborhoodAddress, sNeighborhoodKeywords]
aResults = IniFormDialogMultiInput('Neighborhood Search', aLabels, aValues)
if not aResults: Exit()

sNeighborhoodAddress = aResults[0]
sNeighborhoodKeywords = aResults[1]
WriteValue('NeighborhoodAddress', sNeighborhoodAddress)
WriteValue('NeighborhoodKeywords', sNeighborhoodKeywords)
iResult, sResult = GetGoogleLocalSearch(sNeighborhoodAddress, sNeighborhoodKeywords)
sText = 'Neighborhood Search\r\n' + sNeighborhoodAddress + '\r\n' + str(iResult) + ' Matching ' + sNeighborhoodKeywords + '\r\n\r\n'
sText += sResult
sText = sText.strip() + '\r\n'
SayLine('Loading results')
StringToFile(sText, sOutputFile)
	
