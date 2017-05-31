//EdSharp 4.0
// May 30, 2017
//Copyright 2007 - 2017 by Jamal Mazrui
// GNU Lesser General Public License (LGPL)

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Compatibility.VB6;
using MyIO = Microsoft.VisualBasic.FileIO;
using Microsoft.VisualBasic.MyServices;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

using Tektosyne.NetMail ;
using Tektosyne.Win32Api;

[assembly: AssemblyTitle("EdSharp")]
[assembly: AssemblyProduct("EdSharp")]
[assembly: AssemblyVersion("4.0.*")]
[assembly: AssemblyDescription("EdSharp editor")]
[assembly: AssemblyCompany("EmpowermentZone.com")]
[assembly: AssemblyCopyright("Copyright 2007 - 2016 by Jamal Mazrui")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCulture("")]

namespace EdSharp {
public class App : WindowsFormsApplicationBase {
public static App Shell;
public static MdiFrame Frame;
public static string ProgramName;
public static string NetDir;
public static string ProgramDir;
public static string DataDir;
public static string DefaultIniFile;
public static string HotkeyIniFile;
public static string IniFile;
public static string IndentModeFile;
public static string TempFile;
public static List<string> TempFiles = new List<string>();
//public static object Word = null;
public static object Boo = null;
public static object JAWS = null;
public static object Wineyes = null;
public static bool WordCreated = false;
public static bool ExtraSpeech = true;
public static bool IndentChange = true;
public static bool CaptureOutput = false;
public static string SpeechLog;
public static string MatchChunk = @"\s+";
public static string MatchParagraph = @"\n(\s*\n)+\s*";
public static string MatchSentence = @"([.?!]\s+)|(" + MatchParagraph + ")";
public static Dictionary<string, int> BomDictionary = null;

[STAThread]
public static void Main(string[] cmdLineArgs) {
// Environment.SetEnvironmentVariable("EdSharpIndent", "", EnvironmentVariableTarget.User);
if (System.IO.File.Exists(App.IndentModeFile)) System.IO.File.Delete(App.IndentModeFile);
Application.EnableVisualStyles();
//Application.SetCompatibleTextRenderingDefault(true);
Application.SetCompatibleTextRenderingDefault(false);
Application.OleRequired();

Shell = new App();
Shell.Run(cmdLineArgs);
} // Main method

public App() {
base.IsSingleInstance = true;
/*
//this.IsSingleInstance = true;
base.IsSingleInstance = true;
App.ProgramName = GetAppName();
App.NetDir = RuntimeEnvironment.GetRuntimeDirectory();
// App.ProgramDir = GetProgramDir();
App.ProgramDir = GetProgramDir();
App.DataDir = GetDataDir();
App.TempFile = GetTempFile();
App.DefaultIniFile = GetDefaultIniFile();
App.HotkeyIniFile = Path.Combine(App.ProgramDir, "Hotkeys.ini");
App.IniFile = GetIniFile();

App.BomDictionary = Util.GetBomDictionary();
SetConfigurationValues();
App.SpeechLog = Path.Combine(App.DataDir, "Speech.log");
if (File.Exists(App.SpeechLog)) File.Delete(App.SpeechLog);
App.ExtraSpeech = (App.ReadOption("E&xtraSpeech", "Y").ToLower().Substring(0, 1) == "n") ? false : true;

InitNetSdk();
InitJFW();
*/

this.Shutdown += delegate(object o, EventArgs e) {
if (App.WordCreated) {
Util.Say("Exiting Microsoft Word");
COM.WordExit();
}
if (App.Boo != null) COM.Release(ref App.Boo);
if (App.JAWS != null) COM.Release(ref App.JAWS);
if (App.Wineyes != null) COM.Release(ref App.Wineyes);

if (System.IO.File.Exists(App.IndentModeFile)) System.IO.File.Delete(App.IndentModeFile);
foreach (string sFile in App.TempFiles) if (File.Exists(sFile)) File.Delete(sFile);
};

this.UnhandledException += delegate(object sender, Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs e) {
Exception ex = (Exception) e.Exception;
string sMessage = ex.Message;
sMessage += "\n\nStack trace:\n" + ex.StackTrace;
// sMessage += "\nExit EdSharp?\n\nStack trace:\n" + ex.StackTrace;
// e.ExitApplication = Dialog.Confirm("Confirm", "Unexpected event!\n" + sMessage + ".\nExit EdSharp?", "N") == "Y";
string[] aButtons = {"&Mail to Developer", "Copy to Clipboard", "Exit EdSharp"};
string sButton = Dialog.Choose("Unexpected Event", sMessage, aButtons, 0);
switch (sButton) {
case "&Mail to Developer" :
Util.Say("Please add steps to reproduce the problem, if possible.");
string sSubject = "EdSharp error: " + ex.Message;
KeyValuePair<string, string>[] aAddresses = new KeyValuePair<string, string>[1];
string sName = "Jamal Mazrui";
string sAddress = "jamal@EmpowermentZone.com";
aAddresses[0] = new KeyValuePair<String, String>(sName, sAddress);
try {
MapiMail.SendMail(sSubject, sMessage, aAddresses, null);
}
catch {
Util.MailMessage(sAddress, sSubject, sMessage);
}
break;
case "Copy to Clipboard" :
Clipboard.SetText(sMessage);
break;
case "Exit EdSharp" :
// Application.Exit();
e.ExitApplication = true;
return;
}
e.ExitApplication = false;
};

this.Startup += delegate(object sender, Microsoft.VisualBasic.ApplicationServices.StartupEventArgs e) {

//this.IsSingleInstance = true;
// base.IsSingleInstance = true;
App.ProgramName = GetAppName();
App.NetDir = RuntimeEnvironment.GetRuntimeDirectory();
App.ProgramDir = GetProgramDir();
App.ProgramDir = GetProgramDir();
App.DataDir = GetDataDir();
App.TempFile = GetTempFile();
App.DefaultIniFile = GetDefaultIniFile();
App.HotkeyIniFile = Path.Combine(App.ProgramDir, "Hotkeys.ini");
App.IniFile = GetIniFile();
App.IndentModeFile = Path.Combine(App.DataDir, "IndentMode.tmp");
// IniFile = GetIniFile();

App.BomDictionary = Util.GetBomDictionary();
SetConfigurationValues();
App.SpeechLog = Path.Combine(App.DataDir, "Speech.log");
if (File.Exists(App.SpeechLog)) File.Delete(App.SpeechLog);
App.ExtraSpeech = (App.ReadOption("E&xtraSpeech", "Y").ToLower().Substring(0, 1) == "n") ? false : true;
App.IndentChange = App.ReadOption("E&xtraSpeech", "Y").Contains("-") ? false : true;

InitNetSdk();
InitJFW();

Frame = new MdiFrame();
this.MainForm = Frame;
MdiChild child = new MdiChild(Frame);
if (App.ReadOption("OpenPrevious", "Y").ToLower().Substring(0, 1) != "n") {
string[] aFiles = App.ReadSectionKeys("Previous");
int iCount = 0;
foreach (string s in aFiles) {
if (!File.Exists(s)) continue;
iCount ++;
int iIndex = Int32.Parse(App.ReadValue("Previous", s, "-1"));
// App.Frame.OpenOrActivateWindow(s, 1);
App.Frame.OpenOrActivateWindow(s, App.Frame.GetViewLevel(s));
if (App.Frame.Child.RTB.Index == 0) App.Frame.Child.RTB.Index = iIndex;
}
if (iCount > 0) App.Frame.AddMessage("Opened " + iCount + " previous file" + (iCount == 1 ? "" : "s"));
}
App.DeleteSection("Previous");

ReadOnlyCollection<string> cmdLineArgs = this.CommandLineArgs;
if (cmdLineArgs.Count > 0) {
string sFile = cmdLineArgs[0];
string sLine = "";
string sColumn = "";
if (cmdLineArgs.Count > 1) sLine = cmdLineArgs[1];
if (cmdLineArgs.Count > 2) sColumn = cmdLineArgs[2];
// Frame.OpenOrActivateWindow(sFile, 1, sLine, sColumn);
App.Frame.OpenOrActivateWindow(sFile, App.Frame.GetViewLevel(sFile), sLine, sColumn);
}
};

} // App constructor

protected override void OnStartupNextInstance(StartupNextInstanceEventArgs e) {
/*
Util.ActivatePid(Process.GetCurrentProcess().Id);
Microsoft.VisualBasic.Interaction.AppActivate(App.Frame.Text);
App.Frame.Activate();
Util.ActivateTitle(App.Frame.Text);
*/
//COM.ActivateTitle(App.Frame.Text);
//System.Threading.Thread.Sleep(1000);

/*
object oAutoIt = COM.CreateObject("AutoItX3.Control");
object[] aParams = {"WinTitleMatchMode", 4};
COM.CallMethod(oAutoIt, "AutoItSetOption", aParams);
string sParam = "handle=" + App.Frame.TopLevelControl.Handle.ToString();
COM.CallMethod(oAutoIt, "WinActivate", sParam);
Win32.SetForegroundWindow(App.Frame.TopLevelControl.Handle);
*/

//Process.Start(Path.Combine(App.ProgramDir, "ForceWin.exe"), App.Frame.TopLevelControl.Handle.ToString());
Win32.ForceWindow(App.Frame.TopLevelControl.Handle);

if (e.CommandLine.Count == 0) return;
string sFile = Util.Unquote(e.CommandLine[0]);
string sLine = "";
string sColumn = "";
if (e.CommandLine.Count > 1) sLine = e.CommandLine[1];
if (e.CommandLine.Count > 2) sColumn = e.CommandLine[2];
if (sFile != null && File.Exists(sFile)) App.Frame.OpenOrActivateWindow(sFile, App.Frame.GetViewLevel(sFile), sLine, sColumn);
} // OnStartUpNextInstance handler

public static bool InitJFW() {
string sDir = Win32.GetJFWDir();
if (sDir.Length == 0) return false;

string sPath = Environment.GetEnvironmentVariable("PATH");
sDir += ";";
if (!sPath.ToLower().Contains(sDir.ToLower())) {
sPath = sDir + sPath;
Environment.SetEnvironmentVariable("PATH", sPath);
}
return true;
} // InitJFW method

public static bool InitNetSdk() {
// Does not work
// string sDir = Win32.GetNetRuntimeDir();
// string sDir = Win32.GetNetSdkDir();
string sDir = RuntimeEnvironment.GetRuntimeDirectory();
// Dialog.Show("RuntimeEnvironment", RuntimeEnvironment.GetSystemVersion() + "\r\n" + RuntimeEnvironment.GetRuntimeDirectory() + "\r\n" + RuntimeEnvironment.SystemConfigurationFile);
// Dialog.Show(sDir);

if (sDir.Length == 0) return false;
if (sDir.EndsWith(@"\")) sDir = sDir.Substring(0, sDir.Length - 2);

string sPath = Environment.GetEnvironmentVariable("PATH");
sDir += ";";
if (!sPath.ToLower().Contains(sDir.ToLower())) {
sPath = sDir + sPath;
Environment.SetEnvironmentVariable("PATH", sPath);
// Clipboard.SetText(Environment.GetEnvironmentVariable("PATH"));
}
return true;
} // InitNetSdk method

public static string GetAppName() {
string sExe = Environment.GetCommandLineArgs()[0];
string sReturn = Path.GetFileNameWithoutExtension(sExe);
sReturn = Application.ProductName;
return sReturn;
} // GetAppName method

public static string GetProgramDir() {
//string sApp = System.Reflection.Assembly.GetExecutingAssembly().Location;
//string sApp = Application.ExecutablePath;
//string sReturn = Path.GetDirectoryName(sApp);
string sReturn = Application.StartupPath;
return sReturn;
} // GetProgramDir method

public static string GetDataDir() {
string sName = GetAppName();
//string sDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
//string sDir = Application.UserAppDataPath;
//string sDir = Application.LocalUserAppDataPath
string sDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
string sReturn = Path.Combine(sDir, sName);
if (!Directory.Exists(sReturn)) Directory.CreateDirectory(sReturn);
return sReturn;
} // GetDataDir method

public static string GetTempFile() {
string sName = GetAppName() + ".tmp";
string sDir = GetDataDir();
string sReturn = Path.Combine(sDir, sName);
App.TempFiles.Add(sReturn);
return sReturn;
} // GetTempFile method

public static string GetIniFile() {
string sName = GetAppName() + ".ini";
string sDir = GetDataDir();
string sReturn = Path.Combine(sDir, sName);
return sReturn;
} // GetIniFile method

public static string GetDefaultIniFile() {
string sName = GetAppName() + ".ini";
string sDir = GetProgramDir();
string sReturn = Path.Combine(sDir, sName);
return sReturn;
} // GetDefaultIniFile method

public static string ReadData(string sKey, string sDefault) {
string sSection = "Data";
return ReadValue(sSection, sKey, sDefault);
} // ReadData method

public static bool WriteData(string sKey, string sValue) {
string sSection = "Data";
//return WriteValue(sSection, sKey, sValue);
return Ini.WriteQuote(App.IniFile, sSection, sKey, sValue);
} // WriteData method

public static string ReadDefaultOption(string sKey, string sDefault) {
string sSection = "Options";
return Ini.ReadValue(App.DefaultIniFile, sSection, sKey, sDefault);
} // ReadDefaultOption method

public static string ReadOption(string sKey, string sDefault) {
string sSection = "Options";
return ReadValue(sSection, sKey, sDefault);
} // ReadOption method

public static string ReadValue(string sSection, string sKey, string sDefault) {
return Ini.ReadValue(App.IniFile, sSection, sKey, sDefault);
} // ReadValue method

public static bool WriteOption(string sKey, string sValue) {
string sSection = "Options";
return WriteValue(sSection, sKey, sValue);
} // WriteOption method

public static bool WriteValue(string sSection, string sKey, string sValue) {
// return Ini.WriteValue(App.IniFile, sSection, sKey, sValue);
return Ini.WriteQuote(App.IniFile, sSection, sKey, sValue);
} // WriteValue method

public static bool DeleteKey(string sSection, string sKey) {
return Ini.DeleteKey(App.IniFile, sSection, sKey);
} // DeleteKey method

public static bool DeleteSection(string sSection) {
return Ini.DeleteSection(App.IniFile, sSection);
} // DeleteSection method

public static string[] ReadDefaultOptions() {
string sSection = "Options";
return ReadDefaultSectionKeys(sSection);
} // ReadOptions method

public static string[] ReadDefaultSectionKeys(string sSection) {
bool bIncludeComments = false;
return ReadDefaultSectionKeys(sSection, bIncludeComments);
} // ReadDefaultSectionKeys method

public static string[] ReadDefaultSectionKeys(string sSection, bool bIncludeComments) {
return Ini.ReadSectionKeys(App.DefaultIniFile, sSection, bIncludeComments);
} // ReadDefaultSectionKeys method

public static string[] ReadSectionKeys(string sSection) {
return Ini.ReadSectionKeys(App.IniFile, sSection);
} // ReadSectionKeys method

public static string[] ReadSections() {
return Ini.ReadSections(App.IniFile);
} // ReadSections method

public static void SetConfigurationValues() {
string[] aSections = Ini.ReadSections(App.DefaultIniFile);
foreach (string sSection in aSections) {
string[] aCommands = Ini.ReadSectionKeys(App.DefaultIniFile, sSection, false);
string[] aKeys = new string[aCommands.Length];
for (int i = 0; i < aCommands.Length; i++) {
string sCommand = aCommands[i];
string sKey = Ini.ReadValue(App.DefaultIniFile, sSection, sCommand, "");
sKey = Ini.ReadValue(App.IniFile, sSection, sCommand, sKey);
aKeys[i] = sKey;
}

//Ini.DeleteSection(App.IniFile, sSection);
for (int i = 0; i < aCommands.Length; i++) {
string sCommand = aCommands[i];
string sKey = aKeys[i];
//if (sSection == "Import" || sSection == "Export") Ini.WriteValue(App.IniFile, sSection, sCommand, sKey);
//else Ini.WriteQuote(App.IniFile, sSection, sCommand, sKey);
Ini.WriteQuote(App.IniFile, sSection, sCommand, sKey);
}
}
} // SetConfigurationValues method

} // App class

public class MdiChild : Form {
public HomerRichTextBox RTB;
public Encoding YieldEncoding = null;
public bool IsUnixLineBreak = false;
public int AppendFromClipboard = 0;
public IntPtr NextClipboardViewer = (IntPtr) 0;
public int LastTickCount = 0;
public string LastClipboardText = "";
private string sFile = "";
public string File {
get {
return sFile;
}
set {
sFile = value;
}
} // File property

public DateTime FileTime;
public bool FileTimeChecked = false;
public MdiChild(MdiFrame frame) {
string sTitle = frame.GetNoNameTitle();
new MdiChild(frame, sTitle);
} // MdiChild constructor

public MdiChild(MdiFrame frame, string sTitle) {
this.SuspendLayout();
this.MdiParent = frame;
HomerRichTextBox rtb = new HomerRichTextBox();
rtb.GotFocus += CheckFileTime;
rtb.AccessibleRole = AccessibleRole.Text;
rtb.AutoWordSelection = false;
rtb.Dock = DockStyle.Fill;
rtb.Multiline = true;

string sFont = App.ReadOption("FontDefault", "");
if (sFont.Length > 0) {
string[] a = sFont.Split(',');
List<string> list = new List<string>(a);
int iCount = list.Count;
string sColor = list[iCount - 1];
try {
sColor = sColor.Split('=')[1];
rtb.ForeColor = Util.String2Color(sColor);
}
catch {}

list.RemoveAt(iCount - 1);
a = list.ToArray();
sFont = String.Join(",", a);
try {
//sFont = "Arial Unicode MS";
rtb.Font = Util.String2Font(sFont);
}
catch {}
}

string s = App.ReadOption("WordWrap", "Y").Trim().ToUpper();
if (s == "N" || s == "NO") rtb.SetWrap(false);
else rtb.SetWrap(true);
//rtb.ScrollBars = RichTextBoxScrollBars.Vertical;
rtb.ScrollBars = RichTextBoxScrollBars.Vertical | RichTextBoxScrollBars.Horizontal;
rtb.AcceptsTab = true;
rtb.FindText = "";
rtb.JumpLine = "";
rtb.GoPercent = "";
rtb.SearchTopic = "";
rtb.SelectionChanged += App.Frame.SetStatusAddress;
this.Controls.Add(rtb);
this.RTB = rtb;
//this.File = frame.GetNoNameTitle();
this.File = sTitle;
this.Text = System.IO.Path.GetFileName(this.File);
this.StartPosition = FormStartPosition.CenterParent;
this.AutoSize = true;
this.ResumeLayout();
this.KeyPreview = true;
this.Activated += delegate(object o, EventArgs e) {
frame.SetStatusAddress(this, null);
//this.WindowState = FormWindowState.Maximized;
//Win32.SetForegroundWindow(App.Frame.Handle);
//Win32.SetForegroundWindow(this.Handle);
//COM.ActivateTitle("EdSharp");
//int iPid = Process.GetCurrentProcess().Id;
//if (iPid > 0) Util.ActivatePid(iPid);

//Win32.ForceWindow(App.Frame.TopLevelControl.Handle);
};

string sText, sResult = "";
this.Shown += delegate(object o, EventArgs e) {
this.WindowState = FormWindowState.Maximized;
//Win32.ForceWindow(App.Frame.TopLevelControl.Handle);

sFile = this.File;
if (!sFile.Contains(@"\")) return;

sText = App.ReadValue("Favorites", sFile, "");
try {
string[] a = sText.Split('|');
sText = a[0];
rtb.Index = Int32.Parse(sText);
return;
}
catch {}

sText = App.ReadValue("Recent", sFile, "");
if (sText.Length == 0) return;

rtb = this.RTB;
HomerList hl = new HomerList(sText);
hl.KeepLike(@"^\d+$");
// hl.Remove("-1");
if (hl.Count == 0) {
return;
}

sResult = hl[0];
rtb.Index = Int32.Parse(sResult);
App.Frame.AddMessage("Previous percent " + rtb.Percent);
// Util.Say(rtb.RowText);
}; // Shown

this.Closing += delegate(object o, CancelEventArgs e) {
sFile = this.File;
if (!sFile.Contains(@"\")) return;
rtb = this.RTB;
int iIndex = rtb.Index;
if (iIndex == 0) return;

sText = App.ReadValue("Recent", sFile, "");
HomerList hl = new HomerList(sText);
hl.KeepLike(@"\d+");
hl.Remove("-1");
DateTime dt = DateTime.Now;
string sTime = dt.ToString("u");
sTime = sTime.Substring(0, sTime.Length - 1);
sText = sTime + "|" + iIndex + "|" + (rtb.ReadOnly ? "G" : "M") + "|" + (string) Util.If(rtb.WordWrap, "W", "U");
// hl.AddUniqueRange(sText);
// sText = hl.Segments;
App.WriteValue("Recent", sFile, sText);
}; // Closing

this.FileTime = System.IO.File.GetLastWriteTime(this.File);
this.Show();
} // child constructor

public void CheckFileTime(object sender, EventArgs e) {
if (App.Frame.KeyDescriber) {
App.Frame.SetMessage("No Key Describer");
App.Frame.KeyDescriber = false;
}

string sFile = this.File;
bool b = System.IO.File.Exists(App.IndentModeFile);
if (b && !this.RTB.IndentMode) System.IO.File.Delete(App.IndentModeFile);
else if (!b && this.RTB.IndentMode) System.IO.File.Create(App.IndentModeFile).Close();
if (this.FileTimeChecked || sFile.IndexOf(@"\") == -1 || !System.IO.File.Exists(sFile)) return;

DateTime dt = System.IO.File.GetLastWriteTime(sFile);
//if (this.FileTime >= dt || Util.File2String(sFile).Length == 0) return;
if (this.FileTime >= dt) return;
this.FileTimeChecked = true;
switch (Dialog.Confirm("Confirm", this.Text + " on disk is newer than the version opened in this window.  Open Again?", "Y")) {
case "Y" :
int iIndex = this.RTB.Index;
this.LoadTextOrRtfFile(sFile);
this.RTB.Index = iIndex;
break;
case "N":
break;
default :
this.FileTimeChecked = false;
return;
}
} // CheckFileTime handler

protected override void WndProc(ref Message m) {
base.WndProc(ref m);

const int WM_CHANGECBCHAIN = 0x30D;
const int WM_DRAWCLIPBOARD = 0x308;
//if (m.Msg == 776) {
switch (m.Msg) {
case WM_DRAWCLIPBOARD :
if ((int) this.NextClipboardViewer > 0) Win32.SendMessage(this.NextClipboardViewer, m.Msg, (int) m.LParam, (int) m.WParam);

if (this.AppendFromClipboard == -1) {
this.AppendFromClipboard = 1;
}
else if (this.AppendFromClipboard == 1) {
string sClipboard = Clipboard.GetText();
//if (sClipboard == this.LastClipboardText && ((Environment.TickCount - this.LastTickCount) < 100)) sClipboard = "";
if (sClipboard == this.LastClipboardText) sClipboard = "";
if (sClipboard.Length > 0) {
this.LastTickCount = Environment.TickCount;
this.LastClipboardText = sClipboard;
Console.Beep();

HomerRichTextBox rtb = this.RTB;
string sText = rtb.Text;
sText = sText.TrimEnd(new char[] {'\n'});
int iLength = sText.Length;
if (iLength >0) sText += "\f\n";
//if (iLength > 0 && sText.Substring(iLength - 1) != "\n") sText += "\n";
sClipboard = sClipboard.TrimEnd(new char[] {'\n'});
sText += sClipboard;
rtb.Text = sText;
rtb.Index = rtb.Text.Length - 1;
} // sClipboard.Length
} // this.AppendFromClipboard
break;
case WM_CHANGECBCHAIN :
IntPtr hNextClipboardViewer = m.WParam;
if (this.NextClipboardViewer == hNextClipboardViewer) this.NextClipboardViewer = m.LParam;
else if ((int) this.NextClipboardViewer > 0) Win32.SendMessage(hNextClipboardViewer, m.Msg, (int) m.LParam, (int) m.WParam);
break;
} // switch msg
} // WndProc event handler

public Encoding GetYieldEncoding() {
Encoding en = null;
string sEncoding = App.ReadOption("YieldEncoding", "").Trim();
if (sEncoding.Replace("-", "").ToLower() == "utf8n") {
en = new UTF8Encoding();
this.IsUnixLineBreak = true;
}
else if (sEncoding.Length > 0 ) {
try {
if (Util.IsNumeric(sEncoding)) en = Encoding.GetEncoding(Int32.Parse(sEncoding));
else en = Encoding.GetEncoding(sEncoding);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
en = Encoding.Default;
}
}
return en;
} // GetYieldEncoding method

public void LoadTextOrRtfFile(string sFile) {
bool bLiteral = false;
LoadTextOrRtfFile(sFile, bLiteral);
} // LoadTextOrRtfFile method

public void LoadTextOrRtfFile(string sFile, bool bLiteral) {
this.FileTimeChecked = false;
this.FileTime = System.IO.File.GetLastWriteTime(sFile);
try {
if (!bLiteral && Path.GetExtension(sFile).ToLower() == ".rtf") this.RTB.LoadFile(sFile, RichTextBoxStreamType.RichText);
//else this.RTB.LoadFile(sFile, RichTextBoxStreamType.UnicodePlainText);
else {
Encoding en = GetYieldEncoding();
string sText = Util.File2String(sFile, ref en);
this.RTB.Text = sText;
// Dialog.Show(sText.Length, this.RTB.TextLength);
if (sText.Length > 1 && this.RTB.TextLength == 1) {
en = Encoding.Unicode;
this.RTB.Text = Util.File2String(sFile, ref en);
}
this.YieldEncoding = en;
}
//else this.RTB.Text = Util.OldFile2String(sFile);
//else this.RTB.Text = System.IO.File.ReadAllText(sFile, System.Text.Encoding.UTF8);
//else this.RTB.Text = System.IO.File.ReadAllText(sFile, System.Text.Encoding.Default);
//else this.RTB.Text = System.IO.File.ReadAllText(sFile, System.Text.Encoding.GetEncoding(1252));
this.RTB.Modified = false;
this.Text = Path.GetFileName(sFile);
this.File = sFile;
}
catch {
App.Frame.AddMessage("Cannot open file!  Opening temporary copy.");
if (System.IO.File.Exists(App.TempFile)) System.IO.File.Delete(App.TempFile);
System.IO.File.Copy(sFile, App.TempFile);
App.Frame.OpenOrActivateWindow(App.TempFile);
}
//Dialog.Show(this.File);
// Stop double bookmark at message
// App.Frame.ApplyFileOptions(sFile);
} // LoadTextFile method

public void SaveTextOrRtfFile(string sFile) {
if (System.IO.File.Exists(sFile)) {
string sKeepBackup = App.ReadOption("KeepBackup", "N").Trim().ToLower();
if (sKeepBackup == "y" || sKeepBackup == "yes") {
string sBak = sFile + ".bak";
if (System.IO.File.Exists(sBak)) System.IO.File.Delete(sBak);
System.IO.File.Copy(sFile, sBak);
}
}

if (Path.GetExtension(sFile).ToLower() == ".rtf") this.RTB.SaveFile(sFile, RichTextBoxStreamType.RichText);
//else if (Util.IsUnicode(this.RTB.Text)) Util.String2File(this.RTB.Text, sFile);
//else this.RTB.SaveFile(sFile, RichTextBoxStreamType.PlainText);
else {
Encoding en = this.YieldEncoding;
if (en == null) en = GetYieldEncoding();
string sText = this.RTB.Text;
if (!this.IsUnixLineBreak) sText = Util.Convert2WinLineBreak(sText);
Util.String2File(sText, sFile, ref en);
}
this.RTB.Modified = false;
App.Frame.SetRecent(sFile);
this.Text = Path.GetFileName(sFile);
this.File = sFile;
this.FileTime = System.IO.File.GetLastWriteTime(sFile);
this.FileTimeChecked = false;
} // SaveTextOrRtfFile method

protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
return App.Frame.ProcessCmdKey_Helper(ref msg, keyData);
} // ProcessCmdKey handler

} // MdiChild class

public class MdiFrame : Form {
public string LastDescription = "";
public bool KeyDescriber = false;
public string KeyString = "";
public int KeyRepeat = 0;
public int KeyIndex = -1;

public MdiChild Child {
get {
MdiChild child = this.ActiveMdiChild as MdiChild;
if (child == null) {
Form[] children = this.MdiChildren;
int iLength = children.Length;
if (children.Length > 0) child = (MdiChild) children[iLength - 1];
}
return child;
}
set {
value.Activate();
}
} // Child property

public bool FindWithRegExp = false;
public bool bCommandComplete = true;
public static string CR = "\r";
public static string LF = "\n";
public static string LB = LF;
public static string LineBreak = Environment.NewLine;
public static string FF = "\f";
public static string SB = FF + LB;
public static string DD = "----------";
public static string SectionBreak = LB + DD + LB + SB;
public static string EOD = LB + DD + LB + "End of Document" + LB;

public static Dictionary<Keys, ToolStripMenuItem> hashKey = new Dictionary<Keys, ToolStripMenuItem>();
public MenuStrip menuMain;
public ToolStripMenuItem menuFile, menuFileNew, menuFileNewFromClipboard, menuFileOpen, menuFileOpenOtherFormat, menuFileOpenAgain, menuFileRecent, menuFileSetFavorite, menuFileClearFavorite, menuFileListFavorites, menuFileFind, menuFileSave, menuFileSaveAs, menuFileSaveCopy, menuFileExport, menuFileRename, menuFileProperties, menuFileMailBody, menuFileMailAttach, menuFilePrint, menuFileRun, menuFileCurrentWindows, menuFileClose, menuFileCloseAllButCurrentWindow, menuFileExit;
public ToolStripMenuItem menuEdit, menuEditSelectAll, menuEditUnselectAll, menuEditCopy, menuEditCopyAppend, menuEditCopyRichText, menuEditCut, menuEditCutAppend, menuEditPaste, menuEditPasteFile, menuEditUndo, menuEditRedo, menuEditStartSelection, menuEditCompleteSelection, menuEditReselect, menuEditCopyAll, menuEditSelectChunk, menuEditAppendFromClipboard, menuEditQuote, menuEditUnquote, menuEditUpperCase, menuEditLowerCase, menuEditProperCase, menuEditSwapCase, menuEditYieldEncoding, menuEditJoinLines, menuEditHardLineBreak, menuEditEnterNewLine, menuEditIndentNewLine, menuEditIndentNewLinePrior, menuEditIndent, menuEditOutdent, menuEditAlign, menuEditIndentMode, menuEditJustify, menuEditStyle, menuEditBaseline, menuEditSetSelectionFont;
public ToolStripMenuItem menuDelete, menuDeleteReplaceRegular, menuDeleteReplaceWithRegExp, menuDeleteHardLine, menuDeleteParagraph, menuDeleteLine, menuDeleteRight, menuDeleteLeft, menuDeleteDown, menuDeleteUp, menuDeleteFile, menuDeleteTrimBlanks;
public ToolStripMenuItem menuNavigate, menuNavigateForwardFind, menuNavigateReverseFind, menuNavigateForwardFindWithRegExp, menuNavigateReverseFindWithRegExp,  menuNavigateForwardFindAtCursor, menuNavigateReverseFindAtCursor, menuNavigateForwardFindAgain, menuNavigateReverseFindAgain, menuNavigateJumpToLine, menuNavigateJumpToLineAgain, menuNavigateGoToPercent, menuNavigateGoToPercentAgain, menuNavigateGoToPart, menuNavigateSetBookmark, menuNavigateClearBookmark, menuNavigateGoToBookmark, menuNavigateHomeCharacter, menuNavigateEndCharacter, menuNavigateStartTag, menuNavigateEndTag,  menuNavigateNextJustify, menuNavigatePriorJustify, menuNavigateNextStyle, menuNavigatePriorStyle, menuNavigateNextBaseline, menuNavigatePriorBaseline, menuNavigateNextFont, menuNavigatePriorFont, menuNavigateRightBrace, menuNavigateNextBlock, menuNavigatePriorBlock, menuNavigateLeftBrace, menuNavigateNextIndent, menuNavigatePriorIndent, menuNavigateNextChunk,  menuNavigatePriorChunk, menuNavigateNextSentence, menuNavigatePriorSentence, menuNavigateNextParagraph, menuNavigatePriorParagraph, menuNavigateNextPart, menuNavigatePriorPart, menuNavigateNextSection, menuNavigatePriorSection, menuNavigateGoToSection, menuNavigateGoToContents, menuNavigateSearchForTopic, menuNavigateSearchForTopicAgain, menuNavigateGoToStartOfSelection;
public ToolStripMenuItem menuQuery, menuQueryAddress, menuQueryBraces, menuQueryBlock, menuQueryIndent, menuQueryPath, menuQueryTopic, menuQueryYield, menuQueryStatus, menuQueryCompiler, menuQuerySelected, menuQueryChunk, menuQueryReadAll, menuQueryWindowsOpen, menuQueryClipboard, menuQueryTime, menuQueryStyles, menuQueryFont;
public ToolStripMenuItem menuMisc, menuMiscSetDefaultFont, menuMiscConfigurationOptions, menuMiscManualOptions, menuMiscResetConfiguration, menuMiscGoToFolder, menuMiscGoToSpecialFolder, menuMiscWordWrap, menuMiscUnwrap, menuMiscExtraSpeechToggle, menuMiscExtraSpeechLog, menuMiscEnvironmentVariables, menuMiscSpellCheck, menuMiscThesaurus, menuMiscLookupTerm, menuMiscTranslateLanguage, menuMiscGuardDocument, menuMiscNoGuard, menuMiscPyBrace, menuMiscPyDent, menuMiscInferIndent, menuMiscFormatCode, menuMiscRepeatLine, menuMiscSectionBreak, menuMiscPathToClipboard, menuMiscPathList, menuMiscInsertTime, menuMiscCalculateDate, menuMiscHTMLFormat, menuMiscTextConvert, menuMiscTextCombine, menuMiscTextContents, menuMiscYieldWithRegExp, menuMiscExtractWithRegExp, menuMiscRunAtCursor, menuMiscSpecialCharacter, menuMiscEvaluateExpression, menuMiscReplaceTokens, menuMiscTransformFiles, menuMiscGoToEnvironment, menuMiscCompile, menuMiscPickCompiler, menuMiscPromptCommand, menuMiscReviewOutput, menuMiscSaveSnippet, menuMiscInvokeSnippet, menuMiscViewSnippet, menuMiscKeepUniqueItems, menuMiscNumberItems, menuMiscOrderItems, menuMiscReverseItems, menuMiscListDifferentItems, menuMiscQueryCommonItems, menuMiscExplorerFolder, menuMiscCommandPrompt, menuMiscBurnToCD, menuMiscWebDownload, menuMiscWebClientUtilities;
public ToolStripMenuItem menuWindow, menuWindowNext, menuWindowPrior, menuWindowArrangeIcons, menuWindowCascade, menuWindowTileHorizontal, menuWindowTileVertical;
public ToolStripMenuItem menuHelp, menuHelpAbout, menuHelpDocumentation, menuHelpHistoryOfChanges, menuHelpKeyDescriber, menuHelpHotKeySummary, menuHelpAlternateMenu, menuHelpContextMenu, menuHelpSendToMenu, menuHelpElevateVersion;
public StatusStrip statusBar;
public ToolStripStatusLabel lblStatus;

public MdiFrame() {
SectionBreak = Util.Literalize(App.ReadOption("SectionBreak", SectionBreak));
this.SuspendLayout();
this.IsMdiContainer = true;
menuMain = CreateMainMenu();
//menuMain.ShowItemToolTips = true;
menuMain.AccessibleRole = AccessibleRole.MenuBar;
menuFile = CreateMenu("&File");
menuFileNew = CreateMenuItem("&New", "Control+N", menuItem_Click, "frame speak");
menuFileNewFromClipboard = CreateMenuItem("New from Clipboard", "Control+Shift+N", menuItem_Click, "frame speak");
menuFileOpen = CreateMenuItem("&Open ...", "Control+O", menuItem_Click, "frame speak");
menuFileOpenOtherFormat = CreateMenuItem("Open Other Format ...", "Control+Shift+O", menuItem_Click, "frame speak");
menuFileOpenAgain = CreateMenuItem("Open Again", "Alt+O", menuItem_Click, "child speak");
menuFileRecent = CreateMenuItem("Recent Files ...", "Alt+R", menuItem_Click, "frame silent");
menuFileSetFavorite = CreateMenuItem("Set Favorite", "Control+&L", menuItem_Click, "child speak");
//menuFileSetFavorite = CreateMenuItem("Set on Favorite &List", "Control+L", menuItem_Click, "child speak");
menuFileClearFavorite = CreateMenuItem("Clear Favorite", "Control+Shift+L", menuItem_Click, "child speak");
menuFileListFavorites = CreateMenuItem("List Favorites ...", "Alt+L", menuItem_Click, "frame silent");
menuFileFind = CreateMenuItem("File Find ...", "Alt+Shift+F", menuItem_Click, "frame speak");
menuFileSave = CreateMenuItem("&Save", "Control+S", menuItem_Click, "child speak");
menuFileSaveAs = CreateMenuItem("Save &As ...", "Control+Shift+S", menuItem_Click, "child silent");
menuFileSaveCopy = CreateMenuItem("Save Copy ...", "Alt+Shift+S", menuItem_Click, "child speak");
menuFileExport = CreateMenuItem("Export Format ...", "Alt+Shift+E", menuItem_Click, "child silent");
menuFileRename = CreateMenuItem("Rename ...", "Alt+Shift+R", menuItem_Click, "child silent");
menuFileProperties = CreateMenuItem("Properties", "Alt+Enter", menuItem_Click, "child speak");
menuFileMailBody = CreateMenuItem("&Mail Body ...", "Control+M", menuItem_Click, "child speak");
menuFileMailAttach = CreateMenuItem("Mail Attachment ...", "Control+Shift+M", menuItem_Click, "child speak");
menuFilePrint = CreateMenuItem("&Print", "Control+P", menuItem_Click, "child silent");
menuFileRun = CreateMenuItem("Run", "F5", menuItem_Click, "child speak");
menuFileCurrentWindows = CreateMenuItem("Current Windows ...", "F4", menuItem_Click, "frame silent");
menuFileClose = CreateMenuItem("&Close Window", "Control+F4", menuItem_Click, "child speak");
menuFileCloseAllButCurrentWindow = CreateMenuItem("Close All but Current Window", "Control+Shift+F4", menuItem_Click, "child speak");
menuFileExit = CreateMenuItem("&E&xit EdSharp", "Alt+F4", menuItem_Click, "frame speak");
menuFile.DropDownItems.AddRange(new ToolStripItem[] {menuFileNew, menuFileNewFromClipboard, menuFileOpen, menuFileOpenOtherFormat, menuFileOpenAgain, menuFileRecent, menuFileSetFavorite, menuFileClearFavorite, menuFileListFavorites, menuFileFind, menuFileSave, menuFileSaveAs, menuFileSaveCopy, menuFileExport, menuFileRename, menuFileProperties, menuFileMailBody, menuFileMailAttach, menuFilePrint, menuFileRun, menuFileCurrentWindows, menuFileClose, menuFileCloseAllButCurrentWindow, menuFileExit});
//Dialog.Show("File.", menuFile.DropDownItems.Count);

menuEdit = CreateMenu("&Edit");
menuEditSelectAll = CreateMenuItem("Select &All", "Control+A", menuItem_Click, "child speak");
menuEditUnselectAll = CreateMenuItem("Unselect All", "Control+Shift+A", menuItem_Click, "child speak");
menuEditCopy = CreateMenuItem("&Copy", "Control+C", menuItem_Click, "child speak");
menuEditCopyAppend = CreateMenuItem("Copy Append", "Alt+C", menuItem_Click, "child speak");
menuEditCopyRichText = CreateMenuItem("Copy Rich Text", "Control+Shift+C", menuItem_Click, "child speak");
menuEditCut = CreateMenuItem("Cut", "Control+&X", menuItem_Click, "child speak");
menuEditCutAppend = CreateMenuItem("Cut Append", "Alt+X", menuItem_Click, "child speak");
menuEditPaste = CreateMenuItem("Paste", "Control+&V", menuItem_Click, "child speak");
menuEditPasteFile = CreateMenuItem("Paste File ...", "Control+Shift+V", menuItem_Click, "child speak");
menuEditUndo = CreateMenuItem("Undo", "Control+&Z", menuItem_Click, "child speak");
menuEditRedo = CreateMenuItem("Redo", "Control+Shift+Z", menuItem_Click, "child speak");
menuEditStartSelection = CreateMenuItem("Start Selection", "F8", menuItem_Click, "child speak");
menuEditCompleteSelection = CreateMenuItem("Complete Selection", "Shift+F8", menuItem_Click, "child speak");
menuEditReselect = CreateMenuItem("Reselect", "Control+Shift+F8", menuItem_Click, "child speak");
menuEditCopyAll = CreateMenuItem("Copy All", "Control+F8", menuItem_Click, "child speak");
menuEditSelectChunk = CreateMenuItem("Select Chunk", "Control+Space", menuItem_Click, "child silent");
menuEditAppendFromClipboard = CreateMenuItem("Append from Clipboard", "Alt+D7", menuItem_Click, "child silent");
menuEditQuote = CreateMenuItem("&Quote", "Control+Q", menuItem_Click, "child speak");
menuEditUnquote = CreateMenuItem("Unquote", "Control+Shift+Q", menuItem_Click, "child speak");
menuEditUpperCase = CreateMenuItem("&Upper Case", "Control+U", menuItem_Click, "child speak");
menuEditLowerCase = CreateMenuItem("Lower Case", "Control+Shift+U", menuItem_Click, "child speak");
menuEditProperCase = CreateMenuItem("Proper Case", "Alt+U", menuItem_Click, "child speak");
menuEditSwapCase = CreateMenuItem("Swap Case", "Alt+Shift+U", menuItem_Click, "child speak");
menuEditYieldEncoding = CreateMenuItem("Yield Encoding", "Alt+Shift+Y", menuItem_Click, "child silent");
menuEditJoinLines = CreateMenuItem("Join Lines", "Control+Shift+J", menuItem_Click, "child speak");
menuEditHardLineBreak = CreateMenuItem("Hard Line Break ...", "Control+Shift+H", menuItem_Click, "child silent");
menuEditEnterNewLine = CreateMenuItem("Enter New Line", "Enter", menuItem_Click, "child silent");
menuEditIndentNewLine = CreateMenuItem("Indent New Line", "Shift+Enter", menuItem_Click, "child silent");
menuEditIndentNewLinePrior = CreateMenuItem("Indent New Line Prior", "Alt+Shift+Enter", menuItem_Click, "child speak");
menuEditIndent = CreateMenuItem("Indent", "Tab", menuItem_Click, "child silent");
menuEditOutdent = CreateMenuItem("Outdent", "Shift+Tab", menuItem_Click, "child silent");
menuEditAlign = CreateMenuItem("Align", "Alt+Shift+A", menuItem_Click, "child speak");
menuEditIndentMode = CreateMenuItem("Indent Mode", "Alt+Shift+I", menuItem_Click, "child speak");
menuEditJustify = CreateMenuItem("Justify ...", "Alt+Shift+J", menuItem_Click, "child silent");
menuEditStyle = CreateMenuItem("Style ...", "Alt+Shift+OemQuestion", menuItem_Click, "child silent");
menuEditBaseline = CreateMenuItem("Baseline ...", "Alt+Shift+D6", menuItem_Click, "child silent");
menuEditSetSelectionFont = CreateMenuItem("Set Selection Font ...", "Alt+Shift+OemMinus", menuItem_Click, "child speak");
menuEdit.DropDownItems.AddRange(new ToolStripItem[] {menuEditSelectAll, menuEditUnselectAll, menuEditCopy, menuEditCopyAppend, menuEditCopyRichText, menuEditCut, menuEditCutAppend, menuEditPaste, menuEditPasteFile, menuEditUndo, menuEditRedo, menuEditStartSelection, menuEditCompleteSelection, menuEditReselect, menuEditCopyAll, menuEditSelectChunk, menuEditAppendFromClipboard, menuEditQuote, menuEditUnquote, menuEditUpperCase, menuEditLowerCase, menuEditProperCase, menuEditSwapCase, menuEditYieldEncoding, menuEditJoinLines, menuEditHardLineBreak, menuEditEnterNewLine, menuEditIndentNewLine, menuEditIndentNewLinePrior, menuEditIndent, menuEditOutdent, menuEditAlign, menuEditIndentMode, menuEditJustify, menuEditStyle, menuEditBaseline, menuEditSetSelectionFont});
//Dialog.Show("Edit.", menuEdit.DropDownItems.Count);

menuDelete = CreateMenu("&Delete");
menuDeleteReplaceRegular = CreateMenuItem("&Replace ...", "Control+R", menuItem_Click, "child silent");
menuDeleteReplaceWithRegExp = CreateMenuItem("Replace with Regular Expression ...", "Control+Shift+R", menuItem_Click, "child silent");
menuDeleteHardLine = CreateMenuItem("Delete Hard Line", "Control+D", menuItem_Click, "child silent");
menuDeleteParagraph = CreateMenuItem("Delete Paragraph", "Control+Shift+D", menuItem_Click, "child silent");
menuDeleteLine = CreateMenuItem("Delete Line", "Alt+Back", menuItem_Click, "child silent");
menuDeleteRight = CreateMenuItem("Delete Right", "Control+Shift+Delete", menuItem_Click, "child silent");
menuDeleteLeft = CreateMenuItem("Delete Left", "Control+Shift+Back", menuItem_Click, "child silent");
menuDeleteDown = CreateMenuItem("Delete Down", "Alt+Shift+Delete", menuItem_Click, "child speak");
menuDeleteUp = CreateMenuItem("Delete Up", "Alt+Shift+Back", menuItem_Click, "child speak");
menuDeleteFile = CreateMenuItem("Delete File", "Alt+Shift+D", menuItem_Click, "child speak");
menuDeleteTrimBlanks = CreateMenuItem("Trim Blanks", "Control+Shift+Enter", menuItem_Click, "child speak");
menuDelete.DropDownItems.AddRange(new ToolStripMenuItem[] {menuDeleteReplaceRegular, menuDeleteReplaceWithRegExp, menuDeleteHardLine, menuDeleteParagraph, menuDeleteLine, menuDeleteRight, menuDeleteLeft, menuDeleteDown, menuDeleteUp, menuDeleteFile, menuDeleteTrimBlanks});
//Dialog.Show("Delete.", menuDelete.DropDownItems.Count);

menuNavigate = CreateMenu("&Navigate");
menuNavigateForwardFind = CreateMenuItem("Forward &Find ...", "Control+F", menuItem_Click, "child silent");
menuNavigateReverseFind = CreateMenuItem("Reverse Find ...", "Control+Shift+F", menuItem_Click, "child silent");
menuNavigateForwardFindWithRegExp = CreateMenuItem("Forward Find with Regular Expression ...", "Control+F3", menuItem_Click, "child silent");
menuNavigateReverseFindWithRegExp = CreateMenuItem("Reverse Find with Regular Expression ...", "Control+Shift+F3", menuItem_Click, "child silent");
menuNavigateForwardFindAtCursor = CreateMenuItem("Forward Find at Cursor", "Alt+F3", menuItem_Click, "child silent");
menuNavigateReverseFindAtCursor = CreateMenuItem("Reverse Find at Cursor", "Alt+Shift+F3", menuItem_Click, "child silent");
menuNavigateForwardFindAgain = CreateMenuItem("Forward Find Again", "F3", menuItem_Click, "child silent");
menuNavigateReverseFindAgain = CreateMenuItem("Reverse Find Again", "Shift+F3", menuItem_Click, "child silent");
menuNavigateJumpToLine = CreateMenuItem("&Jump to Line ...", "Control+J", menuItem_Click, "child silent");
menuNavigateJumpToLineAgain = CreateMenuItem("Jump to Line Again", "Alt+J", menuItem_Click, "child silent");
menuNavigateGoToPercent = CreateMenuItem("&Go to Percent ...", "Control+G", menuItem_Click, "child silent");
menuNavigateGoToPercentAgain = CreateMenuItem("Go to Percent Again", "Alt+G", menuItem_Click, "child silent");
menuNavigateGoToPart = CreateMenuItem("Go to Part", "Alt+Shift+G", menuItem_Click, "child silent");
menuNavigateSetBookmark = CreateMenuItem("Set Bookmar&k", "Control+K", menuItem_Click, "child speak");
menuNavigateClearBookmark = CreateMenuItem("Clear Bookmark", "Control+Shift+K", menuItem_Click, "child speak");
menuNavigateGoToBookmark = CreateMenuItem("Go to Bookmark", "Alt+K", menuItem_Click, "child speak");
menuNavigateHomeCharacter = CreateMenuItem("Home Character", "Alt+Home", menuItem_Click, "child silent");
menuNavigateEndCharacter = CreateMenuItem("End Character", "Alt+End", menuItem_Click, "child silent");
menuNavigateStartTag = CreateMenuItem("Start Tag", "Control+Shift+Oemcomma", menuItem_Click, "child silent");
menuNavigateEndTag = CreateMenuItem("End Tag", "Control+Shift+OemPeriod", menuItem_Click, "child silent");
menuNavigateNextJustify = CreateMenuItem("Next Alignment", "Control+OemCloseBrackets", menuItem_Click, "child silent");
menuNavigatePriorJustify = CreateMenuItem("Prior Alignment", "Control+OemOpenBrackets", menuItem_Click, "child silent");
menuNavigateNextStyle = CreateMenuItem("Next Style", "Control+OemQuestion", menuItem_Click, "child silent");
menuNavigatePriorStyle = CreateMenuItem("Prior Style", "Control+Shift+OemQuestion", menuItem_Click, "child silent");
menuNavigateNextBaseline = CreateMenuItem("Next Baseline", "Control+D6", menuItem_Click, "child silent");
menuNavigatePriorBaseline = CreateMenuItem("Prior Baseline", "Control+Shift+D6", menuItem_Click, "child silent");
menuNavigateNextFont = CreateMenuItem("Next Font", "Control+OemMinus", menuItem_Click, "child silent");
menuNavigatePriorFont = CreateMenuItem("Prior Font", "Control+Shift+OemMinus", menuItem_Click, "child silent");
menuNavigateRightBrace = CreateMenuItem("Right Brace", "Control+Shift+OemCloseBrackets", menuItem_Click, "child silent");
menuNavigateLeftBrace = CreateMenuItem("Left Brace", "Control+Shift+OemOpenBrackets", menuItem_Click, "child silent");
menuNavigateNextBlock = CreateMenuItem("Next Block", "Control+B", menuItem_Click, "child silent");
menuNavigatePriorBlock = CreateMenuItem("Prior Block", "Control+Shift+B", menuItem_Click, "child silent");
menuNavigateNextIndent = CreateMenuItem("Next Indent", "Control+I", menuItem_Click, "child silent");
menuNavigatePriorIndent = CreateMenuItem("Prior Indent", "Control+Shift+I", menuItem_Click, "child silent");
menuNavigateNextChunk = CreateMenuItem("Next Chunk", "Alt+Right", menuItem_Click, "child silent");
menuNavigatePriorChunk = CreateMenuItem("Prior Chunk", "Alt+Left", menuItem_Click, "child silent");
menuNavigateNextSentence = CreateMenuItem("Next Sentence", "Alt+Down", menuItem_Click, "child silent");
menuNavigatePriorSentence = CreateMenuItem("Prior Sentence", "Alt+Up", menuItem_Click, "child silent");
menuNavigateNextParagraph = CreateMenuItem("Next Paragraph", "Control+Down", menuItem_Click, "child silent");
menuNavigatePriorParagraph = CreateMenuItem("Prior Paragraph", "Control+Up", menuItem_Click, "child silent");
menuNavigateNextPart= CreateMenuItem("Next Part", "Alt+PageDown", menuItem_Click, "child silent");
menuNavigatePriorPart= CreateMenuItem("Prior Part", "Alt+PageUp", menuItem_Click, "child silent");
menuNavigateNextSection= CreateMenuItem("Next Section", "Control+PageDown", menuItem_Click, "child silent");
menuNavigatePriorSection= CreateMenuItem("Prior Section", "Control+PageUp", menuItem_Click, "child silent");
menuNavigateGoToSection= CreateMenuItem("Go to Section", "F6", menuItem_Click, "child speak");
menuNavigateGoToContents = CreateMenuItem("Go to Contents", "Shift+F6", menuItem_Click, "child speak");
menuNavigateSearchForTopic = CreateMenuItem("Search for Topic ...", "Control+F6", menuItem_Click, "child silent");
menuNavigateSearchForTopicAgain = CreateMenuItem("Search for Topic Again", "Alt+F6", menuItem_Click, "child silent");
menuNavigateGoToStartOfSelection = CreateMenuItem("Go to Start of Selection", "Alt+Shift+F8", menuItem_Click, "child speak");
menuNavigate.DropDownItems.AddRange(new ToolStripItem[] {menuNavigateForwardFind, menuNavigateReverseFind, menuNavigateForwardFindWithRegExp, menuNavigateReverseFindWithRegExp,  menuNavigateForwardFindAtCursor, menuNavigateReverseFindAtCursor, menuNavigateForwardFindAgain, menuNavigateReverseFindAgain, menuNavigateJumpToLine, menuNavigateJumpToLineAgain, menuNavigateGoToPercent, menuNavigateGoToPercentAgain, menuNavigateGoToPart, menuNavigateSetBookmark, menuNavigateClearBookmark, menuNavigateGoToBookmark, menuNavigateHomeCharacter, menuNavigateEndCharacter, menuNavigateStartTag, menuNavigateEndTag,  menuNavigateNextJustify, menuNavigatePriorJustify, menuNavigateNextStyle, menuNavigatePriorStyle, menuNavigateNextBaseline, menuNavigatePriorBaseline, menuNavigateNextFont, menuNavigatePriorFont, menuNavigateRightBrace, menuNavigateNextBlock, menuNavigatePriorBlock, menuNavigateLeftBrace, menuNavigateNextIndent, menuNavigatePriorIndent, menuNavigateNextChunk,  menuNavigatePriorChunk, menuNavigateNextSentence, menuNavigatePriorSentence, menuNavigateNextParagraph, menuNavigatePriorParagraph, menuNavigateNextPart, menuNavigatePriorPart, menuNavigateNextSection, menuNavigatePriorSection, menuNavigateGoToSection, menuNavigateGoToContents, menuNavigateSearchForTopic, menuNavigateSearchForTopicAgain, menuNavigateGoToStartOfSelection});
//Dialog.Show("Navigate.", menuNavigate.DropDownItems.Count);

menuQuery = CreateMenu("&Query");
menuQueryAddress = CreateMenuItem("Address", "Alt+A", menuItem_Click, "child silent");
menuQueryBraces = CreateMenuItem("Braces", "Alt+Shift+OemCloseBrackets", menuItem_Click, "child silent");
menuQueryBlock = CreateMenuItem("Block", "Alt+B", menuItem_Click, "child silent");
menuQueryIndent = CreateMenuItem("Indentation", "Alt+I", menuItem_Click, "child silent");
menuQueryPath = CreateMenuItem("Path", "Alt+P", menuItem_Click, "child silent");
menuQueryTopic = CreateMenuItem("Topic", "Alt+T", menuItem_Click, "child speak");
menuQueryYield = CreateMenuItem("Yield", "Alt+Y", menuItem_Click, "child speak");
menuQueryStatus = CreateMenuItem("Status", "Alt+Z", menuItem_Click, "child silent");
menuQueryCompiler = CreateMenuItem("Compiler", "Alt+D0", menuItem_Click, "frame silent");
menuQuerySelected = CreateMenuItem("Selected", "Shift+Space", menuItem_Click, "child silent");
menuQueryChunk = CreateMenuItem("Chunk", "Shift+Back", menuItem_Click, "child silent");
menuQueryReadAll = CreateMenuItem("Read All", "Alt+F8", menuItem_Click, "child speak");
menuQueryWindowsOpen = CreateMenuItem("Windows Open", "Shift+F4", menuItem_Click, "child speak");
menuQueryClipboard = CreateMenuItem("Clipboard", "Alt+OemQuotes", menuItem_Click, "frame silent");
menuQueryTime = CreateMenuItem("Time", "Alt+OemSemicolon", menuItem_Click, "frame silent");
menuQueryStyles = CreateMenuItem("Styles", "Alt+OemQuestion", menuItem_Click, "child silent");
menuQueryFont = CreateMenuItem("Font", "Alt+OemMinus", menuItem_Click, "child silent");
menuQuery.DropDownItems.AddRange(new ToolStripItem[] {menuQueryAddress, menuQueryBraces, menuQueryBlock, menuQueryIndent, menuQueryPath, menuQueryTopic, menuQueryYield, menuQueryStatus, menuQueryCompiler, menuQuerySelected, menuQueryChunk, menuQueryReadAll, menuQueryWindowsOpen, menuQueryClipboard, menuQueryTime, menuQueryStyles, menuQueryFont});
//Dialog.Show("Query.", menuQuery.DropDownItems.Count);

menuMisc = CreateMenu("&Misc");
menuMiscSetDefaultFont = CreateMenuItem("Set Default Font and Color ...", "Alt+Shift+Oemplus", menuItem_Click, "child speak");
menuMiscConfigurationOptions = CreateMenuItem("Configuration Options ...", "Alt+Shift+C", menuItem_Click, "frame silent");
menuMiscManualOptions = CreateMenuItem("Manual Options", "Alt+Shift+M", menuItem_Click, "frame silent");
menuMiscResetConfiguration = CreateMenuItem("Reset Configuration", "Alt+Shift+D0", menuItem_Click, "frame silent");
menuMiscGoToFolder = CreateMenuItem("Go to Folder", "Control+D0", menuItem_Click, "frame silent");
menuMiscGoToSpecialFolder = CreateMenuItem("Go to Special Folder", "Control+Shift+D0", menuItem_Click, "frame silent");
menuMiscWordWrap = CreateMenuItem("&Word Wrap", "Control+W", menuItem_Click, "child speak");
menuMiscUnwrap = CreateMenuItem("Unwrap", "Control+Shift+W", menuItem_Click, "child speak");
menuMiscExtraSpeechToggle = CreateMenuItem("Extra Speech Toggle", "Control+Shift+X", menuItem_Click, "frame silent");
menuMiscExtraSpeechLog = CreateMenuItem("Extra Speech Log", "Alt+Shift+X", menuItem_Click, "frame speak");
menuMiscEnvironmentVariables = CreateMenuItem("&Environment Variables ...", "Control+E", menuItem_Click, "frame speak");
menuMiscSpellCheck = CreateMenuItem("Spell Check", "F7", menuItem_Click, "child speak");
menuMiscThesaurus = CreateMenuItem("Thesaurus", "Shift+F7", menuItem_Click, "child speak");
menuMiscLookupTerm = CreateMenuItem("Lookup Term", "Alt+F7", menuItem_Click, "frame silent");
menuMiscTranslateLanguage = CreateMenuItem("Translate Language", "Alt+Shift+F7", menuItem_Click, "frame speak");
menuMiscGuardDocument = CreateMenuItem("Guard Document", "Control+F7", menuItem_Click, "child speak");
menuMiscNoGuard = CreateMenuItem("No Guard", "Control+Shift+F7", menuItem_Click, "child speak");
menuMiscPyBrace = CreateMenuItem("PyBrace", "Alt+Shift+OemOpenBrackets", menuItem_Click, "child speak");
menuMiscPyDent = CreateMenuItem("PyDent", "Alt+OemOpenBrackets", menuItem_Click, "child speak");
menuMiscInferIndent = CreateMenuItem("Infer Indent", "Alt+OemCloseBrackets", menuItem_Click, "child silent");
menuMiscFormatCode = CreateMenuItem("Format Code", "Control+D4", menuItem_Click, "child speak");
menuMiscRepeatLine = CreateMenuItem("Repeat Line", "Control+Y", menuItem_Click, "child speak");
menuMiscSectionBreak = CreateMenuItem("Section Break", "Control+Enter", menuItem_Click, "child speak");
menuMiscPathToClipboard = CreateMenuItem("Path to Clipboard", "Alt+Shift+P", menuItem_Click, "child speak");
menuMiscPathList = CreateMenuItem("Path List", "Control+Shift+P", menuItem_Click, "frame speak");
menuMiscInsertTime = CreateMenuItem("Insert Time", "Alt+Shift+OemSemicolon", menuItem_Click, "child speak");
menuMiscCalculateDate = CreateMenuItem("Calculate Date ...", "Control+Shift+OemSemicolon", menuItem_Click, "child silent");
menuMiscHTMLFormat = CreateMenuItem("HTML Format", "Control+H", menuItem_Click, "child speak");
menuMiscTextConvert = CreateMenuItem("&Text Convert", "Control+T", menuItem_Click, "child speak");
menuMiscTextCombine = CreateMenuItem("Text Combine", "Control+Shift+T", menuItem_Click, "child speak");
menuMiscTextContents = CreateMenuItem("Text Contents", "Alt+Shift+T", menuItem_Click, "child speak");
menuMiscYieldWithRegExp = CreateMenuItem("Yield with Regular Expression ...", "Control+Shift+Y", menuItem_Click, "child silent");
//menuMiscYieldWithRegExp.ShortcutKeys = Util.String2Key("Control+Shift+Y");
menuMiscExtractWithRegExp = CreateMenuItem("Extract with Regular Expression ...", "Control+Shift+E", menuItem_Click, "child silent");
menuMiscRunAtCursor = CreateMenuItem("Run at Cursor ...", "Shift+F5", menuItem_Click, "child silent");
menuMiscSpecialCharacter = CreateMenuItem("Special Character ...", "F2", menuItem_Click, "child silent");
menuMiscEvaluateExpression = CreateMenuItem("Evaluate Expression", "Control+Oemplus", menuItem_Click, "child speak");
menuMiscReplaceTokens = CreateMenuItem("Replace Tokens", "Control+Shift+Oemplus", menuItem_Click, "child silent");
menuMiscTransformFiles = CreateMenuItem("Transform Files", "Alt+Oemplus", menuItem_Click, "child speak");
menuMiscGoToEnvironment = CreateMenuItem("Go to Environment", "Control+Shift+G", menuItem_Click, "frame speak");
menuMiscCompile = CreateMenuItem("Compile", "Control+F5", menuItem_Click, "child speak");
menuMiscPickCompiler = CreateMenuItem("Pick Compiler", "Control+Shift+F5", menuItem_Click, "frame silent");
menuMiscPromptCommand = CreateMenuItem("Prompt Command", "Alt+F5", menuItem_Click, "child silent");
menuMiscReviewOutput = CreateMenuItem("Review Output", "Alt+Shift+F5", menuItem_Click, "child speak");
menuMiscSaveSnippet = CreateMenuItem("Save Snippet", "Alt+S", menuItem_Click, "child speak");
menuMiscInvokeSnippet = CreateMenuItem("Invoke Snippet", "Alt+V", menuItem_Click, "child speak");
menuMiscViewSnippet = CreateMenuItem("View Snippet", "Alt+Shift+V", menuItem_Click, "frame speak");
menuMiscKeepUniqueItems = CreateMenuItem("Keep Unique Items", "Alt+Shift+K", menuItem_Click, "child speak");
menuMiscNumberItems = CreateMenuItem("Number Items ...", "Alt+Shift+N", menuItem_Click, "child silent");
menuMiscOrderItems = CreateMenuItem("Order Items", "Alt+Shift+O", menuItem_Click, "child speak");
menuMiscReverseItems = CreateMenuItem("Reverse Items", "Alt+Shift+Z", menuItem_Click, "child speak");
menuMiscListDifferentItems = CreateMenuItem("List Different Items", "Alt+Shift+L", menuItem_Click, "child speak");
menuMiscQueryCommonItems = CreateMenuItem("Query Common Items", "Alt+Shift+Q", menuItem_Click, "child speak");
menuMiscExplorerFolder = CreateMenuItem("Explorer Folder", "Alt+Oem5", menuItem_Click, "frame speak");
menuMiscCommandPrompt = CreateMenuItem("Command Prompt", "Control+Oem5", menuItem_Click, "frame speak");
menuMiscBurnToCD = CreateMenuItem("Burn to CD", "Alt+Shift+B", menuItem_Click, "child speak");
menuMiscWebDownload = CreateMenuItem("Web Download", "Alt+Shift+W", menuItem_Click, "frame speak");
menuMiscWebClientUtilities = CreateMenuItem("Web Client Utilities", "Alt+Shift+Space", menuItem_Click, "frame speak");
menuMisc.DropDownItems.AddRange(new ToolStripItem[] {menuMiscSetDefaultFont, menuMiscConfigurationOptions, menuMiscManualOptions, menuMiscResetConfiguration, menuMiscGoToFolder, menuMiscGoToSpecialFolder, menuMiscWordWrap, menuMiscUnwrap, menuMiscExtraSpeechToggle, menuMiscExtraSpeechLog, menuMiscEnvironmentVariables, menuMiscSpellCheck, menuMiscThesaurus, menuMiscLookupTerm, menuMiscTranslateLanguage, menuMiscGuardDocument, menuMiscNoGuard, menuMiscPyBrace, menuMiscPyDent, menuMiscInferIndent, menuMiscFormatCode, menuMiscRepeatLine, menuMiscSectionBreak, menuMiscPathToClipboard, menuMiscPathList, menuMiscInsertTime, menuMiscCalculateDate, menuMiscHTMLFormat, menuMiscTextConvert, menuMiscTextCombine, menuMiscTextContents, menuMiscYieldWithRegExp, menuMiscExtractWithRegExp, menuMiscRunAtCursor, menuMiscSpecialCharacter, menuMiscEvaluateExpression, menuMiscReplaceTokens, menuMiscTransformFiles, menuMiscGoToEnvironment, menuMiscCompile, menuMiscPickCompiler, menuMiscPromptCommand, menuMiscReviewOutput, menuMiscSaveSnippet, menuMiscInvokeSnippet, menuMiscViewSnippet, menuMiscKeepUniqueItems, menuMiscNumberItems, menuMiscOrderItems, menuMiscReverseItems, menuMiscListDifferentItems, menuMiscQueryCommonItems, menuMiscExplorerFolder, menuMiscCommandPrompt, menuMiscBurnToCD, menuMiscWebDownload, menuMiscWebClientUtilities});
//Dialog.Show("Misc.", menuMisc.DropDownItems.Count);

menuWindow = CreateMenu("&Window");
menuWindowNext = CreateMenuItem("Next Window", "Control+Tab", menuItem_Click, "child speak");
menuWindowPrior = CreateMenuItem("Prior Window", "Control+Shift+Tab", menuItem_Click, "child speak");
menuWindowArrangeIcons = CreateMenuItem("Arrange Icons", "Alt+F11", menuItem_Click, "child speak");
menuWindowCascade = CreateMenuItem("Cascade", "Control+F11", menuItem_Click, "child speak");
menuWindowTileHorizontal = CreateMenuItem("Tile Horizontal", "Alt+Shift+F11", menuItem_Click, "child speak");
menuWindowTileVertical = CreateMenuItem("Tile Vertical", "Control+Shift+F11", menuItem_Click, "child speak");
menuWindow.DropDownItems.AddRange(new ToolStripMenuItem[] {menuWindowNext, menuWindowPrior, menuWindowArrangeIcons, menuWindowCascade, menuWindowTileHorizontal, menuWindowTileVertical});
//Dialog.Show("Window.", menuWindow.DropDownItems.Count);

menuHelp = CreateMenu("&Help");
menuHelpAbout = CreateMenuItem("&About ...", "Alt+F1", menuItem_Click, "frame silent");
menuHelpDocumentation = CreateMenuItem("Documentation", "F1", menuItem_Click, "frame speak");
menuHelpHistoryOfChanges = CreateMenuItem("History of Changes", "Shift+F1", menuItem_Click, "frame speak");
menuHelpKeyDescriber = CreateMenuItem("Key Describer", "Control+F1", menuItem_Click, "frame silent");
menuHelpHotKeySummary = CreateMenuItem("Hotkey Summary", "Alt+Shift+H", menuItem_Click, "frame speak");
menuHelpAlternateMenu= CreateMenuItem("Alternate Menu ...", "Alt+F10", menuItem_Click, "frame silent");
menuHelpContextMenu= CreateMenuItem("Context Menu ...", "Shift+F10", menuItem_Click, "child silent");
menuHelpSendToMenu= CreateMenuItem("SendTo Menu ...", "Control+F10", menuItem_Click, "child silent");
menuHelpElevateVersion = CreateMenuItem("Elevate Version", "F11", menuItem_Click, "frame speak");
menuHelp.DropDownItems.AddRange(new ToolStripItem[] {menuHelpAbout, menuHelpDocumentation, menuHelpHistoryOfChanges, menuHelpKeyDescriber, menuHelpHotKeySummary, menuHelpAlternateMenu, menuHelpContextMenu, menuHelpSendToMenu, menuHelpElevateVersion});
//Dialog.Show("Help.", menuHelp.DropDownItems.Count);

menuMain.Items.AddRange(new ToolStripItem[] {menuFile, menuEdit, menuDelete, menuNavigate, menuQuery, menuMisc, menuWindow, menuHelp});
//menuMain.Items.AddRange(new ToolStripItem[] {menuFile, menuEdit, menuDelete, menuNavigate, menuQuery, menuMisc, menuHelp});
statusBar = CreateStatusBar();
// this.Controls.AddRange(new Control[] {menuMain, statusBar});
this.Controls.AddRange(new Control[] {statusBar, menuMain});
this.MainMenuStrip = menuMain;
menuMain.MdiWindowListItem = menuWindow;
//this.AutoSize = true;
this.Size = new Size(600, 600);
this.StartPosition = FormStartPosition.CenterScreen;
this.Text = "EdSharp";
this.ResumeLayout();
this.KeyPreview = true;
//this.MdiChildActivate += delegate(object o, EventArgs e) {this.Child = (MdiChild) this.ActiveMdiChild;};
string s = App.ReadOption("MaximizeWindow", "N").Trim().ToUpper();
if (s == "Y" || s == "YES") this.Shown += delegate(object o, EventArgs e) {
this.WindowState = FormWindowState.Maximized;
this.Activate();
Win32.SetForegroundWindow(this.Handle);
};
this.Shown += delegate(object o, EventArgs e) {
Util.ActivateTitle(this.Text);
};

string sDir = Directory.GetCurrentDirectory();
string sFile = Path.Combine(App.DataDir, App.ReadData("Compiler", "Default") + ".ini");
s = Ini.ReadValue(sFile, "Data", "Directory", "");
if (Directory.Exists(s) && !Util.Equiv(sDir, s)) {
//Dialog.Show(sDir, s);
//Directory.SetCurrentDirectory(s);
AddMessage("Folder " + Path.GetFileName(s));
Directory.SetCurrentDirectory(s);
}

} // MdiFrame constructor

public void SetStatus(object o) {
string sText = o.ToString();
this.statusBar.Items[0].Text = sText;
} // SetStatus method

public string GetNoNameTitle() {
object[] children = this.MdiChildren;
List<int> list = new List<int>();
foreach (object o in children) {
MdiChild child = (MdiChild) o;
string sTitle = child.Text;
if (sTitle.StartsWith("NoName") && Path.GetExtension(sTitle).Length == 0) {
string s = sTitle.Substring(6);
int i = Int32.Parse(s);
list.Add(i);
}
}

int iTitle = 0;
for (int i = 1; i <= children.Length; i++) {
if (!list.Contains(i)) {
iTitle = i;
break;
}
}

if (iTitle == 0) iTitle = children.Length + 1;
string sReturn = "NoName" + iTitle.ToString();
return sReturn;
} // GetNoNameTitle

public bool ProcessCmdKey_Helper(ref Message msg, Keys keyData) {
string sKey = keyData.ToString();
int iIndex = -1;
if (this.Child != null) iIndex = this.Child.RTB.Index;
if (sKey.StartsWith("Menu,") || sKey.StartsWith("ControlKey,") || sKey.StartsWith("ShiftKey,")) sKey = "";
else if (iIndex == this.KeyIndex && sKey == this.KeyString) this.KeyRepeat += 1;
else this.KeyRepeat = 0;
if (sKey.Length > 0) {
this.KeyString = sKey;
this.KeyIndex = iIndex;
}
// Util.Say(keyData.ToString());
//Clipboard.SetText(Clipboard.GetText() + keyData.ToString() + "\r\n");
// Util.Say("Repeat " + this.KeyRepeat);

ToolStripMenuItem menuItem;
if (keyData == Keys.F9) {
HomerRichTextBox rtb = App.Frame.Child.RTB;
string sText = rtb.GetRange(rtb.Index, rtb.TextLength);
//Dialog.Show(App.TempFile, sText);
/*
sText = Util.ConvertQuotes(sText);
Util.String2FileA(sText, App.TempFile);
*/
//File.WriteAllText(App.TempFile, sText, Encoding.GetEncoding(0, null, null));
File.WriteAllText(App.TempFile, sText, Encoding.GetEncoding(0));
//File.WriteAllText(App.TempFile, sText, Encoding.GetEncoding("US-ASCII", new EncoderReplacementFallback(" "), new DecoderReplacementFallback(" ")));
//File.WriteAllText(App.TempFile, sText, Encoding.GetEncoding("US-ASCII", null, null));
// Win32.JFWRunFunction("SayAllTempFile");
COM.JFWRunFunction("SayAllTempFile");
base.ProcessCmdKey (ref msg, keyData);
return true;
}
else if (keyData == (Keys.Shift | Keys.F9)) {
string s = Util.File2String(App.TempFile);
if (!Util.IsNumeric(s)) return true;
int i = Int32.Parse(s);
HomerRichTextBox rtb = App.Frame.Child.RTB;
rtb.Index += i;
base.ProcessCmdKey (ref msg, keyData);
return true;
}

else if (keyData == (Keys.Insert)) {
// Util.Say("Insert key now");
return true;
}

else if (hashKey.TryGetValue(keyData, out menuItem)) {
//if (this.Child != null && !this.Child.RTB.IndentMode && menuItem == menuEditEnterNewLine) return base.ProcessCmdKey (ref msg, keyData);
this.bCommandComplete = false;
menuItem.PerformClick();
this.bCommandComplete = true;
return true;
}
else return base.ProcessCmdKey (ref msg, keyData);
} // ProcessCmdKey_Helper method

protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
return this.ProcessCmdKey_Helper(ref msg, keyData);
} // ProcessCmdKey handler

static MenuStrip CreateMainMenu() {
MenuStrip menuMain = new MenuStrip();
menuMain.AccessibleRole = AccessibleRole.MenuBar;
//menuMain.AutoSize = true;
//menuMain.CanOverflow = false;
menuMain.Dock = DockStyle.Top;
//menuMain.LayoutStyle = ToolStripLayoutStyle.Flow;
//menuMain.Stretch = false;
return menuMain;
} // CreateMainMenu method

static ToolStripMenuItem CreateMenu(string sText) {
ToolStripMenuItem menuItem = new ToolStripMenuItem(sText);
menuItem.AccessibleRole = AccessibleRole.MenuItem;
return menuItem;
} // CreateMenu method

static ToolStripMenuItem CreateMenuItem(string sText, string sKey, EventHandler eh) {
bool bFrame = false;
return CreateMenuItem(sText, sKey, eh, bFrame);
} // CreateMenuItem method

static ToolStripMenuItem CreateMenuItem(string sText, string sKey, EventHandler eh, bool bFrame) {
string sOptions = "";
if (bFrame) sOptions += "frame ";
if (sText.EndsWith(" ...") || sText.EndsWith("Again")) sOptions += "silent ";
return CreateMenuItem(sText, sKey, eh, sOptions);
}  // CreateMenuItem method

static ToolStripMenuItem CreateMenuItem(string sText, string sKey, EventHandler eh, string sOptions) {
ToolStripMenuItem menuItem = new ToolStripMenuItem(sText, null, eh);
menuItem.AccessibleRole = AccessibleRole.MenuItem;
menuItem.Tag = sOptions;

string sCommand = sText.Replace("&", "").Replace("...", "").Trim();
menuItem.Name = sCommand;
// Ini.WriteValue(@"c:\temp\temp.ini", "Keys", sCommand, sKey);
string s = Ini.ReadValue(App.IniFile, "Keys", sCommand, "").Trim();
if (!Util.Equiv(s, "None")) {
if (s.Length > 0) sKey = s;
sKey = sKey.Replace("&", "");
Keys keyData = Util.String2Key(sKey);
if (hashKey.ContainsKey(keyData)) {
//s = hashKey[keyData].Text.Split('\t')[0].Replace("&", "").Replace("...", "").Trim();
s = hashKey[keyData].Name;
Dialog.Show("Alert", "Cannot assign " + sKey + " to " + sCommand + ",\nsince already assigned to " + s);
}
else {
//sText += "\t" + Util.GetFriendlyKeyName(sKey);
//menuItem.ToolTipText = sKey;
//menuItem.ShortcutKeyDisplayString = sKey;
//sText += "   " + Util.GetFriendlyKeyName(sKey);
//menuItem.AccessibleDescription = Util.GetFriendlyKeyName(sKey);
string sFriendlyKey = Util.GetFriendlyKeyName(sKey);
menuItem.ShortcutKeyDisplayString = sFriendlyKey;
menuItem.AccessibleName = sText.Replace("&", "") + "   " + sFriendlyKey;
menuItem.Text = sText;
hashKey.Add(keyData, menuItem);
}
}

menuItem.Paint += delegate(object oSender, PaintEventArgs e) {
// if (!App.Frame.KeyDescriber) return;
foreach (ToolStripMenuItem menu in App.Frame.menuMain.Items) {
foreach (object o in menu.DropDownItems) {
ToolStripMenuItem item = o as ToolStripMenuItem;
if (item == null) continue;
if (!item.Selected) continue;
string[] aSummary = App.Frame.GetKeySummary(item);
string sSummary = aSummary[0] + " = " + aSummary[1] + ", " + aSummary[2];
string sDescription = aSummary[2];
if (sDescription != App.Frame.LastDescription) {
// System.Threading.Thread.Sleep(1000);
// Util.Say(sDescription);
App.Frame.SetStatus(sDescription);
App.Frame.LastDescription = sDescription;
}
break;
}
}
};

return menuItem;
} // CreateMenuItem method

static StatusStrip CreateStatusBar() {
StatusStrip sb = new StatusStrip();
sb.AccessibleRole = AccessibleRole.StatusBar;
sb.SuspendLayout();
ToolStripStatusLabel lblStatus = new ToolStripStatusLabel("Ready");
lblStatus.AutoSize = true;
sb.Items.AddRange(new ToolStripItem[] {lblStatus});
sb.AutoSize = true;
sb.Dock = DockStyle.Bottom;
sb.ResumeLayout();
return sb;
} // CreateStatusBar method

public string GetPercentAddress(HomerRichTextBox rtb) {
return String.Format("Line {0}   Column {1}   Percent{2}", rtb.Line, rtb.Column, rtb.Percent);
} // GetPercentAddress method

public string GetPageAddress(HomerRichTextBox rtb) {
string sText = rtb.Text;
int iIndex = rtb.Index;
sText = sText.Substring(0, iIndex);
int iPage = sText.Length - sText.Replace("\f", "").Length + 1;
iIndex = sText.LastIndexOf("\f");
if (iIndex >= 0) sText = sText.Substring(iIndex);
if (sText.StartsWith("\f")) sText = sText.Remove(0, 1);
int iLine = sText.Length - sText.Replace("\n", "").Length + 1;
iIndex = sText.LastIndexOf("\n");
int iColumn = sText.Length - iIndex;
return String.Format("Page {0}   Line {1}   Column {2}", iPage, iLine, iColumn);
} // GetPageAddress method

public void SetStatusAddress(object sender, EventArgs e) {
if (sender != null && !this.bCommandComplete) return;
if (this.Child == null) {
SetStatus("");
return;
}

HomerRichTextBox rtb = this.Child.RTB;
//string sText = String.Format("Line {0}\tColumn {1}\tPercent{2}", rtb.Line, rtb.Column, rtb.Percent);
string sText = "";
int iIndex = -1;
bool bPageAddress = true;
if (App.ReadOption("HardPageAddress", "N").ToLower().Substring(0, 1) != "y") bPageAddress = false;
if (bPageAddress) sText = GetPageAddress(rtb);
else  sText = GetPercentAddress(rtb);

iIndex = rtb.Index;
char c = ' ';
if (iIndex < rtb.TextLength) c = rtb.Text[iIndex];

int iNewIndex = rtb.Index;
int iNewTextLength = rtb.TextLength;
int iDelta = Math.Abs(iNewIndex - rtb.OldIndex);
if (!bPageAddress || iDelta != 1 || iNewTextLength != rtb.OldTextLength) {} // Do nothing
else if (c == '\f') Util.Say("FormFeed");
else if (c == '\n') Util.Say("LineFeed");
else if (c == '\t') Util.Say("TabChar");
rtb.OldIndex = iNewIndex;
rtb.OldTextLength = iNewTextLength;

if (sender == null) Util.Say(sText);
this.SetStatus(sText);

if (!rtb.IndentMode) return;
string sLine = rtb.RowText.Trim();
if (sLine.Length == 0) return;
string sComment = App.ReadOption("QuotePrefix", "> ");
if (sLine.StartsWith(sComment)) return;
int iLevels = GetIndent();
if (rtb.IndentLevels == iLevels) {
// Environment.SetEnvironmentVariable("EdSharpIndent", "", EnvironmentVariableTarget.User);
// Ini.WriteValue(App.IndentModeFile, "Data", "IndentChange", "", false);
System.IO.File.Create(App.IndentModeFile).Close();
return;}
string sDelta = GetDelta(rtb.IndentLevels, iLevels);
// Environment.SetEnvironmentVariable("EdSharpIndent", sDelta, EnvironmentVariableTarget.User);
Ini.WriteValue(App.IndentModeFile, "Data", "IndentChange", sDelta, false);
if (App.IndentChange) Util.Say(sDelta);
rtb.IndentLevels = iLevels;
} // SetStatusAddress method

public void AddMessage(object oText) {
bool bGlobal = false;
AddMessage(oText, bGlobal);
} // AddMessage method

public void AddMessage(object oText, bool bGlobal) {
string sText = oText.ToString();
Util.Say(sText, bGlobal);
if (App.CaptureOutput) Util.StringAppend2File(sText + "\r\n", App.TempFile);
//sText = this.statusBar.Items[0].Text + "\t" + sText;
sText = this.statusBar.Items[0].Text + "   " + sText;
SetStatus(sText);
} // AddMessage method

public void SetMessage(object oText) {
SetStatus(oText);
Util.Say(oText);
} // SetMessage method

public void GetRowAndCol(out int iRow, out int iCol) {
HomerRichTextBox rtb = this.Child.RTB;
int iIndex = rtb.SelectionStart + rtb.SelectionLength;
iRow = rtb.GetLineFromCharIndex(iIndex);
iCol = iIndex - rtb.GetFirstCharIndexOfCurrentLine();
} // GetRowAndCol method

bool IsEmptyWindow() {
return !(this.Child == null || this.Child.RTB.Modified || this.Child.RTB.TextLength > 0);
} // IsEmptyWindow method

public bool IsCharacter() {
int iIndex;
return IsCharacter(out iIndex);
} // IsCharacter method

public bool IsCharacter(out int iIndex) {
HomerRichTextBox rtb = this.Child.RTB;
iIndex = rtb.Index;
if (iIndex >= rtb.TextLength) {
AddMessage("No character at cursor!");
return false;
}
else return true;
} // IsCharacter method

public int GetIndent() {
return GetIndent(this.Child.RTB.Row);
} // GetIndent method

public int GetIndent(int iRow) {
string sIndent = App.ReadOption("IndentUnit", "  ");
sIndent = Util.Literalize(sIndent);
MdiChild child = this.Child;
HomerRichTextBox rtb = child.RTB;
string sLine = rtb.GetRowText(iRow);
int iLength = sIndent.Length;
int iLevels = 0;
while (sLine.StartsWith(sIndent)) {
iLevels++;
if (sLine.Length == iLength) sLine = "";
else sLine = sLine.Substring(iLength);
}
return iLevels;
} // GetIndent method

public string GetDelta(int iBefore, int iAfter) {
if (iBefore < iAfter) return "In " + (iAfter - iBefore);
else return "Out " + (iBefore - iAfter);
} // GetDelta method

public string GetStyleText() {
HomerRichTextBox rtb = this.Child.RTB;
string sText = "";
if (rtb.SelectionFont.Bold) sText += "Bold ";
if (rtb.SelectionFont.Italic) sText += "Italic ";
if (rtb.SelectionFont.Underline) sText += "Underline";
sText = sText.Trim();
if (sText.Length == 0) sText = "Regular";
return sText;
} // GetStyleText method

public string GetJustifyText() {
HomerRichTextBox rtb = this.Child.RTB;
string sText = "Left";
HorizontalAlignment ha = rtb.SelectionAlignment;
if (rtb.SelectionBullet) sText = "Bullet";
else if (ha == HorizontalAlignment.Center) sText = "Center";
else if (ha == HorizontalAlignment.Right) sText = "Right";
return sText;
} // GetJustifyText method

public string GetBaselineText() {
HomerRichTextBox rtb = this.Child.RTB;
string sText = "Flat";
int iOffset = rtb.SelectionCharOffset;
if (iOffset < 0) sText = "Down";
else if (iOffset > 0) sText = "Up";
return sText;
} // GetBaselineText method

public string GetFontText(Font font, Color color) {
string sFont = Util.Font2String(font);
string sColor = Util.Color2String(color);
sFont += ", Color=" + sColor;
return sFont;
} // GetFontText method

public string[] GetSnippetFiles(out string[] aValues) {
string sBaseDir = @"Snippets\" + App.ReadData("Compiler", "Default");
string sDir = Path.Combine(App.DataDir, sBaseDir);
if (!Directory.Exists(sDir)) Directory.CreateDirectory(sDir);
string[] aResults = Directory.GetFiles(sDir);

List<string> listResults = new List<string>(aResults);
List<string> listFiles = new List<string>();
foreach (string s in aResults) listFiles.Add(Path.GetFileName(s).ToLower());

sBaseDir = @"Snippets\Default";
sDir = Path.Combine(App.DataDir, sBaseDir);
if (!Directory.Exists(sDir)) Directory.CreateDirectory(sDir);
aResults = Directory.GetFiles(sDir);
foreach (string s in aResults) if (!listFiles.Contains(Path.GetFileName(s).ToLower())) listResults.Add(s);
aResults = listResults.ToArray();

aValues = new string[aResults.Length];
for (int i = 0; i < aResults.Length; i++) aValues[i] = Path.GetFileName(aResults[i]);
return aResults;
} // GetSnippetFiles method

public void GetDateAndTime(out string sDate, out string sTime) {
DateTime dt = DateTime.Now;
string sDateFormat = App.ReadOption("DateFormat", "");
string sTimeFormat = App.ReadOption("TimeFormat", "");
if (sDateFormat == "0") sDate = "";
else sDate = (sDateFormat.Length > 0) ? dt.ToString(sDateFormat) : dt.ToLongDateString();

if (sTimeFormat == "0") sTime = "";
else sTime = (sTimeFormat.Length > 0) ? dt.ToString(sTimeFormat) : dt.ToShortTimeString();
} // GetDateAndTime method

public string ReplaceTokens(string sText) {
string[] aTokens = App.ReadSectionKeys("Tokens");
foreach (string sToken in aTokens) {
if (sText.IndexOf(sToken) == -1) continue;
string s = App.ReadValue("Tokens", sToken, "");
string sFile = GetSnippetDir() + @"\" + s;
if (File.Exists(sFile)) s = Util.File2String(sFile);
//Dialog.Show(sFile, s);
string sResult = JS.Eval(s).ToString();
if (sResult == null) sResult = "";
sText = sText.Replace("%" + sToken + "%",sResult);
}

return sText;
} // ReplaceTokens method

public void TransFormFiles() {
if (File.Exists(App.TempFile)) File.Delete(App.TempFile);
App.CaptureOutput = true;
HomerRichTextBox rtb = this.Child.RTB;

string sJob = App.ReadData("Job", "");
string sTransformFile = Dialog.OpenFile("Open Job", sJob);
if (sTransformFile.Length == 0) return;

App.WriteData("Job", sTransformFile);
string sTransformBody = Util.File2String(sTransformFile);

string sSourceList = rtb.Text.Trim();
string[] aSourceLines = sSourceList.Split('\n');
string sDir = Directory.GetCurrentDirectory();
string[] aChoices = {"&Test", "&Run", "&Verbose"};
string sChoice = Dialog.Choose("Choose Mode", "", aChoices, 0);
if (sChoice.Length == 0) return;

foreach (string sSourceLine in aSourceLines) {
string sSourceFile = sSourceLine.Trim();
if (sSourceFile.Length == 0) continue;
string sSourceDir = Path.GetDirectoryName(sSourceFile);
if (sSourceDir.Length > 0 && Directory.Exists(sSourceDir)) sDir = sSourceDir;
else sSourceFile = Path.Combine(sDir, Path.GetFileName(sSourceFile));

if (!File.Exists(sSourceFile)) continue;

AddMessage(Path.GetFileName(sSourceFile));
Encoding en = App.Frame.Child.GetYieldEncoding();
string sSourceBody = Util.File2String(sSourceFile, ref en);

string sComment = "";
string sMatch = "";
string sReplace = "";
int iLine = 1;
string[] aTransformLines = sTransformBody.Split('\n');
foreach (string sTransformLine in aTransformLines) {
string sLine = sTransformLine.Trim('\r');
int iRemainder = iLine % 4;
if (iRemainder == 1) {
sComment = sLine;
if (sChoice != "&Run") AddMessage(sComment);
}
else if (iRemainder == 2) {
sMatch = sLine;
}
else if (iRemainder == 3) {
try {
Regex rx = new Regex(sMatch, RegexOptions.Multiline);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}

sReplace = Util.Literalize(sLine);

if (sChoice != "&Run") {
int iMatches = Util.RegExpCountCase(sSourceBody, sMatch);
AddMessage(Util.Pluralize(iMatches, "match", "matches"));
}
if (sChoice != "&Test") {
sSourceBody = Util.RegExpReplaceCase(sSourceBody, sMatch, sReplace);
Util.String2File(sSourceBody, sSourceFile, ref en);
}
}
iLine++;
}
}
AddMessage("Done!", true);
App.CaptureOutput = false;
} // TransForm files method

public static string GetSnippetDir() {
string sBaseDir = @"Snippets\" + App.ReadData("Compiler", "Default");
string sDir = Path.Combine(App.DataDir, sBaseDir);
if (!Directory.Exists(sDir)) Directory.CreateDirectory(sDir);
return sDir;
} // GetSnippetDir method

public string GetDirChoice() {
string[] aButtons = {"&Current", "&Program", "&Data", "&Snippet", "&Other"};
string sButton = Dialog.Choose("Choose Directory", "", aButtons, 0);
if (sButton.Length == 0) return "";

string sDir = Directory.GetCurrentDirectory();
switch (sButton) {
case "&Current" :
if (this.Child != null && this.Child.File.IndexOf(@"\") >= 0) sDir = Path.GetDirectoryName(this.Child.File);
break;
case "&Program" :
sDir = App.ProgramDir;
break;
case "&Data" :
sDir = App.DataDir;
break;
case "&Snippet" :
sDir = App.DataDir + @"\Snippets\" + App.ReadData("Compiler", "Default");
if (!Directory.Exists(sDir)) Directory.CreateDirectory(sDir);
break;
case "&Other" :
sDir = Dialog.OpenFolder("Open Folder", "Name", Directory.GetCurrentDirectory());
if (sDir.Length == 0) return "";
break;
}
return sDir;
} // GetDirChoice method

public int GetViewLevel(string sFile) {
string sExt = Path.GetExtension(sFile).TrimStart('.');
int iReturn = 1;
string sViewLevels = App.ReadOption("ViewLevels", "");
string[] aViewLevels = sViewLevels.Split(' ');
foreach (string sViewLevel in aViewLevels) {
string[] aViewLevel = sViewLevel.Split(':');
if (aViewLevel.Length < 2) continue;
if (sExt == aViewLevel[0].Trim().ToLower()) {
try {
iReturn = Int32.Parse(aViewLevel[1]);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
}
}
}
return iReturn;
} // GetViewLevel method

public string[] GetKeySummary(ToolStripMenuItem item) {
string sCommand = item.Name;
string sValue = Ini.ReadValue(App.HotkeyIniFile, "Hotkeys", sCommand, "");
if (sValue.Length == 0) sValue = Ini.ReadValue(App.HotkeyIniFile, "Hotkeys", "Say " + sCommand, "");
if (sValue.Length == 0) sValue = "No description available";
string sKey = "";
string sDescription = "";
int iComma = sValue.IndexOf(",");
if (iComma == -1) sDescription = sValue;
else {
sKey = sValue.Substring(0, iComma);
sDescription = sValue.Substring(iComma + 1);
}
return new string[] {sCommand, sKey, sDescription};
} // GetKeySummary method

public void menuItem_Click(object sender, EventArgs e) {
//Util.Beep();
HomerRichTextBox rtb = null;
string[] aLabels, aValues, aResults;
bool bSelected;
int iLength, iStart, iEnd, iResult, iIndex, iLine, iPercent, iCount;
string sFile, sMatch, sReplace, sPattern, sSubstitute, sLine, sTitle, sText, sResult, sLabel, sValue;

ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;
string sOptions = (string) menuItem.Tag;
sOptions = " " + sOptions.Trim().ToLower() + " ";
//sLabel = menuItem.Text.Replace("&", "").Replace(" ...", "").Split('\t')[0];
sLabel = menuItem.Name;
if (this.KeyDescriber && sLabel != "Key Describer") {
string[] aSummary = GetKeySummary(menuItem);
SetMessage(aSummary[0]);
AddMessage(aSummary[1]);
AddMessage(aSummary[2]);
return;
}

if (sOptions.Contains(" silent ")) SetStatus(sLabel);
else SetMessage(sLabel);

MdiChild child = this.Child;
if (child == null) {
if (!sOptions.Contains(" frame ")) return;
}
else {
rtb = Child.RTB;
}

if (menuItem == menuFileNew) {
child = new MdiChild(App.Frame);
}

if (menuItem == menuFileNewFromClipboard) {
new MdiChild(App.Frame);
child = App.Frame.Child;
Child.RTB.Text = Clipboard.GetText();
child.RTB.Modified = true;
}

if (menuItem == menuFileOpen) {

string sDir = "";
/*
if (child != null) sDir = Path.GetDirectoryName(child.File);
*/
sFile = Dialog.OpenFile("", sDir);
if (sFile.Length == 0) return;

int iConvert = 0;
if (Path.GetExtension(sFile).ToLower() == ".rtf") {
switch (Dialog.Confirm("Confirm", "Treat as rich text?", "Y")) {
case "Y" :
iConvert = 2;
break;
case "" :
return;
}
}
OpenOrActivateWindow(sFile, iConvert);
}

if (menuItem == menuFileOpenOtherFormat) {
sFile = Dialog.OpenFile("", "");
if (sFile.Length == 0) return;

OpenOrActivateWindow(sFile, 2);
return;
/*
AddMessage("Converting");
try {
sText = COM.ConvertFile2String(sFile);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}

if (!IsEmptyWindow()) new MdiChild(this);
child = this.Child;
rtb = child.RTB;
rtb.Text = sText;
rtb.Index = 0;
child.Text = Path.GetFileNameWithoutExtension(sFile) + ".txt";
//rtb.Modified = true;
rtb.Modified = false;
//AddMessage("Done!");
*/
}

if (menuItem == menuFileOpenAgain) {
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

iIndex = rtb.Index;
if (Dialog.Confirm("Confirm", "Existing text will be  replaced.  Sure?", "Y") != "Y") return;
child.LoadTextOrRtfFile(sFile);
rtb.Index = iIndex;
}

if (menuItem == menuFileRecent) {
aResults = App.ReadSectionKeys("Recent");
List<string> list = new List<string>(aResults);
for (int i = list.Count - 1; i >=0; i--) {
string s = list[i];
if (File.Exists(s)) continue;
App.DeleteKey("Recent", s);
list.RemoveAt(i);
}

aResults = list.ToArray();
if (aResults.Length == 0) {
AddMessage("No items!");
return;
}

string[] aTime = new string[aResults.Length];
for (int i = 0; i < aTime.Length; i++) aTime[i] = App.ReadValue("Recent", aResults[i], "");
Array.Sort(aTime, aResults);
//Array.Reverse(aTime);
Array.Reverse(aResults);

iLength = aResults.Length;
int iMax = Int32.Parse(App.ReadOption("RecentFiles", "30"));
if (iLength > iMax) {
List<string> listResults = new List<string>(aResults);
for (int i = iLength - 1; i >= iMax; i--) {
App.DeleteKey("Recent", listResults[i]);
listResults.RemoveAt(i);
}
aResults = listResults.ToArray();
}

string[] aDisplay = new string[aResults.Length];
for (int i = 0; i < aDisplay.Length; i++) aDisplay[i] = Path.GetFileName(aResults[i]);

sFile = Dialog.Pick("Recent Files", aResults, aDisplay, false, 0);
if (sFile.Length == 0) return;

OpenOrActivateWindow(sFile, GetViewLevel(sFile));
}

if (menuItem == menuFileFind) {
FileFind();
}

if (menuItem == menuFileSaveCopy) {
AddMessage("Save Copy");
sFile = child.File + ".bak";
sFile = Dialog.SaveFile("", sFile);
if (sFile.Length == 0) return;
if (sFile.ToLower().IndexOf(".rtf") >= 0) rtb.SaveFile(sFile, RichTextBoxStreamType.RichText);
//else if (Util.IsUnicode(rtb.Text)) Util.String2File(rtb.Text, sFile);
//else rtb.SaveFile(sFile, RichTextBoxStreamType.PlainText);
else Util.String2File(Util.Convert2WinLineBreak(rtb.Text), sFile);
this.SetRecent(sFile);
}

if ((menuItem == menuFileSave) || (menuItem == menuFileSaveAs)) {
sFile = child.File;
if ((menuItem == menuFileSave) && sFile.Contains(@"\")) sText = "";//AddMessage("Save");
else {
sFile = Dialog.SaveFile("", sFile);
if (sFile.Length == 0) return;
}

//Dialog.Show(sFile);
//if (Path.GetExtension(sFile).Length == 0) sFile += ".txt";
child.SaveTextOrRtfFile(sFile);
//this.SetRecent(sFile);
rtb.Modified = false;
}

if (menuItem == menuFileExport) {
sFile = child.File;
aValues = Ini.ReadSectionKeys(App.IniFile, "Export");
//aValues = Array.FindAll(aValues, delegate(string s) {return s.StartsWith("pdf2");} );
//aValues = aValues.ConvertAll(delegate(string s){s.ToLower();});
HomerList hl = new HomerList(aValues);
hl.ToLower();
//list = list.ConvertAll<string>(delegate(string s) { return s.ToLower(); });
string sExt = Path.GetExtension(child.File).ToLower().TrimStart('.');
sMatch = @"^\w+2\w+$";
HomerList hl2 = hl.FindLike(sMatch);
hl.RemoveLike(sMatch);
sMatch = "^" + sExt + @"2\w+";
hl2 = hl2.FindLike(sMatch);
hl.AddRange(hl2);
hl.AddRange("asc|doc|htm|mac|rtf|unx|xml");
// do not offer original format, since already available with Control+O
if (sExt.Length > 0 && !hl.Contains(sExt)) hl.Add(sExt + "2" + sExt);
// hl.Add("Other");
hl.KeepUnique();
aValues = hl.ToArray();
hl.ReplaceLike(@"^\w+2(\w+)$", "$1");
string[] aDisplay = hl.ToArray();
Array.Sort(aDisplay, aValues);

hl.Clear();
hl.AddRange(aDisplay);
hl.Add("Other");
aDisplay = hl.ToArray();
hl.Clear();
hl.AddRange(aValues);
hl.Add("Other");
aValues = hl.ToArray();

sTitle = "Export " + sExt + " to ";
sResult = Dialog.Pick(sTitle, aValues, aDisplay, false, 0);
//Dialog.Show(sResult);
if (sResult.Length == 0) return;

int iCodePage = -1;
string sCodePage = "";
if (sResult == "Other") {
iCodePage = Dialog.PickEncoding("", 0);
if (iCodePage == -1) return;
sCodePage = iCodePage.ToString();
sResult = sCodePage;
}

string sTargetExt = Util.RegExpReplaceCase(sResult, @"^\w+2", "");
sFile = Path.ChangeExtension(sFile, sTargetExt);
sFile = Dialog.SaveFile("", sFile);
if (sFile.Length == 0) return;

if (sCodePage.Length > 0) sExt = "Other";
else sExt = sResult.ToLower();
switch (sExt) {
case "Other" :
Encoding en = Encoding.GetEncoding(iCodePage);
sText = rtb.Text;
sText = Util.Convert2WinLineBreak(sText);
File.WriteAllText(sFile, sText, en);
break;
case "asc" :
case "mac" :
case "unx" :
sText = rtb.Text;
if (sExt == "asc") sText = Util.Convert2Ascii(sText);
else if (sExt == "mac") sText = Util.Convert2MacLineBreak(sText);
else if (sExt == "unx") sText = Util.Convert2UnixLineBreak(sText);
// Util.String2File(sText, sFile);
File.WriteAllText(sFile, sText, Encoding.Default);
break;
case "rtf" :
rtb.SaveFile(sFile, RichTextBoxStreamType.RichText);
break;
default :
string s = Path.GetExtension(child.File);
if (Util.Equiv(s, ".rtf")) sText = rtb.Rtf;
else sText = rtb.Text;
Util.ConvertString2FileFormat(sText, s, sFile, sExt);
break;
}
if (File.Exists(sFile)) AddMessage("Done!");
else AddMessage("Error!");
}

if (menuItem == menuFileRename) {
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

string sName = Dialog.Input("Rename", "File Name", child.Text).Trim();
if (sName.Length == 0) return;

string sNewFile = Path.Combine(Path.GetDirectoryName(sFile), sName);
try {
File.Move(sFile, sNewFile);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}
child.Text = sName;
child.File = sNewFile;
SetRecent(sNewFile);
}

if (menuItem == menuFileMailBody) {
string sSubject = Path.GetFileNameWithoutExtension(child.Text);
string sBody = rtb.Text;
// string sRecipient = "";
try {
MapiMail.SendMail(sSubject, sBody, null, null);
}
catch {
Util.MailMessage("", sSubject, sBody);
}
return;

/*
try {
sText = rtb.Text;
sText = Util.Convert2WinLineBreak(sText);
sText = Util.RegExpReplaceCase(sText, "\r\n", "%0D%0A");
sText = Util.RegExpReplaceCase(sText, " ", "%20");
sText = Util.RegExpReplaceCase(sText, "\t", "%09");
sText = Util.RegExpReplaceCase(sText, "\"", "%22");
sText = Util.RegExpReplaceCase(sText, "'", "%27");
sText = Util.RegExpReplaceCase(sText, "\\\\", "%5C");
string sCommand = "mailto:?BODY=" + sText;
Process.Start(sCommand);
}
catch {
Mail(false);
}
*/
}

if (menuItem == menuFileMailAttach) {
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

string sSubject = Path.GetFileName(child.Text) + " attached";
KeyValuePair<string, string>[] aAttachments = {new KeyValuePair<String, String>(Path.GetFileName(sFile), sFile)};
try {
MapiMail.SendMail(sSubject, "", null, aAttachments);
}
catch {
}
return;

//Mail(true);
}

if (menuItem == menuFileRun) {
sFile = child.File;
if (!sFile.Contains(@"\") || rtb.Modified) {
sFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(sFile));
sText = rtb.Text;
Util.String2File(sText, sFile);
}

Process.Start(sFile);
}

if (menuItem == menuFilePrint) {
sFile = child.File;
sFile = Path.GetFileName(sFile);
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

if (Dialog.Confirm("Confirm", "Print " + sFile + "?", "Y") != "Y") return;

if (Path.GetExtension(sFile).ToLower() == ".rtf") COM.InvokeVerb(sFile, "Print");
else Util.RunHideWait("Notepad.exe /p " + Util.Quote(sFile));
/*
sFile = Path.Combine(Path.GetTempPath(), sFile);
sText = rtb.Text;
sText = Util.Convert2WinLineBreak(sText);
Util.String2File(sText, sFile);
string sExe;
if (Path.GetExtension(sFile).ToLower() == ".rtf") sExe = "cmd.exe /c WordPad.exe";
else sExe = "Notepad.exe";
string sCommand = sExe + " /P " + Util.Quote(sFile);
Util.RunHideWait(sCommand);
File.Delete(sFile);
*/
}

if (menuItem == menuFileProperties) {
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

Dialog.Properties(sFile);
}

if (menuItem == menuFileCurrentWindows) {
CurrentWindows();
}

if (menuItem == menuFileClose) {
CloseWindow(child);
}

if (menuItem == menuFileCloseAllButCurrentWindow) {
CloseAllButCurrentWindow();
}

if (menuItem == menuFileExit) {
ExitApp();
}

if (menuItem == menuEditSelectAll) {
rtb.SelectAll();
iCount = rtb.SelectedText.Length;
AddMessage(Util.Pluralize(iCount, "character"));
}

if (menuItem == menuEditUnselectAll) {
rtb.DeselectAll();
iIndex = rtb.Index;
if (iIndex >= rtb.TextLength) return;
sText = rtb.GetRange(rtb.Index, rtb.Index + 1);
AddMessage(sText);
}

if (menuItem == menuEditCopy) {
if (rtb.SelectionLength == 0) {
AddMessage("Line");
sText = rtb.RowText + LineBreak;
}
else {
AddMessage("Selected");
sText = rtb.SelectedText;
rtb.StoreSelection();
}
sText = Util.Convert2WinLineBreak(sText);
Clipboard.SetText(sText);
}

if (menuItem == menuEditCopyAppend) {
sText = Clipboard.GetText();
sText = Util.Convert2UnixLineBreak(sText);
if (sText.Length > 0 && !sText.EndsWith(LB)) sText += LB;
if (rtb.SelectionLength == 0) {
AddMessage("Line");
sText += rtb.RowText + LB;
}
else {
AddMessage("Selected");
sText += rtb.SelectedText;
rtb.StoreSelection();
}
sText = Util.Convert2WinLineBreak(sText);
Clipboard.SetText(sText);
}

if (menuItem == menuEditCopyRichText) {
rtb.Copy();
}

if (menuItem == menuEditCut) {
if (rtb.SelectionLength == 0) {
AddMessage("Line");
sText = rtb.RowText + LineBreak;
rtb.Select(rtb.RowStart, rtb.RowLength);
}
else {
AddMessage("Selected");
sText = rtb.SelectedText;
}
rtb.Cut();
sText = Util.Convert2WinLineBreak(sText);
Clipboard.SetText(sText);
Util.Say(rtb.RowText);
}

if (menuItem == menuEditCutAppend) {
sText = Clipboard.GetText();
sText = Util.Convert2UnixLineBreak(sText);
if (sText.Length > 0 && !sText.EndsWith(LB)) sText += LB;
if (rtb.SelectionLength == 0) {
AddMessage("Line");
sText += rtb.RowText + LB;
rtb.Select(rtb.RowStart, rtb.RowLength);
}
else {
AddMessage("Selected");
sText += rtb.SelectedText;
}
rtb.Cut();
sText = Util.Convert2WinLineBreak(sText);
Clipboard.SetText(sText);
Util.Say(rtb.RowText);
}

if (menuItem == menuEditPaste) {
rtb.Paste();
sText = Clipboard.GetText();
sText = Util.Convert2UnixLineBreak(sText);
aResults = Util.RegExpExtractCase(sText, @"\s+\Z");
if (aResults.Length > 0) {
iIndex = rtb.Index;
sText = aResults[0];
rtb.ReplaceRange(iIndex, iIndex, sText);
rtb.Index = iIndex + sText.Length;
}
}

if (menuItem == menuEditPasteFile) {
sFile = Dialog.OpenFile("", "");
if (sFile.Length == 0) return;

sText = Util.File2String(sFile);
string sChoice = "N";
if (Path.GetExtension(sFile).ToLower() == ".rtf") sChoice = Dialog.Confirm("Confirm", "Treat as rich text?", "Y");
if (sChoice.Length == 0) return;

rtb.Index = rtb.SelectionStart + rtb.SelectionLength;
if (sChoice == "Y") rtb.SelectedRtf = sText;
else rtb.SelectedText = sText;
Util.Say(rtb.RowText);
}

if (menuItem == menuEditUndo) {
rtb.Undo();
}

if (menuItem == menuEditRedo) {
rtb.Redo();
}

if (menuItem == menuEditStartSelection) {
//if (!IsCharacter(out iIndex)) return;
iIndex = rtb.Index;
rtb.StartSelection = iIndex;
if (iIndex >=0 && iIndex < rtb.TextLength) {
sText = rtb.GetRange(rtb.Index, rtb.Index + 1);
AddMessage(sText);
}
}

if (menuItem == menuEditCompleteSelection) {
iStart = rtb.StartSelection;
iEnd = rtb.Index;
if (iStart > iEnd) Util.Swap(ref iStart, ref iEnd);
rtb.SelectRange(iStart, iEnd);
iCount = rtb.SelectedText.Length;
AddMessage(Util.Pluralize(iCount, "character"));
rtb.OldSelectionStart = rtb.SelectionStart;
rtb.OldSelectionLength = rtb.SelectionLength;
}

if (menuItem == menuEditReselect) {
rtb.Reselect();
}

if (menuItem == menuEditCopyAll) {
sText = rtb.Text;
sText = Util.Convert2WinLineBreak(sText);
Clipboard.SetText(sText);
}

if (menuItem == menuEditSelectChunk) {
bool bLoop = false;
string c = "";
object[] a = GetChunk();
iStart = (int) a[0];
iIndex = iStart;
sText = rtb.Text;
if (rtb.SelectionLength == 0) {
AddMessage("Select Chunk");
}
else {
iStart = rtb.SelectionStart;
bLoop = iIndex < sText.Length;
while (bLoop) {
c = sText.Substring(iIndex, 1);
bLoop = (c.Trim().Length == 0);
iIndex++;
bLoop = (bLoop && iIndex < sText.Length);
}
}

bLoop = iIndex < sText.Length;
while (bLoop) {
c = sText.Substring(iIndex, 1);
bLoop = (c.Trim().Length > 0);
iIndex++;
bLoop = (bLoop && iIndex < sText.Length);
}
iEnd = iIndex;
rtb.SelectRange(iStart, iEnd);
}

if (menuItem == menuEditAppendFromClipboard) {
if (child.AppendFromClipboard == 0) {
AddMessage("Append from Clipboard On");
child.AppendFromClipboard = -1;
child.NextClipboardViewer =  Util.SetClipboardViewer(child.Handle);
}
else {
AddMessage("No Append from Clipboard");
child.AppendFromClipboard = 0;
Util.ChangeClipboardChain(child.Handle, child.NextClipboardViewer);
child.NextClipboardViewer = (IntPtr) 0;
}
}

if (menuItem == menuEditQuote) {
//if (!IsCharacter(out iIndex)) return;

string sPrefix = App.ReadOption("QuotePrefix", "> ");
if (rtb.SelectionLength == 0) {
AddMessage("Line");
iStart = rtb.RowStart;
iEnd = iStart + rtb.RowText.Length;
//Dialog.Show(iStart, iEnd);
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

sText = rtb.GetRange(iStart, iEnd);
sText = Util.RegExpReplaceCase(sText, "^", sPrefix);
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
Util.Say(rtb.RowText);
}

if (menuItem == menuEditUnquote) {
string sPrefix = App.ReadOption("QuotePrefix", "> ");
if (rtb.SelectionLength == 0) {
AddMessage("Line");
iStart = rtb.RowStart;
iEnd = iStart + rtb.RowText.Length;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

sText = rtb.GetRange(iStart, iEnd);
//sText = Util.RegExpReplaceCase(sText, @"^( |\t|\>)+", "");
sText = Util.RegExpReplaceCase(sText, @"^" + sPrefix, "");
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
Util.Say(rtb.RowText);
}

if (menuItem == menuEditUpperCase) {
if (rtb.SelectionLength == 0) {
AddMessage("Character");
iStart = rtb.Index;
iEnd = iStart + 1;
bSelected = false;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
bSelected = true;
}

sText = rtb.GetRange(iStart, iEnd);
sText = sText.ToUpper();
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
if (bSelected) sText = rtb.RowText;
else sText = rtb.GetRange(rtb.Index, rtb.Index + 1);
AddMessage(sText);
}

if (menuItem == menuEditLowerCase) {
if (rtb.SelectionLength == 0) {
AddMessage("Character");
iStart = rtb.Index;
iEnd = iStart + 1;
bSelected = false;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
bSelected = true;
}

sText = rtb.GetRange(iStart, iEnd);
sText = sText.ToLower();
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
if (bSelected) sText = rtb.RowText;
else sText = rtb.GetRange(rtb.Index, rtb.Index + 1);
AddMessage(sText);
}

if (menuItem == menuEditProperCase) {
if (rtb.SelectionLength == 0) {
AddMessage("Character");
iStart = rtb.Index;
iEnd = iStart + 1;
bSelected = false;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
bSelected = true;
}

sText = rtb.GetRange(iStart, iEnd);
sText = Util.ProperCase(sText);
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
if (bSelected) sText = rtb.RowText;
else sText = rtb.GetRange(rtb.Index, rtb.Index + 1);
AddMessage(sText);
}

if (menuItem == menuEditSwapCase) {
if (rtb.SelectionLength == 0) {
AddMessage("Character");
iStart = rtb.Index;
iEnd = iStart + 1;
bSelected = false;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
bSelected = true;
}

sText = rtb.GetRange(iStart, iEnd);
sText = Util.SwapCase(sText);
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
if (bSelected) sText = rtb.RowText;
else sText = rtb.GetRange(rtb.Index, rtb.Index + 1);
AddMessage(sText);
}

if (menuItem == menuEditYieldEncoding) {
if (rtb.SelectionLength == 0) {
sTitle = "Yield Encoding All";
iStart = 0;
iEnd = rtb.TextLength;
}
else {
sTitle = "Yield Encoding Selected";
iStart = rtb.SelectionStart;
// Dialog.Show(iStart, rtb.SelectionLength);
iEnd = iStart + rtb.SelectionLength;
}

sText = rtb.GetRange(iStart, iEnd);

// string[] aButtons = {"&Default", "&ASCII", "UTF-&7", "UTF-&8", "&Unicode", "UTF-&32", "&Latin1", "&Other"};
string[] aButtons = {"&ASCII", "&Latin1", "UTF-&8", "&UTF-16", "UTF-&7", "UTF-&32", "&Other", "&Codes"};
string sButton = Dialog.Choose(sTitle, "", aButtons, 0);
if (sButton.Length == 0) return;

Encoding def = Encoding.Default;
byte[] aBytes = new byte[def.GetByteCount(sText)];
def.GetBytes(sText, 0, sText.Length, aBytes, 0);
switch (sButton) {
case "&Default" :
sText = def.GetString(aBytes);
break;
case "&ASCII" :
Encoding asc = Encoding.GetEncoding("us-ascii", new EncoderReplacementFallback(""), new DecoderReplacementFallback(""));
// aBytes = new byte[asc.GetByteCount(sText)];
// asc.GetBytes(sText, 0, sText.Length, aBytes, 0);
sText = asc.GetString(aBytes);
break;
case "UTF-&7" :
sText = Encoding.UTF7.GetString(aBytes);
break;
case "UTF-&8" :
sText = Encoding.UTF8.GetString(aBytes);
break;
case "&UTF-16" :
sText = Encoding.Unicode.GetString(aBytes);
break;
case "UTF-&32" :
sText = Encoding.UTF32.GetString(aBytes);
break;
case "&Latin1" :
Encoding latin1 = Encoding.GetEncoding(1252, new EncoderReplacementFallback(""), new DecoderReplacementFallback(""));
// aBytes = new byte[latin1.GetByteCount(sText)];
// latin1.GetBytes(sText, 0, sText.Length, aBytes, 0);
sText = latin1.GetString(aBytes);
break;
case "&Codes" :
sResult = "\n";
foreach (char c in sText) {
sResult+= ((int) c).ToString() + "\n";
}
sText = sResult;
break;

case "&Other" :
/*
string sCodePage = Dialog.Input("Input", "Code Page:", "");
if (sCodePage.Length == 0) return;
Encoding other;
try {
if (Util.IsNumeric(sCodePage)) other = Encoding.GetEncoding(Int32.Parse(sCodePage), new EncoderReplacementFallback(""), new DecoderReplacementFallback(""));
else other = Encoding.GetEncoding(sCodePage, new EncoderReplacementFallback(""), new DecoderReplacementFallback(""));
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}
*/

int iCodePage = Dialog.PickEncoding("", 0);
if (iCodePage == -1) return;

Encoding other = Encoding.GetEncoding(iCodePage, new EncoderReplacementFallback(""), new DecoderReplacementFallback(""));

// aBytes = new byte[other.GetByteCount(sText)];
// other.GetBytes(sText, 0, sText.Length, aBytes, 0);
sText = other.GetString(aBytes);
break;
}

rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart + sText.Length;
Util.Say(rtb.RowText);
}

if (menuItem == menuEditJoinLines) {
if (rtb.SelectionLength == 0) {
AddMessage("All");
iStart = 0;
iEnd = rtb.TextLength;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

sText = rtb.GetRange(iStart, iEnd);
sText = Util.RegExpReplaceCase(sText, @" +\n", "\n");
sText = Util.RegExpReplaceCase(sText, "([^\n])\n([^\n])", "$1 $2");
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart + sText.Length;
Util.Say(rtb.RowText);
}

if (menuItem == menuEditHardLineBreak) {
HardLineBreak();
}

if (menuItem == menuEditEnterNewLine|| menuItem == menuEditIndentNewLine) {
SetStatus("");
if ((!rtb.IndentMode && menuItem == menuEditEnterNewLine) || (rtb.IndentMode && menuItem == menuEditIndentNewLine)) {
// Reduce verbosity
// if (rtb.IndentMode) AddMessage("Enter New Line");
// else SetStatus("Enter New Line");
SetStatus("Indent New Line");
sText = "\n";
iIndex = rtb.Index;
rtb.ReplaceRange(iIndex, iIndex, sText);
rtb.Index = iIndex + sText.Length;
}
else {
SetStatus("Indent New Line");
// Reduce verbosity
// AddMessage("Indent New Line");
sText = rtb.RowText;
iIndex = rtb.RowStart + sText.Length;
sMatch = @"^(\s*).*";
sReplace = "$1";
sText = Util.RegExpReplaceCase(sText, sMatch, sReplace);
sText = "\n" + sText;
rtb.ReplaceRange(iIndex, iIndex, sText);
rtb.Index = iIndex + sText.Length;
AddMessage("Level " + this.GetIndent());
// Reduce verbosity
// Util.Say(rtb.RowText);
}
}

if (menuItem == menuEditIndentNewLinePrior) {
sText = rtb.RowText;
iIndex = rtb.RowStart;
sMatch = @"^(\s*).*";
sReplace = "$1";
sText = Util.RegExpReplaceCase(sText, sMatch, sReplace);
sText = sText + "\n";
rtb.ReplaceRange(iIndex, iIndex, sText);
rtb.Index--;
AddMessage("Level " + this.GetIndent());
// Reduce verbosity
// Util.Say(rtb.RowText);
}

if (menuItem == menuEditIndent) {
string sIndent = App.ReadOption("IndentUnit", "  ");
sIndent = Util.Literalize(sIndent);
iIndex = rtb.Index;
bool bLine;
if (rtb.SelectionLength == 0) {
// AddMessage("Line");
iStart = rtb.RowStart;
iEnd = iStart + rtb.RowText.Length;
bLine = true;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
bLine = false;
}

sText = rtb.GetRange(iStart, iEnd);
sText = Util.RegExpReplaceCase(sText, "^", sIndent);
rtb.ReplaceRange(iStart, iEnd, sText);

//if (bLine) rtb.Index = iIndex;
if (bLine) rtb.Index = iIndex + sIndent.Length;
else rtb.Index  = iIndex + sText.Length;
AddMessage("Level " + this.GetIndent());
//Util.Say(rtb.RowText);
}

if (menuItem == menuEditOutdent) {
string sIndent = App.ReadOption("IndentUnit", "  ");
sIndent = Util.Literalize(sIndent);
iIndex = rtb.Index;
if (rtb.SelectionLength == 0) {
// AddMessage("Line");
iStart = rtb.RowStart;
iEnd = iStart + rtb.RowText.Length;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

sText = rtb.GetRange(iStart, iEnd);
iLength = sText.Length;
sText = Util.RegExpReplaceCase(sText, "^" + sIndent, "");
rtb.ReplaceRange(iStart, iEnd, sText);
if (sText.Length < iLength) rtb.Index = iIndex - sIndent.Length;
AddMessage("Level " + this.GetIndent());
//Util.Say(rtb.RowText);
}

if (menuItem == menuEditAlign) {
string sIndent = App.ReadOption("IndentUnit", "  ");
sIndent = Util.Literalize(sIndent);
iIndex = rtb.Index;
bool bLine;
if (rtb.SelectionLength == 0) {
AddMessage("Line");
iStart = rtb.RowStart;
iEnd = iStart + rtb.RowText.Length;
bLine = true;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
bLine = false;
}

char[] a = {' ', '\t'};
sLine = "";
string sComment = App.ReadOption("QuotePrefix", "> ");
int iRow = rtb.GetIndexRow(iStart);
// Dialog.Show("iStart " + iStart, "iRow " + iRow);
int iLevels = GetIndent(iRow);
int i = iLevels;
int iTop = 0;
while (iRow > iTop) {
iRow--;
// Dialog.Show("row " + iRow);
sLine = rtb.GetRowText(iRow).Trim(a);
if (sLine.Length == 0 || sLine.StartsWith(sComment)) continue;
i = GetIndent(iRow);
if (iLevels != i) break;
}

int iDelta = i - iLevels;
// Dialog.Show(iDelta);

sText = rtb.GetRange(iStart, iEnd);
if (iDelta > 0) {
for (i = 0; i < iDelta; i++) {
iLength = sText.Length;
sText = Util.RegExpReplaceCase(sText, "^", sIndent);
if (sText.Length > iLength) iIndex += sIndent.Length;
}
}
else {
iDelta = iDelta * -1;
for (i = 0; i < iDelta; i++) {
iLength = sText.Length;
sText = Util.RegExpReplaceCase(sText, "^" + sIndent, "");
if (sText.Length < iLength) iIndex -= sIndent.Length;
}
}

rtb.ReplaceRange(iStart, iEnd, sText);
bLine = !bLine;
rtb.Index = iIndex;
AddMessage("Level " + this.GetIndent());
}

if (menuItem == menuEditIndentMode) {
rtb.IndentMode = !rtb.IndentMode;
AddMessage(rtb.IndentMode ? "On" : "Off");
// Environment.SetEnvironmentVariable("EdSharpIndent", "", EnvironmentVariableTarget.User);
bool b = System.IO.File.Exists(App.IndentModeFile);
if (b && !rtb.IndentMode) System.IO.File.Delete(App.IndentModeFile);
else if (!b && rtb.IndentMode) System.IO.File.Create(App.IndentModeFile).Close();
//return;
}

if (menuItem == menuEditJustify) {
if (rtb.SelectionLength == 0) {
sTitle = "Justify Cursor";
}
else {
sTitle = "Justify Selected";
}

aValues = new string[] {"&Left", "&Bullet", "&Center", "&Right"};
int i = 0;
if (rtb.SelectionBullet) i = 1;
if (rtb.SelectionAlignment == HorizontalAlignment.Center) i = 2;
else if (rtb.SelectionAlignment == HorizontalAlignment.Right) i = 3;
sResult = Dialog.Choose(sTitle, "", aValues, i);
if (sResult.Length == 0) return;

rtb.SelectionBullet = false;
switch (sResult) {
case "&Left" :
rtb.SelectionAlignment = HorizontalAlignment.Left;
break;
case "&Bullet" :
rtb.SelectionBullet = true;
break;
case "&Center" :
rtb.SelectionAlignment = HorizontalAlignment.Center;
break;
case "&Right" :
rtb.SelectionAlignment = HorizontalAlignment.Right;
break;
}
}

if (menuItem == menuEditStyle) {
if (rtb.SelectionLength == 0) {
sTitle = "Style Cursor";
}
else {
sTitle = "Style Selected";
}

aValues = new string[] {"Bold", "Italic", "Underline"};
List<int> listSelect = new List<int>();
if (rtb.SelectionFont.Bold) listSelect.Add(0);
if (rtb.SelectionFont.Italic) listSelect.Add(1);
if (rtb.SelectionFont.Underline) listSelect.Add(2);
int[] aSelect = listSelect.ToArray();

//aResults = Dialog.MultiPick(sTitle, aValues, aSelect, false);
aResults = Dialog.MultiCheck(sTitle, aValues, aSelect, false, 0);
if (aResults.Length == 0) return;

if (!listSelect.Contains(0) && Array.IndexOf(aResults, "Bold") >= 0) rtb.SelectionFont = Util.SetBold(rtb.SelectionFont, true);
if (listSelect.Contains(0) && Array.IndexOf(aResults, "Bold") < 0) rtb.SelectionFont = Util.SetBold(rtb.SelectionFont, false);
if (!listSelect.Contains(0) && Array.IndexOf(aResults, "Italic") >= 0) rtb.SelectionFont = Util.SetItalic(rtb.SelectionFont, true);
if (listSelect.Contains(0) && Array.IndexOf(aResults, "Italic") < 0) rtb.SelectionFont = Util.SetItalic(rtb.SelectionFont, false);
if (!listSelect.Contains(0) && Array.IndexOf(aResults, "Underline") >= 0) rtb.SelectionFont = Util.SetUnderline(rtb.SelectionFont, true);
if (listSelect.Contains(0) && Array.IndexOf(aResults, "Underline") < 0) rtb.SelectionFont = Util.SetUnderline(rtb.SelectionFont, false);
}

if (menuItem == menuEditBaseline) {
if (rtb.SelectionLength == 0) {
sTitle = "Baseline Cursor";
}
else {
sTitle = "Baseline Selected";
}

aValues = new string[] {"&Down", "&Flat", "&Up"};
int i = 1;
if (rtb.SelectionCharOffset < 0) i = 0;
else if (rtb.SelectionCharOffset > 0) i = 2;
sResult = Dialog.Choose(sTitle, "", aValues, i);
if (sResult.Length == 0) return;

switch (sResult) {
case "&Down" :
rtb.SelectionCharOffset = -4;
break;
case "&Flat" :
rtb.SelectionCharOffset = 0;
break;
case "&Up" :
rtb.SelectionCharOffset = 4;
break;
}
}

if (menuItem == menuEditSetSelectionFont) {
if (rtb.SelectionLength == 0) {
AddMessage("Cursor");
}
else {
AddMessage("Selected");
}

object[] a = Dialog.GetFont(rtb.SelectionFont, rtb.SelectionColor);
if (a.Length == 0) return;

rtb.SelectionFont = (Font) a[0];
rtb.SelectionColor = (Color) a[1];
}

if (menuItem == menuMiscEnvironmentVariables) {
string sChoice = Dialog.Choose("Target", "", new string[] {"&Process", "&User", "&Machine"}, 0);
if (sChoice.Length == 0) return;

EnvironmentVariableTarget target = EnvironmentVariableTarget.Process;
if (sChoice == "&User") target = EnvironmentVariableTarget.User;
else if (sChoice == "&Machine") target = EnvironmentVariableTarget.Machine;

IDictionary dic = Environment.GetEnvironmentVariables(target);
iCount = dic.Count;
aLabels = new string[iCount];
aValues = new string[iCount];
string[] aKeys = new string[iCount];

int iKey = 0;
foreach (DictionaryEntry de in dic) {
aKeys[iKey] = ((string) de.Key).ToLower();
aLabels[iKey] = "&" + (string) de.Key;
iKey++;
}
Array.Sort(aKeys, aLabels);

iKey = 0;
foreach (DictionaryEntry de in dic) {
aKeys[iKey] = ((string) de.Key).ToLower();
aValues[iKey] = (string) de.Value;
iKey++;
}
Array.Sort(aKeys, aValues);

sTitle = "Variables for ";
if (sChoice == "&Process") sTitle += "Process " + Process.GetCurrentProcess().ProcessName;
else if (sChoice == "&User") sTitle += "User " + Environment.UserName;
else if (sChoice == "&Machine") sTitle += "Machine " + Environment.MachineName;
aResults = Dialog.MultiInput(sTitle, aLabels, aValues);
if (aResults.Length == 0) return;

try {
for (int i = 0; i < iCount; i++) {
if (aResults[i] == aValues[i]) continue;

if (Dialog.Confirm("Confirm", "Change " + aKeys[i] + "?", "Y") != "Y") continue;
Environment.SetEnvironmentVariable(aKeys[i], aResults[i], target);
}
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}
AddMessage("Done!");
}

if (menuItem == menuMiscSpellCheck) {
SpellCheck();
}

if (menuItem == menuMiscExtraSpeechToggle) {
SetStatus("");
bool b = !App.ExtraSpeech;
App.ExtraSpeech = true;
AddMessage("Extra Speech");
AddMessage(b ? "On" : "Off");
App.ExtraSpeech = b;
App.WriteOption("E&xtraSpeech", (b ? "Y" : "N"));
}

if (menuItem == menuMiscExtraSpeechLog) {
OpenOrActivateWindow(App.SpeechLog, 0);
}

if (menuItem == menuMiscThesaurus) {
Thesaurus();
}

if (menuItem == menuMiscLookupTerm) {
if (this.Child != null) {
if (rtb.SelectionLength == 0) {
//AddMessage("Chunk");
object[] a = GetChunk();
iStart = (int) a[0];
sText = (string) a[1];
}
else {
//AddMessage("Selected");
iStart = rtb.SelectionStart;
sText = rtb.SelectedText;
iEnd = iStart + sText.Length;
}

sText = sText.TrimEnd();
}
else sText = "";

if (sText.Length == 0) sText = App.ReadData("Term", "");
sResult = Dialog.Input("Lookup", "Term", sText).Trim();
if (sResult.Length == 0) return;

App.WriteData("Term", sResult);
//AddMessage("Please wait");
AddMessage("Connecting");
sText = VB.LookupTerm(sResult);
if (!IsEmptyWindow()) new MdiChild(this);
child = this.Child;
child.Text = sResult + ".txt";
child.File = child.Text;
rtb = child.RTB;
rtb.Text = sText;
}

if (menuItem == menuMiscTranslateLanguage) {
if (rtb.SelectionLength == 0) {
iStart = 0;
iEnd = rtb.TextLength;
}
else {
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}
sText = rtb.GetRange(iStart, iEnd);

string[] aLanguageNames, aLanguageAbbreviations;
Util.GetGoogleLanguages(out aLanguageNames, out aLanguageAbbreviations);
string sSourceLanguage = Dialog.Pick("Source Language", aLanguageAbbreviations, aLanguageNames, false, 0);
if (sSourceLanguage.Length == 0) return;

string sTargetLanguage = Dialog.Pick("Target Language", aLanguageAbbreviations, aLanguageNames, false, 0);
if (sTargetLanguage.Length == 0) return;

string sExe = App.ProgramDir + @"\Convert\TranLang.exe";
string sSourceFile = App.TempFile;
Encoding en = Encoding.UTF8;
en = null;
Util.String2File(sText, sSourceFile, ref en);
string sTargetFile = sSourceFile;
string sCommand = Util.Quote(sExe) + " " + sSourceLanguage + " " + Util.Quote(sSourceFile) + " " + sTargetLanguage + " " + Util.Quote(sTargetFile);
Util.RunHideWait(sCommand);
en = Encoding.UTF8;
// en = null;
sText = Util.File2String(sTargetFile, ref en);
File.Delete(sSourceFile);
File.Delete(sTargetFile);

if (!IsEmptyWindow()) new MdiChild(this);
child = this.Child;
// child.Text = sResult + ".txt";
child.File = child.Text;
rtb = child.RTB;
rtb.Text = sText;

}

if (menuItem == menuMiscGuardDocument) {
rtb.SetGuard(true);
SetRecent(child.File);
}

if (menuItem == menuMiscNoGuard) {
rtb.SetGuard(false);
SetRecent(child.File);
}

if (menuItem == menuMiscPyBrace) {
if (rtb.SelectionLength == 0) {
AddMessage("All");
iStart = 0;
iEnd = rtb.TextLength;
sFile = Path.GetFileNameWithoutExtension(child.Text);
if (Path.GetExtension(child.Text).ToLower() == ".boo") sFile += ".bob";
else sFile += ".pyb";
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
sFile = "";
}
sText = rtb.GetRange(iStart, iEnd);
sText = PyDent2Brace(sText);

if (sFile.Length == 0) {
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
}
else {
child = new MdiChild(App.Frame, sFile);
Child.RTB.Text = sText;
child.RTB.Modified = true;
}
AddMessage("Done!");
}

if (menuItem == menuMiscPyDent) {
sFile = child.File;
string sExt = Path.GetExtension(sFile).ToLower();
sFile = Path.GetFileNameWithoutExtension(sFile);
if (sExt == ".bob") sFile += ".boo";
else sFile += ".py";

if (rtb.SelectionLength == 0) {
AddMessage("All");
iStart = 0;
iEnd = rtb.TextLength;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
sFile = "";
}
sText = rtb.GetRange(iStart, iEnd);

//if (sExt != ".bob" && sExt != ".pyb") sText = PyDent2Brace(sText);
if (sExt == ".boo" || sExt == ".py") sText = PyDent2Brace(sText);
sText = PyBrace2Dent(sText);

if (sFile.Length == 0) {
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
}
else {
child = new MdiChild(App.Frame, sFile);
Child.RTB.Text = sText;
child.RTB.Modified = true;
}
AddMessage("Done!");
}

if (menuItem == menuMiscInferIndent) {
sText = rtb.Text;
aResults = Util.RegExpExtractCase(sText, @"^( |\t)+");
if (aResults.Length == 0) {
AddMessage("No indentation found!");
return;
}

string sIndent = aResults[0];
if (this.KeyRepeat % 2 == 0) {
AddMessage("Infer Indent");
if (sIndent.Contains(" ") && sIndent.Contains("\t")) {
foreach (char c in sIndent) {
if (c == ' ') AddMessage("Space");
else AddMessage("Tab");
}
}
else {
if (sIndent.StartsWith(" ")) sText = "space";
else sText = "tab";
AddMessage(Util.Pluralize(sIndent.Length, sText));
}
}
else {
sIndent = sIndent.Replace(" ", @"\040");
sIndent = sIndent.Replace("\t", @"\t");
App.WriteOption("IndentUnit", sIndent);
AddMessage("IndentUnit configured");
}
}

if (menuItem == menuMiscFormatCode) {
// String sExe = Path.Combine(App.ProgramDir, @"Convert\Uncrustify\uncrustify.exe");
sFile = App.Frame.Child.File;
string sExt = Path.GetExtension(sFile).ToLower();
string sCommand = "";
if (sExt.Contains(sExt)) {
sCommand = "%ProgDir%\\Convert\\Tidy\\tidy.exe -config %ProgDir%\\Convert\\Tidy\\tidy.cfg -m \"%SourceLong%\"";
sCommand = Util.ExpandCommandLine(sCommand, sFile, sFile);
Util.RunHideWait(sCommand);
sText = File.ReadAllText(sFile);
}
else {
String sExe = Path.Combine(App.ProgramDir, @"Convert\astyle\astyle.exe");
sExe = Win32.GetShortPath(sExe);
String sCfg = Path.Combine(App.ProgramDir, @"Convert\Uncrustify\defaults.cfg");
sCfg = Win32.GetShortPath(sCfg);

string sSourceFile = Path.GetTempFileName();
sSourceFile = Path.ChangeExtension(sSourceFile, Path.GetExtension(App.Frame.Child.File));
File.WriteAllText(sSourceFile, App.Frame.Child.RTB.Text);
if (File.Exists(App.TempFile)) File.Delete(App.TempFile);
File.Copy(sSourceFile, App.TempFile);
string sTargetFile = Path.GetTempFileName();
// string sCommand = sExe + " -c " + sCfg + " -f " + sSourceFile + " -o " + sTargetFile;

        string sIndent = App.ReadOption("IndentUnit", "  ");
        sIndent = Util.Literalize(sIndent);
sCommand = sExe + " " + sSourceFile;
if (sIndent == "\t") sCommand = sExe + " --indent=tab " + sSourceFile;
Util.RunHideWait(sCommand);
// sText = File.ReadAllText(sTargetFile);
sText = File.ReadAllText(sSourceFile);
File.Delete(sSourceFile);
File.Delete(sTargetFile);
} // if html file

if (sText.Length == 0) {
AddMessage(" Error !");
return;
}

App.Frame.Child.RTB.Text = sText;
App.Frame.Child.RTB.Modified = true;
AddMessage(" Done !");
}

if (menuItem == menuMiscRepeatLine) {
sText = CR + rtb.RowText;
iIndex = rtb.RowStart + rtb.RowText.Length;
rtb.ReplaceRange(iIndex, iIndex, sText);
rtb.Index = iIndex + 1;
Util.Say(rtb.RowText);
}

if (menuItem == menuMiscSectionBreak) {
rtb.ReplaceRange(rtb.Index, rtb.Index, SectionBreak);
Util.Say(rtb.RowText);
}

if (menuItem == menuDeleteReplaceRegular) {
if (rtb.SelectionLength == 0) {
sTitle = "Replace All";
iStart = 0;
iEnd = rtb.TextLength;
}
else {
sTitle = "Replace Selected";
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

aLabels = new string[] {"&Match", "&Replace"};
sMatch = App.ReadData("Match", "");
sReplace = App.ReadData("Replace", "");
aValues = new string[] {sMatch, sReplace};
aResults = Dialog.MultiInput(sTitle, aLabels, aValues);
if (aResults == null || aResults.Length == 0) return;

sMatch = aResults[0];
App.WriteData("Match", sMatch);
sMatch = Util.Literalize(sMatch, true);
sMatch = Regex.Escape(sMatch);
sReplace = aResults[1];
App.WriteData("Replace", sReplace);
sReplace = Util.Literalize(sReplace, true);

sText = rtb.GetRange(iStart, iEnd);
iCount = Util.RegExpCountEquiv(sText, sMatch);
sText = Util.RegExpReplaceEquiv(sText, sMatch, sReplace);
if (iCount > 0) rtb.ReplaceRange(iStart, iEnd, sText);
AddMessage(Util.Pluralize(iCount, "match", "matches"));
}

if (menuItem == menuDeleteReplaceWithRegExp) {
if (rtb.SelectionLength == 0) {
sTitle = "Replace All with Regular Expression";
iStart = 0;
iEnd = rtb.TextLength;
}
else {
sTitle = "Replace Selected with Regular Expression";
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

aLabels = new string[] {"&Pattern", "&Substitute"};
sPattern = App.ReadData("Pattern", "");
sSubstitute = App.ReadData("Substitute", "");
aValues = new string[] {sPattern, sSubstitute};
aResults = Dialog.MultiInput(sTitle, aLabels, aValues);
if (aResults == null || aResults.Length == 0) return;

sPattern = aResults[0];
App.WriteData("Pattern", sPattern);
//sPattern = Util.Literalize(sPattern);
sSubstitute = aResults[1];
App.WriteData("Substitute", sSubstitute);
sSubstitute = Util.Literalize(sSubstitute);
sText = rtb.GetRange(iStart, iEnd);
iCount = Util.RegExpCountCase(sText, sPattern);
sText = Util.RegExpReplaceCase(sText, sPattern, sSubstitute);
if (iCount > 0) rtb.ReplaceRange(iStart, iEnd, sText);
AddMessage(Util.Pluralize(iCount, "match", "matches"));
}

if (menuItem == menuMiscYieldWithRegExp) {
if (rtb.SelectionLength == 0) {
sTitle = "Yield All with Regular Expression";
iStart = 0;
iEnd = rtb.TextLength;
}
else {
sTitle = "Yield Selected with Regular Expression";
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

sLabel = "Pattern";
sValue = App.ReadData("Pattern", "");
sResult = Dialog.Input(sTitle, sLabel, sValue);
if (sResult.Length == 0) return;

App.WriteData("Pattern", sResult);
//sResult = Util.Literalize(sResult);
sText = rtb.GetRange(iStart, iEnd);
iCount = Util.RegExpCountCase(sText, sResult);
AddMessage(Util.Pluralize(iCount, "match", "matches"));
}

if (menuItem == menuMiscExtractWithRegExp) {
if (rtb.SelectionLength == 0) {
sTitle = "Extract All with Regular Expression";
iStart = 0;
iEnd = rtb.TextLength;
}
else {
sTitle = "Extract Selected with Regular Expression";
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

sLabel = "Pattern";
sValue = App.ReadData("Pattern", "");
sResult = Dialog.Input(sTitle, sLabel, sValue);
if (sResult.Length == 0) return;

App.WriteData("Pattern", sResult);
//sResult = Util.Literalize(sResult);
sText = rtb.GetRange(iStart, iEnd);
aResults = Util.RegExpExtractCase(sText, sResult);
iCount = aResults.Length;
AddMessage(Util.Pluralize(iCount, "match", "matches"));
if (iCount == 0) return;

new MdiChild(this);
rtb = App.Frame.Child.RTB;
sText = String.Join(SectionBreak, aResults);
rtb.ReplaceRange(0, 0, sText);
rtb.Index = 0;
}

if (menuItem == menuMiscRunAtCursor) {
if (rtb.SelectionLength == 0) {
sTitle = "Run Chunk at Cursor";
object[] a = GetChunk();
sText = (string) a[1];
}
else {
sTitle = "Run Selected at Cursor";
sText = rtb.SelectedText;
}

sLabel = "Path";
sReplace = "";
sMatch = "(\r|\n)";
sText = Util.RegExpReplaceCase(sText, sMatch, sReplace);
sMatch = "^(\\<| )+";
sText = Util.RegExpReplaceCase(sText, sMatch, sReplace);
sMatch = "(\\>| |\\.)+$";
sText = Util.RegExpReplaceCase(sText, sMatch, sReplace);

if (sText.Contains("://")) sText = sText.Trim(); //do nothing
else if (sText.ToLower().StartsWith("www.")) sText = "http://" + sText;
else if (sText.Contains("@") && !sText.ToLower().StartsWith("mailto")) sText = "MailTo:" + sText;

sResult = Dialog.Input(sTitle, sLabel, sText).Trim();
if (sResult.Length == 0) return;
Process.Start(sResult);
}

if (menuItem == menuMiscSpecialCharacter) {
string sCode = App.ReadData("Code", "");
sResult = Dialog.Input("Special Character", "Code:", sCode).Trim().ToLower();
if (sResult.Length == 0) return;

App.WriteData("Code", sResult);
if (sResult.StartsWith(@"\")) sResult = sResult.Remove(0, 1);
if (sResult.StartsWith("d")) {
sResult = sResult.Remove(0, 1);
try {
int iCode = Int32.Parse(sResult);
sText = Util.Code2String(iCode);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}
}
else {
if (sResult.StartsWith("u")) sResult = sResult.Remove(0, 1);
string s = sResult;
sText = Util.Literalize(@"\u" + sResult.PadLeft(4, '0'));
if (sText.Length == 0 || sText == "\u0000") {
Dialog.Show("Error", "Invalid Unicode number");
return;
}
}

iIndex = rtb.Index;
rtb.ReplaceRange(iIndex, iIndex, sText);
rtb.Index = iIndex + 1;
AddMessage(sText);
}

if (menuItem == menuDeleteHardLine) {
iStart = rtb.RowStart;
iEnd = rtb.Text.IndexOf("\n", iStart);
if (iEnd >= 0) iEnd++;
else iEnd = rtb.TextLength;
rtb.ReplaceRange(iStart, iEnd, "");
rtb.Index = iStart;
Util.Say(rtb.RowText);
}

if (menuItem == menuDeleteParagraph) {
iStart = rtb.RowStart;
sMatch = @"\n\s*\n";
object[] a = Util.RegExpContainsCase(rtb.Text, sMatch, iStart);
iEnd = (int) a[0];
//Dialog.Show(iEnd, ((string) a[1]).Length);
if (iEnd >= 0) iEnd += ((string) a[1]).Length;
else iEnd = rtb.TextLength;
rtb.ReplaceRange(iStart, iEnd, "");
rtb.Index = iStart;
Util.Say(rtb.RowText);
}

if (menuItem == menuDeleteLine) {
iStart = rtb.RowStart;
iEnd = iStart + rtb.RowLength;
rtb.ReplaceRange(iStart, iEnd, "");
rtb.Index = iStart;
Util.Say(rtb.RowText);
}

if (menuItem == menuDeleteRight) {
iStart = rtb.Index;
iEnd = rtb.RowStart + rtb.RowText.Length;
//if (iEnd != rtb.TextLength) iEnd--;
rtb.ReplaceRange(iStart, iEnd, "");
rtb.Index = iStart;
Util.Say(rtb.RowText);
}

if (menuItem == menuDeleteLeft) {
iStart = rtb.RowStart;
iEnd = rtb.Index;
rtb.ReplaceRange(iStart, iEnd, "");
rtb.Index = iStart;
Util.Say(rtb.RowText);
}

if (menuItem == menuDeleteDown) {
iStart = rtb.Index;
iEnd = rtb.TextLength;
rtb.ReplaceRange(iStart, iEnd, "");
rtb.Index = iStart;
Util.Say(rtb.RowText);
}

if (menuItem == menuDeleteUp) {
iStart = 0;
iEnd = rtb.Index;
rtb.ReplaceRange(iStart, iEnd, "");
rtb.Index = iStart;
Util.Say(rtb.RowText);
}

if (menuItem == menuDeleteFile) {
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

if (Dialog.Confirm("Confirm", "Delete " + child.Text + "?", "N") != "Y") return;
File.Delete(sFile);
child.Close();
}

if (menuItem == menuDeleteTrimBlanks) {
if (rtb.SelectionLength == 0) {
AddMessage("Line");
iStart = rtb.RowStart;
iEnd = iStart + rtb.RowText.Length;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

sText = rtb.GetRange(iStart, iEnd);
sText = Util.RegExpReplaceCase(sText, "^( |\t)+", "");
sText = Util.RegExpReplaceCase(sText, "( |\t)+$", "");
sText = Util.RegExpReplaceCase(sText, "\n\n\n\n+", "\n\n\n");
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigateForwardFind || menuItem == menuNavigateForwardFindAtCursor || (!this.FindWithRegExp && menuItem == menuNavigateForwardFindAgain)) {
iIndex = rtb.Index;
iStart = 0;
sText = App.ReadData("Find", "");
if (menuItem == menuNavigateForwardFind) sText = Dialog.Input("Forward Find", "Text", sText);
else if (menuItem == menuNavigateForwardFindAtCursor) {
if (rtb.SelectionLength == 0) {
object[] a = GetChunk();
iStart = (int) a[0];
sText = (string) a[1];
}
else {
iStart = rtb.SelectionStart;
sText = rtb.SelectedText;
}
}
if (sText.Length == 0) return;

App.WriteData("Find", sText);
this.FindWithRegExp = false;
sText = Util.Literalize(sText, true);
sText = Util.Convert2MacLineBreak(sText);

if (menuItem == menuNavigateForwardFindAtCursor) {
iStart += sText.Length;
iEnd = -1;
}
else if (rtb.SelectionLength == 0) {
iStart = iIndex;
iEnd = -1;
}
else {
iStart = rtb.SelectionStart;
iEnd = rtb.SelectionStart + rtb.SelectionLength;
}

iIndex = rtb.Find(sText, iStart, iEnd, RichTextBoxFinds.NoHighlight);
if (iIndex >= 0) {
rtb.Index = iIndex + sText.Length;
Util.Say(rtb.RowText);
}
else AddMessage("Not found!");
}

if (menuItem == menuNavigateReverseFind || menuItem == menuNavigateReverseFindAtCursor || (!this.FindWithRegExp && menuItem == menuNavigateReverseFindAgain)) {
iIndex = rtb.Index;
iEnd = 0;
sText = App.ReadData("Find", "");
if (menuItem == menuNavigateReverseFind) sText = Dialog.Input("Reverse Find", "Text", sText);
else if (menuItem == menuNavigateReverseFindAtCursor) {
if (rtb.SelectionLength == 0) {
object[] a = GetChunk();
iEnd = (int) a[0];
sText = (string) a[1];
}
else {
iEnd = rtb.SelectionStart;
sText = rtb.SelectedText;
}
}
if (sText.Length == 0) return;

App.WriteData("Find", sText);
this.FindWithRegExp = false;
sText = Util.Literalize(sText, true);
sText = Util.Convert2MacLineBreak(sText);

if (menuItem == menuNavigateReverseFindAtCursor) {
iStart = 0;
}
else if (rtb.SelectionLength == 0) {
iStart = 0;
iEnd = iIndex;
}
else {
iStart = rtb.SelectionStart;
iEnd = rtb.SelectionStart + rtb.SelectionLength;
}

iIndex = rtb.Find(sText, iStart, iEnd, RichTextBoxFinds.Reverse | RichTextBoxFinds.NoHighlight);
//if (iIndex >= 0) {
if (iIndex >= 0 && iIndex < iEnd) {
rtb.Index = iIndex;
Util.Say(rtb.RowText);
}
else AddMessage("Not found!");
}

if (menuItem == menuNavigateForwardFindWithRegExp || (this.FindWithRegExp && menuItem == menuNavigateForwardFindAgain)) {
sMatch = App.ReadData("Pattern", "");
if (menuItem == menuNavigateForwardFindWithRegExp) sMatch = Dialog.Input("Forward Find with Regular Expression", "Pattern", sMatch);
if (sMatch.Length == 0) return;

App.WriteData("Pattern", sMatch);
this.FindWithRegExp = true;

if (rtb.SelectionLength == 0) {
iStart = rtb.Index;
iEnd = rtb.TextLength;
}
else {
iStart = rtb.SelectionStart;
iEnd = rtb.SelectionStart + rtb.SelectionLength;
}
sText = rtb.GetRange(iStart, iEnd);

object[] a = Util.RegExpContainsCase(sText, sMatch);
iIndex = (int) a[0];
if (iIndex >= 0) {
sValue = (string) a[1];
rtb.Index = iStart + iIndex + sValue.Length;
Util.Say(rtb.RowText);
}
else AddMessage("Not found!");
}

if (menuItem == menuNavigateReverseFindWithRegExp || (this.FindWithRegExp && menuItem == menuNavigateReverseFindAgain)) {
sMatch = App.ReadData("Pattern", "");
if (menuItem == menuNavigateReverseFindWithRegExp) sMatch = Dialog.Input("Reverse Find with Regular Expression", "Pattern", sMatch);
if (sMatch.Length == 0) return;

App.WriteData("Pattern", sMatch);
this.FindWithRegExp = true;

if (rtb.SelectionLength == 0) {
iStart = 0;
iEnd = rtb.Index;
}
else {
iStart = rtb.SelectionStart;
iEnd = rtb.SelectionStart + rtb.SelectionLength;
}
sText = rtb.GetRange(iStart, iEnd);

object[] a = Util.RegExpContainsLastCase(sText, sMatch);
iIndex = (int) a[0];
if (iIndex >= 0) {
sValue = (string) a[1];
rtb.Index = iStart + iIndex;
Util.Say(rtb.RowText);
}
else AddMessage("Not found!");
}

if (menuItem == menuNavigateJumpToLine || menuItem == menuNavigateJumpToLineAgain) {
sText = App.ReadData("Jump", "");
if (menuItem == menuNavigateJumpToLine) sText = Dialog.Input("Jump to", "Line", sText);
if (sText.Length == 0) return;

string[] a = sText.Split(',');
sLine = a[0].Trim();
if (sLine.Length == 0) sLine = rtb.Line.ToString();
string sColumn = a.Length > 1 ? a[1].Trim() : "1";

try {
iLine = Int32.Parse(sLine);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}

int iColumn = 1;
try {
iColumn = Int32.Parse(sColumn);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}

App.WriteData("Jump", sText);
try {
rtb.Line = iLine;
rtb.Column = iColumn;
Util.Say(rtb.RowText);
}
catch {
Dialog.Show("Error", "Invalid position!");
return;
}
}
if (menuItem == menuNavigateSearchForTopic || menuItem == menuNavigateSearchForTopicAgain) {
sText = App.ReadData("Topic", "");
if (menuItem == menuNavigateSearchForTopic) {
sText = Dialog.Input("Search For", "Topic", sText);
iStart = 0;
}
else iStart = rtb.Index;
if (sText.Length == 0) return;

App.WriteData("Topic", sText);
sMatch = SB + ".*?" + sText + ".*?" + LB;
sText = rtb.Text;
iIndex = (int) Util.RegExpContainsEquiv(sText, sMatch, iStart)[0];
if (iIndex == -1) {
AddMessage("Not found!");
return;
}
rtb.Index = iIndex + SB.Length;
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigateGoToPercent || menuItem == menuNavigateGoToPercentAgain) {
sText = App.ReadData("Percent", "");
if (menuItem == menuNavigateGoToPercent) sText = Dialog.Input("Go to", "Percent", sText);
if (sText.Length == 0) return;

try {
iPercent = Int32.Parse(sText);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}

App.WriteData("Percent", sText);
rtb.Percent = iPercent;
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigateGoToPart) {
sText = rtb.Text;
sMatch = @"^\s*((Chapter)|(Section)|(Part))\s+\d";
sMatch = App.ReadOption("NavigatePart", sMatch);
object[][] aMatches = Util.RegExpExtractWithIndex(sText, sMatch, false);
if (aMatches.Length == 0) {
AddMessage("No matches for NavigatePart expression!");
return;
}

iIndex = rtb.Index;
int iPosition = 0;
HomerList lNames = new HomerList();
HomerList lValues = new HomerList();
for (int i = 0; i < aMatches.Length; i++) {
object[] a = (object[]) aMatches[i];
string sIndex = ((int) a[0]).ToString();
string sPart = (string) a[1];
if (iIndex >= Int32.Parse(sIndex)) iPosition = i;
lNames.Add(sPart);
lValues.Add(sIndex);
}

string[] aNames = lNames.ToArray();
aValues = lValues.ToArray();
string s = Dialog.Pick("Go to Part", aValues, aNames, false, iPosition);
if (s.Length == 0) return;

iIndex = Int32.Parse(s);
rtb.Index = iIndex;
}

if (menuItem == menuNavigateSetBookmark) {
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

sText = App.ReadValue("Favorites", sFile, "");
HomerList hl = new HomerList(sText);
hl.KeepLike(@"\d+");
hl.Remove("-1");
sText = rtb.Index + "|" + (rtb.ReadOnly ? "G" : "M") + "|" + (string) Util.If(rtb.WordWrap, "W", "U");
hl.AddUniqueRange(sText);
sText = hl.Segments;
App.WriteValue("Favorites", sFile, sText);
}

if (menuItem == menuNavigateClearBookmark) {
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

sText = App.ReadValue("Favorites", sFile, "");
if (sText.Length == 0) return;
HomerList hl = new HomerList(sText);
hl.Remove(rtb.Index.ToString());
sText = hl.Segments;
App.WriteValue("Favorites", sFile, sText);
}

if (menuItem == menuNavigateGoToBookmark) {
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

sText = App.ReadValue("Favorites", sFile, "");
HomerList hl = new HomerList(sText);
hl.KeepLike(@"\d+");
hl.Remove("-1");
if (hl.Count == 0) {
AddMessage("No bookmark!");
return;
}

if (hl.Count == 1) sResult = hl[0];
else {
hl.PadLeft(hl.MaxLength(), ' ');
hl.Sort();
HomerList hlLines = new HomerList();
foreach (string sIndex in hl) {
iIndex = Int32.Parse(sIndex);
int iRow = rtb.GetLineFromCharIndex(iIndex);
iStart = rtb.GetFirstCharIndexFromLine(iRow);
iEnd = rtb.GetFirstCharIndexFromLine(iRow + 1);
if (iEnd == -1) iEnd = rtb.TextLength;
sLine = rtb.GetRange(iStart, iEnd).Trim();
hlLines.Add(sLine);
}
string[] aDisplay = hlLines.ToArray();
aValues = hl.ToArray();
iIndex = rtb.Index;
int iDefault = -1;
for (int i = 0; i < hl.Count; i++) {
//Dialog.Show(iIndex, Int32.Parse(hl[i]));
if (iIndex < Int32.Parse(hl[i])) {
iDefault = i;
break;
}
}
//Dialog.Show(iDefault);

sResult = Dialog.Pick("Bookmarks", aValues, aDisplay, false, iDefault);
if (sResult.Length == 0) return;
}

rtb.Index = Int32.Parse(sResult);
Util.Say(rtb.RowText);
}

if (menuItem == menuFileSetFavorite) {
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

sText = App.ReadValue("Favorites", sFile, "");
HomerList hl = new HomerList(sText);
hl.KeepLike(@"\d+");
if (hl.Count == 0) hl.Add("-1");
sText = (rtb.ReadOnly ? "G" : "M") + "|" + (string) Util.If(rtb.WordWrap, "W", "U");
hl.AddUniqueRange(sText);
sText = hl.Segments;
App.WriteValue("Favorites", sFile, sText);
}

if (menuItem == menuFileClearFavorite) {
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

App.DeleteKey("Favorites", sFile);
}

if (menuItem == menuFileListFavorites) {
aResults = App.ReadSectionKeys("Favorites");
List<string> list = new List<string>(aResults);
for (int i = list.Count - 1; i >=0; i--) {
string s = list[i];
if (File.Exists(s)) continue;
App.DeleteKey("Favorites", s);
list.RemoveAt(i);
}

aResults = list.ToArray();
if (aResults.Length == 0) {
AddMessage("No items!");
return;
}

string[] aDisplay = new string[aResults.Length];
for (int i = 0; i < aDisplay.Length; i++) aDisplay[i] = Path.GetFileName(aResults[i]);
sFile = Dialog.Pick("List Favorites", aResults, aDisplay, true, 0);
if (sFile.Length == 0) return;

OpenOrActivateWindow(sFile, 0);
}

if (menuItem == menuNavigateGoToStartOfSelection) {
rtb.Index = rtb.StartSelection;
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigateHomeCharacter) {
sText = rtb.RowText;
iLength = sText.TrimStart().Length;
if (iLength == 0) return;
iIndex = rtb.RowStart + (sText.Length - iLength);
rtb.Index = iIndex;
sText = rtb.GetRange(iIndex, iIndex + 1);
AddMessage(sText);
}

if (menuItem == menuNavigateEndCharacter) {
sText = rtb.RowText;
iLength = sText.TrimEnd().Length;
if (iLength == 0) return;
iIndex = rtb.RowStart + iLength - 1;
rtb.Index = iIndex;
sText = rtb.GetRange(iIndex, iIndex + 1);
AddMessage(sText);
}

if (menuItem == menuNavigateStartTag) {
iIndex = rtb.Index;
iEnd = rtb.Text.IndexOf(">", rtb.Index);
if (iEnd == -1) {
//AddMessage("Not found!");
//return;
iEnd = iIndex - 1;
}

iStart = 0;
iEnd++;
sText = rtb.GetRange(iStart, iEnd);
sMatch = @"</?\w+( |>)";
object[] a = Util.RegExpContainsLastCase(sText, sMatch);
iStart = (int) a[0];
if (iStart == -1) {
AddMessage("Not found!");
return;
}

string sWord = (string) a[1];
if (sWord.IndexOf("/") == -1) {
if (iStart == iIndex) {
if (iIndex > 0) iIndex = sText.Substring(0, iIndex - 1).LastIndexOf("<");
if (iStart == 0 || iIndex < 0) {
AddMessage("Not found!");
return;
}
}
else iIndex = iStart;
}

else {
sWord = "<" + sWord.Substring(2, sWord.Length - 3);
sMatch = sWord + "( |>)";
iIndex = (int) Util.RegExpContainsEquiv(rtb.Text, sMatch)[0];
if (iIndex == -1) {
AddMessage("Not found!");
return;
}

}

rtb.Index = iIndex;
sText = (string) GetChunk()[1];
Util.Say(sText);
}

if (menuItem == menuNavigateEndTag) {
iIndex = rtb.Index;
iEnd = rtb.Text.IndexOf(">", rtb.Index);
if (iEnd == -1) {
AddMessage("Not found!");
return;
}

iStart = 0;
iEnd++;
sText = rtb.GetRange(iStart, iEnd);
sMatch = @"</?\w+( |>)";
object[] a = Util.RegExpContainsLastCase(sText, sMatch);
iStart = (int) a[0];
if (iStart == -1) {
AddMessage("Not found!");
return;
}

string sWord = (string) a[1];
iStart += sWord.Length - 1;
if (sWord.IndexOf("/") >= 0) {
if (iStart == iIndex) {
if (iIndex < rtb.TextLength - 1) iIndex = rtb.Text.IndexOf(">", iIndex + 1);
if (iStart == rtb.TextLength - 1 || iIndex < 0) {
AddMessage("Not found!");
return;
}
}
else iIndex = iStart;
}

else {
sMatch = "</" + sWord.Substring(1, sWord.Length -2) + ">";
a = Util.RegExpContainsEquiv(rtb.Text, sMatch, iEnd);
iIndex = (int) a[0];
if (iIndex == -1) {
AddMessage("Not found!");
return;
}

iIndex += ((string) a[1]).Length - 1;
}

rtb.Index = iIndex;
sText = (string) GetChunk()[1];
Util.Say(sText);
}

if (menuItem == menuNavigateNextJustify) {
HorizontalAlignment ha = rtb.SelectionAlignment;
bool bBullet = rtb.SelectionBullet;
iIndex = rtb.Index;
iEnd = rtb.TextLength;
while (iIndex < iEnd && rtb.SelectionAlignment == ha && rtb.SelectionBullet == bBullet) {
iIndex++;
rtb.Index = iIndex;
}
if (iIndex == iEnd) AddMessage("Bottom!");
if (rtb.SelectionAlignment != ha || rtb.SelectionBullet != bBullet) {
sText = GetJustifyText();
AddMessage(sText);
}
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigatePriorJustify) {
HorizontalAlignment ha = rtb.SelectionAlignment;
bool bBullet = rtb.SelectionBullet;
iIndex = rtb.Index;
iStart = 0;
while (iIndex > iStart && rtb.SelectionAlignment == ha && rtb.SelectionBullet == bBullet) {
iIndex--;
rtb.Index = iIndex;
}
if (iIndex == iStart) AddMessage("Top!");
if (rtb.SelectionAlignment != ha || rtb.SelectionBullet != bBullet) {
sText = GetJustifyText();
AddMessage(sText);
}
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigateNextStyle) {
bool bBold = rtb.SelectionFont.Bold;
bool bItalic = rtb.SelectionFont.Italic;
bool bUnderline = rtb.SelectionFont.Underline;
iIndex = rtb.Index;
iEnd = rtb.TextLength;
while (iIndex < iEnd && rtb.SelectionFont.Bold == bBold && rtb.SelectionFont.Italic == bItalic && rtb.SelectionFont.Underline == bUnderline) {
iIndex++;
rtb.Index = iIndex;
}

if (iIndex == iEnd) AddMessage("Bottom!");
if (!(rtb.SelectionFont.Bold == bBold && rtb.SelectionFont.Italic == bItalic && rtb.SelectionFont.Underline == bUnderline)) {
sText = GetStyleText();
AddMessage(sText);
}
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigatePriorStyle) {
bool bBold = rtb.SelectionFont.Bold;
bool bItalic = rtb.SelectionFont.Italic;
bool bUnderline = rtb.SelectionFont.Underline;
iIndex = rtb.Index;
iStart = 0;
while (iIndex > iStart && rtb.SelectionFont.Bold == bBold && rtb.SelectionFont.Italic == bItalic && rtb.SelectionFont.Underline == bUnderline) {
iIndex--;
rtb.Index = iIndex;
}

if (iIndex == iStart) AddMessage("Top!");
if (!(rtb.SelectionFont.Bold == bBold && rtb.SelectionFont.Italic == bItalic && rtb.SelectionFont.Underline == bUnderline)) {
sText = GetStyleText();
AddMessage(sText);
}
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigateNextBaseline) {
int iOffset = rtb.SelectionCharOffset;
iIndex = rtb.Index;
iEnd = rtb.TextLength;
while (iIndex < iEnd && rtb.SelectionCharOffset == iOffset) {
iIndex++;
rtb.Index = iIndex;
}
if (iIndex == iEnd) AddMessage("Bottom!");
if (rtb.SelectionCharOffset != iOffset) {
sText = GetBaselineText();
AddMessage(sText);
}
if (iIndex < iEnd) {
sText = rtb.GetRange(iIndex, iIndex + 1);
AddMessage(sText);
}
}

if (menuItem == menuNavigatePriorBaseline) {
int iOffset = rtb.SelectionCharOffset;
iIndex = rtb.Index;
iStart = 0;
while (iIndex > iStart && rtb.SelectionCharOffset == iOffset) {
iIndex--;
rtb.Index = iIndex;
}
if (iIndex == iStart) AddMessage("Top!");
if (rtb.SelectionCharOffset != iOffset) {
sText = GetBaselineText();
AddMessage(sText);
}
if (rtb.TextLength > 0) {
sText = rtb.GetRange(iIndex, iIndex + 1);
AddMessage(sText);
}
}

if (menuItem == menuNavigateNextFont) {
string sFont = Util.Font2String(rtb.SelectionFont);
string sColor = Util.Color2String(rtb.SelectionColor);
iIndex = rtb.Index;
iEnd = rtb.TextLength;
while (iIndex < iEnd && Util.Font2String(rtb.SelectionFont) == sFont && Util.Color2String(rtb.SelectionColor) == sColor) {
iIndex++;
rtb.Index = iIndex;
}
if (iIndex == iEnd) AddMessage("Bottom!");
if (!(Util.Font2String(rtb.SelectionFont) == sFont && Util.Color2String(rtb.SelectionColor) == sColor)) {
sText = Util.Font2String(rtb.SelectionFont);
//if (Util.Color2String(rtb.SelectionColor) != sColor) sText+= ", Color " + Util.Color2String(rtb.SelectionColor);
if (Util.Color2String(rtb.SelectionColor) != sColor) sText = GetFontText(rtb.SelectionFont, rtb.SelectionColor);
AddMessage(sText);
}
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigatePriorFont) {
string sFont = Util.Font2String(rtb.SelectionFont);
string sColor = Util.Color2String(rtb.SelectionColor);
iIndex = rtb.Index;
iStart = 0;
while (iIndex > iStart && Util.Font2String(rtb.SelectionFont) == sFont && Util.Color2String(rtb.SelectionColor) == sColor) {
iIndex--;
rtb.Index = iIndex;
}
if (iIndex == iStart) AddMessage("Top!");
if (!(Util.Font2String(rtb.SelectionFont) == sFont && Util.Color2String(rtb.SelectionColor) == sColor)) {
sText = Util.Font2String(rtb.SelectionFont);
if (Util.Color2String(rtb.SelectionColor) != sColor) sText = GetFontText(rtb.SelectionFont, rtb.SelectionColor);
AddMessage(sText);
}
Util.Say(rtb.RowText);
}

if (menuItem == menuQueryBlock) {
char[] a = {' ', '\t'};
sLine = "";
string sComment = App.ReadOption("QuotePrefix", "> ");
int iLevels = GetIndent();
int i = iLevels;
int iRow = rtb.Row;
int iTop = 0;
string sPre = "";
if (this.KeyRepeat % 2 != 0) {
while (iRow > iTop) {
// Util.Say(iRow);
iRow--;
sLine = rtb.GetRowText(iRow).Trim(a);
sPre = sLine + "\n" + sPre;
if (sLine.Length == 0 || sLine.StartsWith(sComment)) continue;
i = GetIndent(iRow);
if (iLevels > i) break;
}
}

i = iLevels;
iRow = rtb.Row;
string sRest = rtb.GetRowText(iRow);
sRest = "";
iCount = rtb.Lines.Length;
while (iRow < iCount) {
// Util.Say(iRow);
sLine = rtb.GetRowText(iRow).Trim();
i = GetIndent(iRow);
if (sLine.Length == 0 || sLine.StartsWith(sComment) || i >= iLevels) {
sRest += sLine + "\n";
}
else break;
if ((iRow == rtb.Row + 1) && (i > iLevels)) iLevels = i;
iRow++;
}
sText = sPre + sRest;
AddMessage(sText);
}

if (menuItem == menuNavigateRightBrace || menuItem == menuNavigateLeftBrace || menuItem == menuQueryBraces) {
if (rtb.Text.Length == 0) {
AddMessage("No text!");
return;
}

sText = App.ReadOption("BraceMatch", "{}");
string sLeft = sText.Substring(0, 1);
string sRight = sText.Substring(1, 1);
iIndex = rtb.Index;
string s = rtb.GetRange(iIndex, iIndex + 1);
switch (s) {
case "{" :
case "}" :
sLeft = "{";
sRight = "}";
break;
case "<" :
case ">" :
sLeft = "<";
sRight = ">" ;
break;
case "[" :
case "]" :
sLeft = "[";
sRight = "]";
break;
case "(" :
case ")" :
sLeft = "(";
sRight = ")";
break;
}

if (menuItem == menuNavigateRightBrace) {
iStart = iIndex;
iEnd = rtb.TextLength;
sText = rtb.GetRange(iStart, iEnd);
iCount = 0;
int i = 0;
// Dialog.Show(i, sText.Length);
bool bLoop = true;
while (bLoop) {
if (i == sText.Length) {
bLoop = false;
AddMessage("Not found!");
}
else if (sText.Substring(i, 1) == sLeft && i > 0) {
iCount++;
i++;
}
else if (sText.Substring(i, 1) == sRight && iCount > 0) {
iCount --;
i++;
}
else if (sText.Substring(i, 1) == sRight && iCount == 0 && i > 0) {
bLoop = false;
iIndex = iStart + i;
rtb.Index = iIndex;
}
else i++;
}
sText = (string) GetChunk()[1];
Util.Say(sText);
//Util.Say(rtb.RowText);
}
else if (menuItem == menuNavigateLeftBrace) {
iStart = 0;
iEnd = iIndex;
sText = rtb.GetRange(iStart, iEnd);
sText = Util.Reverse(sText);
iCount = 0;
int i = 0;
bool bLoop = true;
while (bLoop) {
if (i == sText.Length) {
bLoop = false;
AddMessage("Not found!");
}
else if (sText.Substring(i, 1) == sRight) {
iCount++;
i++;
}
else if (sText.Substring(i, 1) == sLeft && iCount > 0) {
iCount --;
i++;
}
else if (sText.Substring(i, 1) == sLeft && iCount == 0) {
bLoop = false;
iIndex = iEnd - i - 1;
rtb.Index = iIndex;
}
else i++;
}
//Util.Say(rtb.RowText);
sText = (string) GetChunk()[1];
Util.Say(sText);
}
else if (menuItem == menuQueryBraces) {
iStart = iIndex;
iEnd = rtb.TextLength;
sText = rtb.GetRange(iStart, iEnd);
iCount = 0;
int i = 0;
bool bLoop = true;
while (bLoop) {
if (i == sText.Length) {
bLoop = false;
}
else if (sText.Substring(i, 1) == sLeft && i > 0) {
iCount++;
i++;
}
else if (sText.Substring(i, 1) == sRight) {
iCount--;
i++;
}
else i++;
}
int iOutLevels = iCount;
iStart = 0;
iEnd = iIndex;
sText = rtb.GetRange(iStart, iEnd);
sText = Util.Reverse(sText);
iCount = 0;
i = 0;
bLoop = true;
while (bLoop) {
if (i == sText.Length) {
bLoop = false;
}
else if (sText.Substring(i, 1) == sRight) {
iCount++;
i++;
}
else if (sText.Substring(i, 1) == sLeft) {
iCount--;
i++;
}
else i++;
}
int iInLevels = iCount;
AddMessage(Util.Absolute(iInLevels) + " left");
AddMessage(Util.Absolute(iOutLevels) + " right");
}
}

if (menuItem == menuNavigateNextBlock) {
char[] a = {' ', '\t'};
sLine = "";
string sComment = App.ReadOption("QuotePrefix", "> ");
int iLevels = GetIndent();
int i = iLevels;
int iRow = rtb.Row;
string sPre = "";
i = iLevels;
iRow = rtb.Row;
string sRest = rtb.GetRowText(iRow);
sRest = "";
iCount = rtb.Lines.Length;
// Dialog.Show(iRow, iCount);
if (iRow >= iCount - 1) {
AddMessage("Bottom!");
return;
}

bool bNested = false;
while (iRow < iCount) {
// Util.Say(iRow);
sLine = rtb.GetRowText(iRow).Trim();
i = GetIndent(iRow);
if (!bNested && i > iLevels) {
iLevels ++;
bNested = true;
}
if (sLine.Length == 0 || sLine.StartsWith(sComment) || i >= iLevels) {
sRest += sLine + "\n";
}
else break;
// if ((iRow == rtb.Row + 1) && (i > iLevels)) iLevels = i;
iRow++;
}
sText = sPre + sRest;
if (iRow == iCount) {
AddMessage("Bottom!");
rtb.Row = iRow - 1;
return;
}

rtb.Row = iRow;
AddMessage(rtb.RowText);
}

if (menuItem == menuNavigatePriorBlock) {
char[] a = {' ', '\t'};
sLine = "";
string sComment = App.ReadOption("QuotePrefix", "> ");
int iLevels = GetIndent();
int i = iLevels;
int iRow = rtb.Row;
int iTop = 0;
if (iRow == iTop) {
AddMessage("Top!");
return;
}

string sPre = "";
bool bNested = false;
while (iRow > iTop) {
// Util.Say(iRow);
iRow--;
sLine = rtb.GetRowText(iRow).Trim(a);
sPre = sLine + "\n" + sPre;
if (sLine.Length == 0 || sLine.StartsWith(sComment)) continue;
i = GetIndent(iRow);
if (!bNested && i > iLevels) {
iLevels ++;
bNested = true;
}
if (iLevels > i) break;
}

rtb.Row = iRow;
AddMessage(rtb.RowText);
}

if (menuItem == menuNavigateNextIndent) {
string sComment = App.ReadOption("QuotePrefix", "> ");
//sComment = Util.Literalize(sComment);
int iLevels = GetIndent();
int i = iLevels;
int iRow = rtb.Row;
int iBottom = rtb.BottomRow;
//rtb.BeginUpdate();
while (iRow < iBottom) {
iRow++;
rtb.Row = iRow;
sLine = rtb.RowText.Trim();
if (sLine.Length == 0 || sLine.StartsWith(sComment)) continue;
i = GetIndent();
if (iLevels != i) break;
}
//rtb.EndUpdate();

if (iLevels == i) {
AddMessage("Bottom!");
rtb.Index = rtb.TextLength;
}
else AddMessage(GetDelta(iLevels, i));
//Util.Say(rtb.RowText);
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigatePriorIndent) {
string sComment = App.ReadOption("QuotePrefix", "> ");
//sComment = Util.Literalize(sComment);
int iLevels = GetIndent();
int i = iLevels;
int iRow = rtb.Row;
int iTop = 0;
//rtb.BeginUpdate();
while (iRow > iTop) {
iRow--;
rtb.Row = iRow;
sLine = rtb.RowText.Trim();
if (sLine.Length == 0 || sLine.StartsWith(sComment)) continue;
i = GetIndent();
if (iLevels != i) break;
}
//rtb.EndUpdate();

if (iLevels == i) {
AddMessage("Top!");
rtb.Index = 0;
}
else AddMessage(GetDelta(iLevels, i));
//Util.Say(rtb.RowText);
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigateNextChunk) {
NavigateNextMatch(App.MatchChunk);
}

if (menuItem == menuNavigatePriorChunk) {
NavigatePriorMatch(App.MatchChunk);
}

if (menuItem == menuNavigateNextSentence) {
NavigateNextMatch(App.MatchSentence);
}

if (menuItem == menuNavigatePriorSentence) {
NavigatePriorMatch(App.MatchSentence);
}

if (menuItem == menuNavigateNextParagraph) {
NavigateNextMatch(App.MatchParagraph);
}

if (menuItem == menuNavigatePriorParagraph) {
NavigatePriorMatch(App.MatchParagraph);
}

if (menuItem == menuNavigateNextPart) {
sMatch = @"^\s*((Chapter)|(Section)|(Part))\s+\d";
sMatch = App.ReadOption("NavigatePart", sMatch);
NavigateNextMatch(sMatch, true);
}

if (menuItem == menuNavigatePriorPart) {
sMatch = @"^\s*((Chapter)|(Section)|(Part))\s+\d";
sMatch = App.ReadOption("NavigatePart", sMatch);
NavigatePriorMatch(sMatch, true);
}

if (menuItem == menuNavigateNextSection) {
iStart = rtb.Index;
sText = rtb.Text;
iEnd = rtb.TextLength;
iIndex = sText.IndexOf(SB, iStart);
if (iIndex == -1) {
AddMessage("Bottom!");
rtb.Index = iEnd;
sLine = rtb.Lines[rtb.Lines.Length - 1];
return;
}
else {
rtb.Index = iIndex + 2;
sText = sText.Substring(0, iIndex);
string[] aText = sText.Split('\n');
iLine = aText.Length;
sLine = rtb.Lines[iLine];
}
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigatePriorSection) {
iEnd = rtb.Index;
if (iEnd > 0) iEnd--;
sText = rtb.Text.Substring(0, iEnd);
iIndex = sText.LastIndexOf(SB);
if (iIndex == -1) {
AddMessage("Top!");
rtb.Index = 0;
}
else {
rtb.Index = iIndex + 2;
Util.Say(rtb.RowText);
}
}

if (menuItem == menuNavigateGoToSection) {
//sLine = SB + rtb.RowText;
sLine = SB + rtb.RowText + LF;
sText = rtb.Text;
iIndex = sText.IndexOf(sLine);
if (iIndex == -1) {
AddMessage("Not found!");
}
else {
rtb.Index = iIndex + SB.Length;
Util.Say(rtb.RowText);
}
}

if (menuItem == menuNavigateGoToContents) {
sText = rtb.Text;
iIndex = sText.IndexOf(LB, rtb.Index);
if (iIndex == -1) {
AddMessage("Not found!");
return;
}
iEnd = iIndex + LB.Length;
sText = sText.Substring(0, iEnd);
iIndex = sText.LastIndexOf(SB);
if (iIndex == -1) {
AddMessage("Not found!");
return;
}
iStart = iIndex + SB.Length;
iIndex = sText.IndexOf(LB, iStart);
if (iIndex == -1) {
AddMessage("Not found!");
return;
}
iEnd = iIndex + LB.Length;
sLine = LB + sText.Substring(iStart, iEnd - iStart);
iIndex = sText.IndexOf(sLine);
if (iIndex == -1) {
AddMessage("Not found!");
return;
}
rtb.Index = iIndex + LB.Length;
Util.Say(rtb.RowText);
}

if (menuItem == menuNavigateSearchForTopic) {
}

if (menuItem == menuQueryAddress) {
if (this.KeyRepeat % 2 == 0) SetStatusAddress(null, null);
else if (App.ReadOption("HardPageAddress", "N").ToLower().Substring(0, 1) != "y") AddMessage(GetPageAddress(rtb));
else AddMessage(GetPercentAddress(rtb));
}

if (menuItem == menuQueryIndent) {
// Jared request to always say levels
//if (!rtb.IndentMode)  AddMessage("Level " + this.GetIndent());
// else {
if (this.KeyRepeat % 2 == 0) {
AddMessage("Level " + this.GetIndent());
return;
}
// AddMessage("Block");

char[] a = {' ', '\t'};
sLine = "";
string sComment = App.ReadOption("QuotePrefix", "> ");
int iLevels = GetIndent();
int i = iLevels;
int iRow = rtb.Row;
int iTop = 0;
while (iRow > iTop) {
iRow--;
sLine = rtb.GetRowText(iRow).Trim(a);
if (sLine.Length == 0 || sLine.StartsWith(sComment)) continue;
i = GetIndent(iRow);
// Util.Say("row " + iRow + " indent " + i);
// Only stop for less indentation
// if (iLevels != i) break;
if (iLevels > i) break;
}

if (iLevels == i) {
//AddMessage("Top!");
}
//else AddMessage(GetDelta(iLevels, i));
Util.Say(sLine);
// }
}

if (menuItem == menuQueryPath) {
sText = child.File;
if (this.KeyRepeat % 2 == 0) AddMessage(sText);
else {
Util.Spell(sText);
}
}

if (menuItem == menuQueryTopic) {
sText = rtb.Text;
iIndex = sText.IndexOf(LB, rtb.Index);
if (iIndex == -1) {
AddMessage("Not found!");
return;
}
iEnd = iIndex + LB.Length;
sText = sText.Substring(0, iEnd);
iIndex = sText.LastIndexOf(SB);
if (iIndex == -1) {
AddMessage("Not found!");
return;
}
iStart = iIndex + SB.Length;
iIndex = sText.IndexOf(LB, iStart);
if (iIndex == -1) {
AddMessage("Not found!");
return;
}
iEnd = iIndex + LB.Length;
sLine = sText.Substring(iStart, iEnd - iStart);
sText = sLine;
if (this.KeyRepeat % 2 == 0) AddMessage(sText);
else {
Util.Spell(sText);
}
}

if (menuItem == menuQueryYield) {
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
if (iStart == iEnd) {
AddMessage("All");
iStart = 0;
iEnd = rtb.TextLength;
}
else  AddMessage("Selected");

sText = rtb.GetRange(iStart, iEnd);
iResult = sText.Length;
AddMessage(Util.Pluralize(iResult, "character"));

if (iResult > 0) {
iResult = Util.RegExpCountCase(sText, "\\S+");
AddMessage(Util.Pluralize(iResult, "Word"));
iResult = Util.RegExpCountCase(sText, LB) + 1;
AddMessage("\t" + Util.Pluralize(iResult, "Line"));
}

}

if (menuItem == menuQueryStatus) {
if (this.KeyRepeat % 2 == 0) {
sText = rtb.Modified ? "Modified" : "Unmodified" + "\t";
sText += rtb.WordWrap ? "Wrap" : "Unwrap";
sText += rtb.ReadOnly ? "Guard" : "";
//sText += App.ReadData("Compiler", "Default");
}
else {
if (child == null || child.YieldEncoding == null) sText = "No disk file with encoding information is open!";
else if (child.IsUnixLineBreak) sText = "Encoding Unicode (UTF-8N)";
else sText = "Encoding " + child.YieldEncoding.EncodingName + " = " + child.YieldEncoding.CodePage;
}
AddMessage(sText);
}

if (menuItem == menuQueryCompiler) {
AddMessage("Compiler " + App.ReadData("Compiler", "Default"));
AddMessage("Folder " + Path.GetFileName(Directory.GetCurrentDirectory()));
}

if (menuItem == menuQuerySelected) {
sText = rtb.SelectedText;
if (sText.Length == 0) sText = "No text!";
if (this.KeyRepeat % 2 == 0) AddMessage(sText);
else {
Util.Spell(sText);
}
}

if (menuItem == menuQueryChunk) {
object[] a = GetChunk();
sText = (string) a[1];
if (sText.Length == 0) sText = "No text!";
if (this.KeyRepeat % 2 == 0) AddMessage(sText);
else {
Util.Spell(sText);
}
}

if (menuItem == menuQueryReadAll) {
sText = rtb.Text;
if (sText.Length == 0) sText = "No text!";
sText = Util.ConvertQuotes(sText);
if (this.KeyRepeat % 2 == 0) AddMessage(sText);
else {
Util.Spell(sText);
}
}

if (menuItem == menuQueryWindowsOpen) {
WindowsOpen();
}

if (menuItem == menuQueryClipboard) {
sText = Clipboard.GetText();
if (sText.Length == 0) sText = "No text!";
if (this.KeyRepeat % 2 == 0) AddMessage(sText);
else {
Util.Spell(sText);
// SetStatus(sText);
}
}

if (menuItem == menuQueryTime || menuItem == menuMiscInsertTime) {
string sDate, sTime;
GetDateAndTime(out sDate, out sTime);
//sText = dt.ToShortTimeString() + " on " + dt.ToLongDateString();
// sText = sTime + " on " + sDate;
sText = sTime;
if (sTime.Length > 0 && sDate.Length > 0) sText += " ";
sText += sDate;
if (menuItem == menuQueryTime) {
if (sTime.Length > 0) AddMessage(sTime);
if (sDate.Length > 0) AddMessage(sDate);
}
else {
rtb.ReplaceRange(rtb.Index, rtb.Index, sText);
Util.Say(rtb.RowText);
}
}

if (menuItem == menuQueryStyles) {
sText = GetStyleText() + " ";
sText += GetJustifyText() + " ";
sText += GetBaselineText() + " ";
sText = sText.Replace("Regular ", "");
sText = sText.Replace("Left ", "");
sText = sText.Replace("Flat ", "");
if (sText.Trim().Length == 0) sText = "Default";
AddMessage(sText);
}

if (menuItem == menuQueryFont) {
sText = GetFontText(rtb.SelectionFont, rtb.SelectionColor);
AddMessage(sText);
}

if (menuItem == menuMiscCalculateDate) {
CalculateDate();
}

if (menuItem == menuMiscHTMLFormat) {
sText = rtb.Text.Trim();
aResults = sText.Split('\n');
sTitle = aResults[0].Trim();
if (sTitle.ToLower() == "contents") sTitle = Path.GetFileNameWithoutExtension(this.Child.File);
else {
List<string> list = new List<string>(aResults);
list.RemoveAt(0);
aResults = list.ToArray();
sText = String.Join("\n", aResults);
}
sText = Util.String2Html(sText);
//sMatch = @"\nContents\s*\n(.|\n)*?\f\n";
sMatch = @"^\s*?Contents\s*?\n[^\f]*";
string sOldContents = "";
string sContents = "";
string sPreContents = "";
string sPostContents = "";
// Clipboard.SetText(sText);
object[] a = Util.RegExpContainsEquiv(sText, sMatch);
iIndex = (int) a[0];
// Dialog.Show(iIndex);
if (iIndex >= 0) {
sOldContents = (string) a[1];
//sPreContents = sText.Substring(0, iIndex - 1);
sPreContents = sText.Substring(0, iIndex);
sPostContents = sText.Substring(iIndex + sOldContents.Length);
sPostContents = Util.RegExpReplaceCase(sPostContents, "\n----------\n", "\n");
sContents = sOldContents.Trim();
aResults = sContents.Split('\n');
StringBuilder sb = new StringBuilder();
int j = 0;
for (int i = 0; i < aResults.Length; i++) {
string s = aResults[i];
if (i == 0) {
s = "<h1>" + s + "</h1>\n<ul>";
}
else if (s.Trim().Length == 0 || s.StartsWith("----------")) {
continue;
//s = s.Trim();
}
else {
j++;
string sHeader = "\n<h2><a name=\"A" + j + "\">" + s + "</a1></h2>\n";
sPostContents = Util.RegExpReplaceCase(sPostContents, "\f\n" + Regex.Escape(s) + "\n", sHeader);
s = "<li><a href=\"#A" + j + "\">" + s + "</a></li>";
}
sb.Append(s + "\n");
}
sb.Append ("</ul>\n");
sContents = sb.ToString();
sContents = "\n" + sContents;
}
else sPreContents = sText;

sPreContents = Util.RegExpReplaceCase(sPreContents, @"^(\w*\:\/\/.*?)$", "<a href=\"$1\">$1</a>");
sPreContents = Util.RegExpReplaceCase(sPreContents, @"^(\w*\@\w*\.\w*)$", "<a href=\"mailto:$1\">$1</a>");
sPreContents = Util.RegExpReplaceCase(sPreContents, " +\n", "\n");
sPreContents = Util.RegExpReplaceCase(sPreContents, "([^\n])\n([^\n])", "$1<br>\n$2");
sPreContents = Util.RegExpReplaceCase(sPreContents, "\n\n+", "\n<p>\n");

sPostContents = Util.RegExpReplaceCase(sPostContents, @"^(\w*\:\/\/.*?)$", "<a href=\"$1\">$1</a>");
sPostContents = Util.RegExpReplaceCase(sPostContents, @"^(\w*\@\w*\.\w*)$", "<a href=\"mailto:$1\">$1</a>");
sPostContents = Util.RegExpReplaceCase(sPostContents, " +\n", "\n");
sPostContents = Util.RegExpReplaceCase(sPostContents, "([^\n])\n([^\n])", "$1<br>\n$2");
sPostContents = Util.RegExpReplaceCase(sPostContents, "\n\n+", "\n<p>\n");

sText = sPreContents + sContents + sPostContents;
sText = sText.Trim();
sText = "<html>\n<head>\n<title>" + sTitle + "</title>\n</head>\n<body>\n" + sText + "\n</body>\n</html>\n";
if (child.File.IndexOf(@":\") > 0) Directory.SetCurrentDirectory(Path.GetDirectoryName(child.File));
sFile = Path.GetFileNameWithoutExtension(child.File) + ".htm";
new MdiChild(this, sFile);
this.Child.File = sFile;
this.Child.RTB.Text = sText;
this.Child.RTB.Modified = false;
}

if (menuItem == menuMiscTextConvert || menuItem == menuMiscTextCombine) {
List<string> list = new List<string>();
aResults = rtb.Lines;
string sDir = Directory.GetCurrentDirectory();
string sTempDir = "";
for (int i = 0; i < aResults.Length; i++) {
string s = aResults[i].Trim();
if (s.Length == 0) continue;
sTempDir = Path.GetDirectoryName(s);
if (sTempDir.Length == 0) s = Path.Combine(sDir, s);
else if (Directory.Exists(sTempDir)) sDir = sTempDir;
if (File.Exists(s)) list.Add(s);
}

aResults = list.ToArray();
if (aResults.Length == 0) {
AddMessage("No files found!");
return;
}

sText = Util.GetExtensions(aResults);
sResult = Dialog.Input("Filter", "Extensions", sText).Trim();
if (sResult.Length == 0) return;

aResults = Util.GetPathsWithExtensions(aResults, sResult);
if (aResults.Length == 0) {
AddMessage("No files!");
return;
}

StringBuilder sb = new StringBuilder();
iCount = 0;
AddMessage("Converting");
for (int i = 0; i < aResults.Length; i++) {
string sSource = aResults[i];
string sTarget = Path.ChangeExtension(sSource, ".txt");
string sName = Path.GetFileName(sSource);
AddMessage(sName);
//sText = COM.WordFile2String(sSource);
//sText = COM.ConvertFile2String(sSource);
int iConvert = 2;
string sTargetExt = "txt";
bool bTextOnly = true;
sText = COM.ConvertFile2String(sSource, ref iConvert, ref sTargetExt, bTextOnly);
if (sText.Length == 0) {
AddMessage("Error!");
continue;
}

iCount++;
if (menuItem == menuMiscTextConvert) Util.String2File(sText, sTarget);
else if (iCount == 1) sb.Append(sName + LB + LB + sText);
else sb.Append(SectionBreak + sName + LB + LB + sText);
}

AddMessage("Converted " + Util.Pluralize(iCount, "file"), true);
if (menuItem == menuMiscTextConvert || iCount == 0) return;

if (!IsEmptyWindow()) new MdiChild(this);
sText = sb.ToString();
sText += EOD;
rtb = this.Child.RTB;
rtb.Text = sText;
rtb.Modified = false;
}

if (menuItem == menuMiscTextContents) {
//sMatch = "(^|(" + SB + "))" + "[^\n]*";
sMatch = "(\\A|(" + SB + "))" + "[^\n]*";
aResults = Util.RegExpExtractCase(rtb.Text, sMatch);
iCount = aResults.Length;
AddMessage(Util.Pluralize(iCount, "topic"));
if (iCount == 0) return;

sText = String.Join(LB, aResults);
sText = sText.Replace(SB, "");
sText = "Contents" + LB + LB + sText + SectionBreak;
rtb.ReplaceRange(0, 0, sText);
rtb.Index = 0;
}

if (menuItem == menuMiscSetDefaultFont) {
object[] a = Dialog.GetFont(rtb.Font, rtb.ForeColor);
if (a.Length == 0) return;

rtb.Font = (Font) a[0];
rtb.ForeColor = (Color) a[1];
string sFont = GetFontText(rtb.Font, rtb.ForeColor);
App.WriteOption("FontDefault", sFont);
}

if (menuItem == menuMiscConfigurationOptions) {
aResults = App.ReadDefaultOptions();
//Array.Sort(aResults);
aLabels = new string[aResults.Length];
string[] aDefaults = new string[aResults.Length];
aValues = new string[aResults.Length];
for (int i = 0; i < aResults.Length; i++) {
aLabels[i] = (aResults[i].IndexOf("&") >= 0 ? "" : "&") + aResults[i];
aDefaults[i] = App.ReadDefaultOption(aResults[i], "");
aValues[i] = App.ReadOption(aResults[i], aDefaults[i]);
}

string[] a = Dialog.MultiInput("Configuration Options", aLabels, aValues);
if (a.Length == 0) return;
for (int i = 0; i < a.Length; i++) App.WriteOption(aResults[i], a[i]);
}

if (menuItem == menuMiscManualOptions) {
//OpenOrActivateWindow(App.IniFile, 0);
string sCompiler = App.ReadData("Compiler", "Default");
//sText = sCompiler + " Compiler";
//sResult = Dialog.Choose("Manual Options", "", new string[] {"&Main", "&" + sText}, 0);
sResult = Dialog.Choose("Manual Options", "", new string[] {"&Main", "&" + sCompiler}, 0);
if (sResult.Length == 0) return;

if (sResult == "&Main") sFile = App.IniFile;
else sFile = Path.Combine(App.DataDir, sCompiler + ".ini");
OpenOrActivateWindow(sFile, 0);
}

if (menuItem == menuMiscResetConfiguration) {
/*
if (Dialog.Confirm("Confirm", "Reset Configuration to Default?", "Y") == "Y") {
System.IO.File.Delete(App.IniFile);
App.SetConfigurationValues();
*/

string sCompiler = App.ReadData("Compiler", "Default");
//sText = sCompiler + " Compiler";
//sResult = Dialog.Choose("Manual Options", "", new string[] {"&Main", "&" + sText}, 0);
sResult = Dialog.Choose("Reset Configuration", "", new string[] {"&Main", "&" + sCompiler, "&Both", "&New"}, 0);
if (sResult.Length == 0) return;

if (sResult == "&Main" || sResult == "&Both") {
if (System.IO.File.Exists(App.IniFile)) System.IO.File.Delete(App.IniFile);
System.IO.File.Copy(App.DefaultIniFile, App.IniFile);
}

if (sResult == sCompiler || sResult == "&Both") {
sFile = Path.Combine(App.DataDir, App.ReadData("Compiler", "Default") + ".ini");
if (System.IO.File.Exists(sFile)) System.IO.File.Delete(sFile);
}

if (sResult == "&New") {
aLabels = new string[] {"&Name", "&CompileCommand", "&JumpPosition", "&AbbreviateOutput", "&NavigatePart", "&QuotePrefix", "&ExtensionDefault", "&GoToEnvironment"};
aValues = new string[] {"", "", "", "", "", "", "", ""};
aResults = Dialog.MultiInput("Create Compiler setting", aLabels, aValues);
if (aResults.Length == 0) return;

sCompiler = aResults[0];
HomerList hl = new HomerList(aResults);
hl.RemoveAt(0);
string sSetting = hl.GetSegments('~');
App.WriteValue("Compilers", sCompiler, sSetting);
}
AddMessage("Done!");
return;
}

if (menuItem == menuMiscGoToFolder) {
string sDir;
HomerList hl = new HomerList();
aResults = App.ReadSectionKeys("Recent");
foreach (string s in aResults) {
sDir = Path.GetDirectoryName(s);
if (!hl.Contains(sDir)) hl.Add(sDir);
}

aResults = App.ReadSectionKeys("Favorites");
foreach (string s in aResults) {
sDir = Path.GetDirectoryName(s);
if (!hl.Contains(sDir)) hl.Add(sDir);
}

if (hl.Count == 0) {
AddMessage("No items!");
return;
}

aResults = hl.ToArray();
string[] aDisplay = new string[aResults.Length];
for (int i = 0; i < aDisplay.Length; i++) aDisplay[i] = Path.GetFileName(aResults[i]);
sDir = Dialog.Pick("Go to Folder", aResults, aDisplay, true, 0);
if (sDir.Length == 0) return;

Directory.SetCurrentDirectory(sDir);
}

if (menuItem == menuMiscGoToSpecialFolder) {
string sDir = PickSpecialFolder();
if (sDir.Length == 0) return;

Directory.SetCurrentDirectory(sDir);
}

if (menuItem == menuMiscWordWrap) {
rtb.SetWrap(true);
SetRecent(child.File);
}

if (menuItem == menuMiscUnwrap) {
rtb.SetWrap(false);
SetRecent(child.File);
}

if (menuItem == menuMiscPathToClipboard) {
sText = child.File;
Clipboard.SetText(sText);
AddMessage(sText);
}

if (menuItem == menuMiscPathList) {
sTitle = "Open Folder";
string sDir = Dialog.OpenFolder(sTitle, "Name", Directory.GetCurrentDirectory());
if (sDir.Length == 0) return;

Directory.SetCurrentDirectory(sDir);

sText = Util.GetExtensions(sDir);
if (sText.Length == 0) {
AddMessage("No files!");
return;
}

sResult = Dialog.Input("Filter", "Extensions", sText);
if (sResult.Length == 0) return;

aResults = Util.GetPathsWithExtensions(Directory.GetFiles(sDir), sResult);
iLength = aResults.Length;
sText = Util.Pluralize(iLength, "file");
AddMessage(sText);

if (!IsEmptyWindow()) new MdiChild(this);
child = this.Child;
rtb = child.RTB;
for (int i = 0; i < iLength; i++) {
if (i == 0) sText = aResults[i] + "\n";
else sText += Path.GetFileName(aResults[i]) + "\n";
}
rtb.Text = sText;
rtb.Modified = true;
}

if (menuItem == menuMiscExplorerFolder) {
string sDir = GetDirChoice();
if (sDir.Length == 0) return;
ExplorerFolder(sDir);
}

if (menuItem == menuMiscEvaluateExpression) {
if (rtb.SelectionLength == 0) {
AddMessage("Line");
sText = rtb.RowText;
iIndex = rtb.RowStart + sText.Length;
}
else {
AddMessage("Selected");
sText = rtb.SelectedText;
iIndex = rtb.SelectionStart + sText.Length;
}

sText = JS.Eval(sText, new object[] {}).ToString();
if (sText.Length == 0) return;

sText = LB + sText;
rtb.ReplaceRange(iIndex, iIndex, sText);
rtb.Index = iIndex + 1;
Util.Say(rtb.RowText);
}

if (menuItem == menuMiscReplaceTokens) {
if (rtb.SelectionLength == 0) {
//AddMessage("Chunk");
object[] a = GetChunk();
iStart = (int) a[0];
sText = (string) a[1];
iEnd = iStart + sText.Length;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}
sText = rtb.GetRange(iStart, iEnd);

if (rtb.SelectionLength == 0 && !sText.StartsWith("%")) {
AddMessage("Replace Snippet");
aResults = GetSnippetFiles(out aValues);
HomerList hlResults = new HomerList(aResults);
HomerList hlValues = new HomerList(aValues);
//sMatch = @"^" + sText + @".*";
sMatch = sText;
Regex rx = new Regex(sMatch, RegexOptions.IgnoreCase);
iCount = hlResults.Count;
for (int i = iCount - 1; i >= 0; i--) {
if (rx.IsMatch(aValues[i])) continue;
hlResults.RemoveAt(i);
hlValues.RemoveAt(i);
}

if (hlResults.Count == 0) {
AddMessage("No match!");
return;
}

aResults = hlResults.ToArray();
aValues = hlValues.ToArray();

string sSnippet;
if (aResults.Length == 1) sSnippet = aResults[0];
else {
sSnippet = Dialog.Pick("Pick", aResults, aValues, false, 0);
if (sSnippet.Length == 0) return;
}

InvokeSnippet(sSnippet, sText, iStart, iEnd);
}
else {
if (rtb.SelectionLength == 0) AddMessage("Replace Token");
else AddMessage("Replace Selected Tokens");
string sTemp = sText;
sText = ReplaceTokens(sText);
if (sText == sTemp) {
AddMessage("No match!");
return;
}

rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart + sText.Length;
AddMessage(rtb.RowText);
}
}

if (menuItem == menuMiscTransformFiles) {
TransFormFiles();
}

if (menuItem == menuMiscPickCompiler) {
aResults = Ini.ReadSectionKeys(App.IniFile, "Compilers");
//Array.Sort(aResults);
sResult = App.ReadData("Compiler", "Default");
//Dialog.Show(sResult, String.Join("\n", aResults));
int i = Array.IndexOf(aResults, sResult);
//Dialog.Show(i);
if (i == -1) i = 0;
sResult = Dialog.Pick("Pick Compiler", aResults, false, i);
if (sResult.Length == 0) return;

sFile = Path.Combine(App.DataDir, App.ReadData("Compiler", "Default") + ".ini");
string sDir = Directory.GetCurrentDirectory();
Ini.WriteValue(sFile, "Data", "Directory", sDir);
App.WriteData("Compiler", sResult);
sFile = Path.Combine(App.DataDir, sResult + ".ini");
string s = Ini.ReadValue(sFile, "Data", "Directory", "");
if (Directory.Exists(s) && !Util.Equiv(sDir, s)) {
AddMessage("Folder " + Path.GetFileName(s));
Directory.SetCurrentDirectory(s);
}

sValue = Ini.ReadValue(App.IniFile, "Compilers", sResult, "");
string[] a = sValue.Split('~');
Ini.WriteQuote(App.IniFile, "Options", "CompileCommand", a[0]);
if (a.Length > 1) Ini.WriteQuote(App.IniFile, "Options", "JumpPosition", a[1]);
if (a.Length > 2) Ini.WriteQuote(App.IniFile, "Options", "AbbreviateOutput", a[2]);
if (a.Length > 3) Ini.WriteQuote(App.IniFile, "Options", "NavigatePart", a[3]);
if (a.Length > 4) Ini.WriteQuote(App.IniFile, "Options", "QuotePrefix", a[4]);
if (a.Length > 5) Ini.WriteQuote(App.IniFile, "Options", "ExtensionDefault", a[5]);
if (a.Length > 6) Ini.WriteQuote(App.IniFile, "Options", "GoToEnvironment", a[6]);
}

if (menuItem == menuMiscGoToEnvironment) {
string sCommand = @"%ProgDir%\ijs.exe";
sCommand = App.ReadOption("GoToEnvironment", sCommand);
if (this.Child == null) sFile = "temp.txt";
else sFile = child.File;
if (!sFile.Contains(@"\")) sFile = Path.Combine(Directory.GetCurrentDirectory(), sFile);
sCommand = Util.ExpandCommandLine(sCommand, sFile, sFile);
string sProcess = Path.GetFileNameWithoutExtension(sCommand);
if (!Util.ActivateProcess(sProcess)) Util.Run(sCommand);
}

if (menuItem == menuMiscCompile|| menuItem == menuMiscPromptCommand) {
string sCommand;
if (menuItem == menuMiscCompile) sCommand = App.ReadOption("CompileCommand", "");
else {
sCommand = App.ReadOption("PromptCommand", "");
sCommand = Dialog.Input("Prompt", "Command", sCommand).Trim();
if (sCommand.Length == 0) return;
App.WriteOption("PromptCommand", sCommand);
}

sFile = child.File;
if (!sFile.Contains(@"\")) sFile = "";
if (sCommand.IndexOf("%Source") >=0 && sFile.Length == 0) {
AddMessage("No disk file is open for this command!");
return;
}
else if (sFile.Length > 0) child.SaveTextOrRtfFile(sFile);

string sDir = Directory.GetCurrentDirectory();
if (sCommand.IndexOf("%SourceDir%") >=0) Directory.SetCurrentDirectory(Path.GetDirectoryName(sFile));
sCommand = Util.ExpandCommandLine(sCommand, sFile, Path.ChangeExtension(sFile, ".exe"));
// Dialog.Show(sCommand);

// Try with COMSpec
// string sOutput = Util.GetProgramOutput(@"c:\windows\system32\cmd.exe", "/c " + sCommand);

// Debug JAWS script
// if (!sCommand.Trim().EndsWith(">1")) sOutput = File.ReadAllText(App.TempFile);
// Util.Run(sCommand);

string sOutput = "";
// if (sCommand.Trim().EndsWith(">1") || sCommand.Trim().EndsWith("&1")) Util.GetProgramOutput("cmd.exe", "/c " + sCommand);
if (sCommand.Trim().EndsWith(">1") || sCommand.Trim().EndsWith("&1")) sOutput = Util.GetProgramOutput("cmd.exe", "/c " + sCommand);
else {
Util.RunHideWait(sCommand);
sOutput = File.ReadAllText(App.TempFile);
}

// Dialog.Show("output", sOutput.Length);

/*
Dialog.Show(sCommand);
int i = sCommand.IndexOf(".exe");
string sParams = sCommand.Substring(i + 5);
sCommand = sCommand.Substring(0, i + 4);
Dialog.Show(sCommand, sParams);
string sOutput = Util.GetProgramOutput(sCommand, sParams);
Dialog.Show(sOutput);
*/

if (sDir != Directory.GetCurrentDirectory()) Directory.SetCurrentDirectory(sDir);

if (menuItem == menuMiscCompile) {
string sJumpPosition = App.ReadOption("JumpPosition", "");
object[] a = Util.RegExpContainsCase(sOutput, sJumpPosition);
iIndex = (int) a[0];
if (iIndex >= 0) {
sText = (string) a[1];
a = Util.RegExpContainsCase(sText, @"\d+");
iIndex = (int) a[0];
if (iIndex >= 0) {
sLine = (string) a[1];
iIndex += sLine.Length;
sText = sText.Substring(iIndex);
a = Util.RegExpContainsCase(sText, @"\d+");
iIndex = (int) a[0];
string sColumn;
if (iIndex == -1) sColumn = "1";
else sColumn = (string) a[1];
string s = sLine + ", " + sColumn;
// Dialog.Show("s", s);
App.WriteData("Line", s);

try {
rtb.Line = Int32.Parse(sLine);
rtb.Column = Int32.Parse(sColumn);
}
catch {}
}
}

string sAbbreviateOutput = App.ReadOption("AbbreviateOutput", "\r");
sOutput = Util.RegExpReplaceEquiv(sOutput, sAbbreviateOutput, "\n").Trim();
if (sOutput.Length == 0) sOutput = "Done!";
AddMessage(sOutput);
}
Util.String2File(sOutput, App.TempFile);
}

if (menuItem == menuMiscReviewOutput) {
sFile = App.TempFile;
if (!File.Exists(sFile)) {
AddMessage("No output file found!");
return;
}
OpenOrActivateWindow(sFile, 0);
}

if (menuItem == menuMiscSaveSnippet) {
if (rtb.SelectionLength == 0) {
AddMessage("All");
iStart = 0;
iEnd = rtb.TextLength;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}
sText = rtb.GetRange(iStart, iEnd);

string sDir = @"Snippets\" + App.ReadData("Compiler", "Default");
sDir = Path.Combine(App.DataDir, sDir);
if (!Directory.Exists(sDir)) Directory.CreateDirectory(sDir);
sFile = Path.Combine(sDir, Path.GetFileName(child.File));
//sFile = Path.ChangeExtension(sFile, ".txt");
if (Path.GetExtension(sFile).Length == 0) sFile += ".txt";
sFile = Dialog.SaveFile("", sFile);
if (sFile.Length == 0) return;

if (rtb.SelectionLength == 0) child.SaveTextOrRtfFile(sFile);
else Util.String2File(sText, sFile);
AddMessage("Done!");
}

if (menuItem == menuMiscInvokeSnippet) {
if (rtb.SelectionLength == 0) {
AddMessage("Cursor");
//iStart = 0;
//iEnd = rtb.TextLength;
iStart = rtb.Index;
iEnd = iStart;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}
sText = rtb.GetRange(iStart, iEnd);

aResults = GetSnippetFiles(out aValues);

/*
foreach (string sSnippetFile in aResults) {
Util.Say(Path.GetFileNameWithoutExtension(sSnippetFile));
string sSnippetText = Util.File2String(sSnippetFile);
string[] aSnippetLines = sSnippetText.Split('\n');
StringBuilder sbSnippet = new StringBuilder();
foreach (string sSnippetLine in aSnippetLines) {
if (sSnippetLine.Trim().Length == 0) continue;
sbSnippet.Append(sSnippetLine.Trim() + "\r\n");
}
Util.String2File(sbSnippet.ToString(), sSnippetFile);
}
*/

if (aResults.Length == 0) {
AddMessage("No files!");
return;
}

string sSnippet = Dialog.Pick("Pick", aResults, aValues, false, 0);
if (sSnippet.Length == 0) return;

InvokeSnippet(sSnippet, sText, iStart, iEnd);
}

if (menuItem == menuMiscViewSnippet) {
aResults = GetSnippetFiles(out aValues);
if (aResults.Length == 0) {
AddMessage("No files!");
return;
}

sResult = Dialog.Pick("Pick", aResults, aValues, false, 0);
if (sResult.Length == 0) return;

OpenOrActivateWindow(sResult, 0);
}

if (menuItem == menuMiscKeepUniqueItems) {
string sLimitItem = Util.Literalize(App.ReadOption("LimitItem", "\n"));
if (rtb.SelectionLength == 0) {
AddMessage("All");
iStart = 0;
iEnd = rtb.TextLength;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

sText = rtb.GetRange(iStart, iEnd);
aResults = Regex.Split(sText, sLimitItem);
List<string> listNormal = new List<string>();
List<string> listLower = new List<string>();
foreach (string s in aResults) {
string sLower = s.ToLower();
if (listLower.Contains(sLower)) continue;
listLower.Add(sLower);
listNormal.Add(s);
}

aResults = listNormal.ToArray();
sText = String.Join(sLimitItem, aResults);
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
}

if (menuItem == menuMiscNumberItems) {
string sLimitItem = Util.Literalize(App.ReadOption("LimitItem", "\n"));
if (rtb.SelectionLength == 0) {
sTitle = "Number Items All";
iStart = 0;
iEnd = rtb.TextLength;
}
else {
sTitle = "Number Items Selected";
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

sResult = Dialog.Input(sTitle, "Start", "1").Trim();
if (sResult.Length == 0) return;

try {
iLine = Int32.Parse(sResult);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}

sText = rtb.GetRange(iStart, iEnd);
aResults = Regex.Split(sText, sLimitItem);
for (int i = 0; i < aResults.Length; i++) {
string s = aResults[i];
// if (s.Trim().Length > 0) s = iLine++ + ". " + s;
s = iLine++ + ". " + s;
aResults[i] = s;
}

sText = String.Join(sLimitItem, aResults);
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
}

if (menuItem == menuMiscOrderItems) {
string sLimitItem = Util.Literalize(App.ReadOption("LimitItem", "\n"));
if (rtb.SelectionLength == 0) {
AddMessage("All");
iStart = 0;
iEnd = rtb.TextLength;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

sText = rtb.GetRange(iStart, iEnd);
// aResults = sText.Split('\n');
aResults = Regex.Split(sText, sLimitItem);
string[] a = new string[aResults.Length];
for (int i = 0; i < a.Length; i++) a[i] = aResults[i].ToLower();
Array.Sort(a, aResults);
// sText = String.Join("\n", aResults);
sText = String.Join(sLimitItem, aResults);
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
}

if (menuItem == menuMiscReverseItems) {
string sLimitItem = Util.Literalize(App.ReadOption("LimitItem", "\n"));
if (rtb.SelectionLength == 0) {
AddMessage("All");
iStart = 0;
iEnd = rtb.TextLength;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

sText = rtb.GetRange(iStart, iEnd);
aResults = Regex.Split(sText, sLimitItem);
Array.Reverse(aResults);
sText = String.Join(sLimitItem, aResults);
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
}

if (menuItem == menuMiscListDifferentItems) {
string sLimitItem = Util.Literalize(App.ReadOption("LimitItem", "\n"));
// aResults = rtb.GetRange(0, rtb.RowStart).Split('\n');
sText = rtb.GetRange(0, rtb.RowStart);
aResults = Regex.Split(sText, sLimitItem);
sText = rtb.GetRange(rtb.RowStart, rtb.TextLength);
string[] a = Regex.Split(sText, sLimitItem);
// string[] a = rtb.GetRange(rtb.RowStart, rtb.TextLength).Split('\n');
List<string> list = new List<string>();
foreach (string s in aResults) if (s.Trim().Length > 0 && Array.IndexOf(a, s) == -1) list.Add(s);
aResults = list.ToArray();
if (aResults.Length == 0) {
AddMessage("No output!");
return;
}

AddMessage(Util.Pluralize(aResults.Length, "line"));
// sText = String.Join("\n", aResults).TrimEnd('\n') + "\n";
sText = String.Join(sLimitItem, aResults);
child = new MdiChild(App.Frame);
Child.RTB.Text = sText;
rtb.Index = 0;
}

if (menuItem == menuMiscQueryCommonItems) {
// aResults = rtb.GetRange(0, rtb.RowStart).Split('\n');
// string[] a = rtb.GetRange(rtb.RowStart, rtb.TextLength).Split('\n');
string sLimitItem = Util.Literalize(App.ReadOption("LimitItem", "\n"));
sText = rtb.GetRange(0, rtb.RowStart);
aResults = Regex.Split(sText, sLimitItem);
sText = rtb.GetRange(rtb.RowStart, rtb.TextLength);
string[] a = Regex.Split(sText, sLimitItem);

List<string> list = new List<string>();
foreach (string s in aResults) if (s.Trim().Length > 0 && Array.IndexOf(a, s) >= 0) list.Add(s);
aResults = list.ToArray();
if (aResults.Length == 0) {
AddMessage("No output!");
return;
}

AddMessage(Util.Pluralize(aResults.Length, "line"));
// sText = String.Join("\n", aResults).TrimEnd('\n') + "\n";
sText = String.Join(sLimitItem, aResults);
child = new MdiChild(App.Frame);
Child.RTB.Text = sText;
rtb.Index = 0;
}

if (menuItem == menuMiscCommandPrompt) {
string sDir = GetDirChoice();
if (sDir.Length == 0) return;
CommandPrompt(sDir);
}

if (menuItem == menuMiscBurnToCD) {
BurnToCD();
}

if (menuItem == menuMiscWebDownload) {
string sButton = "Web Page";
if (App.Frame.Child != null) {
sButton = Dialog.Choose("Choose Source of URLs", "", new string[] {"&Web Page", "&Current Document"}, 0);
if (sButton.Length == 0) return;
} // if

List<string[]> listLinks;
if (sButton.Replace("&", "") == "Web Page") {
string sUrl = COM.GetUrl();
if (sUrl.Length == 0) sUrl = App.ReadData("Url", "");
sUrl = Dialog.Input("Web Download", "Address", sUrl);
if (sUrl.Length == 0) return;

AddMessage("Please wait");
App.WriteData("Url", sUrl);
listLinks = VB.GetLinks(sUrl);
}
else {
listLinks = new List<string[]>();
aResults = Util.RegExpExtractCase(App.Frame.Child.RTB.Text, @"\w+\:\/\/[^\s""\'\)]+");
if (aResults.Length == 0) {
AddMessage("No URLs found!");
return;
}

for (int i = 0; i < aResults.Length; i++) {
listLinks.Add(new string[] {aResults[i], ""});
} // for
}

List<string> listFiles = new List<string>();
string sRef;
foreach (string[] aLink in listLinks) {
sRef = aLink[0];
sFile = Util.GetFileFromUri(sRef);
listFiles.Add(sFile);
}

string[] aFiles = listFiles.ToArray();
sText = Util.GetExtensions(aFiles);
sResult = Dialog.Input("Filter", "Extensions", sText).Replace(".", "").Trim().ToLower();
if (sResult.Length == 0) return;

aResults = Util.GetPathsWithExtensions(aFiles, sResult);

listFiles.Clear();
List<string> listItems = new List<string>();
List<string> listRefs = new List<string>();
foreach (string[] aLink in listLinks) {
sRef = aLink[0];
sFile = Util.GetFileFromUri(sRef);
string sExt = Path.GetExtension(sFile).TrimStart('.').ToLower();
//if (Array.IndexOf(aResults, sExt) == -1) continue;
if (Array.IndexOf(aResults, sFile) == -1) continue;

sText = aLink[1];
if (String.IsNullOrEmpty(sText)) sText = sRef;

listItems.Add(sText + " = " + sFile);
listFiles.Add(sFile);
listRefs.Add(sRef);
}

if (listItems.Count == 0) {
AddMessage("No items!");
return;
}

aValues = listItems.ToArray();
//aResults = Dialog.MultiPick("Pick Files", aValues, new int[] {}, false);
aResults = Dialog.MultiCheck("Pick Files", aValues, new int[] {}, false, 0);
if (aResults.Length == 0) return;

sTitle = "Open Folder";
string sDir = App.ReadData("DownloadFolder", Directory.GetCurrentDirectory());
sDir = Dialog.OpenFolder(sTitle, "Name", sDir);

if (sDir.Length == 0) return;

App.WriteData("DownloadFolder", sDir);
Directory.SetCurrentDirectory(sDir);
var wc = new WebClient();
AddMessage("Downloading");
foreach (string s in aResults) {
int i = listItems.IndexOf(s);
sFile = listFiles[i];
sRef = listRefs[i];
sFile = Path.Combine(sDir, sFile);
sFile = Util.GetUniqueName(sFile);
AddMessage(Path.GetFileName(sFile));
//Win32.Url2File(sRef, sFile);
try {
// VB.DownloadFile(sRef, sFile, "", "");
wc.DownloadFile(sRef, sFile);
}
catch (Exception ex) {
Dialog.Show("Alert", ex.Message);
// catch {}
// AddMessage("Error!");
}
}
AddMessage("Done!", true);
}
if (menuItem == menuMiscWebClientUtilities) {
App.Frame.WebClientUtilities();
}

if (menuItem == menuWindowNext) {
NextWindow();
}

if (menuItem == menuWindowPrior) {
PriorWindow();
}

if (menuItem == menuWindowArrangeIcons) {
this.LayoutMdi(MdiLayout.ArrangeIcons);
return;
}

if (menuItem == menuWindowCascade) {
this.LayoutMdi(MdiLayout.Cascade);
return;
}

if (menuItem == menuWindowTileHorizontal) {
this.LayoutMdi(MdiLayout.TileHorizontal);
return;
}

if (menuItem == menuWindowTileVertical) {
this.LayoutMdi(MdiLayout.TileVertical);
return;
}

if (menuItem == menuHelpAbout) {
sText = "EdSharp 4.0\nMay 30, 2017\n\n";
sText += "Copyright 2007 - 2017 by Jamal Mazrui\nGNU Lesser General Public License (LGPL)\n\n";
sText += ".NET Framework " + RuntimeEnvironment.GetSystemVersion() + "\n\n";
sText += Util.GetPortableExecutableKind();
Dialog.Show("About", sText);
}

if (menuItem == menuHelpDocumentation) {
sFile = Path.Combine(App.ProgramDir, App.ProgramName) + ".htm";
Process.Start(sFile);
}
if (menuItem == menuHelpHistoryOfChanges) {
sFile = Path.Combine(App.ProgramDir, "History.txt");
OpenOrActivateWindow(sFile, 1);
}

if (menuItem == menuHelpKeyDescriber) {
if (this.KeyDescriber) {
SetMessage("No Key Describer");
this.KeyDescriber = false;
}
else {
SetMessage("Key Describer On");
this.KeyDescriber = true;
}
}

if (menuItem == menuHelpHotKeySummary) {
sFile = Path.Combine(App.ProgramDir, "HotKeys.txt");
OpenOrActivateWindow(sFile, 1);
}

if (menuItem == menuHelpAlternateMenu) {
AlternateMenu();
}

if (menuItem == menuHelpContextMenu) {
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

ContextMenu(sFile);
}

if (menuItem == menuHelpSendToMenu) {
sFile = child.File;
if (!sFile.Contains(@"\")) {
AddMessage("No disk file is open for this command!");
return;
}

SendToMenu(sFile);
}

if (menuItem == menuHelpElevateVersion) {
ElevateVersion();
}

} // menuItem_Click handler

object[] GetChunk() {
bool bLoop;
int iIndex, iStart, iEnd;
string c, sText;
HomerRichTextBox rtb = this.Child.RTB;
sText = rtb.Text;
iIndex = rtb.Index;
c = "";

bLoop = true;
while (bLoop) {
if (iIndex == sText.Length) c = "";
else c = sText.Substring(iIndex, 1);
bLoop = (c.Trim().Length == 0);
bLoop = (bLoop && iIndex > 0);
if (bLoop) iIndex--;
}

bLoop = iIndex < sText.Length;
while (bLoop) {
c = sText.Substring(iIndex, 1);
bLoop = (c.Trim().Length > 0);
bLoop = (bLoop && iIndex > 0);
if (bLoop) iIndex--;
}
if (c.Trim().Length == 0) iIndex++;
iStart = iIndex;

bLoop = iIndex < sText.Length;
while (bLoop) {
c = sText.Substring(iIndex, 1);
bLoop = (c.Trim().Length > 0);
iIndex++;
bLoop = (bLoop && iIndex < sText.Length);
}
iEnd = iIndex;

if (iStart == iEnd) sText = "";
else sText = rtb.GetRange(iStart, iEnd);
sText = sText.TrimEnd();
return new object[] {iStart, sText};
} // GetChunk method

public void InvokeSnippet(string sSnippet, string sText, int iStart, int iEnd) {
string[] aLabels, aValues, aResults;
int iIndex;
HomerRichTextBox rtb = this.Child.RTB;
string sLabel, sValue, sMatch;
string sExt = Path.GetExtension(sSnippet).ToLower().TrimStart('.');
string sBody = Util.File2String(sSnippet);
sBody = Util.Convert2UnixLineBreak(sBody);
if (sExt == "js") {
JS.Eval(sBody, new object[] {});
return;
}
else if (sExt == "boo") {
if (App.Boo == null) App.Boo = COM.CreateObject("Iron.COM");
sSnippet = (string) COM.CallMethod(App.Boo, "Eval", new string[] {sBody, "", "", "", ""});
//Dialog.Show(sSnippet);
return;
}

aResults = sBody.Split('\n');

string sPre = "";
string sPost = "";

string sKeywords = aResults[0];
string[] aKeywords = sKeywords.Split(' ');
string sType = aKeywords[0];
if (sType == "text") {
sText = sBody.Substring(sKeywords.Length + 1);
iIndex = rtb.Index;
}
else if (sType == "html") {
List<string> listResults = new List<string>(aResults);
for (int i = listResults.Count - 1; i > 0; i--) if (listResults[i].Trim().Length == 0 || listResults[i].StartsWith(";")) listResults.RemoveAt(i);
aResults = listResults.ToArray();
sPre = "<" + aResults[1];
if (Array.IndexOf(aResults, "empty") == -1) sPost = "</" + sPre.Substring(1) + ">";

if (aResults.Length > 2) {
List<string> listLabels = new List<string>();
List<string> listValues = new List<string>();
for (int i = 2; i < aResults.Length; i++) {
string sLine = aResults[i];
//if (sLine.Trim().Length == 0 || sLine.StartsWith(";")) continue;
string[] a = sLine.Split('=');
sLabel = a[0];
sValue = "";
if (a.Length > 1) sValue = a[1];
listLabels.Add("&" + sLabel);
listValues.Add(sValue);
}

aLabels = listLabels.ToArray();
aValues = listValues.ToArray();
aResults = Dialog.MultiInput("Attributes", aLabels, aValues);
if (aResults.Length == 0) return;

for (int i = 0; i < aResults.Length; i++) {
sLabel = aLabels[i].Substring(1);
sValue = aResults[i];
sValue = Util.Literalize(sValue);
if (sValue.Length == 0) continue;
sPre += " " + sLabel + "=\"" + sValue + "\"";
}
}

sPre += ">";
if (Array.IndexOf(aKeywords, "phrase") == -1) sPost += "\n";
sText = sPre + sText + sPost;
}
else {
sText = sBody;
iIndex = iStart + sText.Length;
}

if (Array.IndexOf(aKeywords, "form") >= 0) {
string sDate, sTime;
GetDateAndTime(out sDate, out sTime);
sText = sText.Replace("%Date%", sDate);
sText = sText.Replace("%Time%", sTime);
string sUserName = Environment.UserName;
sUserName = sUserName.Replace(".", " ");
sText = sText.Replace("%UserName%", sUserName);
string[] aNames = (sUserName + " ").Split(' ');
sText = sText.Replace("%UserFirstName%", aNames[0]);
sText = sText.Replace("%UserLastName%", aNames[1]);

sMatch = @"\%\w+\=.*?\%";
string[] aVars = Util.RegExpExtractCase(sText, sMatch);
if (aVars.Length > 0) {
List<string> listLabels = new List<string>();
List<string> listValues = new List<string>();
List<string> listVars = new List<string>(aVars);
foreach (string sVar in aVars) {
string[] aParts = sVar.Split('=');
sLabel = aParts[0];
sLabel = "&" + sLabel.Substring(1, sLabel.Length - 1);
sValue = aParts[1];
sValue = sValue.Substring(0, sValue.Length - 1);
if (listLabels.Contains(sLabel)) {
// Stop reverse bug
listVars.Reverse();
listVars.Remove(sVar);
listVars.Reverse();
continue;
}

listLabels.Add(sLabel);
listValues.Add(sValue);
}
aLabels = listLabels.ToArray();
aValues = listValues.ToArray();
aResults = Dialog.MultiInput("Variables", aLabels, aValues);
if (aResults.Length == 0) return;

aVars = listVars.ToArray();
for (int i = 0; i < aVars.Length; i++) {
string sVar = aVars[i];
// Dialog.Show("sVar=" + sVar, "result=" + aResults[i]);
sText = sText.Replace(sVar, aResults[i]);
sVar = sVar.Split('=')[0] + "=%";
sText = sText.Replace(sVar, aResults[i]);
}
}

sText = ReplaceTokens(sText);
}

if (Array.IndexOf(aKeywords, "caret") >= 0) {
int i = sText.IndexOf("^^");
if (i >= 0) {
iIndex = iStart + i;
sText = sText.Remove(i, 2);
}
else iIndex = iStart;
}
else {
iIndex = iStart + sText.Length;
if (rtb.SelectionLength == 0) iIndex -= sPost.Length;
}

rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iIndex;
Util.Say(rtb.RowText);
} // InvokeSnippet method

public string PyDent2Brace(string sText) {
sText = Util.RegExpReplaceCase(sText, @"^\t*\# end \w+$", "");
string sOld = sText;
while (true) {
sText = Util.RegExpReplaceCase(sText, @"^(\t*) ", "$1\t");
if (sText == sOld) break;
else sOld = sText;
}

// does not work
/*
sText = Util.RegExpReplaceCase(sText, @"^\t*\#", "#");
sText = Util.RegExpReplaceCase(sText, @"^\#([^ ])", "# $1");
string[] aIndent = Util.RegExpExtractCase(sText, @"^\t+");
int iMax = 0;
int iMin = 1000;
foreach (string s in aIndent) {
iLength = s.Length;
if (iLength > iMax) iMax = iLength;
if (iLength < iMin) iMin = iLength;
}

if (iMin > 0) {
string sMin = "\t".PadRight(iMin, '\t');
string sAbbrev = "\t".PadRight(iMin - 1, '\t');
for (int n = 1; n <= iMax / iMin; n++) {
sText = Util.RegExpReplaceCase(sText, @"^" + sMin, sAbbrev);
}
}
*/

HomerList hl = new HomerList(sText.Split('\n'));
int i = 0;
int iOldLevel = 0;
int iCount = 0;
char[] a = {' ', ':'};
HomerList hlCode = new HomerList();
HomerList hlLevel = new HomerList();
bool bTripleQuote = false;
int iBrace = 0;
int iBracket = 0;
int iParen = 0;
int iTripleQuote = 0;
int iDoubleQuote = 0;
int iSingleQuote = 0;
bool bQuote = false;

while ( i < hl.Count) {
string sLine = hl[i];
string sTrim = sLine.TrimEnd();
string sPack = sTrim.TrimStart();
int iTrim = sTrim.Length;
int iPack = sPack.Length;

if (!bTripleQuote && (iPack == 0 || sPack.StartsWith("#"))) hl[i] = sPack;
else {
iParen = 0;
iBracket = 0;
iBrace = 0;
iSingleQuote = 0;
iDoubleQuote = 0;
while (true) {
int iCharCount = iPack;
int iChar = 0;
while (iChar < iCharCount) {
switch (sPack[iChar]) {
case '"' :
if (iChar > 0 && sPack[iChar - 1] == '\\') break;
if ((iChar + 2 < iCharCount) && sPack[iChar + 1] == '"' && sPack[iChar + 2] == '"') {
if (bTripleQuote && iTripleQuote > 0) {
bTripleQuote = false;
bQuote = false;
iTripleQuote--;
iSingleQuote = 0;
iDoubleQuote = 0;
}
else if (!bQuote && !bTripleQuote && iTripleQuote == 0) {
bTripleQuote = true;
bQuote = true;
iTripleQuote++;
iSingleQuote = 0;
iDoubleQuote = 0;
}
iChar += 2;
}
else if (bQuote && iDoubleQuote > 0) {
bQuote = false;
iDoubleQuote--;
}
else if (!bQuote && iDoubleQuote == 0) {
bQuote = true;
iDoubleQuote++;
}
break;
case '\'' :
if (iChar > 0 && sPack[iChar - 1] == '\\') break;
if (bQuote && iSingleQuote > 0) {
bQuote = false;
iSingleQuote--;
}
else if (!bQuote && iSingleQuote == 0) {
bQuote = true;
iSingleQuote++;
}
break;
case '(' :
if (!bQuote) iParen++;
break;
case ')' :
if (!bQuote) iParen--;
break;
case '[' :
if (!bQuote) iBracket++;
break;
case ']' :
if (!bQuote) iBracket--;
break;
case '{' :
if (!bQuote) iBrace++;
break;
case '}' :
if (!bQuote) iBrace--;
break;
}
iChar++;
}

if ((iParen + iBracket + iBrace == 0) || bTripleQuote) break;
hl[i] = sPack + @" \";

do  i++;
while (i < hl.Count && (hl[i].Trim().Length == 0 || hl[i].TrimStart().StartsWith("#")));
if (i == hl.Count) break;

sLine = hl[i];
sTrim = sLine.TrimEnd();
iTrim = sTrim.Length;
sPack = sTrim.TrimStart();
iPack = sPack.Length;
}

if (i == hl.Count) break;

if (bTripleQuote) {
hl[i] = sTrim;
}
else {
int iNewLevel = iTrim - iPack;
//if (i == 75 || i == 76) Dialog.Show(iNewLevel, hl[i]);
int iDelta = iOldLevel - iNewLevel;

int k = i - 1;
while (k >= 0 && hl[k].StartsWith("#")) k--;
k++;

while (hlCode.Count > 0 && iDelta > 0 && Int32.Parse(hlLevel[hlLevel.Max]) >= iNewLevel) {
hl.Insert(k, "} end " + hlCode.Pop());
hlLevel.Pop();
iCount--;
i++;
k++;
iDelta--;
}

if (iOldLevel > iNewLevel) {
hl.Insert(k, "");
i++;
}
iOldLevel = iNewLevel;

if (sPack.EndsWith(":")) {
sPack = sPack.TrimEnd(a) + " {";
iCount++;
string[] aCode = sPack.Split(' ');
hlCode.Add(aCode[0].TrimEnd('{'));
hlLevel.Add(iNewLevel.ToString());
}
else if (sPack.EndsWith(@"\")) {
hl[i] = sPack;
i++;
if (i == iCount) break;
sPack = hl[i].Trim();
}
hl[i] = sPack;

}
}
i++;
}

while (iCount > 0) {
hl.Add("} end " + hlCode.Pop());
iCount--;
}

sText = String.Join("\n", hl.ToArray()).Trim() + "\n";;
sText = Util.RegExpReplaceCase(sText, @"\n\n+", "\n\n");
sText = Util.RegExpReplaceCase(sText, @"\n+\n\}", "\n}");
sText = Util.RegExpReplaceCase(sText, @"\n+el", "\nel");
return sText;
} // PyDent2Brace method

public string PyBrace2Dent(string sText) {
sText = Util.RegExpReplaceCase(sText, @"^\t*\# end \w+$", "");
//sText = Util.RegExpReplaceCase(sText, @"^\t*\#", "#");
//sText = Util.RegExpReplaceCase(sText, @"^\#([^ ])", "# $1");

HomerList hl = new HomerList(sText.Split('\n'));
int i = 0;
int iCount = 0;
char[] a = {' ', '{'};
HomerList hlCode = new HomerList();
string sIndent = App.ReadOption("IndentUnit", "  ");
sIndent = Util.Literalize(sIndent);

while ( i < hl.Count) {
string sPack = hl[i].Trim();
//if (iCount > 0) sLine = "\t".PadLeft(iCount, '\t') + sPack;
string sLine;
if (iCount > 0) sLine = Util.Replicate(sIndent, iCount) + sPack;
else sLine = sPack;

if (sPack.EndsWith("{")) {
sLine = sLine.TrimEnd(a) + ":";
iCount++;
string[] aCode = sPack.Split(' ');
hlCode.Add(aCode[0].TrimEnd('{'));
}
else if (sPack.StartsWith("}")) {
sLine = "# end " + hlCode.Pop();
//if (iCount > 1) sLine = "\t".PadLeft(iCount - 1, '\t') + sLine;
if (iCount > 1) sLine = Util.Replicate(sIndent, iCount - 1) + sLine;
iCount--;
}
hl[i] = sLine;
i++;
}

sText = String.Join("\n", hl.ToArray()).Trim() + "\n";;
sText = Util.RegExpReplaceCase(sText, @"\n+\n", "\n\n");
//sText = Util.RegExpReplaceCase(sText, @"\n+(\t*)el", "\n$1el");
sText = Util.RegExpReplaceCase(sText, @"\n+(" + sIndent + ")el", "\n$1el");
return sText;
} // PyBrace2Dent method

public void HardLineBreak() {
bool bLoop;
string sResult, sTitle, sBody, sText, sLine;
int iWidth, iLength, iIndex, iStart, iEnd, i;
HomerRichTextBox rtb = this.Child.RTB;
if (rtb.SelectionLength == 0) {
sTitle = "Hard Line Break All";
iStart = 0;
iEnd = rtb.TextLength;
}
else {
sTitle = "Hard Line Break Selected";
iStart = rtb.SelectionStart;
iEnd = iStart + rtb.SelectionLength;
}

iWidth = 0;
foreach (string s in rtb.Lines) {
iLength = s.Length;
if (iLength > iWidth) iWidth = iLength;
}

sText = iWidth.ToString();
sResult = Dialog.Input(sTitle, "Width", sText).Trim();
if (sResult.Length == 0) return;

try {
iWidth = Int32.Parse(sResult);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}

sText = rtb.GetRange(iStart, iEnd);
sBody = "";
iIndex = 0;
iLength = sText.Length;
bLoop = true;

while (bLoop) {
//Dialog.Show("index", iIndex);
if (iLength - iIndex <= iWidth) {
sLine = sText.Substring(iIndex);
bLoop = false;
}
else {
sLine = sText.Substring(iIndex, iWidth);
i = sLine.LastIndexOf("\n");
//Dialog.Show("break", i);
if (i >=0) {
sLine = sLine.Substring(0, i + 1);
}
else {
i = sLine.LastIndexOf(" ");
//Dialog.Show("space", i);
if (i >=0) sLine = sLine.Substring(0, i + 1);
}
}
sBody += sLine.TrimEnd('\n') + "\n";
iIndex += sLine.Length;
}

rtb.ReplaceRange(iStart, iEnd, sBody);
rtb.Index = iStart + sText.Length;
Util.Say(rtb.RowText);
} // HardLineBreak method

public void CalculateDate() {
DateTime dt = new DateTime();
int iIndex, iResult, iYear, iMonth, iWeek, iDay;
string sText, sTitle, sYear, sMonth, sWeek, sDay;
string[] aResults, aLabels, aValues;
HomerRichTextBox rtb = this.Child.RTB;

sYear = App.ReadData("Year", "");
sMonth = App.ReadData("Month", "");
sWeek = App.ReadData("Week", "");
sDay = App.ReadData("Day", "");
aLabels = new string[] {"&Year", "&Month", "&Week", "&Day"};
aValues = new string[] {sYear, sMonth, sWeek, sDay};
sTitle = "Calculate Date";
aResults = Dialog.MultiInput(sTitle, aLabels, aValues);
if (aResults.Length == 0) return;

sYear = aResults[0].Trim();
sMonth = aResults[1].Trim();
sWeek = aResults[2].Trim();
sDay = aResults[3].Trim();
App.WriteData("Year", sYear);
App.WriteData("Month", sMonth);
App.WriteData("Week", sWeek);
App.WriteData("Day", sDay);

iResult = Util.Month2Num(sMonth);
if (iResult != -1) sMonth = iResult.ToString();
iResult = Util.Day2Num(sDay);
if (iResult != -1) sDay = iResult.ToString();

try {
iYear = (sYear.Length == 0) ? 0 : Int32.Parse(sYear);
iMonth = (sMonth.Length == 0) ? 0 : Int32.Parse(sMonth);
iWeek = (sWeek.Length == 0) ? 0 : Int32.Parse(sWeek);
iDay = (sDay.Length == 0) ? 0 : Int32.Parse(sDay);
if (iWeek == 0) {
dt = new DateTime(iYear, iMonth, iDay);
}
else {
dt = new DateTime(iYear, iMonth, 1);
dt = dt.AddDays(7 * (iWeek - 1));
while ((int) dt.DayOfWeek != iDay) dt = dt.AddDays(1);
}
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
}

iIndex = rtb.Index;
sText = dt.ToLongDateString();
rtb.ReplaceRange(iIndex, iIndex, sText);
iIndex += sText.Length;
rtb.Index = iIndex;
Util.Say(rtb.RowText);
} // CalculateDate method

public void Mail(bool bAttach) {
bool bCreate, bVisible, bSendMailAttach;
int iDisplayAlerts;
string sText, sFile, sDir;
object oApp, oOptions, oDocs, oDoc;

HomerRichTextBox rtb = this.Child.RTB;
sText = rtb.Text;

if (sText.Length == 0) {
AddMessage("No text!");
return;
}
sText = Util.Convert2WinLineBreak(sText);
sFile = this.Child.Text;
sDir = Path.GetTempPath();
sFile = Path.Combine(sDir, sFile);
if (Path.GetExtension(sFile).Length == 0) sFile += ".txt";
//Util.String2File(sText, sFile);
Util.String2File(sText, App.TempFile);
App.TempFiles.Add(sFile);

bool bAppVisible = false;
//oApp = COM.GetOrCreateObject("Word.Application", out bCreate);
oApp = COM.WordAccess(out bCreate);
bVisible = (bool) COM.GetProperty(oApp, "Visible");
iDisplayAlerts = (int) COM.GetProperty(oApp, "DisplayAlerts");
COM.SetProperty(oApp, "Visible", bAppVisible);
COM.SetProperty(oApp, "DisplayAlerts", 0);
oOptions = COM.GetProperty(oApp, "Options");
bSendMailAttach = (bool) COM.GetProperty(oOptions, "SendMailAttach");
COM.SetProperty(oOptions, "SendMailAttach", bAttach);
oDocs = COM.GetProperty(oApp, "Documents");
oDoc = VB.WordOpen(oDocs, App.TempFile, bAppVisible);
if (File.Exists(sFile)) File.Delete(sFile);
VB.WordSaveAs(oDoc, sFile, 2);
COM.CallMethod(oDoc, "SendMail");
VB.WordClose(oDoc);
COM.Release(ref oDoc);
COM.Release(ref oDocs);
if (bCreate) {
//VB.WordQuit(oApp);
}
else {
COM.SetProperty(oApp, "Visible", bVisible);
COM.SetProperty(oApp, "DisplayAlerts", iDisplayAlerts);
COM.SetProperty(oOptions, "SendMailAttach", bSendMailAttach);
}
COM.Release(ref oOptions);
COM.Release(ref oApp);
File.Delete(sFile);

App.Frame.Activate();
App.Frame.Child.RTB.Select();
} // MailBody method

public void SpellCheck() {
bool bCreate, bVisible;
int iDisplayAlerts, iStart, iEnd, iLength;
string sText, sOldText;
object oApp, oDocs, oDoc, oSelection;

HomerRichTextBox rtb = this.Child.RTB;
if (rtb.SelectionLength == 0) {
AddMessage("All");
iStart = 0;
sText = rtb.Text;
}
else {
AddMessage("Selected");
iStart = rtb.SelectionStart;
sText = rtb.SelectedText;
}

if (sText.Length == 0) {
AddMessage("No text!");
return;
}

iEnd = iStart + sText.Length;
sText = sText.TrimEnd();
sOldText = sText;
sText = Util.Convert2MacLineBreak(sText);

bool bAppVisible = true;
//oApp = COM.GetOrCreateObject("Word.Application", out bCreate);
oApp = COM.WordAccess(out bCreate);
bVisible = (bool) COM.GetProperty(oApp, "Visible");
iDisplayAlerts = (int) COM.GetProperty(oApp, "DisplayAlerts");
COM.SetProperty(oApp, "Visible", bAppVisible);
COM.SetProperty(oApp, "DisplayAlerts", 0);
oDocs = COM.GetProperty(oApp, "Documents");
oDoc = COM.CallMethod(oDocs, "Add");
COM.CallMethod(oDoc, "Activate");
oSelection = COM.GetProperty(oApp, "Selection");
COM.CallMethod(oSelection, "TypeText", sText);
Util.ActivateProcess("WinWord");
COM.CallMethod(oDoc, "CheckSpelling");
iLength = (int) COM.GetProperty(oSelection, "StoryLength");
COM.CallMethod(oSelection, "SetRange", new object[] {0, iLength});
sText = (string) COM.GetProperty(oSelection, "Text");
sText = sText.Trim();
COM.Release(ref oSelection);
VB.WordClose(oDoc);
COM.Release(ref oDoc);
COM.Release(ref oDocs);
if (bCreate) {
//VB.WordQuit(oApp);
}
else {
COM.SetProperty(oApp, "Visible", bVisible);
COM.SetProperty(oApp, "DisplayAlerts", iDisplayAlerts);
}
COM.Release(ref oApp);

App.Frame.Activate();
App.Frame.Child.RTB.Select();
sText = Util.Convert2UnixLineBreak(sText);
if (sText == sOldText) AddMessage("No changes!");
else {
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
//AddMessage("Done!");
}
} // SpellCheck method

public void Thesaurus() {
bool bCreate, bVisible;
int iDisplayAlerts, iStart, iEnd, iLength;
string sText, sOldText;
object[] aResults;
object oApp, oDocs, oDoc, oSelection, oRange;

HomerRichTextBox rtb = this.Child.RTB;
if (rtb.SelectionLength == 0) {
//AddMessage("Chunk");
aResults = GetChunk();
iStart = (int) aResults[0];
sText = (string) aResults[1];
}
else {
//AddMessage("Selected");
iStart = rtb.SelectionStart;
sText = rtb.SelectedText;
}

sText = sText.TrimEnd();
if (sText.Length == 0) {
AddMessage("No text!");
return;
}

iEnd = iStart + sText.Length;
sOldText = sText;
sText = Util.Convert2MacLineBreak(sText);

bool bAppVisible = true;
//oApp = COM.GetOrCreateObject("Word.Application", out bCreate);
oApp = COM.WordAccess(out bCreate);
bVisible = (bool) COM.GetProperty(oApp, "Visible");
iDisplayAlerts = (int) COM.GetProperty(oApp, "DisplayAlerts");
COM.SetProperty(oApp, "Visible", bAppVisible);
COM.SetProperty(oApp, "DisplayAlerts", 0);
oDocs = COM.GetProperty(oApp, "Documents");
oDoc = COM.CallMethod(oDocs, "Add");
oSelection = COM.GetProperty(oApp, "Selection");
COM.CallMethod(oSelection, "TypeText", sText);
oRange = COM.GetProperty(oSelection, "Range");
Util.ActivateProcess("WinWord");
COM.CallMethod(oRange, "CheckSynonyms");
iLength = (int) COM.GetProperty(oSelection, "StoryLength");
COM.CallMethod(oSelection, "SetRange", new object[] {0, iLength});
sText = (string) COM.GetProperty(oSelection, "Text");
sText = sText.Trim();
COM.Release(ref oRange);
COM.Release(ref oSelection);
VB.WordClose(oDoc);
COM.Release(ref oDoc);
COM.Release(ref oDocs);
if (bCreate) {
//VB.WordQuit(oApp);
}
else {
COM.SetProperty(oApp, "Visible", bVisible);
COM.SetProperty(oApp, "DisplayAlerts", iDisplayAlerts);
}
COM.Release(ref oApp);

App.Frame.Activate();
App.Frame.Child.RTB.Select();
sText = Util.Convert2UnixLineBreak(sText);
if (sText == sOldText) AddMessage("No changes!");
else {
rtb.ReplaceRange(iStart, iEnd, sText);
rtb.Index = iStart;
//AddMessage("Done!");
}
} // Thesaurus method

public void ElevateVersion() {
Util.Say("Comparing with server");
string sIniFile = Path.GetTempFileName();
// string sIniUrl = "http://www.EmpowermentZone.com//appstamp.ini";
string sIniUrl = "http://www.EmpowermentZone.com/AppStamp.ini";
Win32.Url2File(sIniUrl, sIniFile);
string sValue = Ini.ReadValue(sIniFile, "Versions", "EdSharp", "");
string sRemoteVersion = sValue.Split('|')[0];
string sRemoteTime = sValue.Split('|')[1];
DateTime time = File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location);
string sLocalTime = time.ToString("u");
sLocalTime = sLocalTime.Substring(0, sLocalTime.Length - 4);
// string sDir = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
string sDir = Path.GetTempPath();
string sName = "EdSharp_setup.exe";
string sUrl = @"http://www.EmpowermentZone.com/" + sName;
string sFile = Path.Combine(sDir, sName);

string sMsg = "Current";
if (sRemoteTime.CompareTo(sLocalTime) == 1) sMsg = "Newer";
else if (sRemoteTime.CompareTo(sLocalTime) == -1) sMsg = "Older";
sMsg +=" EdSharp " + sRemoteVersion;
time = DateTime.Parse(sRemoteTime);
sMsg += "\nReleased " + time.ToLongDateString() + " at " + time.ToShortTimeString();
sMsg += "\ndownload from\n" + sUrl + "\nto Temp directory, and run installer?";
string sDefault = sMsg.StartsWith("Newer") ? "Y" : "N";
if (Dialog.Confirm("Confirm", sMsg, sDefault) != "Y") return;
Util.Say("Please wait");
try {
File.Delete(sFile);
if (!Win32.Url2File(sUrl, sFile)) {
Dialog.Show("Error", "Cannot download file");
return;
}
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}
Util.Say("Exiting EdSharp and running installer");
//Process.Start(sFile);
Util.Run(Util.Quote(sFile));
Application.Exit();
} // ElevateVersion method

public bool ExitApp() {
while (this.Child != null) {
if (!CloseWindow(this.Child, true)) return false;
}
Application.Exit();
return true;
} // ExitApp method

public bool CloseWindow(MdiChild child) {
bool bExiting = false;
return CloseWindow(child, bExiting);
} // CloseWindow method

public bool CloseWindow(MdiChild child, bool bExiting) {
HomerRichTextBox rtb = child.RTB;
if (rtb.Modified) {
switch (Dialog.Confirm("Confirm", "Save changes to " + child.Text + "?", "Y")) {
case "Y" :
menuFileSave.PerformClick();
if (rtb.Modified) return false;
else break;
case "" :
return false;
}
}

if (bExiting && !rtb.Modified && child.File.IndexOf(@"\") >=0 && !Util.Equiv(child.File, App.IniFile)) Ini.WriteValue(App.IniFile, "Previous", child.File, rtb.Index.ToString());
child.Close();
return true;
} // CloseWindow method

public void SetRecent(string sFile) {
if (!sFile.Contains(@"\")) return;
if (Util.Equiv(sFile, App.IniFile)) return;

DateTime dt = DateTime.Now;
string sTime = dt.ToString("u");
sTime = sTime.Substring(0, sTime.Length - 1);
int iIndex = this.Child.RTB.Index;
sTime += "|" + iIndex;
if (this.MdiChildren.Length == 0) sTime += "|N|W";
else sTime += "|" + (this.Child.RTB.ReadOnly ? "G" : "M") + "|" + Util.If(this.Child.RTB.WordWrap, "W", "U");
App.WriteValue("Recent", sFile, sTime);
string sDir = Path.GetDirectoryName(sFile);
if (Directory.Exists(sDir)) Directory.SetCurrentDirectory(sDir);
sFile = Path.Combine(App.DataDir, App.ReadData("Compiler", "Default") + ".ini");
Ini.WriteValue(sFile, "Data", "Directory", sDir);
} // SetRecent method

bool ApplyWrap(string sSection, string sFile) {
string sText = App.ReadValue(sSection, sFile, "");
if (sText.Length == 0) return false;

HomerRichTextBox rtb = this.Child.RTB;
sText = App.ReadOption("WordWrap", "Y");
sText = "-1|M|" + Util.If((sText == "N"), "U", "W");
sText = App.ReadValue(sSection, sFile, sText);
bool b = (bool) Util.If(sText.EndsWith("U"), false, true);
if (b && !rtb.WordWrap) {
AddMessage("Word wrap");
rtb.SetWrap(true);
}
else if (!b && rtb.WordWrap) {
AddMessage("Unwrap");
rtb.SetWrap(false);
}
return true;
} // ApplyWrap method

bool ApplyGuard(string sSection, string sFile) {
string sText = App.ReadValue(sSection, sFile, "");
if (sText.Length == 0) return false;

HomerRichTextBox rtb = this.Child.RTB;
sText = App.ReadOption("WordWrap", "Y");
sText = "-1|M|" + Util.If((sText == "N"), "U", "W");
sText = App.ReadValue(sSection, sFile, sText);
if (sText.IndexOf("G") >= 0) {
AddMessage("Guard");
rtb.SetGuard(true);
}
return true;
} // ApplyGuard method

public void ApplyFileOptions(string sFile) {
if (!ApplyGuard("Favorites", sFile)) ApplyGuard("Recent", sFile);
if (!ApplyWrap("Favorites", sFile)) {
ApplyWrap("Recent", sFile);
return;
}

HomerRichTextBox rtb = this.Child.RTB;
string sText = App.ReadValue("Favorites", sFile, "");
try {
string[] a = sText.Split('|');
sText = a[0];
rtb.Index = Int32.Parse(sText);
AddMessage("Bookmark at percent " + rtb.Percent);
}
catch {}
} // ApplyFavorite method

public string PickSpecialFolder() {
string sName = "";
string sPath = "";
StringBuilder sbNames = new StringBuilder();
StringBuilder sbPaths = new StringBuilder("\n");
object oShell = COM.CreateObject("Shell.Application");
for (int i = 0; i < 100; i++) {
try {
Object oDir = COM.CallMethod(oShell, "Namespace", new object[] {i});
Object oItem = COM.GetProperty(oDir, "Self");
sPath = (string) COM.GetProperty(oItem, "Path");
if (!Directory.Exists(sPath)) continue;
if (Util.IsNumeric(Path.GetFileName(sPath))) continue;
if (sbPaths.ToString().ToLower().Trim('\\').Contains("\n" + sPath.ToLower().Trim('\\') + "\n")) continue;
sbPaths.Append(sPath + "\n");
sName = (string) COM.GetProperty(oItem, "Name");
if (Util.Equiv(sName, "Temporary Internet Files")) sName = "Internet Cache";
else if (Util.Equiv(sName, "History")) sName = "Internet History";
else if (Util.Equiv(sName, "NetHood")) sName = "Network Neighborhood";
else if (Util.Equiv(sName, "PrintHood")) sName = "Printer Neighborhood";
else if ((@"\" + sPath.ToLower() + @"\").Contains(@"\all users\")) sName = "Common " + sName;
else if (!Util.Equiv(sName, "History") && (@"\" + sPath.ToLower() + @"\").Contains(@"\local settings\")) sName = "Local " + sName;
sbNames.Append(sName + "\n");
}
catch {
continue;
}
}

Environment.SpecialFolder folder;
for (int i = 0; i < 100; i++) {
sPath = "";
try {
folder = (Environment.SpecialFolder) i;
sPath = Environment.GetFolderPath(folder);
}
catch {
continue;
}
if (!Directory.Exists(sPath)) continue;
if (Util.IsNumeric(Path.GetFileName(sPath))) continue;
if (sbPaths.ToString().ToLower().Trim('\\').Contains("\n" + sPath.ToLower().Trim('\\') + "\n")) continue;
sbPaths.Append(sPath + "\n");
sName = folder.ToString();
sbNames.Append(sName + "\n");
}
sbNames.Append("Temp" + "\n");
sbPaths.Append(Util.GetTempFolder() + "\n");

string[] aNames = sbNames.ToString().Trim().Split('\n');
string[] aPaths = sbPaths.ToString().Trim().Split('\n');

string sDir = Dialog.Pick("Go to Special Folder", aPaths, aNames, true, 0);
return sDir;
} // PickSpecialFolder method

public void OpenOrActivateWindow(string sFile) {
int iConvert = 0;
OpenOrActivateWindow(sFile, iConvert);
} // OpenOrActivateWindow method

public void OpenOrActivateWindow(string sFile, int iConvert) {
string sLine = "";
string sColumn = "";
OpenOrActivateWindow(sFile, iConvert, sLine, sColumn);
} // OpenOrActivateWindow method

public void OpenOrActivateWindow(string sFile, int iConvert, string sLine, string sColumn) {
string sText;
sFile = Util.Unquote(sFile);
if (!File.Exists(sFile)) {
AddMessage("File not found!");
return;
}

sFile = Util.GetLfn(sFile);
// ApplyFileOptions(sFile);
// SetRecent(sFile);
object[] children = this.MdiChildren;
foreach (MdiChild child in children) {
if (Util.Equiv(child.File, sFile)) {
Util.Say("returning");
child.Activate();
SetCursorPosition(child.RTB, sLine, sColumn);
return;
}
}

string sTargetExt = "txt";
if (iConvert == 0) sText = "";
else {
// Dialog.Show("iConvert " + iConvert, "sTargetExt " + sTargetExt);
sText = COM.ConvertFile2String(sFile, ref iConvert, ref sTargetExt);
// Dialog.Show("iConvert " + iConvert, "sTargetExt " + sTargetExt);

if (iConvert >= 1 && sText.Trim().Length == 0) {
AddMessage("No text!");
return;
}
// Disable because also speaks after recent files
// else App.Frame.AddMessage("Done!");
}

// Did so above
// SetRecent(sFile);
//if (!IsEmptyWindow()) new MdiChild(this);
//if (!IsEmptyWindow()) new MdiChild(this, "");
if (!IsEmptyWindow()) new MdiChild(this, sFile);
if (iConvert <= 0) {
this.Child.LoadTextOrRtfFile(sFile, (iConvert == 0 ? true : false));
//Dialog.Show(sFile);
ApplyFileOptions(sFile);

if (sFile == App.IniFile) return;
}
else {
string s = sText.Trim().ToLower();
if (s.StartsWith(@"{\rtf") && s.EndsWith("}")) this.Child.RTB.Rtf = sText;
else this.Child.RTB.Text = sText;
this.Child.Text = Path.GetFileNameWithoutExtension(sFile) + "." + sTargetExt;
this.Child.File = this.Child.Text;
this.Child.RTB.Modified = false;

}
// Try disabling for auto bookmark
// SetRecent(sFile);

SetCursorPosition(this.Child.RTB, sLine, sColumn);
} // OpenOrActivateWindow method

public static bool SetCursorPosition(HomerRichTextBox rtb, string sLine, string sColumn) {
bool bReturn = false;
try {
if (sLine.Length > 0) rtb.Line = Int32.Parse(sLine);
if (sColumn.Length > 0) rtb.Column = Int32.Parse(sColumn);
bReturn = true;
}
catch {}
return bReturn;
} // SetCursorPosition method

public void NextWindow() {
object[] children = this.MdiChildren;
if (children.Length == 0) AddMessage("No windows!");
else if (children.Length == 1) AddMessage("Only this window!");
else {
MdiChild child = this.Child;
int iPosition = Array.IndexOf(children, child);
iPosition++;
if (iPosition == children.Length) iPosition = 0;
((MdiChild) children[iPosition]).Activate();
}
} // NextWindow method

public void PriorWindow() {
object[] children = this.MdiChildren;
if (children.Length == 0) AddMessage("No windows!");
else if (children.Length == 1) AddMessage("Only this window!");
else {
MdiChild child = this.Child;
int iPosition = Array.IndexOf(children, child);
iPosition--;
if (iPosition == -1) iPosition = children.Length - 1;
((MdiChild) children[iPosition]).Activate();
}
} // PriorWindow method

public void CloseAllButCurrentWindow() {
MdiChild child = this.Child;
if (child == null) return;

object[] children = this.MdiChildren;
int iCount = 0;
foreach (MdiChild o in children) {
if (o != child) {
o.Close();
iCount++;
}
}
} // CloseAllButCurrent method

public void WindowsOpen() {
object[] children = this.MdiChildren;
int iCount = children.Length;
if (iCount == 0) AddMessage("No windows!");
else {
//string s = Util.Pluralize(iCount, "window");
AddMessage(iCount);
foreach (MdiChild child in children) {
string sTitle = child.Text;
string sText = sTitle;
if (this.KeyRepeat % 2 == 0) AddMessage(sText);
else {
Util.Spell(sText);
}
}
}
} // WindowsOpen method

public void NavigateNextMatch(string sMatch) {
bool bLine = false;
NavigateNextMatch(sMatch, bLine);
} // NavigateNextMatch method

public void NavigateNextMatch(string sMatch, bool bLine) {
int iIndex, iStart, iEnd, iForward;
string sValue, sText;
object[] aResults;
HomerRichTextBox rtb = this.Child.RTB;
if (bLine) iIndex = rtb.RowEnd + 1;
else iIndex = rtb.Index;
iStart = iIndex;
iEnd = rtb.TextLength;
if (iStart >= iEnd) aResults = new object[] {-1, ""};
else {
sText = rtb.GetRange(iStart, iEnd);
aResults = Util.RegExpContainsEquiv(sText, sMatch);
}
if ((int) aResults[0] == -1) {
this.AddMessage("Bottom!");
iStart = iEnd;
iIndex = iEnd;
}
else if (bLine) {
iIndex += (int) aResults[0];
iIndex += ((string) aResults[1]).Length;
}
else {
iForward = (int) aResults[0];
sValue = (string) aResults[1];
iIndex += iForward + sValue.Length;
iStart = iIndex;
sText = rtb.GetRange(iStart, iEnd);
aResults = Util.RegExpContainsEquiv(sText, sMatch);
if ((int) aResults[0] == -1) {
}
else {
iForward = (int) aResults[0];
sValue = (string) aResults[1];
iEnd = iStart + iForward + sValue.Length;
}
}

if (bLine) {
rtb.Index = iIndex;
rtb.Col = 0;
sText = rtb.RowText;
}
else {
sText = rtb.GetRange(iStart, iEnd);
rtb.Index = iIndex;
}
this.AddMessage(sText);
} // NavigateNextMatch method

public void NavigatePriorMatch(string sMatch) {
bool bLine = false;
NavigatePriorMatch(sMatch, bLine);
} // NavigatePriorMatch method

public void NavigatePriorMatch(string sMatch, bool bLine) {
int iIndex, iStart, iEnd, iBackward;
string sValue, sText;
object[] aResults;
HomerRichTextBox rtb = this.Child.RTB;
if (bLine) iIndex = rtb.RowStart;
else iIndex = rtb.Index;
iStart = 0;
iEnd = iIndex;
sText = rtb.GetRange(iStart, iEnd);
aResults = Util.RegExpContainsLastEquiv(sText, sMatch);
if ((int) aResults[0] == -1) {
this.AddMessage("Top!");
iIndex = iStart;
iEnd = iStart;
}
else if (bLine) {
iIndex = (int) aResults[0];
iIndex += ((string) aResults[1]).Length;

if (iIndex == rtb.Index) {
iEnd = (int) aResults[0];
sText = rtb.GetRange(iStart, iEnd);
aResults = Util.RegExpContainsLastEquiv(sText, sMatch);
iIndex = (int) aResults[0];
if ((int) aResults[0] == -1) {
this.AddMessage("Top!");
iIndex = iStart;
iEnd = iStart;
}
else iIndex += ((string) aResults[1]).Length;
}
}
else {
iBackward = (int) aResults[0];
sValue = (string) aResults[1];
// Dialog.Show(sValue, iBackward);
iEnd = iBackward;
sText = rtb.GetRange(iStart, iEnd);
aResults = Util.RegExpContainsLastEquiv(sText, sMatch);
if ((int) aResults[0] == -1) {
iIndex = iStart;
}
else {
iBackward = (int) aResults[0];
sValue = (string) aResults[1];
// Dialog.Show(sValue, iBackward);
iStart = iBackward + sValue.Length;
iIndex = iStart;
}
}

if (bLine) {
rtb.Index = iIndex;
rtb.Col = 0;
sText = rtb.RowText;
}
else {
sText = rtb.GetRange(iStart, iEnd);
rtb.Index = iIndex;
}
this.AddMessage(sText);
} // NavigatePriorMatch method

public void FileFind() {
string sContains, sFilter, sDir, sFile;
string[] aLabels, aValues, aFilters, aResults, aFiles, aNames;

sContains = App.ReadData("Contains", "");
sFilter = App.ReadData("Filter", "*.*");
string sTitle = "Open Folder";
sDir = Dialog.OpenFolder(sTitle, "Name", Directory.GetCurrentDirectory());
if (sDir.Length == 0) return;

Directory.SetCurrentDirectory(sDir);
aLabels = new string[] {"&Contains", "&Filter"};
aValues = new string[] {sContains, sFilter};
aResults = Dialog.MultiInput("Criteria", aLabels, aValues);
if (aResults.Length == 0) return;

sContains = aResults[0];
sFilter = aResults[1].Trim();
if (sFilter.Length == 0) sFilter = "*.*";
App.WriteData("Contains", sContains);
App.WriteData("Filter", sFilter);
aFilters = sFilter.Split('|');
sDir = Directory.GetCurrentDirectory();
aFiles = Util.FindInFiles(sContains, sDir, aFilters, false);
if (aFiles.Length == 0) {
Dialog.Show("Alert", "No matches!");
return;
}

aNames = new string[aFiles.Length];
for (int i = 0; i < aNames.Length; i++) aNames[i] = Path.GetFileName(aFiles[i]);
//Array.Sort(aNames, aFiles);
sFile = Dialog.Pick("Pick", aFiles, aNames, true, 0);
if (sFile.Length == 0) return;

OpenOrActivateWindow(sFile, 1);
/*
string[] aNames = null;
string[] aPaths = null;
int iIndex = -1;
string sPath = "";
string sName = "";
string sPaths = "";
string sNames = "";

string sDir = Directory.GetCurrentDirectory();
string sMatch = App.ReadData("FileFindMatch", "");
string sFilter = App.ReadData("FileFindFilter", "");
string sFields = "&Text\t&Filter";
string sValues = sMatch + "\t" + sFilter;
string[] aFields = sFields.Split('\t');
string[] aValues = sValues.Split('\t');
string[] aResults = Dialog.MultiInput("File Find", aFields, aValues);
if (aResults.Length == 0) return;

sMatch = aResults[0];
sFilter = aResults[1];
if (true) {
//if (sDir == App.sFileFindDir && sMatch == App.sFileFindMatch && sFilter == App.sFileFindFilter) {
AddMessage("Repeat search");
//aNames = App.aFileFind;
//iIndex = App.iFileFind + 1;
if (iIndex == -1) iIndex = 0;
}
else {
AddMessage("Please wait");
//ReadOnlyCollection<string> oPaths = null;
string[] aPaths = Util.GetFiles(sDir);
//if (sMatch == "") oPaths = LbcVB.GetFiles(sDir, sFilter);
//else oPaths = LbcVB.FindInFiles(sDir, sMatch, sFilter);
//if (oPaths.Count == 0) {
AddMessage("No files found!");
return;
}
//foreach (string s in oPaths) {
foreach (string s in aPaths) {
sPaths += s + "\n";
sName = s.Substring(sDir.Length + 1);
sNames += sName + "\n";
}
aPaths = sPaths.Trim().Split('\n');
aNames = sNames.Trim().Split('\n');
iIndex = 0;
}

App.WriteData("FileFindMatch", sMatch);
App.WriteData("FileFindFilter", sFilter);
sName = Dialog.Pick("Pick", aNames, true, iIndex);
if (sName.Length == 0) return;

int iName = Array.IndexOf(aNames, sName);
App.WriteData("FileFindDir", sDir);
App.aFileFind = aNames;
App.iFileFind = iName;
sPath = aPaths[iName];
sDir = Path.GetDirectoryName(sPath);
if (sDir.Length == 0) return;
if (Directory.Exists(sDir)) {
OpenOrActivateWindow(sFile, 1);
}
else AddMessage("Folder " + sDir + " not found!");
*/
} //FileFind method

public void CurrentWindows() {
object[] children = this.MdiChildren;
string sTitles = "";
foreach (MdiChild child in children) {
sTitles += child.Text + "\n";
}
string[] aTitles = sTitles.Trim().Split('\n');
string sTitle = Dialog.Pick("Current Windows", aTitles, true, 0);
if (sTitle.Length == 0) return;

int iTitle = Array.IndexOf(aTitles, sTitle);
((MdiChild) children[iTitle]).Activate();
} // CurrentWindows method

public void ExplorerFolder(string sDir) {
string sCommand = sDir;
try {
Process.Start(sCommand);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}
} // ExplorerFolder method

public void CommandPrompt(string sDir) {
string sCommand = Environment.GetEnvironmentVariable("COMSPEC");
sDir = Util.Quote(sDir);
string sParams = "/k cd " + sDir;
try {
Process.Start(sCommand, sParams);
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
return;
}
} // CommandPrompt method

public void WebClientUtilities() {

bool bSort;
int iCount, iIndex;
string sCommand, sExe, sDir, sFile, sName, sValue, sBase, sTitle, sInputFile, sOutputFile, sCodeFile;

sDir = Path.Combine(App.ProgramDir, "WebClient");
string[] aFiles = Directory.GetFiles(sDir, "WebClient_*.py");
iCount = aFiles.Length;
HomerList hlNames = new HomerList();
HomerList hlValues = new HomerList();
for (int iFile = 0; iFile <iCount; iFile++) {
sFile = aFiles[iFile];
sName = Path.GetFileName(sFile);
sBase = Path.GetFileNameWithoutExtension(sName);
sBase = sBase.Substring("WebClient_".Length);
hlNames.Add(sBase);
sValue = Path.Combine(sDir, sName);
hlValues.Add(sValue);
} // for

sBase = App.ReadData("WebClientUtilities", "");
iIndex = -1;
if (sBase.Length > 0) {
iIndex = hlNames.IndexOf(sBase);
}
if (iIndex == -1) iIndex = 0;
sTitle = "Web Client Utilities";
bSort = false;
string[] aNames = hlNames.ToArray();
sName = Dialog.Pick(sTitle, aNames, bSort, iIndex);
if (sName.Length == 0) return;

App.WriteData("WebClientUtilities", sName);
iIndex = hlNames.IndexOf(sName);
sFile = hlValues[iIndex];
sExe = Path.Combine(sDir, "InPy.exe");
sExe = Win32.GetShortPath(sExe);
sInputFile = Path.Combine(App.DataDir, "WebClient.ini");
sBase = Path.GetFileNameWithoutExtension(sFile);
sOutputFile = Path.Combine(App.DataDir, sBase + ".txt");
sCodeFile = sFile;
sCommand = sExe + " " + Util.Quote(sCodeFile) + " " + Util.Quote(sInputFile) + " " + Util.Quote(sOutputFile);
if (File.Exists(sOutputFile)) File.Delete(sOutputFile);
Clipboard.SetText(sCommand);
Util.RunWait(sCommand);
if (File.Exists(sOutputFile))  Process.Start(sOutputFile);
} // WebClientUtilities method

public void BurnToCD() {
string[] aPaths = GetPathsFromDocument();
// MessageBox.Show(String.Join("\n", aPaths), "Paths");
if (aPaths.Length == 0) return;
string sPathList = String.Join("\r\n", aPaths).Trim() + "\r\n";
Util.DeleteFile(App.TempFile, false);
Util.String2File(sPathList, App.TempFile);
string sExe = Path.Combine(App.ProgramDir, "Burn2CD.exe");
Util.Run(sExe + " " + App.TempFile);
} // BurnToCD method

public string[] GetPathsFromDocument() {
HomerRichTextBox rtb = App.Frame.Child.RTB;
List<string> list = new List<string>();
string[] aResults = rtb.Lines;
string sDir = Directory.GetCurrentDirectory();
string sTempDir = "";
for (int i = 0; i < aResults.Length; i++) {
string s = aResults[i].Trim();
if (s.Length == 0) continue;
sTempDir = Path.GetDirectoryName(s);
if (Directory.Exists(sTempDir)) sDir = sTempDir;
if (!File.Exists(s) && !Directory.Exists(s)) s = Path.Combine(sDir, s);
if (File.Exists(s) || Directory.Exists(s)) list.Add(s);
}

aResults = list.ToArray();
if (aResults.Length == 0) {
AddMessage("No files found!");
return aResults;
}

string sText = Util.GetExtensions(aResults);
string sResult = Dialog.Input("Filter", "Extensions", sText).Trim();
if (sResult.Length == 0) return new string[] {};

aResults = Util.GetPathsWithExtensions(aResults, sResult);
if (aResults.Length == 0) AddMessage("No files!");
return aResults;
} // GetPathsFromDocument method

public void AlternateMenu() {
int iChoice = -1;
List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();
string sItems = "";
StringBuilder sb = new StringBuilder();
foreach (ToolStripMenuItem menu in menuMain.Items) {
foreach (object o in menu.DropDownItems) {
ToolStripMenuItem item = o as ToolStripMenuItem;
if (item == null) continue;
if (item == menuHelpAlternateMenu) continue;
// string sText = item.Text.Replace("&", "") + "\t" + item.ShortcutKeyDisplayString;
// if ("1234567890".Contains(sText.Substring(0, 1))) continue;
if (item.IsMdiWindowListEntry) continue;
string[] aSummary = GetKeySummary(item);
string sText = aSummary[0] + " = " + aSummary[1] + ", " + aSummary[2];
sb.Append(sText + "\n");
items.Add(item);
}
}
sItems = sb.ToString();
string[] aItems = sItems.Trim().Split('\n');
string sItem = Dialog.Pick("Alternate Menu", aItems, true, 0);
if (sItem.Length == 0) return;

foreach (ToolStripMenuItem item in items) {
//if (sItem == item.Text.Replace("&", "")) {
// if (sItem == item.Text.Replace("&", "") + "\t" + item.ShortcutKeyDisplayString) {
string[] aSummary = GetKeySummary(item);
string sText = aSummary[0] + " = " + aSummary[1] + ", " + aSummary[2];
if (sItem == sText) {
iChoice = items.IndexOf(item);
break;
}
}
items[iChoice].PerformClick();
} // AlternateMenu method

new void ContextMenu(string sFile) {
MdiChild child = this.Child;

string[] aVerbs = COM.Verbs(sFile);
bool bFound = false;
foreach (string s in aVerbs) {
if (s.Contains("pen Wit")) bFound = true;
if (bFound) break;
} // foreach s
if (!bFound) {
Array.Resize(ref aVerbs, aVerbs.Length + 1);
aVerbs[aVerbs.Length - 1] = "Open With...";
}

string[] aNames = new string[aVerbs.Length];
for (int iVerb = 0; iVerb < aVerbs.Length; iVerb++) aNames[iVerb] = aVerbs[iVerb].Replace("&", "");

string sName = Dialog.Pick("Context Menu", aNames, true, 0);
if (sName.Length == 0) return;

int i = Array.IndexOf(aNames, sName);
string sVerb = aVerbs[i];

// Clipboard.SetText(sVerb);
// if (sVerb.Replace("&", "") == "Open With...") Win32.OpenWith(sFile);
// if (sVerb.Replace("&", "") == "Open With...") Process.Start("Rundll32.exe", "shell32.dll, OpenAs_RunDLL " + Util.Quote(sFile));
if (sVerb.Replace("&", "") == "Open With...") Util.Run("Rundll32.exe shell32.dll, OpenAs_RunDLL " + sFile);
else COM.InvokeVerb(sFile, sVerb);
} // ContextMenu method

public void SendToMenu(string sFile) {
MdiChild child = this.Child;
string sDir = Environment.GetFolderPath(Environment.SpecialFolder.SendTo);
string[]aLinks = Directory.GetFiles(sDir);
string sNameList = "";
foreach (string s in aLinks) sNameList += Path.GetFileNameWithoutExtension(s) + "\n";
string[]aNames = sNameList.Trim().Split('\n');
string sName = Dialog.Pick("SendTo Menu", aNames, true, 0);
if (sName.Length == 0) return;

int i = Array.IndexOf(aNames, sName);
string sLink = aLinks[i];

Process.Start(sLink, sFile);
} // SendToMenu method

public void ListBox_KeyUp(Object sender, KeyEventArgs e) {
ListBox lst = (ListBox) sender;
bool bChecked = false;
if (lst is CheckedListBox) bChecked = true;

if(e.KeyCode == Keys.Space && !e.Alt && !e.Control && e.Shift) {
if (bChecked) {
foreach (int i in ((CheckedListBox) sender).CheckedIndices) {
//Util.Say(lst.Items[i].ToString());
Util.Say(i);
}
e.Handled = true;
}
}
else if(e.KeyCode == Keys.J && ((e.Alt && !e.Control) || (!e.Alt && e.Control)) && !e.Shift) {
string sText = Dialog.Jump;
if (e.Control) {
sText = Dialog.Input("Jump", "Text", sText);
if (sText.Length == 0) return;
}

int iIndex = lst.SelectedIndex;
if (e.Alt || sText == Dialog.Jump) iIndex++;
else iIndex = 0;
Dialog.Jump = sText;

int iCount = lst.Items.Count;
//while (iIndex < iCount && lst.Items[iIndex].ToString().ToLower().IndexOf(sText) == -1) iIndex ++;
while (iIndex < iCount && lst.Items[iIndex].ToString().ToLower().IndexOf(sText) == -1) {
//Util.Say(iIndex);
iIndex ++;
}
if (iIndex < iCount) lst.SelectedIndex = iIndex;
else AddMessage("Not found!");
//lst.Update();
e.Handled = true;
}
else e.Handled = false;
} // ListBox_KeyUp handler

} // MdiFrame class

public class HomerRichTextBox : RichTextBox {
public int OldIndex = -1;
public int OldTextLength = -1;
public static string CR = "\r";
public static string LF = "\n";
public static string LB = LF;
public static string LineBreak = Environment.NewLine;
public static string FF = "\f";
public static string SB = FF + LB;
public static string DD = "----------";
public static string SectionBreak = LB + DD + LB + SB;
public static string EOD = LB + DD + LB + "End of Document" + LB;

public bool IndentMode = false;
public int IndentLevels = 0;
public int Index {
get {
return this.SelectionStart + this.SelectionLength;
}
set {
this.DeselectAll();
this.SelectionStart = value;
this.ScrollToCaret();
this.Update();
this.Refresh();
Application.DoEvents();
System.Threading.Thread.Sleep(100);
//this.OnNotifyMessage();
//this.OnSelectionChanged();
}
} // Index property

public int Row {
get {
return this.GetLineFromCharIndex(this.Index);
}
set {
int iIndex = this.GetFirstCharIndexFromLine(value);
this.DeselectAll();
this.SelectionStart = iIndex;
}
} // Row property

public int Col {
get {
return this.Index - this.GetFirstCharIndexOfCurrentLine();
}
set {
this.Index = GetFirstCharIndexOfCurrentLine() + value;
}
} // Col property

public int RowStart {
get {
return this.GetFirstCharIndexOfCurrentLine();
}
set {
}
} // RowStart property

public string RowText {
get {
return this.GetRowText(this.Row);
}
set {
}
} // RowText property

public int RowEnd {
get {
return this.RowStart + this.RowText.Length;
}
set {
}
} // RowEnd property

public int Line {
get {
return this.Row + 1;
}
set {
this.Row = value - 1;
}
} // Line property

public int Column {
get {
return this.Col + 1;
}
set {
this.Col = value - 1;
}
} // Column property

public double Percent {
get {
if (this.Text.Length == 0) return 0;
else return Math.Round((double) ((100.0 * this.Index) / this.Text.Length), 1);
}
set {
int iIndex = (int) ((this.Text.Length * value) / 100.0);
this.DeselectAll();
this.SelectionStart = iIndex;
}
} // Percent property

public void SetRowAndCol(int iRow, int iCol) {
int iRowStart = this.GetFirstCharIndexFromLine(iRow);
int iIndex = iRowStart + iCol;
this.DeselectAll();
this.SelectionStart = iIndex;
} // SetRowAndCol method

public void SetLineAndColumn(int iLine, int iColumn) {
int iRow = iLine - 1;
int iCol = iColumn - 1;
this.SetRowAndCol(iRow, iCol);
} // SetLineAndColumn method

public string GetRange(int iStart, int iEnd) {
int iLength = iEnd - iStart ;
string sText = this.Text;
return sText.Substring(iStart, iLength);
} // GetRange method

public void ReplaceRange(int iStart, int iEnd, string sText) {
this.DeselectAll();
this.Select(iStart, iEnd - iStart);
this.SelectedText = sText;
this.Index = iStart + sText.Length;
} // ReplaceRange method

public void SelectRange(int iStart, int iEnd) {
this.DeselectAll();
this.Select(iStart, iEnd - iStart);
} // SelectRange method

private int iStartSelection;
public int StartSelection {
get {
return iStartSelection;
}
set {
iStartSelection = value;
}
} // StartSelection property

private int iBookmark;
public int Bookmark {
get {
return iBookmark;
}
set {
iBookmark = value;
}
} // Bookmark property

private string sFindText;
public string FindText {
get {
return sFindText;
}
set {
sFindText = value;
}
} // FindText property

private string sMatchText;
public string MatchText {
get {
return sMatchText;
}
set {
sMatchText = value;
}
} // MatchText property

private string sReplaceText;
public string ReplaceText {
get {
return sReplaceText;
}
set {
sReplaceText = value;
}
} // ReplaceText property

private string sPatternText;
public string PatternText {
get {
return sPatternText;
}
set {
sPatternText = value;
}
} // PatternText property

private string sSubstituteText;
public string SubstituteText {
get {
return sSubstituteText;
}
set {
sSubstituteText = value;
}
} // SubstituteText property

private string sJumpLine;
public string JumpLine {
get {
return sJumpLine;
}
set {
sJumpLine = value;
}
} // JumpLine property

private string sGoPercent;
public string GoPercent {
get {
return sGoPercent;
}
set {
sGoPercent = value;
}
} // GoPercent property

private string sSearchTopic;
public string SearchTopic {
get {
return sSearchTopic;
}
set {
sSearchTopic = value;
}
} // SearchTopic property

private int iOldSelectionStart;
public int OldSelectionStart {
get {
return iOldSelectionStart;
}
set {
iOldSelectionStart = value;
}
} // OldSelectionStart property

private int iOldSelectionLength;
public int OldSelectionLength {
get {
return iOldSelectionLength;
}
set {
iOldSelectionLength = value;
}
} // OldSelectionLength property

public void StoreSelection() {
this.OldSelectionStart = this.SelectionStart;
this.OldSelectionLength = this.SelectionLength;
this.DeselectAll();
this.Index = this.OldSelectionStart + this.OldSelectionLength;
} // StoreSelection method

public void Reselect() {
this.DeselectAll();
this.Select(this.OldSelectionStart, this.OldSelectionLength);
} // Reselect method

public bool IsBottomRow {
get {
int iIndex = GetFirstCharIndexFromLine(this.Row + 1);
return iIndex < 0;
}
set {
}
} // IsBottomRow property

public int BottomRow {
get {
return this.Text.Split('\n').Length - 1;
}
set {
}
} // BottomRow property

public int RowLength {
get {
int iRow = this.Row;
int iStart = GetFirstCharIndexFromLine(iRow);
int iEnd = GetFirstCharIndexFromLine(iRow + 1);
//if (iEnd <= 0) iEnd = iStart;
if (iEnd <= 0) iEnd = this.TextLength;
;
int iLength = iEnd - iStart;
/*
int iLength = this.Lines[this.Row].Length;
if (!this.IsBottomRow) iLength++;
*/
return iLength;
}
set {
}
} // RowLength property

public HomerRichTextBox() {
SectionBreak = App.ReadOption("SectionBreak", SectionBreak);
string s = App.ReadOption("UseIndentModeDefault", "N").Trim().ToUpper();
if (s == "Y" || s == "YES") this.IndentMode = true;
Ini.WriteValue(App.IniFile, "Data", "IndentMode", (this.IndentMode ? "1" : "0"), false);
} // HomerRichTextBox constructor

public int GetIndexRow(int iIndex) {
return this.GetLineFromCharIndex(iIndex);
} // GetIndexRow method

public int GetRowStart(int iRow) {
return this.GetFirstCharIndexFromLine(iRow);
} // GetRowStart method

public string GetRowText(int iRow) {
int iStart = this.GetFirstCharIndexFromLine(iRow);
int iEnd = this.GetFirstCharIndexFromLine(iRow + 1);
// Dialog.Show("iStart " + iStart, "iEnd " + iEnd);
if (iEnd == -1) iEnd = this.Text.Length;
else iEnd --;
return this.GetRange(iStart, iEnd);
} // GetRowText method

public bool SetWrap(bool bWrap) {
bool bOldWrap = this.WordWrap;
bool bModified = this.Modified;
this.WordWrap = bWrap;
this.Modified = bModified;
return bOldWrap;
} // SetWrap method

public bool SetGuard(bool bGuard) {
bool bOldGuard = this.ReadOnly;
bool bModified = this.Modified;
this.ReadOnly = bGuard;
this.Modified = bModified;
return bOldGuard;
} // SetGuard method

protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
return App.Frame.ProcessCmdKey_Helper(ref msg, keyData);
} // ProcessCmdKey handler

} // HomerRichTextBox class

public class ListForm : Form {

public ListBox lst;
public DataTable tbl;
public BindingSource bs ;
public string Filter;
public DataTable tblDefault = null;
public int CheckFirst = -1;
public int CheckLast = -1;

protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
ListBox lst = this.lst;
bool bChecked = false;
if (lst is CheckedListBox) bChecked = true;

switch (keyData) {
case Keys.Alt | Keys.A :
App.Frame.AddMessage("Alpha order");
bs.Sort = "Item asc";
bs.Position = 0;
return true;
case Keys.Alt | Keys.Shift | Keys.A :
App.Frame.AddMessage("Reverse alpha order");
bs.Sort = "Item desc";
bs.Position = 0;
return true;
case Keys.Alt | Keys.D :
App.Frame.AddMessage("Default order");
if (this.tblDefault == null) {
this.tblDefault = new DataTable();
this.tblDefault.Columns.Add("Item", typeof(string));
this.tblDefault.Columns.Add("Value", typeof(string));
for (int i = 0; i < tbl.Rows.Count; i++)  this.tblDefault.Rows.Add(tbl.Rows[i][0].ToString(), tbl.Rows[i][1].ToString());
}

tbl = this.tblDefault;
bs.Sort = "";
bs.Position = 0;
return true;
case Keys.Alt | Keys.Shift | Keys.D :
App.Frame.AddMessage("Reverse default order");
if (this.tblDefault == null) {
this.tblDefault = new DataTable();
this.tblDefault.Columns.Add("Item", typeof(string));
this.tblDefault.Columns.Add("Value", typeof(string));
for (int i = 0; i < tbl.Rows.Count; i++)  this.tblDefault.Rows.Add(tbl.Rows[i][0].ToString(), tbl.Rows[i][1].ToString());
}

DataTable tblNew = new DataTable();
tblNew.Columns.Add("Item", typeof(string));
tblNew.Columns.Add("Value", typeof(string));
for (int i = this.tblDefault.Rows.Count -1; i >= 0; i--) tblNew.Rows.Add(this.tblDefault.Rows[i][0].ToString(), tblDefault.Rows[i][1].ToString());
tbl = tblNew;
//bs = new BindingSource();
bs.DataSource = tbl;
//this.BS = bs;
//bs.ResetBindings();
bs.Sort = "";
bs.Position = 0;
return true;
case Keys.Alt | Keys.Delete :
App.Frame.AddMessage((bs.Position + 1) + " of " + tbl.DefaultView.Count);
return true;
case Keys.Shift | Keys.Space :
if (bChecked) {
int iChecked = ((CheckedListBox) lst).CheckedItems.Count;
if (iChecked == 0) App.Frame.AddMessage("No items checked!");
else App.Frame.AddMessage("Checked" + iChecked);
List<int> listChecked = new List<int>();
foreach (int i in ((CheckedListBox) lst).CheckedIndices) listChecked.Add(i);
listChecked.Sort();
foreach (int i in listChecked) App.Frame.AddMessage(tbl.DefaultView[i][0].ToString());
}
else {
App.Frame.AddMessage("Selected");
foreach (int i in lst.SelectedIndices) App.Frame.AddMessage(tbl.DefaultView[i][0].ToString());
}
return true;
case Keys.Space :
//if (!bChecked || this.ActiveControl is Button) return base.ProcessCmdKey (ref msg, keyData);
if (!bChecked || !(this.ActiveControl is ListBox)) return base.ProcessCmdKey (ref msg, keyData);

{
int i = bs.Position;
bool b = ((CheckedListBox) lst).GetItemChecked(i);
((CheckedListBox) lst).SetItemChecked(i, !b);
return true;
}
case Keys.Control | Keys.Home :
if (!bChecked || !(this.ActiveControl is ListBox)) return base.ProcessCmdKey (ref msg, keyData);

int iStart = -1;
for (int i = 0; i < tbl.DefaultView.Count; i++) {
if (((CheckedListBox) lst).GetItemChecked(i)) {
iStart = i;
break;
}
}

if (iStart >= 0) bs.Position = iStart;
else App.Frame.AddMessage("Not found!");
return true;
case Keys.Control | Keys.End :
if (!bChecked || !(this.ActiveControl is ListBox)) return base.ProcessCmdKey (ref msg, keyData);

int iEnd = -1;
for (int i = tbl.DefaultView.Count - 1; i >= 0; i--) {
if (((CheckedListBox) lst).GetItemChecked(i)) {
iEnd = i;
break;
}
}

if (iEnd >= 0) bs.Position = iEnd;
else App.Frame.AddMessage("Not found!");
return true;
case Keys.Control | Keys.Down :
if (!bChecked || !(this.ActiveControl is ListBox)) return base.ProcessCmdKey (ref msg, keyData);

int iNext = -1;
for (int i = bs.Position + 1; i < tbl.DefaultView.Count; i++) {
if (((CheckedListBox) lst).GetItemChecked(i)) {
iNext = i;
break;
}
}

if (iNext >= 0) bs.Position = iNext;
else App.Frame.AddMessage("Not found!");
return true;
case Keys.F8 :
case Keys.Shift | Keys.F8 :
case Keys.Alt | Keys.Shift | Keys.F8 :
case Keys.Shift | Keys.Clear :
case Keys.Alt | Keys.Shift | Keys.Clear :
case Keys.Shift | Keys.Down :
case Keys.Alt | Keys.Shift | Keys.Down :
case Keys.Shift | Keys.Up :
case Keys.Alt | Keys.Shift | Keys.Up :
case Keys.Shift | Keys.End :
case Keys.Alt | Keys.Shift | Keys.End :
case Keys.Shift | Keys.Home :
case Keys.Alt | Keys.Shift | Keys.Home :
if (!bChecked || !(this.ActiveControl is ListBox)) return base.ProcessCmdKey (ref msg, keyData);

bool bState;
int iFirst, iLast;
int iAfter = bs.Position;
string sKey = Util.Key2String(keyData);

if (keyData == Keys.F8) {
App.Frame.AddMessage("Start Check or Uncheck");
this.CheckFirst = iAfter;
return true;
}
else if (keyData == (Keys.Shift | Keys.F8)) {
App.Frame.AddMessage("Complete Check");
bState = true;
iFirst = this.CheckFirst;
iLast = iAfter;
}
else if (keyData == (Keys.Alt | Keys.Shift | Keys.F8)) {
App.Frame.AddMessage("Complete Uncheck");
bState = false;
iFirst = this.CheckFirst;
iLast = iAfter;
}
else {
if (sKey.IndexOf("Alt+") >= 0) bState = false;
else bState = true;

if (sKey.IndexOf("+End") >= 0) {
iLast = tbl.DefaultView.Count - 1;
iAfter = iLast;
}
else iLast = iAfter;

if (sKey.IndexOf("+Home") >= 0) {
iFirst = 0;
iAfter = iFirst;
}
else iFirst = iAfter;

if (sKey.IndexOf("+Up") >= 0) iAfter--;
if (sKey.IndexOf("+Down") >= 0) iAfter++;

}

if (iFirst > iLast) Util.Swap(ref iFirst, ref iLast);
for (int iPosition = iFirst; iPosition <= iLast; iPosition ++) ((CheckedListBox) lst).SetItemChecked(iPosition, bState);
if (iAfter != bs.Position && iAfter >=0 && iAfter < tbl.DefaultView.Count) bs.Position = iAfter;
return true;
case Keys.Control | Keys.Up :
if (!bChecked || !(this.ActiveControl is ListBox)) return base.ProcessCmdKey (ref msg, keyData);

int iPrevious = -1;
for (int i = bs.Position - 1; i >= 0; i--) {
if (((CheckedListBox) lst).GetItemChecked(i)) {
iPrevious = i;
break;
}
}

if (iPrevious >= 0) bs.Position = iPrevious;
else App.Frame.AddMessage("Not found!");
return true;
case Keys.Control | Keys.A :
if (!bChecked || !(this.ActiveControl is ListBox)) return base.ProcessCmdKey (ref msg, keyData);

if (bChecked) {
App.Frame.AddMessage("Check All");
for (int i = 0; i < tbl.DefaultView.Count; i++) ((CheckedListBox) lst).SetItemChecked(i, true);
}
return true;
case Keys.Control | Keys.Shift | Keys.A :
if (!bChecked || !(this.ActiveControl is ListBox)) return base.ProcessCmdKey (ref msg, keyData);

if (bChecked) {
App.Frame.AddMessage("Uncheck All");
for (int i = 0; i < tbl.DefaultView.Count; i++) ((CheckedListBox) lst).SetItemChecked(i, false);
}
return true;
case Keys.Control | Keys.F :
case Keys.Control | Keys.Shift | Keys.F :
string sFilterSql = "";
string sFilter = "";
if (keyData == (Keys.Control | Keys.Shift | Keys.F)) App.Frame.AddMessage("Clear filter");
else {
Dialog.hashFilter.TryGetValue(this.Text, out sFilter);
sFilter = Dialog.Input("Filter", "Text", sFilter);
if (sFilter.Length == 0) return true;
//sFilterSql = "Item like '" + sFilter + "'";
sFilterSql = GetFilterSql(sFilter);
}

string sTemp = bs.Filter;
try {
bs.Filter = sFilterSql;
this.Filter = sFilter;
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
bs.Filter = sTemp;
return true;
}

bs.Position = 0;
App.Frame.AddMessage(Util.Pluralize(bs.Count, "item"));

//if (keyData == (Keys.Control | Keys.F)) {
if (Dialog.hashFilter.ContainsKey(this.Text)) Dialog.hashFilter.Remove(this.Text);
if (sFilter.Trim().Length > 0) Dialog.hashFilter.Add(this.Text, sFilter);
//}
return true;
case Keys.Control | Keys.J :
case Keys.Alt | Keys.J :
string sTitle = this.Text;
string sJump = "";
Dialog.hashJump.TryGetValue(sTitle, out sJump);
if (keyData == (Keys.Control | Keys.J)) {
sJump = Dialog.Input("Jump", "Text", sJump);
if (sJump.Length == 0) return true;
}

int iIndex = bs.Position;
if (keyData == (Keys.Alt | Keys.J) || sJump == Dialog.Jump) iIndex++;
else iIndex = 0;
if (Dialog.hashJump.ContainsKey(sTitle)) Dialog.hashJump.Remove(sTitle);
Dialog.hashJump.Add(sTitle, sJump);

int iCount = tbl.DefaultView.Count;
//while (iIndex < iCount && tbl.DefaultView[iIndex].ToString().ToLower().IndexOf(sJump) == -1) iIndex ++;
/*
while (iIndex < iCount && tbl.DefaultView[iIndex].ToString().ToLower().IndexOf(sJump) == -1) {
//App.Frame.AddMessage(iIndex);
iIndex ++;
}
*/
while (iIndex < iCount && tbl.DefaultView[iIndex][0].ToString().ToLower().IndexOf(sJump) == -1) {
iIndex ++;
}
//if (iIndex < iCount) bs.Position = iIndex;
if (iIndex < iCount) bs.Position = iIndex;
else App.Frame.AddMessage("Not found!");
//lst.Update();
return true;
}

return base.ProcessCmdKey (ref msg, keyData);
} // ProcessCmdKey handler

public string GetFilterSql(string sText) {
if (sText == null) sText = "";
sText = sText.Trim();
if (sText == "" || sText == "*") return "";
string[] aFilters = sText.Split('|');
string s = "";
for (int i =0; i < aFilters.Length; i++) {
if (i == 0) s += "(";
string[] a = aFilters[i].Split('*');
for (int j = 0; j < a.Length; j++) {
string sPrefix = "";
string sSuffix = "";
if (j == 0) s += " (";
if (a[j].Length > 0) {
if (j > 0) sPrefix = "*";
if (j < a.Length - 1) sSuffix = "*";
s += "Item like '" + sPrefix + a[j] + sSuffix + "'";
}

if (j == a.Length - 1) s += ") ";
else s += " and ";
}
if (i == aFilters.Length - 1) s+=")";
else s += " or ";
}

s = s.Replace("( and ", "(");
s = s.Replace(" and )", ")");
s = s.Replace("**", "*");
s = s.Replace("  ", " ");
s = s.Replace("( ", "(");
s = s.Replace(" )", ")");
s = s.Trim();
return s;
} // GetFilterSql method

} // ListForm class

public class Dialog {
public static string Jump = "";
public static Dictionary<string, string> hashItem = new Dictionary<string, string>();
public static Dictionary<string, string> hashFilter = new Dictionary<string, string>();
public static Dictionary<string, string> hashSort = new Dictionary<string, string>();
public static Dictionary<string, string> hashJump = new Dictionary<string, string>();

public static int PickEncoding(string sTitle, int iDefault) {
EncodingInfo[] eis = Encoding.GetEncodings();
int iCount = eis.Length;
string[] aNames = new string[iCount];
int[] aCodes = new int[iCount];
for (int i = 0; i < iCount; i++) {
EncodingInfo ei = eis[i];
Encoding en = ei.GetEncoding();
aNames[i] = en.EncodingName + " = " + en.CodePage;
aCodes[i] = en.CodePage;
}

Array.Sort(aNames, aCodes);
int iPosition = Array.IndexOf(aCodes, Encoding.Default.CodePage);
if (iPosition == -1) iPosition = 0;

if (sTitle.Length == 0) sTitle = "Pick Encoding";
string sItem = "";
if (hashItem.TryGetValue(sTitle, out sItem)) iPosition = 0;

string sName = Dialog.Pick(sTitle, aNames, false, iPosition);
if (sName.Length == 0) return -1;

iPosition = Array.IndexOf(aNames, sName);
int iCodePage = aCodes[iPosition];
return iCodePage;
} // PickEncoding method

public static string OpenFile(string sTitle, string sPath) {
string sReturn = "";
string sDir;

OpenFileDialog dlg = new OpenFileDialog();
if (sTitle.Length > 0) dlg.Title = sTitle;
if (File.Exists(sPath)) {
dlg.FileName = sPath;
sDir = Path.GetDirectoryName(sPath);
}
else sDir = sPath;

if (!Directory.Exists(sDir)) sDir = Directory.GetCurrentDirectory();
dlg.InitialDirectory = sDir;

string sFilter = "All files (*.*)|*.*|Text files (*.txt)|*.txt|Rich Text Format files (*.rtf)|*.rtf";
string sCompiler = App.ReadData("Compiler", "Default");
string sExtensionDefault = App.ReadOption("ExtensionDefault", "");
if (sCompiler != "Default") sFilter = sCompiler + " files (*." + sExtensionDefault + ")|*." + sExtensionDefault + "|" + sFilter;
dlg.Filter = sFilter;
dlg.FilterIndex = 1;
dlg.ValidateNames = true;
dlg.CheckPathExists = true;

if (dlg.ShowDialog() == DialogResult.OK) sReturn = dlg.FileName;
dlg.Dispose();
return sReturn;
} // OpenFile method

public static string SaveFile(string sTitle, string sPath) {
string sReturn = "";
string sDir;

SaveFileDialog dlg = new SaveFileDialog();
if (sTitle.Length > 0) dlg.Title = sTitle;
if (Directory.Exists(sPath)) sDir = sPath;
else {
dlg.FileName = sPath;
sDir = Path.GetDirectoryName(sPath);
}

if (Directory.Exists(sDir)) dlg.InitialDirectory = sDir;

string sFilter = "All files (*.*)|*.*|Text files (*.txt)|*.txt|Rich Text Format files (*.rtf)|*.rtf";
string sCompiler = App.ReadData("Compiler", "Default");
string sExtensionDefault = App.ReadOption("ExtensionDefault", "");
if (sCompiler != "Default") sFilter = sCompiler + " files (*." + sExtensionDefault + ")|*." + sExtensionDefault + "|" + sFilter;
dlg.Filter = sFilter;
dlg.FilterIndex = 1;
dlg.CheckPathExists = true;
dlg.SupportMultiDottedExtensions = true;

dlg.CreatePrompt = false;
dlg.ValidateNames = true;
dlg.AddExtension = true;
//dlg.AddExtension = false;
//dlg.DefaultExt = "txt";
dlg.DefaultExt = App.ReadOption("ExtensionDefault", "");

if (dlg.ShowDialog() == DialogResult.OK) sReturn = dlg.FileName;
dlg.Dispose();
return sReturn;
} // SaveFile method

public static string OldInput(string sTitle, string sLabel, string sValue) {
return Interaction.InputBox(sLabel, sTitle, sValue, -1, -1);
} // Input method

public static string Input(string sTitle, string sLabel, string sValue) {
string[] aLabel = new string[] {sLabel};
string[] aValue = new string[] {sValue};
string[] aReturn = MultiInput(sTitle, aLabel, aValue);
//string sReturn = aReturn[0];
string sReturn = "";
if (aReturn != null && aReturn.Length > 0) sReturn = aReturn[0];
return sReturn;
} // Input method

public static string[] MultiInput(string sTitle, string[] aLabel, string[] aValue) {
Form frm = new Form();
frm.SuspendLayout();
frm.AutoSize = true;
frm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
frm.AutoScroll = true;

FlowLayoutPanel flpMain = new FlowLayoutPanel();
flpMain.SuspendLayout();
flpMain.AutoSize = true;
flpMain.AutoSizeMode = AutoSizeMode.GrowAndShrink;
//flpMain.AutoScroll = true;
flpMain.FlowDirection = FlowDirection.TopDown;

TableLayoutPanel tlpFields = new TableLayoutPanel();
tlpFields.SuspendLayout();
tlpFields.Anchor = AnchorStyles.None;
tlpFields.AutoSize = true;
tlpFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
//tlpFields.AutoScroll = true;

tlpFields.ColumnCount = 2;

for (int i = 0; i < tlpFields.ColumnCount; i++) {
tlpFields.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
}

tlpFields.RowCount = aLabel.Length;

for (int i = 0; i < tlpFields.RowCount; i++) {
tlpFields.RowStyles.Add(new RowStyle(SizeType.AutoSize));
Label lbl = new Label();
lbl.AutoSize = true;
lbl.Text = aLabel[i] + ":";
lbl.AccessibleName = lbl.Text.Replace("&", "");
TextBox txt = new TextBox();
txt.Width *= 2;
txt.Text = aValue[i];
txt.AccessibleName = lbl.AccessibleName;
txt.SelectAll();
tlpFields.Controls.AddRange(new Control[] {lbl, txt});
}
tlpFields.ResumeLayout();

FlowLayoutPanel flpButtons = new FlowLayoutPanel();
flpButtons.SuspendLayout();
flpButtons.Anchor = AnchorStyles.None;
flpButtons.AutoSize = true;
flpButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
//flpButtons.AutoScroll = true;
flpButtons.FlowDirection = FlowDirection.LeftToRight;

Button btnOK = new Button();
btnOK.Text = "OK";
btnOK.AccessibleName = btnOK.Text;

StringBuilder sb = new StringBuilder();
btnOK.Click += delegate(object o, EventArgs e) {
foreach (Control ctl in tlpFields.Controls) {
if (ctl.GetType() == typeof(TextBox)) sb.Append(ctl.Text + "\n");
}
frm.Close();
};

Button btnCancel = new Button();
btnCancel.Text = "Cancel";
btnCancel.AccessibleName = btnCancel.Text;
btnCancel.Click += delegate(object o, EventArgs e) {
/*Util.Say("Cancel");*/ frm.Close();
};

flpButtons.Controls.AddRange(new Control[] {btnOK, btnCancel});
flpButtons.ResumeLayout();

flpMain.Controls.AddRange(new Control[] {tlpFields, flpButtons});
flpMain.ResumeLayout();

frm.AcceptButton = btnOK;
frm.CancelButton = btnCancel;
frm.StartPosition = FormStartPosition.CenterParent;
frm.Text = sTitle;
frm.Controls.Add(flpMain);
frm.ResumeLayout();
frm.ShowDialog();
frm.Dispose();

string s = sb.ToString();
//s = s.TrimEnd('\n');
if (s.Length > 0) s = s.Substring(0, s.Length - 1);
string[] aReturn = {};
if (s.Length > 0) aReturn = s.Split('\n');
return aReturn;
} // MultiInput method

public static string Pick(string sTitle, string[] aValue, bool bSort) {
string[] aDisplay = null;
int iIndex = 0;
return Pick(sTitle, aValue, aDisplay, bSort, iIndex);
} // Pick method

public static string[] MultiPick(string sTitle, string[] aValues, int[] aSelect, bool bSort) {
List<string> listResults = new List<string>();

ListForm frm = new ListForm();
frm.SuspendLayout();
frm.AutoSize = true;
frm.AutoSizeMode = AutoSizeMode.GrowAndShrink;

FlowLayoutPanel flpMain = new FlowLayoutPanel();
flpMain.SuspendLayout();
flpMain.AutoSize = true;
flpMain.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpMain.FlowDirection = FlowDirection.TopDown;

FlowLayoutPanel flpInput = new FlowLayoutPanel();
flpInput.SuspendLayout();
flpInput.Anchor = AnchorStyles.None;
flpInput.AutoSize = true;
flpInput.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpInput.FlowDirection = FlowDirection.LeftToRight;

ListBox lst = new ListBox();
frm.lst = lst;
lst.SelectionMode = SelectionMode.MultiSimple;
if (bSort) lst.Sorted = true;
lst.Items.AddRange(aValues);

for (int i = 0; i < aSelect.Length; i ++) {
lst.SetSelected(aSelect[i], true);
}

flpInput.Controls.AddRange(new Control[] {lst});
flpInput.ResumeLayout();

FlowLayoutPanel flpButtons = new FlowLayoutPanel();
flpButtons.SuspendLayout();
flpButtons.Anchor = AnchorStyles.None;
flpButtons.AutoSize = true;
flpButtons.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpButtons.FlowDirection = FlowDirection.LeftToRight;

Button btnOK = new Button();

btnOK.Click += delegate(object o, EventArgs e) {
foreach (int i in lst.SelectedIndices) {
listResults.Add(lst.Items[i].ToString());
}
frm.Close();
};

btnOK.Text = "OK";
btnOK.AccessibleName = btnOK.Text;

Button btnCancel = new Button();
btnCancel.Click += delegate(object o, EventArgs e) {
Util.Say("Cancel");
frm.Close();
};
btnCancel.Text = "Cancel";
btnCancel.AccessibleName = btnCancel.Text;

flpButtons.Controls.AddRange(new Control[] {btnOK, btnCancel});
flpButtons.ResumeLayout();

flpMain.Controls.AddRange(new Control[] {flpInput, flpButtons});
flpMain.ResumeLayout();

frm.AcceptButton = btnOK;
frm.CancelButton = btnCancel;
frm.StartPosition = FormStartPosition.CenterParent;
frm.Text = sTitle;
frm.Controls.Add(flpMain);
frm.ResumeLayout();
//frm.Shown += delegate(object sender, EventArgs e) {frm.Activate();};
//frm.Shown += delegate(object sender, EventArgs e) {SetForegroundWindow((int) frm.Handle); };
frm.ShowDialog();
frm.Dispose();
string[] aResults = listResults.ToArray();
return aResults;

} // MultiPick method

public static string[] MultiCheck(string sTitle, string[] aValues, int[] aSelect, bool bSort, int iIndex) {
string[] aDisplay = null;
return MultiCheck(sTitle, aDisplay, aValues, aSelect, bSort, iIndex);
} // MultiCheck method

public static string[] MultiCheck(string sTitle, string[] aDisplay, string[] aValues, int[] aSelect, bool bSort, int iIndex) {
List<string> listResults = new List<string>();

ListForm frm = new ListForm();
frm.SuspendLayout();
frm.AutoSize = true;
frm.AutoSizeMode = AutoSizeMode.GrowAndShrink;

FlowLayoutPanel flpMain = new FlowLayoutPanel();
flpMain.SuspendLayout();
flpMain.AutoSize = true;
flpMain.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpMain.FlowDirection = FlowDirection.TopDown;

FlowLayoutPanel flpInput = new FlowLayoutPanel();
flpInput.SuspendLayout();
flpInput.Anchor = AnchorStyles.None;
flpInput.AutoSize = true;
flpInput.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpInput.FlowDirection = FlowDirection.LeftToRight;

CheckedListBox lst = new CheckedListBox();
frm.lst = lst;
lst.Sorted = false;
//lst.SelectionMode = SelectionMode.MultiSimple;
lst.SelectionMode = SelectionMode.One;

string[] aTemp = (string[]) aValues.Clone();
if (aDisplay == null) {
if (bSort) Array.Sort(aValues, new CaseInsensitiveComparer());
aDisplay = (string[]) aValues.Clone();
}
else if (bSort) Array.Sort(aDisplay, aValues);

DataTable tbl = new DataTable();
frm.tbl = tbl;
tbl.Columns.Add("Item", typeof(string));
tbl.Columns.Add("Value", typeof(string));
BindingSource bs = new BindingSource();
frm.bs = bs;
bs.DataSource = tbl;
lst.DataSource = bs;
lst.DisplayMember = "Item";
for (int i = 0; i < aDisplay.Length; i++) tbl.Rows.Add(aDisplay[i], aValues[i]);

/*
if (bSort) lst.Sorted = true;
lst.Items.AddRange(aValues);
lst.SelectedIndex = iIndex;
*/

//for (int i = 0; i < aSelect.Length; i ++) lst.SetItemChecked(aSelect[i], true);
//bs.Position = iIndex;

flpInput.Controls.AddRange(new Control[] {lst});
flpInput.ResumeLayout();

FlowLayoutPanel flpButtons = new FlowLayoutPanel();
flpButtons.SuspendLayout();
flpButtons.Anchor = AnchorStyles.None;
flpButtons.AutoSize = true;
flpButtons.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpButtons.FlowDirection = FlowDirection.LeftToRight;

Button btnOK = new Button();

btnOK.Click += delegate(object o, EventArgs e) {
//foreach (int i in lst.SelectedIndices) {
//foreach (int i in lst.CheckedIndices) {
//listResults.Add(lst.Items[i].ToString());

foreach (int i in lst.CheckedIndices) listResults.Add(((DataRowView) bs[i])[1].ToString());
frm.Close();
for (int i = 0; i < aTemp.Length; i++) aValues[i] = aTemp[i];
};

btnOK.Text = "OK";
btnOK.AccessibleName = btnOK.Text;

Button btnCancel = new Button();
btnCancel.Click += delegate(object o, EventArgs e) {
Util.Say("Cancel");
frm.Close();
};
btnCancel.Text = "Cancel";
btnCancel.AccessibleName = btnCancel.Text;

flpButtons.Controls.AddRange(new Control[] {btnOK, btnCancel});
flpButtons.ResumeLayout();

flpMain.Controls.AddRange(new Control[] {flpInput, flpButtons});
flpMain.ResumeLayout();

frm.AcceptButton = btnOK;
frm.CancelButton = btnCancel;
frm.StartPosition = FormStartPosition.CenterParent;
frm.Text = sTitle;
frm.Controls.Add(flpMain);
frm.ResumeLayout();
//frm.Shown += delegate(object sender, EventArgs e) {frm.Activate();};
//frm.Shown += delegate(object sender, EventArgs e) {SetForegroundWindow((int) frm.Handle); };
frm.Load += delegate(object sender, EventArgs e) {
if (iIndex == 0) {
for (int i = 0; i < aSelect.Length; i ++) lst.SetItemChecked(aSelect[i], true);

string sFilter = "";
if (hashFilter.TryGetValue(sTitle, out sFilter) && sFilter != null && sFilter != "") {
string sFilterSql = frm.GetFilterSql(sFilter);
bs.Filter = sFilterSql;
if (bs.Count == 0) bs.Filter = "";
else App.Frame.AddMessage("Filter " + sFilter);
}

string sSort = "";
if (hashSort.TryGetValue(sTitle, out sSort) && sSort != null && sSort != "") {
bs.Sort = sSort;
if (sSort.EndsWith(" asc")) App.Frame.AddMessage("Alpha order");
else if (sSort.EndsWith(" desc")) App.Frame.AddMessage("Reverse alpha order");
}

string sItem = "";
if (hashItem.TryGetValue(sTitle, out sItem)) {
//iIndex = lst.FindStringExact(sItem);
iIndex = -1;
for (int i = 0; i < bs.Count; i++) {
DataRowView row = (DataRowView) bs[i];
if (row[1].ToString() == sItem) {
iIndex = i;
break;
}
}
} // iIndex == 0

if (iIndex == -1) iIndex = 0;
if (iIndex > 0) Util.Say("Item " + (iIndex + 1).ToString());
//lst.SelectedIndex = iIndex;
bs.Position = iIndex;
}
};

frm.ShowDialog();
frm.Dispose();
string[] aResults = listResults.ToArray();

if (aResults.Length > 0) {
if (hashFilter.ContainsKey(sTitle)) hashFilter.Remove(sTitle);
string sFilter = frm.Filter;
hashFilter.Add(sTitle, sFilter);

if (hashSort.ContainsKey(sTitle)) hashSort.Remove(sTitle);
string sSort = bs.Sort;
hashSort.Add(sTitle, sSort);

if (hashItem.ContainsKey(sTitle)) hashItem.Remove(sTitle);
//sItem = lst.SelectedItem.ToString();
string sItem = ((DataRowView) bs.Current)[1].ToString();
hashItem.Add(sTitle, sItem);
}

return aResults;

} // MultiCheck method

public static string Pick(string sTitle, string[] aValue, bool bSort, int iIndex) {
string[] aDisplay = null;
return Pick(sTitle, aValue, aDisplay, bSort, iIndex);
} // Pick method

public static string Pick(string sTitle, string[] aValue, string[] aDisplay, bool bSort, int iIndex) {
string sReturn = "";

ListForm frm = new ListForm();
frm.SuspendLayout();
frm.AutoSize = true;
frm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
frm.AutoScroll = true;

FlowLayoutPanel flpMain = new FlowLayoutPanel();
flpMain.SuspendLayout();
flpMain.AutoSize = true;
flpMain.AutoSizeMode = AutoSizeMode.GrowAndShrink;
//flpMain.AutoScroll = true;
flpMain.FlowDirection = FlowDirection.TopDown;

FlowLayoutPanel flpInput = new FlowLayoutPanel();
flpInput.SuspendLayout();
flpInput.Anchor = AnchorStyles.None;
flpInput.AutoSize = true;
flpInput.AutoSizeMode = AutoSizeMode.GrowAndShrink;
//flpInput.AutoScroll = true;
flpInput.FlowDirection = FlowDirection.LeftToRight;

ListBox lst = new ListBox();
frm.lst = lst;
lst.Sorted = false;

/*
if (aDisplay == null) lst.Items.AddRange(aValue);
else {
for (int i = 0; i < aDisplay.Length; i++) {
lst.Items.Add(aDisplay[i]);
Support.SetItemData(lst, i, i);
}
}
if (bSort) lst.Sorted = true;
lst.SelectedIndex = iIndex;
*/

string[] aTemp = (string[]) aValue.Clone();
if (aDisplay == null) {
if (bSort) Array.Sort(aValue, new CaseInsensitiveComparer());
aDisplay = (string[]) aValue.Clone();
}
else if (bSort) Array.Sort(aDisplay, aValue);

DataTable tbl = new DataTable();
frm.tbl = tbl;
tbl.Columns.Add("Item", typeof(string));
tbl.Columns.Add("Value", typeof(string));
BindingSource bs = new BindingSource();
frm.bs = bs;
bs.DataSource = tbl;
lst.DataSource = bs;
lst.DisplayMember = "Item";
for (int i = 0; i < aDisplay.Length; i++) tbl.Rows.Add(aDisplay[i], aValue[i]);

flpInput.Controls.Add(lst);
flpInput.ResumeLayout();

FlowLayoutPanel flpButtons = new FlowLayoutPanel();
flpButtons.SuspendLayout();
flpButtons.Anchor = AnchorStyles.None;
flpButtons.AutoSize = true;
flpButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
//flpButtons.AutoScroll = true;
flpButtons.FlowDirection = FlowDirection.LeftToRight;

Button btnOK = new Button();
btnOK.Text = "OK";
btnOK.AccessibleName = btnOK.Text;

btnOK.Click += delegate(object o, EventArgs e) {
/*
if (aDisplay == null) sReturn = lst.Text;
else {
int iItem = lst.SelectedIndex;
int iValue = Support.GetItemData(lst, iItem);
sReturn = aValue[iValue];
}
*/

sReturn = ((DataRowView) bs.Current)[1].ToString();
frm.Close();
for (int i = 0; i < aTemp.Length; i++) aValue[i] = aTemp[i];
};

Button btnCancel = new Button();
btnCancel.Text = "Cancel";
btnCancel.AccessibleName = btnCancel.Text;
btnCancel.Click += delegate(object o, EventArgs e) {
/*Util.Say("Cancel");*/ frm.Close();
};

flpButtons.Controls.AddRange(new Control[] {btnOK, btnCancel});
flpButtons.ResumeLayout();

flpMain.Controls.AddRange(new Control[] {flpInput, flpButtons});
flpMain.ResumeLayout();

frm.AcceptButton = btnOK;
frm.CancelButton = btnCancel;
frm.StartPosition = FormStartPosition.CenterParent;
frm.Text = sTitle;
frm.Controls.Add(flpMain);
frm.ResumeLayout();

/*
if (iIndex == 0) {
string sItem = "";
if (hashItem.TryGetValue(sTitle, out sItem)) {
if (aDisplay == null) iIndex = lst.Items.IndexOf(sItem);
else {
iIndex = Array.IndexOf(aValue, sItem);
if (iIndex >= 0) iIndex = lst.FindStringExact(aDisplay[iIndex]);
}
}
}

if (iIndex == -1) iIndex = 0;
if (iIndex > 0) Util.Say("Item " + (iIndex + 1).ToString());
//lst.SelectedIndex = iIndex;
bs.Position = iIndex;
*/

frm.Load += delegate(object sender, EventArgs e) {
if (iIndex == 0) {
string sFilter = "";
if (hashFilter.TryGetValue(sTitle, out sFilter) && sFilter != null && sFilter != "") {
string sFilterSql = frm.GetFilterSql(sFilter);
bs.Filter = sFilterSql;
if (bs.Count == 0) bs.Filter = "";
else App.Frame.AddMessage("Filter " + sFilter);
}

string sSort = "";
if (hashSort.TryGetValue(sTitle, out sSort) && sSort != null && sSort != "") {
bs.Sort = sSort;
if (sSort.EndsWith(" asc")) App.Frame.AddMessage("Alpha order");
else if (sSort.EndsWith(" desc")) App.Frame.AddMessage("Reverse alpha order");
}

string sItem = "";
if (hashItem.TryGetValue(sTitle, out sItem)) {
//iIndex = lst.FindStringExact(sItem);
iIndex = -1;
for (int i = 0; i < bs.Count; i++) {
DataRowView row = (DataRowView) bs[i];
if (row[1].ToString() == sItem) {
iIndex = i;
break;
}
}
//Dialog.Show(iIndex, sItem);
} // iIndex == 0

if (iIndex == -1) iIndex = 0;
if (iIndex > 0) Util.Say("Item " + (iIndex + 1).ToString());
//lst.SelectedIndex = iIndex;
bs.Position = iIndex;
}
bs.Position = iIndex;
};

frm.ShowDialog();
frm.Dispose();

if (sReturn.Length > 0) {
if (hashFilter.ContainsKey(sTitle)) hashFilter.Remove(sTitle);
string sFilter = frm.Filter;
hashFilter.Add(sTitle, sFilter);

if (hashSort.ContainsKey(sTitle)) hashSort.Remove(sTitle);
string sSort = bs.Sort;
hashSort.Add(sTitle, sSort);

if (hashItem.ContainsKey(sTitle)) hashItem.Remove(sTitle);
hashItem.Add(sTitle, sReturn);
}
return sReturn;
} // Pick method

public static string Confirm(string sTitle, string sText, string sDefault) {
MessageBoxDefaultButton defaultButton;
if (sDefault.ToLower() == "n") defaultButton = MessageBoxDefaultButton.Button2;
else defaultButton = MessageBoxDefaultButton.Button1;

switch (MessageBox.Show(sText, sTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, defaultButton)) {
case DialogResult.Yes :
//Util.Say("Yes");
return "Y";
case DialogResult.No :
//Util.Say("No");
return "N";
}
/*Util.Say("Cancel");*/
return "";
} // Confirm method

public static void Show(object oText) {
Show("Show", oText);
} // Show method

public static void Show(object oTitle, object oText) {
string sTitle = oTitle.ToString();
string sText = oText.ToString();
if (oTitle is bool) sTitle = ((bool) oTitle) ? "true" : "false";
if (oText is bool) sText = (bool) oText ? "true" : "false";
MessageBox.Show(oText.ToString(), oTitle.ToString());
} // Show method

public static void Properties(string sPath) {
COM.InvokeVerb(sPath, "P&roperties");
COM.InvokeVerb(sPath, "Properties");
// Win32.ShellExecute("Properties", sPath);
} // Properties method

public static string Choose (string sTitle, string sText, string[] aButtons, int iDefault) {
string sResult = "";

Form frm = new Form();
frm.SuspendLayout();
frm.AutoSize = true;
frm.AutoSizeMode = AutoSizeMode.GrowAndShrink;

FlowLayoutPanel flpMain = new FlowLayoutPanel();
flpMain.SuspendLayout();
flpMain.AutoSize = true;
flpMain.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpMain.FlowDirection = FlowDirection.TopDown;

if (sText !="") {
//Util.Say(sText);
Label lbl = new Label();
lbl.AutoSize = true;
int iLines = sText.Split('\n').Length;
lbl.AutoSize = false;
lbl.Width = 200;
lbl.Height = 16 * iLines + 16;
lbl.Margin = new Padding(3, 3, 3, 3);
lbl.Text = sText;
lbl.AccessibleName = lbl.Text.Replace("&", "");
lbl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
flpMain.Controls.Add(lbl);
}

for (int i = 0; i < aButtons.Length; i++) {
Button btn = new Button();
btn.Click += delegate(object o, EventArgs e) {
sResult = btn.Text;
frm.Close();
};
btn.Text = aButtons[i];
btn.AccessibleName = aButtons[i].Replace("&", "");
btn.AutoSize = false;
btn.Width = 200;
btn.Anchor = AnchorStyles.None;
flpMain.Controls.Add(btn);
}

Button btnCancel = new Button();
btnCancel.Click += delegate(object o, EventArgs e) {
/*Util.Say("Cancel");*/ frm.Close();
};
btnCancel.Text = "Cancel";
btnCancel.AccessibleName = btnCancel.Text;
btnCancel.AutoSize = false;
btnCancel.Width = 200;
//btnCancel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
flpMain.Controls.Add(btnCancel);

flpMain.ResumeLayout();

frm.CancelButton = btnCancel;
frm.StartPosition = FormStartPosition.CenterParent;
//if (sTitle.Length > 0 && sTitle.Length == sTitle.TrimEnd().Length) sTitle += " (" + aButtons.Length + ")";
frm.Text = sTitle;
frm.Controls.Add(flpMain);

int iButton = 0;
foreach (Control ctl in flpMain.Controls) {
if (ctl.GetType() == typeof(Button)) {
if (iButton == iDefault) ctl.Select();
iButton++;
}
}

frm.ResumeLayout();
//frm.Shown += delegate(object sender, EventArgs e) { Lbc.JFWSayString(sText); };
frm.Shown += delegate(object sender, EventArgs e) {
Win32.SetForegroundWindow(frm.Handle);
};
frm.ShowDialog();
frm.Dispose();
Util.Say(sResult.Replace("&", ""));
return sResult;
} // Choose method

public static object[] PickAndChoose(string sTitle, object[] aValue, string[] aDisplay, string[] aButton, bool bSort, int iIndex) {
object[] aResult = {};

Form frm = new Form();
frm.SuspendLayout();
frm.AutoSize = true;
frm.AutoSizeMode = AutoSizeMode.GrowAndShrink;

FlowLayoutPanel flpMain = new FlowLayoutPanel();
flpMain.SuspendLayout();
flpMain.AutoSize = true;
flpMain.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpMain.FlowDirection = FlowDirection.TopDown;

FlowLayoutPanel flpData = new FlowLayoutPanel();
flpData.SuspendLayout();
flpData.Anchor = AnchorStyles.None;
flpData.AutoSize = true;
flpData.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpData.FlowDirection = FlowDirection.LeftToRight;

ListBox lst = new ListBox();
lst.Sorted = false;
if (aDisplay == null) lst.Items.AddRange(aValue);
else {
for (int i = 0; i < aDisplay.Length; i++) {
lst.Items.Add(aDisplay[i]);
Support.SetItemData(lst, i, i);
}
}
if (bSort) lst.Sorted = true;
lst.SelectedIndex = iIndex;

flpData.Controls.Add(lst);
flpData.ResumeLayout();

FlowLayoutPanel flpButtons = new FlowLayoutPanel();
flpButtons.SuspendLayout();
flpButtons.Anchor = AnchorStyles.None;
flpButtons.AutoSize = true;
flpButtons.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpButtons.FlowDirection = FlowDirection.LeftToRight;

for (int i = 0; i < aButton.Length; i++) {
Button btn = new Button();
btn.Click += delegate(object o, EventArgs e) {
object oItem;
if (aDisplay == null) oItem = lst.Text;
else {
int iItem = lst.SelectedIndex;
int iValue = Support.GetItemData(lst, iItem);
oItem = aValue[iValue];
}
aResult = new object[] {oItem, btn.Text};
Util.Say(btn.Text.Replace("&", ""));
frm.Close();
};

btn.Text = aButton[i];
btn.AccessibleName = aButton[i].Replace("&", "");
//btn.AutoSize = false;
//btn.Width = 200;
//btn.Anchor = AnchorStyles.None;
flpButtons.Controls.Add(btn);
}

Button btnCancel = new Button();
btnCancel.Click += delegate(object o, EventArgs e) {
/*Util.Say("Cancel");*/ frm.Close();
};
btnCancel.Text = "Cancel";
btnCancel.AccessibleName = btnCancel.Text;
//btnCancel.AutoSize = false;
//btnCancel.Width = 200;
//btnCancel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
flpButtons.Controls.Add(btnCancel);

flpButtons.ResumeLayout();

flpMain.Controls.AddRange(new Control[] {flpData, flpButtons});
flpMain.ResumeLayout();

frm.AcceptButton = (Button) flpButtons.Controls[0];
frm.CancelButton = btnCancel;
frm.StartPosition = FormStartPosition.CenterParent;
if (sTitle.Length > 0 && sTitle.Length == sTitle.TrimEnd().Length) sTitle += " (" + aValue.Length + ")";
frm.Text = sTitle;
frm.Controls.Add(flpMain);
frm.ResumeLayout();
frm.Shown += delegate(object sender, EventArgs e) {
Win32.SetForegroundWindow(frm.Handle);
};
frm.ShowDialog();
frm.Dispose();
return aResult;
} // PickAndChoose method

public static object[] GetFont(Font font, Color color) {
//ColorDialog d = new ColorDialog();
//d.ShowDialog();
FontDialog dlg = new FontDialog();
dlg.FontMustExist = true;
dlg.ShowColor = true;
dlg.Font = font;
dlg.Color = color;
object[] aReturn = {};
if(dlg.ShowDialog() == DialogResult.OK) aReturn = new object[] {dlg.Font, dlg.Color};
dlg.Dispose();
return aReturn;
} // GetFont method

public static string OpenFolder(string sTitle, string sLabel, string sValue) {
string sResult = "";

Form frm = new Form();
frm.SuspendLayout();
frm.AutoSize = true;
frm.AutoSizeMode = AutoSizeMode.GrowAndShrink;

FlowLayoutPanel flpMain = new FlowLayoutPanel();
flpMain.SuspendLayout();
flpMain.AutoSize = true;
flpMain.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpMain.FlowDirection = FlowDirection.TopDown;

FlowLayoutPanel flpInput = new FlowLayoutPanel();
flpInput.SuspendLayout();
flpInput.Anchor = AnchorStyles.None;
flpInput.AutoSize = true;
flpInput.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpInput.FlowDirection = FlowDirection.LeftToRight;

Label lbl = new Label();
lbl.AutoSize = true;
lbl.Text = sLabel + ":";
TextBox txt = new TextBox();
//txt.ScrollBars = ScrollBars.None;
txt.Width *= 2;
txt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
txt.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
txt.Text = sValue;
txt.AccessibleName = lbl.Text.Replace("&", "");
txt.GotFocus += delegate(object o, EventArgs e) {
txt.SelectAll();
};

Button btnBrowse = new Button();
btnBrowse.Click += delegate(object o, EventArgs e) {
txt.Text = Dialog.BrowseForFolder("", sValue, false);
txt.Select();
};
btnBrowse.Text = "&Browse";
btnBrowse.AccessibleName = btnBrowse.Text.Replace("&", "");

flpInput.Controls.AddRange(new Control[] {lbl, txt, btnBrowse});
flpInput.ResumeLayout();

FlowLayoutPanel flpButtons = new FlowLayoutPanel();
flpButtons.SuspendLayout();
flpButtons.Anchor = AnchorStyles.None;
flpButtons.AutoSize = true;
flpButtons.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpButtons.FlowDirection = FlowDirection.LeftToRight;

Button btnOK = new Button();
btnOK.Click += delegate(object o, EventArgs e) {
sResult = txt.Text.Trim();
if (sResult != "" && !Directory.Exists(sResult)) {
string sChoice = Dialog.Confirm("Confirm", "Cannot find folder\n" + sResult + "\nCreate it?", "Y");
if (sChoice == "Y") {
try {
DirectoryInfo di = new DirectoryInfo(sResult);
di.Create();
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
}
}
}
if (Directory.Exists(sResult)) frm.Close();
else {
txt.SelectAll();
txt.Select();
}
};

btnOK.Text = "OK";
btnOK.AccessibleName = btnOK.Text;

Button btnCancel = new Button();
btnCancel.Click += delegate(object o, EventArgs e) {
/*Util.Say("Cancel");*/ sResult = "";
frm.Close();
};
btnCancel.Text = "Cancel";
btnCancel.AccessibleName = btnCancel.Text;

flpButtons.Controls.AddRange(new Control[] {btnOK, btnCancel});
flpButtons.ResumeLayout();

flpMain.Controls.AddRange(new Control[] {flpInput, flpButtons});
flpMain.ResumeLayout();

frm.AcceptButton = btnOK;
frm.CancelButton = btnCancel;
frm.StartPosition = FormStartPosition.CenterParent;
frm.Text = sTitle;
frm.Controls.Add(flpMain);
frm.ResumeLayout();
frm.Shown += delegate(object sender, EventArgs e) {
Win32.SetForegroundWindow(frm.Handle);
};
frm.ShowDialog();
frm.Dispose();
return sResult;
} // GetDirectory method

public static string BrowseForFolder(string sTitle, string sDir) {
bool bNewFolder = false;
return BrowseForFolder(sTitle, sDir, bNewFolder);
} // BrowseForFolder method

public static string BrowseForFolder(string sTitle, string sDir, bool bNewFolder) {
string sReturn = "";
FolderBrowserDialog dlg = new FolderBrowserDialog();
dlg.Description = sTitle;
dlg.ShowNewFolderButton = bNewFolder;
//dlg.RootFolder = sRootFolder;
dlg.SelectedPath = sDir;

if (dlg.ShowDialog() == DialogResult.OK) sReturn = dlg.SelectedPath;
dlg.Dispose();
return sReturn;
} // BrowseForFolder method

public static string[] PickAndInputDialog(string sTitle, string sLblList, string[] aValues, string sLblInput, string sValue, bool bSort, int iIndex) {
string[] aResults = {};

Form frm = new Form();
frm.SuspendLayout();
frm.AutoSize = true;
frm.AutoSizeMode = AutoSizeMode.GrowAndShrink;

FlowLayoutPanel flpMain = new FlowLayoutPanel();
flpMain.SuspendLayout();
flpMain.AutoSize = true;
flpMain.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpMain.FlowDirection = FlowDirection.TopDown;

FlowLayoutPanel flpInput = new FlowLayoutPanel();
flpInput.SuspendLayout();
flpInput.Anchor = AnchorStyles.None;
flpInput.AutoSize = true;
flpInput.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpInput.FlowDirection = FlowDirection.LeftToRight;

Label lblList = new Label();
lblList.Text = sLblList + ":";
lblList.AccessibleName = lblList.Text.Replace("&", "");

ListBox lst = new ListBox();
if (bSort) lst.Sorted = true;
lst.Items.AddRange(aValues);
lst.SelectedIndex = iIndex;

Label lblInput = new Label();
lblInput.Text = sLblInput + ":";
lblInput.AccessibleName = lblInput.Text.Replace("&", "");
TextBox txt = new TextBox();
txt.Width *= 2;
txt.AccessibleName = lblInput.AccessibleName;
if (lblInput.Text.Contains("Password:")) txt.UseSystemPasswordChar = true;
txt.Text = sValue;

flpInput.Controls.AddRange(new Control[] {lblList, lst, lblInput, txt});
flpInput.ResumeLayout();

FlowLayoutPanel flpButtons = new FlowLayoutPanel();
flpButtons.SuspendLayout();
flpButtons.Anchor = AnchorStyles.None;
flpButtons.AutoSize = true;
flpButtons.AutoSizeMode  = AutoSizeMode.GrowAndShrink;
flpButtons.FlowDirection = FlowDirection.LeftToRight;

Button btnOK = new Button();
btnOK.Click += delegate(object o, EventArgs e) {
aResults = new string[] {
lst.Text, txt.Text
};
frm.Close();
};

btnOK.Text = "OK";
btnOK.AccessibleName = btnOK.Text;

Button btnCancel = new Button();
btnCancel.Click += delegate(object o, EventArgs e) {
/*Util.Say("Cancel");*/ frm.Close();
};
btnCancel.Text = "Cancel";
btnCancel.AccessibleName = btnCancel.Text;

flpButtons.Controls.AddRange(new Control[] {btnOK, btnCancel});
flpButtons.ResumeLayout();

flpMain.Controls.AddRange(new Control[] {flpInput, flpButtons});
flpMain.ResumeLayout();

frm.AcceptButton = btnOK;
frm.CancelButton = btnCancel;
frm.StartPosition = FormStartPosition.CenterParent;
frm.Text = sTitle;
frm.Controls.Add(flpMain);
frm.ResumeLayout();
frm.Shown += delegate(object sender, EventArgs e) {
Win32.SetForegroundWindow(frm.Handle);
};
frm.ShowDialog();
frm.Dispose();
return aResults;
} // PickAndInput method

} // Dialog class

public class COM {
public static object CreateObject(string sProgID) {
Type t = Type.GetTypeFromProgID(sProgID);
object oResult = Activator.CreateInstance(t);
return oResult;
} // CreateObject method

public static object GetObject(string sProgID) {
object oResult = Interaction.GetObject(null, sProgID);
return oResult;
} // GetObject method

public static object GetOrCreateObject(string sProgID, out bool bCreate, string sMessage) {
object oResult;
try {
oResult = GetObject(sProgID);
bCreate = false;
}
catch {
Util.Say(sMessage);
oResult = CreateObject(sProgID);
bCreate = true;
}
return oResult;
} // GetOrCreateObject method

public static object CallMethod(object o, string sMethod) {
object[] args = {};
return CallMethod(o, sMethod, args);
} // CallMethod method

public static object CallMethod(object o, string sMethod, string sValue) {
object[] args = {sValue};
return CallMethod(o, sMethod, args);
} // CallMethod method

public static object CallMethod(object o, string sMethod, int iValue) {
object[] args = {iValue};
return CallMethod(o, sMethod, args);
} // CallMethod method

public static object CallMethod(object o, string sMethod, object[] args) {
Type t = o.GetType();
object oResult = t.InvokeMember(sMethod, BindingFlags.InvokeMethod, null, o, args);
return oResult;
} // CallMethod method

public static object SetProperty(object o, string sProperty, string sValue) {
object[] args = {sValue};
return SetProperty(o, sProperty, args);
} // SetProperty method

public static object SetProperty(object o, string sProperty, int iValue) {
object[] args = {iValue};
return SetProperty(o, sProperty, args);
} // SetProperty method

public static object SetProperty(object o, string sProperty, bool bValue) {
object[] args = {bValue};
return SetProperty(o, sProperty, args);
} // SetProperty method

public static object SetProperty(object o, string sProperty, object[] args) {
Type t = o.GetType();
object oResult = t.InvokeMember(sProperty, BindingFlags.SetProperty, null, o, args);
return oResult;
} // SetProperty method

public static object GetProperty(object o, string sProperty) {
object[] args = new object[] {};
return GetProperty(o, sProperty, args);
} // GetProperty method

public static object GetProperty(object o, string sProperty, object[] args) {
Type t = o.GetType();
object oResult = t.InvokeMember(sProperty, BindingFlags.GetProperty, null, o, args);
return oResult;
} // GetProperty method

public static bool JFWSay(string sText) {
// object oJFW = null;
// return JFWSay(sText, ref oJFW);
return JFWSay(sText, ref App.JAWS);
} // JFWSay method

public static bool JFWSay(string sText, ref object oJFW) {
try {
if (oJFW == null) oJFW = CreateObject("FreedomSci.JawsApi");
// int iResult = (int) CallMethod(oJFW, "SayString", new object[] {sText, 0});
// return iResult == 1;
// Console.Beep();
bool bResult = (bool) CallMethod(oJFW, "SayString", new object[] {sText, false});
return bResult;
}
catch {
return false;
}
} // JFWSay method

public static bool JFWRunFunction(string sText) {
return JFWRunFunction(sText, ref App.JAWS);
} // JFWRunFunction method

public static bool JFWRunFunction(string sText, ref object oJFW) {
try {
if (oJFW == null) oJFW = CreateObject("FreedomSci.JawsApi");
bool bResult = (bool) CallMethod(oJFW, "RunFunction", new object[] {sText});
return bResult;
}
catch {
return false;
}
} // JFWRunFunction method

public static bool WESay(string sText) {
//object oWE = null;
//return WESay(sText, ref oWE);
return WESay(sText, ref App.Wineyes);
} // WESay method

public static bool WESay(string sText, ref object oWE) {
//if ((int) Win32.FindWindow(0, "Window-Eyes") == 0) return false;
//don't even check since last resort
if (!Win32.IsWinEyesActive()) return false;

try {
if (oWE == null) oWE = CreateObject("GwSpeak.Speak");
CallMethod(oWE, "SpeakString", sText);
// if (oWE == null) oWE = CreateObject("WindowEyes.Application");
// object oSpeech = COM.GetProperty(oWE, "Speech");
// CallMethod(oSpeech, "Speak", sText);
return true;
}
catch {
return false;
}
} // WESay method

public static bool SAPISay(string sText) {
object oSAPI = null;
return SAPISay(sText, oSAPI);
} // SAPISay method

public static bool SAPISay(string sText, object oSAPI) {
try {
if (oSAPI == null) oSAPI = CreateObject("SAPI.SPVoice");
CallMethod(oSAPI, "Speak", sText);
return true;
}
catch {
return false;
}
} // SAPISay method

public static void InvokeVerb(string sPath, string sVerb) {
// Dialog.Show(sPath, sVerb);
object o = COM.CreateObject("Shell.Application");
string sDir = Path.GetDirectoryName(sPath);
string sName = Path.GetFileName(sPath);
o = COM.CallMethod(o, "Namespace", new string[] {sDir});
// o = COM.GetProperty(o, "Self");
o = COM.CallMethod(o, "ParseName", new string[] {sName});
o = COM.CallMethod(o, "InvokeVerb", new string[] {sVerb});
} // InvokeVerb method

public static string[] Verbs(string sPath) {
object o = COM.CreateObject("Shell.Application");
string sDir = Path.GetDirectoryName(sPath);
string sName = Path.GetFileName(sPath);
o = COM.CallMethod(o, "Namespace", new string[] {sDir});
o = COM.CallMethod(o, "ParseName", new string[] {sName});
try {
o = COM.CallMethod(o, "Verbs", new object[] {});
}
catch {
return new string[] {};
}
int iCount = (int) COM.GetProperty(o, "Count");
StringBuilder sb = new StringBuilder();
for (int i = 0; i < iCount; i++) {
object oVerb = COM.CallMethod(o, "Item", new object[] {(int) i});
string sVerb = (string) COM.GetProperty(oVerb, "Name");
if (sVerb.Trim() != "") sb.Append(sVerb + "\n");
}
string[] aVerbs = sb.ToString().Trim().Split('\n');
return aVerbs;
} // Verbs method

public static string ConvertFile2String(string sSource) {
int iConvert = 2;
return ConvertFile2String(sSource, ref iConvert);
} // ConvertFile2String method

public static string ConvertFile2String(string sSource, ref int iConvert) {
string sTargetExt = "txt";
return ConvertFile2String(sSource, ref iConvert, ref sTargetExt);
} // ConvertFile2String method

public static string ConvertFile2String(string sSource, ref int iConvert, ref string sTargetExt) {
bool bTextOnly = false;
return ConvertFile2String(sSource, ref iConvert, ref sTargetExt, bTextOnly);
} // Convert File2String method

public static string ConvertFile2String(string sSource, ref int iConvert, ref string sTargetExt, bool bTextOnly) {
string sText = "";
if (iConvert == 0) sText = Util.File2String(sSource);
else {
//string sTarget = App.TempFile;
string sTarget = Path.GetTempFileName();
App.TempFiles.Add(sTarget);
//sTarget = Win32.GetShortPath(sTarget);
string sResult;
string[] aResults = App.ReadSectionKeys("Import");
HomerList hl = new HomerList(aResults);
hl.AddUniqueRange("rtf|htm|html|xhtml");
hl.ToLower();
string sExt = Path.GetExtension(sSource).ToLower().TrimStart('.');
string sMatch = "^" + sExt + @"(2\w+)?$";
if (bTextOnly) sMatch = "^" + sExt + @"2txt$";
hl.KeepLike(sMatch);
//if (hl.Count > 0) hl.Push(sExt + "2" + sExt);
// do not offer original format, since already available with Control+O
if (hl.Count > 1) hl.Push(sExt + "2" + sExt);
aResults = hl.ToArray();
hl.ReplaceLike("^" + sExt + "$", sExt + "2txt");
hl.ReplaceLike(@"^\w+2", "");
string[] aDisplay = hl.ToArray();
Array.Sort(aDisplay, aResults);
// Solved above instead
// Solve TextConvert with brf
// if (bTextOnly) aResults = new string[] { sExt + "2txt"};
// if (bTextOnly) aResults = new string[] { sExt, sExt + "2txt"};
if (aResults.Length == 0) {
//Dialog.Show("Alert", "No import options for " + sExt);
//return "";
sResult = "";
}
else if (aResults.Length == 1) sResult = aResults[0];
else {
//sResult = Dialog.Pick("Import Format", aResults, true, 0);
string sTitle = "Import " + sExt + " to ";
sResult = Dialog.Pick(sTitle, aResults, aDisplay, true, 0);
//Dialog.Show(sResult);
if (sResult.Length == 0) return "";

}
string sTempExt = Util.RegExpReplaceCase(sResult, @"^\w+2", "");
//Dialog.Show(s, sResult);
if (sTempExt != sResult) sTargetExt = sTempExt;
else sResult = sExt;
string sCommand = Ini.ReadValue(App.IniFile, "Import", sResult, "");
if (sCommand.Length > 0) {
// Dialog.Show(sTargetExt, "target extension");
string s = Path.ChangeExtension(sTarget, sTargetExt);
if (!Util.Equiv(sTarget, s)) {
if (File.Exists(s)) File.Delete(s);
System.IO.File.Move(sTarget, s);
sTarget = s;
}

sCommand = Util.ExpandCommandLine(sCommand, sSource, sTarget);
// Dialog.Show(sTarget, "target file");
// Dialog.Show(sCommand);
//Clipboard.SetText(sCommand);
//Clipboard.SetText(sTarget);
App.Frame.AddMessage("Converting");
if (File.Exists(sTarget)) sTarget = Win32.GetShortPath(sTarget);
if (File.Exists(sTarget)) File.Delete(sTarget);
Util.RunHideWait(sCommand);
if (!File.Exists(sTarget)) {
//Util.RunHide(sCommand);
sCommand = "cmd.exe /c " + sCommand;
//Util.RunHide(sCommand);
Util.RunHideWait(sCommand);
/*
int iLoop = 20;
while (iLoop > 0 && !File.Exists(sTarget)) {
System.Threading.Thread.Sleep(100);
iLoop--;
}
*/
}
// Ensure UTF8B
// if (File.Exists(sTarget)) sText = Util.File2String(sTarget);
if (File.Exists(sTarget)) {
// string sDir = Path.Combine(App.ProgramDir, "WebClient");
string sDir = Path.Combine(App.ProgramDir, @"Convert\EasyEncode");
// string sExe = Path.Combine(sDir, "Encoding.exe");
string sExe = Path.Combine(sDir, "utf8b.exe");
// string sParams = "convert " + sTarget + " utf8b";
string sParams = Util.Quote(sTarget);
sText = Util.GetProgramOutput(sExe, sParams);
sText = Util.File2String(sTarget);
}

if (sText.Length == 0) Dialog.Show("Error", "Command line:\n" + sCommand);
}
else {
if (sTargetExt == sExt) {
sExt = "";
iConvert = 1;
}

switch (sExt) {
case "rtf" :
if (iConvert > 0) iConvert = -1;
break;
//Use OfficeConvert utilities
/*
case "doc" :
case "docx" :
App.Frame.AddMessage("Converting");
sText = WordFile2String(sSource);
break;
case "ppt" :
case "pptx" :
App.Frame.AddMessage("Converting");
VB.Ppt2Txt(sSource, sTarget);
sText = Util.File2String(sTarget);
break;
case "xls" :
case "xlsx" :
App.Frame.AddMessage("Converting");
VB.Xls2Txt(sSource, sTarget);
sText = Util.File2String(sTarget);
break;
*/
default :
// Disable Word conversions of unknown extensions
// if (iConvert == 1) {
if (iConvert != -1) {
sText = Util.File2String(sSource);
iConvert = 0;
}
else {
App.Frame.AddMessage("Converting");
sText = WordFile2String(sSource);
}
break;
}
}
}
App.Frame.Activate();
sText = Util.Convert2UnixLineBreak(sText);
return sText;
} // ConvertFile2String method

public static string WordFile2String(string sSource) {
bool bCreate, bVisible;
int iDisplayAlerts;
bool bAppVisible = false;
//object oApp = COM.GetOrCreateObject("Word.Application", out bCreate);
object oApp = COM.WordAccess(out bCreate);
bVisible = (bool) COM.GetProperty(oApp, "Visible");
iDisplayAlerts = (int) COM.GetProperty(oApp, "DisplayAlerts");
COM.SetProperty(oApp, "Visible", bAppVisible);
COM.SetProperty(oApp, "DisplayAlerts", 0);
object oDocs =COM.GetProperty(oApp, "Documents");
object oDoc = VB.WordOpen(oDocs, sSource, bAppVisible);
string sTarget = Path.GetTempFileName();
if (File.Exists(sTarget)) File.Delete(sTarget);
object oSelection = COM.GetProperty(oApp, "Selection");
int iLength = (int) COM.GetProperty(oSelection, "StoryLength");
COM.CallMethod(oSelection, "SetRange", new object[] {0, iLength});
string sText = (string) COM.GetProperty(oSelection, "Text");
COM.Release(ref oSelection);
sText = sText.Trim();
sText = Util.RegExpReplaceCase(sText, "\r\f", "\f\r");
sText = Util.Convert2UnixLineBreak(sText);
sText = Util.RegExpReplaceCase(sText, MdiFrame.SB, MdiFrame.SectionBreak);
//sText = Util.Convert2WinLineBreak(sText);
Util.String2File(sText, sTarget);
//VB.WordSaveAs(oDoc, sTarget, 2);
VB.WordClose(oDoc);
COM.Release(ref oDoc);
COM.Release(ref oDocs);

if (bCreate) {
//VB.WordQuit(oApp);
}
else {
COM.SetProperty(oApp, "Visible", bVisible);
COM.SetProperty(oApp, "DisplayAlerts", iDisplayAlerts);
}

COM.Release(ref oApp);
if (File.Exists(sTarget)) {
string sReturn = Util.File2String(sTarget);
File.Delete(sTarget);
return sReturn;
}
else return "";
} // WordFile2String();

public static object WordOpen(object oDocs, string sFile, bool bAppVisible) {
bool bConfirmConversions = false;
bool bReadOnly = false;
bool bAddToRecentFiles = false;
object sPasswordDocument = Missing.Value;
object sPasswordTemplate = Missing.Value;
bool bRevert = true;
object sWritePasswordDocument = Missing.Value;
object sWritePasswordTemplate = Missing.Value;
object iFormat = Missing.Value;
object iEncoding = Missing.Value;
bool bVisible = bAppVisible;
object oOpenConflictDocument = Missing.Value;
bool bOpenAndRepair = true;
object iDocumentDirection = Missing.Value;
bool bNoEncodingDialog = true;

object[] oParams = {sFile, bConfirmConversions, bReadOnly, bAddToRecentFiles, sPasswordDocument, sPasswordTemplate, bRevert, sWritePasswordDocument, sWritePasswordTemplate, iFormat, iEncoding, bVisible, oOpenConflictDocument, bOpenAndRepair, iDocumentDirection, bNoEncodingDialog};
oParams = new object[] {sFile, bConfirmConversions, bReadOnly, bAddToRecentFiles};

object oDoc = null;
try {
oDoc = CallMethod(oDocs, "Open", oParams);
}
catch (COMException ex) {
Dialog.Show("Error", ex.Message);
}
return oDoc;
} // OpenWordDocument method

public static void WordSaveAs(object oDoc, string sFile, int iSaveFormat) {
int iFileFormat = iSaveFormat;
bool bLockComments = false;
object oPassword = Type.Missing;
bool bAddToRecentFiles = false;
object oWritePassword = Type.Missing;
bool bReadOnlyRecommended = false;
bool bEmbedTrueTypeFonts = false;
bool bSaveNativePictureFormat = false;
bool bSaveFormsData = false;
bool bSaveAsAOCELetter = false;
object oEncoding= Type.Missing;
bool bInsertLineBreaks = false;
bool bAllowSubstitutions = false;
object sLineEnding = Type.Missing;
bool bAddBiDiMarks = false;
object[] oParams = {sFile, iFileFormat, bLockComments, oPassword, bAddToRecentFiles, oWritePassword, bReadOnlyRecommended, bEmbedTrueTypeFonts, bSaveNativePictureFormat,
bSaveFormsData, bSaveAsAOCELetter, oEncoding, bInsertLineBreaks, bAllowSubstitutions, sLineEnding, bAddBiDiMarks
};
try {
CallMethod(oDoc, "SaveAs", oParams);
}
catch (COMException ex) {
Dialog.Show("Error", ex.Message);
}
} // WordSaveAs method

public static void WordClose(object oDoc) {
object oApp = GetProperty(oDoc, "Application");
ClearNormalTemplate(oApp);

int iSaveChanges = 0;
object iOriginalFormat = Type.Missing;
bool bRouteDocument = false;
object[] oParams = {iSaveChanges, iOriginalFormat, bRouteDocument};
oParams = new object[] {iSaveChanges};

try {
CallMethod(oDoc, "Close", oParams);
}
catch (COMException ex) {
Dialog.Show("Error", ex.Message);
}
} // WordClose method

public static void ClearNormalTemplate(object oApp) {
object oTemplate = COM.GetProperty(oApp, "NormalTemplate");
COM.SetProperty(oTemplate, "Saved", true);
Release(ref oTemplate);
} // ClearNormalTemplate method

public static void WordQuit(object oApp) {
COM.ClearNormalTemplate(oApp);

int iSaveChanges = 0;
object iFormat = Type.Missing;
bool bRouteDocument = false;

object[] oParams = {iSaveChanges, iFormat, bRouteDocument};
oParams = new object[] {iSaveChanges};

try {
CallMethod(oApp, "Quit", oParams);
}
catch (COMException ex) {
Dialog.Show("Error", ex.Message);
}
} // WordQuit method

public static void Release(ref object o) {
Marshal.ReleaseComObject(o);
o = null;
} // Release method

public static object WordAccess(out bool bCreate) {
string sMessage = "Initializing Microsoft Word";
object oApp = GetOrCreateObject("Word.Application", out bCreate, sMessage);
if (bCreate) App.WordCreated = true;
return oApp;
} // WordAccess method

public static void WordExit() {
object oApp = null;
bool bLoop = true;
while (bLoop) {
try {
oApp = GetObject("Word.Application");
//Util.Say("quit");
//WordQuit(oApp);
//break;
VB.WordQuit(oApp);
Release(ref oApp);
}
catch {
break;
}
}
Util.TerminateProcess("WinWord");
} // WordExit method;

public static bool WordSource2TargetFormat(string sSource, string sTarget, string sFormat) {
int iFormat = 2; // text;
if (sFormat == "doc") iFormat = 0;
else if (sFormat == "htm") iFormat = 10;
else if (sFormat == "xml") iFormat = 11;
bool bAppVisible = false;
bool bCreate;
object oApp = COM.WordAccess(out bCreate);
bool bVisible = (bool) COM.GetProperty(oApp, "Visible");
int iDisplayAlerts = (int) COM.GetProperty(oApp, "DisplayAlerts");
COM.SetProperty(oApp, "Visible", bAppVisible);
COM.SetProperty(oApp, "DisplayAlerts", 0);
object oDocs = COM.GetProperty(oApp, "Documents");
object oDoc = VB.WordOpen(oDocs, sSource, bAppVisible);
object oSelection = COM.GetProperty(oApp, "Selection");
//object oRange = COM.GetProperty(oSelection, "Range");
int iLength = (int) COM.GetProperty(oSelection, "StoryLength");
object oRange = COM.CallMethod(oDoc, "Range", new object[] {0, iLength});
COM.CallMethod(oRange, "AutoFormat");
VB.WordSaveAs(oDoc, sTarget, iFormat);
COM.Release(ref oRange);
VB.WordClose(oDoc);
COM.Release(ref oDoc);
COM.Release(ref oDocs);
if (!bCreate) {
COM.SetProperty(oApp, "Visible", bVisible);
COM.SetProperty(oApp, "DisplayAlerts", iDisplayAlerts);
}
COM.Release(ref oApp);
App.Frame.Activate();
return File.Exists(sTarget);
} // WordSource2TargetFormat method

public static string GetUrl() {
string sUrl = "";
try {
object oShell = COM.CreateObject("Shell.Application");
object oWindows = COM.CallMethod(oShell, "Windows");
int iCount = (int) COM.GetProperty(oWindows, "Count");
if (iCount > 0) {
object oWindow = COM.CallMethod(oWindows, "Item", new object[] {iCount - 1});
sUrl = (string) COM.GetProperty(oWindow, "LocationURL");
}
}
catch {}
return sUrl;
} // GetUrl method

public static void ActivateTitle(string sTitle) {
object oShell = CreateObject("WScript.Shell");
CallMethod(oShell, "AppActivate", sTitle);
} // ActivateTitle method

} // COM class

public class Ini {
public static string RedirectFile(string sFile, string sSection) {
if(Util.Equiv(sFile, App.IniFile) && (Util.Equiv(sSection, "Favorites") || Util.Equiv(sSection, "Recent") || Util.Equiv(sFile, "Tokens"))) sFile = Path.Combine(App.DataDir, App.ReadData("Compiler", "Default") + ".ini");
return sFile;
} // RedirectFile method

[DllImport("kernel32.dll")]
public static extern int GetPrivateProfileString(string sSection, string sKey, string sDefault, StringBuilder sReturnString, int iLength, string sFile);
public static String ReadValue(String sFile, String sSection, String sKey, string sDefault) {
sFile = RedirectFile(sFile, sSection);
StringBuilder sb = new StringBuilder(260);
if (GetPrivateProfileString(sSection, sKey, sDefault, sb, sb.Capacity, sFile) > 0) return sb.ToString();
else return sDefault;
} // ReadValue method

[DllImport("kernel32.dll")]
public static extern bool WritePrivateProfileString(string sSection, string sKey, string sValue, string sFile);
public static bool WriteQuote(String sFile, String sSection, String sKey, String sValue) {
bool bQuote = true;
return WriteValue(sFile, sSection, sKey, sValue, bQuote);
} // WriteQuote method

public static bool WriteValue(String sFile, String sSection, String sKey, String sValue) {
bool bQuote = false;
return WriteValue(sFile, sSection, sKey, sValue, bQuote);
} // WriteValue method

public static bool WriteValue(String sFile, String sSection, String sKey, String sValue, bool bQuote) {
sFile = RedirectFile(sFile, sSection);
if (bQuote) sValue = "\"" + sValue + "\"";
bool bReturn = WritePrivateProfileString(sSection, sKey, sValue, sFile);
FlushFile(sFile);
return bReturn;
} // WriteValue method

[DllImport("kernel32.dll")]
public static extern bool WritePrivateProfileString(string sSection, string sKey, int iValue, string sFile);
public static bool DeleteKey(String sFile, String sSection, String sKey) {
sFile = RedirectFile(sFile, sSection);
int iValue = 0;
bool bReturn = WritePrivateProfileString(sSection, sKey, iValue, sFile);
FlushFile(sFile);
return bReturn;
} // DeleteKey method

[DllImport("kernel32.dll")]
public static extern bool WritePrivateProfileString(string sSection, int iKey, int iValue, string sFile);
public static bool DeleteSection(String sFile, String sSection) {
int iKey = 0;
int iValue = 0;
bool bReturn = WritePrivateProfileString(sSection, iKey, iValue, sFile);
FlushFile(sFile);
return bReturn;
} // DeleteSection method

[DllImport("kernel32.dll")]
public static extern bool WritePrivateProfileString(int iSection, int iKey, int iValue, string sFile);
public static bool FlushFile(String sFile) {
int iSection = 0;
int iKey = 0;
int iValue = 0;
return WritePrivateProfileString(iSection, iKey, iValue, sFile);
} // FlushFile method

public static string[] ReadSectionKeys(string sFile, string sSection) {
bool bIncludeComments = false;
return ReadSectionKeys(sFile, sSection, bIncludeComments);
} // ReadSectionKeys method

public static string[] ReadSectionKeys(string sFile, string sSection, bool bIncludeComments) {
sFile = RedirectFile(sFile, sSection);
string[] aDefault = new string[] {};
if (!File.Exists(sFile)) return aDefault;

string sText = Util.File2String(sFile);
string sMatch = "^\\[" + sSection + "\\](.|\n)*?((\n\\[)|\\Z)";
object[] aResult = Util.RegExpContainsCase(sText, sMatch);
int iIndex = (int) aResult[0];
if (iIndex == -1) return aDefault;

string sValue = (string) aResult[1];
string[] aLines = sValue.Split('\n');
StringBuilder sb = new StringBuilder();
foreach (string sLine in aLines) {
string s = sLine.TrimStart();
if (s.Length == 0 || (!bIncludeComments && s.StartsWith(";")) || s.StartsWith("=") || !s.Contains("=")) continue;
int i = s.IndexOf("=");
string sKey = s.Substring(0, i).TrimEnd();
sb.Append(sKey + "\n");
}

string sKeys = sb.ToString().Trim();
if (sKeys.Length == 0) return aDefault;

string[] aReturn = sKeys.Split('\n');
return aReturn;
} // ReadSectionKeys method

public static string[] ReadSections(string sFile) {
string[] aDefault = new string[] {};
if (!File.Exists(sFile)) return aDefault;

string sText = Util.File2String(sFile);
string sMatch = "^\\[.+?\\]\r\n";
string[] aResults = Util.RegExpExtractCase(sText, sMatch);
string sSections = String.Join("", aResults).Trim();
if (sSections.Length == 0) return aDefault;

sSections = sSections.Replace("[", "").Replace("]", "").Replace("\r", "");
string[] aReturn = sSections.Split('\n');
return aReturn;
} // ReadSections method

} // Ini class

public class Win32 {
[DllImport("user32.dll")]
public static extern int AttachThreadInput(int iThread1, int iThread2, int iAttach);

[DllImport("user32.dll")]
public static extern IntPtr GetActiveWindow();

[DllImport("user32.dll")]
public static extern int BringWindowToTop(IntPtr h);

[DllImport("user32.dll")]
public static extern int ShowWindow(IntPtr h, int iState);

[DllImport("kernel32.dll")]
public static extern int GetCurrentThreadId();

[DllImport("user32.dll")]
public static extern int GetWindowThreadProcessId(IntPtr h, int iProcess);

public static bool ForceWindow(IntPtr h) {
int iForegroundThread = GetWindowThreadProcessId(GetForegroundWindow(), 0);
int iAppThread = GetCurrentThreadId();
if (iForegroundThread == iAppThread) {
BringWindowToTop(h);
ShowWindow(h,3);
}
else {
AttachThreadInput(iForegroundThread, iAppThread, 1);
BringWindowToTop(h);
ShowWindow(h,3);
AttachThreadInput(iForegroundThread, iAppThread, 0);
}

return GetActiveWindow() == h;
} // ForceWindow method

[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
public static extern int GetShortPathName(string path, StringBuilder shortPath, int shortPathLength);
public static string GetShortPath(string sLongPath) {
StringBuilder sbShortPath = new StringBuilder(260);
GetShortPathName(sLongPath, sbShortPath, sbShortPath.Capacity);
string sReturn = sbShortPath.ToString().Trim();
if (sReturn.Length == 0) sReturn = Path.Combine(GetShortPath(Path.GetDirectoryName(sLongPath)), Path.GetFileName(sLongPath));
return sReturn;
} // GetShortPath method

[DllImport("user32.dll")]
static extern bool SystemParametersInfo(int iAction, int iParam, out bool bActive, int iUpdate);
public static bool IsScreenReaderActive() {
int iAction = 70; // SPI_GETSCREENREADER constant;
int iParam = 0;
bool bActive;
int iUpdate = 0;
bool bReturn = SystemParametersInfo(iAction, iParam, out bActive, iUpdate);
return bReturn && bActive;
} // IsScreenReaderActive method

public static bool IsJAWSActive() {
string sClass = "JFWUI2";
string sTitle = "JAWS";
return (int) FindWindow(sClass, sTitle) != 0;
} // IsJAWSActive method

public static bool IsWinEyesActive() {
//string sClass = "AfxFrameOrView42";
//string sTitle = "Window-Eyes";
string sClass = "GWMExternalControl";
string sTitle = "External Control";
return (int) FindWindow(sClass, sTitle) != 0;
//int iClass = 0;
//return (int) FindWindow(iClass, sTitle) != 0;
} // IsWinEyesActive method

[DllImport("jfwapi.dll")]
public static extern int JFWRunFunction(string sText);

public static bool JAWSSay(string sText) {
//if (sText.Length < 2000) return JFWSay(sText);
if (JFWSay(sText)) return true;
//if (!JFWSay("")) return false;
if (!IsJAWSActive()) return false;

Util.String2FileA(sText, App.TempFile);
return JFWRunFunction("SayTempFile") == 1;
} // JAWSSay method

[DllImport("jfwapi.dll")]
public static extern int JFWSayString(string sText, int iInterrupt);

public static bool JFWSay(string sText) {
try {
return JFWSayString(sText, 0) == 1;
}
catch {
return false;
}
} // JFWSay method

[DllImport("nvdaControllerClient32.dll", CharSet = CharSet.Auto)]
public static extern int nvdaController_testIfRunning();

public static bool IsNVDAActive() {
return nvdaController_testIfRunning() == 0;
} // IsNVDAActive method

[DllImport("nvdaControllerClient32.dll", CharSet = CharSet.Auto)]
public static extern int nvdaController_speakText(string sText);

public static bool NVDASay(string sText) {
return nvdaController_speakText(sText) == 0;
} // NVDASay method
[DllImport("nvdaControllerClient32.dll", CharSet = CharSet.Auto)]
public static extern int nvdaController_brailleMessage(string sText);

public static bool NVDABraille(string sText) {
return nvdaController_brailleMessage(sText) == 0;
} // NVDASay method

[DllImport("saapi32.dll")]
public static extern int SA_IsRunning();

public static bool IsSAActive() {
try {
return SA_IsRunning() == 1;
}
catch {
return false;
}
} // IsSAActive method

[DllImport("saapi32.dll")]
public static extern int SA_SayU(string sText);

public static bool SASay(string sText) {
try {
return SA_SayU(sText) == 1;
}
catch {
return false;
}
} // SASay method

[DllImport("user32.dll")]
public static extern int SendMessage(IntPtr h, int iMsg, int wParam, int lParam);

[DllImport("user32.dll")]
public static extern IntPtr GetForegroundWindow();

[DllImport("user32.dll")]
public static extern int SetForegroundWindow(IntPtr h);

[DllImport("user32.dll")]
public static extern IntPtr FindWindow(string sClass, string sTitle);

[DllImport("user32.dll")]
public static extern IntPtr FindWindow(int iClass, string sTitle);

[DllImport("user32.dll")]
public static extern IntPtr FindWindow(string sClass, int iTitle);

[DllImport("shell32.dll")]
public static extern int ShellExecute(int i1, string sVerb, string sFile, int i2, int i3, int i4);

public static int ShellExecute(string sVerb, string sFile) {
return ShellExecute(0, sVerb, sFile, 0, 0, 1);
} // ShellExecute method

[DllImport("shell32.dll")]
public static extern int ShellExecute(int i1, int i2, string sFile, int i3, int i4, int i5);

public static int ShellDefault(string sFile) {
return ShellExecute(0, 0, sFile, 0, 0, 1);
} // ShellDefault method

[DllImport("MSCorEE.dll", CharSet = CharSet.Auto)]
public static extern int GetCORSystemDirectory  (StringBuilder sbPath, int iSize, out int iLength);
public static string GetNetSdkDir() {
int iSize = 260;
StringBuilder sbPath = new StringBuilder(iSize);
int iLength;
GetCORSystemDirectory  (sbPath, iSize, out iLength);
return sbPath.ToString();
} // GetNetSdkDir method

[DllImport("MSCorEE.dll", CharSet = CharSet.Auto)]
// public static extern int GetRuntimeDirectory  (StringBuilder sbPath, int iSize, out int iLength);
public static extern int GetRuntimeDirectory  (StringBuilder sbPath, out int iLength);
public static string GetNetRuntimeDir() {
int iSize = 260;
StringBuilder sbPath = new StringBuilder(iSize);
// int iLength;
int iLength = 260;
// GetRuntimeDirectory  (sbPath, iSize, out iLength);
GetRuntimeDirectory  (sbPath, out iLength);
return sbPath.ToString();
} // GetNetRuntimeDir method


public static string GetJFWDir() {
RegistryKey key = Registry.LocalMachine;
string sSubKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\";
string sName = "Path";
string sPath = GetRegString(key, (sSubKey + "jfw.exe"), sName);

if (sPath == "") {
string[] sVersions = {"12", "11", "10", "90", "81", "80", "8", "71", "70", "7", "62", "61", "60", "6"};
sName = "";
foreach (string sVersion in sVersions) {
sPath = GetRegString(key, (sSubKey + "jaws" + sVersion + ".exe"), sName);
if (sPath != "") {
sPath = Path.GetDirectoryName(sPath);
break;
}
}
}
//if (sPath !="" && !sPath.EndsWith(@"\")) sPath = String.Concat(sPath, @"\");
sPath = sPath.TrimEnd('\\');
return sPath;
} // GetJFWDir method

public static string GetWEDir() {
RegistryKey key = Registry.LocalMachine;
string sSubKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinEyes.exe";
string sName = "Path";
string sPath = GetRegString(key, sSubKey, sName);
if (sPath !="" && !sPath.EndsWith(@"\")) sPath = String.Concat(sPath, @"\");
return sPath;
} // GetWEDir method

public static string GetRegString(RegistryKey key, string sSubKey, string sName) {
RegistryKey subkey = null;
string sData = "";

try {
subkey = key.OpenSubKey(sSubKey);
sData = subkey.GetValue(sName).ToString();
}
catch {
}
finally {
if (subkey != null) subkey.Close();
}
return sData;
} // GetRegString method

[DllImport("urlmon.dll")]
public static extern int URLDownloadToFile(int i1, string sUrl, string sFile, int i2, int i3, int i4);

public static bool Url2File(string sUrl, string sFile) {
int iResult = URLDownloadToFile(0, sUrl, sFile, 0, 0, 0);
return iResult == 0;
} // Url2File method

[Serializable]
public struct ShellExecuteInfo {
public int Size;
public uint Mask;
public IntPtr hwnd;
public string Verb;
public string File;
public string Parameters;
public string Directory;
public uint Show;
public IntPtr InstApp;
public IntPtr IDList;
public string Class;
public IntPtr hkeyClass;
public uint HotKey;
public IntPtr Icon;
public IntPtr Monitor;
}

[DllImport("shell32.dll", SetLastError = true)]
extern public static bool ShellExecuteEx(ref ShellExecuteInfo lpExecInfo);

public const uint SW_NORMAL = 1;

public static void OpenWith(string file) {
ShellExecuteInfo sei = new ShellExecuteInfo();
sei.Size = Marshal.SizeOf(sei);
sei.Verb = "openas";
sei.File = file;
sei.Show = SW_NORMAL;
if (!ShellExecuteEx(ref sei))
throw new System.ComponentModel.Win32Exception();
} //OpenAs method

} // Win32 class

public class Util {

public static string GetPortableExecutableKind() {
PortableExecutableKinds peKind  ;
ImageFileMachine machine  ;

// Module module = App.Shell.GetType().Module;
Module module = Assembly.GetExecutingAssembly().ManifestModule;
module.GetPEKind(out peKind, out machine);

if ((peKind & PortableExecutableKinds.ILOnly) != 0) // Assembly is platform independent.
{}
else { // assembly is platform dependent
switch (machine) {
case ImageFileMachine.I386: // i386, x86, IA-32, ... dependent.
break;
case ImageFileMachine.IA64: // IA-64 dependent.
break;
case ImageFileMachine.AMD64: // AMD-64, x64 dependent.
break;
} // switch
} // if

Dictionary<string, string> dFlags = new Dictionary<string, string>();
dFlags.Add("NotAPortableExecutableImage", "The file is not in portable executable (PE) file format.");
dFlags.Add("ILOnly", "The executable contains only Microsoft intermediate language (MSIL), and is therefore neutral with respect to 32-bit or 64-bit platforms.");
dFlags.Add("Required32Bit", "The executable can be run on a 32-bit platform, or in the 32-bit Windows on Windows (WOW) environment on a 64-bit platform.");
dFlags.Add("PE32Plus", "The executable requires a 64-bit platform.");
dFlags.Add("Unmanaged32Bit", "The executable contains pure unmanaged code.");
dFlags.Add("I386", "Targets a 32-bit Intel processor.");
dFlags.Add("IA64", "Targets a 64-bit Intel processor.");
dFlags.Add("AMD64", "Targets a 64-bit AMD processor.");
string sReturn = "";
string sPEKind = peKind.ToString();
string[] aPEKind = sPEKind.Split(',');
string sMachine = machine.ToString();
foreach (string s in aPEKind) {
sPEKind = s.Trim();
if (dFlags.ContainsKey(sPEKind)) sReturn += dFlags[sPEKind] + "\n\n";
else sReturn += sPEKind + "\n\n";
} // foreach

// Not useful info
// if (dFlags.ContainsKey(sMachine)) sReturn += dFlags[sMachine] + "\n\n";
// else sReturn += sMachine + "\n\n";
sReturn += "Running in " + (IntPtr.Size == 8 ? "64" : "32") + "-bit mode.";
// sReturn = sReturn.Replace("\nTargets a ", "\nRunning on a ");
// Dialog.Show("Portable Executable Kind", sReturn);
return sReturn;
} // GetPortableExecutableKind method

public static string GetBomStringFromBytes(byte[] aBom) {
string sReturn = "";
foreach (byte b in aBom) {
if (sReturn.Length > 0) sReturn += "|";
sReturn += b;
}
return sReturn;
} // GetBomStringFromBytes method

public static string GetBomStringFromFile(string sFile) {
FileStream file = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.Read);
byte[] aBom = new byte[4];
int iCount = file.Read(aBom, 0, 4);
file.Close();
byte[] aReturn = new byte[iCount];
for (int i = 0; i < iCount; i++) aReturn[i] = aBom[i];
return GetBomStringFromBytes(aReturn);
} // GetBom method

public static Dictionary<string, int> GetBomDictionary() {
Dictionary<string, int> dCodes = new Dictionary<string, int>();
Dictionary<string, int> dBoms = new Dictionary<string, int>();
dCodes.Add("Unicode (Big-Endian)", 1201);
dCodes.Add("Unicode (UTF-32 Big-Endian)", 12001);
dCodes.Add("Unicode (UTF-32)", 12000);
// dCodes.Add("Unicode (UTF-7)", 65000);
dCodes.Add("Unicode (UTF-8)", 65001);
dCodes.Add("Unicode", 1200);

string sBody = "";
foreach (string sKey in dCodes.Keys) {
int iValue = dCodes[sKey];
Encoding en = Encoding.GetEncoding(iValue);
string sFile = Path.GetTempFileName();
File.WriteAllText(sFile, sBody, en);

string sBom = GetBomStringFromFile(sFile);
// MessageBox.Show(en.EncodingName, sBom);
// if (dBoms.ContainsKey(sBom)) MessageBox.Show(en.EncodingName, Encoding.GetEncoding(dBoms[sBom]).EncodingName);
dBoms.Add(sBom, iValue);
File.Delete(sFile);
}
return dBoms;
} // GetBomDictionary method

public static Encoding GetFileEncoding(string sFile) {
Dictionary<string, int> dBom = GetBomDictionary();
return GetFileEncoding(sFile, dBom);
} // GetFileEncoding method

public static Encoding GetFileEncoding(string sFile, Dictionary<string, int> dBom) {
// foreach (string s in dBom.Keys) Console.WriteLine(s);

string sBom = GetBomStringFromFile(sFile);
Encoding en = Encoding.Default;
// if (dBom.ContainsKey(sBom)) en = Encoding.GetEncoding(dBom[sBom]);
foreach (string s in dBom.Keys) {
if (sBom.StartsWith(s)) {
en = Encoding.GetEncoding(dBom[s]);
break;
}
}
return en;
} // GetFileEncoding method

public static void GetGoogleLanguages(out string[] aLanguageNames, out string[] aLanguageAbbreviations) {
List<string[]> lLanguages = new List<string[]>();
lLanguages.Add(new string[] {"AFRIKAANS", "af"});
lLanguages.Add(new string[] {"ALBANIAN", "sq"});
lLanguages.Add(new string[] {"AMHARIC", "am"});
lLanguages.Add(new string[] {"ARABIC", "ar"});
lLanguages.Add(new string[] {"ARMENIAN", "hy"});
lLanguages.Add(new string[] {"AZERBAIJANI", "az"});
lLanguages.Add(new string[] {"BASQUE", "eu"});
lLanguages.Add(new string[] {"BELARUSIAN", "be"});
lLanguages.Add(new string[] {"BENGALI", "bn"});
lLanguages.Add(new string[] {"BIHARI", "bh"});
lLanguages.Add(new string[] {"BULGARIAN", "bg"});
lLanguages.Add(new string[] {"BURMESE", "my"});
lLanguages.Add(new string[] {"CATALAN", "ca"});
lLanguages.Add(new string[] {"CHEROKEE", "chr"});
lLanguages.Add(new string[] {"CHINESE", "zh"});
lLanguages.Add(new string[] {"CHINESE_SIMPLIFIED", "zh-CN"});
lLanguages.Add(new string[] {"CHINESE_TRADITIONAL", "zh-TW"});
lLanguages.Add(new string[] {"CROATIAN", "hr"});
lLanguages.Add(new string[] {"CZECH", "cs"});
lLanguages.Add(new string[] {"DANISH", "da"});
lLanguages.Add(new string[] {"DHIVEHI", "dv"});
lLanguages.Add(new string[] {"DUTCH", "nl"});
lLanguages.Add(new string[] {"ENGLISH", "en"});
lLanguages.Add(new string[] {"ESPERANTO", "eo"});
lLanguages.Add(new string[] {"ESTONIAN", "et"});
lLanguages.Add(new string[] {"FILIPINO", "tl"});
lLanguages.Add(new string[] {"FINNISH", "fi"});
lLanguages.Add(new string[] {"FRENCH", "fr"});
lLanguages.Add(new string[] {"GALICIAN", "gl"});
lLanguages.Add(new string[] {"GEORGIAN", "ka"});
lLanguages.Add(new string[] {"GERMAN", "de"});
lLanguages.Add(new string[] {"GREEK", "el"});
lLanguages.Add(new string[] {"GUARANI", "gn"});
lLanguages.Add(new string[] {"GUJARATI", "gu"});
lLanguages.Add(new string[] {"HEBREW", "iw"});
lLanguages.Add(new string[] {"HINDI", "hi"});
lLanguages.Add(new string[] {"HUNGARIAN", "hu"});
lLanguages.Add(new string[] {"ICELANDIC", "is"});
lLanguages.Add(new string[] {"INDONESIAN", "id"});
lLanguages.Add(new string[] {"INUKTITUT", "iu"});
lLanguages.Add(new string[] {"ITALIAN", "it"});
lLanguages.Add(new string[] {"JAPANESE", "ja"});
lLanguages.Add(new string[] {"KANNADA", "kn"});
lLanguages.Add(new string[] {"KAZAKH", "kk"});
lLanguages.Add(new string[] {"KHMER", "km"});
lLanguages.Add(new string[] {"KOREAN", "ko"});
lLanguages.Add(new string[] {"KURDISH", "ku"});
lLanguages.Add(new string[] {"KYRGYZ", "ky"});
lLanguages.Add(new string[] {"LAOTHIAN", "lo"});
lLanguages.Add(new string[] {"LATVIAN", "lv"});
lLanguages.Add(new string[] {"LITHUANIAN", "lt"});
lLanguages.Add(new string[] {"MACEDONIAN", "mk"});
lLanguages.Add(new string[] {"MALAY", "ms"});
lLanguages.Add(new string[] {"MALAYALAM", "ml"});
lLanguages.Add(new string[] {"MALTESE", "mt"});
lLanguages.Add(new string[] {"MARATHI", "mr"});
lLanguages.Add(new string[] {"MONGOLIAN", "mn"});
lLanguages.Add(new string[] {"NEPALI", "ne"});
lLanguages.Add(new string[] {"NORWEGIAN", "no"});
lLanguages.Add(new string[] {"ORIYA", "or"});
lLanguages.Add(new string[] {"PASHTO", "ps"});
lLanguages.Add(new string[] {"PERSIAN", "fa"});
lLanguages.Add(new string[] {"POLISH", "pl"});
lLanguages.Add(new string[] {"PORTUGUESE", "pt-PT"});
lLanguages.Add(new string[] {"PUNJABI", "pa"});
lLanguages.Add(new string[] {"ROMANIAN", "ro"});
lLanguages.Add(new string[] {"RUSSIAN", "ru"});
lLanguages.Add(new string[] {"SANSKRIT", "sa"});
lLanguages.Add(new string[] {"SERBIAN", "sr"});
lLanguages.Add(new string[] {"SINDHI", "sd"});
lLanguages.Add(new string[] {"SINHALESE", "si"});
lLanguages.Add(new string[] {"SLOVAK", "sk"});
lLanguages.Add(new string[] {"SLOVENIAN", "sl"});
lLanguages.Add(new string[] {"SPANISH", "es"});
lLanguages.Add(new string[] {"SWAHILI", "sw"});
lLanguages.Add(new string[] {"SWEDISH", "sv"});
lLanguages.Add(new string[] {"TAJIK", "tg"});
lLanguages.Add(new string[] {"TAMIL", "ta"});
lLanguages.Add(new string[] {"TAGALOG", "tl"});
lLanguages.Add(new string[] {"TELUGU", "te"});
lLanguages.Add(new string[] {"THAI", "th"});
lLanguages.Add(new string[] {"TIBETAN", "bo"});
lLanguages.Add(new string[] {"TURKISH", "tr"});
lLanguages.Add(new string[] {"UKRAINIAN", "uk"});
lLanguages.Add(new string[] {"URDU", "ur"});
lLanguages.Add(new string[] {"UZBEK", "uz"});
lLanguages.Add(new string[] {"UIGHUR", "ug"});
lLanguages.Add(new string[] {"VIETNAMESE", "vi"});
lLanguages.Add(new string[] {"UNKNOWN", ""});

int iCount = lLanguages.Count;
aLanguageNames = new string[iCount];
aLanguageAbbreviations = new string[iCount];
for (int i = 0; i < iCount; i++) {
string[] a = lLanguages[i];
aLanguageNames[i] = a[0];
aLanguageAbbreviations[i] = a[1];
};
} // GetGoogleLanguages method

public static string[] OldGetGoogleLanguages() {
HomerList hl = new HomerList();
hl.Add("Arabic");
hl.Add("Bulgarian");
hl.Add("Chinese");
hl.Add("Catalan");
hl.Add("Croatian");
hl.Add("Czech");
hl.Add("Danish");
hl.Add("Dutch");
hl.Add("English");
hl.Add("Filipino");
hl.Add("Finnish");
hl.Add("French");
hl.Add("German");
hl.Add("Greek");
hl.Add("Hebrew");
hl.Add("Hindi");
hl.Add("Indonesian");
hl.Add("Italian");
hl.Add("Japanese");
hl.Add("Korean");
hl.Add("Latvian");
hl.Add("Lithuanian");
hl.Add("Norwegian");
hl.Add("Polish");
hl.Add("Portuguese");
hl.Add("Romanian");
hl.Add("Russian");
hl.Add("Spanish");
hl.Add("Serbian");
hl.Add("Slovak");
hl.Add("Slovenian");
hl.Add("Swedish");
hl.Add("Turkish");
hl.Add("Ukrainian");
hl.Add("Vietnamese");
hl.Add("Unknown");
return hl.ToArray();
} // OldGetGoogleLanguages method

public static bool MailMessage(string sRecipient, string sSubject, string sBody) {
sBody = Util.RegExpReplaceCase(sBody, "\r\n", "\r");
sBody = Util.RegExpReplaceCase(sBody, "\n", "\r");
sBody = Util.RegExpReplaceCase(sBody, "\r", "\r\n");
sBody = Util.RegExpReplaceCase(sBody, "\r\n", "%0D%0A");
sBody = Util.RegExpReplaceCase(sBody, " ", "%20");
sBody = Util.RegExpReplaceCase(sBody, "\t", "%09");
sBody = Util.RegExpReplaceCase(sBody, "\"", "%22");
sBody = Util.RegExpReplaceCase(sBody, "'", "%27");
sBody = Util.RegExpReplaceCase(sBody, "\\\\", "%5C");
// sBody = StringReplaceCase(sBody, "\\", "%5C");
// string sCommand = "mailto:?BODY=" + sBody;
string sCommand = "mailto:" + sRecipient + "?SUBJECT=" + sSubject + "&BODY=" + sBody;
bool bResult;
try {
Process.Start(sCommand);
bResult = true;
}
catch (Exception ex) {
Dialog.Show("Error", ex.Message);
bResult = false;
}
return bResult;
} // MailMessage method

public static Encoding OldGetFileEncoding(string sFile) {
System.Text.Encoding enc = null;
System.IO.FileStream file = new System.IO.FileStream(sFile,
FileMode.Open, FileAccess.Read, FileShare.Read);
if (file.CanSeek)
{
byte[] bom = new byte[4]; // Get the byte-order mark, if there is one
file.Read(bom, 0, 4);
if ((bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) || // utf-8
(bom[0] == 0xff && bom[1] == 0xfe) || // ucs-2le, ucs-4le, and ucs-16le
(bom[0] == 0xfe && bom[1] == 0xff) || // utf-16 and ucs-2
(bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)) // ucs-4
{
enc = System.Text.Encoding.Unicode;
}
else
{
// enc = System.Text.Encoding.ASCII;
enc = System.Text.Encoding.Default;
}

// Now reposition the file cursor back to the start of the file
file.Seek(0, System.IO.SeekOrigin.Begin);
}
else
{
// The file cannot be randomly accessed, so you need to decide what to set the default to
// based on the data provided. If you're expecting data from a lot of older applications,
// default your encoding to Encoding.ASCII. If you're expecting data from a lot of newer
// applications, default your encoding to Encoding.Unicode. Also, since binary files are
// single byte-based, so you will want to use Encoding.ASCII, even though you'll probably
// never need to use the encoding then since the Encoding classes are really meant to get
// strings from the byte array that is the file.

// enc = System.Text.Encoding.ASCII;
enc = System.Text.Encoding.Default;
}
file.Close();
return enc;
} // OldGetFileEncoding method

[DllImport("user32.dll")]
public static extern IntPtr SetClipboardViewer(IntPtr h);

[DllImport("user32.dll")]
public static extern IntPtr     ChangeClipboardChain(IntPtr hCurrentClipboardViewer, IntPtr hNextClipboardViewer);

public static string GetTempFolder() {
object oSystem = COM.CreateObject("Scripting.FileSystemObject");
Object oDir = COM.CallMethod(oSystem, "GetSpecialFolder", new object[] {2});
string sPath = (string) COM.GetProperty(oDir, "Path");
return sPath;
} // GetTempFolder method

public static bool IsUnicode(string sText) {
foreach (char c in sText) {
// if (((int) c) > 255) Dialog.Show(c, (int) c);
if (((int) c) > 255) return true;
}
return false;
} // IsUnicode method

public static string Replicate(string sText, int iCount) {
string sReturn = sText;
for (int i = 1; i < iCount; i++) sReturn += sText;
return sReturn;
} // Replicate method

public static bool IsAppActiveWindow() {
IntPtr h = Win32.GetForegroundWindow();
foreach (Form frm in Application.OpenForms) if (frm.Handle == h) return true;
return false;
} // IsAppActiveWindow method

public static bool Spell(object oText) {
string sText = oText.ToString();
bool bReturn = false;
string sReturn = "";
for (int i = 0; i < sText.Length; i++) {
string s = sText.Substring(i, 1);
switch (s) {
case " " :
s = "Space";
sReturn += "Space\n";
break;
default :
sReturn += s + "\n";
break;
}
s = " " + s + " ";
bReturn = Say(s);
}
// return Say(sReturn, bGlobal);
return bReturn;
} // Spell method

public static bool Say(object oText) {
bool bGlobal = false;
return Say(oText, bGlobal);
} // Say method

public static bool Say(object oText, bool bGlobal) {
string sText = oText.ToString();
if (sText.Trim().Length == 0) sText = "Blank";
if (!App.ExtraSpeech) {
Util.StringAppend2File(sText + "\r\n", App.SpeechLog);
return false;
}

if (!bGlobal) {
if (!IsAppActiveWindow()) return false;

Microsoft.VisualBasic.Devices.Keyboard keyboard = new Microsoft.VisualBasic.Devices.Keyboard();
if (keyboard.AltKeyDown && keyboard.CtrlKeyDown) return false;
//if (keyboard.ScrollLock ) return false;
}

// if (Win32.JAWSSay(sText)) return true;
if (COM.JFWSay(sText)) return true;
else if (COM.WESay(sText)) return true;
else if (Win32.SASay(sText)) return true;
else if (Win32.NVDASay(sText)) return true;
else if (COM.SAPISay(sText)) return true;
else return false;
} // Say method

public static string Key2String(Keys keyData) {
return TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString(keyData);
} // Key2String method

public static Keys String2Key(string sKey) {
return (Keys) TypeDescriptor.GetConverter(typeof(Keys)).ConvertFromString(sKey);
} // String2Key method

public static string Font2String(Font font) {
return TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(font);
} // Font2String method

public static Font String2Font(string sFont) {
return (Font) TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(sFont);
} // String2Font method

public static string Color2String(Color color) {
return TypeDescriptor.GetConverter(typeof(Color)).ConvertToString(color);
} // Color2String method

public static Color String2Color(string sColor) {
return (Color) TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(sColor);
} // String2Color method

public static string GetFriendlyKeyName(string sKey) {
if (sKey.Contains("+OemQuotes")) sKey = sKey.Replace("+OemQuotes", "+Apostrophe");
if (sKey.Contains("+Back")) sKey = sKey.Replace("+Back", "+Backspace");
if (sKey.Contains("+Oem5")) sKey = sKey.Replace("+Oem5", "+Backslash");
if (sKey.Contains("+Oemplus")) sKey = sKey.Replace("+Oemplus", "+Equals");
if (sKey.Contains("+OemMinus")) sKey = sKey.Replace("+OemMinus", "+Dash");
if (sKey.Contains("+OemSemicolon")) sKey = sKey.Replace("+OemSemicolon", "+Semicolon");
if (sKey.Contains("+D6")) sKey = sKey.Replace("+D6", "+Caret");
if (sKey.Contains("+D0")) sKey = sKey.Replace("+D0", "+0");
if (sKey.Contains("+OemQuestion")) sKey = sKey.Replace("+OemQuestion", "+Slash");
if (sKey.Contains("+OemOpenBrackets")) sKey = sKey.Replace("+OemOpenBrackets", "+LeftBracket");
if (sKey.Contains("+OemCloseBrackets")) sKey = sKey.Replace("+OemCloseBrackets", "+RightBracket");
if (sKey.Contains("+Oemcomma")) sKey = sKey.Replace("+Oemcomma", "+Comma");
if (sKey.Contains("+OemPeriod")) sKey = sKey.Replace("+OemPeriod", "+Period");
return sKey;
} // GetFriendlyKeyName method

public static string RegExpReplaceEquiv(string sText, string sMatch, string sReplace) {
bool bCaseSensitive = false;
return RegExpReplace(sText, sMatch, sReplace, bCaseSensitive);
} // RegExpReplaceEquiv method

public static string RegExpReplaceCase(string sText, string sMatch, string sReplace) {
bool bCaseSensitive = true;
return RegExpReplace(sText, sMatch, sReplace, bCaseSensitive);
} // RegExpReplaceCase method

public static string RegExpReplace(string sText, string sMatch, string sReplace, bool bCaseSensitive) {
RegexOptions options = RegexOptions.Multiline;
if (!bCaseSensitive) options |= RegexOptions.IgnoreCase;
Regex rx = new Regex(sMatch, options);
string sReturn = rx.Replace(sText, sReplace);
return sReturn;
} // RegExpReplace method

public static int RegExpCountEquiv(string sText, string sMatch) {
bool bCaseSensitive = false;
return RegExpCount(sText, sMatch, bCaseSensitive);
} // RegExpCountEquiv method

public static int RegExpCountCase(string sText, string sMatch) {
bool bCaseSensitive = true;
return RegExpCount(sText, sMatch, bCaseSensitive);
} // RegExpCountCase method

public static int RegExpCount(string sText, string sMatch, bool bCaseSensitive) {
RegexOptions options = RegexOptions.Multiline;
if (!bCaseSensitive) options |= RegexOptions.IgnoreCase;
Regex rx = new Regex(sMatch, options);
MatchCollection matches = rx.Matches(sText);
int iReturn = matches.Count;
return iReturn;
} // RegExpCount method

public static string[] RegExpExtractEquiv(string sText, string sMatch) {
bool bCaseSensitive = false;
return RegExpExtract(sText, sMatch, bCaseSensitive);
} // RegExpExtractEquiv method

public static string[] RegExpExtractCase(string sText, string sMatch) {
bool bCaseSensitive = true;
return RegExpExtract(sText, sMatch, bCaseSensitive);
} // RegExpExtractCase method

public static string[] RegExpExtract(string sText, string sMatch, bool bCaseSensitive) {
RegexOptions options = RegexOptions.Multiline;
if (!bCaseSensitive) options |= RegexOptions.IgnoreCase;
Regex rx = new Regex(sMatch, options);
MatchCollection matches = rx.Matches(sText);
string[] aReturn = new string[matches.Count];
for (int i = 0; i < aReturn.Length; i++) aReturn[i] = matches[i].Value;
return aReturn;
} // RegExpExtract method

public static object[][] RegExpExtractWithIndex(string sText, string sMatch, bool bCaseSensitive) {
RegexOptions options = RegexOptions.Multiline;
if (!bCaseSensitive) options |= RegexOptions.IgnoreCase;
Regex rx = new Regex(sMatch, options);
MatchCollection matches = rx.Matches(sText);
object[][] aReturn = new object[matches.Count][];
for (int i = 0; i < aReturn.Length; i++) aReturn[i] = new object[] {matches[i].Index, matches[i].Value};
return aReturn;
} // RegExpExtractWithIndex method

public static object[] RegExpContainsEquiv(string sText, string sMatch) {
int iStart = 0;
return RegExpContainsEquiv(sText, sMatch, iStart);
} // RegExpContainsEquiv method

public static object[] RegExpContainsEquiv(string sText, string sMatch, int iStart) {
bool bCaseSensitive = false;
bool bLast = false;
return RegExpContains(sText, sMatch, bCaseSensitive, bLast, iStart);
} // RegExpContainsEquiv method

public static object[] RegExpContainsCase(string sText, string sMatch) {
int iStart = 0;
return RegExpContainsCase(sText, sMatch, iStart);
} // RegExpContainsCase method

public static object[] RegExpContainsCase(string sText, string sMatch, int iStart) {
bool bCaseSensitive = true;
bool bLast = false;
return RegExpContains(sText, sMatch, bCaseSensitive, bLast, iStart);
} // RegExpContainsCase method

public static object[] RegExpContainsLastEquiv(string sText, string sMatch) {
bool bCaseSensitive = false;
bool bLast = true;
return RegExpContains(sText, sMatch, bCaseSensitive, bLast);
} // RegExpContainsLastEquiv method

public static object[] RegExpContainsLastCase(string sText, string sMatch) {
bool bCaseSensitive = true;
bool bLast = true;
return RegExpContains(sText, sMatch, bCaseSensitive, bLast);
} // RegExpContainsLastCase method

public static object[] RegExpContains(string sText, string sMatch, bool bCaseSensitive, bool bLast) {
int iStart;
//if (bLast)  iStart = sText.Length - 1;
if (bLast)  iStart = sText.Length;
else iStart = 0;
return RegExpContains(sText, sMatch, bCaseSensitive, bLast, iStart);
} // RegExpContains method

public static object[] RegExpContains(string sText, string sMatch, bool bCaseSensitive, bool bLast, int iStart) {
RegexOptions options = RegexOptions.Multiline;
if (!bCaseSensitive) options |= RegexOptions.IgnoreCase;
if (bLast) options |= RegexOptions.RightToLeft;
Regex rx = new Regex(sMatch, options);

Match match = rx.Match(sText, iStart);
object[] aReturn = new object[] {-1, null};
// Dialog.Show(match.Success);
if (match.Success) aReturn = new object[] {match.Index, match.Value};
return aReturn;
} // RegExpContains method

public static bool Equiv(string s1, string s2) {
return String.Compare(s1, s2, true) == 0;
} // Equiv method

public static string Pluralize(int iCount, string sSingular) {
string sPlural = null;
return Pluralize(iCount, sSingular, sPlural);
} // Pluralize method

public static string Pluralize(int iCount, string sSingular, string sPlural) {
if (sPlural == null) sPlural = sSingular + "s";
string sReturn = iCount.ToString() + " ";
if (iCount == 1) sReturn += sSingular;
else sReturn += sPlural;
return sReturn;
} // Pluralize method

public static string File2String(string sFile) {
Encoding en = null;
return File2String(sFile, ref en);
} // File2String method

public static string File2String(string sFile, ref Encoding en) {
//return MyIO.FileSystem.ReadAllText(sFile);
//return System.IO.File.ReadAllText(sFile);
// Dialog.Show("Encoding", Util.GetFileEncoding(sFile));
if (en == null) en = Util.GetFileEncoding(sFile, App.BomDictionary);
// return System.IO.File.ReadAllText(sFile, System.Text.Encoding.Default);
// return System.IO.File.ReadAllText(sFile, encoding);
string sText = System.IO.File.ReadAllText(sFile, en);
return sText;
} // File2String method

public static string OldFile2String(string sFile) {
if (!File.Exists(sFile)) return "";
StreamReader textReader = new StreamReader(sFile);
string sBody = textReader.ReadToEnd();
textReader.Close();
return sBody;
} // OldFile2String method

public static void String2FileU(string sBody, string sFile) {
// bool bAppend = false;
// MyIO.FileSystem.WriteAllText(sFile, sBody, bAppend);
System.IO.File.WriteAllText(sFile, sBody, Encoding.UTF8);
} // String2FileU method

public static void StringAppend2File(string sBody, string sFile) {
bool bAppend = true;
MyIO.FileSystem.WriteAllText(sFile, sBody, bAppend);
} // StringAppend2File method

public static void String2FileA(string sBody, string sFile) {
Encoding en = null;
String2FileA(sBody, sFile, en);
} // String2FileA method

public static void String2FileA(string sBody, string sFile, Encoding en) {
StreamWriter textWriter = new StreamWriter(sFile);
textWriter.Write(sBody);
textWriter.Close();
} // OldString2File method

public static void String2File(string sBody, string sFile) {
Encoding en = null;
String2File(sBody, sFile, ref en);
} // String2File method

public static void String2File(string sBody, string sFile, ref Encoding en) {
// Dialog.Show(IsUnicode(sBody));
if (en != null) {}
// Do nothing
else if (IsUnicode(sBody))en = Encoding.UTF8;
else en = Encoding.Default;

// sBody = Util.Convert2WinLineBreak(sBody);
File.WriteAllText(sFile, sBody, en);
} // String2File method

public static string Quote(string sText) {
return "\"" + Unquote(sText) + "\"";
} // Quote method

public static string Unquote(string sText) {
return sText.Trim('"');
} // Unquote method

public static string ConvertQuotes(string sText) {
string sReturn = sText.Replace(@"?", @"""");
sReturn  = sReturn.Replace(@"?", @"""");
sReturn  = sReturn.Replace(@"-", @"-");
sReturn  = sReturn.Replace(@"?", @"...");
sReturn  = sReturn.Replace(@"?", @"'");
sReturn  = sReturn.Replace(Util.Code2String(65533), @"'");
return sReturn;
} // ConvertQuotes method

public static string Convert2Ascii(string sText) {
int iLength = sText.Length;
for (int i = iLength - 1; i >= 0; i--) {
if ((int) sText[i] > 127) sText = sText.Remove(i, 1);
}
return sText;
} //Convert2Ascii method

public static string Convert2MacLineBreak(string sText) {
//Convert to Macintosh line break, \r;
string sMatch, sReplace;

sMatch = "\r\n";
sReplace = "\r";
sText = Util.RegExpReplaceCase(sText, sMatch, sReplace);
sMatch = "\n";
sText = Util.RegExpReplaceCase(sText, sMatch, sReplace);
return sText;
} // Convert2MacLineBreak metod

public static string Convert2UnixLineBreak(string sText) {
//Convert to Unix line break, \n;
string sMatch, sReplace;
sMatch = "\r\n";
sReplace = "\n";
sText = Util.RegExpReplaceCase(sText, sMatch, sReplace);
sMatch = "\r";
sText = Util.RegExpReplaceCase(sText, sMatch, sReplace);
return sText;
} // Convert2UnixLineBreak method

public static string Convert2WinLineBreak(string sText) {
//Convert to standard Windows line break, \r\nVar;
string sMatch, sReplace;
sMatch = "\r\n";
sReplace = "\n";
sText = Util.RegExpReplaceCase(sText, sMatch, sReplace);
sMatch = "\r";
sText = Util.RegExpReplaceCase(sText, sMatch, sReplace);
sMatch = "\n";
sReplace = "\r\n";
sText = Util.RegExpReplaceCase(sText, sMatch, sReplace);
return sText;
} // Convert2WinLineBreakMethod

public static int RunHideWait(string sPath) {
return Interaction.Shell(sPath, AppWinStyle.Hide, true, -1);
} //RunHideWait method

public static int RunHide(string sPath) {
return Interaction.Shell(sPath, AppWinStyle.Hide, false, -1);
} //RunHide method

public static int Run(string sPath) {
return Interaction.Shell(sPath, AppWinStyle.NormalFocus, false, -1);
} //Run method

public static int RunWait(string sPath) {
return Interaction.Shell(sPath, AppWinStyle.NormalFocus, true, -1);
} //RunWait method

public static void ActivatePid(int iPid) {
Interaction.AppActivate(iPid);
} // ActivatePid method

public static bool ActivateProcess(string sProcess) {
Process[] processes = Process.GetProcessesByName(sProcess);
if (processes.Length == 0) return false;
Process process = processes[0];
//Dialog.Show(process.ProcessName, process.MainWindowTitle);

int iPid = processes[0].Id;
try {
ActivatePid(iPid);
return true;
}
catch {
return false;
}
} // ActivateProcess method

public static void ActivateTitle(string sTitle) {
try {
Interaction.AppActivate(sTitle);
}
catch {
IntPtr h = Win32.FindWindow(0, sTitle);
if ((int) h != 0) Win32.SetForegroundWindow(h);
}
} // ActivateTitle method

public static void Beep() {
Interaction.Beep();
} // Beep method

public static object If(bool bExp, object oTrue, object oFalse) {
return Interaction.IIf(bExp, oTrue, oFalse);
} // If method

public static int If(bool bExp, int iTrue, int iFalse) {
if (bExp) return iTrue;
else return iFalse;
} // If method

public static string If(bool bExp, string sTrue, string sFalse) {
if (bExp) return sTrue;
else return sFalse;
} // If method

public static string GetCommandLine() {
return Interaction.Command();
} // GetCommandLine method

public static string[] OldGetFiles(string sDir, string sFilter, bool bSubDirs) {
string sFiles;
string[] a, aDirs, aFiles;
StringBuilder sb = new StringBuilder();

aDirs = Directory.GetDirectories(sDir);
if (bSubDirs) {
foreach (string s in aDirs) {
a = OldGetFiles(s, sFilter, bSubDirs);
sFiles = String.Join("\n", a);
if (sFiles.Length > 0) sb.Append(sFiles + "\n");
}
}

aFiles = Directory.GetFiles(sDir, sFilter);
sFiles = String.Join("\n", aFiles);
if (sFiles.Length > 0) sb.Append(sFiles + "\n");

sFiles = sb.ToString().TrimEnd();
if (sFiles.Length > 0) aFiles = sFiles.Split('\n');
else aFiles = new string[] {};
return aFiles;
} // OldGetFiles method

public static string[] FindInFiles(string sText, string sDir, string[] aFilters, bool bSubdirs) {
bool bIgnoreCase = true;
MyIO.SearchOption searchOption;
if (bSubdirs) searchOption = MyIO.SearchOption.SearchAllSubDirectories;
else searchOption = MyIO.SearchOption.SearchTopLevelOnly;
ReadOnlyCollection<string> oReturn = MyIO.FileSystem.FindInFiles(sDir, sText, bIgnoreCase, searchOption, aFilters);
string[] aReturn = new string[oReturn.Count];
for (int i = 0; i < aReturn.Length; i++) aReturn[i] = oReturn[i];
return aReturn;
} // FindInFiles method

public static string[] GetFiles(string sDir, string[] aFilters, bool bSubdirs) {
MyIO.SearchOption searchOption;
if (bSubdirs) searchOption = MyIO.SearchOption.SearchAllSubDirectories;
else searchOption = MyIO.SearchOption.SearchTopLevelOnly;
ReadOnlyCollection<string> oReturn = MyIO.FileSystem.GetFiles(sDir, searchOption, aFilters);
string[] aReturn = new string[oReturn.Count];
for (int i = 0; i < aReturn.Length; i++) aReturn[i] = oReturn[i];
return aReturn;
} // GetFiles method

public static string[] GetPathsWithExtensions(string[] aFiles, string sExtensions) {
string sResult = "." + sExtensions.Trim().Replace(" ", " .");
sResult = sResult.Replace("..", ".");
string [] aResults = sResult.Split(' ');
List<string> list = new List<string>(aFiles);
for (int i = list.Count -1; i >=0; i--) {
string sFile = list[i];
string sExtension = Path.GetExtension(sFile).ToLower();
if (sExtension.Length == 0) sExtension = ".";
if (Array.IndexOf(aResults, sExtension) == -1) list.RemoveAt(i);
}
return list.ToArray();
} // GetPathsWithExtensions method

public static string GetExtensions(string sDir) {
return GetExtensions(Directory.GetFiles(sDir));
} // GetExtensions method

public static string GetExtensions(string[] aFiles) {
//string[] aFilters = new string[] {"*.*"};
//bool bSubdirs = false;
//string[] aFiles = GetFiles(sDir, aFilters, bSubdirs);
List<string> list = new List<string>(aFiles.Length);
for (int i = 0; i < aFiles.Length; i++) {
string s = aFiles[i];
s = Path.GetExtension(s);
//if (s.Length == 0) continue;
s = s.TrimStart('.');
s = s.ToLower();
if (s.Length == 0) s = ".";
if (!list.Contains(s)) list.Add(s);
}

list.Sort();
string[] aExtensions = list.ToArray();
return String.Join(" ", aExtensions);
} // GetExtensions method

public static bool PathExists(string sPath) {
return (Directory.Exists(sPath) || File.Exists(sPath));
} // PathExists method

public static void DeletePath(string sPath, bool bRecycle) {
FileAttributes attr = File.GetAttributes(sPath);
FileAttributes flag = FileAttributes.ReadOnly;
File.SetAttributes(sPath, (attr | flag) ^ flag);
if (Directory.Exists(sPath)) DeleteDirectory(sPath, bRecycle);
else if (File.Exists(sPath)) DeleteFile(sPath, bRecycle);
} // DeletePath method

public static void DeleteDirectory(string sPath, bool bRecycle) {
if (!Directory.Exists(sPath)) return;
if (bRecycle) MyIO.FileSystem.DeleteDirectory(sPath, MyIO.UIOption.OnlyErrorDialogs, MyIO.RecycleOption.SendToRecycleBin, MyIO.UICancelOption.ThrowException);
//if (bRecycle) MyIO.FileSystem.DeleteDirectory(sPath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
else MyIO.FileSystem.DeleteDirectory(sPath, MyIO.UIOption.OnlyErrorDialogs, MyIO.RecycleOption.DeletePermanently, MyIO.UICancelOption.ThrowException);
//else MyIO.FileSystem.DeleteDirectory(sPath, MyIO.UIOption.AllDialogs, MyIO.RecycleOption.DeletePermanently, MyIO.UICancelOption.ThrowException);
} // DeleteDirectory method

public static void DeleteFile(string sPath, bool bRecycle) {
if (!File.Exists(sPath)) return;
if (bRecycle) MyIO.FileSystem.DeleteFile(sPath, MyIO.UIOption.OnlyErrorDialogs, MyIO.RecycleOption.SendToRecycleBin, MyIO.UICancelOption.ThrowException);
else MyIO.FileSystem.DeleteFile(sPath, MyIO.UIOption.OnlyErrorDialogs, MyIO.RecycleOption.DeletePermanently, MyIO.UICancelOption.ThrowException);
} // DeleteFile method

public static void CopyDirectory(string sSource, string sTarget, bool bRecycle) {
if (Directory.Exists(sTarget)) DeleteDirectory(sTarget, bRecycle);
else if (File.Exists(sTarget)) DeleteFile(sTarget, bRecycle);
//MyIO.FileSystem.CopyDirectory(sSource, sTarget, MyIO.UIOption.OnlyErrorDialogs, MyIO.UICancelOption.ThrowException);
MyIO.FileSystem.CopyDirectory(sSource, sTarget, MyIO.UIOption.AllDialogs, MyIO.UICancelOption.ThrowException);
} // CopyDirectory method

public static void MoveDirectory(string sSource, string sTarget, bool bRecycle) {
if (Directory.Exists(sTarget)) DeleteDirectory(sTarget, bRecycle);
else if (File.Exists(sTarget)) DeleteFile(sTarget, bRecycle);
//MyIO.FileSystem.MoveDirectory(sSource, sTarget, MyIO.UIOption.OnlyErrorDialogs, MyIO.UICancelOption.ThrowException);
MyIO.FileSystem.MoveDirectory(sSource, sTarget, MyIO.UIOption.AllDialogs, MyIO.UICancelOption.ThrowException);
} // MoveDirectory method

public static void CopyFile(string sSource, string sTarget, bool bRecycle) {
if (Directory.Exists(sTarget)) DeleteDirectory(sTarget, bRecycle);
else if (File.Exists(sTarget)) DeleteFile(sTarget, bRecycle);
MyIO.FileSystem.CopyFile(sSource, sTarget, MyIO.UIOption.OnlyErrorDialogs, MyIO.UICancelOption.ThrowException);
} // CopyFile method

public static void MoveFile(string sSource, string sTarget, bool bRecycle) {
if (Directory.Exists(sTarget)) DeleteDirectory(sTarget, bRecycle);
else if (File.Exists(sTarget)) DeleteFile(sTarget, bRecycle);
MyIO.FileSystem.MoveFile(sSource, sTarget, MyIO.UIOption.OnlyErrorDialogs, MyIO.UICancelOption.ThrowException);
} // MoveFile method

public static void SendKeys(string sKeys) {
System.Windows.Forms.SendKeys.SendWait(sKeys);
} // SendKeys method

public static string ProperCase(string sText) {
return Microsoft.VisualBasic.Strings.StrConv(sText, VbStrConv.ProperCase, 0);

/*
string[] aWords = sText.Split(' ');
for (int i = 0; i < aWords.Length; i++) {
string sWord = aWords[i];
string sInitial = sWord.Substring(0, 1).ToUpper();
string sRest = "";
if (sWord.Length > 1) sRest = sWord.Substring(1).ToLower();
sWord = sInitial + sRest;
aWords[i] = sWord;
}

string sReturn = String.Join(" ", aWords);
return sReturn;
*/
} // ProperCase method

public static string SwapCase(string sText) {
string sReturn = "";
StringBuilder sb = new StringBuilder(sText.Length);
for (int i = 0; i < sText.Length; i++) {
string s = sText.Substring(i, 1);
string sLower = s.ToLower();
string sUpper = s.ToUpper();
if (sLower == sUpper) sb.Append(s);
else if (s == sLower) sb.Append(sUpper);
else if (s == sUpper) sb.Append(sLower);
}

sReturn = sb.ToString();
return sReturn;
} // SwapCase method

public static int Month2Num(string sMonth) {
sMonth = Util.ProperCase(sMonth.Trim());
string[] aMonths = {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
int iReturn = -1;
for (int i = 0; i < aMonths.Length; i++) {
string s = aMonths[i];
if (!s.StartsWith(sMonth)) continue;
iReturn = i + 1;
break;
}
return iReturn;
} // Month2Num method

public static int Day2Num(string sDay) {
sDay = Util.ProperCase(sDay.Trim());
string[] aDays = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
int iReturn = -1;
for (int i = 0; i < aDays.Length; i++) {
string s = aDays[i];
if (!s.StartsWith(sDay)) continue;
iReturn = i;
break;
}
return iReturn;
} // Day2Num method

public static string Type2String(object o) {
return TypeDescriptor.GetConverter(o.GetType()).ConvertToString(o);
} // Type2String method

public static object String2Type(string s, object o) {
return TypeDescriptor.GetConverter(o.GetType()).ConvertFromString(s);
} // String2Type method

public static void TerminateProcess(string sName) {
bool bLoop = true;
while (bLoop) {
Process[] processes = Process.GetProcessesByName(sName);
if (processes.Length == 0) break;

Process process = processes[0];
int iPid = process.Id;
process.CloseMainWindow();
System.Threading.Thread.Sleep(500);
try {
process = Process.GetProcessById(iPid);
process.Kill();
}
catch {
break;
}
}
} // TerminateProcess method

public static string GetLfn(string sPath) {
object oShell = COM.CreateObject("WScript.Shell");
object oShortcut = COM.CallMethod(oShell, "CreateShortcut", "temp.lnk");
COM.SetProperty(oShortcut, "TargetPath", sPath);
string sReturn = (string) COM.GetProperty(oShortcut, "TargetPath");
//COM.Release(ref oShortcut);
//COM.Release(ref oShell);
return sReturn;
} // GetLfn method

public static string String2Html(string sText) {
return HttpUtility.HtmlEncode(sText);
} // String2Html method

public static string ExpandCommandLine(string sCommand, string sSource, string sTarget ) {
// Dialog.Show(sTarget);

sCommand = sCommand.Replace("%NetDirLong%", App.NetDir);
sCommand = sCommand.Replace("%NetDir%", Win32.GetShortPath(App.NetDir));

sCommand = sCommand.Replace("%ProgDirLong%", App.ProgramDir);
sCommand = sCommand.Replace("%ProgDir%", Win32.GetShortPath(App.ProgramDir));

sCommand = sCommand.Replace("%DataDirLong%", App.DataDir);
sCommand = sCommand.Replace("%DataDir%", Win32.GetShortPath(App.DataDir));

if (sSource.Length > 0) {
sCommand = sCommand.Replace("%SourceLong%", sSource);
sCommand = sCommand.Replace("%Source%", Win32.GetShortPath(sSource));

sCommand = sCommand.Replace("%SourceDirLong%", Path.GetDirectoryName(sSource));
sCommand = sCommand.Replace("%SourceDir%", Win32.GetShortPath(Path.GetDirectoryName(sSource)));

sCommand = sCommand.Replace("%SourceNameLong%", Path.GetFileName(sSource));
sCommand = sCommand.Replace("%SourceName%", Path.GetFileName(Win32.GetShortPath(sSource)));

sCommand = sCommand.Replace("%SourceRootLong%", Path.GetFileNameWithoutExtension(sSource));
sCommand = sCommand.Replace("%SourceRoot%", Path.GetFileNameWithoutExtension(Win32.GetShortPath(sSource)));

sCommand = sCommand.Replace("%SourceExtLong%", Path.GetExtension(sSource));
sCommand = sCommand.Replace("%SourceExt%", Path.GetExtension(Win32.GetShortPath(sSource)));
}

if (sTarget.Length > 0) {
sCommand = sCommand.Replace("%TargetLong%", sTarget);
// Dialog.Show(sCommand, "");
sCommand = sCommand.Replace("%Target%", Win32.GetShortPath(sTarget));
// Dialog.Show(sCommand, "");

sCommand = sCommand.Replace("%TargetDirLong%", Path.GetDirectoryName(sTarget));
sCommand = sCommand.Replace("%TargetDir%", Win32.GetShortPath(Path.GetDirectoryName(sTarget)));

sCommand = sCommand.Replace("%TargetNameLong%", Path.GetFileName(sTarget));
sCommand = sCommand.Replace("%TargetName%", Path.GetFileName(Win32.GetShortPath(sTarget)));

sCommand = sCommand.Replace("%TargetRootLong%", Path.GetFileNameWithoutExtension(sTarget));
sCommand = sCommand.Replace("%TargetRoot%", Path.GetFileNameWithoutExtension(Win32.GetShortPath(sTarget)));

sCommand = sCommand.Replace("%TargetExtLong%", Path.GetExtension(sTarget));
sCommand = sCommand.Replace("%TargetExt%", Path.GetExtension(Win32.GetShortPath(sTarget)));
}

sCommand = sCommand.Replace("%TempFile%", App.TempFile);
sCommand = Environment.ExpandEnvironmentVariables(sCommand);
return sCommand.Trim();
} // ExpandCommandLine method

public static string GetProgramOutput(string sExe, string sParams) {
Process process = new Process();
ProcessStartInfo startInfo = new ProcessStartInfo(sExe, sParams);
startInfo.UseShellExecute = false;
//startInfo.RedirectStandardInput = true;
startInfo.RedirectStandardOutput = true;
startInfo.RedirectStandardError = true;
startInfo.WorkingDirectory = Path.GetDirectoryName(sExe);
startInfo.ErrorDialog = true;
startInfo.CreateNoWindow = true;
startInfo.WindowStyle = ProcessWindowStyle.Hidden;
process.StartInfo = startInfo;
process.Start();
StreamReader stream = process.StandardOutput;
process.WaitForExit();
string sText = stream.ReadToEnd();
stream.Close();
process.Close();
return sText;
} // GetProgramOutput method

public static bool ConvertString2FileFormat(string sText, string sTarget, string sTargetFormat) {
string sSourceFormat = "";
return ConvertString2FileFormat(sText, sSourceFormat, sTarget, sTargetFormat);
} // ConvertString2FileFormat method

public static bool ConvertString2FileFormat(string sText, string sSourceFormat, string sTarget, string sTargetFormat) {
/*
string sSource = Path.GetTempFileName();
if (sSourceFormat.Length > 0) {
string s = Path.ChangeExtension(sSource, sSourceFormat);
if (File.Exists(s)) File.Delete(s);
File.Move(sSource, s);
sSource = s;
}

//sSource = @"C:\edsharp\edsharp.htm";
App.TempFiles.Add(sSource);
//Util.String2File(sText, sSource);
//Util.StringAppend2File(sText, sSource);
*/

string sDir = Path.Combine(App.DataDir, "Temp");
if (Directory.Exists(sDir)) Util.DeleteDirectory(sDir, false);
Directory.CreateDirectory(sDir);
string sSource = Path.Combine(sDir, "Source.tmp");
if (sSourceFormat.Length > 0) sSource = Path.ChangeExtension(sSource, sSourceFormat);

Util.String2FileA(sText, sSource);
sSource = Win32.GetShortPath(sSource);
; Dialog.Show(sSource, Util.File2String(sSource));
string sCommand = Ini.ReadValue(App.IniFile, "Export", sTargetFormat, "");
//Dialog.Show(sCommand);
if (sCommand.Length > 0) {
sCommand = Util.ExpandCommandLine(sCommand, sSource, sTarget);
// Dialog.Show("show", sCommand);
App.Frame.AddMessage("Converting");
if (File.Exists(sTarget)) File.Delete(sTarget);
Util.RunHideWait(sCommand);
if (!File.Exists(sTarget)) {
sCommand = "cmd.exe /c " + sCommand;
Util.RunHideWait(sCommand);
}
sText = "";
if (File.Exists(sTarget)) sText = Util.File2String(sTarget);
if (sText.Length == 0) {
if (File.Exists(sTarget)) File.Delete(sTarget);
Dialog.Show("Error", "Command line:\n" + sCommand);
}
}
else {
COM.WordSource2TargetFormat(sSource, sTarget, sTargetFormat);
}
//Dialog.Show("Error", "Command line:\n" + sCommand);
App.Frame.Activate();
return File.Exists(sTarget);
} // ConvertString2FileFormat method

public static string Literalize(string sText) {
bool bCheckPrefix = false;
return Literalize(sText, bCheckPrefix);
} // Literalize method

public static string Literalize(string sText, bool bCheckPrefix) {
if (bCheckPrefix) {
if (sText.StartsWith("@")) return sText.Substring(1);
else if (sText.StartsWith(@"\@")) sText = sText.Substring(1);
}
string sReturn = null;
try {
sReturn = (string) JS.Eval("\"" + sText + "\"", new object[] {});
}
catch {}

//string sReturn = JS.Eval("\"" + sText + "\"").ToString();
//if (sReturn.Length == 0) sReturn = sText;
if (sReturn == null) sReturn = sText;
return sReturn;
} // Literalize method

public static string Reverse(string sText) {
/*
string[] a = sText.Split();
Array.Reverse(a);
sText = String.Join("", a);
*/
int iLength = sText.Length;
StringBuilder sb = new StringBuilder(iLength);
for (int i = iLength - 1; i >= 0; i--) sb.Append(sText.Substring(i, 1));
sText = sb.ToString();
return sText;
} // Reverse method

public static int Absolute(int i) {
if (i < 0) i = -1 * i;
return i;
} // Absolute method

public static bool IsNumeric(string sText) {
return Microsoft.VisualBasic.Information.IsNumeric(sText);
} // IsNumeric method

public static bool IsDate(string sText) {
return Microsoft.VisualBasic.Information.IsDate(sText);
} // IsDate method

public static bool IsNothing(string sText) {
return Microsoft.VisualBasic.Information.IsNothing(sText);
} // IsNothing method

public static string Left(string sText, int iChars) {
return Microsoft.VisualBasic.Strings.Left(sText, iChars);
} // Left method

public static string Right(string sText, int iChars) {
return Microsoft.VisualBasic.Strings.Right(sText, iChars);
} // Right method

public static Font SetBold(Font font, bool bState) {
return Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeBold(font, bState);
} // SetBold method

public static Font SetItalic(Font font, bool bState) {
return Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeItalic(font, bState);
} // SetItalic method

public static Font SetUnderline(Font font, bool bState) {
return Microsoft.VisualBasic.Compatibility.VB6.Support.FontChangeUnderline(font, bState);
} // SetUnderline method

public static string GetFileFromUri(string sUri) {
string sFile;
Uri oUri = new Uri(sUri);
//if (oUri.IsFile) {
sFile = oUri.LocalPath;
try {
sFile = Path.GetFileName(sFile);
}
catch {
sFile = "";
}
//else {
if (sFile.Length == 0) {
sFile = oUri.PathAndQuery;
sFile = Uri.UnescapeDataString(sFile);
StringBuilder sb = new StringBuilder();
for (int i = 0; i < sFile.Length; i++) {
if (Char.IsLetterOrDigit(sFile, i)) sb.Append(sFile.Substring(i, 1));
else sb.Append("_");
}
sFile = sb.ToString();
sFile = Util.RegExpReplaceCase(sFile, @"_+", "_");
sFile = sFile.Trim(new Char[] {'_', ' '});
if (sFile.Length == 0) sFile = "page";
if (!sFile.ToLower().EndsWith(".htm") && !sFile.ToLower().EndsWith(".html")) sFile += ".htm";
}
if (Path.GetExtension(sFile).Length == 0) sFile += ".htm";
return sFile;
} // GetFileFromUri method

public static string GetUniqueName(string sSource) {
if (!Directory.Exists(sSource) && !File.Exists(sSource)) return sSource;
string sTarget = "";
string sDir = Path.GetDirectoryName(sSource);
string sRoot = Path.GetFileNameWithoutExtension(sSource);
sRoot = Regex.Replace(sRoot, @"_\d\d$", "");
//Regex rx = new Regex(@"_\d\d$");
//sRoot = rx.Replace(sRoot, "");
string sExt = Path.GetExtension(sSource);
//for (int i = 1; i < 100; i++) {
for (int i = 1; i < 10000; i++) {
//string sNewName = sRoot + "_" + i.ToString().PadLeft(2, '0') + sExt;
string sNewName = sRoot + "_" + i.ToString().PadLeft(4, '0') + sExt;
sTarget = Path.Combine(sDir, sNewName);
if (!Directory.Exists(sTarget) && !File.Exists(sTarget)) break;
}
//if (Directory.Exists(sTarget) || File.Exists(sTarget)) sTarget = "";
return sTarget;
} // GetUniqueName method

public static void Swap(ref int i1, ref int i2) {
int i  = i1;
i1 = i2;
i2 = i;
} // Swap method

public static char Code2Char(int iCode) {
return (char) iCode;
} // Code2Char method

public static string Code2String(int iCode) {
return Code2Char(iCode).ToString();
} // Code2String method

} // Util class

public class HomerList : List<string> {

public char Delimiter = '|';
public bool CaseSensitive = false;

public int Max {
get {
return this.Count - 1;
}
}

public string Segments {
get {
string[] aSegments = this.ToArray();
string sSegments = String.Join(this.Delimiter.ToString(), aSegments);
return sSegments;
}
set {
string[] aSegments = value.Split(this.Delimiter);
this.Clear();
if (value.Length > 0) this.AddRange(aSegments);
}
} // Segments property

public HomerList() {
//new HomerList(this.Segments, this.Delimiter, this.CaseSensitive);
} // HomerList constructor

public HomerList(string sSegments) {
//new HomerList(sSegments, this.Delimiter, this.CaseSensitive);
this.Segments = sSegments;
//new HomerList();
} // HomerList constructor

public HomerList(string sSegments, char cDelimiter) {
this.Delimiter = cDelimiter;
this.Segments = sSegments;
} // HomerList constructor

public HomerList(string sSegments, char cDelimiter, bool bCaseSensitive) {
this.Delimiter = cDelimiter;
this.Segments = sSegments;
this.CaseSensitive = bCaseSensitive;
} // HomerList constructor

public HomerList(string[] aItems) {
this.AddRange(aItems);
} // HomerList constructor

public new int IndexOf(string sItem) {
if (this.CaseSensitive) return base.IndexOf(sItem);
else {
int iIndex = -1;
string sValue = sItem.ToLower();
for (int i = 0; i < this.Count; i++) {
if (this[i].ToLower() == sValue) {
iIndex = i;
break;
}
}
return iIndex;
}
} // IndexOf method

public new bool Contains(string sItem) {
return this.IndexOf(sItem) >= 0;
} // Contains method

public new void Sort() {
if (this.CaseSensitive) base.Sort();
else {
string[] a = this.ToArray();
Array.Sort(a, new CaseInsensitiveComparer());
this.Clear();
this.AddRange(a);
}
} // Sort method

public string GetSegments(char cDelimiter) {
this.Delimiter = cDelimiter;
return this.Segments;
} // GetSegments method

public void KeepUnique() {
for (int i = this.Count - 1; i >=0; i--) {
string s = this[i];
if (this.IndexOf(s) < i) this.RemoveAt(i);
}
} // KeepUnique method

public void RemoveLike(string sMatch) {
RegexOptions options = RegexOptions.Multiline;
if (!this.CaseSensitive) options |= RegexOptions.IgnoreCase;
Regex rx = new Regex(sMatch, options);

for (int i = this.Count - 1; i >= 0; i--) {
if (rx.IsMatch(this[i])) this.RemoveAt(i);
}
} // RemoveLike method

public void KeepLike(string sMatch) {
HomerList hl = this.FindLike(sMatch);
this.Clear();
this.AddRange(hl);
} // KeepLike method

public HomerList FindLike(string sMatch) {
RegexOptions options = RegexOptions.Multiline;
if (!this.CaseSensitive) options |= RegexOptions.IgnoreCase;
Regex rx = new Regex(sMatch, options);

HomerList hl = new HomerList();
foreach (string s in this) {
if (rx.IsMatch(s)) hl.Add(s);
}
return hl;
} // FindLike method

public void ReplaceLike(string sMatch, string sReplace) {
RegexOptions options = RegexOptions.Multiline;
if (!this.CaseSensitive) options |= RegexOptions.IgnoreCase;
Regex rx = new Regex(sMatch, options);

for (int i = 0; i < this.Count; i++)  this[i] = rx.Replace(this[i], sReplace);
} // ReplaceLike method

public void Push(string sItem) {
this.Insert(0, sItem);
} // Push method

public string Pop() {
int iUpper = this.Count - 1;
string sItem = this[iUpper];
this.RemoveAt(iUpper);
return sItem;
} // Pop method

public string Shift() {
int iLower = 0;
string sItem = this[iLower];
this.RemoveAt(iLower);
return sItem;
} // Shift method

public new void Remove(string sItem) {
bool bLoop = true;
while (bLoop) {
int iIndex = this.IndexOf(sItem);
if (iIndex == -1) break;
this.RemoveAt(iIndex);
}
} // Remove method

public void RemoveRange(HomerList hl) {
foreach (string sItem in hl) this.Remove(sItem);
} // RemoveRange method

public void RemoveRange(string[] aItems) {
HomerList hl = new HomerList(aItems);
this.RemoveRange(hl);
} // RemoveRange method

public void AddUnique(string sItem) {
if (!this.Contains(sItem)) this.Add(sItem);
} // AddUnique method

public void PushUnique(string sItem) {
if (!this.Contains(sItem)) this.Push(sItem);
} // PushUnique method

public void RemoveRange(string sSegments) {
Char cDelimiter = '|';
HomerList hl = new HomerList(sSegments, cDelimiter);
this.RemoveRange(hl);
} // RemoveRange method

public void RemoveRange(string sSegments, Char cDelimiter) {
HomerList hl = new HomerList(sSegments, cDelimiter);
this.RemoveRange(hl);
} // RemoveRange method

public void saveAddRange(HomerList hl) {
this.AddRange(hl);
} // AddRange method

public void OldAddRange(string[] aItems) {
HomerList hl = new HomerList(aItems);
this.AddRange(hl);
} // AddRange method

public void AddRange(string sSegments) {
Char cDelimiter = this.Delimiter;
HomerList hl = new HomerList(sSegments, cDelimiter);
this.AddRange(hl);
} // AddRange method

public void AddRange(string sSegments, Char cDelimiter) {
HomerList hl = new HomerList(sSegments, cDelimiter);
this.AddRange(hl);
} // AddRange method

public void AddUniqueRange(HomerList hl) {
foreach (string s in hl) if (!this.Contains(s)) this.Add(s);
} // AddUniqueRange method

public void AddUniqueRange(string[] aItems) {
HomerList hl = new HomerList(aItems);
this.AddUniqueRange(hl);
} // AddUniqueRange method

public void AddUniqueRange(string sSegments) {
Char cDelimiter = this.Delimiter;
HomerList hl = new HomerList(sSegments, cDelimiter);
this.AddUniqueRange(hl);
} // AddUniqueRange method

public void AddUniqueRange(string sSegments, Char cDelimiter) {
HomerList hl = new HomerList(sSegments, cDelimiter);
this.AddUniqueRange(hl);
} // AddUniqueRange method

public HomerList FindRange(HomerList hl) {
HomerList hlReturn = new HomerList();
foreach (string sItem in hl) if (this.Contains(sItem)) hlReturn.Add(sItem);
return hlReturn;
} // FindRange method

public void FindRange(string[] aItems) {
HomerList hl = new HomerList(aItems);
this.FindRange(hl);
} // FindRange method

public void FindRange(string sSegments) {
Char cDelimiter = '|';
HomerList hl = new HomerList(sSegments, cDelimiter);
this.FindRange(hl);
} // FindRange method

public void FindRange(string sSegments, Char cDelimiter) {
HomerList hl = new HomerList(sSegments, cDelimiter);
this.FindRange(hl);
} // FindRange method

public HomerList Clone() {
string[] aItems = this.ToArray();
HomerList hl = new HomerList(aItems);
return hl;
} // Clone method

public string MinValue() {
if (this.Count == 0) return "";

HomerList hl = this.Clone();
hl.Sort();
return hl[0];
} // MinValue method

public string MaxValue() {
if (this.Count == 0) return "";

HomerList hl = this.Clone();
hl.Sort();
return hl[hl.Count - 1];
} // MaxValue method

public int MinLength() {
int iLength = 2000000000;
foreach (string sItem in this) if (sItem.Length < iLength) iLength = sItem.Length;
if (iLength == 2000000000) iLength = 0;
return iLength;
} // MinLength method

public int MaxLength() {
int iLength = 0;
foreach (string sItem in this) if (sItem.Length > iLength) iLength = sItem.Length;
return iLength;
} // MaxLength method

public void SortLength() {
this.Sort(delegate(string s1, string s2) {
return s1.Length.CompareTo(s2.Length);
} );
} // SortLength method

public void ToLower() {
for (int i = 0; i < this.Count; i++) this[i] = this[i].ToLower();
} // ToLower method

public void ToUpper() {
for (int i = 0; i < this.Count; i++) this[i] = this[i].ToUpper();
} // ToUpper method

public void TrimStart() {
for (int i = 0; i < this.Count; i++) this[i] = this[i].TrimStart();
} // TrimStart method

public void TrimEnd() {
for (int i = 0; i < this.Count; i++) this[i] = this[i].TrimEnd();
} // TrimEnd method

public void TrimStart(char[] a) {
for (int i = 0; i < this.Count; i++) this[i] = this[i].TrimStart(a);
} // TrimStart method

public void TrimEnd(char[] a) {
for (int i = 0; i < this.Count; i++) this[i] = this[i].TrimEnd(a);
} // TrimEnd method

public void PadLeft(int iLength, char c) {
for (int i = 0; i < this.Count; i++) this[i] = this[i].PadLeft(iLength, c);
} // PadLeft method

public void PadRight(int iLength, char c) {
for (int i = 0; i < this.Count; i++) this[i] = this[i].PadRight(iLength, c);
} // PadRight method

public void PushRange(HomerList hl) {
this.InsertRange(0, hl);
} // PushRange method

public void PushRange(string sSegments) {
Char cDelimiter = this.Delimiter;
HomerList hl = new HomerList(sSegments, cDelimiter);
this.PushRange(hl);
} // PushRange method

public void PushRange(string sSegments, Char cDelimiter) {
HomerList hl = new HomerList(sSegments, cDelimiter);
this.PushRange(hl);
} // PushRange method

public void PushUniqueRange(HomerList hl) {
foreach (string s in hl) if (!this.Contains(s)) this.Push(s);
} // PushUniqueRange method

public void PushUniqueRange(string[] aItems) {
HomerList hl = new HomerList(aItems);
this.PushUniqueRange(hl);
} // PushUniqueRange method

public void PushUniqueRange(string sSegments) {
Char cDelimiter = this.Delimiter;
HomerList hl = new HomerList(sSegments, cDelimiter);
this.PushUniqueRange(hl);
} // PushUniqueRange method

public void PushUniqueRange(string sSegments, Char cDelimiter) {
HomerList hl = new HomerList(sSegments, cDelimiter);
this.PushUniqueRange(hl);
} // PushUniqueRange method

} // HomerList class

public class Segment {
public static bool CaseSensitive = false;
public static char Delimiter = '|';

public int Count(string sSegments) {
string[] aSegments = sSegments.Split(Segment.Delimiter);
return sSegments.Length == 0 ? 0 : aSegments.Length;
} // Count method

public static string Get(string sSegments, int iIndex) {
string[] aSegments = sSegments.Split(Segment.Delimiter);
return aSegments[iIndex];
} // Get method

public static int IndexOf(string sSegments, string sSegment) {
string[] aSegments = sSegments.Split(Segment.Delimiter);
List<string> listSegments = new List<string>(aSegments);
if (!Segment.CaseSensitive) for (int i = 0; i < listSegments.Count; i++) listSegments[i] = listSegments[i].ToLower();
return listSegments.IndexOf(sSegment);
} // IndexOf method

public static bool Contains(string sSegments, string sSegment) {
return IndexOf(sSegments, sSegment) >= 0;
} // Contains method

public static string RemoveAt(string sSegments, int iIndex) {
string[] aSegments = sSegments.Split(Segment.Delimiter);
List<string> listSegments = new List<string>(aSegments);
listSegments.RemoveAt(iIndex);
aSegments = listSegments.ToArray();
return String.Join(Segment.Delimiter.ToString(), aSegments);
} // RemoveAt method

public static string Remove(string sSegments, string sSegment) {
int iIndex = IndexOf(sSegments, sSegment);
return RemoveAt(sSegments, iIndex);
} // Remove method

public static string RemoveIfContains(string sSegments, string sSegment) {
int iIndex = IndexOf(sSegments, sSegment);
if (iIndex >= 0) sSegments = RemoveAt(sSegments, iIndex);
return sSegments;
} // RemoveIfContains method

public static string Insert(string sSegments, int iIndex, string sSegment) {
string[] aSegments = sSegments.Split(Segment.Delimiter);
List<string> listSegments = new List<string>(aSegments);
listSegments.Insert(iIndex, sSegment);
aSegments = listSegments.ToArray();
return String.Join(Segment.Delimiter.ToString(), aSegments);
} // Insert method

public static string Add(string sSegments, string sSegment) {
string[] aSegments = sSegments.Split(Segment.Delimiter);
List<string> listSegments = new List<string>(aSegments);
listSegments.Add(sSegment);
aSegments = listSegments.ToArray();
return String.Join(Segment.Delimiter.ToString(), aSegments);
} // Add method

public static string AddIfUnique(string sSegments, string sSegment) {
if (!Contains(sSegments, sSegment)) sSegments = Add(sSegments, sSegment);
return sSegments;
} // AddIfUnique method

public static string ReplaceAt(string sSegments, int iIndex, string sSegment) {
string[] aSegments = sSegments.Split(Segment.Delimiter);
List<string> listSegments = new List<string>(aSegments);
listSegments[iIndex] = sSegment;
aSegments = listSegments.ToArray();
return String.Join(Segment.Delimiter.ToString(), aSegments);
} // ReplaceAt method

public static string Replace(string sSegments, string sSegment) {
int iIndex = IndexOf(sSegments, sSegment);
return ReplaceAt(sSegments, iIndex, sSegment);
} // Replace method

public static string ReplaceIfContains(string sSegments, string sSegment) {
int iIndex = IndexOf(sSegments, sSegment);
if (iIndex >= 0) sSegments = ReplaceAt(sSegments, iIndex, sSegment);
return sSegments;
} // ReplaceIfContains method

public static string Sort(string sSegments) {
string[] aSegments = sSegments.Split(Segment.Delimiter);
List<string> listSegments = new List<string>(aSegments);
if (!Segment.CaseSensitive) for (int i = 0; i < listSegments.Count; i++) listSegments[i] = listSegments[i].ToLower();
string[] aKeys = listSegments.ToArray();
Array.Sort(aKeys, aSegments);
return String.Join(Segment.Delimiter.ToString(), aSegments);
} // Sort method

public static string Unique(string sSegments) {
string[] aSegments = sSegments.Split(Segment.Delimiter);
List<string> listSegments = new List<string>();
if (Segment.CaseSensitive) foreach (string sSegment in aSegments) if (!listSegments.Contains(sSegment)) listSegments.Add(sSegment);
else {
List<string> listLower = new List<string>();
foreach (string s in aSegments) {
if (!listLower.Contains(s.ToLower())) {
listSegments.Add(s);
listLower.Add(s.ToLower());
}
}
}

aSegments = listSegments.ToArray();
return String.Join(Segment.Delimiter.ToString(), aSegments);
} // Unique method

} // Segment class

} // EdSharp namespace
