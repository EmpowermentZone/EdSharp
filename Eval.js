/*
Layout by Code for .NET development (JScript library)
Version 2.7
July 10, 2007
Copyright 2006-2007 by Jamal Mazrui
Modified GPL License
*/

import EdSharp;
import Accessibility;
import Microsoft.VisualBasic;
import System;
import System.Collections;
import System.ComponentModel;
import System.Data;
import System.Diagnostics;
import System.Drawing;
import System.IO;
import System.Reflection;
import System.Runtime.InteropServices;
import System.Text;
import System.Text.RegularExpressions;
import System.Web;
import System.Windows.Forms;
import System.Xml;

public class JS {
static function Eval(sCode : String) {
//var oParams = [];
//return Eval(sCode, oParams);
return Eval(sCode, []);
} // Eval method

static function Eval(sCode : String , oParams : Object[]) {
try {
var o = eval(sCode, "unsafe");
return o;
}
catch (e) {
//MessageBox.Show("Error!", e.Message);
return null;
}
} // Eval method

} // JS class
