#Compile Exe

#Register None

#Dim All

#Include "Win32Api.Inc"
#Include "fn.inc"
#Include "c:\pdf2txt\MyLib.inc"

'    %HKEY_CLASSES_ROOT = &H80000000
%REG_SZ = 1
$Extention = ".txt"
$Title = "txtFile"
$Path = "rubyw.exe"
'$Cmt = "Plain Text"
$Cmt = ""

Function AppPath() As String
  Local hModule As Long
  Local buffer  As Asciiz * 256
  Local I As Long
  hModule = GetModuleHandle(ByVal 0&)
  GetModuleFileName hModule, Buffer, 256
  Local xStr$
  xStr$ = Trim$(buffer)
  For I = 1 To Len(xStr$)
   If Mid$(xStr$, Len(xStr$) - I, 1) = "/" Or Mid$(xStr$, Len(xStr$) - I, 1) = "\" Then
      Function = Left$(xStr$, Len(xStr$) - I)
      Exit Function
   End If
  Next I
  Function = xStr$
End Function

Function PBMain
Dim sDir As String
sDir = FN_GetSpecialPath(%CSIDL_APPDATA)
sDir = sDir + "\EdSharp"
Dim sFile As String
sFile = sDir + "\Assoc.txt"
Dim sText As String
Dim sExts As String
sText = Trim$(FileToString(sFile))
If sText = "" Then sText = "txt ini"
sText = InputBox$("Extensions:", "Disassociate from EdSharp", sText)
If sText = "" Then Exit Function

Dim iExt As Long
Dim iCount As Long
iCount =ParseCount(sText, " ")
For iExt = 1 To iCount
Dim sExt As String
sExt = Parse$(sText, " ", iExt)
sExt = Trim$(sExt)
If sExt = "" Then Iterate
If Left$(sExt, 1) <> "." Then sExt = "." + sExt
sExt = LCase$(sExt)
'MsgBox(sExt)
sExts = sExts + " " + sExt

Dim KeyName   As Asciiz * 256
Dim KeyValue  As Asciiz * 256
Dim KeyHandle As Long

KeyName = sExt
Dim sTitle As String
sTitle = UCase$(Right$(sExt, Len(sExt) - 1)) + " File"
'KeyValue = sTitle
KeyValue = ""

RegCreateKey %HKEY_CLASSES_ROOT, KeyName, KeyHandle

RegSetValue KeyHandle, "", %REG_SZ, KeyValue, 0&

RegCloseKey KeyHandle

If %FALSE Then
KeyName = sTitle: KeyValue = $Cmt

RegCreateKey %HKEY_CLASSES_ROOT, KeyName, KeyHandle

RegSetValue KeyHandle, "", %REG_SZ, KeyValue, 0&

RegCloseKey KeyHandle

KeyName = sTitle: KeyValue = Chr$(34) + AppPath() + $Path + Chr$(34) + " %1"
'KeyValue = command$ + " %1"
'KeyValue = Chr$(34) + Command$ + Chr$(34) + " %1"
RegCreateKey %HKEY_CLASSES_ROOT, KeyName, KeyHandle

RegSetValue KeyHandle, "shell\open\command", %REG_SZ, KeyValue, %MAX_PATH

RegCloseKey KeyHandle
End If

Next
Replace "." With "" In sExts
sExts = Trim$(sExts)

Dim aExts() As String
iCount =ParseCount(sExts, " ")
Dim aExts(iCount - 1)
Parse sExts, aExts(), " "
Array Sort aExts()
sExts = Join$(aExts(), " ")

StringToFile(sExts, sFile)
If Command$ <> "/silent" Then MsgBox ("Extensions " + $LF + sExts + $LF + "are no longer associated with EdSharp"), (%MB_TASKMODAL Or %MB_ICONINFORMATION Or %MB_OK), "Status"
End Function
