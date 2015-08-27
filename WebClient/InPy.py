"""
InPy
Version 1.6
March 15, 2010
Copyright 2009 - 2010 by Jamal Mazrui
GNU Lesser General Public License (LGPL)
"""

import pywinauto
import atexit
import base64
import calendar
import caseless
# import clr
import cgi
import chrono
import cmd
import code
import codecs
import commands
import comtypes
import comtypes.client
import configobj
import ConfigParser
import copy
import csv
import ctypes
import datetime
import dateutil.parser
import dateutil.relativedelta
# import dateutil.rule
import dateutils
import deliciousapi
import win32api
import doctest
import feedfinder
import feedparser
import fnmatch
import ftplib
import getopt
import getpass
import glob
import googlemaps
import html2text
import htmlentitydefs
import htmllib
import httplib
import Image
import ImageGrab
import TiffImagePlugin
import locale
import logging
import lxml
import math
import mimepost
import mimetools
import mimetypes
# import msilib # adds over a meg to .exe
import msvcrt
import odict
import optparse
import os
import os.path
import pathutils
import parsedatetime
import pickle
import pkgutil
import platform
import poster.encode
import poster.streaminghttp
import pprint
import pythoncom
import pywapi
import random
import re
import ConfigParser
import readline
import robotparser
import scrapy
import sets
import sgmllib
import shelve
import shutil
import signal
import simplejson
import Skype4Py
import sqlite3
import standout
import string
import StringIO
import subprocess
import sunlightapi
import sys
import tarfile
import tempfile
import textwrap
import thread
import threading
import time
import tinyurl
import twill
import types
import unicodedata
import uri
import urllib
import urllib2
import urlparse
import urlunshort
import validate
import webbrowser
import win32api
import win32clipboard
import win32com.client
import win32com.client.dynamic
import win32con
import win32console
import win32gui
import winsound
import xml.dom
import xml.etree.ElementTree
import yahoo.search.term
import zipfile

# Constants

# sDefaultEncoding = sys.stdout.encoding
sDefaultEncoding = locale.getdefaultlocale()[1]
xDebugCount = 0
ioForReading = 1
ioForWriting = 2
ioForAppending = 8

formatTristateUseDefault = -2 # Open as System default
formatTristateTrue = -1 # Open as Unicode
formatTriStateFalse = 0 # Open as ASCII

vbBlack = 0x00 # Black
vbRed = 0xFF # Red
vbGreen = 0xFF00 # Green
vbYellow = 0xFFFF # Yellow
vbBlue = 0xFF0000 # Blue
vbMagenta = 0xFF00FF# Magenta
vbCyan = 0xFFFF00# Cyan
vbWhite = 0xFFFFFF # White

vbBinaryCompare = 0 # Perform a binary comparison
vbTextCompare = 1 # Perform a textual comparison

vbSunday = 1 # Sunday
vbMonday = 2 # Monday
vbTuesday = 3 # Tuesday
vbWednesday = 4 # Wednesday
vbThursday = 5 # Thursday
vbFriday = 6 # Friday
vbSaturday = 7 # Saturday
vbUseSystemDayOfWeek = 0 # Use the day of the week specified in your system settings
vbFirstJan = 11 # Use the week in which January = 1 # occurs (default)
vbFirstFourDays = 2 # Use the first week that has at least four days in the new
vbFirstFullWeek = 3 # Use the first full week of the year
vbGeneralDate = 0 # Display a date and/or time. For real numbers, display a date and
vbLongDate = 1 # Display a date using the long date format specified in your
vbShortDate = 2 # Display a date using the short date format specified in your
vbLongTime = 3 # Display a time using the long time format specified in your
vbShortTime = 4 # Display a time using the short time format specified in your
vbObjectError = -2147221504 # User-defined error numbers should be greater than this

vbOKOnly = 0 # Display OK button only
vbOKCancel = 1 # Display OK and Cancel buttons
vbAbortRetryIgnore = 2 # Display Abort, Retry, and Ignore buttons
vbYesNoCancel = 3 # Display Yes, No, and Cancel buttons
vbYesNo = 4 # Display Yes and No buttons
vbRetryCancel = 5 # Display Retry and Cancel buttons
vbCritical = 16 # Display Critical Message icon
vbQuestion = 32 # Display Warning Query icon
vbExclamation = 48 # Display Warning Message icon
vbInformation= 64 # Display Information Message icon
vbDefaultButton1 = 0 # First button is the default
vbDefaultButton2 = 256 # Second button is the default
vbDefaultButton3 = 512 # Third button is the default
vbDefaultButton4 = 768 # Fourth button is the default
vbApplicationModal = 0 # Application modal. The user must respond to the message box
vbSystemModal = 4096 # System modal
vbOK = 1 # OK button was clicked
vbCancel = 2 # Cancel button was clicked
vbAbort = 3 # Abort button was clicked
vbRetry = 4 # Retry button was clicked
vbIgnore = 5 # Ignore button was clicked
vbYes = 6 # Yes button was clicked
vbNo = 7 # No button was clicked
vbCr = '\r' # carriage return
vbFormFeed = '\f' # form feed
vbLf = '\n' # line feed
vbNewLine = '\r\n' # platform-specific new line
vbNullChar = 0x000 # None character
vbNullString = '\0x0000'
vbTab = '\t' # tab
vbVerticalTab = '\v' # vertical tab
vbUseDefault = -2 # Use default from computer's regional settings
vbtrue = -1 # True
vbfalse = 0 # False
vbEmpty = 0 # Uninitialized (default)
vbNull = 1 # Contains no valid data
vbInteger = 2 # Integer subtype
vbLong = 3 # Long subtype
vbSingle = 4 # Single subtype
vbDouble = 5 # Double subtype
vbCurrency = 6 # Currency subtype
vbDate = 7 # Date subtype
vbString = 8 # String subtype
vbObject = 9 # Object
vbError = 10 # Error subtype
vbBoolean = 11 # Boolean subtype
vbVt = 12 # Vt (used only for arrays of Vts)
vbDataObject = 13 # Data access object
vbDecimal = 14 # Decimal subtype
vbByte = 17 # Byte subtype
vbArray = 8192 # Array

vbUseSystem = 0 # Use National Language Support (NLS) API setting
vbDefaultButton = 1 # = 0 # First button is default
vbCrLf = '\r\n'
xMaxPath = 260
xSpace = ' '
xMute = ''
xComma = ','
xCommaSpace = ', '
xBar = '|'
xQuote = '"'
xApostrophe = "'"
xSlash = '/'
xBackslash = '\\'
xEquals = '='
xColon = ':'
xSemicolon = ';'
xLessThan = '<'
xGreaterThan = '>'
xLeftBrace = '{'
xRightBrace = '}'
xUTF = 8 # 'EFBBBF'
xSectionBreak = vbCrLf + '----------' + vbCrLf + vbFormFeed + vbCrLf
xEndOfDocument = vbCrLf + '----------' + vbCrLf + 'End of Document' + vbCrLf

adVarWChar = 202

adAffectCurrent = 1 #

adSaveCreateNotExist = 1 # Default, create if absent
adSaveCreateOverWrite = 2 # create even if present
adTypeBinary = 1 # Binary
adTypeText = 2 # default, text

adAffectGroup = 2 #
adAffectAll = 3 #
adFilterNone = 0 #
adFilterPendingRecords = 1 #
adFilterAffectedRecords = 2 #
adFilterFetchedRecords = 3 #
adFilterConflictingRecords = 5 #
adClipString = 2 #
adUseServer = 2 #
adUseClient = 3 #
adOpenForwardOnly = 0 #
adOpenKeySet = 1 #
adOpenDynamic = 2 #
adOpenStatic = 3 #
adLockReadOnly = 1 #
adLockOptimistic = 3 #
adBatchOptimistic = 4 #
adCmdText = 1 #
adCmdTableDirect = 512 #
adExecuteNoRecords = 128 #
adSearchBackward = -1 #
adSearchForward	= 1 #
adBookmarkCurrent = 0 #
adBookmarkFirst = 1 #
adBookmarkLast = 2 #
adPosBOF = -2 #
adPosEOF = -3 #
adPosUnknown = -1 #

# Functions

# pythoncom.CoInitialize()

# Initialization may be needed for encoding support
# locale.setlocale(locale.LC_ALL, '')

def EncodeBasicCredentials(sUserName, sPassword): return base64.encodestring('%s:%s') % (sUserName, sPassword)

def AddBasicAuthorizationHeader(oRequest, sUserName, sPassword):
	sCredentials = EncodeBasicCredentials(sUserName, sPassword)
	sHeaderName = 'Authorization'
	sHeaderValue = 'Basic %s' % sCredentials
	oRequest.add_header(sHeaderName, sHeaderValue)

def InitForBasicAuthorization(sUserName, sPassword):
	oPasswordManager = urllib2.HTTPPasswordMgrWithDefaultRealm()     
	oPasswordManager.add_password(None, sAddress, sUserName, sPassword)
	oAuthHandler = urllib2.HTTPBasicAuthHandler(oPasswordManager)
	oOpener = urllib2.build_opener(oAuthHandler)
	urllib2.install_opener(oOpener)

def AddLineIfKey(sText, d, sKey):
	sValue = d.get(sKey)
	if sValue: return sText + sValue + '\r\n'
	else: return sText

def IniFormGetSection(sFile, sSection, sDefault=''):
	sBody = FileToString(sFile)
	sMatch = '[[' + sSection + ']]\r\n'
	iStart = sBody.find(sMatch)
	if iStart == -1: return sDefault

	iStart = iStart + len(sMatch)
	sBody = sBody[iStart:]
	sMatch = '\r\n[['
	iEnd = sBody.find(sMatch)
	if iEnd >= 0: sBody = sBody[0:iEnd]
	return sBody

def IniFormDialogBrowseForFolder(sFolder):
# Get choice  from a Browse For Folder dialog
	
	sTempDir = PathCreateTempFolder()
	sInputIni = PathCombine(sTempDir, 'Input.ini')
	sOutputIni = PathCombine(sTempDir, 'Output.ini')
	sInputTxt = PathCombine(sTempDir, 'Input.txt')
	sOutputTxt = PathCombine(sTempDir, 'Output.txt')
	sCommand = sIniFormExe + " " + sTempDir + "\\"
	
	sTitle = 'Folder'
	sBody = "[" + sTitle + "]\r\ncontrol=folder\r\nvalue=" + sFolder + "\r\nMisc=NoStatus\r\n"
	StringToFile(sBody, sInputIni)
	ShellRun(sCommand, 1, True)
	FileDelete(sInputIni)
	
	sReturn = ''
	if FileExists(sOutputIni):
		sReturn = win32api.GetProfileVal("Results", "folder", "", sOutputIni)
		FileDelete(sOutputIni)

	FolderDelete(sTempDir)
	return sReturn
	
def IniFormDialogSaveFile(sFile, bForce=False):
# Get choice  from a Save File dialog
	
	sTempDir = PathCreateTempFolder()
	sInputIni = PathCombine(sTempDir, 'Input.ini')
	sOutputIni = PathCombine(sTempDir, 'Output.ini')
	sInputTxt = PathCombine(sTempDir, 'Input.txt')
	sOutputTxt = PathCombine(sTempDir, 'Output.txt')
	sCommand = sIniFormExe + " " + sTempDir + "\\"
	
	sTitle = 'Save File'
	sBody = "[" + sTitle + "]\r\ncontrol=save\r\nvalue=" + sFile + "\r\n"
	if bForce: sBody = sBody + "Misc=NoOverWritePrompt\r\n"
	else: sBody = sBody + "Misc=NoStatus\r\n"
	
	StringToFile(sBody, sInputIni)
	ShellRun(sCommand, 1, True)
	FileDelete(sInputIni)

	sReturn = ''
	if FileExists(sOutputIni):
		sReturn = win32api.GetProfileVal("Results", "Save File", "", sOutputIni)
		FileDelete(sOutputIni)

	FolderDelete(sTempDir)
	return sReturn
	
def IniFormDialogOpenFile(sFile):
# Get choice  from an Open File dialog
	
	sTempDir = PathCreateTempFolder()
	sInputIni = PathCombine(sTempDir, 'Input.ini')
	sOutputIni = PathCombine(sTempDir, 'Output.ini')
	sInputTxt = PathCombine(sTempDir, 'Input.txt')
	sOutputTxt = PathCombine(sTempDir, 'Output.txt')
	sCommand = sIniFormExe + " " + sTempDir + "\\"
	
	sTitle = 'Open File'
	sBody = "[" + sTitle + "]\r\ncontrol=open\r\nvalue=" + sFile + "\r\nMisc=NoStatus\r\n"
	StringToFile(sBody, sInputIni)
	ShellRun(sCommand, 1, True)
	FileDelete(sInputIni)

	sReturn = ''
	if FileExists(sOutputIni):
		sReturn = win32api.GetProfileVal("Results", "Open File", "", sOutputIni)
		FileDelete(sOutputIni)

	FolderDelete(sTempDir)
	return sReturn
	
def IniFormDialogMultiPick(sTitle, lValues, bSort=False):
# Get choices from a multiple selection list box using IniForm.exe
	
	sTempDir = PathCreateTempFolder()
	sInputIni = PathCombine(sTempDir, 'Input.ini')
	sOutputIni = PathCombine(sTempDir, 'Output.ini')
	sInputTxt = PathCombine(sTempDir, 'Input.txt')
	sOutputTxt = PathCombine(sTempDir, 'Output.txt')
	sCommand = sIniFormExe + " " + sTempDir + "\\"
	
	sValues = '\r\n'.join(lValues)
	sValues = "[[Multi]]\r\n" + sValues
	StringToFile(sValues, sInputTxt)

	sBody = "[" + sTitle + "]\r\nMultiWidth=600\r\nMultiHeight=600\r\nMisc=NoStatus\r\n"
	sBody = sBody + "[Multi]\r\nControl=Multi\r\n"
	if bSort: sBody = sBody + "Misc=NoLabel|Sort\r\n"
	else: sBody = sBody + "Misc=NoLabel|SelectAll\r\n"

	sBody = sBody + "[OK]\r\nControl=Button\r\n"
	sBody = sBody + "[Cancel]\r\nControl=Button\r\n"
	StringToFile(sBody, sInputIni)
	ShellRun(sCommand, 1, True)
	FileDelete(sInputIni)

	lReturn = []
	if FileExists(sOutputIni):
		sIndexes = win32api.GetProfileVal("Results", "Multi", "", sOutputIni)
		lIndexes = sIndexes.split('|')
		iCount = len(lIndexes)
		for i in range(iCount):
			iIndex = int(lIndexes[i])
			iIndex -= 1
			sResult = lValues[iIndex]
			lReturn.append(sResult)

		FileDelete(sOutputIni)

	FolderDelete(sTempDir)
	return lReturn
	

def IniFormDialogPick(sTitle, lValues, bSort=False):
# Get choices from a single selection list box using IniForm.exe
	
	sTempDir = PathCreateTempFolder()
	sInputIni = PathCombine(sTempDir, 'Input.ini')
	sOutputIni = PathCombine(sTempDir, 'Output.ini')
	sInputTxt = PathCombine(sTempDir, 'Input.txt')
	sOutputTxt = PathCombine(sTempDir, 'Output.txt')
	sCommand = sIniFormExe + " " + sTempDir + "\\"
	
	sValues = '\r\n'.join(lValues)
	sValues = "[[List]]\r\n" + sValues
	StringToFile(sValues, sInputTxt)

	sBody = "[" + sTitle + "]\r\nListWidth=600\r\nListHeight=600\r\nMisc=NoStatus\r\n"
	sBody = sBody + "[List]\r\nControl=List\r\n"
	sBody = sBody + 'Selection=1' + '\r\n'
	if bSort: sBody = sBody + "Misc=NoLabel|Sort\r\n"
	else: sBody = sBody + "Misc=NoLabel\r\n"

	sBody = sBody + "[OK]\r\nControl=Button\r\n"
	sBody = sBody + "[Cancel]\r\nControl=Button\r\n"
	StringToFile(sBody, sInputIni)
	ShellRun(sCommand, 1, True)
	FileDelete(sInputIni)

	sReturn = ''
	if FileExists(sOutputIni):
		sReturn = win32api.GetProfileVal('Results', 'List', '', sOutputIni)
		FileDelete(sOutputIni)
	
		FileDelete(sOutputIni)

	FolderDelete(sTempDir)
	return sReturn
	
	

def IniFormDialogMultiInput( sTitle, lFields, lValues):
#Get input from multiple edit boxes using IniForm.exe
	
	sTempDir = PathCreateTempFolder()
	sInputIni = PathCombine(sTempDir, 'Input.ini')
	sOutputIni = PathCombine(sTempDir, 'Output.ini')
	sCommand = sIniFormExe + ' ' + sTempDir + '\\'
	iCount = len(lFields)
	sBody = '[' + sTitle + ']\r\nMisc=NoStatus\r\n'
	
	for iLoop in range(iCount):
		sField = lFields[iLoop]
		sValue = lValues[iLoop]
		sBody = sBody + '[' + sField + ']\r\nValue=' + sValue + '\r\n'
	
	sBody = sBody + '[OK]\r\nControl=Button\r\n'
	sBody = sBody + '[Cancel]\r\nControl=Button\r\n'
	FileDelete(sInputIni)
	StringToFile(sBody, sInputIni)
	ShellRun(sCommand, 1, True)
	FileDelete(sInputIni)
	
	lReturn = []
	if FileExists(sOutputIni):
		for iLoop in range(iCount):
			sField = lFields[iLoop]
			sResult = win32api.GetProfileVal('Results', sField, '', sOutputIni)
			lReturn.append(sResult)
	
		FileDelete(sOutputIni)
	
	FolderDelete(sTempDir)
	return lReturn
	
def IniFormDialogInfo( sTitle, sText):
#Display information in a multiline edit box using IniForm.exe
	
	sTempDir = PathCreateTempFolder()
	sInputIni = PathCombine(sTempDir, 'Input.ini')
	sOutputIni = PathCombine(sTempDir, 'Output.ini')
	sCommand = sIniFormExe + ' ' + sTempDir + '\\'
	sInputTxt = PathCombine(sTempDir, 'Input.txt')
	sOutputTxt = PathCombine(sTempDir, 'Output.txt')
	sBody = '[' + sTitle + ']\r\nMemoWidth=300\r\nMemoHeight=300\r\nMisc=NoStatus\r\n'
	sBody = sBody + '[InfoView]\r\nControl=Memo\r\n'
	sBody = sBody + 'Misc=NoLabel|ReadOnly\r\n'
	sBody = sBody + '[Close]\r\nControl=Button\r\nID=2\r\n'
	StringToFile(sBody, sInputIni)
	
	sText = '[[InfoView]]\r\n' + sText
	StringToFile(sText, sInputTxt)
	
	ShellRun(sCommand, 1, True)
	
	FolderDelete(sTempDir)
	
def IniFormDialogInput( sTitle, sField, sValue):
# /Get single field of input

	lFields = [sField]
	lValues = [sValue]
	lResults = IniFormDialogMultiInput(sTitle, lFields, lValues)
	if lResults: sReturn = lResults[0]
	else: sReturn = xMute
	return sReturn
	
