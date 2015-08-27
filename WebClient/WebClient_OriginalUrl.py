# sShortUrl = GetUrl()
sShortUrl = ''
if not sShortUrl: sShortUrl = ReadValue('ShortUrl')
sShortUrl = IniFormDialogInput('Input', 'Short URL', sShortUrl)
if not sShortUrl: Exit()
if not sShortUrl.lower().find('://') >= 0: sShortUrl = 'http://' + sShortUrl
WriteValue('ShortUrl', sShortUrl)
# sUrl = urlunshort.resolve(sShortUrl)
# fResponse = urllib2.urlopen(sShortUrl)
# sFinalUrl = fResponse.geturl()
sFinalUrl = WebUrlRedirect(sShortUrl)
aParts = urlparse.urlparse(sFinalUrl)
sQuery = aParts[4]
dParams = cgi.parse_qs(sQuery)
if dParams.has_key('url'): lFinalUrl = dParams['url']; sFinalUrl = lFinalUrl[0]
SayLine(sFinalUrl)
SayLine('Copied to clipboard')
SetClipboardText(sFinalUrl)

