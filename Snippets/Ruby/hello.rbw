# Ruby

require 'win32ole'

oST =WIN32OLE.new("Say.Tools")
sText = "Hello world"
oST.Say(sText)
oSt = nil
