sHoroscopeReading = ReadValue('HoroscopeReading')
if not sHoroscopeReading: sHoroscopeReading = ReadValue('WordInfo')
sHoroscopeReading = IniFormDialogInput('Input', 'Zodiac', sHoroscopeReading)
if sHoroscopeReading: sHoroscopeReading = sHoroscopeReading.strip()
if not sHoroscopeReading: Exit()
WriteValue('HoroscopeReading', sHoroscopeReading)
SayLine('Please wait')
# sAddress = 'http://www.astrology.com'
sAddress = 'http://my.horoscope.com/astrology/free-daily-horoscope-' + sHoroscopeReading.lower() + '.html'
# sText = HtmlGetText(sAddress, False)
sHtml = WebRequestGetToString(sAddress)
sText = HtmlToText(sHtml)
# sMatch = r'\| More.*?\S.*?\r\n(.|\n)*?\r\n\r\n'
sMatch = r'\n.*?\, ' + sHoroscopeReading.title() + r'\W(.|\n)*?\r\n\r\n'
lExtracts = RegExpExtract(sText, sMatch, False)
#  if lExtracts.Count < 2: Exit('Not found!')
if lExtracts.Count < 1: Exit('Not found!')
sExtract = lExtracts.Item(0).strip()

sMatch = r'\n.*?\, ' + str(datetime.date.today().year) + r'.*?\n'
lExtracts = RegExpExtract(sText, sMatch, False)
sDate = lExtracts(0).strip()
sHeading = 'Horoscope for ' + sHoroscopeReading[0].upper() + sHoroscopeReading[1:].lower() + '\r\n'
sExtract = sHeading + sDate + '\r\n\r\n' + sExtract + '\r\n'
StringToFile(sExtract, sOutputFile)
SayLine('Loading results')
Exit()

oBrowser = twill.get_browser()
twill.commands.add_extra_header('User-Agent', 'HomerJax')
# twill.commands.config('use_tidy', False)
# twill.commands.config('use_BeautifulSoup', False)
oBrowser.go(sAddress)
sMonth = 'December'
sDay = '14'
sYear = '1963'
iForm = 4
twill.commands.formvalue(iForm, 'date_birth_date1_month', sMonth)
twill.commands.formvalue(iForm, 'date_birth_date1_day', sDay)
twill.commands.formvalue(iForm, 'date_birth_date1_year', sYear)
twill.commands.submit()
sHtml = oBrowser.result.get_page()
sText = HtmlGetText(sHtml, False)
sText = StringTrim(sText) + '\r\n'
sUrl = oBrowser.get_url()
SayLine('Loading results and opening web page')
StringToFile(sText, sOutputFile)
# os.startfile(sUrl)



