[Setup]
AppContact=Jamal Mazrui
AppCopyright=Copyright 2017 by Jamal Mazrui
AppName=EdSharp
AppPublisher=NonvisualDevelopment.org
AppPublisherURL=http://NonvisualDevelopment.org
AppReadmeFile=http://github.com/JamalMazrui/EdSharp
; AppSupportPhone=
; AppSupportURL=http://EdSharp.org
; AppUpdatesURL=http://EdSharp.org
AppVerName=EdSharp 4.0
AppVersion=4.0
; ArchitecturesAllowed=x86 x64 ia64
; ArchitecturesInstallIn64BitMode=x64 ia64
ChangesAssociations=yes
ChangesEnvironment=yes
Compression=lzma2/max
CreateAppDir=yes
CreateUninstallRegKey=yes
DefaultDirName={pf}\EdSharp
DefaultGroupName=EdSharp
DisableDirPage=no
DisableFinishedPage=no
DisableProgramGroupPage=yes
DisableReadyMemo=no
DisableReadyPage=no
DisableStartupPrompt=yes
; DisableWelcomePage=yes
; InfoBeforeFile=Preview.txt
OutputBaseFilename=EdSharp_setup
OutputDir=C:\EdSharp
; OutputManifestFile=EdSharp_setup.txt
PrivilegesRequired=admin
SetupLogging=yes
SolidCompression=yes
SourceDir=C:\EdSharp
Uninstallable=yes

[Files]
Source: "c:\EdSharp\dotnet.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\SayLine.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\Burn2CD.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\Burn2CD.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\Tektosyne.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\EdSharp.cs"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\JsSupport.js"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\JsSupport.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\nvdaControllerClient32.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\saapi32.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\VbSupport.vb"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\VbSupport.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\compileEdSharp.cmd"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\compileVbSupport.cmd"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\compileJsSupport.cmd"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\EdSharp.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\EdSharp.exe.config"; DestDir: "{app}"; Flags: ignoreversion
; Source: "c:\EdSharp\EdSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\EdSharp.ini"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\Hotkeys.ini"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\EdSharp.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\hotkeys.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\history.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\lgpl.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\EdSharp.htm"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\EdSharp_setup.iss"; DestDir: "{app}"; Flags: ignoreversion
Source: "c:\EdSharp\Scripts\EdSharp_Scripts_setup.exe"; DestDir: "{app}\Scripts"; Flags: ignoreversion
Source: "c:\EdSharp\Convert\*.*"; DestDir: "{app}\Convert"; Flags: RecurseSubdirs IgnoreVersion
Source: "c:\EdSharp\Snippets\*.*"; DestDir: "{app}\Snippets"; Flags: RecurseSubdirs
Source: "c:\EdSharp\Snippets\*.*"; DestDir: "{userappdata}\EdSharp\Snippets"; Flags: RecurseSubdirs OnlyIfDoesntExist

[Dirs]
; Name: "{localappdata}\EdSharp";
Name: "{userappdata}\EdSharp";
Name: "{userappdata}\EdSharp\Temp";
; Name: "{app}\Convert"

