sAddress = 'http://letsbetrends.com/api/current_trends'
sResponse = WebRequestGetToString(sAddress)
dResponse = JsToPyObject(sResponse)
lTrends = dResponse.get('trends', None)
if not lTrends: Exit('No information available!')
sText = ''
for dTrend in lTrends: 
	s = StringUnicode(dTrend['name']).strip() + '\r\n'
	if dTrend.has_key('description'): s += StringUnicode(dTrend['description']['text']).strip()
	s = s.replace('\r\n\r\n', '\r\n')
	sText += s + '\r\n\r\n'
sText = sText.strip()
sText = sText.replace('\nUrl: ', '\r\n')
# sText = sText.replace('\nUrl: ', '')
sTitle = Pluralize('Trend Topic', len(lTrends)) + ' from Letsbetrends.com'
sText = sTitle + '\r\n\r\n' + sText
SayLine('Loading results')
StringToFile(sText, sOutputFile)

