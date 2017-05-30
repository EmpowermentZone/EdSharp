# Perl

use strict;
use warnings;
use Win32::OLE;

my $oST = Win32::OLE->new("Say.Tools");
my $sText = "Hello world";
$oST->Say($sText);
$oST = undef;