def old_IniFormDialogPick( sTitle, lValues, bSort=False):
#Get choice from a single selection list box using IniForm.exe
	

	sTempDir = PathCreateTempFolder()
	sInputIni = PathCombine(sTempDir, 'Input.ini')
	sOutputIni = PathCombine(sTempDir, 'Output.ini')
	sTempDir = PathCreateTempFolder()
	sCommand = sIniFormExe + ' ' + sTempDir + '\\'
	sInputTxt = PathCombine(sTempDir, 'Input.txt')
	sOutputTxt = PathCombine(sTempDir, 'Output.txt')

	sValues = '\r\n'.join(lValues)
	sValues = '[[List]]\r\n' + sValues
	StringToFile(sValues, sInputTxt)

	sBody = '[' + sTitle + ']\r\nListWidth=300\r\nListHeight=300\r\nMisc=NoStatus\r\n'
	sBody = sBody + '[List]\r\nControl=List\r\n'
	sBody = sBody + 'Selection=1' + '\r\n'
	if bSort: sBody = sBody + 'Misc=NoLabel|Sort\r\n'
	else: sBody = sBody + 'Misc=NoLabel\r\n'
	
	sBody = sBody + '[OK]\r\nControl=Button\r\n'
	sBody = sBody + '[Cancel]\r\nControl=Button\r\n'
	StringToFile(sBody, sInputIni)
	ShellRun(sCommand, 1, True)
	
	FileDelete(sInputIni)
	sReturn = ''
	if FileExists(sOutputIni):
		sReturn = win32api.GetProfileVal('Results', 'List', '', sOutputIni)
		FileDelete(sOutputIni)
	
	FolderDelete(sTempDir)
	return sReturn
	
def IniFormDialogMemo( sTitle, sText):
#Get input from a multiline edit box using IniForm.exe
	
	sTempDir = PathCreateTempFolder()
	sInputIni = PathCombine(sTempDir, 'Input.ini')
	sOutputIni = PathCombine(sTempDir, 'Output.ini')
	sCommand = sIniFormExe + ' ' + sTempDir + '\\'
	sInputTxt = PathCombine(sTempDir, 'Input.txt')
	sOutputTxt = PathCombine(sTempDir, 'Output.txt')
	sBody = '[' + sTitle + ']\r\nMemoWidth=300\r\nMemoHeight=300\r\nMisc=NoStatus\r\n'
	sBody = sBody + '[MemoEdit]\r\nControl=Memo\r\n'
	sBody = sBody + 'Misc=NoLabel\r\n'
	sBody = sBody + '[OK]\r\nControl=Button\r\n'
	sBody = sBody + '[Cancel]\r\nControl=Button\r\n'
	StringToFile(sBody, sInputIni)
	
	sText = '[[MemoEdit]]\r\n' + sText
	StringToFile(sText, sInputTxt)
	
	ShellRun(sCommand, 1, True)
	
	FileDelete(sInputIni)
	FileDelete(sInputTxt)

	sReturn = ''
	if FileExists(sOutputIni):
		sReturn = IniFormGetSection(sOutputTxt, 'MemoEdit')
		FileDelete(sOutputIni)
		FileDelete(sOutputTxt)
	
	FolderDelete(sTempDir)
	return sReturn
	
def JsToPyObject(sText): return simplejson.loads(sText)

def StringUnicode(sText): return utf16str(sText)

def Exit(sText=None):
	if sText: SayLine(sText)
	sys.exit(0)

def WriteValue(sKey, sValue, sSection='Configuration', sIni=None):
	if not sIni: sIni = sInputFile
	return win32api.WriteProfileVal(sSection, sKey, sValue, sIni)

def ReadValue(sKey, vDefault='', sSection='Configuration', sIni=None):
	if not sIni: sIni = sInputFile
	return win32api.GetProfileVal(sSection, sKey, vDefault, sIni)

def FileCopy(sSource, sTarget):
	# Copy source to destination file, replacing if it exists
	
	if not FileDelete(sTarget): return False
	
	oSystem =VtCreateFileSystemObject()
	try: oSystem.CopyFile(sSource, sTarget)
	except: pass

	oSystem = None
	return FileExists(sTarget)
	# FileCopy method
	
def FileDelete(sFile):
	# Delete a file if it exists, and test whether it is subsequently absent
	# either because it was successfully deleted or because it was not present in the first place
	
	if not FileExists(sFile): return True
	oSystem =VtCreateFileSystemObject()
	try: oSystem.DeleteFile(sFile, True)
	except: pass
	
	oSystem = None
	return not FileExists(sFile)
	# FileDelete method
	
def FileExists(sFile):
	# Test whether File exists
	
	oSystem =VtCreateFileSystemObject()
	bReturn = not oSystem.FolderExists(sFile) and oSystem.FileExists(sFile)
	
	oSystem = None
	return bReturn
	# FileExists method
	
def FileGetDate(sFile):
	# Get date of a file
	
	if not FileExists(sFile): return None
	
	oSystem =VtCreateFileSystemObject()
	oFile =oSystem.GetFile(sFile)
	dtReturn =oFile.DateLastModified
	
	oFile = None
	oSystem = None
	return dtReturn
	# FileGetDate method
	
def FileGetSize(sFile):
	# Get size of a file
	
	if not FileExists(sFile): return 0 
	
	oSystem =VtCreateFileSystemObject()
	oFile =oSystem.GetFile(sFile)
	iReturn =oFile.size
	
	oFile = None
	oSystem = None
	return iReturn
	# FileGetSize method
	
def FileGetType(sFile):
	# Get file type
	
	if not FileExists(sFile): return xMute
	
	oSystem =VtCreateFileSystemObject()
	oFile =oSystem.GetFile(sFile)
	sReturn =oFile.Type
	
	oFile = None
	oSystem = None
	return sReturn
	# FileGetType method
	
def FileMove(sSource, sTarget):
	# Move source to destination file, replacing if it exists
	
	if not FileDelete(sTarget): return False
	
	oSystem =VtCreateFileSystemObject()
	try: oSystem.MoveFile(sSource, sTarget)
	except: pass
	
	oSystem = None
	return FileExists(sTarget)
	# FileMove method
	
def FileToString(sFile):
	# Get content of text file
	
	if FileGetSize(sFile) == 0: return xMute
	
	oSystem =VtCreateFileSystemObject()
	bCreate = False
	oFile = oSystem.OpenTextFile(sFile, ioForReading, bCreate, formatTriStateFalse)
	sReturn =oFile.ReadAll()
	oFile.Close()
	
	oFile = None
	oSystem = None
	return sReturn
	# FileToString method
	
def FolderCopy(sSource, sTarget):
	# Copy source to destination Folder, replacing if it exists
	
	if not FolderExists(sTarget): return False
	
	oSystem =VtCreateFileSystemObject()
	try: oSystem.CopyFolder(sSource, sTarget)
	except: pass
	
	oSystem = None
	return FolderExists(sTarget)
	# FolderCopy method
	
def FolderCreate(sFolder):
	# Create folder
	
	if FolderExists(sFolder): return True
	
	oSystem =VtCreateFileSystemObject()
	try: oSystem.CreateFolder(sFolder)
	except: pass
	
	oSystem = None
	return FolderExists(sFolder)
	# FolderCreate method
	
def FolderDelete(sFolder):
	# Delete a Folder if it exists, and test whether it is subsequently absent
	# either because it was successfully deleted or because it was not present in the first place
	
	if not FolderExists(sFolder): return True
	
	oSystem =VtCreateFileSystemObject()
	try: oSystem.DeleteFolder(sFolder, True)
	except: pass
	
	oSystem = None
	return not FolderExists(sFolder)
	# FolderDelete method
	
def FolderExists(sFolder):
	# Test whether folder exists
	
	oSystem =VtCreateFileSystemObject()
	bReturn =oSystem.FolderExists(sFolder)
	
	oSystem = None
	return bReturn
	# Folder exists method
	
def FolderGetDate(sFolder):
	# Get date of a Folder
	
	if not FolderExists(sFolder): return None
	
	oSystem =VtCreateFileSystemObject()
	oFolder =oSystem.GetFolder(sFolder)
	dtReturn =oFolder.DateLastModified
	
	oFolder = None
	oSystem = None
	return dtReturn
	# FolderGetDate method
	
def FolderGetSize(sFolder):
	# Get size of folder, summing the sizes of files and subfolders it contains
	
	if not FolderExists(sFolder): return 0
	
	oSystem =VtCreateFileSystemObject()
	oFolder =oSystem.GetFolder(sFolder)
	iReturn =oFolder.size
	
	oFolder = None
	oSystem = None
	return iReturn
	# FolderGetSize method
	
def FolderMove(sSource, sTarget):
	# Move source to destination Folder, replacing if it exists
	
	if not FolderExists(sSource): return False
	
	oSystem =VtCreateFileSystemObject()
	try: oSystem.MoveFolder(sSource, sTarget)
	except: pass
	
	oSystem = None
	return FolderExists(sTarget)
	# FolderMove method
	
def HtmlEncodeString(sText):
	# Encode a string for HTML or XML
	
	sReturn = sText.replace('<', '&lt;')
	sReturn = sReturn.replace('>', '&gt;')
	sReturn = sReturn.replace('&', '&amp;')
	sReturn = sReturn.replace(';', '&sc;')
	return sReturn
	# HtmlEncodeString method
	
def HtmlGetLinks(sUrl, bIncludeDownloadName):
	# Get a list of two or three -item lists containing link url, link text, and optionally download file name
	
	lLinks = VtCreateList()
	dUrls = VtCreateDictionary()
	HKCU = 0x80000001 # HKEY_CURRENT_USER
	sRegPath = 'HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\Disable Script Debugger\\'

	"""
	sOldRegValue = RegistryGetString(HKCU, 'Software\\Microsoft\\Internet Explorer\\Main', 'Disable Script Debugger')
	if not StringEquiv(sOldRegValue, 'yes'): RegistrySetString(HKCU, 'Software\\Microsoft\\Internet Explorer\\Main', 'Disable Script Debugger', 'yes')
	"""

	oDoc = VtCreateHtmlFile()
	# DialogShow('url', sUrl)
	bIsUrl = False
	if FileExists(sUrl): sBody = FileToString(sUrl)
	elif RegExpTest(sUrl, '^\\w+:\\S+$', True):
	# elif RegExpTest(sUrl, '\\w+://\\S+', True): {
		sBody = WebUrlToString(sUrl)
		bIsUrl = True
	else: sBody = sUrl
	
	# DialogShow('body length', len(sBody))
	oDoc.write(sBody)
	# DialogShow('IsUrl', bIsUrl.toString())
	
	# Does not work
	# sUrl = oDoc.URL
	# sUrl = oDoc.location
	# sUrl = oDoc.address
	# sUrl = oDoc.hRef
	
	sBase = xMute
	if bIsUrl:
	# sBase = UrlGetPrePath(sUrl)
		sBase = sUrl
	# DialogShow('sBase', sBase)
		sTitle = oDoc.title
	# DialogShow('title', sTitle)
		dUrls.Add(sUrl, '')
		lLink = VtCreateList()
		lLink.Add(sUrl)
		lLink.Add(sTitle)
		if bIncludeDownloadFileName: lLink.Add(WebUrlDownloadFileName(sUrl))
		lLinks.Add(lLink)
	
	oLinks = oDoc.links

	# oLinks = oDoc.all.tags('a')
	# DialogShow('links', oLinks.length)
	# DialogShow(sUrl, sBase)
	# These do not work
	# PyPrint('domain=' + oDoc.domain)
	# PyPrint('hostname=' + oDoc.hostname)

	for i in range(oLinks.length):
	# ShellOpenWith('SayLine.exe', i.toString())
		oLink = oLinks.item(i)
		sUrl = oLink.href
		if not sUrl: continue
		sText = oLink.innerText
		# if sText == 'About the FCC') DialogShow('url', sUrl:
		if not sText: sText = xMute
		if dUrls.Exists(sUrl): continue
		# if not i: DialogShow(len(sBase), '':
		if bIsUrl: sUrl = UrlNormalize(sUrl, sBase)
		else: sUrl = UrlNormalize(sUrl, xMute)
		if sUrl.find(':') == -1: continue
		if dUrls.Exists(sUrl): continue
		dUrls.Add(sUrl, '')
		
		lLink = VtCreateList()
		lLink.Add(sUrl)
		lLink.Add(sText)
		if bIncludeDownloadName: lLink.Add(WebUrlDownloadFileName(sUrl))
		lLinks.Add(lLink)
	
	oDoc.close()
	# if not StringEquiv(sOldRegValue, 'yes'): RegistrySetString(HKCU, 'Software\\Microsoft\\Internet Explorer\\Main', 'Disable Script Debugger', sOldRegValue)
	
	lLink = None
	dUrls = None
	oDoc = None
	# DialogShow('my links', lLinks.Count)
	return lLinks
	# HtmlGetLinks method
	
def HtmlGetLinkTextAndUrls(sUrl):
	# Get link text and urls of a web page
	
	# DialogShow('my url', sUrl)
	sReturn = xMute
	lLinks = HtmlGetLinks(sUrl, True)
	# DialogShow('links', lLinks.Count)
	iCount = lLinks.Count
	for i in range(iCount):
		lLink = lLinks.Item(i)
		sLinkUrl = lLink.Item(0)
		sLinkText = lLink.Item(1)
		sLinkFileName = lLink.Item(2)
		# ShellOpenWith('SayLine.exe', i.toString())
		sReturn += sLinkText + ' = ' + sLinkUrl + ' = ' + sLinkFileName + '\r\n'
		# sReturn += sLinkText + '\r\n'
		# DialogShow('links', lLinks.Count)
	
	if not iCount: 
		lUrls = RegExpExtract(sUrl, '\\w+:\\/\\/[^"\\s]*', False)
		sReturn = VtListToString(lUrls, '\r\n')
		if sReturn: sReturn = ' = ' + StringReplace(sReturn, '\r\n', '\r\n = ', True)
		iCount = lUrls.Count
	
	if sReturn: 
		sReturn = StringPlural('Url', iCount) + '\r\n' + sReturn
		sReturn = StringTrim(sReturn)
		sReturn = StringReplace(sReturn, '\r\n', '\r\n\r\n', False) + '\r\n'
	
	lLink = None
	lLinks = None
	return sReturn
	# HtmlGetLinkTextAndUrls method
	
def HtmlGetTableText(sUrl, iTable):
	# Get the text of an HTML table specified by number, or None for all of them
	
	HKCU = 0x80000001 # HKEY_CURRENT_USER
	sRegPath = 'HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\Disable Script Debugger\\'
	# sOldRegValue = RegistryGetString(HKCU, 'Software\\Microsoft\\Internet Explorer\\Main', 'Disable Script Debugger')
	# if not StringEquiv(sOldRegValue, 'yes'): RegistrySetString(HKCU, 'Software\\Microsoft\\Internet Explorer\\Main', 'Disable Script Debugger', 'yes')
	oDoc = VtCreateHtmlFile()
	sBody = WebUrlToString(sUrl)
	oDoc.write(sBody)
	oTables = oDoc.all.tags('table')
	
	sText = xMute
	if PyIsNone(iTable): 
		for i in range(oTables.length):
			oTable = oTables.item(i)
			sText += oTable.innerText + vbCrLf + vbCrLf
	else: 
		oTable = oTables.item(iTable)
		sText = oTable.innerText + vbCrLf + vbCrLf

	oDoc.close()
	# if not StringEquiv(sOldRegValue, 'yes'): RegistrySetString(HKCU, 'Software\\Microsoft\\Internet Explorer\\Main', 'Disable Script Debugger', sOldRegValue)
	
	oTable = None
	oTables = None
	oDoc = None
	return sText
	# HtmlGetTableText method
	
def HtmlGetText(sUrl, bAddHeader=False):
	# Get the text of an HTML page, optionally including the source URL at the top
	
	HKCU = 0x80000001 # HKEY_CURRENT_USER
	sRegPath = 'HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\Disable Script Debugger\\'
	# sOldRegValue = RegistryGetString(HKCU, 'Software\\Microsoft\\Internet Explorer\\Main', 'Disable Script Debugger')
	# if not StringEquiv(sOldRegValue, 'yes'): RegistrySetString(HKCU, 'Software\\Microsoft\\Internet Explorer\\Main', 'Disable Script Debugger', 'yes')
	oDoc = VtCreateHtmlFile()
	if FileExists(sUrl): sBody = FileToString(sUrl)
	elif RegExpTest(sUrl, '^\\w+:\\S+$', True): sBody = WebUrlToString(sUrl)
	else: sBody = sUrl
	
	oDoc.write(sBody)
	sText = StringTrim(oDoc.body.innerText)
	sTitle = StringTrim(oDoc.title)
	if sText and sTitle: sText = sTitle + '\r\n\r\n' + sText
	if bAddHeader and sText: sText = 'From the web page\r\n' + sUrl + '\r\n\r\n' + sText
	if sText: sText += '\r\n'
	oDoc.close()
	
	# if not StringEquiv(sOldRegValue, 'yes'): RegistrySetString(HKCU, 'Software\\Microsoft\\Internet Explorer\\Main', 'Disable Script Debugger', sOldRegValue)
	
	oDoc = None
	return sText
	# HtmlGetText method
	
def HtmlGetUrls(sUrl):
	# Get a list of URLs linked to a web page
	
	lReturn = VtCreateList()
	if RegExpTest(sUrl, '^\\w+:\\S+$', True): lReturn.Add(sUrl)
	lLinks = HtmlGetLinks(sUrl, false)
	# DialogShow('links', lLinks.Count)
	for i in range(lLinks.Count):
		lLink = lLinks.Item(i)
		sLinkUrl = lLink.Item(0)
	# sLinkUrl = UrlNormalize(sLinkUrl, sUrl)
		if not lReturn.Contains(sLinkUrl): lReturn.Add(sLinkUrl)
	
	lLink = None
	lLinks = None
	return lReturn
	# HtmlGetUrls method
	
def PyDictionaryToEncodedString(d):
	# Convert dictionary to string with http encoding
	return utf8urlencode(d)

	sReturn = ''
	for sKey in d.keys():
		sValue = d[sKey]
		sComponent = '' + encodeURIComponent(sKey) + '=' + encodeURIComponent(sValue)
	# sComponent = '' + sKey + '=' + sValue
		if len(sReturn) > 0: sReturn += '&'
		sReturn += sComponent

	return sReturn
	# PyDictionaryToEncodedString method
	
def PyDictionaryToVt(d):
	# Convert Python dictionary to variant
	
	dReturn = VtCreateDictionary()
	for sKey in d.keys(): dReturn.Add(sKey, d[sKey])
	return dReturn
	# PyDictionaryToVt method
	
def PyEval(sCode, o1, o2, o3, o4):
	# Evaluate sCode, optionally referencing up to 4 parameters of any type, and return the result
	
	return eval(sCode)
	# PyEval method
	
def PyInitArray(oValue, iLength):
	# Initialize a Python list/array
	
	aReturn = []
	for i in Range(iLength): aReturn[i] = oValue
	return aReturn
	# PyInitArray method
	
def PyInspectObject(sName, oValue):
	# Report on type of object and its subobjects
	
	sReturn = ''
	sType = typeof(oValue)
	if sType != 'object' or oValue == None: sReturn += sType + ', ' + sName + ', ' + oValue + '\n'
	elif typeof(oValue.length) == 'number': 
		sReturn += 'array, ' + sName + '\n'
		for i in range(oValue.length):
	# sReturn += 'index ' + i + '\n'
			oItem = oValue[i]
			sReturn += PyInspectObject(sName + '.' + i, oItem)

	else:
		sReturn += 'object, ' + sName+ '\n'
		for sAttribute in oValue.keys():
	# sReturn += 'attribute ' + sAttribute + '\n'
			oItem = oValue[sAttribute]
			sReturn += PyInspectObject(sName + '.' + sAttribute, oItem)

	return sReturn
	# PyInspectObject method
	
def PyIsBlank(sText):
	# Test if string is empty or white space
	
	s = PyString(sText)
	return not len(s.strip())

	if len(s): s = s.replace(xSpace, xMute)
	if len(s): s = s.replace(vbTab, xMute)
	if len(s): s = s.replace(vbCr, xMute)
	if len(s): s = s.replace(vbLf, xMute)
	if len(s): s = s.replace(vbFormFeed, xMute)
	if len(s): s = s.replace(vbVerticalTab, xMute)
	
	if not len(s): return True
	else: return False
	# PyIsBlank method
	
