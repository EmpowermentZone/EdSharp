' Visual Basic .NET
' Compile on the command line as follows: vbc.exe hello.vb

Option Explicit

Module Test
Sub Main()
Dim oST As Object = CreateObject("Say.Tools")
Dim sText As String = "Hello world"
oST.Say(sText)
oST = Nothing
End Sub
End Module
