// C#
// Compile on the command line as follows: csc.exe hello.cs

using System;
using System.Reflection;
using System.Runtime.InteropServices;

class Test {
static void Main() {
Type t = Type.GetTypeFromProgID("Say.Tools");
object oST = Activator.CreateInstance(t);
string sText = "Hello world";
object[] aParams = {sText};
t.InvokeMember("Say", BindingFlags.InvokeMethod, null, oST, aParams);
}
}