def PyIsMute(o):
	# test for empty string
	
	return isinstance(o, str) and not len(o)

	if typeof(o) != 'string': return False
	if o == vbFormFeed: return True
	else: return False
	# PyIsMute method
	
def PyIsNone(o):
	# Test for None
	
	return o == None
	# PyIsNone method
	
def PyIsObject(o):
	# Test for object
	
	return not (isinstance(o, bool) or isinstance(o, dict) or isinstance(o, int) or isinstance(o, list) or isinstance(o, str) or isinstance(o, unicode) or o is None)

	# return (typeof(o) == 'object') and (o != None)
	# PyIsObject method
	
def PyIsUndefined(o):
	# Test whether object is undefined
	
	return PyIsNone(o)

	return typeof(o) == 'undefined'
	# PyIsUndefined method
	
def PyMin(i, j):
	# Return minimum of two numbers
	
	return min(i, j)

	if i <= j: return i
	return j
	# PyMin method
	
def JSNumber(o):
	# Convert to Python number
	
	try: return float(o)
	except: return 0

	nReturn = 0
	try: nReturn = 0 + o
	except: pass
	
	return nReturn
	# JSNumber method
	
def PyObjectToVt(oValue):
	# Convert Python object to variant
	
	sType = typeof(oValue)
	# if sType != 'object' or oValue == None: return oValue
	if oValue is None or not isinstance(oValue, dict): return oValue
	# elif typeof(oValue.length) == 'number':
	elif isinstance(oValue, list) or isinstance(oValue, tuple):
		a = VtCreateList()
		for i in range(len(oValue)):
			oItem = oValue[i]
			a.Add(PyObjectToVt(oItem))
		return a

	else:
		d = VtCreateDictionary()
		for sAttribute in oValue.keys():
			oItem = oValue[sAttribute]
			d.Add(sAttribute, PyObjectToVt(oItem))
		return d
	# PyObjectToVt method
	
def PyPrint(o):
	# Print to screen using console mode of Windows Script Host
	
	# return WScript.Echo(o)
	# return WriteLine(o)
	print o
	# PyPrint method
	
def PyPrintObject(sName, oValue):
	# Print type of object and its subobjects
	
	sType = typeof(oValue)
	# if sType != 'object' or oValue == None: PyPrint(sType + ', ' + sName + ', ' + oValue)
	if oValue is None or not PyIsObject(oValue): PyPrint(sType + ', ' + sName + ', ' + oValue)
	# elif typeof(oValue.length) == 'number': 
	elif isinstance(oValue, list) or isinstance(oValue, tuple): 
		PyPrint('array, ' + sName)
		for i in range(oValue.length):
			oItem = oValue[i]
			PyPrintObject(sName + '.' + i, oItem)
	else:
		PyPrint('object, ' + sName)
		for sAttribute in oValue.keys():
			oItem = oValue[sAttribute]
			PyPrintObject(sName + '.' + sAttribute, oItem)
	# PyPrintObject method
	
def PySplit(sText, sDelimiter):
	# Split but ensure an empty array for empty text
	
	if not sText: return []
	return sText.split(sDelimiter)
	# PySplit method
	
def PySplitLines(sText):
	# PySplit by line feeds after chomping
	
	sText = StringConvertToUnixLineBreak(sText)
	sText = StringChomp(sText)
	return PySplit(sText, '\n')
	# PySplitLines method
	
def PyString(o):
	# Convert to a Python string
	
	try: return str(o)
	except: return xMute

	sReturn = xMute
	try: sReturn = xMute + o
	except: pass

	if not sReturn: sReturn = xMute
	return sReturn
	# PyString method
	
def PyToVt(sJs):
	# Convert string in JavaScript Object Notation to COM exchange format
	
	sPy = simplejson.loads(sJs)
	sPy = '(' + sPy + ')'
	oValue = eval(sPy)
	return PyObjectToVt(oValue)
	# PyToVt method
	
def PathCombine(sFolder, sName):
	# Combine folder and name to form a valid path
	
	sReturn = sFolder + '\\' + sName
	sReturn = sReturn.replace('\\\\', '\\')
	return sReturn
	# PathCombine method
	
def PathCreateTempFolder():
	# Create temporary folder and return its full path
	
	sFolder = PathCombine(PathGetTempFolder(), PathGetTempName())
	if FolderCreate(sFolder): return sFolder
	else: return xMute
	# PathCreateTempFolder method
	
def PathExists(sPath):
	# Test whether path exists
	
	oSystem =VtCreateFileSystemObject()
	bReturn =oSystem.FolderExists(sPath) or oSystem.FileExists(sPath)
	
	oSystem = None
	return bReturn
	# PathExists method
	
def PathGetBase(sPath):
	# Get base/root name of a file or folder
	
	oSystem =VtCreateFileSystemObject()
	sReturn =oSystem.GetBaseName(sPath)
	
	oSystem = None
	return sReturn
	# PathGetBase method
	
def PathGetCurrentDirectory():
	# Get current directory of active process
	
	oShell =VtCreateWScriptShell()
	sReturn =oShell.CurrentDirectory
	
	oShell = None
	return sReturn
	# PathGetCurrentDirectory method
	
def PathGetExtension(sPath):
	# Get extention of file or folder
	
	oSystem =VtCreateFileSystemObject()
	sReturn =oSystem.GetExtensionName(sPath)
	
	oSystem = None
	return sReturn
	# PathGetExtension method
	
def PathGetFolder(sPath):
	# Get the parent folder of a file or folder
	
	oSystem =VtCreateFileSystemObject()
	sReturn =oSystem.GetParentFolderName(sPath)
	
	oSystem = None
	return sReturn
	# PathGetFolder method
	
def PathGetInternetCacheFolder():
	# Get Windows folder for temporary Internet files
	
	iTemporaryInternetFiles = 32
	oShell = VtCreateShellApplication()
	oFolder = oShell.Namespace(iTemporaryInternetFiles)
	oItem = oFolder.Self
	sReturn = oItem.Path
	
	oItem = None
	oFolder = None
	oShell = None
	return sReturn
	# GetInternetCacheFolder method
	
def PathGetLong(sPath):
	# Get long name of file or folder
	
	if not FileExists(sPath): return sPath
	oShell = VtCreateWScriptShell()
	sFile = PathGetTempFile()
	sFile = StringChopRight(sFile, 3) + 'lnk'
	oShortcut = oShell.CreateShortcut(sFile)
	oShortcut.TargetPath = sPath
	sReturn = oShortcut.TargetPath
	
	oShortcut = None
	oShell = None
	FileDelete(sFile)
	return sReturn
	# PathGetLong method
	
def PathGetName(sPath):
	# Get the file or folder name at the end of a path
	
	oSystem =VtCreateFileSystemObject()
	sReturn =oSystem.GetFileName(sPath)
	
	oSystem = None
	return sReturn
	# PathGetName method
	
def PathGetShort(sPath):
	# Get short path (8.3 style) of a file or folder
	
	if not FileExists(sPath): return sPath
	oSystem =VtCreateFileSystemObject()
	if FolderExists(sPath): 
		oFolder =oSystem.GetFolder(sPath)
		sReturn =oFolder.ShortPath
	else:
		oFile =oSystem.GetFile(sPath)
		sReturn =oFile.ShortPath
	
	oFile = None
	oFolder = None
	oSystem = None
	return sReturn
	# PathGetShort method
	
def PathGetSpec(sDir, sWildcards, sFlags):
	# Get a list of paths, specifying folder, wild card pattern, and sort order
	
	lReturn = VtCreateList()
	iWindowStyle = 0 # hidden
	bWait = True
	sCommand = '%COMSPEC% /c dir /b ' + sFlags + ' ' + '"' + sDir + '\\' + sWildcards + '"'
	sTempFile = PathGetTempFile()
	sCommand = sCommand + ' >' + sTempFile
	ShellRun(sCommand, iWindowStyle, bWait)
	sReturn = StringTrim(FileToString(sTempFile))
	FileDelete(sTempFile)
	aReturn = sReturn.split(vbCrLf)
	for i in range(aReturn.length):
		s = aReturn[i]
	if not StringContains(s, ':', False): s = PathCombine(sDir, s)
	lReturn.Add(s)
	return lReturn
	# PathGetSpec method
	
def PathGetSpecialFolder(sName):
# Get a special folder of Windows

	oShell = VtCreateWScriptShell()
	o = oShell.SpecialFolders
	iCount = o.Length
	i = 0
	while i < iCount:
		sReturn = o[i]
		if StringEndsWith(sReturn, '\\' + sName, True): i = iCount +1
		else: i += 1
	
	o = None
	oShell = None
	return sReturn

def PathGetTempFile():
	# Get full path of a temporary file
	
	return PathGetTempFolder() + '\\' + PathGetTempName()
	# PathGetTempFile method
	
def PathGetTempFolder():
	# Get Windows folder for temporary files
	
	iTempFolder = 2
	oSystem =VtCreateFileSystemObject()
	sReturn =oSystem.GetSpecialFolder(iTempFolder).path
	
	oSystem = None
	return sReturn
	# PathGetTempFolder method
	
def PathGetTempName():
	# Get Name for temporary file or folder
	
	oSystem =VtCreateFileSystemObject()
	sReturn = oSystem.GetTempName()
	
	oSystem = None
	return sReturn
	# PathGetTempName method
	
def PathGetValid(sDir, sBase, sExt, bUnique):
	sIllegal = "&=@%*+\\|':'<>/?" + xQuote
	sViewable = "!#$%&'()*+,-./0123456789:'<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~"
	sPrintable = xSpace + sViewable
	
	sSourceDir = sDir
	sSourceBase = sBase
	sSourceExt = sExt
	if StringLength(sSourceExt) and not StringStartsWith(sSourceExt, '.', False): sSourceExt = '.' + sSourceExt
	# sLine = sSourceBase
	sLine = sBase
	iCount = StringLength(sIllegal)
	for i in range(iCount):
		s = sIllegal[i : i + 1]
	# if StringContains(sLine, s, False)) sLine = sLine.replace(s, xSpace:
		if StringContains(sLine, s, False): sLine = StringReplace(sLine, s, xSpace, False)
	
	# sLine = StringReplaceAll(sLine, '  ', xSpace)
	# sLine = StringReplaceAll(sLine, '  ', '_')
		sLine = StringTrim(sLine)
	# sLine = RegExpReplace(sLine, ' +', '_', False)
		sLine = RegExpReplace(sLine, ' +', ' ', False)
	
	sTargetBase = sLine
	sTargetFile = sSourceDir + '\\' + sTargetBase + sSourceExt
	if bUnique and FileExists(sTargetFile): 
		s = '_01'
		sTargetFile = sSourceDir + '\\' + sTargetBase + s + sSourceExt
		i = 1
	while FileExists(sTargetFile) and i <= 99: 
		if i < 10: s = '_0' + str(i)
		else: s = '_' + str(i)
	
		sTargetFile = sSourceDir + '\\' + sTargetBase + s + sSourceExt
		i += 1

	return sTargetFile
	# PathGetValid method
	
def PathSetCurrentDirectory(sDir):
	# Set current directory of active process, and return previously current directory
	
	oShell =VtCreateWScriptShell()
	sReturn = oShell.CurrentDirectory
	oShell.CurrentDirectory = sDir
	
	oShell = None
	return sReturn
	# PathSetCurrentDirectory method
	
def RegExpContains(sText, sMatch, bIgnoreCase=False):
	"""
	Get Array containing the starting index and text of the first match of a regular expression
	where sText is the string to search
	sMatch is the regular expression to match
	bIgnoreCase indicates whether capitalization matters
	"""
	
	lReturn = VtCreateList()
	oExp = VtCreateRegExp()
	oExp.Pattern = sMatch
	oExp.Ignorecase = bIgnoreCase
	oExp.Multiline = False
	oExp.Global = False
	
	oMatches = oExp.Execute(sText)
	iCount = oMatches.Count
	if iCount: 
		oMatch = oMatches.Item(0)
		iIndex = oMatch.FirstIndex
		sValue = oMatch.Value
		lReturn.Add(iIndex)
		lReturn.Add(sValue)
	
	oMatch = None
	oMatches = None
	oExp = None
	return lReturn
	# RegExpContains method
	
def RegExpContainsLast(sText, sMatch, bIgnoreCase=False):
	"""
	Get list containing the starting index and text of the last match of a regular expression
	where sText is the string to search
	sMatch is the regular expression to match
	bIgnoreCase indicates whether capitalization matters
	"""
	
	lReturn = VtCreateList()
	oExp = VtCreateRegExp()
	oExp.Pattern = sMatch
	oExp.Ignorecase = bIgnoreCase
	oExp.Multiline = False
	oExp.Global = True
	
	oMatches = oExp.Execute(sText)
	iCount = oMatches.Count
	if iCount: 
		oMatch = oMatches.Item(iCount - 1)
		iIndex = oMatch.FirstIndex
		sValue = oMatch.Value
		lReturn.Add(iIndex, sValue)
	
	oMatch = None
	oMatches = None
	oExp = None
	return lReturn
	# RegExpContainsLast method
	
def RegExpCount(sText, sMatch, bIgnoreCase=False):
	"""
	Count matches of a regular expression
	where sText is the string to search
	sMatch is the regular expression to match
	bIgnoreCase indicates whether capitalization matters
	"""
	
	oExp = VtCreateRegExp()
	oExp.Pattern = sMatch
	oExp.Ignorecase = bIgnoreCase
	oExp.Multiline = False
	oExp.Global = True
	
	oMatches = oExp.Execute(sText)
	iReturn = oMatches.Count
	
	oMatches = None
	oExp = None
	return iReturn
	# RegExpCount method
	
def RegExpExtract(sText, sMatch, bIgnoreCase=False):
	"""
	Get list containing matches of a regular expression
	where sText is the string to search
	sMatch is the regular expression to match
	bIgnoreCase indicates whether capitalization matters
	"""
	
	lReturn = VtCreateList()
	oExp = VtCreateRegExp()
	oExp.Pattern = sMatch
	oExp.Ignorecase = bIgnoreCase
	oExp.Multiline = False
	oExp.Global = True
	
	oMatches = oExp.Execute(sText)
	iCount = oMatches.Count
	for i in range(iCount):
		oMatch = oMatches.Item(i)
		lReturn.Add(oMatch.Value)
	
	oMatch = None
	oMatches = None
	oExp = None
	return lReturn
	# RegExpExtract method
	
def RegExpReplace(sText, sMatch, sReplace, bIgnoreCase=False):
	"""
	Replace text matching a regular expression
	where sText is the string to search
	sMatch is the regular expression to match
	sReplace is the replacement text
	bIgnoreCase indicates whether capitalization matters
	"""
	
	oExp = VtCreateRegExp()
	oExp.Pattern = sMatch
	oExp.Ignorecase = bIgnoreCase
	oExp.Multiline = False
	# oExp.Multiline = True
	oExp.Global = True
	
	sReturn = oExp.replace(sText, sReplace)
	
	oExp = None
	return sReturn
	# RegExpReplace method
	
def RegExpTest(sText, sMatch, bIgnoreCase=False):
	"""
	Test match of a regular expression
	where sText is the string to search
	sMatch is the regular expression to match
	bIgnoreCase indicates whether capitalization matters
	"""
	
	oExp = VtCreateRegExp()
	oExp.Pattern = sMatch
	oExp.Ignorecase = bIgnoreCase
	oExp.Multiline = False
	oExp.Global = True
	
	bReturn = oExp.Test(sText)
	
	oExp = None
	return bReturn
	# RegExpTest method
	
def ShellCreateShortcut(sFile, sTargetPath, sWorkingDirectory, iWindowStyle, sHotkey):
	# Create a .lnk or .url file
	
	if not FileDelete(sFile): return False
	
	oShell = VtCreateWScriptShell()
	oShortcut = oShell.CreateShortcut(sFile)
	oShortcut.TargetPath = sTargetPath
	oShortcut.WorkingDirectory = sWorkingDirectory
	oShortcut.WindowStyle = iWindowStyle
	oShortcut.Hotkey = sHotkey
	oShortcut.Save()
	
	oShortcut = None
	oShell = None
	return FileExists(sFile)
	# ShellCreateShortcut method
	
def ShellExec(sCommand):
	# Run a console mode command and return its standard output
	
	oShell =VtCreateWScriptShell()
	oExec =oShell.Exec(sCommand)
	while  oExec.Status ==0:
		VtSleep(10)
	
	oOutput =oExec.StdOut
	sReturn =oOutput.ReadAll()
	oExec.Terminate()
	
	oOutput = None
	oExec = None
	oShell = None
	return sReturn
	# ShellExec method
	
def ShellExecute(sFile, sParams, sFolder, sVerb, iWindowStyle):
	# Execute a command with a verb like RunAs
	
	bReturn = False
	oShell = VtCreateShellApplication()
	try: 
		oShell.ShellExecute(sFile, sParams, sFolder, sVerb, iWindowStyle)
		bReturn = True
	except: pass

	return bReturn
	# ShellExecute method
	
def ShellExpandEnvironmentVariables(sText):
	# Replace environment variables with their values
	
	oShell =VtCreateWScriptShell()
	sReturn =oShell.ExpandEnvironmentStrings(sText)
	
	oShell = None
	return sReturn
	# ShellExpandEnvironmentVariables method
	
def ShellGetEnvironmentVariable(sVariable):
	# Get the value of an environment variable
	
	oShell =VtCreateWScriptShell()
	oEnv =oShell.Environment
	sReturn =oEnv.Item(sVariable)
	
	oEnv = None
	oShell = None
	return sReturn
	# ShellGetEnvironmentVariable method
	
def ShellGetShortcutTargetPath(sFile):
	# Get the target path of a shortcut file
	
	oShell = VtCreateWScriptShell()
	oShortcut = oShell.CreateShortcut(sFile)
	sReturn = oShortcut.TargetPath
	oShortcut = None
	oShell = None
	return sReturn
	# ShellGetShortcutTargetPath method
	
def ShellGetSpecialFolder(vFolder):
	# Get a special Windows folder
	
	oShell = VtCreateShellApplication()
	oNamespace = oShell.Namespace(vFolder)
	oFolder = oNamespace.Self
	sReturn = oFolder.Path
	oFolder = None
	oNamespace = None
	oShell = None
	return sReturn
	# ShellGetSpecialFolder method
	
def ShellInvokeVerb(sPath, sVerb):
	# Invoke a verb on a file or folder
	
	sFolder = PathGetFolder(sPath)
	sName = PathGetName(sPath)
	oShell = VtCreateShellApplication()
	oFolder = oShell.Namespace(sFolder)
	oName = oFolder.ParseName(sName)
	bReturn = oName.InvokeVerb(sVerb)
	
	oName = None
	oFolder = None
	oShell = None
	return bReturn
	# ShellInvokeVerb method
	
def ShellOpen(sPath):
	# Open a file or folder with the default program associated with its type
	
	return ShellRun(StringQuote(sPath), 1, False)
	# ShellOpen method
	
def ShellOpenWith(sExe, sParam):
	# Open a program with a file
	
	return ShellRun(StringQuote(sExe) + ' ' + StringQuote(sParam), 1, False)
	# ShellRunWith method
	
def ShellRun(sFile, iStyle, bWait):
	"""
	Launch a program or file, indicating its window style and whether to wait before returning
	window styles:
	0 Hides the window and activates another window
	1 Activates and displays a window. If the window is minimized or maximized, the
	system restores it to its original size and position. This flag should be used
	when specifying an application for the first time
	2 Activates the window and displays it minimized
	3 Activates the window and displays it maximized
	4 Displays a window in its most recent size and position. The active window
	remains active
	5 Activates the window and displays it in its current size and position
	6 Minimizes the specified window and activates the next top-level window in the Z
	order
	7 Displays the window as a minimized window. The active window remains active
	8 Displays the window in its current state. The active window remains active
	9 Activates and displays the window. If it is minimized or maximized, the system
	restores it to its original size and position. An application should specify
	this flag when restoring a minimized window
	10 Sets the show state based on the state of the program that started the
	application
	"""
	
	oShell =VtCreateWScriptShell()
	iReturn = -2
	try: iReturn =oShell.Run(sFile, iStyle, bWait)
	except: pass

	
	oShell = None
	return iReturn
	# ShellRun method
	
