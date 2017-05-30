/*
May 6, 2017
Copyright 2006-2017 by Jamal Mazrui
GNU Lesser General Public License (LGPL)
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
