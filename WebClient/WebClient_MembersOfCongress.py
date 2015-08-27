sZipCode = ReadValue('ZipCode')
sZipCode = IniFormDialogInput('Input', 'ZipCode', sZipCode)
if not sZipCode: Exit()

WriteValue('ZipCode', sZipCode)
sZipCode = StrDefault(sZipCode)
print sZipCode
print type(sZipCode)
try: sText = GetLegislators(sZipCode)
except: Exit('Invalid U.S. zip code!')
SayLine('Loading results')
StringToFile(sText, sOutputFile)