def ShellRunCommandPrompt(sDir):
	# Open a command prompt in the directory specified
	
	return ShellRun('%COMSPEC% /k cd ' + StringQuote(sDir), 1, False)
	# ShellRunCommandPrompt method
	
def ShellRunExplorerWindow(sDir):
	# Open Windows Explorer in the directory specified
	
	return ShellOpen(sDir)
	# ShellRunExplorerWindow method
	
def ShellWait(sPath):
	# Run a program and wait for it to return
	
	return ShellRun(StringQuote(sPath), 0, True)
	# ShellWait method
	
def StringAppendToFile(sText, sFile, sDivider=xSectionBreak):
	# Append string to File, omitting divider if the first one
	
	sBody = xMute
	if FileExists(sFile): sBody = FileToString(sFile)
	if sBody: sBody += sDivider
	sBody += sText
	StringToFile(sBody, sFile)
	return FileExists(sFile)
	# StringAppendToFile method
	
def StringChomp(sText):
	# Chop \n for both ends of a string
	
	sReturn = sText
	while len(sReturn):
		if not StringStartsWith(sReturn, '\n', False): break
		sReturn = StringChopLeft(sReturn, 1)

	while len(sReturn):
		if not StringEndsWith(sReturn, '\n', False): break
		sReturn = StringChopRight(sReturn, 1)

	return sReturn
	# StringChomp method
	
def StringChopLeft(sText, iCount):
	# Remove iCount characters from left of sText
	
	iIndex = PyMin(iCount, len(sText))
	return sText[iIndex:]
	return sText.slice(iIndex)
	# return sText.Substring(iIndex)
	# StringChopLeft method
	
def StringChopRight(sText, iCount):
	# Remove iCount characters from Right of sText
	
	iIndex = PyMin(iCount, len(sText)) * -1
	return sText[0:iIndex]
	return sText.slice(0, iIndex)
	# StringChopRight method
	
def StringContains(sText, sMatch, bIgnoreCase=False):
	# Test if a string is contained in another
	
	if bIgnoreCase: 
		sText = sText.lower()
		sMatch = sMatch.lower()
	
	return sText.find(sMatch) >= 0
	# StringContains method
	
def StringConvertToMacLineBreak(sText):
	# Convert to Macintosh line break, \r
	
	sReturn = sText
	sReturn = StringReplace(sReturn, vbCrLf, vbCr, False)
	sReturn = StringReplace(sReturn, vbLf, vbCr, False)
	return sReturn
	# StringConvertToMacLineBreak method
	
def StringConvertToUnixLineBreak(sText):
	# Convert to Unix line break, \n
	
	sReturn = sText
	sReturn = sReturn.replace(vbCrLf, vbLf)
	sReturn = sReturn.replace(vbCr, vbLf)
	return sReturn
	# StringConvertToUnixLineBreak method
	
def StringConvertToWinLineBreak(sText):
	# Convert to standard Windows line break, \r\n
	
	sReturn = sText
	sReturn = sReturn.replace(vbCrLf, vbLf)
	sReturn = sReturn.replace(vbCr, vbLf)
	sReturn = sReturn.replace(vbLf, vbCrLf)
	return sReturn
	# StringConvertToWinLineBreak method
	
def StringCount(sText, sMatch):
	# Count occurrences of a string within another string
	
	iLength = len(sText)
	s = sText.replace(sMatch, xMute)
	iDelta = iLength - len(s)
	iReturn = iDelta / len(sMatch)
	return iReturn
	# StringCount method
	
def StringEndsWith(sText, sSuffix, bIgnoreCase=False):
	# Test whether first string ends with second one
	
	if bIgnoreCase: 
		sText = sText.lower()
		sSuffix = sSuffix.lower()
	
	return sText.endswith(sSuffix)

	iText = len(sText)
	iSuffix = len(sSuffix)
	if iSuffix > iText: return False
	return sText[-iSuffix:] == sSuffix
	return sText.slice(-iSuffix) == sSuffix
	# StringEndsWith method
	
def StringEqual(s1, s2):
	# Test if two strings are exactly equal
	
	return s1 == s2
	# StringEqual method
	
def StringEquiv(s1, s2):
	# Test whether two strings are the same except for capitalization
	
	return s1.lower() == s2.lower()
	# StringEquiv method
	
def StringGetASCII(sText):
	# Get space delimited ASCII codes for characters in string
	
	sReturn = xMute
	iCount = len(sText)
	for i in range(iCount):
		s = sText[i, i + 1]
		if len(sReturn): sReturn += xSpace
		sReturn += ord(s)

	return sReturn
	# StringGetASCII method
	
def StringLeft(sText, iCount):
	# Return leftmost characters of a string
	
	return sText[0:iCount]
	# StringLeft method
	
def StringLength(sText):
	# Return length of a string
	
	return len(sText)
	# StringLength method
	
def StringLower(sText):
	# Convert string to lower case
	
	return sText.lower()
	# StringLower method
	
def StringPlural(sItem, iCount):
	# Return singular or plural form of a string, depending on whether count equals one
	
	sReturn = PyString(iCount) + ' ' + sItem
	if iCount != 1: sReturn += 's'
	return sReturn
	# StringCount method
	
def StringQuote(sText):
	# Quote a string
	
	return '"' + sText + '"'
	# StringQuote method
	
def StringReplace(sText, sMatch, sReplace, bIgnoreCase=False):
# Replace matching text

	iMatch =StringLength(sMatch)
	sReturn =sText
	sRest =sReturn
	iRest =StringLength(sRest)
	
	bLoop =True
	while bLoop:
		iReturn =StringLength(sReturn)
		iRest =StringLength(sRest)
		iPre =iReturn -iRest
		if bIgnoreCase: iFound =StringContains(StringLower(sRest), StringLower(sMatch))
		else: iFound =StringContains(sRest, sMatch)
		
		if iFound:
			sRest =sRest[iFound + iMatch:]
			sReturn = sReturn[:iPre + iFound - 1] + sReplace + sRest
		else: bLoop =False
	
	return sReturn
	
	
def StringReplaceAll(sText, sMatch, sReplace, bIgnoreCase=False):
	# Replace all occurrences of a string within another
	
	if StringContains(sReplace, sMatch, bIgnoreCase): return StringReplace(sText, sMatch, sReplace, bIgnoreCase)
	
	sReturn = sText
	while  StringContains(sReturn, sMatch, bIgnoreCase):
		sReturn = StringReplace(sReturn, sMatch, sReplace, bIgnoreCase)

	return sReturn
	# StringReplaceAll method
	
def StringRight(sText, iCount):
	# Return rightmost characters of a string
	
	iLength = len(sText)
	iPosition = iLength - iCount
	return sText[iPosition : ]
	# StringRight method
	
def StringSlice(sText, iStart, iEnd):
	return sText[iStart, iEnd]
	# StringSlice method
	
def StringStartsWith(sText, sPrefix, bIgnoreCase=False):
	# Test whether first string starts with second one
	
	if bIgnoreCase:
		sText = sText.lower()
		sPrefix = sPrefix.lower()
	
	return sText.startswith(sPrefix)

	iText = len(sText)
	iPrefix = len(sPrefix)
	if iPrefix > iText: return False
	
	return sText[0:iPrefix] == sPrefix
	return sText.slice(0, iPrefix) == sPrefix
	# StringStartsWith method
	
def StringToFile(sText, sFile):
	# Saves string to text file, replacing if it exists
	
	if not FileDelete(sFile): return False
	
	sText = StrDefault(sText)
	bReplace = True
	bUnicode = False
	oSystem =VtCreateFileSystemObject()
	oFile =oSystem.CreateTextFile(sFile, bReplace, bUnicode)
	try: 
		oFile.Write(sText)
		oFile.Close()
	except: pass

	oFile = None
	oSystem = None
	return FileExists(sFile)
	# StringToFile method
	
def StringToList(sText, sDelimiter):
	# Convert string with specified delimiter to list
	
	lReturn = VtCreateList()
	aParts = sText.split(sDelimiter)
	for i in range(len(aParts)):
		sPart = aParts[i]
		lReturn.Add(sPart)

	return lReturn
	# StringToList method
	
def StringTrim(sText):
	# Trim white space from both ends of a string
	
	return sText.strip()

	# return sText.replace(/(^\s*)|(\s*$)/g/, '')
	sText = RegExpReplace(sText, '^\\s+', xMute, False)
	sText = RegExpReplace(sText, '\\s+$', xMute, False)
	return sText
	# StringTrim method
	
def StringTrimLeft(sText):
	# Trim white space from left end of a string
	
	return sText.lstrip()

	return RegExpReplace(sText, '^\\s+', xMute, False)
	# StringTrimLeft method
	
def StringTrimRight(sText):
	# Trim white space from right end of a string
	
	return sText.rstrip()

	return RegExpReplace(sText, '\\s+$', xMute, False)
	# StringTrimRight method
	
def StringUnquote(sText):
	# Unquote a string
	
	return sText.strip('"')

	sReturn = sText
	while  StringLeft(sReturn, 1) == '"':
		sReturn = StringChopLeft(sReturn, 1)
	
	while StringRight(sReturn, 1) == '"':
		sReturn = StringChopRight(sReturn, 1)

	return sReturn
	# StringUnquote method
	
def StringUpper(sText):
	# Convert string to upper case
	
	return sText.upper()
	# StringUpper method
	
def StringWrap(sText, iMaxLine):
	# Wrap text

	return WrapText(sText, iMaxLine)

	aLines = sText.split(vbCrLf)
	sReturn = xMute
	iCount = aLines.length
	for i in range(iCount):
		sLine = aLines[i]
		if len(sLine) > iMaxLine:
			aWords = sLine.split(' ')
			sLine = xMute
			for j in range(aWords.length):
				sWord = aWords(j)
				if len(sLine) + len(sWord) > iMaxLine: 
					sReturn += StringTrim(sLine) + vbCrLf
					sLine = sWord + ' '
				else:
					sLine = sLine + sWord + ' '
		else:
			sReturn += StringTrimRight(sLine) + vbCrLf

	return sReturn
	# StringWrap method
	
def typeof(o): return VtTypeName(o)

def UrlCreate(sProtocol, sHost, sPath, sQuery):
	# Create a URL from components
	
	sReturn = xMute
	if not sProtocol: sProtocol = 'http'
	sReturn += sProtocol + '://'
	if sHost: sReturn += sHost
	if sPath: sReturn += '/' + sPath
	if sQuery: sReturn += '?' + sQuery
	return sReturn
	# UrlCreate method
	
def UrlGetBaseDomain(sUrl):
	# Get base domain of a URL
	
	sReturn = xMute
	s = UrlGetHost(sUrl)
	a = s.split('.')
	iLength = len(a)
	if iLength >= 2: s = a[iLength -2] + '.' + a[iLength - 1]
	if iLength >= 3 and len(a[-1]) == 2: s = a[-3] + '.' + s
	if s.find('.') >= 0: sReturn = s
	return sReturn
	# UrlGetBaseDomain method
	
def UrlGetExtension(sUrl):
	# Get extension of a URL
	
	sReturn = UrlGetFileName(sUrl)
	if sReturn: sReturn = PathGetExtension(sReturn)
	return sReturn
	# UrlGetExtension method
	
def UrlGetFileName(sUrl):
	# Get file of a URL
	
	sReturn = UrlGetPath(sUrl)
	if sReturn: sReturn = PathGetName(sReturn)
	return sReturn
	# UrlGetFileName method
	
def UrlGetFolder(sUrl):
	# Get folder of a URL
	
	sReturn = UrlGetPath(sUrl)
	# if sReturn) sReturn = PathGetFolder(sReturn:
	if sReturn and not StringEndsWith(sReturn, '/'): sReturn = PathGetFolder(sReturn)
	return sReturn
	# UrlGetFolder method
	
def UrlGetFragment(sUrl):
	# Get fragment of a URL
	
	sReturn = xMute
	i = sUrl.find('#')
	# if i >= 0: sReturn = sUrl.slice(i + 1)
	if i >= 0: sReturn = sUrl[i + 1:]
	return sReturn
	# UrlGetFragment method
	
def UrlGetFullFolder(sUrl):
	# Get full url tup to and including folder
	
	# ShellOpen('SayLine.exe', 'full folder')
	sPrepath = UrlGetPrePath(sUrl)
	# sPrepath = ''
	# DialogShow('prepath', sPrepath)
	sFolder = UrlGetFolder(sUrl)
	# DialogShow('folder', sFolder)
	sReturn = sPrepath + sFolder
	return sReturn
	# UrlGetFullFolder method
	
def UrlGetHost(sUrl):
	# Get subdomain and domain of a URL
	
	sReturn = sUrl
	i = sReturn.find('://')
	# if i >= 0: sReturn = sReturn.slice(i + 3)
	if i >= 0: sReturn = sReturn[i + 3:]
	# if StringStartsWith(sReturn, 'www.', True)) sReturn = StringChopLeft(sReturn, 4:
	i = sReturn.find('/')
	# if i >= 0: sReturn = sReturn.slice(0, i)
	if i >= 0: sReturn = sReturn[0:i]
	if sReturn.find('.') == -1: sReturn = xMute
	return sReturn
	# UrlGetHost method
	
def UrlGetPath(sUrl):
	# Get path of a URL
	
	sReturn = UrlGetPathPlus(sUrl)
	sFragment = UrlGetFragment(sReturn)
	if sFragment: sReturn = StringChopRight(sReturn, len(sFragment) + 1)
	sQuery = UrlGetQuery(sReturn)
	if sQuery: sReturn = StringChopRight(sReturn, len(sQuery) + 1)
	return sReturn
	# UrlGetPath method
	
def UrlGetPathPlus(sUrl):
	# Get path and fragment or query of a URL
	
	sReturn = xMute
	s = sUrl
	sProtocol = UrlGetProtocol(s)
	if sProtocol: s = StringChopLeft(s, len(sProtocol) + 3)
	sHost = UrlGetHost(s)
	if sHost: s = StringChopLeft(s, len(sHost) + 1)
	sReturn = s
	return sReturn
	# UrlGetPathPlus method
	
def UrlGetPrePath(sUrl):
	# Get part of URL before path
	
	# ShellOpenWith('SayLine.exe', 'prepath')
	sReturn = xMute
	sReturn = sUrl
	sPathPlus = UrlGetPathPlus(sUrl)
	# ShellOpenWith('SayLine.exe', 'PathPlus: ' + sPathPlus)
	if sPathPlus: sReturn = StringChopRight(sUrl, len(sPathPlus))
	# ShellOpenWith('SayLine.exe', 'Prepath: ' + sReturn)
	return sReturn
	# UrlGetPrePath method
	
def UrlGetProtocol(sUrl):
	# Get protocol of URL
	
	sReturn = xMute
	i = sUrl.find('://')
	if i >= 0: sReturn = sUrl[0:i]
	return sReturn
	# UrlGetProtocol method
	
def UrlGetQuery(sUrl):
	# Get query of URL
	
	sReturn = xMute
	i = sUrl.find('?')
	# if i >= 0: sReturn = sUrl.slice(i + 1)
	if i >= 0: sReturn = sUrl[i + 1:]
	return sReturn
	# UrlGetQuery method
	
def UrlGetSubdomain(sUrl):
	# Get subdomain of a URL
	
	sReturn = xMute
	s = UrlGetHost(sUrl)
	a = s.split('.')
	for i in range(len(a) - 2):
		if i: sReturn += '.'
		sReturn += a[i]

	return sReturn
	# UrlGetSubdomain method
	
def UrlNormalize(sUrl, sBase):
	# Normalize a URL for comparison
	
	# ShellOpenWith('SayLine.exe', 'normalize')
	sReturn = sUrl
	if not sBase: sBase = xMute
	sBase = UrlGetFullFolder(sBase)
	# DialogShow('base', sBase)
	
	# return sReturn
	if StringEqual(StringLeft(sReturn, 2), '//'): sReturn = 'http:' + sReturn
	# DialogShow(sReturn, sBase)
	sProtocol = UrlGetProtocol(sReturn)
	# DialogShow('protocol', sProtocol)
	# If (sUrl.lower().find('aboutus') >= 0) DialogShow('base', sBase)
	if not sProtocol: 
	# DialogShow(StringLeft(sReturn, 2), '')
		iIndex = sReturn.find(':')
		if iIndex >= 0: sReturn = StringChopLeft(sReturn, iIndex + 1)
	
	if not sProtocol and sBase: 
		if StringStartsWith(sReturn, '/', False): sBase = UrlGetPrePath(sBase)
		if StringEndsWith(sBase, '/', False) and StringStartsWith(sReturn, '/', False): 
			sReturn = StringChopRight(sBase, 1) + sReturn
		elif not StringEndsWith(sBase, '/', False) and not StringStartsWith(sReturn, '/', False):
			sReturn = sBase + '/' + sReturn
		else:
			sReturn = sBase + sReturn
	
	sProtocol = UrlGetProtocol(sReturn)
	if not sProtocol: sReturn = 'http://' + sReturn
	
	sPrePath = UrlGetPrePath(sReturn)
	if sReturn == sPrePath and not StringEndsWith(sPrePath, '/', False): sReturn += '/'
	
	sFragment = UrlGetFragment(sReturn)
	if sFragment: sReturn = StringChopRight(sReturn, len(sFragment) + 1)
	return sReturn
	# UrlNormalize method
	
def VtArrayToPy(a):
	# Convert variant array to Python
	
	# return (new VBArray(a)).toArray()
	# return VBArray(a).toArray()
	return a
	# VtArrayToPy method
	
def VtArrayToList(a):
	# Convert variant array to list
	
	aPy = VtArrayToPy(a)
	lReturn = VtCreateList()
	for i in range(aPy.length): lReturn.Add(aPy[i])
	return lReturn
	# VtArrayToList method
	
def VtCopyDictionary(dItems):
	# Copy a dictionary
	
	dReturn = VtCreateDictionary()
	aKeys = VtDictionaryKeysToPy(dItems)
	for i in range(aKeys.length):
		sKey = aKeys[i]
		# dReturn.Item(sKey) = dItems.Item(sKey)
		dReturn.Add(sKey, dItems(sKey))

	return dReturn
	# VtCopyDictionary method
	
def VtCopyList(lItems):
	# Copy a list
	
	lReturn = VtCreateList()
	for i in range(lItems.Count):
		sItem = lItems(i)
		lReturn.Add(sItem)

	return lReturn
	# VtCopyList method
	
def VtCreateDictionary():
	# Return a variant dictionary
	
	return win32com.client.dynamic.Dispatch('Scripting.Dictionary')
	# VtCreateDictionary method
	
def VtCreateFileSystemObject():
	# Return a file system object
	
	return win32com.client.dynamic.Dispatch('Scripting.FileSystemObject')
	# return comtypes.client.CreateObject('Scripting.FileSystemObject', dynamic=True)
	# VtCreateFileSystemObject
	
def VtCreateHtmlFile():
	# Create a variant HTMLFile object
	
	return win32com.client.dynamic.Dispatch('HTMLFile')
	# CreateHtmlFile method
	
def VtCreateInternetExplorerApplication():
	# Return an Internet Explorer object
	
	return win32com.client.dynamic.Dispatch('InternetExplorer.Application')
	# VtCreateInternetExplorerApplication method
	
def VtCreateList():
	# Create a variant list object
	
	return win32com.client.dynamic.Dispatch('System.Collections.ArrayList')
	# VtCreateList method
	
