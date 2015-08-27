#ifdef DJGPP
#undef unix
#endif
#if defined(unix) || defined(DJGPP)     /* Only use if we're compiling under
                                         * Unix */
#include <stdio.h>
#include <string.h>
#include <sys/types.h>
#endif
#ifdef unix
#ifdef aix
#include <termios.h>
#include <sys/select.h>
#include <sys/access.h>
#else
#include <sys/termios.h>
#endif             /* aix */
#include <sys/stat.h>
#include <sys/time.h>
#include <unistd.h>
#endif             /* unix */
#include <windows.h>
#define CASE_SHIFT 32   /* diference between upper and lower case */
#define SYSVR4     /* use with gcc and solaris 2.1 see getpgrp. */
#ifdef ultrix
#define ECHOCTL TCTLECH
#endif

#ifdef unix
struct termios original_stdin;
#endif


//char *strlwr(char *string)
/*Takes the indicated string and changes uppercase letters to lowercase
ones.  All other characters are unaffected.  Returns a pointer to the
string.*/
/*
{
  int i;

  for (i = 0; string[i]; i++)   // for the entire string
  {
    if ((string[i] >= 'A') && (string[i] <= 'Z'))
    {
      string[i] = string[i] + CASE_SHIFT;
    }
  }
  return (string);
}                  // strlwr 
*/

//char *strupr(char *string)
/*Takes the indicated string and changes lowercase letters to uppercase
ones.  All other characters are unaffected.  Returns a pointer to the
string.*/
/*
{
  int i;
  for (i = 0; string[i]; i++)
  {
    if ((string[i] >= 'a') && (string[i] <= 'z'))
    {
      string[i] = string[i] - CASE_SHIFT;
    }
  }
  return (string);
}                  // strupr 
*/

int strcmpi(char *s1, char *s2)
{
  char temp1[256], temp2[256];
  strcpy(temp1, s1);
  strlwr(temp1);
  strcpy(temp2, s2);
  strlwr(temp2);
  return (strcmp(temp1, temp2));
}                  /* strcmpi */

void strnset(char *string, char c, short n)
/*sets the first n or length of string characters to c whichever is less*/
{
  short i;
  for (i = 0; (i < n && string[i] != '\0'); i++)
    string[i] = c;
}                  /* strnset */

#ifdef unix
int inforeground()
/*Returns 1 if the program is running in the foreground, 0 otherwise.*/
{
  int i;

  i = tcgetpgrp(0);/* Use stdin as the file descriptor */
#ifdef SYSVR4
  if (i == getpgrp())
    return (1);
#else
  if (i == getpgrp(0))
    return (1);
#endif             /* sysv */
  else
    return (0);
}                  /* inforeground */

int getch()
/*Reads a character from standard input.  It doesn't require a carriage
return. No echo to stdout.*/
{
  tcflag_t oldval; /* Value of the terminal flags */
  tcflag_t mask = ECHO | ECHOCTL | ICANON;      /* bits to turn off
                                                 * temporarily */
  char charin;     /* input character */
  cc_t oldmin;     /* save original min */
  struct termios termset;       /* What is our terminal set to */

  tcgetattr(0, &termset);
  oldval = termset.c_lflag;
  oldmin = termset.c_cc[VMIN];
  termset.c_lflag |= mask;      /* turn canonical input and echo on */
  termset.c_lflag ^= mask;      /* turn them off now */
  termset.c_cc[VMIN] = 1;       /* fgetc will return after first character */
  tcsetattr(0, TCSANOW, &termset);
  charin = (char) fgetc(stdin);
  termset.c_lflag = oldval;     /* restore original settings */
  termset.c_cc[VMIN] = oldmin;
  tcsetattr(0, TCSANOW, &termset);
  fprintf(stderr, " \010");
  return ((int) charin);
}                  /* getch */

