sOddAddress = 'http://feeds.reuters.com/reuters/oddlyEnoughNews'
sText = GetRssItems(sOddAddress, 'Odd News Item')
SayLine('Loading results')
StringToFile(sText, sOutputFile)
