aLabels = ['Source Value', 'Target Unit']
sSourceValue = ReadValue('SourceValue')
sTargetUnit = ReadValue('TargetUnit')
aValues = [sSourceValue, sTargetUnit]
aResults = IniFormDialogMultiInput('Convert', aLabels, aValues)
if not aResults: Exit()

SayLine('Please wait')
sSourceValue = aResults[0]
sTargetUnit = aResults[1]
WriteValue('SourceValue', sSourceValue)
WriteValue('TargetUnit', sTargetUnit)

sSearch = sSourceValue + ' = ?' + sTargetUnit
oBrowser = twill.get_browser()
twill.commands.add_extra_header('User-Agent', 'HomerJax')
sAddress = 'http://google.com'
oBrowser.go(sAddress)
twill.commands.formvalue(1, 'q', sSearch)
twill.commands.submit()
sUrl = oBrowser.result.get_url()
sHtml = oBrowser.result.get_page()
sText = HtmlGetText(sHtml)
# SetClipboardText(sText)
sText = StringConvertToUnixLineBreak(sText)
# sMatch = 'Results 1 \- .*?\n+.*?\n'
sMatch = 'Results .*?\n+.*?\n'
lMatches = RegExpExtract(sText, sMatch, True)
sText = lMatches[0]
lLines = sText.split('\n')
sText = lLines[-2]
sText = sText.strip() + '\r\n'
SayLine('Loading results and opening web page')
StringToFile(sText, sOutputFile)
os.startfile(sUrl)

