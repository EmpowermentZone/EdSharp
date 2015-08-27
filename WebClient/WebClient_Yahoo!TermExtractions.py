sWebContent = GetUrl()
if not sWebContent: sWebContent = ReadValue('WebContent')
sWebContent = IniFormDialogInput('Input', 'Web Content', sWebContent)
if not sWebContent: Exit()
SayLine('Please wait')
WriteValue('WebContent', sWebContent)
if sWebContent.find('://') == -1: sWebContent = 'http://' + sWebContent
sHtml = WebRequestGetToString(sWebContent)
sText = HtmlToText(sHtml)
oSearch = yahoo.search.term.TermExtraction(app_id="ofjjsj7V34FERPRQJW.uOeIOJcBSpM8fMZ.0S2rC7j45C9o3L4v.NbTjIkVPCHOJU9Q-", query="Yahoo")
oSearch.context = sText
lTerms = oSearch.parse_results()
# print type(lTerms)
sText = ''
i = 0
for sTerm in lTerms: i += 1; sText += sTerm + '\r\n'
sText = Pluralize('Yahoo! Term Extraction', i) + ' from ' + sWebContent + '\r\n\r\n' + sText
sText = sText.strip()
SayLine('Loading results')
StringToFile(sText, sOutputFile)

