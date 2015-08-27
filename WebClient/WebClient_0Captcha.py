aLabels = ['Solona Name', 'Solona Password', 'Image File']
sSolonaName = ReadValue('SolonaName')
sSolonaPassword = ReadValue('SolonaPassword')
sImageFile = ReadValue('ImageFile', r'C:\11.png')
aValues = [sSolonaName, sSolonaPassword, sImageFile]
aResults = IniFormDialogMultiInput('Multi Input', aLabels, aValues)
if not aResults: Exit()

sSolonaName = aResults[0].strip()
sSolonaPassword = aResults[1].strip()
sImageFile = aResults[2].strip()
WriteValue('SolonaName', sSolonaName)
WriteValue('SolonaPassword', sSolonaPassword)
WriteValue('ImageFile', sImageFile)

"""
Captcha-found
Captcha-id
Captcha-solution
Captcha-solved
Login-status
Message
Operator-status
Password
Session-key
Session-status
Submit-status
Upload-error
Upload-error-message
Userfile
Username
Operator-status:1
"""

sOperatorAddress = 'http://www.solona.net/operators_json.php'
sLogInAddress = 'http://www.solona.net/login_json.php'
sSubmitAddress = 'http://www.solona.net/submit_json.php'
sRetrieveAddress = 'http://www.solona.net/retrieve_json.php'

dData = {}
dData['username'] = sSolonaName
dData['password'] = base64.b64encode(sSolonaPassword)
# dData['password'] = base64.encodestring(sSolonaPassword)
dHeaders = None
sUserName = None
sPassword = None
sAddress = sLogInAddress
# DialogShow(dData['username'], dData['password'])
sResponse = WebRequestPostToString(sAddress, dData, dHeaders, sUserName, sPassword)
print sResponse
dResponse = simplejson.loads(sResponse)
# if not dResponse.get('Operator-status', False): Exit('No operator available!')
if not dResponse.get('Login-status', False): Exit('Unable to log in!')

sSessionKey = dResponse['Session-key']
sAddress = sSubmitAddress
sAddress += '?Session-key=' + sSessionKey
fImage = open(sImageFile, 'rb')
oFileInfo = [('userfile', os.path.basename(sImageFile), fImage.read())]
sResponse = mimepost.post_multipart(sAddress, [], oFileInfo).read()
print sResponse
dResponse = simplejson.loads(sResponse)
if not dResponse.get('Submit-status', False): SayLine(dResponse.get('Upload-error-message', ' ')); Exit('Unable to submit image file!')

sMessage = dResponse['Message']
SayLine(sMessage)
sImageId = dResponse['Captcha-id']
sAddress = sRetrieveAddress
dData = {'image_id':sImageId, 'session-key':sSessionKey}
sAddress += '?' + urllib.urlencode(dData)
SayLine('Please wait')
for iTry in range(9):
	if iTry: SayLine('Check ' + str(iTry + 1))
	time.sleep(10)
	sResponse = WebRequestPostToString(sAddress, dData, dHeaders, sUserName, sPassword)
	print sResponse
	dResponse = simplejson.loads(sResponse)
	if dResponse.get('Captcha-solved', False): break

if not dResponse.get('Captcha-solved', False): Exit('Timed out waiting for CAPTCHA solution!')

sText = dResponse['Captcha-solution']
SayLine('Copying solution to clipboard')
SetClipboardText(sText)
SayLine(sText)

