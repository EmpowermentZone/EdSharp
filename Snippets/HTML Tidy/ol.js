// import System;
// import Microsoft.VisualBasic;

var sTag;
var iCount = Interaction.InputBox("Number of Items:", "Input", "3");

if (iCount > 0) { 
  sTag = makeTag("<ol>\n");
} else {
}

function makeTag( sTag )

{
var i = 1;
while (i <= iCount) {
  sTag += "<li>Item" + i + "</li>\n";
  i++;
} // end while
sTag += "</ol>\n";
return sTag;

} // end makeTag