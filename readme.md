#EdSharp
Modified GPL License
## Description

EdSharp is a full featured text editor that is friendly, powerful,
and open source. It uses a standard Windows interface for an
application that supports multiple document windows. Though
intended for sighted users as well, it seeks to enhance
productivity for users of the JAWS, Window-Eyes, or System Access
screen readers by automatically verbalizing relevant information.
These speech messages supplement default speech heuristics,
providing confirmation or results of commands without the need for
manually interrogating the screen. If JAWS, Window-Eyes, or System
Access are not detected in memory, EdSharp uses the default SAPI
voice, if available, which may be configured via the Speech applet
in Control Panel.

Written in the C\# (pronounced C Sharp) language, EdSharp
implements the Homer editor interface, which originally evolved
with the TextPal editor, available at  
[http://www.EmpowermentZone.com/palsetup.exe](http://www.empowermentzone.com/palsetup.exe)  
The same interface was also implemented in the package of JAWS
scripts and tools called HomerKit, available at  
[http://www.EmpowermentZone.com/kitsetup.exe](http://www.empowermentzone.com/kitsetup.exe)

EdSharp requires the .NET Framework 2.0 to run -- a free Microsoft
download from  
[http://www.microsoft.com/download/en/details.aspx?displaylang=en&id=19](http://www.microsoft.com/download/en/details.aspx?displaylang=en&id=19)

Almost every EdSharp command may be done through a mnemonic
keystroke, as well as a menu or mouse operation. These commands
begin with the standard keys available in Notepad or most
Windows-based editors. EdSharp then adds many beneficial features.
Optional JAWS scripts provide further fine tuning of the speech
interface for those users.

## Installation

The installation program for EdSharp is called edsetup.exe. When
executed, it prompts for a program folder, the default being  
C:\\Program Files\\EdSharp  
The installer also creates a program group for EdSharp on the
Windows start menu, containing choices to launch EdSharp, read
Documentation, and uninstall. Additional choices either set or
clear an association between EdSharp and files with a particular
extension, such as .txt or .ini. Binary formats such as .pdf or
.ppt may also be associated with EdSharp, thus permitting automatic
conversion to text when opened via Windows Explorer.

The EdSharp setup program checks whether the required .NET
Framework 2.0 is already installed, and if not, lets you do so. You
may need to reboot Windows and restart the EdSharp installer during
the process.

After installing EdSharp, the setup program presents a list of
three checkboxes that are on by default. The first checkbox offers
an optional set of JAWS scripts to fine tune the EdSharp speech
interface in a few ways that could not be accomplished otherwise.
Mainly, these scripts suppress the often unnecessary verbalization
of keystroke names, such as "Control+S," leaving just the command
name if appropriate, such as "Save." If the scripts were installed
and you would later prefer default JAWS behavior instead, delete
files matching the EdSharp.\* pattern from the user script folder,
which may be reached via the "Explore Settings" option in the JAWS
program group of the Windos Start menu. Alternatively, press
Insert+0 when EdSharp is active to load JAWS Script Manager, then
cursor down to the following line:

;SwitchToConfiguration("default")

Delete the initial semicolon character (;), which uncomments that
code, then press Control+S to save and recompile the scripts. Press
Alt+F4 to exit script manager.

If you prefer not to install the JAWS scripts in the first place,
e.g., because you are a Window-Eyes, System Access, or HAL user,
then press Spacebar to uncheck that option of the setup program.

The second checkbox sets Alt+Control+E as a system-wide key
associated with the EdSharp shortcut placed on the Windows desktop.
If this hot key is found to conflict with an existing shortcut,
navigate to either the EdSharp or other shortcut on the desktop,
press Alt+Enter for properties, and then change the hot key to
something else (or leave it blank).

The third checkbox in the list, available via DownArrow, offers
EdSharp documentation in the default web browser (usually Internet
Explorer).

EdSharp may be safely installed over previous versions. The About
option from its Help menu, or Alt+F1 key, indicates the current
version number and release date. The History of Changes command,
Shift+F1, summarizes fixes and improvements over time.

EdSharp features may be explained in the following categories:
editing, navigating, querying, managing files, pasting snippets,
working with structured text, word processing, programming, and
miscellaneous.



