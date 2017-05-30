var iCount = Interaction.InputBox("Number of Items:", "Input", "0");
var sTag = "<ul>\n";
var i = 1;
while (i <= iCount) {
sTag += "<li>Item" + i + "</li>\n";
i++;
}
sTag += "</ul>\n";
