sTempDir = PathCreateTempFolder()
sInputIni = PathCombine(sTempDir, 'Input.ini')
sInputTxt = PathCombine(sTempDir, 'Input.Txt')
sOutputIni = PathCombine(sTempDir, 'Output.ini')
sOutputTxt = PathCombine(sTempDir, 'Output.Txt')

sInputIniText = """[Language Translation]
control=form
[Source]
control=list
[Target]
control=list
[Text]
control=memo
[OK]
control=button
[Cancel]
control=button
"""

# StringToFile(sInputIniText, sInputIni)
WriteTextFile(sInputIni, sInputIniText)

dDetectLanguages = {}
dDetectLanguages['AFRIKAANS'] = 'af'
dDetectLanguages['ALBANIAN'] = 'sq'
dDetectLanguages['AMHARIC'] = 'am'
dDetectLanguages['ARABIC'] = 'ar'
dDetectLanguages['ARMENIAN'] = 'hy'
dDetectLanguages['AZERBAIJANI'] = 'az'
dDetectLanguages['BASQUE'] = 'eu'
dDetectLanguages['BELARUSIAN'] = 'be'
dDetectLanguages['BENGALI'] = 'bn'
dDetectLanguages['BIHARI'] = 'bh'
dDetectLanguages['BULGARIAN'] = 'bg'
dDetectLanguages['BURMESE'] = 'my'
dDetectLanguages['CATALAN'] = 'ca'
dDetectLanguages['CHEROKEE'] = 'chr'
dDetectLanguages['CHINESE'] = 'zh'
dDetectLanguages['CHINESE_SIMPLIFIED'] = 'zh-CN'
dDetectLanguages['CHINESE_TRADITIONAL'] = 'zh-TW'
dDetectLanguages['CROATIAN'] = 'hr'
dDetectLanguages['CZECH'] = 'cs'
dDetectLanguages['DANISH'] = 'da'
dDetectLanguages['DHIVEHI'] = 'dv'
dDetectLanguages['DUTCH'] = 'nl'
dDetectLanguages['ENGLISH'] = 'en'
dDetectLanguages['ESPERANTO'] = 'eo'
dDetectLanguages['ESTONIAN'] = 'et'
dDetectLanguages['FILIPINO'] = 'tl'
dDetectLanguages['FINNISH'] = 'fi'
dDetectLanguages['FRENCH'] = 'fr'
dDetectLanguages['GALICIAN'] = 'gl'
dDetectLanguages['GEORGIAN'] = 'ka'
dDetectLanguages['GERMAN'] = 'de'
dDetectLanguages['GREEK'] = 'el'
dDetectLanguages['GUARANI'] = 'gn'
dDetectLanguages['GUJARATI'] = 'gu'
dDetectLanguages['HEBREW'] = 'iw'
dDetectLanguages['HINDI'] = 'hi'
dDetectLanguages['HUNGARIAN'] = 'hu'
dDetectLanguages['ICELANDIC'] = 'is'
dDetectLanguages['INDONESIAN'] = 'id'
dDetectLanguages['INUKTITUT'] = 'iu'
dDetectLanguages['ITALIAN'] = 'it'
dDetectLanguages['JAPANESE'] = 'ja'
dDetectLanguages['KANNADA'] = 'kn'
dDetectLanguages['KAZAKH'] = 'kk'
dDetectLanguages['KHMER'] = 'km'
dDetectLanguages['KOREAN'] = 'ko'
dDetectLanguages['KURDISH'] = 'ku'
dDetectLanguages['KYRGYZ'] = 'ky'
dDetectLanguages['LAOTHIAN'] = 'lo'
dDetectLanguages['LATVIAN'] = 'lv'
dDetectLanguages['LITHUANIAN'] = 'lt'
dDetectLanguages['MACEDONIAN'] = 'mk'
dDetectLanguages['MALAY'] = 'ms'
dDetectLanguages['MALAYALAM'] = 'ml'
dDetectLanguages['MALTESE'] = 'mt'
dDetectLanguages['MARATHI'] = 'mr'
dDetectLanguages['MONGOLIAN'] = 'mn'
dDetectLanguages['NEPALI'] = 'ne'
dDetectLanguages['NORWEGIAN'] = 'no'
dDetectLanguages['ORIYA'] = 'or'
dDetectLanguages['PASHTO'] = 'ps'
dDetectLanguages['PERSIAN'] = 'fa'
dDetectLanguages['POLISH'] = 'pl'
dDetectLanguages['PORTUGUESE'] = 'pt'
dDetectLanguages['PUNJABI'] = 'pa'
dDetectLanguages['ROMANIAN'] = 'ro'
dDetectLanguages['RUSSIAN'] = 'ru'
dDetectLanguages['SANSKRIT'] = 'sa'
dDetectLanguages['SERBIAN'] = 'sr'
dDetectLanguages['SINDHI'] = 'sd'
dDetectLanguages['SINHALESE'] = 'si'
dDetectLanguages['SLOVAK'] = 'sk'
dDetectLanguages['SLOVENIAN'] = 'sl'
dDetectLanguages['SPANISH'] = 'es'
dDetectLanguages['SWAHILI'] = 'sw'
dDetectLanguages['SWEDISH'] = 'sv'
dDetectLanguages['TAJIK'] = 'tg'
dDetectLanguages['TAMIL'] = 'ta'
dDetectLanguages['TAGALOG'] = 'tl'
dDetectLanguages['TELUGU'] = 'te'
dDetectLanguages['THAI'] = 'th'
dDetectLanguages['TIBETAN'] = 'bo'
dDetectLanguages['TURKISH'] = 'tr'
dDetectLanguages['UKRAINIAN'] = 'uk'
dDetectLanguages['URDU'] = 'ur'
dDetectLanguages['UZBEK'] = 'uz'
dDetectLanguages['UIGHUR'] = 'ug'
dDetectLanguages['VIETNAMESE'] = 'vi'
dDetectLanguages['UNKNOWN'] = ''

