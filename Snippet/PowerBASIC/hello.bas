' PowerBASIC

Function PBMain()
Dim oST As Dispatch, vText As Variant

Set oST = New Dispatch In "Say.Tools"
vText = "Hello world"
Object Call oST.Say(vText)
Set oST = Nothing
End Function
