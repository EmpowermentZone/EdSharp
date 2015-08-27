sProductSearch = ReadValue('ProductSearch')
if not sProductSearch: sProductSearch = ReadValue('WordInfo')
sProductSearch = IniFormDialogInput('Input', 'Product', sProductSearch)
if not sProductSearch: Exit()
WriteValue('ProductSearch', sProductSearch)
SayLine('Please wait')
# sAddress = 'http://www.google.com/ie'
sAddress = 'http://www.google.com'
oBrowser = twill.get_browser()
twill.commands.add_extra_header('User-Agent', 'HomerJax')
# twill.commands.config('use_tidy', False)
# twill.commands.config('use_BeautifulSoup', False)
oBrowser.go(sAddress)
twill.commands.formvalue(1, 'q', 'site:amazon.com ' + sProductSearch)
twill.commands.submit()
twill.commands.follow(r'http://(\w|\.)*amazon\.com')
sHtml = oBrowser.result.get_page()
# sHtml = utf8str(sHtml)
# sText = HtmlToText(sHtml)
sText = HtmlGetText(sHtml, False)
sText = RegExpReplace(sText, r'^(.|\n)*\namazon Home.*?\s*', '')
sText = sText.replace(' Email a FriendPrint Article', '')
sText = StringTrim(sText) + '\r\n'
sUrl = oBrowser.get_url()
# SayLine('Loading results and opening web page')
# StringToFile(sText, sOutputFile)
SayLine('Opening web page')
os.startfile(sUrl)