def VtCreateRecordSet():
	# Create a variant record set object
	
	oRs = win32com.client.dynamic.Dispatch('ADODB.RecordSet')
	oRs.CursorLocation = adUseClient
	return oRs
	# VtCreateRecordSet method
	
def VtCreateRegExp():
	# Return a scripting RegExp object
	
	return win32com.client.dynamic.Dispatch('VBScript.RegExp')
	# VtCreateRegExp method
	
def VtCreateShellApplication():
	# Return a Shell.Application object
	
	return win32com.client.dynamic.Dispatch('Shell.Application')
	# VtCreateShellApplication method
	
def VtCreateStream():
	# Return an ADODB.Stream object
	
	oStream = win32com.client.dynamic.Dispatch('ADODB.Stream')
	oStream.Type = adTypeBinary
	oStream.Open()
	return oStream
	# VtCreateStream method
	
def VtCreateVBScriptControl():
	# Return MSScriptControl.ScriptControl object
	
	oScript = win32com.client.dynamic.Dispatch('MSScriptControl.ScriptControl')
	oScript.Language = 'VBScript'
	return oScript
	# VtCreateVBScriptControl method
	
def VtCreateWebRequest():
	# Create web request object
	
	return win32com.client.dynamic.Dispatch('MSXML2.ServerXMLHTTP')
	
def VtCreateWinHttpRequest():
	# Create WinHttp request object
	
	oRequest = comtypes.client.CreateObject('WinHttp.WinHttpRequest.5.1', dynamic=True)
	# oRequest = win32com.client.dynamic.Dispatch('WinHttp.WinHttpRequest.5.1')
	WinHttpRequestOption_SslErrorIgnoreFlags  = 4
	oRequest.Option[WinHttpRequestOption_SslErrorIgnoreFlags] = 0x3300 # ignore all server certificate errors
	WinHttpRequestOption_EnableHttpsToHttpRedirects = 12
	oRequest.Option[WinHttpRequestOption_EnableHttpsToHttpRedirects  ] = True
	return oRequest
	# VtCreateWinHttpRequest method
	
def VtCreateWScriptShell():
	# Return WScript.Shell object
	
	return win32com.client.dynamic.Dispatch('Wscript.Shell')
	# VtCreateWScriptShell method
	
def VtCreateXmlDocument():
	# Return an XML document
	
	oDoc = comtypes.client.CreateObject('MSXML2.DOMDocument', dynamic=True)
	oDoc.async = False
	oDoc.setProperty('SelectionLanguage', 'XPath')
	# Not necessary, since the following line is the default
	# Set oNode = oDoc.createProcessingInstruction ('xml', 'version=''1.0'' encoding=''UTF-8''')
	# o Doc.appendChild(oNode)
	return oDoc
	# VtCreateXmlDocument method
	
def VtDateToPy(dt):
	# Convert a variant date to Python
	
	return dt

	# return Date.parse(dt)
	# VtDateToPy method
	
def VtDictionaryKeysToPy(d):
	# Convert variant array of dictionary keys to Python array
	
	a = VtArrayToPy(d.Keys())
	return a
	# VtDictionaryKeysToPy method
	
def VtDictionaryKeysToList(d):
	# Convert variant array of dictionary keys to list
	
	l = VtArrayToList(d.Keys())
	return l
	# VtDictionaryKeysToList method
	
def VtDictionaryToPy(d):
	# Convert a variant dictionary to Python
	
	aKeys = d.Keys()
	a = VtArrayToPy(aKeys)
	dReturn = {}
	for i in range(len(a)):
		sKey = a[i]
		dReturn[sKey] = d.Item(sKey)

	return dReturn
	# VtDictionaryToPy method
	
def VtDictionaryToXml(dItems, sFile):
	# Save dictionary to XML file
	
	d = VtDictionaryToPy(dItems)
	for sKey in d.keys():
		sValue = d[sKey]
		XmlSetValue(sFile, sKey, sValue)
	# VtDictionaryToXml method
	
def VtGetWMIObject():
	# Get WMI object
	
	return comtypes.client.GetObject('winmgmts:{impersonationLevel=impersonate}!\\\\.\\root\\default:StdRegProv')
	# VtGetWMIObject method
	
def VtInitDictionary(vValue):
	# Create a variant dictionary and initialize it with a single key/value pair
	
	dReturn = VtCreateDictionary()
	dReturn.Add('v', vValue)
	return dReturn
	# VtInitDictionary method
	
def VtInitXmlDocument(sXml):
	# Create and initialize an Xml document with a string of XML
	
	oDoc = VtCreateXmlDocument()
	oDoc.loadXML(sXml)
	return oDoc
	# VtInitXmlDocument method
	
def VtListFilterByExtension(lPaths, lExtensions):
	# Get a subset from a list of paths that match an extension
	
	lReturn = VtCreateList()
	sExtensions = xMute
	# DialogShow(lPaths.Count.toString(), lExtensions.Count.toString())
	for i in range(lPaths.Count):
	# ShellOpenWith('c:\\mctwit\\sayline.exe', i.toString())
		sPath = lPaths(i)
	# sPath = StringTrim(sPath)
		sExtension = PathGetExtension(sPath)
	# sExtension = StringRight(sPath, 3)
	# if not i) DialogShow(sExtension, len(sExtension).toString():
		if StringStartsWith(sPath, '.', False): sExtension = StringChopLeft(sExtension, 1)
		sExtension = sExtension.lower()
		if lExtensions.Contains(sExtension): lReturn.Add(sPath)
		sExtensions += sPath + ' = ' + sExtension + '\r\n'
	# sExtension = StringTrim(sExtension)
	# if not i: sExtensions = sExtension
	# StringToFile(sExtensions, 'c:\\temp\\temp.txt')
	# ShellOpenWith('c:\\mctwit\\sayline.exe', lReturn.Count.toString())
	# StringToFile(sExtensions, 'c:\\temp\\temp.txt')
	return lReturn
	# VtFilterByExtension method
	
def VtListFilterByRegExp(lItems, sMatch, bIgnoreCase=False):
	# Return list of matches of a regular expression
	
	lReturn = VtCreateList()
	for i in range(lItems.Count):
		sItem = lItems(i)
		if RegExpTest(sItem, sMatch, bIgnoreCase): lReturn.Add(sItem)

	return lReturn
	# VtListFilterByRegExp method
	
def VtListFilterByWildcard(lItems, sMatch):
	# Return list of matches of a wildcard filter expression
	
	lReturn = VtCreateList()
	if not lItems: return lReturn
	
	oRs = VtCreateRecordSet()
	oFields = oRs.Fields
	oFields.Append('Item', adVarWChar, xMaxPath)
	oRs.Open()
	
	for i in range(lItems.Count):
		sItem = lItems(i)
		oRs.AddNew('Item', sItem)

	oRs.Update()
	
	oRs.Filter = "Item Like '" + sMatch + "'"
	oField = oFields('Item')
	if not oRs.BOF: oRs.MoveFirst()
	while not oRs.EOF:
		sValue = oField.Value
		lReturn.Add(sValue)
	
		oRs.MoveNext()

	oRs.Close()
	
	oField = None
	oFields = None
	oRs = None
	return lReturn
	# VtListFilterByWildcard method
	
def VtListGetExtensions(lPaths):
	# Get a list of extensions from a list of paths, converting to lower case, removing duplicates, and sorting
	
	lReturn = VtCreateList()
	for i in range(lPaths.Count):
		sPath = lPaths(i)
	# if StringContains(sPath, '://', False)) sPath = WebUrlDownloadFileName(sPath, 'C:\\temp', False:
		sPath = PathGetName(sPath)
		sExtension = PathGetExtension(sPath)
		if StringStartsWith(sExtension, '.', False): sExtension = StringChopLeft(sExtension, 1)
		sExtension = RegExpReplace(sExtension, '[^0-9a-zA-Z]', '', True)
		if len(sExtension) > 5: sExtension = ''
	# if not sExtension: continue
		if not sExtension: sExtension = "htm"
		sExtension = sExtension.lower()
		if lReturn.Contains(sExtension): continue
		lReturn.Add(sExtension)

	# lReturn.Sort()
	return lReturn
	# VtListGetExtensions method
	
def VtListToPy(l):
	# Convert variant list to Python array
	
	aReturn = []
	for i in range(l.Count): aReturn[i] = l.Item(i)
	return aReturn
	# VtListToPy method
	
def VtListToString(lItems, sDelimiter):
	# Convert a list to a string with a specified delimiter
	
	sReturn = xMute
	for i in range(lItems.Count):
		oItem = lItems(i)
		sItem = PyString(oItem)
		if i > 0: sReturn += sDelimiter
		sReturn += sItem

	return sReturn
	# VtListToStringMethod
	
def VtStreamToFile(aBytes, sFile):
	# Save a variant byte array to a binary file
	
	try:
		oStream = VtCreateStream()
		oStream.Write(aBytes)
		oStream.SaveToFile(sFile, adSaveCreateOverWrite)
		oStream.Close()
	except: pass

	oStream = None
	return FileExists(sFile)
	# VtStreamToFile method
	
def VtTypeName(o):
	t = type(o)
	# print t.__name__
	# print t
	return str(t)

"""
def WebFetch(dParams):
	# Fetch files from web according to specifications in dictionary of parameters
	
	# SearchScope values:
	# Path, with same directory and subdomain
	# Subdomain, with any directory
	# Domain, with any directory or subdomain
	# Other, restricted by filters or additional limits
	
	if not dParams.Exists('LinkSegment'): dParams('LinkSegment') = 1
	if not dParams.Exists('TimeLimit'): dParams('TimeLimit') = 0
	if not dParams.Exists('TimeStart'): dParams('TimeStart') = Date().getTime()
	if not dParams.Exists('MaximumSize'): dParams('MaximumSize') = 0
	if not dParams.Exists('TotalSize'): dParams('TotalSize') = 0
	if not dParams.Exists('EventLog'): dParams('EventLog') = xMute
	if not dParams.Exists('CrawlUrls'): dParams('CrawlUrls') = VtCreateList()
	if not dParams.Exists('FetchUrls'): dParams('FetchUrls') = VtCreateList()
	
	iNow = Date().getTime()
	iDelta = iNow - dParams('TimeStart')
	
	if (dParams('TimeLimit') > 0) and (iDelta > dParams('TimeLimit')): 
		sReturn += 'Time limit of ' + StringCount('minute', iDelta / Math.pow(60, 2)) + ' exceeded' + '\n'
		return dParams
	
	if (dParams('MaximumSize') > 0) and (dParams('TotalSize') > dParams('MaximumSize')): 
		sReturn += 'Maximum size of ' + StringCount('megabyte', d('MaximumSize') / Math.pow(1024, 2)) + ' exceeded' + '\n'
		return dParams
	
	sReturn = 'Parameters:' + '\n'
	d = VtDictionaryToPy(dParams)
	for sKey in d.keys():  sReturn += sKey +'=' + d[sKey] + '\n'
	sReturn += '\n'
	
	sProjectFolder = d['ProjectFolder']
	bIncludeSubfolders = d['IncludeSubfolders']
	bUniquePath = True
	if bIncludeSubfolders: bUniquePath = False
	sSearchScope = d['SearchScope']
	bQueryOnly = d['QueryOnly']
	
	aBodyFilters = PySplitLines(d['BodyFilters'])
	aIncludeBodyFilters = []
	aExcludeBodyFilters = []
	for iBodyFilter in range(aBodyFilters.length):
		sBodyFilter = aBodyFilters[iBodyFilter]
		if not sBodyFilter: continue
		if StringStartsWith(sBodyFilter, '-', False):
			sBodyFilter = StringChopLeft(sBodyFilter, 1)
			aExcludeBodyFilters.push(sBodyFilter)
		else:
			if StringStartsWith(sBodyFilter, '+', False): sBodyFilter = StringChopLeft(sBodyFilter, 1)
			aIncludeBodyFilters.push(sBodyFilter)

	# create BodyFilter arrays

	aUrlFilters = PySplitLines(d['UrlFilters'])
	aIncludeUrlFilters = []
	aExcludeUrlFilters = []
	for iUrlFilter in range(aUrlFilters.length):
		sUrlFilter = aUrlFilters[iUrlFilter]
		if not sUrlFilter: continue
		if StringStartsWith(sUrlFilter, '-', False): 
			sUrlFilter = StringChopLeft(sUrlFilter, 1)
			aExcludeUrlFilters.push(sUrlFilter)
		else:
			if StringStartsWith(sUrlFilter, '+', False): sUrlFilter = StringChopLeft(sUrlFilter, 1)
			aIncludeUrlFilters.push(sUrlFilter)
	# create URL filter arrays
	
	aWebSource = PySplitLines(d['WebSource'])
	sReturn += 'Fetching' +'\n'
	for iWebSource in range(aWebSource.length):
		sWebSource = aWebSource[iWebSource]
		if not sWebSource: continue
		if dParams('FetchUrls').Contains(sWebSource): continue
		sReturn += 'Web source ' + sWebSource + '\n'
		bIncludeWebSource = True
		for iExcludeUrlFilter in range(aExcludeUrlFilters):
			sExcludeUrlFilter = aExcludeUrlFilters[iExcludeUrlFilter]
			if not sExcludeUrlFilter: continue
	if RegExpTest(sWebSource, sExcludeUrlFilter, True): 
				bIncludeWebSource = False
				sReturn += 'No fetch from exclude URL filter ' + sExcludeUrlFilter + '\n'
				break
	# Test exclude URL filters
			if not bIncludeWebSource: continue
	
	if aIncludeUrlFilters.length: bIncludeWebSource = False
	for iIncludeUrlFilter in range(aIncludeUrlFilters.length):
	sIncludeUrlFilter = aIncludeUrlFilters[iIncludeUrlFilter]
	if not sIncludeUrlFilter: continue
	if RegExpTest(sWebSource, sIncludeUrlFilter, True): {
	bIncludeWebSource = True
	sReturn += 'Fetch from include URL filter ' + sIncludeUrlFilter + '\n'
	break
	# Test include URL filters
	if not bIncludeWebSource: continue
	
	sFolder = UrlGetFolder(sWebSource)
	if bIncludeSubfolders and sFolder) sFolder = PathCombine(sProjectFolder, sFolder:
	sFile = WebUrlDownloadFileName(sWebSource, sFolder, bUniquePath)
	dParams('FetchUrls').Add(sWebSource)
	if bQueryOnly: continue
	elif WebUrlToFile(sWebSource, sFile): {
	sReturn += 'Download ' + sFile + '\n'
	iSize = FileGetSize(sFile)
	d('TotalSize') += iSize
	}
	else: sReturn += 'Cannot download ' + sFile + '\n'
	# Fetch web sources
	if d['LinkSegment'] >= d['LinkDistance']: return sReturn
	
	sReturn += 'Crawling' + '\n'
	for iWebSource in range(aWebSource.length):
	sWebSource = aWebSource[iWebSource]
	if not sWebSource: continue
	if dParams('CrawlUrls').Contains(sWebSource): continue
	if not WebUrlIsHtml(sWebSource): continue
	dParams('CrawlUrls').Add(sWebSource)
	sReturn += 'HTML ' + sWebSource + '\n'
	sBody = HtmlGetText(sWebSource, False)
	
	bIncludeWebSource = True
	for iExcludeBodyFilter in range(aExcludeBodyFilters):
	sExcludeBodyFilter = aExcludeBodyFilters[iExcludeBodyFilter]
	if not sExcludeBodyFilter: continue
	if RegExpTest(sBody, sExcludeBodyFilter, True): {
	bIncludeWebSource = False
	sReturn += 'No crawl from exclude body filter ' + sExcludeBodyFilter + '\n'
	break
	}
	# Test exclude body filters
	if not bIncludeWebSource: continue
	
	if aIncludeBodyFilters.length: bIncludeWebSource = False
	for iIncludeBodyFilter in range(aIncludeBodyFilters):
	sIncludeBodyFilter = aIncludeBodyFilters[iIncludeBodyFilter]
	if not sIncludeBodyFilter: continue
	if RegExpTest(sBody, sIncludeBodyFilter, True): {
	bIncludeWebSource = True
	sReturn += 'Crawl from include body filter ' + sIncludeBodyFilter + '\n'
	break
	}
	# Test include body filters
	if not bIncludeWebSource: continue
	
	lLinkUrls = HtmlGetUrls(sWebSource)
	for iLinkUrl in range(lLinkUrls.Count):
	sLinkUrl = lLinkUrls(iLinkUrl)
	sReturn += 'Link URL ' + sLinkUrl + '\n'
	if StringEquiv(sSearchScope, 'Path') and not (StringEquiv(UrlGetHost(sWebSource), UrlGetHost(sLinkUrl)) and StringEquiv(UrlGetFolder(sWebSource), UrlGetFolder(sLinkUrl))): {
	sReturn += 'No crawl from search scope ' + sSearchScope + '\n'
	continue
	}
	elif StringEquiv(sSearchScope, 'Subdomain') and not StringEquiv(UrlGetHost(sWebSource), UrlGetHost(sLinkUrl)): {
	sReturn += 'No crawl from search scope ' + sSearchScope + '\n'
	continue
	}
	elif StringEquiv(sSearchScope, 'Domain') and not StringEquiv(UrlGetBaseDomain(sWebSource) , UrlGetBaseDomain(sLinkUrl)): {
	sReturn += 'No crawl from search scope ' + sSearchScope + '\n'
	continue
	}
	
	dCopy = VtCopyDictionary(dParams)
	dCopy('WebSource') = sLinkUrl
	dCopy('LinkSegment')++
	dReturn = WebFetch(dCopy)
	if PyIsObject(dReturn): {
	sReturn += dReturn('EventLog') + '\n'
	dParams('TotalSize') = dReturn('TotalSize')
	dParams('CrawlUrls') = dReturn('CrawlUrls')
	dParams('FetchUrls') = dReturn('FetchUrls')
	sReturn += '\n'
	}
	# Iterate LinkURLs
	# Iterate web sources
	dParams('EventLog') = sReturn
	return dParams
	# WebFetch method
	
"""

def WebListDownload(lUrls, sFolder, bUnique):
	# Download a list of URLs, specifying folder and whether to ensure unique file names
	
	for i in range(lUrls.Count):
		sUrl = lUrls(i)
		sFile = WebUrlDownloadFileName(sUrl, sFolder, bUnique)
	# PyPrint(sFile)
		WebUrlToFile(sUrl, sFile)
	# WebListDonload method
	
def WebRequest(sType, sUrl, dData, dHeaders, sUser, sPassword):
	# Send a web request and return the response
	
	sType = sType.upper()
	sData = None
	d = dData
	if PyIsObject(dData): d = VtDictionaryToPy(dData)
	if isinstance(d, dict):
		sData = PyDictionaryToEncodedString(d)
	
	if sType == 'GET' and sData: sUrl += '?' + sData
	
	if sType == 'POST': oRequest = VtCreateWebRequest()
	else: oRequest = VtCreateWinHttpRequest()
	
	try: 
		oRequest.open(sType, sUrl, False, sUser, sPassword)
	except: return None

	oRequest.setRequestHeader('User-Agent', 'HomerJax')
	if sType == 'POST': oRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded')
	
	d = dHeaders
	if PyIsObject(d): d = VtDictionaryToPy(dHeaders)
	if isinstance(d, dict):
		for sKey in d.keys():
			sValue = d[sKey]
			oRequest.setRequestHeader(sKey, sValue)
	
	try: 
		if sType == 'POST': oRequest.send(sData)
		else: oRequest.send()
	except: pass

	return oRequest
	# WebRequest method
	
