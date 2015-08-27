sAddress = '  http://api.tweetmeme.com/stories/popular.json'
dParams = {'page' : 1, 'count' : 100}
sParams = utf8urlencode(dParams)
sAddress += '?' + sParams
req = urllib2.Request(sAddress)
fResponse = urllib2.urlopen(req)
sResponse = fResponse.read()
dResponse = JsToPyObject(sResponse)
lStories = dResponse.get('stories', None)
if not lStories: Exit('No information available!')
lKeys = 'title excerpt url'.split()  
sText = ''
for dStory in lStories: sText += GetDictionaryText(dStory, lKeys) + '\n'
sText = ('\n' +sText).replace('\nTitle: ', '\r\n\r\n')
sText = ('\n' +sText).replace('\nExcerpt: ', '\r\n\r\n')
sText = sText.strip()
sText = sText.replace('\nUrl: ', '\r\n')
sTitle = '100 Most Common URLs from TweetMeme.com'
sText = sTitle + '\r\n\r\n' + sText
# return IniFormDialogShow(sTitle, message=sText)
SayLine('Loading results')
StringToFile(sText, sOutputFile)


