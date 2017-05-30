var rtb = App.Frame.Child.RTB
var s = rtb.SelectedText.Trim()
s = s.Replace("\n", "</li>\n<li>")
s = "<ul>\n<li>" + s + "/li>\n</ul>\n"
rtb.SelectedText = s
rtb.Index = rtb.SelectionStart + rtb.SelectionLength
