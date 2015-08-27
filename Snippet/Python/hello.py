# Python

import win32com.client

oST = win32com.client.Dispatch("Say.Tools")
sText = "Hello world"
oST.Say(sText)
oSt = None
