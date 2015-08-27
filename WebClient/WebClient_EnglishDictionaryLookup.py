sDictionaryLookup = ReadValue('DictionaryLookup')
if not sDictionaryLookup: sDictionaryLookup = ReadValue('WordInfo')
sDictionaryLookup = IniFormDialogInput('Input', 'Term', sDictionaryLookup)
if not sDictionaryLookup: Exit()
WriteValue('DictionaryLookup', sDictionaryLookup)
SayLine('Please wait')
# sAddress = 'http://en.wikipedia.org/wiki/' + sDictionaryLookup.replace(' ', '_')
sAddress = 'http://en.wiktionary.org'
oBrowser = twill.get_browser()
twill.commands.add_extra_header('User-Agent', 'HomerJax')
oBrowser.go(sAddress)
twill.commands.formvalue(1, 'search', sDictionaryLookup)
twill.commands.submit()
sHtml = oBrowser.result.get_page()
sHtml = utf8str(sHtml)
sText = HtmlToText(sHtml)
# sText = oHomer.HtmlGetText(sHtml, False)
sText = RegExpReplace(sText, r'\[edit\]', '')
sText = RegExpReplace(sText, r'\r\nJump to\:.*?\n', '')
sText = RegExpReplace(sText, r'\A.*?\n', '')
sText = RegExpReplace(sText, r'Wikipedia has an article on\:\s*.*?\s*Wikipedia', '')
while '\r\n\r\n\r\n' in sText: sText = sText.replace('\r\n\r\n\r\n', '\r\n\r\n')
lLines = sText.split('\n')
lLines = lLines[1:]
sText = '\n'.join(lLines)
sText = sText.strip() + '\r\n'
SayLine('Loading results and opening web page')
sUrl = oBrowser.get_url()
os.startfile(sUrl)
# sText = sHtml
# WriteFile(sOutputFile, sText)
StringToFile(sText, sOutputFile)


