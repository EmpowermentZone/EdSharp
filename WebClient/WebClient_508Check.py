sWebAddress = GetUrl()
if not sWebAddress: sWebAddress = ReadValue('WebAddress')
sWebAddress = IniFormDialogInput('Input', 'Web Address', sWebAddress)
if not sWebAddress: Exit()
if sWebAddress.find('://') == -1: sWebAddress = 'http://' + sWebAddress
WriteValue('WebAddress', sWebAddress)
SayLine('Please wait')
sAddress = 'http://CynthiaSays.com'
oBrowser = twill.get_browser()
twill.commands.add_extra_header('User-Agent', 'HomerJax')
oBrowser.go(sAddress)
twill.commands.formvalue(1, 'url', sWebAddress)
twill.commands.submit()
sHtml = oBrowser.result.get_page()
sHtml = utf8str(sHtml)
# print sHtml
# sText = html2text.html2text(sHtml)
sText = HtmlToText(sHtml)
sMatch = r'HiSoftware can help you meet (.|\n)*?show all detail.' 
sText = RegExpReplace(sText, sMatch, '')
sAlpha = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
for s in sAlpha: sText = sText.replace('\r\n' + s + '. ', '\r\n\r\n' + s + '. ')
while sText.find('\r\n\r\n\r\n') >= 0: sText = sText.replace('\r\n\r\n\r\n', '\r\n\r\n')
sText = sText.strip()
SayLine('Loading results and opening web page')
StringToFile(sText, sOutputFile)
os.startfile('http://wave.webaim.org/report?url=' + sWebAddress)
Exit()

# sAddress = 'http://www.34alabs.com/achecker/checkacc.php'
# sAppKey = 'ff38290cfdbad5f4d54a0772c05d77e7184a7002'
sAddress = 'http://achecker.ca/checkacc.php'
sAppKey = 'f06d23ded5b6c1a37da25ac4066d9bbc2a997826'
# dData = {'uri' : sWebAddress, 'id' : sAppKey, 'guide' : 'WCAG2-AA', 'output' : 'html'}
dData = {'uri' : sWebAddress, 'id' : sAppKey}
sResponse = WebRequestGetToString(sAddress, dData)
# print sResponse


