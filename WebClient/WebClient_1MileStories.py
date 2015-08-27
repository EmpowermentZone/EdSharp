sLocalAddress = ReadValue('LocalAddress')
if not sLocalAddress: sLocalAddress = ReadValue('NeighborhoodAddress')
if not sLocalAddress: sLocalAddress = ReadValue('StartAddress')
sLocalAddress = IniFormDialogInput('Input', 'Local Address', sLocalAddress)
if not sLocalAddress: Exit()

WriteValue('LocalAddress', sLocalAddress)
iLat, iLng = GoogleAddressToLatLng(sLocalAddress)
sWebAddress = 'http://api.outside.in/radar.json?lat=' + str(iLat) + '&lng=' + str(iLng) + '&radius=1&only=stories'
sResponse = WebRequestGetToString(sWebAddress)
lStories = simplejson.loads(sResponse)
sText = ''
iCount = 0
for dStory in lStories:
	if not dStory.get('type', None) == 'Story': continue
	# for sKey in dStory.keys(): print sKey
	iCount += 1
	sText = AddLineIfKey(sText, dStory, 'title')
	sText = AddLineIfKey(sText, dStory, 'author')
	sText = AddLineIfKey(sText, dStory, 'published_at')
	sText = AddLineIfKey(sText, dStory, 'url')
	sText = AddLineIfKey(sText, dStory, 'body')
	sText += '\r\n'

if iCount == 1: sHeader = '1 Story'
else: sHeader = str(iCount) + ' Stories'
sHeader += ' from Outside.In\r\n'
sHeader += 'Within 1 Mile of\r\n' + sLocalAddress + '\r\n\r\n'
sText = sHeader + sText.strip() + '\r\n'
SayLine('Loading results')
StringToFile(sText, sOutputFile)
	