[Run]
FileName: "{app}\dotnet.exe"; Parameters: """{app}\dotnet_report.txt"""; WorkingDir: "{app}"; Description: "Show report of .NET Framework versions installed"; Flags:
FileName: "{code:NgenExe}"; Parameters: "uninstall EdSharp /nologo /silent"; Flags: runhidden; Check: FileExists(ExpandConstant('{code:NgenExe}'));
FileName: "{code:NgenExe}"; Parameters: "install ""{app}\EdSharp.exe"" /AppBase:""{app}"" /nologo /silent"; Flags: runhidden; Check: FileExists(ExpandConstant('{code:NgenExe}'));
FileName: "cmd.exe"; parameters: "/c"; Description: "Set EdSharp Shortcut Key to Alt+Control+E"; Flags: PostInstall RunAsCurrentUser WaitUntilTerminated; AfterInstall: PostHotkey 
FileName:"http://download.microsoft.com/download/b/e/6/be61cfa4-b59e-4f26-a641-5dbf906dee24/filterpackx86.exe"; Parameters: ""; WorkingDir: "{app}"; Description: "Download and install Microsoft filter pack so EdSharp can read Office 2007 files on 32-bit Windows"; Flags: PostInstall RunAsCurrentUser WaitUntilTerminated unchecked shellexec
FileName:"http://download.microsoft.com/download/b/e/6/be61cfa4-b59e-4f26-a641-5dbf906dee24/filterpackx64.exe"; Parameters: ""; WorkingDir: "{app}"; Description: "Download and install Microsoft filter pack so EdSharp can read Office 2007 files on 64-bit Windows"; Flags: PostInstall RunAsCurrentUser WaitUntilTerminated unchecked shellexec
FileName:"http://status.calibre-ebook.com/dist/win32"; Parameters: ""; WorkingDir: "{app}"; Description: "Download and install calibre ebook software so EdSharp can read ebug files"; Flags: PostInstall RunAsCurrentUser WaitUntilTerminated unchecked shellexec
FileName:"{app}\Scripts\EdSharp_Scripts_setup.exe"; WorkingDir: "{app}"; Description: "Install optional scripts to fine tune JAWS speech"; Flags: PostInstall RunAsCurrentUser WaitUntilTerminated Unchecked
FileName:"{app}\EdSharp.htm"; Description: "Read Documentation for EdSharp"; Flags: shellexec PostInstall RunAsCurrentUser WaitUntilTerminated 
; FileName:"{log}"; Description: "Read log file of this installation"; Flags: PostInstall ShellExec SkipIfSilent; Check: ToggleLogDisplay()
FileName:"http://www.microsoft.com/en-us/download/confirmation.aspx?id=48130      "; Description: "Download and install the Microsoft .NET Framework 4.6 (the latest, recommended .NET Framework)"; Flags: PostInstall RunAsCurrentUser ShellExec Unchecked SkipIfSilent
FileName: "notepad.exe"; Parameters: """{app}\dotnet_report.txt"""; WorkingDir: "{app}"; Description: "Show report of .NET Framework versions installed"; Flags: PostInstall Unchecked

[UninstallRun]
FileName: "{code:NgenExe}"; Parameters: "uninstall EdSharp /nologo /silent"; Flags: runhidden; Check: FileExists(ExpandConstant('{code:NgenExe}'));

[UninstallDelete]
Type: files; Name: "{app}\EdSharp.*"

[Icons]
Name: "{group}\Launch EdSharp"; Filename: "{app}\EdSharp.exe"; Parameters: ""; WorkingDir: "{app}";
Name: "{group}\Read Documentation for EdSharp"; Filename: "{app}\EdSharp.htm"; Flags: RunMaximized
; FileName:"{app}\EdSharp.htm"; Description: "Read Documentation for EdSharp"; Flags: ShellExec PostInstall SkipIfSilent
Name: "{group}\Set Extensions to Open with EdSharp"; Filename: "{app}\assocon.exe"
Name: "{group}\Turn off Association between Extensions and EdSharp"; Filename: "{app}\assocoff.exe"
Name: "{group}\Uninstall EdSharp"; Filename: "{uninstallexe}"
; Name: "{group}\View License for EdSharp"; Filename: "{app}\lgpl.txt"; Flags: RunMaximized
Name: "{group}\View License for EdSharp"; Filename: "{app}\lgpl-3.0.txt"; Flags:
Name: "{app}\EdSharp"; Filename: "{app}\EdSharp.exe"; Parameters: ""; WorkingDir: "{app}";
Name: "{userdesktop}\EdSharp"; HotKey: Alt+Ctrl+E; Filename: "{app}\EdSharp.exe"; Parameters: ""; WorkingDir: "{app}";

[Registry]
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\EdSharp.exe"; ValueType: string; ValueName: ""; ValueData: "{app}\EdSharp.exe";

[Code]
Const
X86 = '\Program Files (x86)\';

