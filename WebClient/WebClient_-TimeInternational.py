sTimeInternational = ReadValue('TimeInternational')
if not sTimeInternational: sTimeInternational = ReadValue('LocationID')
sTimeInternational = IniFormDialogInput('Input', 'Location', sTimeInternational)
if not sTimeInternational: Exit()
WriteValue('TimeInternational', sTimeInternational)
SayLine('Please wait')
oBrowser = twill.get_browser()
twill.commands.add_extra_header('User-Agent', 'HomerJax')
sAddress = 'http://google.com'
oBrowser.go(sAddress)
twill.commands.formvalue(1, 'q', 'time ' + sTimeInternational)
twill.commands.submit()
sUrl = oBrowser.result.get_url()
os.startfile(sUrl)
sHtml = oBrowser.result.get_page()
sText = HtmlGetText(sHtml)
sText = StringConvertToUnixLineBreak(sText)
sMatch = 'Results 1 \- .*?\n+(.|\n)*?\n\n'
lMatches = RegExpExtract(sText, sMatch)
if len(lMatches): SayLine('Loading results and opening web page')
else: Exit('Opening web page')

sText = lMatches[0]
lLines = sText.split('\n')
sText = lLines[-3]
sText = sText.strip() + '\r\n'
StringToFile(sText, sOutputFile)

