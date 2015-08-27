sKnowledgeWikipedia = ReadValue('KnowledgeWikipedia')
if not sKnowledgeWikipedia: sKnowledgeWikipedia = ReadValue('WordInfo')
sKnowledgeWikipedia = IniFormDialogInput('Input', 'Topic', sKnowledgeWikipedia)
if not sKnowledgeWikipedia: Exit()
WriteValue('KnowledgeWikipedia', sKnowledgeWikipedia)
SayLine('Please wait')
sAddress = 'http://www.google.com/ie'
sAddress = 'http://www.google.com'
oBrowser = twill.get_browser()
twill.commands.add_extra_header('User-Agent', 'HomerJax')
# twill.commands.config('use_tidy', False)
# twill.commands.config('use_BeautifulSoup', False)
# oBrowser.go(sAddress)
twill.commands.go(sAddress)
twill.commands.formvalue(1, 'q', 'site:wikipedia.org ' + sKnowledgeWikipedia)
twill.commands.submit()
# twill.commands.follow('en.wikipedia.org')
twill.commands.follow(r'http://(\w|\.)*wikipedia\.org')

sHtml = oBrowser.result.get_page()
# sHtml = utf8str(sHtml)
# sText = HtmlToText(sHtml)
sText = HtmlGetText(sHtml, False)
sText = RegExpReplace(sText, r'\[edit\]', '')
sText = RegExpReplace(sText, r'\r\nJump to\:.*?\n', '')
sText = RegExpReplace(sText, r'\A.*?\n', '')
# sText = RegExpReplace(sText, r'Wikipedia has an article on\:\s*.*?\s*Wikipedia', '')
while '\r\n\r\n\r\n' in sText: sText = sText.replace('\r\n\r\n\r\n', '\r\n\r\n')
sText = StringTrim(sText) + '\r\n'
sUrl = oBrowser.get_url()
SayLine('Loading results and opening web page')
StringToFile(sText, sOutputFile)
os.startfile(sUrl)