def WebRequestAllHeaders(sUrl):
	# Get all response headers
	
	dData = None
	dHeaders = None
	sUser = None
	sPassword = None
	
	oRequest = WebRequestHead(sUrl, dData, dHeaders, sUser, sPassword)
	sReturn = xMute
	try: sReturn = oRequest.getAllResponseHeaders()
	except: pass
	
	oRequest = None
	return sReturn
	# WebRequestAllHeaders method
	
	
def WebRequestGet(sUrl, dData=None, dHeaders=None, sUser=None, sPassword=None):
	# Send a get request
	
	return WebRequest('GET', sUrl, dData, dHeaders, sUser, sPassword)
	# WebRequestGet method
	
def WebRequestGetToFile(sUrl, dData, dHeaders, sUser, sPassword, sFile):
	# Send a get request and save response to file
	
	return WebRequestToFile('GET', sUrl, dData, dHeaders, sUser, sPassword, sFile)
	# WebRequestGetToFile method
	
def WebRequestGetToString(sUrl, dData=None, dHeaders=None, sUser=None, sPassword=None):
	# Send a get request and return response as string
	
	return WebRequestToString('GET', sUrl, dData, dHeaders, sUser, sPassword)
	# WebRequestGetToString method
	
def WebRequestHead(sUrl, dData, dHeaders, sUser, sPassword):
	# Send a head request
	
	return WebRequest('HEAD', sUrl, dData, dHeaders, sUser, sPassword)
	# WebRequestHead method
	
def WebRequestHeader(sUrl, sHeader):
	# Get response header
	
	dData = None
	dHeaders = None
	sUser = None
	sPassword = None
	
	oRequest = WebRequestHead(sUrl, dData, dHeaders, sUser, sPassword)
	sReturn = xMute
	try: sReturn = oRequest.getResponseHeader(sHeader)
	except: pass
	
	oRequest = None
	return sReturn
	# WebRequestHeader method
	
def PyWebRequestPostToString(sUrl, dData=None, dHeaders=None, sUser=None, sPassword=None):
	sData = None
	if dData: sData = utf8urlencode(dData)
	# print 'data', sData
	oRequest = urllib2.Request(sUrl, sData)
	# oRequest.add_header(u'Authorization', t.auth)
	for sKey, sValue in dHeaders.items(): oRequest.add_header(sKey, sValue)
	fResponse = urllib2.urlopen(oRequest)
	sResponse = fResponse.read()
	fResponse.close()
	return sResponse
	
def PyWebRequestGetToString(sUrl, dData=None, dHeaders=None, sUser=None, sPassword=None):
	sData = None
	if dData: sData = utf8urlencode(dData)
	if dData: sUrl += '?' + sData
	# print sUrl
	oRequest = urllib2.Request(sUrl)
	fResponse = urllib2.urlopen(oRequest)
	sResponse = fResponse.read()
	fResponse.close()
	return sResponse
	
	

def WebRequestPost(sUrl, dData, dHeaders, sUser, sPassword):
	# Send a post request
	
	return WebRequest('POST', sUrl, dData, dHeaders, sUser, sPassword)
	# WebRequestPost method
	
def WebRequestPostToFile(sUrl, dData, dHeaders, sUser, sPassword, sFile):
	# Send a post request and save response to file
	
	return WebRequestToFile('POST', sUrl, dData, dHeaders, sUser, sPassword, sFile)
	# WebRequestPostToFile method
	
def WebRequestPostToString(sUrl, dData, dHeaders, sUser, sPassword):
	# Send a post request and return response as string
	
	if not sUser: sUser = None
	return WebRequestToString('POST', sUrl, dData, dHeaders, sUser, sPassword)
	# WebRequestPostToString method
	
def WebRequestToFile(sType, sUrl, dData, dHeaders, sUser, sPassword, sFile):
	# Save web resource to file
	
	oRequest = WebRequest(sType, sUrl, dData, dHeaders, sUser, sPassword)
	aBytes = oRequest.responseBody
	bReturn = VtStreamToFile(aBytes, sFile)
	oRequest = None
	return bReturn
	# WebRequestToFile method
	
def WebRequestToString(sType, sUrl, dData, dHeaders, sUser, sPassword):
	# Return web resource as string
	
	# return urllib.urlopen(sUrl).read()
	oRequest = WebRequest(sType, sUrl, dData, dHeaders, sUser, sPassword)
	try: sReturn = oRequest.responseText
	except:
		aBytes = oRequest.responseBody
		sFile = PathGetTempFile()
		bResult = VtStreamToFile(aBytes, sFile)
		sReturn = FileToString(sFile)
		FileDelete(sFile)

	# print sReturn
	oRequest = None
	return sReturn
	# WebRequestToString method
	
def WebUrlContentDisposition(sUrl):
	# Get suggested file name of a web resource
	
	sHeader = 'content-disposition'
	sReturn = WebRequestHeader(sUrl, sHeader)
	aParts = sReturn.split('=')
	iLength = aParts.length
	sReturn = aParts[iLength - 1]
	sReturn = StringTrim(sReturn)
	return sReturn
	# WebUrlContentDisposition method
	
	
def WebUrlContentEncoding(sUrl):
	# Get encoding of a web resource
	
	sHeader = 'Content-Encoding'
	sReturn = WebRequestHeader(sUrl, sHeader)
	return sReturn
	# WebUrlContentEncoding method
	
def WebUrlContentLanguage(sUrl):
	# Get natural language code of a web resource
	
	sHeader = 'Content-Language'
	sReturn = WebRequestHeader(sUrl, sHeader)
	return sReturn
	# WebUrlContentLanguage method
	
def WebUrlContentLength(sUrl):
	# Get content length of a web resource
	
	sHeader = 'content-length'
	sReturn = WebRequestHeader(sUrl, sHeader)
	return sReturn
	# WebUrlContentLength method
	
def WebUrlContentLocation(sUrl):
	# Get redirected location of a web resource
	
	sHeader = 'Content-Location'
	sReturn = WebRequestHeader(sUrl, sHeader)
	return sReturn
	# WebUrlContentLocation method
	
def WebUrlContentType(sUrl):
	# Get content type of a web resource
	
	sHeader = 'content-type'
	sReturn = WebRequestHeader(sUrl, sHeader)
	return sReturn
	# WebUrlContentType method
	
def WebUrlDownloadFileName(sUrl):
	# Get or generate download file name from URL, trying various techniques
	
	sFile = xMute
	sBase = xMute
	sExtension = xMute
	
	sUrl = UrlNormalize(sUrl, None)
	sQuery = UrlGetQuery(sUrl)
	if not sQuery: sFile = UrlGetFileName(sUrl)
	
	if not sFile: sFile = WebUrlContentDisposition(sUrl)
	
	if not sFile: 
		sRedirectUrl = WebUrlRedirect(sUrl)
		if sRedirectUrl: sFile = UrlGetFileName(sRedirectUrl)
	
	if sFile:
		sBase = PathGetBase(sFile)
		sExtension = PathGetExtension(sFile)
	
	if not sBase:
		sPathPlus = UrlGetPathPlus(sUrl)
		if sPathPlus: sBase = sPathPlus
	
	if not sExtension: 
		if WebUrlIsHtml(sUrl): 
			sExtension = 'htm'
			if not sBase: sBase = 'page'
		else:
			sType = WebUrlContentType(sUrl)
			aParts = sType.split('/')
			iLength = len(aParts)
			if iLength and aParts[0]: sExtension = aParts[iLength - 1]
	
	if not sBase: sBase = 'file'
	if not sExtension: sExtension = 'bin'
	sReturn = sBase + '.' + sExtension
	return sReturn
	# WebUrlDownloadFileName method
	
	
def WebUrlDownloadPath(sUrl, sFolder, bUnique):
	# Obtain download path for a web resource
	
	sName = WebUrlDownloadFileName(sUrl)
	sBase = PathGetBase(sName)
	sExtension = PathGetExtension(sName)
	sReturn = PathGetValid(sFolder, sBase, sExtension, bUnique)
	return sReturn
	# WebDownloadPath method
	
def WebUrlIsHtml(sUrl):
	# Test whether content type is text/html
	
	sType = WebUrlContentType(sUrl)
	return StringStartsWith(sType, 'text/html', True)
	# WebUrlIsHtml method
	
def WebUrlLastModified(sUrl):
	# Get last modification time stamp of a web resource
	
	sHeader = 'LastModified'
	sReturn = WebRequestHeader(sUrl, sHeader)
	return sReturn
	# WebUrlLastModified method
	
def WebUrlRedirect(sUrl):
	# Get url after redirects
	
	dData = None
	dHeaders = None
	sUser = None
	sPassword = None
	
	oRequest = WebRequestHead(sUrl, dData, dHeaders, sUser, sPassword)
	sReturn = xMute
	try: sReturn = oRequest.Option(1)
	except: pass
	
	oRequest = None
	return sReturn
	# WebUrlRedirect method
	
def WebUrlServer(sUrl):
	# Get server name and version
	
	sHeader = 'Server'
	sReturn = WebRequestHeader(sUrl, sHeader)
	return sReturn
	# WebUrlServer method
	
def WebUrlToFile(sUrl, sFile):
	# Save web resource to file
	
	return urllib.urlretrieve(sUrl, sFile)

	dData = None
	dHeaders = None
	sUser = None
	sPassword = None
	
	return WebRequestGetToFile(sUrl, dData, dHeaders, sUser, sPassword, sFile)
	# WebUrlToFile method
	
def WebUrlToString(sUrl):
	# Return web resource as string
	
	dData = None
	dHeaders = None
	sUser = None
	sPassword = None
	
	return WebRequestGetToString(sUrl, dData, dHeaders, sUser, sPassword)
	# WebUrlToString method
	
def WebUrlToXml(sUrl):
	# Return web resource as XML document
	
	dData = None
	dHeaders = None
	sUser = None
	sPassword = None
	
	sText = WebUrlToString(sUrl)
	oDoc = VtCreateXmlDocument()
	oDoc.loadXML(sText)
	return oDoc
	# WebUrlToXml method
	
def XmlAppendElement(oParent, sName, sValue):
	# Append element to a node
	
	oChild = XmlCreateElement(oParent, sName, sValue)
	oParent.appendChild(oChild)
	return oChild
	# XmlAppendElement method
	
def XmlCreateElement(oNode, sName, sValue):
	# Create an XML element
	
	oChild = None
	if PyIsUndefined(sValue): sValue = None
	if PyIsObject(oNode): 
		# if VtTypeName(oNode) == 'DOMDocument': oDoc = oNode
		if StringContains(VtTypeName(oNode), 'DOMDocument'): oDoc = oNode
		else: oDoc = oNode.ownerDocument
	
	oChild = oDoc.createElement(sName)
	if not PyIsNone(sValue): 
		oData = oDoc.createCDATASection(sValue)
		oChild.appendChild(oData)
	
	oDoc = None
	return oChild
	# XmlCreateElement method
	
def XmlCreateFile(sFile):
	# Create a root XML file
	
	oDoc = VtCreateXmlDocument()
	XmlAppendElement(oDoc, 'Root', None)
	FileDelete(sFile)
	oDoc.save(sFile)
	return oDoc
	# XmlCreateFile method
	
def XmlEnsureGetNode(sFile, sPath):
	# Return a node, creating preceding elements if necessary
	
	oNode = None
	if PyIsObject(sFile): oDoc = sFile
	else: oDoc = XmlEnsureOpenFile(sFile)
	
	if PyIsObject(oDoc): 
		if StringContains(VtTypeName(oDoc), 'DOMDocument'): oParent = oDoc.documentElement
		else: oParent = oDoc
	
		if not len(sPath): oNode = oParent
		else:
			oNode = XmlGetNode(oParent, sPath)
			if PyIsNone(oNode): 
				aNames = sPath.split('/')
				sName = aNames.pop()
				# sPath = aNames.join('/')
				sPath = '/'.join(aNames)
				oParent = XmlEnsureGetNode(oDoc, sPath)
				oNode = XmlAppendElement(oParent, sName, None)
	
	oParent = None
	oDoc = None
	return oNode
	# XmlEnsureGetNode method
	
def XmlEnsureOpenFile(sFile):
	# Open an XML file, creating it if necessary
	
	oDoc = XmlOpenFile(sFile)
	if PyIsNone(oDoc): oDoc = XmlCreateFile(sFile)
	return oDoc
	# XmlEnsureOpenFile method
	
def XmlGetAttribute(sFile, sPath, sAttribute, sDefault):
	# Get an attribute of a node
	
	sReturn = sDefault
	if PyIsUndefined(sReturn): sReturn = None
	if PyIsObject(sFile): oDoc = sFile
	else: oDoc = XmlOpenFile(sFile)
	
	if PyIsObject(oDoc): 
		if StringContains(VtTypeName(oDoc), 'DOMDocument'): oParent = oDoc.documentElement
		else: oParent = oDoc
	
	oNode = oParent.selectSingleNode(sPath)
	if PyIsObject(oNode): 
		sText = oNode.getAttribute(sAttribute)
		if sText != None: sReturn = sText
	
	oNode = None
	oParent = None
	oDoc = None
	return sReturn
	# XmlGetAttribute method
	
def XmlGetNode(sFile, sPath):
	# Get a node
	
	oNode = None
	if PyIsObject(sFile): oDoc = sFile
	else: oDoc = XmlOpenFile(sFile)
	
	if PyIsObject(oDoc): 
		if StringContains(VtTypeName(oDoc), 'DOMDocument'): oParent = oDoc.documentElement
		else: oParent = oDoc
	
		oNode = oParent.selectSingleNode(sPath)
	
	oParent = None
	oDoc = None
	return oNode
	# XmlGetNode method
	
def XmlGetNodeNames(sFile, sPath):
	# Get node names
	
	lReturn = VtCreateList()
	if PyIsObject(sFile): oDoc = sFile
	else: oDoc = XmlOpenFile(sFile)
	
	if PyIsObject(oDoc): 
		if StringContains(VtTypeName(oDoc), 'DOMDocument'): oParent = oDoc.documentElement
		else: oParent = oDoc
	
		oNodes = oParent.selectNodes(sPath)
		for i in range(oNodes.length):
			oNode = oNodes.item(i)
			lReturn.Add(oNode.nodeName)
	
	oNode = None
	oNodes = None
	oParent = None
	oDoc = None
	return lReturn
	# XmlGetNodeNames method
	
def XmlGetNodes(sFile, sPath):
	# Get a node list
	
	lReturn = VtCreateList()
	if PyIsObject(sFile): oDoc = sFile
	else: oDoc = XmlOpenFile(sFile)
	
	if PyIsObject(oDoc): 
		if StringContains(VtTypeName(oDoc), 'DOMDocument'): oParent = oDoc.documentElement
		else: oParent = oDoc
	
		oNodes = oParent.selectNodes(sPath)
		for i in range(oNodes.length):
			oNode = oNodes.item(i)
			lReturn.Add(oNode)
	
	# oNode = None
	# oNodes = None
	# oParent = None
	# oDoc = None
	return lReturn
	# XmlGetNodes method
	
def XmlGetText(sFile, sPath, sDefault):
	# Get the text of a node
	
	sReturn = sDefault
	if PyIsUndefined(sReturn): sReturn = None
	if PyIsObject(sFile): oDoc = sFile
	else: oDoc = XmlOpenFile(sFile)
	
	if PyIsObject(oDoc): 
		if StringContains(VtTypeName(oDoc), 'DOMDocument'): oParent = oDoc.documentElement
		else: oParent = oDoc
	
		oNode = oParent.selectSingleNode(sPath)
		if PyIsObject(oNode): 
			sText = oNode.text
			if sText != None: sReturn = sText
	
	oNode = None
	oParent = None
	oDoc = None
	return sReturn
	# XmlGetText method
	
def XmlGetValue(sFile, sPath, sDefault):
	# Get the value of a node
	
	sReturn = sDefault
	if PyIsUndefined(sReturn): sReturn = None
	if PyIsObject(sFile): oDoc = sFile
	else: oDoc = XmlOpenFile(sFile)
	
	if PyIsObject(oDoc): 
		if StringContains(VtTypeName(oDoc), 'DOMDocument'): oParent = oDoc.documentElement
		else: oParent = oDoc
	
		oNode = oParent.selectSingleNode(sPath)
		if PyIsObject(oNode): 
			sValue = oNode.nodeValue
			if sValue != None: sReturn = sValue
	
	oNode = None
	oParent = None
	oDoc = None
	return sReturn
	# XmlGetValue method
	
def XmlOpenFile(sFile):
	# Open an XML file
	
	if not FileExists(sFile): return None
	
	oDoc = VtCreateXmlDocument()
	try: oDoc.load(sFile)
	except: pass

	oDoc = None

	return oDoc
	# XmlOpenFile method
	
def XmlRemoveAttribute(sFile, sPath, sAttribute):
	# Remove an attribute
	
	if PyIsObject(sFile): oDoc = sFile
	else: oDoc = XmlOpenFile(sFile)
	
	if PyIsObject(oDoc): 
		if StringContains(VtTypeName(oDoc), 'DOMDocument'): oParent = oDoc.documentElement
		else: oParent = oDoc
	
		oNode = oParent.selectSingleNode(sPath)
		if PyIsObject(oNode): 
			oNode.removeAttribute(sAttribute)
			oDoc.save(sFile)
	
	oNode = None
	oParent = None
	oDoc = None
	# XmlRemoveAttribute method
	
def XmlRemoveNode(sFile, sPath):
	# Remove a node
	
	if PyIsObject(sFile): oDoc = sFile
	else: oDoc = XmlOpenFile(sFile)
	
	if PyIsObject(oDoc): 
		oParent
		if StringContains(VtTypeName(oDoc), 'DOMDocument'): oParent = oDoc.documentElement
		else: oParent = oDoc
	
		oNode = oParent.selectSingleNode(sPath)
		if PyIsObject(oNode): 
			oParent = oNode.parentNode
			oParent.removeChild(oNode)
			oDoc.save(sFile)
	
	oNode = None
	oParent = None
	oDoc = None
	# XmlRemoveNode method
	
def XmlRemoveNodes(sFile, sPath):
	# Remove a node collection
	
	if PyIsObject(sFile): oDoc = sFile
	else: oDoc = XmlOpenFile(sFile)
	
	if PyIsObject(oDoc): 
		if StringContains(VtTypeName(oDoc), 'DOMDocument'): oParent = oDoc.documentElement
		else: oParent = oDoc
	
		oNodes = oParent.selectNodes(sPath)
		if oNodes.length: 
			oParent = oNodes.firstChild.parentNode
			for i in range(oNodes.length).reverse():
				oNode = oNodes.item(i)
				oParent.removeChild(oNode)

	oDoc.save(sFile)

	
	oNode = None
	oNodes = None
	oParent = None
	oDoc = None
	# XmlRemoveNodes method
	
def XmlSetAttribute(sFile, sPath, sAttribute, sText):
	# Set an attribute
	
	oNode = XmlEnsureGetNode(sFile, sPath)
	if PyIsObject(oNode): 
		oNode.setAttribute(sAttribute, sText)
		if VtTypeName(oNode) == 'DOMDocument': oDoc = oNode
		else: oDoc = oNode.ownerDocument
		if not PyIsObject(sFile): oDoc.save(sFile)
	
	oNode = None
	oDoc = None
	return (XmlGetAttribute(sFile, sPath, sAttribute, None) == sText)
	# XmlSetAttribute method
	
def XmlSetText(sFile, sPath, sText):
	# Set text of an element
	
	return XmlSetValue(sFile, sPath, sText)
	# XmlSetText method
	
