options: excerpt ignorecase global
; line after form feed
(^|\f\r\n)([^a-zA-Z0-9]*?\n)*.+\r\n

; chapter heading
\r\n\r\n\t*chapter *\w+ *(-|:).*\r\n

; another chapter heading
\r\n\r\n\t*chapter *\w+\r\n

; section heading
\r\n\r\n\t*section *\w+ *(-|:).*\r\n

; another section heading
\r\n\r\n\t*section *\w+\r\n

; part heading
\r\n\r\n\t*part *\w+ *(-|:).*\r\n

; another part heading
\r\n\r\n\t*part *\w+\r\n

; lesson heading
\r\n\r\n\t*lesson *\w+ *(-|:).*\r\n

; another lesson heading
\r\n\r\n\t*lesson *\w+\r\n

; appendix heading
\r\n\r\n\t*appendix *\w+ *(-|:).*\r\n

; another appendix heading
\r\n\r\n\t*appendix *\w+\r\n

