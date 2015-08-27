sApiKey = 'bc3txdwxaktwdekkttjqdgp5'
sAddress = 'http://api.zoominfo.com/PartnerAPI/XmlOutput.aspx'
lNames = 'FirstName LastName EmailAddress'.split()
dData = {'query_type' : 'people_search_query', 'pc' : sApiKey}
lValues = []
for sName in lNames: sValue = ReadValue('ZoomInfo' + sName); lValues.append(sValue)
lResults = IniFormDialogMultiInput('Multi Input', lNames, lValues)
if not lResults: Exit()
for i, sName in enumerate(lNames): 
	sResult = lResults[i].strip()
	WriteValue('ZoomInfo' + sName, sResult)
	if sResult and sName.startswith('Email'): dData[sName] = sResult
	elif sResult: dData[sName[0:1].lower() + sName[1:]] = sResult

SayLine('Please wait')
# sResponse = WebRequestGetToString(sAddress, dData)
sAddress += '?' + utf8urlencode(dData)
fResponse = urllib.urlopen(sAddress)
sPrefix = '{http://partners.zoominfo.com/PartnerAPI/XSD/PeopleQuery.xsd}'
oRoot = xml.etree.ElementTree.parse(fResponse).getroot()
# lPersons = oRoot[2]
lPersons = oRoot.findall('.//' + sPrefix + 'PersonRecord')
if not lPersons: Exit('No information available!')
sText = Pluralize('Result', len(lPersons)) + ' from ZoomInfo.com\r\n\r\n'
for oPerson in lPersons: 
	sText += oPerson.findtext(sPrefix + 'FirstName') + ' ' + oPerson.findtext(sPrefix + 'LastName') + '\r\n'
	sText += oPerson.findtext('.//' + sPrefix + 'JobTitle') + '\r\n'
	sText += oPerson.findtext('.//' + sPrefix + 'CompanyName') + '\r\n'
	sText += '\r\n'

sText = sText.strip()
SayLine('Loading results')
StringToFile(sText, sOutputFile)
