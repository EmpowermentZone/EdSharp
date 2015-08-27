SayLine('Please wait')
# sAddress = 'http://backstage.bbc.co.uk/data/NewsHeadlinesWorldEdition?v=6ce'
sCnnAddress = 'http://rss.cnn.com/rss/cnn_topstories.rss'
# sAddress = 'http://rss.cnn.com/rss/cnn_latest.rss'
sNyTimesAddress = 'http://www.nytimes.com/services/xml/rss/nyt/HomePage.xml'
sBbcAddress = 'http://newsrss.bbc.co.uk/rss/newsonline_world_edition/front_page/rss.xml'
# sAddress = 'http://rss.news.yahoo.com/rss/topstories'
sCsMonitorAddress = 'http://www.csmonitor.com/rss/world.rss'
sReutersAddress = 'http://feeds.reuters.com/reuters/topNews'
sOddAddress = 'http://feeds.reuters.com/reuters/oddlyEnoughNews'
sYahooAddress = 'http://rss.news.yahoo.com/rss/world'

sBbcText = GetRssItems(sBbcAddress, 'World Report')
sCnnText = GetRssItems(sCnnAddress, 'World Report')
sCsMonitorText = GetRssItems(sCsMonitorAddress, 'World Report')
sNyTimesText = GetRssItems(sNyTimesAddress, 'World Report')
sReutersText = GetRssItems(sReutersAddress, 'World Report')

sText = GetRssItems(sYahooAddress, 'World Report')
sText = StringConvertToUnixLineBreak(sText)
sMatch = r'\n\s*\((\w|\.|\s)+\)\s*\n'
sReplace = '\n'
sText = RegExpReplace(sText, sMatch, sReplace)
sText = StringConvertToWinLineBreak(sText)
sText = sText.replace(r'http://us.rd.yahoo.com/dailynews/rss/world/*', '')
sText = sText.strip() + '\r\n'
sYahooText = sText

s = '----------\r\n\f\r\n'
sText = s + sBbcText + s + sCnnText + s + sCsMonitorText + s + sNyTimesText + s + sReutersText + s + sYahooText
lMatches = RegExpExtract(sText, r'\f\r\n.*?\r\n')
l = copy.copy(lMatches)
lMatches = [s.strip() for s in l]
sMatches = '\r\n'.join(lMatches)
lMatches = RegExpExtract('\r\n' + sMatches, r'\r\n\d+')
iCount = 0
for s in lMatches: iCount += int(s.strip())
sHeader = StringPlural('World Report', iCount) + '\r\n\r\nContents\r\n\r\n' + sMatches + '\r\n'
sText = sHeader + sText

SayLine('Loading results')
StringToFile(sText, sOutputFile)
