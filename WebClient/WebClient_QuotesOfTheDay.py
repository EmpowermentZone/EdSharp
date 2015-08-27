sAddress = '  http://api.tweetmeme.com/stories/popular.json'
dParams = {'page' : 1, 'count' : 100}
sAddress = 'http://www.iheartquotes.com/api/v1/random?show_source=1&show_permalink=0&source=humorists+humorix_misc+humorix_stories'
sText = WebRequestGetToString(sAddress)
lLines = sText.strip().split('\n')
sText = '\r\n'.join(lLines[0:-1]).strip()
# sText = sText.replace('\n\n   --  From ', '\r\nFrom ')
sText = RegExpReplace(sText, '[\r\n]+[ -]+', '\r\n')
sText = '1 Funny Quote from IHeartQuotes.com\r\n\r\n' + sText
sText += '\r\n\r\n\r\n' + GetQuotesOfTheDay()
SayLine('Loading results')
StringToFile(sText, sOutputFile)


