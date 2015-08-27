/*
HTM2TXT
Version 1.0
Copyright 2011 by Jamal Mazrui
GNU Lesser General Public License (LGPL)
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

class App {
static void Main(string[] aArgs) {
if (aArgs.Length == 0) return;

string sHtm = aArgs[0];
sHtm = Path.GetFullPath(sHtm);
string sTxt = Path.ChangeExtension(sHtm, "txt");
if (aArgs.Length > 1) sTxt = aArgs[1];
sTxt = Path.GetFullPath(sTxt);

string sBody = @"[Config]
OpenInNotepad=0
CharsPerLine=9999
Source=c:\temp\test.htm
Dest=c:\temp\test.txt
SkipTitleText=0
AddLineUnderHeader=1
SkipTableHeaderText=0
TableCellDelimit=1
HeadingLineChars=
HorRuleChar=-
ListChars=*****
ConvertMode=1
AllowCenterText=0
AllowRightText=0
DLSpc=4
LinksDisplayFormat=%T (%L)
EncloseBoldCharsStart=<<
EncloseBoldCharsEnd=>>
EncloseBold=0
SubFolders=0
";

sBody = sBody.Replace(@"c:\temp\test.htm", sHtm);
sBody = sBody.Replace(@"c:\temp\test.txt", sTxt);

string sDir = Application.StartupPath;
string sExe = Path.Combine(sDir, "HTMLAsText.exe");
string sCfg = Path.GetTempFileName();
sCfg = @"c:\temp\temp.txt";
File.WriteAllText(sCfg, sBody);

string sParam = "/run " + sCfg;
// MessageBox.Show(sHtm, sTxt);
Process process = Process.Start(sExe, sParam);
; while (!process.HasExited) Thread.Sleep(2000);
process.WaitForExit();
File.Delete(sCfg);


} // Main method
} // App class
