sLocation = ReadValue('LocationID')
if not sLocation: sLocation = ReadValue('ZipCode')
sLocation = IniFormDialogInput('Input', 'Location', sLocation)
if not sLocation: Exit()

WriteValue('LocationID', sLocation)
# dYahoo = pywapi.get_weather_from_google(sLocation) ; bFound = True
# print dYahoo
# Exit()
try: dYahoo = pywapi.get_weather_from_yahoo(sLocation) ; bFound = True
except: bFound = False
if bFound:
	# for sKey, sValue in dYahoo.items(): print sKey, '=', sValue
	SayLine('Loading results and opeing web page')
	# print "Yahoo says: It is " + string.lower(yahoo_result['condition']['text']) + " and " + yahoo_result['condition']['temp'] + "C now in New York.\n\n"
	sText = RemoveHtmlTags(dYahoo['html_description'])
	sText = RegExpReplace(sText, r'Full Forecast(.|\n)+', '')
	sTitle = dYahoo['condition']['title']
	# sText = 'Yahoo and Weather Channel Information\r\n' + 'Location ID ' + sLocation + '\r\n\r\n' + sText
	sText = 'Yahoo and Weather Channel Information\r\n' + sTitle + '\r\n\r\n' + sText
	sText = sText.strip()
	# SayLine('Loading results')
	StringToFile(sText, sOutputFile)
SayLine('Loading results and opeing web page')
sAddress = 'http://braille.wunderground.com/cgi-bin/findweather/hdfForecast'
dData = {'brand' : 'braille', 'query' : sLocation}
sQuery = utf8urlencode(dData)
sAddress += '?' + sQuery
sText = HtmlGetText(sAddress)
sMatch = r'\nFind the Weather (.|\n)*'
sText = RegExpReplace(sText, sMatch, '')
sText = sText.strip() + '\r\n'
StringToFile(sText, sOutputFile)
os.startfile(sAddress)


