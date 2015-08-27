Welcome to nfbtrans 7.74 November 4, 2002.

     Introduction.  NFBTRANS is a Grade Two braille translator.  It is freeware
and comes with complete C sourse and documentation.  The program converts ascii
text into braille and sends the result to a file or an embossor.  Special
formatting commands may be placed in the text to customize the output.	No
knowledge of braille is required to use this program,  however you must be able
to edit ascii files and be familiar with MSDOS commands.  This archive contains
the DOS executable version of NFBTRANS.  The program has been successfully
compiled to run in unix.  NFBTRANS is suitable for brailling menus, letters, and
manuals.  Many users have found that NFBTRANS serves their translation needs
without having to purchase expensivbe commercial translators.

     Features:	NFBTRANS is a very accurate Grade Two braille translator.
It can also back translate a Grade Two file into normal text.  The program
has many options which allow the user to customize its operation.  Formatting
commands can be used to generate Tables of Contents, ink print page numbers,
running headers and much more.	Translation rules are in a text file and
can easily be modified by the user.  The program can be configured to
hyphenate words to save space.

     Limitations: NFBTRANS can only translate from ascii text.	It cannot
convert binary files such as those produced by Microsoft Word unless they
are first converted to ascii text.  The text may have lines of unlimited
length and they may contain extended graphics characters.  Nfbtrans only
has a partial implementation of Grade 3 braille.

files included:
back.tab: Table to translate a Grade 2 file back to text.
vcomp.c: C source for braille compare utility.
bcomp.exe DOS executable for braille compare utility.
braille.tab: English braille translation table.
english.dic: hyphenation dictionary.
install.bat: Install batch file for nfbtrans.
makedoc: script file for unix users to convert nfbtrans.fmt to nfbtrans.txt
     which can be printed.
makedoc.bat: batch file to convert nfbtrans.fmt to a print manual.
Makefile: make file for unix.
menu.cnf: sample menu file.
nfbasm.asm: sound routines for MicroSoft C.
nfbtrans: make file for MicroSoft C.
nfbtrans.c: source for nfbtrans.
nfbtrans.cnf: sample configuration file.
nfbtrans.exe: executable DOS version of nfbtrans.
nfbtrans.fmt: documentation for nfbtrans formatted for brailling with nfbtrans.
nfbpatch.c: routines required to run nfbtrans in unix.
readme.txt: this file.
spanish.zip spanish tables
spell.dat: spelling dictionary file.
tables.zip: foreign language tables for nfbtrans.
tvfreqs.fmt example of a braille table.

My purpose in releasing this version of nfbtrans is to make a high quality
braille translation program available to anyone who has a use for it.
It is very usable for most situations and will at least give you a
feel for braille translation.

Installing nfbtrans for MSDOS:
1.  Unzip this archive into a scratch directory.
2.  Change to that directory.
3.  Run the install program using the command
   INSTALL drive:\nfbtrans_directory.
4.  Examine the entries in nfbtrans.cnf and
    make changes if you wish.  See nfbtrans.txt for a complete
    description of options.  Include this directory in your path if you want to
    run nfbtrans from anywhere on your disk.

Installing nfbtrans for Unix:
     Unzip nfbtrans into an empty directory.  From the unix shell type
mv MAKEFILE Makefile
Then enter
make lowercase

     This is necessary because the program files were zipped in MSDOS and the
file names are in upper case when unzipped in unix.  Files specifically for
the MSDOS version are also removed at this time.

     Compile nfbtrans by entering make target where target is ultrix, linux,
sunos, djgpp, or aix.  You will have to add the target for your machine to
makefile if it is not one of these. The termcap entry in
the LIB= statement may need to be removed in linux.  When the compile is
successful, copy nfbtrans to usually /usr/local/bin and set permissions
usually chmod 755 nfbtrans.  Copy nfbtrans.cnf, back.tab, english.dic, spell.dat
and braille.tab to /usr/local/lib.  Edit nfbtrans.cnf and change options if
necessary.

