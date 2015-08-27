sWordInfo = ReadValue('WordInfo')
if not sWordInfo: sWordInfo = ReadValue('DictionaryLookup')
sWordInfo = IniFormDialogInput('Input', 'Word', sWordInfo)
if not sWordInfo: Exit()
WriteValue('WordInfo', sWordInfo)
SayLine('Please wait')
# sAddress = 'http://api.wordnik.com/api/word.json/' + sWordInfo + '/definitions'
sAddress = 'http://api.wordnik.com/api-v2/word.json/' + sWordInfo + '/definitions'
dData = {'api_key' : '582e51dde9940b44120020cf4c80f871af2430e98b76dd698'}
sResponse = ''
try: sResponse = WebRequestGetToString(sAddress, dData)
except: SayLine('No information available!')
d = JsToPyObject(sResponse)
lDefinitions = d['definition']
sText = Pluralize('Definition', len(lDefinitions)) + ' of ' + sWordInfo + ' from wordnik.com\r\n\r\n'
# for dDefinition in lDefinitions: sText += dDefinition['headword'] + (', ' + dDefinition['partOfSpeech'] if dDefinition.has_key('partOfSpeech') else '') + '\r\n' + dDefinition['defTxtSummary'] + ('\r\n' + dDefinition.get('defTxtExtended', '')).rstrip() + '\r\n\r\n'
# for dDefinition in lDefinitions: sText += (dDefinition['partOfSpeech'] + '\r\n' if dDefinition.has_key('partOfSpeech') else '').lstrip() + dDefinition['defTxtSummary'] + ('\r\n' + dDefinition.get('defTxtExtended', '')).rstrip() + '\r\n\r\n'
for dDefinition in lDefinitions:
	if dDefinition.has_key('partOfSpeech'): sText += dDefinition['partOfSpeech'] + '\r\n'
	if dDefinition.has_key('defTxtSummary'): sText += dDefinition['defTxtSummary'] + '\r\n'
	if dDefinition.has_key('defTxtExtended'): sText += dDefinition['defTxtExtended'] + '\r\n'
	sText += '\r\n'

sDefinitions = sText.strip()

# sAddress = 'http://api.wordnik.com/api/word.json/' + sWordInfo + '/examples'
sAddress = 'http://api.wordnik.com/api-v2/word.json/' + sWordInfo + '/examples'
dData = {'api_key' : '582e51dde9940b44120020cf4c80f871af2430e98b76dd698'}
sResponse = WebRequestGetToString(sAddress, dData)
d = JsToPyObject(sResponse)
lExamples = d['example']
sText = Pluralize('Example', len(lExamples)) + ' of ' + sWordInfo + ' from wordnik.com\r\n\r\n'
# for dExample in lExamples: print dExample.keys()
for dExample in lExamples:
	if dExample.has_key('year'): sText += dExample['year'] + '\r\n'
	if dExample.has_key('title'): sText += dExample['title'] + '\r\n'
	if dExample.has_key('url'): sText += dExample['url'] + '\r\n'
	if dExample.has_key('display'): sText += dExample['display'] + '\r\n'
	sText += '\r\n'

sExamples = sText.strip()


sApiKey = '20f06c210d4894da53dcb065925e0158'
# sAddress = 'http://words.bighugelabs.com/api/2/' + sApiKey + '/' + sWordInfo + ' + '/json'
sAddress = 'http://words.bighugelabs.com/api/2/' + sApiKey + '/' + sWordInfo + '/'
sResponse = WebRequestGetToString(sAddress)
sResponse = sResponse.replace('|syn|', '|synonym|')
sResponse = sResponse.replace('|ant|', '|antonym|')
sResponse = sResponse.replace('|rel|', '|related term|')
sResponse = sResponse.replace('|sim|', '|similar term|')
sResponse = sResponse.replace('|usr|', '|user suggestion|')
sResponse = sResponse.strip()
lResults = sResponse.split('\n')
sText = Pluralize('Result', len(lResults)) + ' from BigHugeLabs.com\r\n\r\n'
for sResult in lResults:
	lParts = sResult.split('|')
	sText += lParts[0] + ', ' + lParts[1] + '\r\n' + lParts[2] + '\r\n\r\n'

sText = (sDefinitions + '\r\n\r\n\r\n' + sExamples + '\r\n\r\n\r\n' + sText).strip()
SayLine('Loading results and opening web page')
sUrl = 'http://www.etymonline.com/index.php?search=' + sWordInfo.replace(' ', '+') + '&searchmode=none'
os.startfile(sUrl)
StringToFile(sText, sOutputFile)