def XmlSetValue(sFile, sPath, sValue):
	# Set a value
	
	oNode = XmlEnsureGetNode(sFile, sPath)
	if PyIsObject(oNode): 
		aNames = sPath.split('/')
		sName = aNames.pop()
	
		if VtTypeName(oNode) == 'DOMDocument': oDoc = oNode
		else: oDoc = oNode.ownerDocument
		oNew = XmlCreateElement(oDoc, sName, sValue)
		oAttributes = oNode.attributes
		for i in range(oAttributes.length):
			oAttribute = oAttributes.item(i)
			sAttribute = oAttribute.nodeName
			sText = oNode.getAttribute(sAttribute)
			oNew.setAttribute(sAttribute, sText)

	oParent = oNode.parentNode
	oParent.replaceChild(oNew, oNode)
	if not PyIsObject(sFile): oDoc.save(sFile)
	
	oNew = None
	oNode = None
	oDoc = None
	return (XmlGetValue(sFile, sPath, None) == sValue)
	# XmlSetValue method
	
def XmlToVtDictionary(sFile):
	# Convert XML file to variant dictionary
	
	dReturn = VtCreateDictionary()
	oNodes = XmlGetNodes(sFile, '*')
	for i in range(oNodes.length):
		oNode = oNodes.item(i)
		sName = oNode.nodeName
		sValue = oNode.text
		# dReturn(sName) = sValue
		dReturn.Add(sName, sValue)

	return dReturn
	# XmlToVtDictionary method
	
def ActivateWindow(hWindow):
	KERNEL32 = ctypes.windll.KERNEL32
	GetCurrentThreadId = KERNEL32.GetCurrentThreadId
	
	USER32 = ctypes.windll.USER32
	AttachThreadInput = USER32.AttachThreadInput
	BringWindowToTop = USER32.BringWindowToTop
	GetForegroundWindow = USER32.GetForegroundWindow
	GetWindowThreadProcessId = USER32.GetWindowThreadProcessId
	ShowWindow=USER32.ShowWindow
	
	iForegroundThread = GetWindowThreadProcessId(GetForegroundWindow(), 0)
	#print 'foreground thread', iForegroundThread
	iAppThread = GetCurrentThreadId()
	#print 'app thread', iAppThread
	if iForegroundThread == iAppThread:
		BringWindowToTop(hWindow)
		ShowWindow(hWindow,3)
	else:
		iResult = AttachThreadInput(iForegroundThread, iAppThread, 1)
		#print 'result', iResult
		iResult = BringWindowToTop(hWindow)
		iResult = ShowWindow(hWindow,3)
		iResult = AttachThreadInput(iForegroundThread, iAppThread, 0)
		#print 'result', iResult

def AddText(existing='', new='', delimiter="\n"):
	sText = str(existing)
	if sText: sText += delimiter
	sText += str(new)
	return sText

def FindWindow(classname='', title=''):
	try: h = win32gui.FindWindow(sClass, sTitle)
	except: h = 0
	return h

def SortNamesAndValues(names, values):
	l = [(names[i].lower(), i, names[i], values[i]) for i in range(len(names))]
	l.sort()
	names = [name for name_lower, i, name, value in l]
	values = [value for name_lower, i, name, value in l]
	return names, values
	# end def

def IsWin64(): return platform.architecture()[0].find('64') >= 0

def Exec(sCode, v1 = None, v2 = None, v3 = None, v4 = None):
	try:
		exec(sCode)
	except:
		pass

def Eval(sCode, v1 = None, v2 = None, v3 = None, v4 = None):
	aLines = sCode.rstrip().splitlines()
	try:
		if len(aLines) > 1: exec('\n'.join(aLines[0:-1]))
		sLine = aLines[len(aLines) - 1]
		vReturn = eval(sLine)
	except:
		vReturn = None
	return vReturn

def IsJAWSActive(self):
	sClass = 'JFWUI2'
	sTitle = 'JAWS'
	try:
		h = win32gui.FindWindow(sClass, sTitle)
	except:
		h = 0
	return h

def IsSAActive(self):
	return SA_IsRunning()

def IsWEActive(self):
	sClass = 'GWMExternalControl'
	sTitle = 'External Control'
	try:
		h = win32gui.FindWindow(sClass, sTitle)
	except:
		h = 0
	return h

def JAWSSay(sText):
	iInterrupt = 0
	iResult = JFWSayString(StrDefault(sText), iInterrupt)
	return iResult

def COM_JAWSSay(sText):
	oJFW = win32com.client.Dispatch('FreedomSci.JawsApi')
	iResult = oJFW.SayString(unicode(sText), iInterrupt)
	oJFW = None
	return iResult

def JAWSSilence(self):
	oJFW = win32com.client.Dispatch('FreedomSci.JawsApi')
	iResult = oJFW.StopSpeech()
	oJFW = None
	return iResult

def JAWSRunScript(sScript):
	oJFW = win32com.client.Dispatch('FreedomSci.JawsApi')
	iResult = oJFW.RunScript(str(sScript))
	oJFW = None
	return iResult

def JAWSRunFunction(sFunction):
	oJFW = win32com.client.Dispatch('FreedomSci.JawsApi')
	iResult = oJFW.RunFunction(str(sFunction))
	oJFW = None
	return iResult

def SASay(sText):
	return SA_SayU(unicode(sText))

def OldSAPISay(sText):
	oSAPI = win32com.client.Dispatch('SAPI.SPVoice')
	oSAPI.Speak(sText)
	oSAPI = None

def WESay(sText):
	oWE = win32com.client.Dispatch('GWSpeak.Speak')
	oWE.SpeakString(unicode(sText))
	oWE = None

def Say(sText, sTemp=''):
	# if not ExtensionEnhanced(): return False
	if not sTemp: sTemp = os.path.join(os.environ['APPDATA'] + r'\McTwit')
	if not os.path.isdir(sTemp): os.mkdir(sTemp)
	sTemp = os.path.join(sTemp, 'McTwit.tmp')
	# print 'sTemp', sTemp
	# fTemp = open(sTemp, 'wu')
	fTemp = open(sTemp, 'wb')
	sText = StrDefault(sText)
	fTemp.write(sText)
	# fTemp.flush()
	fTemp.close()
	sExe = 'SayFile.exe'
	if IsWin64(): sExe = 'SayFile64.exe'
	sExe = os.path.join(sys.prefix, sExe)
	return os.spawnv(0, sExe, ('.', sTemp))
	# return win32api.ShellExecute(0, None, os.path.join(sys.prefix, 'SayFile.exe'), sTemp, '', 0)
	bReturn = False
	if self.IsJAWSActive(): bReturn = self.JAWSSay(sText)
	if not bReturn and self.IsWEActive(): bReturn = self.WESay(sText)
	if not bReturn and self.IsSAActive(): bReturn = self.SASay(sText)
	if not bReturn and self.UseSAPIAsBackup: bReturn = self.SAPISay(sText)
	return bReturn
	
def SayLine(sText):
	sText = PyString(sText)
	if not sText: return False
	sText = sText.replace('\n', '   ').replace('\r', '')
	sText = StrDefault(sText)
	sExe = 'SayLine.exe'
	if IsWin64(): sExe = 'SayLine64.exe'
	sExe = os.path.join(sys.prefix, sExe)
	return os.spawnv(0, sExe, ('.', sText))

def OldUseSAPI(bState):
	bOldState = self.UseSAPIAsBackup
	self.UseSAPIAsBackup = bState
	return bOldState
	
def ReadTextFile(sFile):
	try:
		fFile = open(sFile, 'ru')
		sText = fFile.read()
		fFile.close()
	except:
		sText = ''
	return sText

def ReadBinaryFile(sFile):
	try:
		fFile = open(sFile, 'rb')
		sBinary = fFile.read()
		fFile.close()
	except:
		sBinary = ''
	return sBinary

def WriteTextFile(sFile, sText):
	try:
		fFile = open(sFile, 'wu')
		fFile.write(sText)
		fFile.close()
	except:
		pass

def DialogShow(sTitle='Show', sMessage=''):
	win32gui.MessageBox(0, sMessage, sTitle, 0)

def ConsoleSetTitle(sTitle) :
	win32console.SetConsoleTitle(sTitle)

def ConsoleMaximizeWindow(self) :
	hWindow = win32console.GetConsoleWindow()
	win32gui.ShowWindow(hWindow, win32con.SW_SHOWMAXIMIZED)

def ConsoleReadLine(sPrompt) :
	return raw_input(sPrompt)
	rl = readline.rlmain.Readline()
	return rl.readline(sPrompt)

def ConsoleReadPassword(sPrompt) :
	return getpass.getpass(str(sPrompt))

def ConsoleWrite(sText) :
	print(sText)

def ConsoleWriteLine(sLine) :
	self.ConsoleWrite(sLine + '\n')

def ConsoleClearScreen(self) :
	os.system('cls')

def ConsoleIsCharacterWaiting(self):
	return msvcrt.kbhit()

def ConsoleGetCharacter(self):
	return msvcrt.getche()

def WrapText(sText='', iWidth=70):
	return '\r\n'.join(textwrap.wrap(sText, iWidth))

def UrlToFile(sUrl='', sFile=''):
	if os.isfile(sFile): 		win32api.DeleteFile(sFile)
	urllib.urlretrieve(sUrl, sFile)
	return os.isfile(sFile)

def GetRssItems(sFeedUrl, sItem):
	dResult = feedparser.parse(sFeedUrl)
	lItems = dResult.entries
	sDomain = UrlGetBaseDomain(sFeedUrl)
	sText = Pluralize(sItem, len(lItems)) + ' from ' + sDomain + '\r\n\r\n'
	for dItem in lItems:
		sText = AddLineIfKey(sText, dItem, 'title')
		sSummary = dItem.get('summary', None)
		try: sSummary = HtmlGetText(sSummary)
		except: pass
		if sSummary: sSummary = sSummary.strip()
		if sSummary: sText += sSummary + '\r\n'
		sText = AddLineIfKey(sText, dItem, 'link')
		sText += '\r\n'

	sText = sText.strip() + '\r\n'
	return sText

def GetExecutablePath(exe):
	sName = exe
	if sName.find('.') == -1: sName += '.exe'
	sComputer = '.'
	oWMIService = win32com.client.GetObject(r'winmgmts:' + r'{impersonationLevel=impersonate}!\\' + sComputer  + r'\root\cimv2')
	sQuery = "select name,ProcessID, ExecutablePath from win32_process where name='" + sName + "'"
	oProcesses = oWMIService.ExecQuery(sQuery)
	
	sPath = ''
	for oProcess in oProcesses: sPath = oProcess.ExecutablePath; break
	return sPath
	
def old_GetExecutablePath(exe):
	oWMI = wmi.WMI(find_classes=False)
	lProcesses = oWMI.Win32_Process(['ExecutablePath', 'Caption', 'ProcessID'], Name=exe)
	if lProcesses: return lProcesses[0].ExecutablePath
	else: return ''

def AddLine(sText):
	sReturn = sText.strip()
	if sReturn: sReturn += '\r\n'
	return sReturn

def GetQuotesOfTheDay():
	dResult = feedparser.parse('http://www.quotationspage.com/data/mqotd.rss')
	dFeed = dResult.feed
	lEntries = dResult.entries
	sText = Pluralize('Motivational Quote', len(lEntries)) + ' of the Day from QuotationsPage.com\r\n\r\n'
	for oEntry in lEntries:
		sTitle_detail = oEntry.title_detail
		s = oEntry.description
		sMatch = r'\".*?\"'
		sItem = RegExpExtract(s, sMatch)[0]
		sText += sItem + '\r\n' + oEntry.title + '\r\n\r\n'
	sText = sText.strip()
	return sText
	
	
def HtmlToText(sHtml):
	# oDoc = comtypes.client.CreateObject('HTMLFile', dynamic=True)
	oDoc = win32com.client.dynamic.Dispatch('HTMLFile')
	oDoc.write(sHtml)
	oBody = oDoc.body
	# print type(oBody)
	sText = oBody.innerText
	# print len(sText)
	return sText

def GetUrl():
	sText = ''
	sUrl = GetIEUrl()
	return sUrl
	
def GetIEUrl():
	url = ''
	# shell = win32com.client.Dispatch('Shell.Application')
	shell = comtypes.client.CreateObject('Shell.Application', dynamic=True)
	windows = shell.Windows()
	count = windows.Count
	if count == 0: return url
	try:
		i = count - 1
		window = windows.Item(i)
		url = window.LocationURL
	except: pass
	return url
	
# def RunBackground(sCommand): win32api.ShellExecute(0, None, sCommand, '', '', 4)
# def RunBackground(sCommand): ShellRun(sCommand, 7, 0)
# def RunBackground(sCommand): ShellRun('IExplore.exe ' + sCommand, 7, 0)
def RunBackground(sCommand):
	h = dlg.GetHandle()
	os.startfile(sCommand)
	# win32api.Sleep(1000)
	win32api.Sleep(500)
	lbc.ActivateWindow(h)

def GetContentType(sUrl):
	fResponse = urllib2.urlopen(sUrl)
	sFinalUrl = fResponse.geturl()
	dInfo = fResponse.info()
	dHeaders = fResponse.headers
	sType = dHeaders.type
	# print 'url', sFinalUrl
	# print 'type', sType
	return sType

def IsWebPage(url): return GetContentType(url).startswith('text/html')

def DialogTranslateLanguage(sText=''):
	dDetectLanguages = {}
	dDetectLanguages['AFRIKAANS'] = 'af'
	dDetectLanguages['ALBANIAN'] = 'sq'
	dDetectLanguages['AMHARIC'] = 'am'
	dDetectLanguages['ARABIC'] = 'ar'
	dDetectLanguages['ARMENIAN'] = 'hy'
	dDetectLanguages['AZERBAIJANI'] = 'az'
	dDetectLanguages['BASQUE'] = 'eu'
	dDetectLanguages['BELARUSIAN'] = 'be'
	dDetectLanguages['BENGALI'] = 'bn'
	dDetectLanguages['BIHARI'] = 'bh'
	dDetectLanguages['BULGARIAN'] = 'bg'
	dDetectLanguages['BURMESE'] = 'my'
	dDetectLanguages['CATALAN'] = 'ca'
	dDetectLanguages['CHEROKEE'] = 'chr'
	dDetectLanguages['CHINESE'] = 'zh'
	dDetectLanguages['CHINESE_SIMPLIFIED'] = 'zh-CN'
	dDetectLanguages['CHINESE_TRADITIONAL'] = 'zh-TW'
	dDetectLanguages['CROATIAN'] = 'hr'
	dDetectLanguages['CZECH'] = 'cs'
	dDetectLanguages['DANISH'] = 'da'
	dDetectLanguages['DHIVEHI'] = 'dv'
	dDetectLanguages['DUTCH'] = 'nl'
	dDetectLanguages['ENGLISH'] = 'en'
	dDetectLanguages['ESPERANTO'] = 'eo'
	dDetectLanguages['ESTONIAN'] = 'et'
	dDetectLanguages['FILIPINO'] = 'tl'
	dDetectLanguages['FINNISH'] = 'fi'
	dDetectLanguages['FRENCH'] = 'fr'
	dDetectLanguages['GALICIAN'] = 'gl'
	dDetectLanguages['GEORGIAN'] = 'ka'
	dDetectLanguages['GERMAN'] = 'de'
	dDetectLanguages['GREEK'] = 'el'
	dDetectLanguages['GUARANI'] = 'gn'
	dDetectLanguages['GUJARATI'] = 'gu'
	dDetectLanguages['HEBREW'] = 'iw'
	dDetectLanguages['HINDI'] = 'hi'
	dDetectLanguages['HUNGARIAN'] = 'hu'
	dDetectLanguages['ICELANDIC'] = 'is'
	dDetectLanguages['INDONESIAN'] = 'id'
	dDetectLanguages['INUKTITUT'] = 'iu'
	dDetectLanguages['ITALIAN'] = 'it'
	dDetectLanguages['JAPANESE'] = 'ja'
	dDetectLanguages['KANNADA'] = 'kn'
	dDetectLanguages['KAZAKH'] = 'kk'
	dDetectLanguages['KHMER'] = 'km'
	dDetectLanguages['KOREAN'] = 'ko'
	dDetectLanguages['KURDISH'] = 'ku'
	dDetectLanguages['KYRGYZ'] = 'ky'
	dDetectLanguages['LAOTHIAN'] = 'lo'
	dDetectLanguages['LATVIAN'] = 'lv'
	dDetectLanguages['LITHUANIAN'] = 'lt'
	dDetectLanguages['MACEDONIAN'] = 'mk'
	dDetectLanguages['MALAY'] = 'ms'
	dDetectLanguages['MALAYALAM'] = 'ml'
	dDetectLanguages['MALTESE'] = 'mt'
	dDetectLanguages['MARATHI'] = 'mr'
	dDetectLanguages['MONGOLIAN'] = 'mn'
	dDetectLanguages['NEPALI'] = 'ne'
	dDetectLanguages['NORWEGIAN'] = 'no'
	dDetectLanguages['ORIYA'] = 'or'
	dDetectLanguages['PASHTO'] = 'ps'
	dDetectLanguages['PERSIAN'] = 'fa'
	dDetectLanguages['POLISH'] = 'pl'
	dDetectLanguages['PORTUGUESE'] = 'pt'
	dDetectLanguages['PUNJABI'] = 'pa'
	dDetectLanguages['ROMANIAN'] = 'ro'
	dDetectLanguages['RUSSIAN'] = 'ru'
	dDetectLanguages['SANSKRIT'] = 'sa'
	dDetectLanguages['SERBIAN'] = 'sr'
	dDetectLanguages['SINDHI'] = 'sd'
	dDetectLanguages['SINHALESE'] = 'si'
	dDetectLanguages['SLOVAK'] = 'sk'
	dDetectLanguages['SLOVENIAN'] = 'sl'
	dDetectLanguages['SPANISH'] = 'es'
	dDetectLanguages['SWAHILI'] = 'sw'
	dDetectLanguages['SWEDISH'] = 'sv'
	dDetectLanguages['TAJIK'] = 'tg'
	dDetectLanguages['TAMIL'] = 'ta'
	dDetectLanguages['TAGALOG'] = 'tl'
	dDetectLanguages['TELUGU'] = 'te'
	dDetectLanguages['THAI'] = 'th'
	dDetectLanguages['TIBETAN'] = 'bo'
	dDetectLanguages['TURKISH'] = 'tr'
	dDetectLanguages['UKRAINIAN'] = 'uk'
	dDetectLanguages['URDU'] = 'ur'
	dDetectLanguages['UZBEK'] = 'uz'
	dDetectLanguages['UIGHUR'] = 'ug'
	dDetectLanguages['VIETNAMESE'] = 'vi'
	dDetectLanguages['UNKNOWN'] = ''
	
	dTranslateLanguages = {}
	dTranslateLanguages['Arabic'] = ''
	dTranslateLanguages['Bulgarian'] = ''
	dTranslateLanguages['Chinese'] = ''
	dTranslateLanguages['Catalan'] = ''
	dTranslateLanguages['Croatian'] = ''
	dTranslateLanguages['Czech'] = ''
	dTranslateLanguages['Danish'] = ''
	dTranslateLanguages['Dutch'] = ''
	dTranslateLanguages['English'] = ''
	dTranslateLanguages['Filipino'] = ''
	dTranslateLanguages['Finnish'] = ''
	dTranslateLanguages['French'] = ''
	dTranslateLanguages['German'] = ''
	dTranslateLanguages['Greek'] = ''
	dTranslateLanguages['Hebrew'] = ''
	dTranslateLanguages['Hindi'] = ''
	dTranslateLanguages['Indonesian'] = ''
	dTranslateLanguages['Italian'] = ''
	dTranslateLanguages['Japanese'] = ''
	dTranslateLanguages['Korean'] = ''
	dTranslateLanguages['Latvian'] = ''
	dTranslateLanguages['Lithuanian'] = ''
	dTranslateLanguages['Norwegian'] = ''
	dTranslateLanguages['Polish'] = ''
	dTranslateLanguages['Portuguese'] = ''
	dTranslateLanguages['Romanian'] = ''
	dTranslateLanguages['Russian'] = ''
	dTranslateLanguages['Spanish'] = ''
	dTranslateLanguages['Serbian'] = ''
	dTranslateLanguages['Slovak'] = ''
	dTranslateLanguages['Slovenian'] = ''
	dTranslateLanguages['Swedish'] = ''
	dTranslateLanguages['TURKISH'] = 'tr'
	dTranslateLanguages['Ukrainian'] = ''
	dTranslateLanguages['Vietnamese'] = ''
	dTranslateLanguages['Unknown'] = ''
	
	dLanguages = {}
	for sLanguage in dDetectLanguages.keys():
		if dTranslateLanguages.has_key(sLanguage):
			sAbbreviation = dDetectLanguages[sLanguage]
			dLanguages[sLanguage] = sAbbreviation
		
	dReverseLanguages = {}
	for sKey, sValue in dDetectLanguages.items(): dReverseLanguages[sValue] = sKey
	# dlgTranslate = lbc.Dialog(title='Translate Language')
	lNames = [sKey for sKey in dDetectLanguages.keys()]
	lValues = lNames[:]
	# lstSource = dlgTranslate.AddListBox(label='Source', names=lNames, values=lValues, sort=True)
	sSourceNames = '\r\n'.join(lNames)
	sSourceNames = '[[Source]]\r\n' + sSourceNames
	sSourceLanguage = ReadValue('SourceLanguage')
	if sText: sSourceLanguage = 'UNKNOWN'
	# if sSourceLanguage: lstSource.SetStringSelection(sSourceLanguage)
	lNames = [sKey for sKey in dTranslateLanguages.keys()]
	lValues = lNames[:]
	sTargetNames = '\r\n'.join(lNames)
	sTargetNames = '[[Target]]\r\n' + sTargetNames
	# lstTarget = dlgTranslate.AddListBox(label='Target', names=lNames, values=lValues, sort=True)
	sTargetLanguage = ReadValue('TargetLanguage')
	if not sTargetLanguage: sTargetLanguage = 'ENGLISH'
	# if sTargetLanguage: lstTarget.SetStringSelection(sTargetLanguage)
	# dlgTranslate.AddBand()
	sMemoText = '[[Text]]\r\n' + sText
	# memoContent = dlgTranslate.AddMemo(label='Text', value=sText)
	if dlgTranslate.Complete() != wx.ID_OK: return
	sSourceLanguage = lstSource.GetStringSelection()
	WriteValue('SourceLanguage', sSourceLanguage)
	sTargetLanguage = lstTarget.GetStringSelection().upper()
	WriteValue('TargetLanguage', sTargetLanguage)
	sSourceText = memoContent.GetValue()
	sDetectedLanguage = sSourceLanguage
	sText = sTargetLanguage
	sSourceLanguage = dDetectLanguages[sSourceLanguage.upper()]
	# print 'source', sSourceLanguage
	sTargetLanguage = dDetectLanguages[sTargetLanguage.upper()]
	sAddress = 'http://ajax.googleapis.com/ajax/services/language/translate'
	dData = {'q' : sSourceText, 'v' : '1.0', 'langpair' : sSourceLanguage + '|' + sTargetLanguage}
	dHeaders = {'Referer' : 'http://EmpowermentZone.com'}
	sResponse = WebRequestPost(sAddress, dData, dHeaders)
	try: 
		sResponse = WebRequestPost(sAddress, dData, dHeaders)
		# print 'response\n', sResponse
		dResponse = simplejson.loads(sResponse)
		# print dResponse
		dResult = dResponse['responseData']
		try: sDetectedLanguage = dResult['detectedSourceLanguage']; sDetectedLanguage = dReverseLanguages[sDetectedLanguage]
		except: pass
		sTargetText = dResult['translatedText']
		# print sTargetText
	except: return AddStatus('Error!')
	sText = 'Translation from ' + sDetectedLanguage + ' to ' + sText + '\r\n' + sSourceText + '\r\n\r\n'
	sText += sTargetText.strip()
	memo.SetValue(sText)
	memo.SetFocus()
	memo.SetInsertionPoint(0)

