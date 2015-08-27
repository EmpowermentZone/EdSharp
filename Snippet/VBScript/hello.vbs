' VBScript

Option Explicit

Dim oST, sText

Set oST = CreateObject("Say.Tools")
sText = "Hello world"
oST.Say sText
Set oST = Nothing
