dapi = deliciousapi.DeliciousAPI()
sBookmarkTag = ReadValue('BookmarkTag')
sBookmarkTag = IniFormDialogInput('Input', 'Bookmark Tag', sBookmarkTag)
if not sBookmarkTag: Exit()
WriteValue('BookmarkTag', sBookmarkTag)
SayLine('Please wait')
lUrls = dapi.get_urls(tag=sBookmarkTag)
if not lUrls: Exit('No URLs found!')
sText = Pluralize('Recommended Bookmark', len(lUrls)) + ' for ' + sBookmarkTag + ' from delicious.com\r\n\r\n'
i = 0
for sUrl in lUrls: 
	i += 1
	oLink = dapi.get_url(sUrl)
	sText += oLink.title + '\r\n' + oLink.url + '\r\n\r\n'
	SayLine(str(100 * i / len(lUrls)) + '%')
sText = sText.strip()
SayLine('Loading results')
StringToFile(sText, sOutputFile)


