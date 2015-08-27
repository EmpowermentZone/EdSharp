sPhysicianOnline = ReadValue('PhysicianOnline')
if not sPhysicianOnline: sPhysicianOnline = ReadValue('WordInfo')
sPhysicianOnline = IniFormDialogInput('Input', 'Topic', sPhysicianOnline)
if not sPhysicianOnline: Exit()
WriteValue('PhysicianOnline', sPhysicianOnline)
SayLine('Please wait')
sAddress = 'http://www.google.com'
# sAddress = 'http://google.com/ie'
oBrowser = twill.get_browser()
twill.commands.add_extra_header('User-Agent', 'HomerJax')
# twill.commands.config('use_tidy', False)
# twill.commands.config('use_BeautifulSoup', False)
oBrowser.go(sAddress)
twill.commands.formvalue(1, 'q', 'site:webmd.com ' + sPhysicianOnline)
twill.commands.submit()
twill.commands.follow(r'http://(\w|\.)*webmd\.com')
twill.commands.follow(r'Print Article')
sHtml = oBrowser.result.get_page()
# sHtml = utf8str(sHtml)
# sText = HtmlToText(sHtml)
sText = HtmlGetText(sHtml, False)
sText = RegExpReplace(sText, r'^(.|\n)*\nWebMD Home.*?\s*', '')
sText = sText.replace(' Email a FriendPrint Article', '')
sText = StringTrim(sText) + '\r\n'
sUrl = oBrowser.get_url()
SayLine('Loading results and opening web page')
StringToFile(sText, sOutputFile)
os.startfile(sUrl)



