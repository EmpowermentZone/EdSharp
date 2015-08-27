sWebSource = GetUrl()
if not sWebSource: sWebSource = ReadValue('WebSource')
sWebSource = IniFormDialogInput('Input', 'Web Source', sWebSource)
if not sWebSource: Exit()

WriteValue('WebSource', sWebSource)
SayLine('Please wait')
lFeeds = feedfinder.feeds(sWebSource)
if not lFeeds: Exit('No feeds found!')
sText = Pluralize('Feed', len(lFeeds)) + ' from ' + sWebSource + '\r\n\r\n'
for sFeed in lFeeds: sText += sFeed + '\r\n\r\n'
sText = sText.strip()
SayLine('Loading results')
StringToFile(sText, sOutputFile)