int getche()
/*Reads a character from standard input, ecoes to standard output, doesn't
require a carriage return.*/
{
  tcflag_t oldval; /* Value of the terminal flags */
  tcflag_t mask = ICANON | ECHOCTL;
  cc_t oldmin;     /* save original min */
  char charin;     /* input character */
  struct termios termset;       /* What is our terminal set to */

  tcgetattr(0, &termset);
  oldval = termset.c_lflag;
  oldmin = termset.c_cc[VMIN];
  /* turn off canonical and echo control */
  termset.c_lflag |= mask;      /* turn both on */
  termset.c_lflag ^= mask;      /* turn both off */
  termset.c_cc[VMIN] = 1;
  tcsetattr(0, TCSANOW, &termset);
  charin = (char) fgetc(stdin);
  termset.c_lflag = oldval;     /* restore original settings */
  termset.c_cc[VMIN] = oldmin;  /* restore original min */
  tcsetattr(0, TCSANOW, &termset);
  return ((int) charin);
}

void unbuf_stdin()
{
  tcflag_t mask = ICANON;       /* bits to turn off temporarily */
  struct termios termset;       /* What is our terminal set to */

  tcgetattr(0, &termset);
  termset.c_lflag |= mask;      /* turn canonical input on */
  termset.c_lflag ^= mask;      /* turn them off now */
  termset.c_cc[VMIN] = 1;       /* fgetc will return after first character */
  tcsetattr(0, TCSANOW, &termset);
}                  /* unbuf_stdin */

void save_stdin()
{
  tcgetattr(0, &original_stdin);
}                  /* save_stdin */

void restore_stdin()
{
  tcsetattr(0, TCSANOW, &original_stdin);
}                  /* restore_stdin */

long filelength(descriptor)
  int descriptor;
/*Returns the length of the associated file in bytes.  If the descriptor is
bad, returns -1 and errno is set to the proper number.*/
{
  struct stat finfo;    /* File information here */
  if (fstat(descriptor, &finfo))
  {
    return (EOF);
  }
  return (finfo.st_size);
}                  /* filelength */

#ifndef linux
#endif

void beep(count)
  int count;
/*Beeps the terminal bell the specified number of times.*/
{
#ifndef linux
  static char tbuf[1024];       /* terminal buffer */
  static have_ent = 0;  /* We want to know if we need to get terminal entry */
  static char *blstr;   /* pointer for bell string */
  static char *ptr = tbuf;
  int i;

  if (!have_ent)
  {
    if ((tgetent(&tbuf, (char *) getenv("TERM"))) == 1)
    {
      blstr = (char *) tgetstr("bl", &ptr);
      if (blstr)
        have_ent = 1;
    }
  }
  if ((have_ent) && (inforeground()))
  {
    if (isatty(fileno(stdout)))
    {
      for (i = 0; i < count; i++)       /* beep count number of times */
      {
        fprintf(stdout, "%s", blstr);
      }
    }
  }
#endif
  return;
}

int kbhit()
/*This function emulates the kbhit function under  MS-DOS.  If the standard
input is ready for reading, then a 1 is returned.  Otherwise, a 0 is
returned, indicating that no input is there.  If this process is not
running in the foreground, then this function returns 0.*/
{
  int i;
  struct timeval timeout;       /* For polling purposes */
  fd_set readfds;  /* Reading flags parm for select */

  if (!inforeground())
    return (0);
  memset(&timeout, 0, sizeof(struct timeval));  /* Zero out the timer */
  FD_ZERO(&readfds);
  FD_SET(0, &readfds);  /* set the descriptors we're interested in */
  return (select(1, &readfds, NULL, NULL, &timeout) > 0);
}                  /* kbhit */

void delay_unix(period)
  unsigned int period;
/*This routine pauses the program for the specified number of milliseconds
before returning.*/
{
  long microperiod;/* amount of time, in microseconds */

  microperiod = period * 100;   /* Convert milliseconds to microseconds */
  return;
}                  /* delay_unix */

#endif