Quick Start:
Always use the parallel port to connect with your embossor if possible.
Otherwise use the mode command to redirect lpt1 to whatever com port
you are using.
Examine nfbtrans.cnf paying special attention to the entries pw=, pl= and sp=.
Make sure your printer is configured to output computer braille, skip perfs,
and that it supports the page width and length given in nfbtrans.cnf.
Make sure the DOS print command is resident if you set sp=1.

Nfbtrans can be run in several ways.
The syntax is:
NFBTRANS [option1] [option2] [...] [file1] [file2] [...].
Options are of the form xx=string.  They are fully described in nfbtrans.txt.
There are over 100 options many of which you will never need to use.  They
may be given on the command line, in nfbtrans.cnf, in the translation table,
or in the document to be embossed. The program can also be run with no command
line arguments. The user will be prompted for required information depending
on the options in nfbtrans.cnf.

1.  To emboss a file, enter nfbtrans with no arguments and answer the questions.
2.  Enter nfbtrans pw=42 pl=27 myfile.txt to emboss myfile.txt page width of
42 and page length of 27.
3.  Enter nfbtrans file1.txt file2.txt to emboss file1.txt and file2.txt.
4.  Enter nfbtrans file1.txt file2.txt >outfile to translate file1.txt and
     file2.txt to outfile.
5.  Enter nfbtrans <file.txt to emboss file.txt.  Output goes to stdout in unix.
6.  Enter nfbtrans <file.txt >outfile.txt to translate file.txt to outfile.txt.
7.  Enter type infile.txt | nfbtrans to emboss infile.txt.
8.  Enter nfbtrans d:\progs\*.c c:\pascal\*.pas to emboss the .c and .pas
     files in the given directories.
9. Enter nfbtrans @listfile c:\temp\*.h to emboss the files contained in
listfile and then the .h files in c:\temp.

The file to be translated is assumed to be an ascii text file.	Lines may
be of any length.  Characters from decimal 128-255 are
processed according to the gm= option. gm=0 causes the high bit to be removed,
gm=1 causes the character to be ignored, and gm=2 leaves the character
unmodified.  This character will be translated according to the rules in
braille.tab.  Graphics characters are used primarily with other languages.

For properly formatted braille you must use the nfbtrans formatting commands.
Formatting commands are indicated by a tilde preceeding the actual formatting
command.  For example (tilde)cChapter One
willl center the line Chapter One.
If you don't care if headings are centered or if columns of tables are
aligned properly then don't worry about formatting commands.  Nfbtrans
can determine the start of a paragraph.  If paragraphs are preceeded by a
blank line use block paragraph mode (tilde)5.  If paragraphs are indented use
(tilde)t. Use (tilde)6 if you want nfbtrans to scan the file before
translating to determine how paragraphs are formatted. Nfbtrans can be set
up to rejoin hyphenated words in the file.  You may also use the optional
hyphenation dictionary english.dic to automatically hyphenate words.

Nfbtrans translates files differently depending on their extension.  This is
determined by the ex= and i0= through i9= options in nfbtrans.cnf.
Note that if input is redirected as in nfbtrans <infile.c, the program never
sees the name infile.c.  The input file is set to stdin if input is
redirected which means that infile.c will not be treated as a .c file.
Use the si= option if you want to name it something else.
By default nfbtrans translates files into grade two braille with the
nfbtrans formatting commands enabled.
Files with .arc, .arj, .com, .exe, .obj, .qwk, .zip and .zoo are considered
binary files and are not translated.
Files with .asm, .bas, .bat, .c, .cpp, .h, .mac, and .pas are considered
program files and are output in non-standard computer braille.
.brl files are output in grade two with formatting enabled.  .prt files are
output in 80 column format standard computer braille.  .man files
are considered unix man pages and are output in non-standard computer braille and
block paragraphs.
.fmt files are output in Grade Two with formatting enabled and no file and
date printed at the top of page one. Blank lines within two lines of the
bottom cause nfbtrans to go to the next page.
All other extensions are translated to Grade two with formatting enabled.

     Nfbtrans can easily be run in Windows even if you don't have a DOS
