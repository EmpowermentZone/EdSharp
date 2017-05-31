# EdSharp Developer Guide
May 30, 1917\

## Code Deficiencies
The EdSharp author is a self-taught developer who did not learn professional coding practices that are commonly used in team programming environments today.  He wrote programs in ways that were most efficient for him.  Notably, this included using a large EdSharp.cs file for most of the code and not using white space indentation with code structures.  This code compiles to the current EdSharp.exe binary.

C# code can be intented with automatic tools, so that issue may be addressed easily.  It is fine to restructure and reformat the code, but a cautious approach of resisting temptation to fix what is not broken may optimize programming efforts.  These efforts are needed in fixing bugs and adding features.  If doing these things also requires restructuring or reformatting, however, then, by all means, do what is needed.

## Coding Style
EdSharp uses Hungarian notation for variable names, where a is array, c is character, d is dictionary, f is file, h is handle, i and j are integers, k is key, l is list, n is real number, o is object, s is string, t is tuple or type, v is variant or value, x, y, and z are Caretisian coordinates.  This style has fallen out of fashion, but the EdSharp author still finds it useful.  In this way, an object can have a few data structures related to it that are held in variable names that are the same except for a single letter prefix before the object name.  That letter, moreover, quickly informs the reader of the data type, if it is a base type of the language.  For consistency, it is probably best to continue with Hungarian notation for code edits or editions.

## Programming Languages
EdSharp is primarily written in C#.  Some libraries/assemblies are in Visual Basic or JScript.net.

## .NET Version
The current EdSharp code base is compilable with 32-bit command-line compilers of the .NET Framework version 2.0.  EdSharp 4.0, however, is compiled with 32-bit compilers of .NET 4.6.  Additional coding syntax could be used in developing improvements to EdSharp.  C# 6.0 is the latest version of the language whereas C# 2.0 has been the language of EdSharp.

## Bits
EdSharp uses some 32-bit assemblies that produce runtime errors if EdSharp is compiled as a 64-bit process.  For now, it is probably not worth trying to convert 32-bit dependencies to 64-bit alternatives.

## Screen Reader Support
EdSharp can send direct speech messages to JAWS, NVDA, SAPI, System Access, and Window-Eyes.  It may not be necessary to continue support of them all.  For example, Window-Eyes might be dropped because the product is discontinued.  System Access might be dropped because no such user of EdSharp has been known.

## Compiling Source
At a Windows command prompt, change to the EdSharp program directory.  Enter CompileEdSharp.cmd to compile the main source code, EdSharp.cs.  CompileJsSupport.cmd and CompileVbSupport.cmd compile the JScript.NET and Visual Basic support libraries, respectively.

## File Conversions
EdSharp is intended as a general text editor and viewer, as well as a coding editor.  Almost any file format may be imported as text into EdSharp via the Open Other Format command, Control+Shift+O.  Some conversions may now be considered obsolete or superceded, so support utilities that EdSharp calls behind the scenes could be removed from the setup package.  Sometimes, a 3rd party utility may be sufficiently large so as to recommend its download separately if such conversions are needed, e.g., the Calibre software.

## Markdown
Besides maximizing compatibility with NVDA, a development goal is to maximize EdSharp support for writing or navigating text documents in Markdown format.  A step underway is converting the EdSharp.txt user guide from structured text format to Markdown format.  As much as possible, EdSharp import and export commands should be configured to use pandora.exe, included in the setup package.

## Snippets
Sets of snippets for more languages may be curated and added to the EdSharp setup package.  Snippet invocation capabilities might also be enhanced.

## Dynamic Evaluation
In a few commands, EdSharp calls the Eval function of JScript for dynamic evaluation of either one-line expressions or blocks of statements, e.g., the Evaluate Expression command, Control+Equals or the Invoke Snippet command, Alt+V.  The assembly used is JsSupport.dll.

## Customization
The EdSharp environment may be customized by adding any of the following elements:  snippets, file conversions, compiler configurations, transformation jobs, and web client utilities. evaluated