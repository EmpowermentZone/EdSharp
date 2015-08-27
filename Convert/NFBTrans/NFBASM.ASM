;for medium model
arg1 equ [bp+6]
arg2 equ [bp+8]
arg3 equ [bp+10]
dgroup group _data
assume cs:nfbasm_text,ds:dgroup
nfbasm_text SEGMENT byte public 'CODE'    ;begin code segment

;void _sound(frequency)
;int frequency;
public _sound,_nosound,_delay

_sound PROC far      ;begin main program

;interface to C
   push  bp
   mov   bp,sp
mov bx,arg1;get frequency
or bx,bx ;is frequency 0?
jz done_sound
mov ax,34ddh
mov dx,0012h
cmp dx,bx
jnb done_sound
div bx
mov bx,ax
in al,61h
test al,03h
jnz a
or al,03h
out 61h,al
mov al,0b6h
       out 43h,al
a:       mov al,bl
       out 42h,al
       mov al,bh
       out 42h,al
       done_sound:
       pop bp
ret
_sound endp

_nosound proc far

;interface to C
   push  bp
   mov   bp,sp
;turn off sound
in al,61h
and al,0fch
out 61h,al
pop bp
ret
_nosound endp

_delay proc far
push bp
mov bp,sp
mov cx,arg1 ;get duration
jcxz exit_delay
again:
push cx
mov cx, _timer
again1:loop again1
pop cx
loop again
exit_delay:
      pop bp
ret
_delay endp

nfbasm_text ends ;end of code segment

_data segment byte public 'DATA'
extrn _timer:word
_data ends
end

