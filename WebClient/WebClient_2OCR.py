SayLine('Saving image')
sBase = PathGetBase(sCodeFile)
sImageFile = PathCombine(sAppDir, sBase + '.bmp')
FileDelete(sImageFile)
oRect = ctypes.wintypes.RECT()
h = ctypes.wintypes.windll.user32.GetForegroundWindow()
bMaximized = ctypes.wintypes.windll.user32.IsZoomed(h) 
if not bMaximized: ctypes.wintypes.windll.user32.ShowWindow(h, 3); time.sleep(1)
ctypes.wintypes.windll.user32.GetWindowRect(h, ctypes.wintypes.byref(oRect))
oImage = ImageGrab.grab((oRect.left, oRect.top, oRect.right, oRect.bottom))
if not bMaximized: ctypes.wintypes.windll.user32.ShowWindow(h, 9)
iDpi = 200
oImage.save(sImageFile, dpi=(iDpi, iDpi))
if not FileExists(sImageFile): Exit('Error!')

sExe = PathCombine(sAppDir, 'tesseract.exe')
sCommand = sExe + ' ' + sImageFile + ' ' + sOutputFile[:-4]
ShellRun(sCommand, 0, True)
# print 'dpi', Image.open(sImageFile).info['dpi']
SayLine('Loading results')
os.startfile(sOutputFile)
