sUrl = 'http://status.calibre-ebook.com/dist/win32'
sFile = 'C:\dl\calibre_setup.msi'
urllib.urlretrieve(sUrl, sFile)
os.system(sFile)
