var frame = App.Frame
var rtb = frame.Child.RTB

frame.AddMessage("ASCII Only")
if (rtb.SelectionLength == 0) {
frame.AddMessage("All")
rtb.Text = Util.Convert2Ascii(rtb.Text)
}
else {
frame.AddMessage("Selected")
rtb.SelectedText = Util.Convert2Ascii(rtb.SelectedText)
}
frame.AddMessage("Done!")