lItems = dDetectLanguages.items()
dDetectLanguages = {}
for sKey, sValue in lItems: sKey = sKey[0 : 1].upper() + sKey[1:].lower(); dDetectLanguages[sKey] = sValue

dTranslateLanguages = {}
dTranslateLanguages['Arabic'] = ''
dTranslateLanguages['Bulgarian'] = ''
dTranslateLanguages['Chinese'] = ''
dTranslateLanguages['Catalan'] = ''
dTranslateLanguages['Croatian'] = ''
dTranslateLanguages['Czech'] = ''
dTranslateLanguages['Danish'] = ''
dTranslateLanguages['Dutch'] = ''
dTranslateLanguages['English'] = ''
dTranslateLanguages['Filipino'] = ''
dTranslateLanguages['Finnish'] = ''
dTranslateLanguages['French'] = ''
dTranslateLanguages['German'] = ''
dTranslateLanguages['Greek'] = ''
dTranslateLanguages['Hebrew'] = ''
dTranslateLanguages['Hindi'] = ''
dTranslateLanguages['Indonesian'] = ''
dTranslateLanguages['Italian'] = ''
dTranslateLanguages['Japanese'] = ''
dTranslateLanguages['Korean'] = ''
dTranslateLanguages['Latvian'] = ''
dTranslateLanguages['Lithuanian'] = ''
dTranslateLanguages['Norwegian'] = ''
dTranslateLanguages['Polish'] = ''
dTranslateLanguages['Portuguese'] = ''
dTranslateLanguages['Romanian'] = ''
dTranslateLanguages['Russian'] = ''
dTranslateLanguages['Spanish'] = ''
dTranslateLanguages['Serbian'] = ''
dTranslateLanguages['Slovak'] = ''
dTranslateLanguages['Slovenian'] = ''
dTranslateLanguages['Swedish'] = ''
dTranslateLanguages['TURKISH'] = 'tr'
dTranslateLanguages['Ukrainian'] = ''
dTranslateLanguages['Vietnamese'] = ''
dTranslateLanguages['Unknown'] = ''

lItems = dTranslateLanguages.items()
dTranslateLanguages = {}
for sKey, sValue in lItems: sKey = sKey[0 : 1].upper() + sKey[1:].lower(); dTranslateLanguages[sKey] = sValue

