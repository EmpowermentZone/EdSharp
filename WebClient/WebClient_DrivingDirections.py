def FixUnits(sText):
	sText = HtmlGetText(sText).strip()
	sText = sText.replace(' mins', ' xinutes')
	sText = sText.replace(' min', ' xinute')
	sText = sText.replace(' secs', ' seconds')
	sText = sText.replace(' mi', ' miles')
	sText = sText.replace(' xin', ' min')
	return sText


def GetGoogleDrivingDirections(sStartAddress, sEndAddress):
	oMaps = googlemaps.GoogleMaps()
	oDirections = oMaps.directions(sStartAddress, sEndAddress)
	sDistance = oDirections['Directions']['Distance']['meters']
	sDistance = oDirections['Directions']['Distance']['html']
	sDistance = FixUnits(sDistance).strip()
	# for sKey, sValue in oDirections['Directions']['Distance'].items(): print sKey + '=' + PyString(sValue)
	sDuration = oDirections['Directions']['Duration']['seconds']
	sDuration = oDirections['Directions']['Duration']['html']
	sDuration = FixUnits(sDuration).strip()
	# for sKey, sValue in oDirections['Directions']['Duration'].items(): print sKey + '=' + PyString(sValue)
	lSteps = []
	# for oStep in oDirections['Directions']['Routes'][0]['Steps']: lSteps.append(oStep['descriptionHtml'])
	# for oStep in oDirections['Directions']['Routes'][0]['Steps']: lSteps.append(oStep['descriptionHtml'] + '\r\n' + oStep['Distance']['html'] + '\r\n' + oStep['Duration']['html'])
	for oStep in oDirections['Directions']['Routes'][0]['Steps']: lSteps.append(FixUnits(oStep['descriptionHtml']).strip() + '\r\n' + FixUnits(oStep['Distance']['html']).strip() + '\r\n' + FixUnits(oStep['Duration']['html']).strip())
	# for oStep in oDirections['Directions']['Routes'][0]['Steps']: 
		# for sKey, sValue in oStep.items(): print sKey + '=' + str(sValue)
	return (sDistance, sDuration, lSteps)
	
	

def FormatDecimal(n):
	sNumber = str(n)
	sMatch = r'(\.\d\d)\d+'
	sReplace = r'$1'
	sReturn = RegExpReplace(sNumber, sMatch, sReplace)
	return sReturn



aLabels = ['Start Address', 'End Address']
sStartAddress = ReadValue('StartAddress')
if not sStartAddress: sStartAddress = ReadValue('NeighborhoodAddress')
sEndAddress = ReadValue('EndAddress')
aValues = [sStartAddress, sEndAddress]
aResults = IniFormDialogMultiInput('Directions', aLabels, aValues)
if not aResults: Exit()

sStartAddress = aResults[0]
sEndAddress = aResults[1]
WriteValue('StartAddress', sStartAddress)
WriteValue('EndAddress', sEndAddress)
aResults = GetGoogleDrivingDirections(sStartAddress, sEndAddress)
# iKilometers = aResults[0] / 1000.0
# iMiles = iKilometers / 1.609344
# sDistance = FormatDecimal(iKilometers) + ' kilometers or ' + FormatDecimal(iMiles) + ' miles'
# sDistance = FormatDecimal(aResults[0] / 1000.0)
#sDuration = FormatDecimal(aResults[1] / 3600.0)
sDistance = aResults[0]
sDuration = aResults[1]
aSteps = aResults[2]
sText = 'Driving Directions\r\nFrom ' + sStartAddress + '\r\nTo ' + sEndAddress + '\r\n'
sText += sDistance + '\r\n' + sDuration + '\r\n\r\n'
for sStep in aSteps: sText += sStep + '\r\n\r\n'
"""
sMatch = r'\<div.*?\>'
sReplace = '\r\n\r\n'
sText = RegExpReplace(sText, sMatch, sReplace)
sMatch = r'\<.*?\>'
sReplace = ''
sText = RegExpReplace(sText, sMatch, sReplace)
"""

sText = sText.strip() + '\r\n'
SayLine('Loading results')
StringToFile(sText, sOutputFile)

