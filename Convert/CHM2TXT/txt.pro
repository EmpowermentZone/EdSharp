options: global
; add carriage return to unpaired line feed
[^\r]\n
\r\n
; remove section break
%s_sb%
\f
; remove document end
%s_eod%$

; remove extra ellipses
\.{4,}
...
; remove space before line break
 +\r\n
\r\n
; remove non alphanum lines at document beginning, then spaces preceding text
^([^A-Za-z0-9]*?(\f|\r\n))* *

; remove space after line break
\r\n +
\r\n
; remove space before page break
 +\f
\f
; remove space after page break
\f +
\f
; remove line break before page break
(\r\n)+\f
\f
; remove line break after page break
\f(\r\n)+
\f
; remove extra page breaks
\f{2,}
\f
; remove nonprintable characters from end
\s+$

; eliminate tab on line with only it
(\f|\n)( |\t)+(\f|\r)
$1$3
; remove duplicate headings
(^|\f)( |\t)*(.+?)( |\t)*(\r\n)+( |\t)*\3
$1$3
; remove extra blank lines
(\r\n){4,}
\r\n\r\n\r\n
; remove extra blank lines after heading
(^|\f)(.+?)(\r\n){3,}
$1$2\r\n\r\n
; replace page break with section break
\f
%s_sb%
; remove extra blank lines before section break
(\r\n)+%s_sb%
%s_sb%
; add document end
$
%s_eod%