var
aJavaVersions, aJavaVersions32, aJavaVersions64: TArrayOfString;
bHotkey, bSetupInitialized, bLogDisplay: boolean;
sJavaDir, sJavaDir32, sJavaDir64: string;
sJAVA_HOME, sJavaHome, sJavaHome32, sJavaHome64: string;
sNgenExe, sNetDir, sNet20Dir, sNet20Dir32, sNet20Dir64, sNet40Dir, sNet40Dir32, sNet40Dir64: string;

function Show(sText: string): integer;
begin
result := SuppressibleMsgBox(sText, mbInformation, MB_OK, MB_OK);
end; // Show function

function Confirm(sText: string): integer;
begin
result := SuppressibleMsgBox(sText, mbConfirmation, MB_YESNO, MB_DEFBUTTON1);
end; // Confirm function

function JavaDir(param: string): string;
var
sVersion: string;

begin
if bSetupInitialized then begin
result := sJavaDir;
exit;
end;

result := '';
if IsWin64() then exit;
try
if not RegQueryStringValue(HKLM, 'SOFTWARE\JavaSoft\Java Runtime Environment', 'CurrentVersion', sVersion) then exit;
if not RegQueryStringValue(HKLM, 'SOFTWARE\JavaSoft\Java Runtime Environment\' + sVersion, 'JavaHome', result) then exit;
except
end;
end; // JavaDir function

function JavaDir32(param: string): string;
var
sVersion: string;

begin
if bSetupInitialized then begin
result := sJavaDir32;
exit;
end;

result := '';
if not IsWin64() then exit;
try
if not RegQueryStringValue(HKLM32, 'SOFTWARE\JavaSoft\Java Runtime Environment', 'CurrentVersion', sVersion) then exit;
if not RegQueryStringValue(HKLM32, 'SOFTWARE\JavaSoft\Java Runtime Environment\' + sVersion, 'JavaHome', result) then exit;
except
end;
end; // JavaDir32 function

function JavaDir64(param: string): string;
var
sVersion: string;

begin
if bSetupInitialized then begin
result := sJavaDir64;
exit;
end;

result := '';
if not IsWin64() then exit;
try
if not RegQueryStringValue(HKLM64, 'SOFTWARE\JavaSoft\Java Runtime Environment', 'CurrentVersion', sVersion) then exit;
if not RegQueryStringValue(HKLM64, 'SOFTWARE\JavaSoft\Java Runtime Environment\' + sVersion, 'JavaHome', result) then exit;
except
end;
end; // JavaDir64 function

function JavaHome(param: string): string;
begin
if bSetupInitialized then begin
result := sJavaHome;
exit;
end;

result := '';
if IsWin64() then exit;
result := sJAVA_HOME;
end; // JavaHome function

function JavaHome32(param: string): string;
var
sDir: string;

begin
if bSetupInitialized then begin
result := sJavaHome32;
exit;
end;

result := '';
if not IsWin64() then exit;
sDir := ExtractFilePath(sJAVA_HOME);
if Pos(X86, sDir) > 0 then result := sJAVA_HOME;
end; // JavaHome32 function

function JavaHome64(param: string): string;
var
sDir: string;

begin
if bSetupInitialized then begin
result := sJavaHome64;
exit;
end;

result := '';
if not IsWin64() then exit;
sDir := ExtractFilePath(sJAVA_HOME);
if Pos(X86, sDir) = 0 then result := sJAVA_HOME;
end; // JavaHome64 function

function JavaVersions(): TArrayOfString;
begin
result := aJavaVersions;
if GetArrayLength(aJavaVersions) <> 0 then exit;

try
if not RegGetSubkeyNames(HKLM, 'SOFTWARE\JavaSoft\Java Runtime Environment', aJavaVersions) then exit;
except
end;
end; // JavaVersions function

function JavaVersions32(): TArrayOfString;
begin
result := aJavaVersions;
if GetArrayLength(aJavaVersions) <> 0 then exit;

try
if not RegGetSubkeyNames(HKLM32, 'SOFTWARE\JavaSoft\Java Runtime Environment', aJavaVersions) then exit;
except
end;
end; // JavaVersions32 function

function JavaVersions64(): TArrayOfString;
begin
result := aJavaVersions;
if GetArrayLength(aJavaVersions) <> 0 then exit;

try
if not RegGetSubkeyNames(HKLM64, 'SOFTWARE\JavaSoft\Java Runtime Environment', aJavaVersions) then exit;
except
end;
end; // JavaVersions64 function

function IsX64: Boolean;
begin
Result := Is64BitInstallMode and (ProcessorArchitecture = paX64);
end; // IsX64 function

function IsIA64: Boolean;
begin
Result := Is64BitInstallMode and (ProcessorArchitecture = paIA64);
end; // IsIA64 function

function IsOtherArch: Boolean;
begin
Result := not IsX64 and not IsIA64;
end; // IsOtherArch function

function NgenExe(param: string): string;
begin
if bSetupInitialized then begin
result := sNgenExe;
exit;
end;

sNgenExe := '';
If sNETDir <> '' Then begin
sNgenExe := sNetDir + '\ngen.exe';
end;
result := sNgenExe;
end; // NgenExe function

function Net20Dir32(param: string): string;
begin
if bSetupInitialized then begin
result := sNet20Dir32;
exit;
end;

try
result := ExpandConstant('{dotnet2032}');
except
result := ''
end;
end; // Net20Dir32 function

function Net20Dir64(param: string): string;
begin
if bSetupInitialized then begin
result := sNet20Dir64;
exit;
end;

try
result := ExpandConstant('{dotnet2064}');
except
result := ''
end;
end; // Net20Dir64 function

function Net40Dir32(param: string): string;
begin
if bSetupInitialized then begin
result := sNet40Dir32;
exit;
end;

try
result := ExpandConstant('{dotnet4032}');
except
result := ''
end;
end; // Net40Dir32 function

function Net40Dir64(param: string): string;
begin
if bSetupInitialized then begin
result := sNet40Dir64;
exit;
end;

try
result := ExpandConstant('{dotnet4064}');
except
result := ''
end;
end; // Net40Dir64 function

function IsJavaDir(): boolean;
begin
result := not IsWin64() and DirExists(ExpandConstant('{code:JavaDir}'));
end; // IsJavaDir function

function IsJavaDir32(): boolean;
begin
result := Is64BitInstallMode() and DirExists(ExpandConstant('{code:JavaDir32}'));
end; // IsJavaDir32 function

function IsJavaDir64(): boolean;
begin
result := Is64BitInstallMode() and DirExists(ExpandConstant('{code:JavaDir64}'));
end; // IsJavaDir64 function

function IsJavaHome(): boolean;
begin
result := not IsWin64() and DirExists(ExpandConstant('{code:JavaHome}'));
end; // IsJavaHome function

function IsJavaHome32(): boolean;
begin
result := Is64BitInstallMode() and DirExists(ExpandConstant('{code:JavaHome32}'));
end; // IsJavaHome32 function

function IsJavaHome64(): boolean;
begin
result := Is64BitInstallMode() and DirExists(ExpandConstant('{code:JavaHome64}'));
end; // IsJavaHome64 function

function GetDetection(): string;
var
sText: string;

begin
sText := 'Locations of Java Runtime Environment:' + Chr(10);
if sJavaDir <> '' then sText := sText + 'JavaDir=' + sJavaDir + Chr(10);
if sJavaDir32 <> '' then sText := sText + 'JavaDir32=' + sJavaDir32 + Chr(10);
if sJavaDir64 <> '' then sText := sText + 'JavaDir64=' + sJavaDir64 + Chr(10);
if sJAVA_HOME <> '' then sText := sText + 'JAVA_HOME=' + sJAVA_HOME + Chr(10);
result := sText;
end; // GetDetection function

function old_UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo, MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
sText: string;

begin
sText := '';
if MemoUserInfoInfo <> '' then sText := sText + MemoUserInfoInfo + NewLine + NewLine;
if MemoDirInfo <> '' then sText := sText + MemoDirInfo + NewLine + NewLine;
if MemoTypeInfo <> '' then sText := sText + MemoTypeInfo + NewLine + NewLine;
if MemoComponentsInfo <> '' then sText := sText + MemoComponentsInfo + NewLine + NewLine;
if MemoGroupInfo <> '' then sText := sText + MemoGroupInfo + NewLine + NewLine;
if MemoTasksInfo <> '' then sText := sText + MemoTasksInfo + NewLine + NewLine;
result := sText + GetDetection();
end; // UpdateReadyMemo function

function InitializeSetup(): boolean;
var
iChoice, iError: integer;
sText: string;

begin
bHotkey := false;
(*
sJavaDir := JavaDir('');
sJavaDir32 := JavaDir32('');
sJavaDir64 := JavaDir64('');

sJAVA_HOME := GetEnv('JAVA_HOME');
sJavaHome := JavaHome('');
sJavaHome32 := JavaHome32('');
sJavaHome64 := JavaHome64('');

// aJavaVersions := JavaVersions();
// aJavaVersions32 := JavaVersions32();
// aJavaVersions64 := JavaVersions64();

sNet20Dir32 := Net20Dir32();
sNet20Dir64 := Net20Dir64();
sNet40Dir32 := Net40Dir32();
sNet40Dir64 := Net40Dir64();
*)

sNetDir := Net40Dir32('');
sNgenExe := NgenExe('');

(*
if FileExists(ExpandConstant('{code:NgenExe}')) then Show('found')
else Show('missing');

if IsJavaDir() Then Show('IsJavaDir')
else Show('Not IsJavaDir');

if IsJavaDir32() Then Show('IsJavaDir32')
else Show('Not IsJavaDir32');

if IsJavaDir64() Then Show('IsJavaDir64')
else Show('Not IsJavaDir64');

Show(ExpandConstant('{code:JavaHome}'));
if IsJavaHome() Then Show('IsJavaHome')
else Show('Not IsJavaHome');

if IsJavaHome32() Then Show('IsJavaHome32')
else Show('Not IsJavaHome32');

if IsJavaHome64() Then Show('IsJavaHome64')
else Show('Not IsJavaHome64');
*)

bSetupInitialized := True;
result := true;

(*
if IsJavaDir() or IsJavaDir32() or IsJavaDir64() or (sJAVA_HOME <> '') then result := True
else begin
bSetupInitialized := False;
iChoice := Confirm('The Java Access Bridge cannot be installed because a Java Runtime Environment (JRE) is not found on this computer.  Get it now from java.com?');
if iChoice = IDYES then begin
Show('When the web site opens, click the link called "Free Java Download."  Afterward, rerun this installer.');
ShellExec('open',     'http://www.java.com/getjava/',     '','',SW_SHOWNORMAL,ewNoWait,iError);
end
result := False;
end
*)
end; // InitializeSetup function

procedure old_DeinitializeSetup();
var
bResult: boolean;
iResult: integer;
sSource, sTarget: string;

begin
sSource := ExpandConstant('{log}');
try
sTarget := ExpandConstant('{app}\') + ExtractFileName(sSource);
bResult := FileCopy(sSource, sTarget, False);
ShellExec('open', sTarget, '','',SW_SHOWNORMAL,ewNoWait,iResult);
except
finally
DeleteFile(sSource);
end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
bResult: boolean;
begin
if CurStep <> ssDone then exit;
if bHotkey then exit;
// Show('About to copy');
// Show(ExpandConstant('{app}\EdSharp.lnk'));
// Show(ExpandConstant('{userdesktop}\EdSharp.lnk'));
bResult := FileCopy(ExpandConstant('{app}\EdSharp.lnk'), ExpandConstant('{userdesktop}\EdSharp.lnk'), false);
// if bResult then Show('success')
// else Show('failed');
end; // CurStepChanged procedure

procedure PostHotkey();
begin
bHotkey := true;
// Show('PostHotkey');
end; // PostHotkey procedure