for sLanguage in dDetectLanguages.keys():
	if dTranslateLanguages.has_key(sLanguage):
		sAbbreviation = dDetectLanguages[sLanguage]
		dTranslateLanguages[sLanguage] = sAbbreviation
	
dReverseLanguages = {}
for sKey, sValue in dDetectLanguages.items(): dReverseLanguages[sValue] = sKey

lSourceNames = [sKey for sKey in dDetectLanguages.keys()]
lSourceNames.sort()
lSourceValues = lSourceNames[:]
sSourceNames = '\r\n'.join(lSourceNames)
sSourceNames = '[[Source]]\r\n' + sSourceNames
sSourceLanguage = ReadValue('SourceLanguage')
iSourceIndex = 1
if sSourceLanguage in lSourceNames: iSourceIndex = lSourceNames.index(sSourceLanguage) + 1

lTargetNames = [sKey for sKey in dTranslateLanguages.keys()]
lTargetNames.sort()
lTargetValues = lTargetNames[:]
sTargetNames = '\r\n'.join(lTargetNames)
sTargetNames = '[[Target]]\r\n' + sTargetNames
sTargetLanguage = ReadValue('TargetLanguage')
if not sTargetLanguage: sTargetLanguage = 'ENGLISH'
iTargetIndex = 1
if sTargetLanguage in lTargetNames: iTargetIndex = lTargetNames.index(sTargetLanguage) + 1
# print iSourceIndex, iTargetIndex
win32api.WriteProfileVal('Source', 'selection', iSourceIndex, sInputIni)
win32api.WriteProfileVal('Target', 'selection', iTargetIndex, sInputIni)
# SetClipboard(FileToString(sInputIni))

sSourceText = ReadValue('SourceText')
sSourceText = sSourceText.replace('|', '\r\n').strip()
sSourceText = '[[Text]]\r\n' + sSourceText
sInputTxtText = sSourceNames + '\r\n' + sTargetNames + '\r\n' + sSourceText
# WriteTextFile(sInputTxt, sInputTxtText)
StringToFile(sInputTxtText, sInputTxt)
# SetClipboard(sInputTxtText)

sCommand = sIniFormExe + ' ' + sTempDir + '\\'
ShellRun(sCommand, 1, True)
if not FileExists(sOutputIni): FolderDelete(sTempDir); Exit()

sSourceLanguage = win32api.GetProfileVal('Results', 'Source', '', sOutputIni)
sTargetLanguage = win32api.GetProfileVal('Results', 'Target', '', sOutputIni)
sSourceText = IniFormGetSection(sOutputTxt, 'Text', '')
WriteValue('SourceLanguage', sSourceLanguage)
WriteValue('TargetLanguage', sTargetLanguage)
WriteValue('SourceText', sSourceText.replace('\r\n', '|').strip())

FolderDelete(sTempDir)
sSourceAbbrev = dDetectLanguages[sSourceLanguage]
sTargetAbbrev = dTranslateLanguages[sTargetLanguage]
# for sKey, sValue in dTranslateLanguages.items(): print sKey + '=' + sValue
# print sSourceAbbrev + '|' + sTargetAbbrev
sAddress = 'http://ajax.googleapis.com/ajax/services/language/translate'
dData = {'q' : sSourceText, 'v' : '1.0', 'langpair' : sSourceAbbrev + '|' + sTargetAbbrev}
dHeaders = {'Referer' : 'http://EmpowermentZone.com'}
sResponse = PyWebRequestPostToString(sAddress, dData, dHeaders, '', '')
# print StrDefault(sResponse)
dResponse = simplejson.loads(sResponse)
dResult = dResponse['responseData']

sDetectedAbbrev = dResult.get('detectedSourceLanguage', None)
if sDetectedAbbrev: sDetectedLanguage = dReverseLanguages[sDetectedAbbrev]
else: sDetectedLanguage = sSourceLanguage

sTargetText = dResult['translatedText']
sText = 'Translation from ' + sDetectedLanguage + ' to ' + sTargetLanguage + '\r\n\r\n' + sSourceText.strip() + '\r\n\r\n'
sText += sTargetText.strip() + '\r\n'
SayLine('Loading results')
StringToFile(sText, sOutputFile)



