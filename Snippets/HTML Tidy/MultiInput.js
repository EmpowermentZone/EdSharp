var aLabels = ["Label1", "Label2", "Label3"]
var aValues = ["Value1", "Value2", "Value3"]
var aResults = Dialog.MultiInput("3 Fields", aLabels, aValues)
//Var sResults = aResults[0] + '\n' + aResults[1] + '\n' + aResults[2]
var sResults = aResults[0] + "\n" + aResults[1] + "\n" + aResults[2]
Dialog.Show("Results", sResults)
