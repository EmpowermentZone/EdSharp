options: ignorecase global
; add carriage return to unpaired line feed
[^\r]\n
\r\n
funcky60.txt
\f(.+)\r\n(.+)(\r\n)+
\f$1 = $2\r\n\r\n
funcky60.txt
^(.+)\r\n(.+)(\r\n)+
$1 = $2\r\n\r\n
funcky60.txt
(\r\n)+FUNCky 6.0(\r\n)+COM Component(\r\n)+
\r\n\r\n
sapi.txt
(^\f).+(\r\n)+Microsoft Speech SDK(\r\n)+SAPI 5\.1(\r\n)+

