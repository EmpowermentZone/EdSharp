using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

class Program {
static void Main(string[] aArgs) {
int iLength = aArgs.Length;
if (iLength == 0) {
Console.WriteLine("Pass the file to reformat as a parameter.");
Application.Exit();
}

string sFile = aArgs[0];
if (!File.Exists(sFile)) {
Console.WriteLine("File " + sFile + " not found!");
Application.Exit();
}

string sBody = File.ReadAllText(sFile);

/*
string sCodes = "";
foreach (char c in sBody) sCodes += ((int) c).ToString() + "\r\n";
sCodes += "\r\n";
File.WriteAllText("temp.txt", sCodes);
*/

// sBody = Regex.Replace(sBody, @"([^.!?])  ", "$1\r\n");
sBody = sBody.Replace("\r\r\n\r\r\n", "\r\n");
sBody = Regex.Replace(sBody, "  *\"\"\"\"*  *", " ... ");
File.WriteAllText(sFile, sBody);
Console.WriteLine("Done!");
} // Main method
} // Program class
