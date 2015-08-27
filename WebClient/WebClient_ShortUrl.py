sOriginalUrl = GetUrl()
if not sOriginalUrl: sOriginalUrl = ReadValue('OriginalUrl')
sText = IniFormDialogInput('Input', 'Original URL', sOriginalUrl)
if not sText: Exit()
if not sText.lower().find('://') >= 0: sText = 'http://' + sText
WriteValue('OriginalUrl', sText)
# sAddress = 'http://api.bit.ly/shorten'
sAddress = 'http://api.j.mp/shorten'
dData = {'version' : '2.0.1', 'longUrl': sText, 'login': 'mctwit', 'apiKey' : 'R_bd192f2656f1d5857cdbf2fc97156941'}
sResponse = WebRequestGetToString(sAddress, dData)
dResponse = JsToPyObject(sResponse)
# print 'url=' + dResponse['results'][sAddress][shortUrl]
sUrl = dResponse['results'][sText]['shortUrl']
# sUrl = tinyurl.create_one(sText)
SayLine(sUrl)
SayLine('Copied to clipboard')
SetClipboardText(sUrl)