def GetLegislators(sZip):
	sText = ''
	sunlightapi.sunlight.apikey = '1bf1192622ffc0bc7bc1de1bb470daf5'
	iCount = 0
	for legislator in sorted(sunlightapi.sunlight.legislators.allForZip(sZip)):
		# print iCount
		iCount += 1
		sText += '\r\n' + utf16str(legislator) + '\r\n'
		for sAttribute in sorted(legislator.__dict__.keys()): sText += sAttribute + ': ' + utf16str(legislator.__dict__[sAttribute]) + '\r\n'
		for com in sorted(sunlightapi.sunlight.committees.allForLegislator(legislator.bioguide_id)):
			sText += utf16str(com) + '\r\n'
			for sc in sorted(com.subcommittees): sText += utf16str(sc) + '\r\n'

	sText = sText.strip()
	sText = Pluralize('Member', iCount) + ' of Congress\r\nFor zip code ' + sZip + '\r\n\r\n' + sText
	return sText

def RemoveHtmlTags(sHtml):
	sMatch = r'\<.*?\>'
	sReplace = ''
	sReturn = RegExpReplace(sHtml, sMatch, sReplace)
	sReturn = sReturn.strip()
	return sReturn
	
def FormatDecimal(n):
	sNumber = str(n)
	sMatch = r'(\.\d\d)\d+'
	sReplace = r'$1'
	sReturn = RegExpReplace(sNumber, sMatch, sReplace)
	return sReturn

def old_RegExpExtract(sText, sMatch):
	oRegExp = re.compile(sMatch, re.I)
	lReturn = oRegExp.findall(sText)
	return lReturn

def old_RegExpReplace(sText, sMatch, sReplace):
	oRegExp = re.compile(sMatch, re.I)
	sReturn = oRegExp.sub(sReplace, sText)
	return sReturn

def ParseHtml(sHtml):
	oParser = htmllib.HTMLParser(None)
	# print type(oParser)
	oParser.save_bgn()
	oParser.feed(sHtml)
	sText = oParser.save_end()
	return sText

def GoogleAddressToLatLng(sAddress):
	oMaps = googlemaps.GoogleMaps()
	dResult = oMaps.geocode(sAddress)
	dPlacemark = dResult['Placemark'][0]
	iLng, iLat = dPlacemark['Point']['coordinates'][0:2]
	return (iLat, iLng)

def GetGoogleLocalSearch(sKeywords, sAddress):
	oMaps = googlemaps.GoogleMaps()
	oSearch = oMaps.local_search(sKeywords + ' ' + sAddress)
	aPlaces = oSearch['responseData']['results']
	sText = ''
	for dPlace in aPlaces: 
		sText += dPlace['titleNoFormatting'] + '\r\n'
		sText += dPlace['streetAddress'] + '\r\n'
		# if len(dPlace['phoneNumbers']): sText += dPlace['phoneNumbers'][0]['number'] + '\r\n'
		if len(dPlace.get('phoneNumbers', [])): sText += dPlace['phoneNumbers'][0]['number'] + '\r\n'
		# for dPhone in dPlace['phoneNumbers']: sText += dPhone['number'] + ' ' + dPhone['type'] + '\r\n'
		sText += '\r\n'

	for dPlace in aPlaces:
		for sKey in dPlace: print sKey + ': ' + type(dPlace[sKey]).__name__
	sText = sText.strip()
	return (len(aPlaces), sText)
	
def GetGoogleDrivingDirections(sStartAddress, sEndAddress):
	oMaps = googlemaps.GoogleMaps()
	oDirections = oMaps.directions(sStartAddress, sEndAddress)
	sDistance = oDirections['Directions']['Distance']['meters']
	sDuration = oDirections['Directions']['Duration']['seconds']
	lSteps = []
	for oStep in oDirections['Directions']['Routes'][0]['Steps']: lSteps.append(oStep['descriptionHtml'])
	return (sDistance, sDuration, lSteps)
	
def old_WebRequestPost(sUrl, dData=None, dHeaders=None, sUser=None, sPassword=None):
	sData = None
	if dData: sData = utf8urlencode(dData)
	# print 'data', sData
	oRequest = urllib2.Request(sUrl, sData)
	oRequest.add_header(u'Authorization', t.auth)
	for sKey, sValue in dHeaders.items(): oRequest.add_header(sKey, sValue)
	fResponse = urllib2.urlopen(oRequest)
	sResponse = fResponse.read()
	fResponse.close()
	return sResponse
	
def old_WebRequestGet(sUrl, dData=None, dHeaders=None, sUser=None, sPassword=None):
	sData = None
	if dData: sData = utf8urlencode(dData)
	if dData: sUrl += '?' + sData
	# print sUrl
	oRequest = urllib2.Request(sUrl)
	fResponse = urllib2.urlopen(oRequest)
	sResponse = fResponse.read()
	fResponse.close()
	return sResponse
	
def SetClipboardText(sText):

	win32clipboard.OpenClipboard()
	win32clipboard.EmptyClipboard()
	win32clipboard.SetClipboardText(sText)
	# win32clipboard.SetClipboardData(win32con.CF_TEXT, sText)
	win32clipboard.CloseClipboard()
	return

def GetClipboardText():
	win32clipboard.OpenClipboard()
	# sReturn = win32clipboard.GetClipboardText()
	sReturn = win32clipboard.GetClipboardData(win32con.CF_TEXT)
	win32clipboard.CloseClipboard()
	return sReturn

def GetEpochTime(sCreatedAt):
	try: dt = datetime.strptime(sCreatedAt + ' UTC', '%a %b %d %H:%M:%S +0000 %Y %Z')
	except: dt = datetime.strptime(sCreatedAt, '%a, %d %b %Y %H:%M:%S +0000')
	return dt

# def StrDefault(sText): return sText.encode(sDefaultEncoding, 'ignore')
def StrDefault(sText):
	if not isinstance(sText, unicode): sText = unicode(sText, 'utf-8', 'ignore')
	return sText.encode(sDefaultEncoding, 'ignore')

def utf16str(sText):
	if sText == None: return''
	# elif isinstance(sText, bool) or isinstance(sText, int): return utf16str(sText)
	elif isinstance(sText, unicode): return unicode(sText)
	# elif isinstance(sText, str): return unicode(sText, sDefaultEncoding, 'ignore')
	elif isinstance(sText, str): return unicode(sText, 'utf-8', 'ignore')
	else: return unicode(sText)

def utf8str(sText): return utf16str(sText).encode('utf-8', 'ignore')

def utf8urlencode(l): return urllib.urlencode([(utf8str(k), utf8str(v)) for k, v in dict(l).items()])

def MailEncode(text):
	sBody = text
	sBody = sBody.replace('\r\n', '\r')
	sBody = sBody.replace('\n', '\r')
	sBody = sBody.replace('\r', '\r\n')
	sBody = sBody.replace('\r\n', '%0D%0A')
	sBody = sBody.replace(' ', '%20')
	sBody = sBody.replace('\t', '%09')
	sBody = sBody.replace('"', '%22')
	sBody = sBody.replace("'", '%27')
	sBody = sBody.replace('\\', '%5C')
	return sBody
	
def FormatDateTime(sCreatedAt):
	sFormat = '%A, %B %d, %Y at %I:%M %p'
	try: dt = datetime.strptime(sCreatedAt, '%a %b %d %H:%M:%S +0000 %Y')
	except: dt = datetime.strptime(sCreatedAt, '%a, %d %b %Y %H:%M:%S +0000')
	iDelta = (datetime.utcnow() - datetime.now()).seconds / 3600
	if iDelta >=0: dt -= timedelta(hours=iDelta)
	else: dt += timedelta(hours=iDelta)
	sText = dt.strftime(sFormat)
	return sText

def GetDateAndTime(sCreatedAt):
	# print 'created_at', sCreatedAt
	sDateFormat = '%A, %B %d, %Y'
	sTimeFormat = '%I:%M %p'
	# try: dt = datetime.strptime(sCreatedAt, '%a %b %d %H:%M:%S +0000 %Y')
	try: dt = datetime.strptime(sCreatedAt + ' UTC', '%a %b %d %H:%M:%S +0000 %Y %Z')
	except: dt = datetime.strptime(sCreatedAt, '%a, %d %b %Y %H:%M:%S +0000')
	sDate = dt.strftime(sDateFormat)
	iDelta = (datetime.utcnow() - datetime.now()).seconds / 3600
	if iDelta >=0: dt -= timedelta(hours=iDelta)
	else: dt += timedelta(hours=iDelta)
	sTime = dt.strftime(sTimeFormat)
	return sDate, sTime

def GetRelativeTime(sCreatedAt):
	try: dt = datetime.strptime(sCreatedAt, '%a %b %d %H:%M:%S +0000 %Y')
	except: dt = datetime.strptime(sCreatedAt, '%a, %d %b %Y %H:%M:%S +0000')
	dtUtc = datetime.utcnow()
	delta = dtUtc - dt
	iDays = delta.days
	iSeconds = delta.seconds
	iHours = iSeconds / 3600
	if iHours < 1: iHours = 0
	iSeconds -= iHours * 3600
	iMinutes = iSeconds / 60
	if iMinutes < 1: iMinutes = 0
	iSeconds -= 60* iMinutes
	sText = ''
	if iDays: sText += Pluralize('day', iDays) + ', '
	if iHours: sText += Pluralize('hour', iHours) + ', '
	if not sText and iMinutes: sText += Pluralize('minute', iMinutes) + ', '
	if not sText and iSeconds: sText += Pluralize('second', iSeconds) + ', '
	sText = sText.strip(', ') + ' ago'
	return sText

def isattr(item, attribute): return hasattr(item, attribute) and getattr(item, attribute)

def str2int(s):
	try: i = int(s)
	except: i = 0
	return i

def bool2str(b): return 'y' if b else 'n'

def str2bool(s):
	sText = s.strip().lower()
	i = (s in ('yes', 'y', 'true', 't', 'on'))
	# print  type(i), i
	if i: return True
	try: i = abs(int(s))
	except: i = 0
	if i > 0: return True
	else: return False

def Pluralize(sText, iCount): return utf16str(iCount) + ' ' + sText + ('' if iCount == 1 else 's')

def GetDictionaryText(d, lKeys=None):
	sText = ''
	if not lKeys: lKeys = d.keys()
	for sKey in lKeys:
		if not d.has_key(sKey): continue
		sValue = utf16str(d[sKey])
		sValue = sValue.strip()
		if not sValue or sValue == 'None': continue
		sKey = utf16str(sKey)
		sKey = sKey.replace('_', ' ').title()
		sText += sKey +': ' + sValue + '\n'

	sText = sText.strip()
	return sText

def old_WriteValue(key, value, ini='', section='', cfg_ini=None):
	if not ini: ini = file_ini
	if not section: section = 'Configuration'
	# print 'ini', ini
	if not cfg_ini: cfg_ini = ConfigObj(ini, list_values=False, create_empty=False, interpolation=False, indent_type='', write_empty_values= False, encoding='utf-8', default_encoding=sDefaultEncoding)
	if not cfg_ini.has_key(section): cfg_ini[section] = {}
	cfg_ini[section][key] = value
	# cfg_ini.BOM = True
	cfg_ini.BOM = False
	cfg_ini.write()

def old_ReadValue(key='', default='', ini='', section='', cfg_ini=None):
	if not ini: ini = file_ini
	if not section: section = 'Configuration'
	# print 'ini', ini
	if not cfg_ini: cfg_ini = ConfigObj(ini, list_values=False, create_empty=False, interpolation=False, indent_type='', write_empty_values= False, encoding='utf-8', default_encoding=sDefaultEncoding)
	if not cfg_ini.has_key(section): return default
	if not cfg_ini[section].has_key(key): return default
	# print 'key', key, 'default', default, 'ini', ini, 'section', section
	return cfg_ini[section][key].replace('"', '').strip()

def WriteBinaryFile(sFile, sBinary):
	try:
		fFile = open(sFile, 'wb')
		fFile.write(sBinary)
		fFile.close()
	except:
		pass

def clearScreen():
	os.system('cls')

def errorOutput(exType, exValue, sTraceback): return output(sTraceback)

def output(sText):
	# sText = repr(sText)
	sText = str(sText)
	lLines = textwrap.wrap(sText) 
	# lLines = sText.split('\n')
	# print 'lines', len(lLines)
#	try:
#		i = lLines.index('ValueError: I/O operation on closed file\r')
#		del(lLines[0 : i])
#	except:
#		pass
	iLen = len(lLines)
	i = 0
	iMax = 23
	while i < iLen:
		while i < min(iLen, iMax):	
			print lLines[i]
			if len(sLog) > 0:
				fileLog = open(sLog, 'a')
				fileLog.write(lLines[i] + '\r\n')
				fileLog.close()

			i = i + 1

		if i == iLen: return
		sChoice = raw_input('More?')
		if sChoice == 'n': return
		del(lLines[0 : 23])
		if sChoice == 'a': iMax = iLen
		iLen -= 23
		i = 0

# Main routine
sAppDir = sys.prefix
if sAppDir.lower() == r'c:\python25': sAppDir = r'C:\InPy'
sAppDir = PathGetShort(sAppDir)
sIniFormExe = PathCombine(sAppDir, 'IniForm.exe')
# sIniFormExe = PathGetShort(sIniFormExe)

iCount = len(sys.argv)
if iCount > 1: sCodeFile = sys.argv[1]
else: sCodeFile = PathCombine(sAppDir, 'InPy.py')
try: sCodeFile = PathGetLong(sCodeFile)
except: pass
sCodeBase = PathGetBase(sCodeFile)
if iCount > 2: sInputFile = sys.argv[2]
else: sInputFile = PathCombine(sAppDir, sCodeBase + '.ini')
if iCount > 3: sOutputFile = sys.argv[3]
else: sOutputFile = PathCombine(sAppDir, sCodeBase + '.txt')
if iCount > 1:
	execfile(sCodeFile)
	# eval(FileToString(sCodeFile))
	sys.exit(0)

# Main

console = code.InteractiveConsole()
sTitle = 'InPy'
sys.ps1 = '> '
sys.ps2 = '. '
sys.displayhook = output
sys.excepthook = errorOutput
sLog = ''
sProgram = sys.argv[0]
aParts = os.path.split(sProgram)
sProgramDir = aParts[0]
if len(sProgramDir) == 0: sProgramDir = os.getcwd()

win32console.SetConsoleTitle(sTitle)
hWindow = win32console.GetConsoleWindow()
# win32gui.ShowWindow(hWindow, win32con.SW_SHOWMAXIMIZED)
clearScreen()

output('Interactive Python\nEnter help for options\n')
# console.interact(sTitle)

sPrompt = sys.ps1
while True:
	sLine = raw_input(sPrompt)

	if sLine == 'help':
		clearScreen()
		sFile = os.path.join(sProgramDir, 'InPyHelp.txt')
		sText = ReadTextFile(sFile)
		output(sText)
	elif sLine.startswith('exec '):
		sFile = sLine[5:]
		try:
			execfile(sFile)
		except:
			pass
	elif sLine == 'log off': sLog = ''
	elif sLine.startswith('log '):
		sLog = sLine[4:]
		try:
			fileLog = open(sLog, 'w')
			fileLog.write('')
			fileLog.close()
		except:
			sLog = ''
			output('Cannot create file!')
	elif sLine == 'cls': clearScreen()
	elif sLine == 'exit' or sLine == 'quit': sys.exit()
	else:
		"""
		fileOldStdOut = sys.stdout
		fileStdOut = open('output.tmp', 'w')
		sys.stdout = fileStdOut
		fileOldStdErr = sys.stderr
		sys.stderr = fileStdOut
		"""

		if console.push(sLine): sPrompt = sys.ps2
		else: sPrompt = sys.ps1
		"""
		fileStdOut.close()
		fileStdOut = open('output.tmp')
		sText = fileStdOut.read()
		fileStdOut.close()
		sys.stdout = fileOldStdOut
		sys.stderr = fileOldStdErr
		"""
		# output(sText)