screenreader.  If you plan to use nfbtrans primarily for backtranslating .brf
files, do the following:
Associate the .brf extension with nfbtrans.  This is done using windows
Explorer, view, options, then file types. If .brf is not a registered file type
then click new, Braille Format File as description, content type Application,
check the show extension and confirm download boxes. For action the command line
should be something like this:
c:\command.com /c c:\tools\nfbtrans.exe tm=13 on=2 od=c:\mybooks
and the action can just be Open. Then click close until you get out of explorer
so your changes are saved.  Now when you click on a .brf file in explorer or run
from the run dialog box, nfbtrans automatically backtranslates your .brf file
and returns to windows.  Use the od= option in nfbtrans.cnf to specify where
to store the output. The output file has the same name as the input file
but with a .txt extension. Review the od= and on= options in nfbtrans.fmt.
This also allows you to download .brf files from web braille.

Note that nfbtrans does not support long file names. If a long name is given
as an argument or in a prompt you will get a file not found error. If you click
a long name from windows explorer the program uses the equivilent 8 by 3
MSDOS file name.

History:
7.74:
Added ns= option.

7.73:
Minor back translation improvements, additions to braille.tab from NLS problem
     words in literary proofreading course. Fixed error embossing an already
     translated file.

7.72:
Corrected backtranslation error regarding braille numbers broken in 7.71.
Turn off computer braille for line if no _& or _:
Can specify bits to set in options i.e. sm=0,2,3,4,13

7.71:
More backtranslation improvements including correct translation of the
     computer indicator symbols.
Wildcard input files will produce separate output files if the on= option is used.

7.70:
More work on back translation. Added to braille.tab and back.tab.

7.69:
Added on= option.
More improvements in back translation.

7.68:
Corrected error in unix makefile.
More improvements to back translation.

7.67:
Added em= option.
Fixed error in kf= option.
Additions to braille.tab and back.tab.
More improvements in back translation.

7.66:
Added more entries to back.tab and improved back translation of numbers.

7.65:
Added type 13 to back translation table.
Improved handling of italics and $ in back translation.
Added two more bits to the sm= option.

7.64:
Fixed ts= option.
Added type 30 to braille.tab.
Added ve= option.
Added several entries to braille.tab.

7.63:
Improvements in back translation.

7.62:
Improvements in back translation.

7.61:
kf= option modified so won't cause errors with headings and footings.

7.60:
Changed kf= option for only back translation.

7.59:
Improved sc= option.
Added sd= and sn= option.

7.58:
Fixed problem with ob= and ls= option.

7.57:
Improved ~w footer command.
Improved handlihng of inner quotes.
Added bp= option for pagebreaks with n lines for textbook pages.
Added sc= spellcheck option.

7.56:
Modified hc= option so calls not translated can be in any case.
Fixed improper handling of files named bat c pas ... ...
Remove letter sign translating numbers ending in 0th
Improved back translation especially concerning dashes.

7.55:
fixed ~d command.
Added indent option to ~s command.

7.54:
Fixed problem with dv= option.
Improved back translation table, does letter signs properly.
Should backtranslate lowercase output from Blazie notetakers.

7.53:
Fixed Y2K problem with hyphenation dictionary.
Improvements to english.dic and braille.tab.
Added ~\k index command.
Fixed ~' and ~} commands.
Added lowercase target in unix make file to convert files to lower case.
Added support for roman page numbers in automatic TOC and index.
Improved ~n and ~r formatting commands.
Corrected unwanted output with ~h command.
Improved lt= option and statistics file.
Added word to EFL commands.
Output roman page numbers with header properly.
Corrected problem where two TOC entries were put on a single line.
Improved statistics file.
Updated back.tab to accomidate Blazie notetakers.

7.52:
Fix problem with pd= and statistics file from Y2K.
Removed debugging message from braille.tab and back.tab.

