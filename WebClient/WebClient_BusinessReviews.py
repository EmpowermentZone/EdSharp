sPhoneNumber = ReadValue('PhoneNumber')
sPhoneNumber = IniFormDialogInput('Input', 'Phone Number', sPhoneNumber)
if not sPhoneNumber: Exit()
WriteValue('PhoneNumber', sPhoneNumber)
SayLine('Please wait')
sAddress = 'http://api.yelp.com/phone_search'
dData = {'phone' : sPhoneNumber, 'ywsid' : '31pKr9tfZrTHlOSDsbrHSg'}
lBusinesses = []
try:
	sResponse = WebRequestGetToString(sAddress, dData)
	dResponse = JsToPyObject(sResponse)
	lBusinesses = dResponse['businesses']
except Exception, e: Exit('Error!  ' + StringUnicode(e))
if not lBusinesses: Exit('No information available!')
sText = ''
for dBusiness in lBusinesses:
	iReviews = dBusiness['review_count']
	if not iReviews: continue
	sText += Pluralize('Business Review', iReviews) + ' for Phone Number ' + sPhoneNumber + ' from yelp.com\r\n'
	sText += dBusiness['name'] + '\r\n'
	sText += 'Average Rating: ' + str(dBusiness['avg_rating']) + '\r\n\r\n'
	lReviews = dBusiness['reviews']
	for dReview in lReviews:
		sText += dReview['text_excerpt'] + '\r\n'
		sText += 'Rating: ' + str(dReview['rating']) + '\r\n\r\n'

sText = sText.strip()
SayLine('Loading results')
StringToFile(sText, sOutputFile)