7.51:
Fixed problem with ~h and line paragraph mode.
Added uk= option for proper dash formatting with British braille.
Added ~# comment in source command.
Added nmatch command and fixed other problems with EFL code.
Added bl=, od= and pm= option.
Added index feature with the ~\i and ~\j formatting commands.
Fixed problem of cut off words when using textbook pages.
Added ~! formatting command.
Allow graphics characters in option strings.

7.50:
Fixed bug evaluating options with two integer arguments broken in 7.49.
Improved operation of rp= and rw= options.
Improved back translation.
Added l3= option for spanish Grade two braille.

7.49:
Added ac=, lb=, of=, m3=, mf= and sl= option.
Added type 17 two-word match and type 18 3-word match for french language.
Added ~7 line paragraph mode.
Added more foreign language tablees.
Added top margin to ~m command.
Corrected problem translating numbers such as 21st 22nd 23rd and 24th.
Improved Grade 3 output.
Allow up to five line headers.

7.48:
Removed hv= option and made improvements in operation of hyphenation
     dictionary, especially for unix.
Allow for mixed case file names in unix.
Added dv= and rf= option.
Fixed problem with ~h command during interpoint embossing and verify headings
     fit on one line.
Improved operation of ht= option.
Added entries to braille.tab and statistics file.
Added ~] and ~{ command.

7.47:
Fixed rare stack overflow problem when using a small page width.
Fixed tables.zip.
Allow up to 2000 entries in translation table and no limit on match and replace
strings.
Added lt= option.

7.46:
Fixed several ~f+ problems.
Fixed problem where #. was placed at the end of a word ending with a period.
Fixed conditional page break command which didn't always work.
Fixed problem redirecting input when command line options were given.

7.45:
Fixed various unix problems including incorrect display of error messages.
Fixed rejoining of hyphenated words in computer braille.
Allow user to specify only Roman pages in the page range to emboss.

7.44:
Added new feature to TOC ~f command.
Added te= and to= options.
Corrected error in back.tab.
Added linux target for unix.

7.43:
Corrected problems translating mixed numbers and letters.
More improvements in back.tab.
Modified the tf= option so the back translation table can also be specified.
Corrected problem left justifying columns with the td= option.
Added kf= option for formatting back translation output.

7.42:
Added rc=, lp= rp= and tp= option.
Simplified operation of the hm= and rw= options.
removed he= option.
Improved back translation.

7.41:
Fixed problem of not joining words with, of, and for broken in 7.35.
Fixed problem where hyphenated words ending in punctuation were split and the
     punctuation was dropped.
Output the file name as well as line number when a character isn't in the
     table.
Do validity check on options such as page width must be less than 255.
Added hp= and ob= option.
Fixed bm=15 option which occasionally crashed.
Added ts= option to output table stats.
Allow any character including space to be in replace string of table.
Added back.tab and modified code to back translate a Grade 2 file.

7.40:
Added hb= hl= hn= and hx= options.
Added bit six to the hm= option to automate adding words to dictionary.
Removed debugging statement accidentally left in the ~h command.

7.39:
Fixed bug in the ~i\ italics command deleting first character of word if
     first word of file.
     Fixed problems with reps* and repw* EFL commands.
Modified operation of oc= to control case of computer braille.
Changed operation of le= option to allow a global .efl file.
Fixed problems caused when input line length changed when using EFL.
Added option, line and graphics keywords to .efl language.
Added type 16 for German language.

7.38:
Added vc= option defining vowels and consonants for table types 13-15.
Fixed problem with trailing punctuation and the hm= option.
Allow the | no translate symbol in the hyphenation dictionary.
Fixed problem where a space was left after to or by before a split word.
Fixed program crash when there is only one entry in table for each letter.
Fixed output case to correspond with oc= option when the | no translate
     symbol is used.
Program automatically does consistency check whenever dictionary files are
     updated.
Changed the order in which nfbtrans looks for nfbtrans.cnf in DOX and UNIX
     to NFBTRANS environment variable, current directory, and location of
     program in DOS or /usr/local/lib in Unix.
Fixed errors in unix version preventing hyphenation dictionary from working.
Allow type 7 contractions like ation at end of hyphenated words.
Prevent type 2 contractions from being used in beginning of hyphenated words.

7.37:
Fixed problem where program could not find file with the hd= and ht= option.
Added ability to store hyphenated words in a file with the hm= option.

7.36:
Fixed problem of incorrect line numbers reported with the ht= option.
Report line numbers with the ht= option making it easier to add words.
Added another bit to the hm= option to output words which were not found.

7.35:
Fixed problems with grate 2 graphics character translation.
Allow graphics characters to be specified within brackets in braille.tab.
Fixed problem where vertical bar and grade two graphics caused the table to be
     modified.
Added bm=book_mode option.
Added tc= option for customized TOC header.
Added hyphenation dictionary options hd= and ht=.
Added hm= option to specify type of words to hyphenate.
Fixed ~p command broken in 7.34.

7.34:
Fixed problem with hyphen file while doing automatic TOC.
Remove space after dash if a hyphenated word is not rejoined. half-an- hour
     becomes half-an-hour.
Output vertical bar if math table is active.
Allow graphics characters to have types 1-11 in braille.tab.
Added type 12 and l2= option for Dutch braille.
Added dm=message to display a message.
Added oc= option to change case of the output.

7.33:
Added math options ma= ms= and mt=.
Added be= option to output a beep.
Added fc= option to change fill character in Table of Contents.
Added if= option to ignore certain format characters.

7.32:
Improved italics support.

7.31:
Added l0 and l1 options.
Improved italics support.
Added li= option to indicate end of input lines.

7.30:
Fixed bug where table entries for extended graphics characters gave an error.
Added tf= option to load another set of translation rules.
braille.tab can contain command line options.
Type 29 was added indicating upper case graphics characters.
eb= option was replaced with ca= for user definable capitalization marks.
Added it= option for user definable italics marks.
Added gm=3 to abort program if a graphics character is used but not in table.
Added tn= option for default translation mode.
Added le= option to load an External Format Language file.
Fixed problem of input lines not being counted properly during automatic
     paragraph formatting.
Modified rw= option so graphics characters can be considered letters.
Added (tilde)_ for hard carriage return.
Added reps* and repw* to replace multiple occurrences of strings or words.
Added several foreign language tables in tables.zip.

7.29:
Added eb= option for English braille, no capitalization marks.
Added options ia through ie so up to 15 extension types can be defined.
Require that nfbtrans be compiled in the large memory model.

7.28:
Abort if the last word of a (tilde)f is too long.
Added gd= option specifying the minimum number of guide dots that will be
output.
Added td= option to define a multi column table.

7.27:
Removed rg= option and added gm= option to support extended graphics
characters.
Added more rules to braille.tab.
Modified operation of (tilde)p:n to disable a prior (tilde)pn until next page.
Modified operation of (tilde)f so user can mark the last word.

7.26:
Read nfbtrans.cnf before each file is processed.
Added replace options to external format language.
Added another page break option (tilde)p:n to cause page breaks if within n lines
from the bottom of the page.
Do not translate the word us if uppercase.

7.25:
Fixed incorrect line length reported in statistics file if margin greater
     than one.
Added pd=2 to print date but not time on first line. Allow more than 20
characters for filename if pd=0.
Fixed problem of options being mistaken for filenames on command line.
Creates different files when sp=1 and two files translated within one second.
Added ability to change where line breaks occur with old type of TOC entry.

7.24:
Modified the (tilde)p command for conditional page breaks.  (tilde)p can also skip
to the next right facing page for interpoint embossors.
Added lf= option so a group of files can be embossed as one file.
Changed nfbtrans.doc to nfbtrans.fmt with nfbtrans formatting
commands to produce a respectable braille manual.
Fixed several problems including incorrect page numbers in Table of Contents,
unnecessary reloading of table switching from grade zero to  two, and certain
words being joined together when switching from grade two to computer braille.
Added more error trapping with format commands.

7.23:
Modified rw= option to produce a hyphen file so users can select words
not to rejoin.
Improved handling of (tilde)f table of contents command.  Nfbtrans can automatically
determine braille page numbers for a Table of Contents.
nfbtrans.exe has been compiled for 80286 Processors.

7.22:
Don't rejoin words if the first word has more than one dash or if the second
word has one or more dashes.
Report the correct line number with rw=2.

7.21:
Fixed problem of embossing an extra page where a page range is specified.
If the interpoint option is set and a page range is specified, the ending page
is set to an even page so the back side of the last page will be printed.
Look for a .efl file only if the input file is an ordinary file.
Added qm quiet mode option.
Added rw=2 to list rejoined words during translation.

7.20:
Command line arguments other than xx= options are considered to be files to
be translated or embossed.  They must come after the xx= options if any.
File names starting with the @ character indicate a file containing a list
of files to be translated.
nfbtrans.cnf is now a required file.
Fixed problem of occasionally outputting a blank page with a page number at
the end of a file.
Improved operation of wildcards in DOS.
Modified the pf= option so that pf=2 means print filename rather than entire
path.
Removed fx option.
Added ef=n option.  See External Format Language in nfbtrans.doc.
Improved coding for external format language.
Added (tilde)6 automatic paragraph format command.
Added sp=2 telling program to pause between files so print queue
won't overflow.
Added si= option.
Added et= option.

7.19:
Allow ex= to specify a base file name.	If a file has no extension, a search
is done on the base name of a file.
Eliminated output of blank lines in text mode.
Fixed problem where long words were split and characters were lost because
of page numbers.
Added pn= print device option.
Fixed problem where progress messages appeared in the output file when output
was redirected.
Improved operation of the (tilde)3 command.
Add kc keep control characters option.
Toggle sound on/off by pressing space during translation.

7.18:
Added new type 11 meaning beginning but not all.  Changed numeric
type to 19.
Fixed problem causing printing of only the first file if sp=1 and wildcards
were used.
Fixed the (tilde)- option so it could be followed by other tilde commands.
Fixed problem of program outputting occasional blank lines.
Fixed error where words > page width lost characters when split to next line.

7.17:
Improved coding for joining hyphenated words.
Added option rg= so characters >127 are ignored.
Improved handling of words greater than the page width.  This could happen
when translating a mathematical formula or an 80 column line of dashes.
Words consisting of repeated characters are truncated to the current line
length.  Alternating characters like -=-=-= are also considered repeating
characters.
Words of non-repeating characters are put on subsequent lines.	Words more than
132 characters are truncated.
Previous versions always used 6 characters for the page number in the upper
right corner.  This has been changed to 4 if page is less than 10 and 5
if page is less than 100.
Backspace ctrl-h characters are handled properly.  This is useful for
printing unix man pages where ^h is used to underline words.

7.16:
Added an alternative to 8-dot braille.	Output is computer braille with the
     addition of braille capitalization marks and dots 56 to indicate
     lowercase.
Eliminated the obsolete options fi= fo= po= tilde tilde =.
Added options fs=format_string.
Added option cs= so embossing time can be estimated if sp=1.
Improved error checking while loading braille.tab.  Match strings must
now be in lexical order.

7.15:
 Added support for IO redirection.
Rejoins hyphenated words before translation.
Change lines of underscores to dashes for nicer looking braille.
Added support for interpoint printers.
Supports the DOS print command for embossing in backround.
Will optionally number first page.
Automatic centering of multiple lines.
Can emboss multiple files in DOS with wildcards.
User can configure nfbtrans to translate files depending on their extension.
     for example .c files in computer braille and .brl in Grade 2.
User can translate to Grade 3 using only dot 4, 5, 45 and 456 contractions.
     See (tilde)31 and (tilde)32 in nfbtrans.doc.
User can select to put filename and/or date at top of first page.
Added format options (tilde)4 block, (tilde)5 block paragraph and (tilde)-option to call a
command line option in a format command.
Ending page can also be entered at the starting page prompt.

Send bug reports to n8kl@mindspring.com.
