#define _CRT_SECURE_NO_WARNINGS
#include <windows.h>
#ifdef DJGPP
#include <pc.h>
#undef unix
#endif
#ifndef unix
#define LINT_ARGS
#define DOS
#else
#define UNIX_PATH "/usr/local/lib/"
#endif             /* unix */
#include	<stdio.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <stdlib.h>
#ifdef DOS
#include <io.h>
#include <string.h>
#include <ctype.h>
#include <fcntl.h>
#include <conio.h>
#include <dos.h>
#include <memory.h>
#include <process.h>
#else
#include <string.h>
#include <fcntl.h>
#include <stdarg.h>
#define MAXARGS 7
#endif             /* DOS */
#if defined(sunos) || defined(linux) || defined(DJGPP)
#define max(a,b)  (((a) > (b)) ? (a) : (b))
#define min(a,b)  (((a) < (b)) ? (a) : (b))
#endif
#include <time.h>
//#include <utime.h>
#include <signal.h>
#include <errno.h>

#ifdef unix
#define CONFIG_FILE "nfbtrans.cnf"      /* For us Unix types who hate
                                         * uppercase */
#define MAXPATHLEN 1023 /* For the longest possible pathname */
#define O_BINARY 0 /* We don't need it here */
/*Definition of mode bits for open*/
#ifndef S_ISUID    /* If we've seen this before, no redefinition */
#define S_ISUID 04000   /* set User id on execution */
#define S_ISGID 02000   /* set Group id on execution */
#define S_ISVTX 01000   /* save text image after termination */
#define S_IREAD 00400   /* read by owner */
#define S_IWRITE 00200  /* write by owner */
#define S_IEXEC 00100   /* execute by owner */
#endif             /* The following are not usually defined */
#define STD_OPEN 00777  /* rwxrwxrwx */
#define delay(a) delay_unix(a)
char *fopen_read[2] = {"r", "r+"};

#define FOPEN_WRITE "w"
#else              /* DOS */
#define CONFIG_FILE "NFBTRANS.CNF"      /* For those DOS heades */
#define STD_OPEN S_IWRITE       /* Standard DOs open */
#define MAXPATHLEN 127  /* for the longest possible path name in DOS */
char *fopen_read[2] = {"rt", "rt+"};

#define FOPEN_WRITE "wt"
#endif             /* unix */

#ifdef _S_IFCHR
#define IFCHR _S_IFCHR
#elif defined(S_IFCHR)
#define IFCHR S_IFCHR
#else
#define IFCHR _IFCHR
#endif

/*defines and defaults*/
#define 	VERSION 	"7.74"
#define DATE "November 4, 2002"
#define COPYRIGHT "Written by the National Federation of the Blind and Randy \
Formenti"
#define MAXTAB		2000    /* number of entries in binary table of
                                 * braille rules */
#define MAX_TABLE_BUF 64000
#define MAX_FILES 50    /* max # of files stored using wildcard spec in dos */
#define MAX_EXTENSIONS 40       /* # of extensions for auto format */
#define MAX_INIT 45/* max length of pre_init & post_init strings */
#define MAX_TOKENS 40   /* max # of token on a line of .efl file */
#define MAX_TOKEN_LEN 40        /* max length of a token in .efl file */
#define MAXWORDLEN 132
#define MAX_TOC_ENTRY 250
#define MAX_EFL_DATA_BUF 1600
#define MAX_EFL_DATA_LEN 40
#define MAX_COL_WIDTH 20        /* in table definition */
#define MAX_DIC_LEN 40  /* max length of word in hyphenation dictionary */
#define MAX_MENU_BUF 1300
#define MAX_INDEX 400
#define TRUE 1
#define FALSE 0
#define BUFSIZE 4096
#define POETRY 1
# define TEXT 2
#define BLOCK 3
#define LISTS 4
#define BLOCK_PARA 5
#define AUTO_FORMAT 6
#define LINE_PARA 7
#define UPPER 2
#define LOWER 1
#define NUMERIC 4
#define NOTRANS 8
#define SKIPTRANS 24
#define UPPER_ALL 34
#define BIT0 1
#define BIT1 2
#define BIT2 4
#define BIT3 8
#define BIT4 16
#define BIT5 32
#define BIT6 64
#define BIT7 128
#define BIT8 256
#define BIT9 512
#define BIT10 1024
#define BIT11 2048
#define BIT12 4096
#define BIT13 8192
#define BIT14 16384
#define BIT15 -32768

/*global variables*/
char temp[256];
char *output_extension[2] = {".brf", ".txt"};
int paramcount;
char **paramstr;

typedef struct
{
  char *name;
  int value;
}   tokentype;

tokentype options[126] = {      /* all allowed options alphabetically */
  "AC", 127, "BE", 127, "BL", 63, "BM", 127, "BP", 127, "CA", 63, "CL", 127,
  "CO", 71, "CS", 127, "CU", 575, "DB", 127, "DE", 127, "DM", 63,
  "DS", 127, "DV", 127, "EF", 103, "EM", 68, "ET", 103, "EX", 2, "FC", 575,
  "FP", 127, "FS", 575, "GD", 111, "GM", 127, "HB", 47, "HC", 127,
  "HD", 47, "HK", 67, "HL", 111, "HM", 111, "HN", 111, "HP", 111,
  "HT", 3, "HX", 111, "I0", 2, "I1", 2, "I2", 2, "I3", 2, "I4", 2,
  "I5", 2, "I6", 2, "I7", 2, "I8", 2, "I9", 2, "I:", 2, "I;", 2, "i<", 2, "I=", 2,
  "I>", 2, "i@", 2, "IA", 2, "IB", 2, "IC", 2,
  "ID", 2, "IE", 2, "IF", 15, "IP", 111, "IT", 575, "JF", 127, "KC", 111,
  "KF", 69, "L0", 6, "L1", 6, "L2", 6, "L3", 71, "LB", 127, "LE", 46,
  "LF", 24, "LI", 63, "LM", 127, "LP", 6, "LR", 127, "LS", 47, "LT", 127,
  "M3", 127, "MA", 100, "MF", 3, "MS", 46, "MT", 46, "NC", 67, "NS", 575,
  "OB", 63, "OC", 102, "OD", 63, "OF", 63, "ON", 127, "OW", 67, "PA", 67,
  "PD", 103, "PE", 75, "PF", 103, "PL", 111, "PM", 127, "PN", 2, "PS", 111,
  "PW", 111, "QM", 127, "RC", 4, "RF", 63, "RP", 71, "RW", 111, "S0", 2,
  "SC", 63, "SD", 127, "SI", 2, "SL", 127, "SM", 127, "SN", 63, "SO", 127,
  "SP", 66, "ST", 47, "TC", 2, "TD", 8, "TE", 8, "TF", 27, "TM", 99, "TN", 66,
  "TO", 63, "TP", 6, "TS", 47, "TT", 127, "TV", 66, "UK", 127, "VC", 4,
"VE", 63, NULL, 0};
char *option_types[6] = {"on command line", "in configuration file",
  "in table file", "during translation", "in EFL file",
"in initialization string"};
char *main_menu[12] = {
  "Please select\n",
  " 1 - Translate a Text File\n",
  " 2 - Emboss a File that has already been Translated\n",
  " 3 - Back Translate a Grade Two file\n",
  "Choice?",
  "\n 1 - Translate and store in a File or\n",
  " 2 - Translate and emboss immediately.\n",
  "Choice?",
  "Starting page?",
  "Enter ending page",
  "\nEnter source File name <RETURN> to exit? ",
"\nEnter name of File to create? "};
char *menu[13], menu_buf[MAX_MENU_BUF];
char *call_prefix[16] =
{"KA", "KB", "KC", "KD", "KE", "KF", "KG", "K", "N", "WA", "WB", "WD", "W", "VE",
"VO", NULL};
char g3_derivatives[20] = {"!#$%&*()-=+[]:'/?"};
char prn[13] = {"prn"};
char stdin_name[13] = {"stdin"};
char blank_line[8] = {" "};
char string_format[4] = {"%s"};
char numeric_def[4] = {"#\000."};
char *progress[2] = {"Skipping", "On"};
char braille_punct[12] = {"012346780-'"};
char leading_punct[8] = {"("};
char lead_back_punct[8] = {".,6780"};
char trailing_punct[16] = {".!?,:;')"};
char end_of_article[16] = {"333333333333"};
char dash = {'-'}, currency_char = {'$'};
char output_dir[128] = {0};

tokentype tokens[24] =
{"D", 1, "T", 2, "LIS", 3, "SK", 4, "C", 5, "IND", 6, "PAG", 80, "FIE", 100, "OM", 101,
  "ST", 102, "A", 199, "MAT", 202, "NMAT", 203, "REPL", 301, "REPS*", 305,
  "REPS", 306, "REPW*", 310, "WORD", 312, "REPW", 311, "OP", 315, "LIN", 320, "G", 325,
NULL, 999};
char *stateid[60] =
{"AK\000Alaska", "AL\000Alabama", "AR\000Arkansas",
  "AZ\000Arizona", "AS\000American Samoa", "CA\000California",
  "CO\000Colorado", "CT\000Connecticut", "CZ\000Canal Zone",
  "DC\000District of Columbia", "DE\000Delaware", "FL\000Florida",
  "GA\000Georgia", "GU\000Guam", "HI\000Hawaii",
  "ID\000Idaho", "IL\000Ilinois", "IN\000Indiana",
  "IA\000Iowa",
  "KS\000Kansas", "KY\000Kentucky", "LA\000Lousiana", "ME\000Maine",
  "MD\000Maryland", "MA\000Massachusetts", "MI\000Michigan",
  "MN\000Minnesota", "MS\000Mississippi", "MO\000Missouri",
  "MT\000Montana", "NE\000Nebraska", "NV\000Nevada",
  "NH\000New Hampshire", "NJ\000New Jersey", "NM\000New Mexico",
  "NY\000New York", "NC\000North Carolina", "ND\000North Dakota",
  "CN\000Northern Mariana Is", "OH\000Ohio", "OK\000Oklahoma",
  "OR\000Oregon", "PA\000Pennsylvania", "PR\000Puerto Rico",
  "RI\000Rhode Island", "SC\000South Carolina", "SD\000South Dakota",
  "TN\000Tennessee", "TT\000Trust Territories", "TX\000Texas",
  "UT\000Utah", ",VT\000Vermont", "VA\000Virginia",
  "VI\000Virgin Islands", "WA\000Washington", "WV\000West Virginia",
"Wi\000Wisconsin", "WY\000Wyoming", NULL};

char *lt_string[4] = {"\r\n", "\r", "\n", " "};

typedef int BOOL;

/*globals for buffered read*/
char iobuf[BUFSIZE + 1];
unsigned int bytes_in_buf;
char *ioptr, *eolptr, *spell_buffer = NULL;
int timer = 0x5000;/* default delay timing value only if linked with
                    * nfbasm.asm */
int emboss_delay = 0, current_table_line;
int hot_key = 0;   /* no hot keys by default */
int ab_flag = 0;   /* abort */
int output_case = 3;    /* output uppercase by default */
int it_flag = 0, math_flag = 0, type17_word_count = 0, type17_table_entry, spanish_flag = 0;
int begin_flag = 0, cap_flag;   /* indicates capital marks during back
                                 * translation */
char word_buf[4][30];
int stdin_tty = 0, stdout_tty = 0;      /* is stdin or stdout 1 keyboard or 0
                                         * file */
int print_date = 0, print_file = 0;     /* prints date and file on first page
                                         * if true */
int current_pass, toc_word, max_input_length, long_flag, input_length, uk_flag = 0;
int spool = 0, charspersec = 40, book_break = 0, output_name = 0;
int keep_together = 0, keep_together_save = 0, keep_format = 0, prog_init;
int file_count, current_file, total_files = 0, total_equations = 0;
int spell_dic_fileh = 0, min_spell_len = 4, cb_flag = 0, capslock = 0;
struct tm *tm;
//struct utimbuf ut;

typedef struct
{
  unsigned int startline, endline;
  BOOL match_active;
  int fopstart, fopend, dummy;
}   loptype;

typedef struct
{
  int fop;         /* operation */
  int fstart;      /* field start */
  int flen;        /* field length */
  char *data;      /* pointer to data if any */
}   foptype;

typedef struct
{                  /* holds binary form of braille rules */
  int typex[MAXTAB + 1];
  int start1[256];
  int start2[27][27];
  char *match[MAXTAB + 1];
  char *replace[MAXTAB + 1];
}   tablet;
char *tablebuf, *end_table_ptr;

typedef struct
{
  int braille, print;
  char roman;
}   toctype;

toctype toc_pages[MAX_TOC_ENTRY], toc_page;
toctype index_pages[MAX_INDEX];

typedef struct
{
  char ext[16];
  int init_val;
}   extension_t;

typedef struct
{
  int total;
  extension_t prog_ext[MAX_EXTENSIONS];
}   prog_ext_t;
prog_ext_t prog_extension;

typedef struct
{
  char pre_init[MAX_INIT + 1], post_init[MAX_INIT + 1], format;
}   init_t;
init_t init[21];   /* for i0 - ie */

typedef char columnt[MAX_COL_WIDTH + 1];

BOOL usr_default = FALSE;
int pagestart = 0, pagestart_save, pageend = -1, pageend_save, total_breaks;
int total_em_pages, total_em_sheets, total_file_pages, total_file_sheets;
int pagestart_roman = 1, pageend_roman = 9999, roman_mode = 1;
char field_[256];
char *field = field_;
int count, copies, lastcopy = 0, rejoin, quiet_mode = 0, book_mode = 3;
int bpagec, pagenumlen, hyphen_searches, dash_searches, hyphen_matches, dash_matches, ham_call = 0, total_ham_calls;
int hyphen_mode = 0, min_hyp_len = 4, hyp_words_added = 0, hyp_int = 5, page_min = 2;
int remove_page_nums, total_removed_pages, hyphens_used, dashes_used, hyp_dic_tested = 0;
int split_word = 0, dash_flag;
int max_consec_hyphens = 3, consec_hyphens, hyphens_skipped, max_hyp_page = 99, hyp_page, hyphenated_line = 0;
char token[MAX_TOKENS][MAX_TOKEN_LEN];
int token_count, token_char;
int current_token;
int fopcount, lopcount;
int lineskips = 99, linesperpage = 25, vbar_fmt, auto_letter = 1, auto_center = 0;
int maxline = 0, make_sound = 1, auto_letter_count, lines_in_header = 0, header_flag = 0;
int display_braille = -1, auto_blanks = 0, auto_indents = 0, auto_punct = 0, scan_lines = 10000;
int display_source = -1, list_break = 0;
BOOL printit = TRUE;
int skip_output = 0, over_write = 0, input_file_arg = 0, start_arg = 1;
int graphics_mode = 0;  /* skip chars with b7 set */
int guide_dots = 2;
int keep_control = 0, expand_tab = 1;
int table_entries, page_breaks[10];
BOOL no_copyright = FALSE;      /* display copyright by default */
int leftmargin = -1, top_margin = 0, top_flag = 0, left_offset = 0;
int capvec[256], subvec[256];
int trans_mode = -1, trans_mode1 = 0, trans_default = 21, stat_mode = 0x5fff;
int pause_time = 0, inf_path_len = 0;
unsigned long total_words, total_dots[8], total_cells, total_lines, emboss_time, number_word;;
unsigned long config_lines, max_input_line_num, total_misspells, line_word;
time_t time1, time2;
long in_length, out_length, total = 0l, total_rejoins, total_not_rejoined;
long tablebuf_offset = 0l, letter_offset[27], table_start_line = 0l;
unsigned int hyphen_line_count;
typedef struct
{
  long pos;
  unsigned int line;
}   dictype;
dictype hyphen_dic_start[191];
int efl_mode = 0;
#ifdef unix
int eol_term = 2;
#else
int eol_term = 0;
#endif
int current_table_grade = -1;
char inf_name[MAXPATHLEN] = {0};
char inf_name_ext[10];
char file_name[MAX_FILES][13];
char hyphen_dic_name[64] = {0};
char hyphen_dic_line[MAX_DIC_LEN + 1];
char part1[MAX_DIC_LEN + 1], part2[MAX_DIC_LEN + 1];
char bad_hyp_fname[64] = {"BAD.DIC"};
char bad_spell_name[64] = {0};
int outf_des = 0;
FILE *intext = NULL, *lfile, *conf_ptr, *hyp_dic_ptr = NULL, *rejoin_out_ptr = NULL;
FILE *dic_out_ptr = NULL, *bad_hyp_ptr = NULL, *table_stat_file = NULL;
FILE *toc_file_ptr = NULL, *option_file = NULL, *stat_file_ptr = NULL, *spell_dic_file_ptr;
int inf_des, inf_des_save = 0, toc_line_count;
char outf_name[MAXPATHLEN] = {0};
char toc_file_name[16] = {"tocfile.$$$"}, toc_line[110];
char toc_format[16] = {"~s2~c~f"}, auto_center_format[16] = {"~s2~c"};
char efl_file[64];
foptype f[40];
char efl_data[MAX_EFL_DATA_BUF];
int efl_data_offset, linecount;
BOOL lopactive = FALSE;
unsigned int inpglen = 66;      /* input page length */
char transpath[MAXPATHLEN + 1] = {0};
char table_file[2][20] = {"braille.tab", "back.tab"};
char active_table[20];
char math_table[20] = {"math.tab"};
char math_symbols[30] = {"=<>"};
char stat_file[MAXPATHLEN + 1] = {0};
char config_file[MAXPATHLEN] = {CONFIG_FILE};
char indent[30] = {"  "};
char format_char = '~';
char ignore_format[20] = {0};
char date_string[20];
char *dayofweek[7] = {"sun", "mon", "tue", "wed", "thu", "fri", "sat"};
char s0_init[20] = {0};
char l0[4] = {0};
char l1[4] = {0};
char *l2[10] = {"DE\000\000\000", "HET\000\000", "EEN\000\000", ""};
char vowels[40] = {"AEIOUY\201\202\203\205\210\211\212\213\214\220\223\225\226\227\232\240\241\242\243"};
char consonants[40] = {"BCDFGHJKLMNPQRSTVWXZ\200\207"};
char *t1[6] = {",TITLE\0\0\0\0", ",PR9T\0\0\0\0\0", ",BRL\0\0\0\0\0\0",
"\0\0\0\0\0\0\0\0\0\0", ",PAGE\0\0\0\0\0", ",PAGE\0\0\0\0\0"};
char table_definition[40] = {0}, table_sscan[25] = {"%s %s %s %s %s %s %s %s"};
char italics[4] = {"_"};
char cap_single[4] = {","};
char cap_all[4] = {",,"};
char vbar = '|';
char letter_sign[4] = {";"};
char quotes[4] = {"\042\047"};
int field_width[8], divide[2] = {0, 0};
int cols_in_table, cols_in_line, chars_in_table, chars_in_line;
loptype l[40];
unsigned char dot_table[128] = {        /* braille dot equivilent for each
                                         * ascii char, used in statistics
                                         * file */
  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,       /* ascii 0-15 b0 = dot
                                                         * 1, b1 = dot 2 ... */
  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,       /* ascii 16-31 */
  0, 46, 16, 60, 43, 41, 47, 4, 55, 62, 33, 44, 32, 36, 40, 12, /* / ascii 32-47 */
  52, 2, 6, 18, 50, 34, 22, 54, 38, 20, 49, 48, 35, 63, 28, 57, /* ? ascii 48-63 */
  8, 65, 67, 73, 89, 81, 75, 91, 83, 74, 90, 69, 71, 77, 93, 85,        /* @-O */
  79, 95, 87, 78, 94, 101, 103, 122, 109, 125, 117, 42, 51, 59, 24, 56, /* _ */
  0, 1, 3, 9, 25, 17, 11, 27, 19, 10, 26, 5, 7, 13, 29, 21,     /* 96-o */
  15, 31, 23, 14, 30, 37, 39, 58, 45, 61, 53, 0, 0, 0, 0, 0,    /* p-127 */
};

unsigned char graph_tab[170] = {
  /* converts ascii representation of braille to ibm graphic characters */
  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,       /* ascii 0-15 */
  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,       /* ascii 16-31 */
  32, 221, 189, 163, 196, 211, 219, 0, 224, 208, 220, 201, 190, 173, 251, 175,  /* 32-47 ' */
  162, 172, 0, 186, 174, 206, 161, 0, 191, 170, 215, 223, 0, 198, 193, 212,     /* 63 27< */
  0, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, /* 64-79 @ */
  240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 188, 197, 252, 253     /* 80-95 */
};

columnt *column = (columnt *) & graph_tab[0];   /* graph_tab unused for now */
char bnumber[] = {"jabcdefghi"};        /* lowercase for 8 dot braille */
char number_back[] = {"1234567890"};
char g3_numbers[44] = {
"JKLMNOPQRSTUVXYZ&=(!(*<%?:$}\\{W1234567890"};
tablet *b;
/*variables for do_translate*/
BOOL done;
char oldword[200];
char oldline[256], oldline6[256];
char tline[256];
char fline[256];
char hbuf[300], *hline[6] = {hbuf};
char bline[256];
char bline6[256];
char wline[256];
int blinec, bpageb, bpageh, linelength, fill_length = 7, auto_toc_flag;
int center_length = 30, heading_length;
int first_page = FALSE;
int curmax, build_level;

unsigned char c, page_sep = '\14';
char words[200];
char bword[200];
char bword6[200];

BOOL join, pjoin, rjoin;
BOOL group;
BOOL pgroup;
BOOL xjoin;
BOOL xgroup;
BOOL xpjoin;
BOOL xpgroup;

int xgrade = -1, grade_mod = 0, default_g3_mod = 255;
BOOL xcenter, find_toc_pages, got_toc_page, last_toc_word = FALSE;
int xformat = -1, xformat_save;
int blank_lines, djoin;
BOOL xdouble, xtab, xacronym, xheading, xfooting, makefoot, makehead, fillit;

BOOL openlevel, closelevel;

int margin, point, firstletter, chardec, lastmatch, prev_type;
char linein[256], *plinein, line_end[4] = {0};
unsigned int lineinct;
BOOL newline;
int quotecount;
BOOL dopagenum = TRUE;
BOOL doroman, dobook, toc_entry, braille_page_nums, roman_flag = 0, arabic_flag = 1;
unsigned char tabtable[256];
BOOL disablecol;
int setmargin, oldmargin;
BOOL tabmargin;
BOOL pageset;
char addchar[11], fill_char = '\042';
int actualpage;
int interpoint = FALSE;
char *roman[10] =
{"", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"};
char *roman10[10] = {"", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"};
struct stat infilestat, outfilestat, hypfilestat;

#ifdef DOS
//struct find_t fileinfo; /* wildcard support for dos */
#endif             /* DOS */

/*function prototypes*/
void top_of_form(void);
void make_top_margin(void);
void page_beep(void);
void get_page_num_length(void);
void backspace_int(int);
void dofillit(void);
void make_roman(int *, char *, int);
void make_arabic(int *, char *);
void make_book(void);
void bpurge(int);
void center(void);
void is_centered(char *);
void write_footer(void);
void format_first_line(void);
void build_line(void);
int split_hyphen_word(int);
void remove_dashes(char *, int);
void remove_page_number(void);
void flush_if_not_blank(void);
void add_case(void);
void add_dots(char *);
int get_integer(char *);
int is_equation(void);
void get_digit(void);
void get_date(int);
void do_commands(int);
void do_advance(void);
void write_fill_line(void);
void make_indent(char *);
void process_fill(char *);
void process_auto_toc(void);
void read_toc_line(void);
void compact_line(char *);
void length_error(char *);
void do_lop_op(foptype *);
void do_lop(void);
void getline(void);
void get_input(char *, int);
int get_paragraph_type(int);
void check_purge(void);
void get_word(void);
void set_vect(char *);
void do_letter(int, char *);
void do_number(char *);
void do_punct(char *);
void test_join(void);
void build_word(char *, char *);
void check_ham_call(char *);
void trans_word(char *);
void undefined_char(void);
BOOL store_next_token(void);
void pop_token(char *);
int check_token(void);
void store_commands(void);
void add_efl_data(char *, foptype *);
void test_range(char *);
void load_template(void);
void do_translate(void);
void translate_computer(char *, char *);
void translate_word(char *, char *);
void write_stat_file(int);
void advance_page(void);
FILE *open_option_file(char *, int);
void load_tables(char *);
int test_bracket(char *, int);
int search_hyphen_dictionary(char *);
int process_rejoined_word(void);
void insert_hyphen_word(int);
void insert_dictionary_word(void);
void get_yn(void);
void insert_report(int, FILE *);
void read_menu(void);
void remove_trailing_punct(char *);
void test_hyphen_dictionary(int);
void translate_file(void);
int get_path_component(char *, int);
int open_input_file(void);
void report_open_error(char *);
int test_file_exist(void);
int test_extension(char *);
int search_extensions(char *);
#ifndef __TURBOC__
#ifndef DJGPP
void delay(int);
void sound(int);
void nosound(void);
BOOL nsound = 0;
#else
void strnset(char *, char, int);
#endif
#endif
void emboss_file(void);
void spool_file(void);
void get_copies(void);
void get_page_range(void);
void get_end_page(char *);
int page_in_range(void);
void get_config(void);
FILE *open_config_file(int);
void process_options(char *, int);
void report_option_error(int, int);
void open_hyp_dictionary(char *, int, int);
void open_spell(char *);
int search_spell(void);
void process_table_definition(void);
void trim(char *);
int strpos(char *, char *);
void insert(char *, char *);
void move(char *, char *, int);
void do_pause(void);
void get_printer_file_name(void);
void pause_program(void);
int check_keyboard(int);
void write_toc_header(void);
void write_string(char *, int);
void write_char(char);
void no_space(void);
void sort_names(void);
void copy_string(char *, char *, int);
void delete(char *, int);
void strncpy_zero(char *, char *, int);
void cleanup(int);
void exit_program(int);
void print_error(char *,...);
int ISalpha(char);
int Isdigit(char);
int main(int, char **);
#ifdef unix
extern char *strlwr(char *string);
extern char *strupr(char *string);
extern int getch(void);
extern int getche(void);
extern long filelength(int descriptor);
extern int beep(int count);
extern int kbhit(void);
#endif             /* unix */
void top_of_form()
{
  int i, r;
  BOOL dopagesave = dopagenum;
  if (bpagec >= pageend)
  {
    done = TRUE;
    bline[0] = bline6[0] = bword[0] = 0;
    return;
  }
  actualpage++;
  bpagec++;
  r = page_in_range();
  if (r > 1)
  {                /* page in range */
    /* skip rest of page */
    if (lineskips < 99)
      for (i = top_margin; i < lineskips + linesperpage - blinec; i++)
        write_string(blank_line, 1);
    else
    {
      write_char(page_sep);     /* formfeed */
      if (lineskips == 101 && stdin_tty && stdout_tty && printit)
      {            /* pause */
        fprintf(stderr, "\7\nPress <ESC> to abort or any other key for next page\n");
        i = getch();
        if (i == 27)
          ab_flag = done = TRUE;
      }            /* pause */
    }
  }                /* page in range */
  blinec = hyp_page = 0;
  keep_together = keep_together_save;
  if (done == FALSE || xfooting == TRUE)
  {                /* next page */
    if (r)
    {
      if (!top_flag)
        make_top_margin();
      else
        top_flag = 0;
    }
    pagenumlen = 0;
/*if interpoint mode do not print even page numbers*/
    if (interpoint && (bpagec & 1) == 0)
      dopagenum = FALSE;
    if (dopagenum && ((bpagec > 1 - (first_page > 0)) || pageset) && !xheading)
      get_page_num_length();
    else
      linelength = curmax + 1 - margin;
    if (xcenter)
      linelength = center_length;
    dopagenum = dopagesave;
    page_beep();
    if (display_braille)
    {              /* display_braille */
      sprintf(wline, "Copy %d of %d   page %d", copies, lastcopy, bpagec);
      strcpy(temp, wline);
      i = ((int) strlen(wline) - maxline) >> 1;
      if (i > 0)
      {
        memset(field, 32, maxline);
        strnset(field, '*', i);
        field[i] = 0;
        sprintf(wline, "%s%s%s", field, temp, field);
      }
      if (!skip_output)
        fprintf(stderr, "%s\n", wline);
    }              /* display_braille */
    else
    if (skip_output + quiet_mode == 0)
    {              /* report progress */
      sprintf(temp, "%s page %d", progress[(page_in_range() > 0)], bpagec);
      if (lastcopy > 1)
        sprintf(temp + strlen(temp), " Copy %d of %d", copies,
                lastcopy);
      fprintf(stderr, "%s\n", temp);
    }              /* report progress */

    if (printit)
      delay(emboss_delay);
  }                /* next page */
  else
    bpagec--;
}                  /* top_of_form */

void make_top_margin()
{
  int i;
  for (i = 0; i < top_margin; i++)
    write_string(blank_line, 1);
}                  /* make_top_margin */

void page_beep()
{
  if (make_sound & BIT0)
  {
#ifndef unix
    sound(2000);
    delay(3);
    nosound();
#else
    beep(1);       /* Beep terminal */
#endif             /* unix */
  }
}                  /* page_beep */

void get_page_num_length()
{
  pagenumlen = 2 + page_min + (bpagec > 9) + (bpagec > 99);
  if (dobook && blinec == 0)
    pagenumlen = 3 + page_min + (bpageb > 9) + (bpageb > 99);
  linelength = curmax - pagenumlen - margin - strlen(addchar) + 1;
}                  /* get_page_num_length */

void backspace_int(int j)
{                  /* output a packspace so cursor is at first number of
                    * integer */
  fprintf(stderr, " %d", j);
  do
  {
    j /= 10;
    fprintf(stderr, "%c", 8);
  }
  while (j > 0);
}                  /* backspace_int */

void dofillit()
{
  int i, j;
  fillit = FALSE;
  i = strlen(bline) - 1;
  if (got_toc_page)
  {                /* add page number to bline */
    bline[curmax] = 0;
    if (toc_entry && (book_mode & BIT2))
    {              /* store print and braille page numbers */
      if (!toc_page.roman)
      {
        make_arabic(&toc_page.print, temp);
        make_arabic(&toc_page.braille, temp + 10);
      }
      else
      {            /* roman */
        make_roman(&toc_page.print, temp, (int) toc_page.roman);
        make_roman(&toc_page.braille, temp + 10, (int) toc_page.roman);
      }
      sprintf(field, "%5s %5s", temp, temp + 10);
    }              /* store print and braille page numbers */
    else
    if (!toc_page.roman > 0)
      make_arabic(&toc_page.braille, field);
    else
      make_roman(&toc_page.braille, field, (int) toc_page.roman);
    i += 2;        /* indexes first character of page number */
    sprintf(bline + i - 1, " %s", field);
  }                /* add page number to bline */
  else
  {                /* process last word on line */
    if (i <= 0)
      return;
    /* find out where last word on bline begins */
    while (bline[i] != ' ' && i > 0)
      i--;
    if (bline[i] == ' ')
      i++;
    strcpy(field, bline + i);   /* store the last word of bline */
    if ((int) strlen(field) >= fill_length)
      print_error("\007Last word %s of TOC longer than %d characters in line %ld\n",
                  field, fill_length - 1, total_lines);
  }                /* process last word on line */
  if (i > maxline)
    print_error("\7Page width < %d characters not allowed in TOC\n", i);
  memset(bline + i, fill_char, maxline - i);    /* fill with quotes dot 5 */
  j = maxline - i - strlen(field) - 1;
  if (j < guide_dots && j > 0)
    memset(bline + i, ' ', j);  /* remove guide dots */
  i = maxline - strlen(field);
  strcpy(bline + i, field);
  bline[i - 1] = ' ';   /* put blank before last word */
}                  /* dofillit */

void make_roman(int *page, char *string, int mode)
{
  if (*page >= 100)
    make_arabic(page, string);
  else
  {                /* roman */
    sprintf(string, "%c%s%s", letter_sign[0], roman10[*page / 10], roman[*page % 10]);
    if (!braille_page_nums)
      delete(string, 1);
    else
    if (mode > 1)
    {
      delete(string, 1);
      translate_word(string, part1);
      strcpy(string, part1);
    }
  }                /* roman */
}                  /* make_roman */

void make_arabic(int *page, char *string)
{
  int i;
  sprintf(string, "#%d", *page);
  if (braille_page_nums)
  {
    i = 1;
    while (string[i])
    {
      string[i] = bnumber[(int) (string[i] - (char) 48)];
      i++;
    }              /* while */
  }
  else
    delete(string, 1);
  if (page == &bpagec)
    strcat(string, addchar);
}                  /* make_arabic */

void make_book()
{
  make_arabic(&bpageb, field + 1);
  if (bpagec - bpageh > 1)
    field[0] = (char) ((int) ('?') + bpagec - bpageh);
  else
    delete(field, 1);   /* remove first char of field */
  if (field[0] > 'Z')
    print_error("\7More than 26 braille pages for print page %d\n", bpageb);
}                  /* make_book */

void bpurge(int bpurge_mode)
{
  int i = 0, j = 0, guide_line = 0, l = 0, list_flag = 0;
  BOOL dopagesave = dopagenum;
  char ch = 0;
bpurge_start:
  blinec++;        /* increment current braille line */
  if (bline[0])
  {                /* bline not empty */
    l = strlen(bline);
    if ((int) strlen(bline6) > l)
      strcpy(bline, bline6);
    ch = bline[curmax];
    bline[curmax] = 0;  /* set bline to <curmax chars long */

    if (margin > 1)
    {              /* insert space before left margin */
      move(bline, bline + margin - 1, curmax + 1 - margin);
      /* insert space in front of line if necessary */
      memset(bline, 32, margin - 1);    /* fill in left margin with spaces */
    }              /* insert space before left margin */
    if (makehead)
      print_error("\7Heading too long in line %ld\n", total_lines);
    if (makefoot)
      print_error("\7Footer too long in line %ld\n", total_lines);
  }                /* bline not empty */

  if (xcenter)
  {                /* center */
    center();
    if (newline)
      xcenter = FALSE;
  }                /* center */

  if (fillit && newline)
  {
    bline[curmax] = ch;
    dofillit();
    guide_line = 1;/* line of toc with dot 5 guidemarks */
  }

  if (blinec == 1)
  {                /* first line on this page */
    total_file_pages++;
    total_file_sheets++;
    field[0] = 0;
    if (interpoint && (bpagec & 1) == 0)
    {
      total_file_sheets--;
      dopagenum = FALSE;        /* no even page numbers for interpoint */
    }
    if (((bpagec > 1 - (first_page > 0)) || pageset || dobook) && dopagenum)
    {              /* store braille page number in field */
      if (doroman)
        make_roman(&bpagec, field, roman_mode);
      else
      if (dobook && (book_mode & 2))
        make_book();
      else
        make_arabic(&bpagec, field);
      if (guide_line || (fillit && toc_word) || (table_definition[0] &&
                                      chars_in_table + pagenumlen > curmax))
      {            /* cannot have this line on a line with a page number */
        strcpy(bline6, bline);
        bline[0] = 0;
        format_first_line();
        if (page_in_range())
          write_string(bline, 1);
        strcpy(bline, bline6);
        blinec++;
        if (toc_word)
          write_toc_header();
        field[0] = 0;
      }            /* cannot have this line on a page number line */
    }              /* store braille page number in field */
    else
    if (toc_word)
      write_toc_header();
    dopagenum = dopagesave;
    if (xheading)
    {              /* xheading */
      for (i = 0; i < lines_in_header; i++)
      {
        if (!i)
        {          /* first line of header */
          j = curmax - strlen(field);
          if (j <= heading_length)
            print_error("\7Heading too long for page number %d\n", bpagec);
          strcpy(hline[0] + j, field);
        }          /* first line of header */
        if (page_in_range())
          write_string(hline[i], 1);
        if (!i)
          memset(hline[0] + j, 32, curmax - j); /* remove page number */
        blinec++;
      }            /* i */
    }              /* xheading */
    else
    if (field[0])
    {
      if (list_flag)
      {            /* list */
        i = l;
        j = curmax - strlen(field) - 1;
        if (l >= j)
        {          /* remove last word */
          while (bline[i - 1] != ' ' && i > 0)
            i--;
          strcpy(oldword, bline + i);
          bline[i - 1] = '\0';
        }          /* remove last word */
      }            /* list */
      format_first_line();
    }
    if (page_in_range())
    {
      total_em_pages++;
      total_em_sheets++;
      if (interpoint)
        if (!(bpagec % 2))
          total_em_sheets--;
    }
  }                /* first line on page */
  else
  if (keep_together)
  {                /* conditional skip & not first line */
    /* find out if bline has only spaces */
    strcpy(field, bline);
    trim(field);
    if (!field[0] && blinec + keep_together > linesperpage - top_margin)
    {              /* skip rest of page */
      if (total_breaks < 10)
        page_breaks[total_breaks] = bpagec;
      blinec--;
      top_of_form();
      total_breaks++;
      return;
    }              /* skip rest of page */
  }                /* conditional skip & not first line */

  if (blinec >= linesperpage - top_margin)
  {                /* last line on page */
    if (dobook && (book_mode & 2) && guide_line == 0 &&
        !(interpoint && (bpagec % 2 == 0)))
    {              /* put braille page in lower right corner */
      make_arabic(&bpagec, field);
      j = (int) strlen(bline);
      memset(bline + j, 32, maxline - j);
      strcpy(bline + maxline - strlen(field), field);
    }              /* put braille page number in lower right corner. */
    else
    if (list_break && xformat == LISTS && bline[0] != ' ' && bpurge_mode == 4)
    {              /* list page break */
      blinec--;
      top_of_form();
      list_flag = 1;
      goto bpurge_start;
    }              /* list page break */
  }                /* last line on page */

  if (page_in_range())
  {                /* output */
    if (keep_format)
      for (j = left_offset; linein[j] == ' '; j++)
        write_char(' ');
    write_string(bline, 1);
    if (xdouble && (blinec < linesperpage - top_margin))
      write_string(blank_line, 1);
  }                /* output */
  if (xdouble && (blinec < linesperpage - top_margin))
    blinec++;

  if ((blinec + 1 == divide[0]) || (blinec + 1 == divide[1]))
  {                /* divider */
    memset(bline, (int) '3', curmax);
    if (page_in_range())
      write_string(bline, 1);
    blinec++;
  }                /* divider */
  bline[0] = bline6[0] = 0;
  if (!(xformat == TEXT) || fillit)
    strcpy(bline, indent);
  if (xformat == BLOCK || xformat == BLOCK_PARA)
    bline[0] = '\0';
  if (xformat == LINE_PARA)
  {
    bline[0] = '\0';
    if (plinein >= eolptr && long_flag <= 0)
      strcpy(bline, indent);
  }
  if (list_flag)
  {
    strcat(bline, oldword);
    list_flag = 0;
  }
  strcpy(bline6, bline);
  pjoin = pgroup = FALSE;
  if (xfooting && (blinec >= linesperpage - top_margin - 1))
    write_footer();
  if (blinec >= linesperpage - top_margin)
    top_of_form();
  else
  if (dobook && (blinec >= linesperpage - top_margin - 1) && !xfooting)
  {
    get_page_num_length();
    if (interpoint && bpagec % 2 == 0)
      pagenumlen = 0;
  }
  else
    linelength = curmax + 1 - margin;
  if (xcenter)
    linelength = center_length;
  newline = FALSE;
  if (!hyphenated_line)
    consec_hyphens = 0;
  hyphenated_line = 0;
}                  /* bpurge */

void center()
{
  int j;
  trim(bline);
  j = (curmax - (int) strlen(bline)) >> 1;
  if (j > 0)
  {
    move(bline, bline + j, (int) strlen(bline));
    memset(bline, 32, j);
  }
}                  /* center */

void is_centered(char *line)
{
  int i, j = 0;
  for (i = 0; line[i]; i++)
    if (line[i] == ' ')
      j = i;
    else
      break;
  if (format_char == '\r' || j < 6)
    return;
  insert(auto_center_format, line);
  auto_center++;
}                  /* is_centered */

void write_footer()
{                  /* put footing on bottom of current page */
  int l, start;
  memset(oldline, 32, maxline);
  oldline[maxline] = '\0';
  if (dobook)      /* put page number in lower right corner */
    make_arabic(&bpagec, field);
  else
    field[0] = '\0';    /* no page number */
  l = strlen(field);
  start = maxline - strlen(fline) - l - (l > 0) * page_min;
  if (start < 0)
    print_error("\7Footer too long on page %d\n", bpagec);
  move(fline, oldline + start, strlen(fline) - 1);
  strcpy(oldline + maxline - l, field);
  if (page_in_range())
    write_string(oldline, 1);
  blinec++;
}                  /* write_footer */

void format_first_line()
{
  int j;
  j = (int) strlen(bline);
  memset(bline + j, 32, maxline - j);
  strcpy(bline + maxline - strlen(field), field);
}                  /* format_first_line */

void build_line()
{
  int i, j, r, build_bpurge = 0, bword_len, dash_pos, bline_len;
  int output_found = (hyphen_mode & BIT4) && current_pass == 1;
  char c;
  i = bline_len = (int) strlen(bline);
  if (xtab)
  {                /* xtab */
    j = r = 0;
    do
    {
      r++;
      if (tabtable[i + r] != 0)
        j = r;     /* found tab */
    }
    while (j == 0 && (i + r < linelength));

    if (!j)
      j = 1;
    for (r = 1; r < j; r++)
    {
      strcat(bline, " ");
      strcat(bline6, " ");
    }
    pjoin = pgroup = xtab = FALSE;
  }                /* xtab */

  if (l1[0] && strcmp(words, l1) == 0)
    pjoin = TRUE;
  if (djoin)
  {                /* dutch braille l2 */
    djoin++;
    if (djoin == 3)
    {
      djoin = 0;
      for (j = 0; l2[j][0]; j++)
        if (!strcmp(words, l2[j]))
          pjoin = TRUE;
    }
  }                /* dutch braille l2 */

  if (!(pjoin || (pgroup && group)))
    i++;           /* account for space */
  j = i + (int) strlen(bword6);
  bword_len = (int) strlen(bword);
  i += bword_len;
  r = linelength;
  if (makefoot)
    r = curmax;    /* footer not shortened by page number */
  if (i > r || j > r)
    build_bpurge = 4;   /* bword won't fit so flush bline */
  if (l0[0])
    if (strchr(l0, words[strlen(words + 1)]))
      join = TRUE;

  if (rjoin)
  {
    join = TRUE;
    rjoin = FALSE;
  }
  if (fillit)
  {                /* fillit */
    if (!toc_word)
      write_toc_header();
    toc_word++;
    if (i > linelength -
        fill_length || j > linelength - fill_length)
      build_bpurge++;   /* flush output unless last_toc_word */
    if (last_toc_word)
      build_bpurge = FALSE;     /* do not flush output line, must be
                                 * processed by dofillit */
  }                /* fillit */
  if (((hyphen_mode & 65) == 65) && current_pass == 1 &&
      ((find_toc_pages == 0 && page_in_range()) || find_toc_pages))
    if (!strpbrk(words, "0123456789|"))
    {              /* check word */
      r = 1;       /* check this word by default */
      strcpy(field, words);
      strlwr(field);    /* produces the shortest length when translated */
      if (strchr(leading_punct, field[0]))
        delete(field, 1);
      remove_trailing_punct(field);
      if (field[0])
      {
        if (xgrade)
          translate_word(field, temp);
        else
          strcpy(temp, field);
        if ((int) strlen(temp) < min_hyp_len)
          r = 0;
      }
      else
        r = 0;
      if (r)
      {            /* test word */
        strupr(field);
        r = search_hyphen_dictionary(field);
        hyphen_searches--;
        if (strpbrk(field, "~-()<>/_'[]\042"))
          r = 1;   /* pretend word is already in dictionary, do not add to
                    * any file */
        if (!r)
        {          /* not in dictionary */
          r = 1;
          if (!bad_hyp_ptr)
            bad_hyp_ptr = fopen(bad_hyp_fname, "r+");
          if (bad_hyp_ptr)
          {        /* search bad word file */
            rewind(bad_hyp_ptr);        /* start from first line */
            while (fgets(temp, 80, bad_hyp_ptr))
            {
              temp[strlen(temp) - 1] = 0;
              r = (int) strcmp(field, temp);
              if (!r)
                break;  /* word already there */
            }      /* while */
          }        /* search bad word file */
          if (r)
          {        /* ask */
            fprintf(stderr, "Insert %s (Y/N) <ESC> to quit: ", field);
            get_yn();
            if (temp[0] == 'y')
              insert_dictionary_word();
            else
            {      /* add to bad words file */
              if (!bad_hyp_ptr)
              {    /* open */
                bad_hyp_ptr = fopen(bad_hyp_fname, "w+");
                if (!bad_hyp_ptr)
                  print_error("\7Cannot open %s\n", bad_hyp_fname);
              }    /* open */
              if (r)
                fprintf(bad_hyp_ptr, "%s\n", field);
            }      /* add to bad word file */
          }        /* ask */
        }          /* not in dictionary */
      }
    }              /* check word */
  if (build_bpurge)
    if (bline[0] && strcmp(bline, indent))
    {              /* line not empty or blank */
      if ((hyp_dic_ptr && (hyphen_mode & BIT0)) || (hyphen_mode & BIT1))
      {
        if (xgrade && fillit == FALSE && bword_len >= min_hyp_len
            && i - bword_len < linelength - 1)
        {          /* search hyphenation dictionary */
          dash_pos = search_hyphen_dictionary(words);
          if (dash_pos)
          {        /* found dictionary word */
            if (!dash_flag)
              hyphen_matches++;
            else
              dash_matches++;
            /* restore original case of word */
            r = 0;
            for (j = 0; hyphen_dic_line[j]; j++)
            {
              if (dash_flag == 0 && (hyphen_dic_line[j] == dash || hyphen_dic_line[j] == vbar))
                continue;
              if (capvec[r] == LOWER)
                hyphen_dic_line[j] = (char) tolower(hyphen_dic_line[j]);
              r++;
            }      /* j */
            do
            {
              dash_pos = split_hyphen_word(dash_pos);
              if (dash_pos)
              {    /* test split word */
                split_word = 1;
                c = format_char;
                format_char = '~';
                translate_word(part1, temp);
                format_char = c;
                if (i - bword_len + (int) strlen(temp) > linelength)
                  continue;     /* split word still too long */
                consec_hyphens++;
                hyp_page++;
                if (consec_hyphens > max_consec_hyphens || hyp_page > max_hyp_page)
                {  /* too many consecutive hyphens */
                  hyphens_skipped++;
                  if (output_found)
                    fprintf(dic_out_ptr, "%s page %d line %d skipped\n", words, bpagec, blinec + 1);
                  continue;
                }  /* too many consecutive hyphens */
                if (!dash_flag)
                  hyphens_used++;
                else
                  dashes_used++;
                hyphenated_line = 1;
                sprintf(bline + bline_len, " %s", temp);
                /* remove space if word should join previous word */
                if (pjoin)
                  delete(bline + bline_len, 1);
                format_char = '~';
                split_word = 2;
                translate_word(part2, bword);
                format_char = c;
                if (output_found)
                  if (fprintf(dic_out_ptr, "%s page %d line %d\n", words, bpagec, blinec + 1) < 0)
                    no_space();
                break;
              }    /* test split word */
            }
            while (dash_pos);
            split_word = 0;
          }        /* found dictionary word */
          else
          {        /* word not in dictionary */
            if (hyphen_mode & BIT5 && current_pass == 1)
            {      /* store word not found */
              if (fprintf(dic_out_ptr, "%s page %d not found\n", words, bpagec) < 0)
                no_space();
            }      /* store word not found */
          }        /* word not in dictionary */
        }          /* search hyphenation dictionary */
      }
      bpurge(build_bpurge);
    }              /* line not empty or blank */
  if (pjoin || (pgroup && group) || (strcmp(bword, "1") == 0 && trans_mode == 1 && xgrade > 0)
      || (bline[0] == '\0'))
  {
    sprintf(bline6, "%s%s", bline, bword6);
    strcat(bline, bword);
  }
  else
  {
    r = (int) strlen(bline);
    if (bline[r - 1] == ' ')
    {
      sprintf(bline6, "%s%s", bline, bword6);
      strcat(bline, bword);
    }
    else
    {
      sprintf(bline6, "%s %s", bline, bword6);
      sprintf(bline + r, " %s", bword);
    }
  }
  if (last_toc_word)
  {                /* output toc line */
    last_toc_word = FALSE;
    newline = TRUE;
    bpurge(0);
  }                /* output toc line */

  if (table_definition[0] && plinein >= eolptr)
  {                /* table defined & last word on line */
    sscanf(bline, table_sscan, column[0], column[1],
           column[2], column[3], column[4], column[5], column[6], column[7]);
    for (i = 0; i < cols_in_table; i++)
    {
      if ((int) strlen(column[i]) > field_width[i])
        print_error("\007Field %d > %d characters in line %ld\n", i + 1,
                    field_width[i], total_lines);
      if (column[i][0] == '`' && column[i][1] == '\0')
        column[i][0] = 0;       /* have a column with blanks */
    }              /* i */
    sprintf(bline, table_definition, column[0], column[1], column[2],
            column[3], column[4], column[5], column[6], column[7]);
    bpurge(-1);
  }                /* table defined & last word on line */

  if (!strcmp(bline, "BE"))
    if (xgrade > 1 && trans_mode == 1)
      strcpy(bline, "2");       /* braille for be to & be separated */
  bword6[0] = '\0';
  pgroup = group;
  pjoin = join;
  join = FALSE;
  r = (int) strlen(bline);
  j = linelength;
  if (makefoot)
    j = curmax;
  if (r > j)
  {                /* line too long */
    /* look for repeating characters */
    j = 0;
    c = bline[0];
    for (i = 1; bline[i]; i++)
      if (bline[i] == c)
        j++;
      else
      {            /* no repeat */
        j = 0;
        c = bline[i];
      }            /* no repeat */
    if (r - j < linelength)
    {              /* truncate repeats */
  truncate:
      if (bline[0] == ' ')
        delete(bline, strlen(indent));
      bline[linelength] = '\0';
      bpurge(-2);
      return;
    }              /* truncate repeats */
    else
    {              /* line still too long */
      /* look for double repeats */
      i = (int) (r - 3);
      while (i >= 0)
        if (bline[i] == bline[r - 1] && bline[i - 1] == bline[r - 2])
          i -= 2;
        else
          break;
      if (i + 3 < linelength)
        goto truncate;
      /* no double repeate so continue on next line */
      build_level++;
      if (!strcmp(bword, bline + linelength))
        print_error("\7Page Width of %d characters is too small\n");
      strcpy(bword, bline + linelength);
      bline[linelength] = bline6[0] = bword6[0] = '\0';
      if (build_level > 3)
      {
        bword[0] = '\0';
        return;
      }
      build_line();
    }              /* word still too long */
  }                /* line too long */
}                  /* build_line */

int split_hyphen_word(int dash_pos)
{
  for (;;)
  {
    dash_pos--;
    if (!dash_pos)
      break;
    if (hyphen_dic_line[dash_pos] == dash)
    {              /* found rightmost dash */
      strcpy(part2, hyphen_dic_line + dash_pos + 1);
      strlwr(part2);
      strcpy(part1, hyphen_dic_line);
      part1[dash_pos + 1] = 0;
      if (!dash_flag)
      {
        remove_dashes(part1, 0);
        remove_dashes(part2, 0);
      }
      break;
    }              /* found rightmost dash */
  }
  return (dash_pos);
}                  /* split_hyphen_word */

void remove_dashes(char *string, int vertical)
{                  /* remove dashes except if last char of string.  If
                    * vertical, also remove | */
  int i, j = 0;
  for (i = 0; string[i]; i++)
    if ((string[i] != dash && !vertical) ||
        (vertical && string[i] != vbar && string[i] != dash))
      string[j++] = string[i];
    else
    if (!string[i + 1])
      string[j++] = dash;       /* leave trailing - */
  string[j] = 0;
}                  /* remove_dashes */

void flush_if_not_blank()
{
  int i;
  if (makefoot || makehead)
    return;
  for (i = 0; bline[i]; i++)
    if (bline[i] != ' ')
    {              /* line contains a nonblank character */
      bpurge(-3);
      break;
    }              /* line contains a nonblank character */
}                  /* flush_if_not_blank */

int get_integer(char *string)
{                  /* returns integer from string and removes digits. 18hello
                    * becomes hello returning 18 */
  int j = 0, retval = 0, minusflag = 0;
  if (string[0] == dash)
  {
    minusflag = 1;
    j++;
  }
  while (Isdigit(string[j]))
  {
    retval = 10 * retval + (int) (string[j] - (char) 48);
    j++;
  }                /* while */
  if (j - minusflag)
    delete(string, j);  /* remove digits */
  if (minusflag)
    retval = -retval;
  return (retval);
}                  /* get_integer */

int is_equation()
{                  /* returns TRUE if word is a mathimatical equation,
                    * otherwise FALSE */
  if (!strpbrk(words, math_symbols))
    return (FALSE);
  if (strpbrk(words, "0123456789<>"))
  {
    total_equations++;
    if (math_flag == 2)
      fprintf(stderr, "%ld - %s\n", total_words, words);
    return (1);
  }
  return (0);
}                  /* is_equation */

void get_digit()
{
  if (hot_key)
  {
    temp[1] = 0;
    temp[0] = (char) getche();
    if (temp[0] <= '\r')
      temp[0] = 0;
    if (temp[0] == (char) 27)
    {              /* escape */
      fprintf(stderr, "\n");
      exit_program(0);
    }              /* escape */
  }
  else
    get_input(temp, 80);
}                  /* get_digit */

void get_date(int mode)
{                  /* stores date in date_string */
  time(&time1);
  tm = localtime(&time1);
  sprintf(date_string, "%s %02d/%02d/%02d %02d:%02d",
     dayofweek[tm->tm_wday], tm->tm_mon + 1, tm->tm_mday, tm->tm_year % 100,
          tm->tm_hour, tm->tm_min);
  if (mode)
  {
    /* remove day of week and add seconds */
    delete(date_string, 4);
    sprintf(date_string + 14, ":%02d", tm->tm_sec);
  }
}                  /* get_date */

void add_dots(char *string)
{
  int dot, i = 0, j;
  while (string[i])
  {
    dot = dot_table[(int) string[i]];
    if (xgrade)
      dot &= 63;   /* remove dot 7 unless we're doing computer braille */
    total_cells++;
    for (j = 0; j < 7; j++)
    {
      if (dot & 1)
        total_dots[j]++;
      dot = dot >> 1;
    }              /* j */
    i++;
  }                /* while */
}                  /* add_dots */
void add_case()
{
  int i, j = 0, k;
  BOOL show_caps = TRUE, show_lower = FALSE;
  for (i = 0; words[i]; i++)
  {
    if (isupper(words[i]) && show_caps)
    {              /* add comma */
      bword[j++] = ',';
      if (isupper(words[i + 1]))
      {            /* add another comma */
        bword[j++] = ',';
        show_caps = FALSE;
        show_lower = TRUE;
      }            /* add another comma */
    }              /* add a comma */
    if (islower(words[i]) && show_lower)
    {
      bword[j++] = letter_sign[0];
      show_lower = FALSE;
      show_caps = TRUE;
    }
    chardec = (int) words[i];
    if ((char) chardec < ' ')
    {
      k = b->start1[(int) words[i]];
      if (!k)
        undefined_char();
      strcpy(bword + j, b->replace[k]);
      j += (int) (strlen(b->replace[k]));
      continue;
    }
    bword[j++] = words[i];
  }                /* i */
  bword[j] = 0;
}                  /* add_case */

void do_commands(int option_mask)
{
  int tilpos, i, j, k, l, grade_modified = 0;
  int it_word = -1;
  char c, *cptr, f_char;
  if ((cptr = strchr(words, format_char)) == NULL)
    return;        /* no format commands in word */
  /* assume command will terminate TOC, centering or table */
  newline = TRUE;

  tilpos = 1 + (int) (cptr - words);
  f_char = format_char; /* incase char gets changed */
  if (words[tilpos])    /* theres a character after the tilda */
    do
    {
      if (option_file)
        fprintf(option_file, "%s\n", cptr);
      c = (char) toupper(words[tilpos]);        /* the character after the ~ */
      cptr = words + tilpos - 1;        /* the tilde character */
      if (!strchr("ILZ[\47", c))
        delete(cptr, 2);        /* for every char except ILZ[ */
      if (strchr("CEFMSTY7[]_>", c))
        flush_if_not_blank();
      switch (c)
      {
      case '#':   /* comment */
        words[tilpos - 1] = '\0';
        plinein = eolptr;       /* terminate word and line */
        break;
      case 'A':   /* Acronym Logic */
        xacronym = TRUE;
        newline = FALSE;
        break;
      case 'B':   /* Textbook Break */
        doroman = FALSE;
        dobook = TRUE;
        bpageh = bpagec;        /* set to current page */
        /* blinec is 0 if first line has not been written to page */
        if (blinec < linesperpage - top_margin)
          bpageh--;
        bpageb = get_integer(cptr);
        if (!bpageb)
          bpageb = 1;   /* set to 1 if invalid integer */
        if (book_mode & 1)
        {          /* mark with dashes */
          flush_if_not_blank();
          memset(tline, dash, maxline); /* dots 3/6 */
          i = maxline;
          make_arabic(&bpageb, field);
          strcpy(tline + maxline - strlen(field), field);       /* put page num at end
                                                                 * of line */
          if (blinec <= 1)
            goto skip_write;    /* not at top two lines */
          if (book_break && blinec >= linesperpage - top_margin - book_break)
          {
            do_advance();
            goto skip_write;
          }
          if (blinec < linesperpage - top_margin - 1)
            /* not at bottom 2 lines of page */
            write_fill_line();
          else
        skip_write:
            linelength = curmax - pagenumlen - margin + 1;
        }          /* mark with dashes */
        break;
      case 'C':   /* Center */
        xcenter = TRUE;
        if (center_length > curmax - 6)
          center_length = curmax - 6;
        linelength = center_length;
        break;
      case 'D':   /* Double Toggle */
        xdouble ^= 1;
        break;
      case 'E':   /* Poetry */
        xformat = POETRY;
        break;
      case 'F':   /* Fill Line - Index table of contents */
        process_fill(cptr);
        break;
      case 'G':   /* Set Right Margin */
        i = curmax;
        curmax = get_integer(cptr);
        if (curmax < 1)
          curmax = 1;
        if (curmax > maxline)
          curmax = maxline;
        linelength = curmax + 1 - margin;
        break;
      case 'H':   /* Heading */
        strcpy(oldline, bline);
        strcpy(oldline6, bline6);
        xheading = TRUE;
        xpgroup = pgroup;
        xpjoin = pjoin;
        xjoin = join;
        xgroup = group;
        makehead = TRUE;
        linelength = curmax + margin - 3;
        bline[0] = 0;
        bline6[0] = 0;
        pgroup = FALSE;
        group = FALSE;
        join = FALSE;
        pjoin = FALSE;
        break;
      case 'I':   /* Italics */
        newline = FALSE;
        words[tilpos] = italics[0];
        if (words[tilpos + 1] == '\\')
        {
          it_flag += 1;
          delete(words + tilpos - 1, 3);        /* remove ~i\ */
          if (it_word == (int) total_words)
          {        /* more than 1 ~i\ in the word */
            delete(words, 1);   /* only leave 1 italics sign */
            break;
          }        /* more than 1 ~i\ in the word */
          it_word = (int) total_words;
          for (i = 0; i <= (it_flag & 1); i++)
            insert(italics, words);
          if (it_flag == 2)
            it_flag = 0;
          break;
        }          /* ~i\ */
        else
          it_flag = 0;
        if (tilpos > 1)
        {          /* >=1 char before ~ */
          if (isalpha(words[tilpos - 2]))
            words[tilpos - 1] = dash;
          else
            delete(words + tilpos - 1, 1);
        }
        else
        {
          words[tilpos - 1] = italics[0];
          delete(words + tilpos - 1, 1);
        }
        break;
      case 'J':   /* Stop Heading */
        xheading = FALSE;
        break;

      case 'K':   /* Stop Footing */
        xfooting = FALSE;
        break;
      case 'L':   /* Letter Sign */
        if (xgrade)
        {
          words[tilpos - 1] = vbar;     /* don't translate next character */
          words[tilpos] = letter_sign[0];
        }
        else
          delete(words + tilpos - 1, 2);
        break;
      case 'M':   /* Left Margin Set */
        i = margin;
        margin = get_integer(cptr);
        if (margin < 1)
          margin = 1;
        margin = min(margin, 30);
        linelength += i - margin;
        setmargin += margin - oldmargin;
        oldmargin = margin;
        if (cptr[0])
        {          /* top margin */
          delete(cptr, 1);
          top_margin = get_integer(cptr) - 1;
          top_margin = max(top_margin, 0);
          top_margin = min(top_margin, linesperpage - 10);
          if (blinec == 0 && bpagec == 1 && outf_des > 0 && current_pass == 1)
          {
            make_top_margin();
            top_flag = 1;
          }
        }          /* top margin */
        break;

      case 'N':   /* Set PageNum */
        addchar[0] = 0;
        dopagenum = TRUE;
        doroman = FALSE;
        bpagec = get_integer(cptr);
        if (bpagec <= 0)
          bpagec = 1;
        if (isupper(words[0]))
        {          /* uppercase letter followed number */
          sprintf(addchar, ";%c", words[0]);
          words[0] = 0;
        }          /* uppercase letter followed word */
        if (bpagec == 1)
          pageset = TRUE;
        bpageh = bpagec;
        if (blinec == 0)
          linelength = curmax - pagenumlen - margin - (int) (strlen(addchar) + 1);
        break;
      case 'O':   /* Offset for paragraph */
        make_indent(cptr);
        break;
      case 'P':   /* page */
        if (*cptr == '+' || *cptr == dash)
        {          /* +- */
          braille_page_nums = FALSE;
          if (*cptr == '+')
            braille_page_nums = TRUE;
          delete(cptr, 1);
          break;
        }          /* +- */
        if (!Isdigit(cptr[0]))
        {          /* always skip page */
          k = 1;
          if (cptr[0] == ':')
          {        /* colon */
            delete(cptr, 1);
            i = get_integer(cptr);
            strcpy(field, bline);
            trim(field);
            if (field[0])
              i++;
            if (blinec + i < linesperpage - top_margin)
              k = -1;   /* no page break */
          }        /* colon */

          if (cptr[0] && isalpha(cptr[0]))
          {        /* letter */
            /* skip to next odd page if interpoint right facing page is odd */
            if (interpoint && (bpagec & 1))
              k++;
            delete(cptr, 1);
          }        /* letter */
          for (l = 0; l < k; l++)
          {
            if (blinec >= linesperpage - top_margin - 1)
              bpurge(-4);       /* last line on page */
            else
            {      /* not last line on page */
              flush_if_not_blank();

              do_advance();
            }      /* not last line on page */
          }        /* l */
        }          /* always skip page */
        else
        {          /* conditional skip */
          keep_together = get_integer(cptr);
          keep_together = min(keep_together, linesperpage / 2);
          keep_together_save = keep_together;
        }          /* conditional skip */
        break;
      case 'Q':   /* clear all tabs */
        memset(tabtable, 0, sizeof(tabtable));
        break;
      case 'R':   /* Roman Numerals */
        doroman = dopagenum = TRUE;
        dobook = FALSE;
        bpagec = get_integer(cptr);
        if (bpagec <= 0)
          bpagec = 1;
        bpageh = bpagec;
        if (*cptr == ':')
        {          /* roman mode */
          delete(cptr, 1);
          i = get_integer(cptr);
          roman_mode = 1 + (i > 0);
        }          /* roman mode */
        break;
      case 'S':   /* Skip a line */
        i = get_integer(cptr);
        if (blinec >= i - 1)
        {
          strcpy(bline, blank_line);
          bpurge(-6);
          if (cptr[0] == 'i' || cptr[0] == 'I')
          {
            strcpy(bline, indent);
            delete(cptr, 1);
          }
        }
        break;
      case 'T':   /* Text Format */
        xformat = TEXT;
        make_indent(cptr);
        strcat(bline, indent);
        strcpy(bline6, bline);
        line_end[0] = '\0';
        break;
      case 'U':   /* page Numbering Off */
        addchar[0] = 0;
        dopagenum = FALSE;
        linelength = curmax + 1 - margin;
        break;
      case 'V':   /* set tab */
        i = get_integer(words);
        if (!i)
          i = 1;
        if (i >= linelength)
          print_error("\007Tab set beyond line length in line %d\n",
                      lineinct);
        tabtable[i] = 1;
        break;
      case 'W':   /* Footing */
        strcpy(oldline, bline);
        strcpy(oldline6, bline6);
        makefoot = xfooting = TRUE;
        xpgroup = pgroup;
        xpjoin = pjoin;
        xjoin = join;
        xgroup = group;
        bline[0] = 0;
        bline6[0] = 0;
        pgroup = FALSE;
        group = FALSE;
        join = FALSE;
        pjoin = FALSE;
        break;
      case 'X':
        j = get_integer(cptr);
        if (j)
        {
          for (i = 0; i < j; i++)
            if ((int) strlen(bline) < linelength)
              strcat(bline, " ");
        }
        else
          xtab = TRUE;
        break;
      case 'Y':
        xformat = LISTS;        /* TABLE Format */
        make_indent(cptr);
        break;
      case 'Z':   /* Terminator */
        words[tilpos] = '`';
        delete(words + tilpos - 1, 1);
        newline = FALSE;
        break;
      case '\47': /* Apostrophe */
        *cptr = '|';
        newline = FALSE;
        break;
      case '_':
        break;
      case '0':   /* Grade 0 */
      case '1':   /* grade 1 */
      case '2':   /* grade 2 */
      case '3':   /* grade 3 */
        if (grade_modified++)
          print_error(
                      "\007Two grade modifiers in word not allowed in line %ld\n", total_lines);
        newline = FALSE;
        group = join = FALSE;
        cptr = strchr(ignore_format, c);
        i = (int) (c - '0');
        if (isdigit(words[tilpos - 1]))
          j = get_integer(words + tilpos - 1);  /* proposed grade_mod */
        else
          j = 0;
        if (!cptr)
        {          /* don't ignore command */
          if (i == 3 && j == 0)
            j = default_g3_mod; /* set default bits */
          if (grade_mod != j && xgrade == 3 && i == 3)
            current_table_grade = 0;    /* forced reload */
          if (xgrade == 3 && i == 3 && current_table_grade != 3)
            current_table_grade = 0;
          xgrade = i;
          grade_mod = j;
          if (xgrade >= 2)
            load_tables(table_file[(trans_mode > 1)]);
        }          /* don't ignore command */
        if (pjoin && xgrade == 0)
        {
          pjoin = FALSE;
          strcpy(bline, bline6);
        }
        break;
      case '4':   /* block */
        newline = FALSE;
        xformat = BLOCK;
        break;
      case '5':   /* blockparagraphs */
        newline = FALSE;
        xformat = BLOCK_PARA;
        break;
      case '6':   /* auto format */
        newline = FALSE;
        xformat = AUTO_FORMAT;
        break;
      case '7':   /* line paragraph */
        xformat = LINE_PARA;
        strcpy(bline, indent);
        strcpy(bline6, bline);
        line_end[0] = '\0';
        break;
      case '[':   /* escape */
        i = tilpos - 1;
        words[i] = (char) 27;   /* escape character */
        delete(words + tilpos, 1);      /* remove the [ */
        do
        {          /* write escape sequence directly to printer or file */
          write_char(words[i]);
          i++;
        }
        while (words[i] != '\0' && words[i] != '~');
        strcpy(words + tilpos - 1, words + i);
        break;
      case ']':
        if (blinec)
        {
          strcpy(bline, end_of_article);
          center();
          bpurge(-7);
        }
        break;
      case '{':
        xfooting = FALSE;
        break;
      case '}':   /* inner quotes */
        quotecount = 2;
        break;
      case '-':   /* process as command line option */
        j = tilpos;
        while (words[j] && words[j] != '~')
          j++;
        strcpy(temp, words + tilpos - 1);
        strcpy(words + tilpos - 1, words + j);
        temp[j - tilpos + 1] = '\0';
        /* if quoted string, the first quote starts in char 4 xx= */
        if ((temp[3] == '\042' || temp[3] == '\047') &&
            (strchr(temp + 4, temp[3]) == NULL))
          while (plinein < eolptr)
          {
            get_word();
            sprintf(temp + strlen(temp), " %s", words);
            if (strchr(temp + 4, temp[3]))
            {
              words[0] = '\0';
              break;
            }
          }        /* while */
        process_options(temp, option_mask);
        break;
      case '.':
        j = test_extension(words + tilpos - 1);
        if (j < 0)
          print_error("\7Excluded extension in line %ld\n", total_lines);
        if (!j)
          print_error("\7Undefined extension %s in line %ld\n",
                      words + tilpos - 1, total_lines);
        strcpy(words, init[j].pre_init);
        break;
      case '~':
        format_char = '\r';
        break;
      case '\\':
        i = get_integer(cptr + 1);
        j = abs(i);
        c = *cptr;
        delete(cptr, 1);        /* remove char after \ */
        switch (toupper(c))
        {
        case 'I': /* store current page number */
          if (j >= MAX_INDEX)
            print_error("\7Index must be less than %d in line %ld\n", MAX_INDEX, total_lines);
          if (index_pages[j].braille)
            print_error("\7Duplicate index entry %d in line %ld\n", j, total_lines);
          if (j && i < 0 && index_pages[j - 1].braille <= 0)
            print_error("\7Index range error in line %ld\n", total_lines);
          index_pages[j].braille = bpagec;
          index_pages[j].print = bpageb;
          if (doroman)
            index_pages[j].roman = (char) roman_mode;
          if (i < 0)
            index_pages[j].braille = -bpagec;
          break;
        case 'J': /* retrieve stored page number */
          if (!index_pages[j].braille)
            print_error("\7Index %d does not exist in line %ld\n", j, total_lines);
          if (index_pages[j + 1].braille >= 0 || (index_pages[j].braille == index_pages[j + 1].braille))
          {
            if (!index_pages[j].roman)
              sprintf(temp, "%d", abs(index_pages[j].braille));
            else
            {      /* roman index */
              i = abs(index_pages[j].braille);
              make_roman(&i, temp, (char) index_pages[j].roman);
              insert("|", temp);
            }      /* roman index */
          }
          else
          {
            sprintf(temp, "%d-%d", abs(index_pages[j].braille), abs(index_pages[j + 1].braille) % 100);
          }
          insert(temp, cptr);
          break;
        case 'K':
          if (!index_pages[j].print)
            print_error("\7Index %d does not exist in line %ld\n", j, total_lines);
          if (index_pages[j + 1].print >= 0 || (index_pages[j].print == index_pages[j + 1].print))
            sprintf(temp, "%d", abs(index_pages[j].print));
          else
            sprintf(temp, "%d-%d", abs(index_pages[j].print), abs(index_pages[j + 1].print) % 100);
          insert(temp, cptr);
        }          /* switch */
        break;
      case '!':
        return;
        break;
      case '>':
        if (!(*cptr))
          *cptr = '-';
        memset(tline, *cptr, maxline);
        write_fill_line();
        *cptr = '\0';
        break;
      default:
    bad_format:
        sprintf(iobuf, "\007Format error in ");
        if (total_lines)        /* error occurred in input file */
          sprintf(iobuf + strlen(iobuf), "line %ld\n%s", total_lines, linein);
        sprintf(iobuf + strlen(iobuf), "i%d=%s|%s\n", prog_init,
                init[prog_init].pre_init, init[prog_init].post_init);
        print_error(iobuf);
      }            /* switch */
      if (strchr("AL", c) && words[0] == '\0')
        goto bad_format;
      if ((cptr = strchr(words, f_char)) != NULL)
        tilpos = 1 + (int) (cptr - words);
    }
    while (cptr != NULL);
}                  /* do_commands */

void do_advance()
{
  int i, j;
  if (dobook)
  {
    /* we have to put page number in lower right corner */
    j = blinec;
    for (i = j; i < linesperpage; i++)
      bpurge(-5);
  }
  else
    top_of_form();
}                  /* do_advance */

void write_fill_line()
{
  if (page_in_range())
    write_string(tline, 1);
  blinec++;
}                  /* write_fill_line */

void make_indent(char *cptr)
{
  int i;
  i = get_integer(cptr);
  if (i >= 30 || i <= 0)
    i = 2;         /* keep values reasonable */
  memset(indent, 32, i);
  indent[i] = '\0';
}                  /* make_indent */

void process_fill(char *cptr)
{
  int i, j;
  char *p;
  fillit = TRUE;
  /* assume a normal ~f not followed by digit */
  got_toc_page = FALSE;
  fill_length = 7;
  if (Isdigit(*cptr))
  {                /* process toc mark */
    i = get_integer(cptr);
    if (i > MAX_TOC_ENTRY)
      print_error("\007More than %d TOC entries not allowed\n", MAX_TOC_ENTRY);
    j = i - 1;
    if (current_pass == 1)
    {              /* first pass */
      if (!i)
      {            /* one pass required before embossing */
        if (find_toc_pages)
          goto duplicate;
        /*
         * set starting and ending page so program translates entire file but
         * never writes to disk during first pass
         */
        pagestart = pageend = 32767;
        find_toc_pages = TRUE;
        fillit = FALSE;
        toc_file_ptr = fopen(toc_file_name, "w+");
        if (!toc_file_ptr)
          print_error("\7Cannot open %s\n", toc_file_name);
        return;
      }            /* one pass required before embossing */
      /* in first pass with i in range */
      if (!find_toc_pages)
        print_error("\007~f0 not found before ~F%d on line %ld\n",
                    i, total_lines);
      if (toc_pages[j].braille > 0)
    duplicate:
        print_error("\007Duplicate ~f%d entry on line %ld\n", i, total_lines);
      if (!toc_pages[j].braille)
      {            /* toc entry on first pass */
        toc_pages[j].braille = -1;      /* found first toc mark */
        got_toc_page = TRUE;    /* page is correct only on second pass */
        if (book_mode & 4)
          fill_length = 13;
        toc_entry = TRUE;
        /* store toc line skipping the ~f digits */
        p = strchr(linein, format_char) + 2;
        while (isdigit(*p))
          p++;
        fprintf(toc_file_ptr, "%s\n", p);
        if (!j)
          strcpy(toc_line, p);
        compact_line(toc_line);
      }            /* toc entry on first pass */
      else
      {            /* found second toc mark first pass */
        if ((book_mode & 4) && bpageb == 0)
          print_error("\7lPrint page undefined for TOC title ~f%d\n", i);
        toc_pages[j].braille = bpagec;
        if (doroman)
          toc_pages[j].roman = (char) roman_mode;
        toc_pages[j].print = bpageb;
        fillit = toc_entry = FALSE;     /* title, not actual toc entry */
      }            /* second toc entry first pass */
    }              /* first pass */
    else
    {              /* second pass */
      if (!i)
      {            /* ~f0 second pass */
        fillit = FALSE;
        return;
      }            /* ~f0 second pass */
      if (toc_pages[j].braille)
      {            /* toc entry second pass */
        toc_page = toc_pages[j];
        toc_pages[j].braille = 0;       /* indicates this contents/title pair
                                         * has been processed */
        got_toc_page = TRUE;
        if (book_mode & 4)
          fill_length = 13;
        toc_entry = TRUE;
        if (!j)
        {
          rewind(toc_file_ptr);
          read_toc_line();
        }
      }            /* toc entry second pass */
      else
      {            /* title entry second pass */
        fillit = toc_entry = FALSE;
      }            /* title entry second pass */
    }              /* second pass */
  }                /* process toc entry */
  else
    switch (*cptr)
    {
    case ':':
      delete(cptr, 1);
      fill_length = get_integer(cptr);
      break;
    case '+':
    case '-':
      fillit = FALSE;
      auto_toc_flag = (*cptr == '+');
      delete(cptr, 1);
    }              /* switch */
}                  /* process_fill */

void process_auto_toc()
{
  int l, l1;
  char *cptr;
  l = l1 = strlen(linein) - 1;
  if (l)
  {                /* line not empty */
    cptr = strchr(linein, format_char);
    if (cptr && toupper(cptr[1]) == 'F')
      return;
    /* find beginning of trailing number */
    while (l >= 0 && isdigit(linein[l]))
      l--;
    if (l < l1)    /* trailing digits found */
      while (l > 0 && (linein[l] == '.' || linein[l] == ' '))
        l--;
    if (l > 1)
    {
      linein[l + 1] = '\0';
      l = 0;
      while (linein[l] == ' ')
        l++;
      sprintf(temp, "%cf%d ", format_char, auto_toc_flag++);
      insert(temp, linein + l);
    }
  }                /* line not empty */
}                  /* process_auto_toc */

void read_toc_line()
{
  int l;
  fgets(toc_line, 100, toc_file_ptr);
  l = strlen(toc_line) - 1;
  if (toc_line[l] == '\n')
    toc_line[l] = '\0';
  toc_line_count++;
  compact_line(toc_line);
}                  /* read_toc_line */

void compact_line(char *string)
{
  int i = 0, j = 0, blanks = 0;
  char c;
  while (string[i] == ' ' || string[i] == dash)
    i++;
  strcpy(string, string + i);
  for (i = 0; string[i]; i++)
  {
    c = string[i];
    if (c == (char) 9)
      string[i] = c = ' ';
    if (c == ' ')
      blanks++;
    else
      blanks = 0;
    if (blanks < 2)
      string[j++] = c;
  }                /* i */
  string[j] = '\0';
}                  /* compact_line */

void length_error(char *string)
{
  print_error("\7Line > 255 characters %s line %ld\n", string, total_lines);
}                  /* length_error */

void do_lop_op(foptype * fop)
{                  /* Execute the Operation */
  int i, j, k, l;
  if (fop->fop < 100)
  {                /* not field match or replace */
    switch (fop->fop)
    {
    case 1:       /* delete */
      linein[0] = 1;
      linein[1] = 0;    /* delete but don't count as blank */
      break;
    case 2:       /* text */
      xformat = TEXT;
      break;
    case 3:       /* list */
      xformat = LISTS;
      break;
    case 4:       /* skip line */
      strcpy(linein, "~S ");
      linein[0] = format_char;
      break;
    case 5:       /* center */
      trim(linein);
      insert("~c", linein);
      linein[0] = format_char;
      break;
    case 6:       /* indent */
      setmargin = oldmargin + 2;
      break;
    case 80:      /* page */
      inpglen = (unsigned int) fop->fstart;
      break;
    }              /* switch */
exit:
    eolptr = linein + strlen(linein);
    return;
  }                /* not field match or replace */
  if (!linein[0])
    return;        /* nothing to do on empty line */
  if (fop->fop < 300)
  {                /* field or match */
    j = (int) strlen(linein);
    if (j < fop->fstart + 1)
      for (i = j; i <= fop->fstart; i++)
        strcat(linein, " ");    /* fill in line so field isn't garbage */
    strcpy(field, linein + fop->fstart);
    field[fop->flen] = 0;

    if (fop->fop < 200)
    {              /* field state omit append */
      tabmargin = TRUE;
      delete(linein + fop->fstart, fop->flen);  /* remove field from linein */
      trim(field);

      switch (fop->fop)
      {            /* switch op */
      case 101:   /* omit */
        field[0] = 0;
        break;
      case 102:   /* state */
        if (field[0])
        {          /* field not empty */
          for (i = 0; stateid[i]; i++)
          {        /* i */
            j = strpos(field, stateid[i]);
            if (j)
            {      /* found state abbreviation */
              delete(field + j - 1, 2);
              insert(stateid[i] + 3, field + j - 1);
              break;
            }      /* found state abbreviation */
          }        /* i */
        }          /* field not empty */
      }            /* switch */
      if (fop->data)
        strcat(field, fop->data);
      insert(field, linein + fop->fstart);
      goto exit;
    }              /* <200 */
  }                /* field or match */
  if (fop->fop > 300 && fop->fop < 313)
  {                /* replace or word */
    l = 0;
    strcpy(field, fop->data);   /* search string */
    do
    {
      strcpy(temp, linein);
      strupr(temp);/* case insensitive search */
      j = strpos(temp + l, field);
      if (!j)
        return;    /* string not found */
      j--;         /* relative to zero */
      if (token_char == 312)
      {            /* word */
        if ((l + j == 0) || linein[l + j - 1] == ' ')
        {          /* at beginning */
          k = strlen(fop[1].data) + strlen(fop[2].data);
          if (strlen(linein) + k >= 255)
            length_error("(WORD");
          /* find end of word */
          i = j + l + 1;
          while (linein[i] != ' ' && linein[i] != '\0')
            i++;
          insert(fop[2].data, linein + i);
          insert(fop[1].data, linein + l + j);
          l = i + k;
        }          /* at beginning */
        else
          l += strlen(fop[1].data);
      }            /* word */
      switch (fop->fop)
      {
      case 301:   /* replace line */
        strcpy(linein, fop[1].data);
        goto exit;
        break;
      case 305:   /* reps* */
      case 306:   /* reps */
        delete(linein + j + l, strlen(field));  /* remove substring */
        if (strlen(linein) + strlen(fop[1].data) > 255)
          length_error("REPS*");
        insert(fop[1].data, linein + j + l);
        l += (int) (strlen(fop[1].data) + j);
        break;
      case 310:   /* replace word */
      case 311:   /* repw */
        /* find first char of word */
        while (linein[j + l] != ' ' && j > 0)
          j--;
        if (linein[j + l] == ' ')
          j++;
        /* j points to first character of word */
        i = j + l + 1;
        while (linein[i] != ' ' && linein[i] != '\0')
          i++;
        strcpy(linein + j + l, linein + i);     /* remove the word */
        if (strlen(linein) + strlen(fop[1].data) > 255)
          length_error("(REPW*)");
        insert(fop[1].data, linein + j + l);
        l += (int) (strlen(fop[1].data) + j);
        break;
      }            /* switch */
    }
    while (fop->fop == 305 || fop->fop == 310 || fop->fop == 312);
  }                /* replace */
  switch (fop->fop)
  {
  case 315:
    strcpy(field, fop->data);
    process_options(field, BIT4);
    break;
  case 320:
    sprintf(temp, "%s%ld", fop->data, total_lines);
    /* insert after leading blanks */
    i = 0;
    while (linein[i] == ' ' && linein[i])
      i++;
    if (linein[i])
    {
      if (strlen(linein) + strlen(temp) > 255)
        length_error("(LINE");
      insert(temp, linein + i);
    }
    break;
  case 325:       /* graphics */
    for (i = 0; linein[i]; i++)
      if (linein[i] < '\0')
      {            /* extended graphics */
        sprintf(temp, "%s%d", fop->data,
                (int) (unsigned char) linein[i]);
        delete(linein + i, 1);
        if (strlen(linein) + strlen(temp) > 255)
          length_error("(GRAPH)");
        insert(temp, linein + i);
      }            /* extended graphics */
  }                /* switch */
  l = strlen(linein);
  while (l > 0 && linein[l - 1] == ' ')
    l--;
  linein[l] = '\0';
  eolptr = linein + l;
}                  /* do_lop_op */

void do_lop()
{                  /* Do Line Operation Processing -- outer loop */
  int i, j, k;
  disablecol = FALSE;
  for (i = 0; i < lopcount; i++)
  {                /* for each .efl line */
    if (lineinct < l[i].startline || lineinct > l[i].endline)
      continue;    /* line not in range */
    if (l[i].match_active)
    {              /* match active */
      j = l[i].fopstart;
      strncpy(field, linein + f[j].fstart, f[j].flen);
      field[f[j].flen] = '\0';
      strupr(field);
      k = strcmp(field, f[j].data);
      if ((k == 0 && l[i].match_active == 1) ||
          (k != 0 && l[i].match_active == 2))
      {
        for (j = l[i].fopstart + 1; j < l[i].fopend; j++)
          do_lop_op(&f[j]);
      }
    }              /* match active */
    else
    {              /* match not active */
      for (j = l[i].fopstart; j < l[i].fopend; j++)
        do_lop_op(&f[j]);
    }              /* match not active */
  }
}                  /* do_lop */

void getline()
{
  int i = -1;
  for (;;)
  {                /* do until line read, line too long, or eof */
    i++;
    if (!bytes_in_buf)
    {              /* empty buffer so fill it up */
      bytes_in_buf = read(inf_des, iobuf, (unsigned int) BUFSIZE);
      if (bytes_in_buf <= 0)
      {            /* eof */
        done = TRUE;
        if (i > 0 && c != '\n')
          done = FALSE; /* file ended without crlf */
        break;
      }            /* eof */
      ioptr = iobuf;
    }              /* buffer empty so fill it up */
    c = (char) *ioptr;
    bytes_in_buf--;
    ioptr++;
    if (c & (unsigned char) 128)
    {              /* b7 set */
      if (!graphics_mode)
        goto skip_char;
      if (graphics_mode == 1)
        c &= 127;  /* remove b7 */
    }              /* b7 set */
    if (trans_mode == 3)
      if (c > 'z' || c == '`')
        c -= ' ';
    switch ((int) c)
    {
    case 31:
      c = vbar;
      break;
    case 8:       /* backspace */
      if (i > 0)
      {            /* backspace */
        i -= 2;
        continue;
      }            /* backspace */
      break;
    case 9:       /* tab */
      if (expand_tab && keep_control == 0)
      {            /* expand tab */
        c = ' ';
        while (i % 8 < 7)
          linein[i++] = c;
      }            /* expand tab */
      break;
    case 30:
  skip_char:
      i--;
      continue;
      break;
    case 10:      /* lf */
      long_flag = -long_flag;   /* normal line termination */
      goto process_chars;
    case 11:
      lineinct = 0;
      break;
    case 12:      /* formfeed */
      if (trans_mode != 2)
        goto skip_char;
      break;
    }              /* switch */
    linein[i] = c;
    if (i > 170)
    {              /* look for word break */
      if (c == ' ' || i > 250)
      {
        total_lines--;
        long_flag += i + 1;
        input_length = 0;
        break;     /* line too long, end at word */
      }
    }              /* look for word break */
    if (c == '\15')
      goto space;
    if (c < ' ' && keep_control == 0)
  space:linein[i] = ' ';
    /* remove all control chars */
  }                /* for */
process_chars:
  linein[i] = linein[i + 1] = '\0';
  i--;
  while (i >= 0 && linein[i] == ' ')
    linein[i--] = 0;    /* remove trailing spaces */
  if (long_flag <= 0)
  {
    input_length = -long_flag + i + 1;
    long_flag = 0;
  }
  eolptr = linein + i + 1;
  plinein = &linein[0];
  line_word = total_words + 1l;
  if (inpglen > 0 && lineinct > inpglen)
    lineinct = 0;  /* reset lineinct for next input page. Used by .efl files */
  lineinct++;
  total_lines++;
  if (input_length > max_input_length)
  {
    max_input_length = input_length;
    max_input_line_num = total_lines;
  }
  if (lopactive)
    do_lop();

  if (display_source)
    printf("=>%s\n", linein);
}                  /* getline */

void get_input(char *string, int length)
{                  /* gets a line of input from stdin up to length bytes */
  int l;
  fgets(string, length, stdin);
  l = (int) (strlen(string) - 1);
  if (string[l] == '\n' && l >= 0)
    string[l] = 0;
}                  /* get_input */

int get_paragraph_type(int mode)
{
  char prev_char = '\1';
  int save_disp = display_source, i;
  auto_blanks = auto_indents = auto_punct = 0;
  if (!mode)
    xformat = TEXT;
  else
    left_offset = 10;
  if (stdin_tty == 0 || (infilestat.st_mode & IFCHR))
    return (0);
  display_source = done = bytes_in_buf = long_flag = max_input_length = 0;
  total_lines = 0;
  do
  {
    getline();
    if (linein[0])
    {              /* line not empty */
      if (prev_char == '\0')
        auto_blanks++;  /* preceeding line was blank */
      if (mode)
      {
        if (linein[0] == ' ')
        {          /* count spaces */
          for (i = 1; linein[i]; i++)
            if (linein[i] != ' ')
              break;
          left_offset = min(left_offset, i);
          if (!strncmp(linein, "    ", 4))
            auto_indents++;
        }          /* count spaces */
        else
          left_offset = 0;
      }
      if (long_flag <= 0 && input_length > 132 && (strchr(".?\42", linein[input_length - 1])))
        auto_punct++;
    }              /* line not empty */
    prev_char = linein[0];
  }
  while (done == FALSE && total_lines < (unsigned long) scan_lines);
  display_source = save_disp;
  if (!mode)
  {
    if (auto_blanks > auto_indents)
      xformat = BLOCK_PARA;
    if (auto_punct > auto_blanks && auto_punct > auto_indents)
      xformat = LINE_PARA;
  }
  linein[0] = '\0';
  return (max_input_length);
}                  /* get_paragraph_type */

void check_purge(void)
{                  /* see if line should be written */
  int i, j;
  BOOL purgit = TRUE;
  /* flush unless the first leftmargin-1 chars contain a nonspace char */
  for (i = 0; i < leftmargin; i++)
    /* Determine whether to flush */
    if (linein[i] != ' ' && linein[i] != '\0')  /* when reading a new Line */
      purgit = FALSE;

  if (xformat == LISTS || xformat == BLOCK)
    purgit ^= 1;   /* toggle purgit */
  if (xformat == BLOCK_PARA)
    if (!linein[0])
    {              /* blank */
      if (!blank_lines++)
        purgit = TRUE;  /* indent since this line is blank */
      else
        purgit = FALSE;
      if (!bline[0])
        purgit = FALSE;
    }              /* blank */
    else
    {              /* not blank */
      purgit = FALSE;
      blank_lines = 0;
    }              /* not blank */

  if (disablecol)
    purgit = TRUE;

  if (purgit)      /* Check for Indentation */
  {
    newline = TRUE;
    j = -1;
    for (i = 0; bline[i]; i++)
      if (bline[i] != ' ')
        j = i;
    if (j >= 0 || xformat == BLOCK_PARA)
      bpurge(-8);
    bline[0] = 0;
    if (xformat == TEXT || xformat == BLOCK_PARA || xformat == LINE_PARA)
      strcat(bline, indent);
    strcpy(bline6, bline);
    quotecount = 0;
    if (tabmargin)
      margin = setmargin;
    else
      margin = oldmargin;
    tabmargin = FALSE;
  }
}                  /* check_purge */

void get_word()
{                  /* extract a word from the input text and check for
                    * commands */
  char *cptr;
  int l, l1 = 0;
  xacronym = FALSE;

  newline = FALSE;
  while (*plinein == '\0' && !done)
  {                /* blank line and not eof */
    /*
     * line could also be blank because all words were removed from read
     * buffer
     */
    l = strlen(bline);  /* length of pending braille line */
    if (makefoot)
    {              /* foot */
      makefoot = FALSE; /* only do once for each ~w command */
      strcpy(fline, bline);
      strcpy(bline, oldline);
      strcpy(bline6, oldline6);
      pgroup = xpgroup;
      pjoin = xpjoin;
      join = xjoin;
      group = xgroup;
    }              /* foot */
    if (makehead)
    {              /* head */
      /* use bline to consstruct header */
      makehead = FALSE; /* only do this once for each ~h command */
      if (!header_flag)
        lines_in_header = 0;    /* first header line in this sequence */
      if (lines_in_header >= 5)
        print_error("\7More than 5 lines not allowed in header line %ld\n", total_lines);
      if (hline[lines_in_header] - hline[0] + curmax + 1 > sizeof(hbuf))
        print_error("\7Combined header more than %d characters in line %ld\n", sizeof(hbuf), total_lines);
      memset(hline[lines_in_header], 32, curmax);       /* fill with spaces */
      hline[lines_in_header][curmax] = 0;
      if (bline[0])
      {            /* pending braille line not empty */
        move(bline, hline[lines_in_header] + (linelength - l) / 2, l - 1);
        if (!header_flag)
        {
          heading_length = 0;
          for (l = 0; hline[l]; l++)
            if (hline[0][l] != ' ')
              heading_length = l;
        }
      }            /* pending braille line not empty */
      strcpy(bline, oldline);
      strcpy(bline6, oldline6);
      header_flag = 1;
      lines_in_header++;
      hline[lines_in_header] = hline[lines_in_header - 1] + curmax + 1;
      pgroup = xpgroup;
      pjoin = xpjoin;
      join = xjoin;
      group = xgroup;
      linelength = curmax + 1 - margin;
    }              /* head */
    else
      header_flag = 0;
    getline();
    if (remove_page_nums)
      remove_page_number();
    if (auto_center)
      is_centered(linein);
    if (cb_flag)
      cb_flag--;   /* end computer braille unless there is a line
                    * continuation */
    if (auto_toc_flag)
      process_auto_toc();
    if (toc_line[0])
    {
      strcpy(temp, linein);
      compact_line(temp);
      if (strcmpi(temp, toc_line) == 0)
      {
        if (toc_line_count == 0 && current_pass == 1)
        {
          rewind(toc_file_ptr);
          read_toc_line();
        }
        sprintf(temp, "%s%d ", toc_format, toc_line_count);
        insert(temp, linein);
        read_toc_line();
      }
    }
    check_purge();
    if (keep_format)
      break;
    if (!linein[0])
      table_definition[0] = '\0';
  }                /* blank line and not eof */
  if (!linein[0])
    if (done)
    {
      words[0] = 0;
      return;
    }
  while (*plinein == ' ')
    plinein++;     /* skip leading blanks */
  cptr = strchr(plinein, ' ');
  if (cptr)
  {                /* space found */
    *cptr = 0;     /* put null at end of word */
    strncpy_zero(words, plinein, sizeof(words));
    *cptr = ' ';   /* restore linein */
    plinein = cptr + 1; /* where to search for next word */
  }                /* space found */
  else
  {                /* word at end of line */
    strncpy_zero(words, plinein, sizeof(words));
    plinein += strlen(words);   /* we are done with this line */
  }                /* word at end of line */
  if (strchr(words, format_char))
    do_commands(8);
  words[MAXWORDLEN] = '\0';     /* words shouldn't be longer than that anyway */
  /* change words containing string of _ to -, looks better in braille */
  if (words[0] == '_')
  {                /* _ */
    l = 0;
    while (words[++l] == '_');
    if (l > 2 && words[l] == '\0')
      strnset(words, dash, l);
  }                /* _ */
  return;
}                  /* get_word */

void remove_page_number()
{
  int l, l1;
  char c = '\0';
  if (trans_mode == 2 || input_length > 132)
    return;
  if ((input_length != max_input_length) && (remove_page_nums & BIT1) == 0)
    return;
  l = input_length - 1;
  if (trans_mode == 3)
  {                /* back translate */
    /* check for arabic braille numbers */
    while ((toupper(linein[l]) >= 'A' && toupper(linein[l]) <= 'J')
           || linein[l] == dash)
      l--;
    c = linein[l];
    if (c == '#')
    {              /* # */
      if ((remove_page_nums & BIT0) == 0)
        return;
      l--;
      if ((remove_page_nums & BIT2) && toupper(linein[l]) >= 'A' && toupper(linein[l]) <= 'J')
        l--;       /* remove lprint page letter */
      if (remove_page_nums & BIT3)
      {            /* book page line */
        l1 = l;
        while (linein[l1] == dash || linein[l1] == ' ')
          l1--;
        if (l1 <= 0)
        {
          linein[0] = '\0';
          l = 0;
          if (keep_format)
            goto done;
        }
      }
      goto test_for_space;
    }              /* # */
  }                /* back translate */
  else
  {
    while (isdigit(linein[l]))
      l--;
    if (linein[l] == ' ')
      if ((remove_page_nums & BIT0) == 0)
        return;
      else
        goto test_for_space;
  }
  /* check for roman braille number */
  if ((remove_page_nums & BIT4) == 0)
    return;
  l = input_length - 1;
  while (strchr("IXVixv", linein[l]))
    l--;
  if (linein[l] == ';' && trans_mode == 3)
    l--;
test_for_space:
  if (l > input_length - 7 && linein[l] == ' ' &&
      linein[l - 1] == ' ' && l > 0)
  {                /* got number */
    while (l > 0 && linein[l - 1] == ' ')
      linein[--l] = '\0';       /* remove trailing spaces */
done:
    eolptr = linein + l;
    total_removed_pages++;
  }                /* got number */
}                  /* remove_page_number */

void set_vect(char *wordstring)
{                  /* set up the vector for the characters in a word */
  char *wptr = &wordstring[0];
  int *cap_ptr = &capvec[0], *sub_ptr = &subvec[0];
  int i;
  do
  {
    *cap_ptr = *sub_ptr = 0;
    if (*wptr >= '\0')
    {              /* not graphics character */
      if (isupper(*wptr))
        *cap_ptr = UPPER;
      else
      {            /* not uppercase */
        if (islower(*wptr))
          *cap_ptr = LOWER;
        else
          if (*wptr == vbar && (format_char != '\r' || vbar_fmt) &&
              wptr[1] != '\0' && isdigit(b->replace[b->start1[48]][0]) == 0)
        {          /* vertical bar */
          *cap_ptr = SKIPTRANS;
          cap_ptr++;
          sub_ptr++;
          wptr++;
          *cap_ptr = NOTRANS;   /* do not translate character after | */
          if (trans_mode != 3)
            *wptr |= 128;       /* set b7 on character not to be translated */
          /* this prevents this char from being a match in do_letter */
          vbar_fmt = 0;
        }          /* vertical bar */
      }            /* is not uppercase */
    }              /* not graphics character */
    else
    {              /* graphics character */
      *cap_ptr = LOWER;
      i = b->typex[b->start1[(int) (unsigned char) *wptr]];
      if (i == 29)
        *cap_ptr = UPPER;
    }              /* graphics character */
    wptr++;
    cap_ptr++;
    sub_ptr++;
  }
  while (*wptr);
  *sub_ptr = *cap_ptr = '\0';
  strupr(wordstring);
  if (trans_mode == 3)
  {
    for (i = 0; wordstring[i]; i++)
      if (capvec[i] != SKIPTRANS)
        capvec[i] = LOWER;
  }
}                  /* set_vect */

void do_letter(int wlength, char *wordstring)
{                  /* Check the extracted word against the table of valid
                    * types: 1-use anywhere 2-must be exact match 3-at
                    * beginning or all 4-only in middle 5-joins with same
                    * type 6-joins next-to,into,by 7-not at beginning 8-the
                    * word BE 9-his,was,were,enough 10-only at end	     */
  int i, j, k, k1, l, matchend, wstart, casetype, idx;
  BOOL matched, end_flag;
  char current_match[80], *next_match, ch, *cptr;
  lastmatch = count;
  wstart = cap_flag;
  k = 0;
  join = group = matched = FALSE;
  firstletter++;
  subvec[count] = point;        /* subvec indexes single letter in case
                                 * there's no match */
  ch = wordstring[count + 1];
  i = (int) (unsigned char) (wordstring[count] - 64);
  if (i <= 0 || i > 26)
    i = point;     /* graphics or control character */
  else
  {                /* letter */
    j = (int) (unsigned char) (ch - 64);
    if (!ch)
      j = 0;
    if ((j <= 0 || j > 26) && ch)
    {              /* not letter */
      i = (int) (point + 1);
      if (b->match[point][0] != b->match[point + 1][0] || trans_mode == 3)
        i--;       /* only one entry for this character in table */
    }              /* not letter */
    else
      i = b->start2[i][j];
  }                /* letter */
  /*
   * skip over leading punctuation to determine if we are at start of word if
   * back translating
   */
  if (trans_mode == 3 && firstletter != wstart)
  {                /* back translation not at beginning */
    for (idx = 0; idx < count; idx++)
      if (strchr(lead_back_punct, wordstring[idx]) == 0)
        break;
    if (count >= 2 && wordstring[count - 1] == '6' && wordstring[count - 2] == '9')
    {
      idx = count;
      if (count >= 3 && strchr(lead_back_punct, wordstring[count - 3]) == 0)
        idx = 0;
    }
    if (idx == count)
      wstart = firstletter;
  }                /* back translation not at beginning */

/*for example the word this i=20 j=8 and start2[i][j] points to where words
* starting with th start*/
  if (xgrade >= 2 && !xacronym)
    do
    {
  restart:
      strcpy(current_match, b->match[i]);
      l = (int) strlen(current_match);
      matchend = count + l - 1; /* points to last character of proposed match */
      if (strncmp(current_match, wordstring + count, l))
        goto try_next_match;    /* not a match */
      k = b->typex[i];
      if (firstletter == wstart || begin_flag)
      {            /* at beginning of word */
        if (split_word)
        {          /* split word */
          if (k == 2 && strcmp(current_match, b->replace[i]))
            goto try_next_match;
          if (split_word == 2)
          {        /* end of hyphenated word */
            if (k == 7)
              k = 1;
            if (k == 3)
              goto try_next_match;
          }        /* end of hyphenated word */
        }          /* split word */
        if (k == 4 || k == 7 || k == 15)
          goto try_next_match;  /* these types cannot be at beginning of word */
      }            /* at beginning of word */
      else
      {            /* not at beginning */
        if (k == 2 || k == 3 || k == 6 || k == 8 || k == 11 || k == 12)
          goto try_next_match;
        if (trans_mode == 3)
        {          /* back translation */
          if (k == 7 && number_word == total_words)
            goto try_next_match;
          if ((k == 13 && current_match[0] != '-') || k == 18)
            goto try_next_match;
        }          /* back translation */
        else
        if (k == 17 || k == 18 || k == 30)
          goto try_next_match;  /* not a match because not at beginning of
                                 * word */
      }            /* not at beginning */
      k1 = b->typex[b->start1[(int) wordstring[matchend + 1]]]; /* Next Letter */
      if (k1 > 0 && k1 < 19)
      {            /* not at end next char is a letter */
        end_flag = FALSE;       /* assume not at end of word */
        /*
         * skip over trailing punctuation to determine if we are at end of
         * word
         */
        if (trans_mode == 3 && wordstring[count] != ',')
        {
          if (k == 13)
            begin_flag = 2;
          if ((!(wordstring[count + 1] == '4' || wordstring[count + 1] == '2')) ||
              wordstring[count - 1] != ',')
          {
            /* if not a capital initial like J. */
            for (idx = matchend + 1; wordstring[idx]; idx++)
            {
              if (!strchr(braille_punct, wordstring[idx]))
                break;
            }      /* idx */
            if (wordstring[idx] == '\0' || idx - matchend > 2)
              end_flag = TRUE;
            if (!end_flag && wordstring[matchend + 1] == '\'' &&
                wordstring[matchend + 2] == 'S')
              end_flag = TRUE;  /* 's */
            if (wordstring[0] != '-' && wordstring[matchend + 1] == '-')
              end_flag = TRUE;
            if (k == 2 && strcmp(wordstring, "P4") == 0)
            {
              if (*plinein == '#')
                goto try_next_match;
              if (!*plinein)
              {    /* check on next line */
                cptr = ioptr;
                while (*cptr == ' ')
                  cptr++;
                if (*cptr == '#')
                  goto try_next_match;
              }    /* check on next line */
            }
          }
        }
        if (end_flag == FALSE)
          if (k == 2 || k == 6 || k == 10 || k == 12 ||
              (trans_mode == 1 && (k == 17 || k == 18 || k == 30)))
            goto try_next_match;
        if (trans_mode == 1)
        {          /* forward translation */
          if (k == 13 || (k == 15 && strchr(vowels, wordstring[matchend + 1]) == NULL))
            goto try_next_match;
          if (k == 14 && strchr(consonants, wordstring[matchend + 1]) == NULL)
            goto try_next_match;
          if (k == 15 && strchr(vowels, wordstring[count - 1]) == NULL)
            goto try_next_match;
        }          /* forward translation */
      }            /* not at end */
      else         /* Set end of Word if next char not Letter */
        end_flag = TRUE;
      if (end_flag)
      {            /* at end */
        if (k == 4 || (k == 3 && strcmp(current_match, "IN") == 0))
          /* in [3,4] */
          goto try_next_match;
        if (k == 11 || (k > 12 && k < 16 && trans_mode == 1))
          goto try_next_match;  /* at beginning not all, or French types */

        if (k == 5 && firstletter == wstart && wordstring[wstart] != '\\')
        {
          group = TRUE; /* join with same type */
          if (pgroup == FALSE && strcmp(current_match, "A") == 0)
            group = FALSE;
        }
        if (pjoin && k == 8)
          goto try_next_match;
        if (k == 2 && strcmp(current_match, "US") == 0 &&
            capvec[firstletter] == UPPER && capvec[firstletter + 1] == UPPER)
          goto try_next_match;
        if (trans_mode == 1 && k >= 17 && k < 19)
        {          /* beginning of possible 2-3 word match */
          strcpy(word_buf[0], words);   /* save word */
          type17_table_entry = i;
          type17_word_count = 0;
          i = table_entries + 1;        /* replace will be null in subvec for
                                         * this word */
          goto got_match;
        }          /* beginning of possible 2-3 word match */
      }            /* at end */
      if ((k == 8) && (matchend == wlength - 1)
          && (wlength != l))
        goto try_next_match;
      if ((k == 9) &&   /* was, his, etc. */
          ((l != wlength) ||
           pjoin))
      {
        /* leading or trailing punctuation */
        if (spanish_flag == 0)
          goto try_next_match;  /* don't translate if there is leading or
                                 * trailing punctuation */
        spanish_flag = 1;
        if ((capvec[count] & LOWER) ||
            (k1 > 0 && k1 <= 19) || (wstart != firstletter))
          goto try_next_match;  /* don't translate if lower case */
        if (count)
        {          /* leading */
          j = (int) dot_table[(int) b->replace[subvec[0]][0]];
          if (!(j & 9))
            goto try_next_match;        /* don't translate if lower sign
                                         * punct */
        }          /* leading */
        else
          goto try_next_match;
        spanish_flag = 2;
      }
      if (k == 6)
      {
        if (matchend - wstart + 1 != wlength)
          goto try_next_match;  /* to Into by */
        join = TRUE;
      }
      if (k == 12)
        djoin = 1;
      if (k == 16)
      {
        if (prev_type == 16)
        {
          i++;
          subvec[count - 1]++;
          prev_type = 1;
        }
      }
      else
      if (prev_type == 16)
        subvec[count - 1]++;
      prev_type = k;

      casetype = capvec[count];
      if (casetype == LOWER)    /* lowercase */
        for (j = 1; current_match[j]; j++)
          if (isupper(current_match[j]))
            if (capvec[count + j] != casetype)
              goto try_next_match;      /* no matches with lower and upper
                                         * case */
      if (trans_mode == 3)
      {            /* back translation */
        if (k == 14)
        {          /* computer braille indicator */
          switch (current_match[1])
          {
          case '&':
            if (cb_flag)
              cb_flag = 2;
            break;
          case '+':
            cb_flag = 1;
            capslock = 0;       /* start output in lower case */
            break;
          case '>':
            cb_flag = 1;
            capslock = 2;       /* output upper case until reset */
            break;
          case ':':
            cb_flag = 0;
            break;
          case '<':
            capslock = 0;
            break;
          }        /* switch */
          if (cb_flag == 1)
          {        /* check */
            /*
             * make sure computer braille is properly terminated or
             * continued, if not end now
             */
            cptr = plinein;     /* beginning of next word or end of line */
            do
            {
              cptr--;
              if (*cptr == '_' &&
                  (cptr[1] == '+' || cptr[1] == '>'))
                break;
            }      /* while */
            while (cptr != linein);
            if (strpos(cptr, "_:") + strpos(cptr, "_&") == 0)
              cb_flag = 0;      /* improper indicators */
          }        /* check */
        }          /* computer braille indicator */
        else
        if (cb_flag)
        {          /* computer braille on */
          if (wordstring[count] == '_' && isalpha(wordstring[count + 1]))
          {        /* single cap */
            delete(wordstring + count, 1);      /* remove _ */
            i = b->start1[(int) wordstring[count] + 1] - 2;
            capslock = 1;
            goto restart;
          }        /* single cap */
          if (capslock)
            capvec[count] |= UPPER;
          if (capslock == 1)
            capslock = 0;
          goto try_next_match;
        }          /* computer braille on */

        if (k == 2 && l == 1 && wordstring[1] == '4' && wordstring[0] > 'A' && wordstring[0] < 'M')
          if (number_word == total_words - 1l ||
              (line_word == total_words && linein[left_offset] == ' '))
            goto try_next_match;
        if (k == 17)
        {          /* set case */
          if (strchr(current_match, ';'))
            for (l = matchend + 1; wordstring[l]; l++)
              capvec[l] = LOWER;
          if (strchr(current_match, ','))
          {        /* capitalization */
            cap_flag = matchend + 1;
            capvec[cap_flag] = UPPER;
            if (strpos(current_match, ",,"))
              for (l = cap_flag; wordstring[l]; l++)
                capvec[l] = UPPER_ALL;
          }        /* capitalization */
          if (strchr(current_match, ','))
            firstletter = cap_flag - 1;
          if (count && wordstring[count - 1] == ';')
            cap_flag--;
        }          /* set case */
      }            /* back translate */
  got_match:
      subvec[count] = i;
      count = matchend;
      matched = TRUE;
      break;

  try_next_match:
      if (table_stat_file)
        if ((long) total_lines >= table_start_line)
          fprintf(table_stat_file, "    %s %d skipped\n", current_match, b->typex[i]);
      i++;
      next_match = b->match[i];
      if (wordstring[count] < 0 || wordstring[count + 1] < 0)
      {            /* extended graphics */
        if (current_match[0] == next_match[0])
          current_match[1] = next_match[1];
      }            /* extended graphics */
      if (trans_mode == 3)
      {
        if (next_match[0] == current_match[0])
          goto restart;
        break;
      }
    }              /* while */
    while (next_match[1] == current_match[1]);

  if (trans_mode == 3)
  {                /* back translate */
    if (cb_flag)
    {
      if (k != 14)
        subvec[count] = i - 1;
    }
    if (k == 18 || k == 19)
      do_number(wordstring);
    if (begin_flag)
      begin_flag--;
  }                /* back translate */
  if (table_stat_file && type17_table_entry == 0 &&
      (long) total_lines >= table_start_line)
  {
    k = subvec[lastmatch];
    fprintf(table_stat_file, "  %s %s %d\n", b->match[k], b->replace[k], b->typex[k]);
  }
}                  /* do_letter */

void do_number(char *wordstring)
{                  /* convert numbers type 19 */
  int i, j;
  char ch;
  if (!xgrade)
    return;        /* no translation */
  if (trans_mode == 3)
  {                /* back translate */
    firstletter++;
    number_word = total_words;
    for (i = count + 1; wordstring[i]; i++)
    {
      ch = wordstring[i];
      if (ch >= 'A' && ch <= 'J')
      {            /* braille digit */
        capvec[i] = NOTRANS;
        wordstring[i] = number_back[(int) ch - 65];
        continue;  /* number back translated so continue */
      }            /* braille digit */
      if (ch > 'J')
        number_word--;  /* allows translation of type 7 ;e ;t ... */
      if (ch == '/' && wordstring[i + 1] == '\0')
        break;     /* back translate / */
      if (ch == ';' || ch > 'J' || (ch < 'A' && strchr("/.-13", ch) == 0))
        break;     /* stop number translation, char doesn't belong */
      else
      {            /* character belongs */
        capvec[i] = NOTRANS;
        if (ch == '1')
          wordstring[i] = ',';
        if (ch == '3')
          wordstring[i] = ':';
      }            /* character belongs */
      if (ch == dash)
      {            /*-*/
        if (wordstring[i + 1] >= 'A' && wordstring[i + 1] <= 'Z' &&
            wordstring[i + 2] == '#')
        {          /* lprint page */
          delete(wordstring + i + 2, 1);        /* remove # sign */
          i++;
          capvec[i] = NOTRANS;  /* don't translate letter before print page
                                 * number */
          continue;
        }          /* print page */
        for (j = i + 1; wordstring[j]; j++)
        {
          if (wordstring[j] == ';')
            break;
          if (wordstring[j] > 'J')
          {
            number_word--;
            return;/* word probably follows dash */
          }
        }          /* j */

        continue;
      }            /*-*/
    }              /* i */
    return;
  }                /* back translate */
  firstletter = -1;
  subvec[count] = point;
  capvec[count] = NUMERIC;
}                  /* do_number */

/*  Convert THE PUNCTUATION    */
/*  Type 21 - Simple Replace   */
/*	 22 - . 	       */
/*	 24 - '                */
/*	 25 - "                */
/*	 27 - - 	       */
void do_punct(char *wordstring)
{
  int i, k;
  BOOL matched;
  char ch = b->match[point][0];
  BOOL apostrophe;

  while (strncmp(b->match[point], wordstring + count, strlen(b->match[point])))
  {
    point++;
    if (b->match[point][0] != ch)
      print_error("\7Unable to match punctuation in line %ld\n", total_lines);
  }                /* while */

  if (count > 0)   /* Check for capital mark requirement */
    if (lastmatch >= 0)
    {
      k = b->typex[abs(subvec[lastmatch])];
      if ((k == 6 || k == 8 || k == 9) && spanish_flag <= 1)
        subvec[lastmatch] = -abs(subvec[lastmatch]);    /* lower sign not
                                                         * allowed followed by
                                                         * punctuation */
    }

  matched = FALSE;

  join = group = pgroup = apostrophe = FALSE;
  k = b->typex[point];

  if (k == 24)
  {                /* apostrophe */
    if (count)
    {              /* not first character */
      if ((isupper(wordstring[count - 1]) || isdigit(wordstring[count - 1])) &&
          (isupper(wordstring[count + 1]) || isdigit(wordstring[count + 1])))
        apostrophe = TRUE;      /* adjacent to uppercase letter or digit */
      if ((isupper(wordstring[count - 1]) || isdigit(wordstring[count - 1])) &&
          quotecount == 0)
        apostrophe = TRUE;
      if (wordstring[0] == '"')
        apostrophe = TRUE;
    }              /* not first character */
    else
    if (!openlevel)
      apostrophe = TRUE;
  }                /* apostrophe */
  if (apostrophe && quotecount == 0)
    test_join();
  if ((k == 24 || k == 25) && apostrophe == FALSE)
  {                /* Handle quotes */
    if (!count)
      goto begin_quote; /* assume quote at beginning of word is an open quote */
    c = wordstring[count - 1];
    if (c == '(' || c == '[')
      goto begin_quote;
    /* ending quote */
    subvec[count] = b->start1[34] + quotecount + 1;
    if (openlevel)
      openlevel--;
    quotecount = 0;
    return;
begin_quote:
    openlevel++;
    if (openlevel > 2)
      openlevel = 0;
    if (openlevel == 2)
      quotecount = 2;   /* inner quotes */
    subvec[count] = b->start1[34] + quotecount;
    test_join();
    return;
  }                /* handle quote */

  do
  {                /* Find replacement */
    i = (int) (count + strlen(b->match[point]) - 1);
    if (!strncmp(b->match[point], wordstring + count, strlen(b->match[point])))
    {
      matched = TRUE;
      subvec[count] = point;
      count = i;
    }
    else
      point++;
  }
  while (!matched);

  if (b->typex[point] != 27)
    firstletter = -1;
}                  /* do_punct */

void test_join()
{
  if (pjoin)
  {
    pjoin = FALSE;
    strcpy(bline, bline6);      /* reset for to into by */
  }
}                  /* test_join */

void build_word(char *wordstring, char *bwordstring)
{                  /* build the word from the information kept in the
                    * contraction vector.  The grade two translation for hour
                    * would set subvec[0] to index h, subvec[1] to index the
                    * ou sign, subvec[2] would be 0, and subvec[3] indexes r */
  BOOL no_lower_sign, num = FALSE, allcaps = FALSE;
  int i, i1, j, k, k1;
  char *ptr;
  bwordstring[0] = bword6[0] = '\0';    /* start with no output word */
  bwordstring[MAXWORDLEN] = '\0';       /* tells if word gets too long */

  strcpy(oldword, wordstring);  /* save input word */
  for (i = 0; wordstring[i]; i++)
  {                /* for each character of the input word */
    if (bwordstring[MAXWORDLEN])
      break;       /* quit before braille word overflows */
    j = (int) strlen(bwordstring);
    if (capvec[i] & NOTRANS)
    {              /* do not translate this char */
      if (capvec[i] != SKIPTRANS)
      {            /* vertical bar */
        wordstring[i] &= (char) 127;
        bwordstring[j] = (char) toupper(wordstring[i]);
        bwordstring[j + 1] = 0; /* only 1 character was added to bword */
      }            /* vertical bar */
    }              /* do not translate this char */
    else
    if (subvec[i])
    {              /* add translation to braille word */
      if (subvec[i] < 0)
      {            /* lower sign not allowed before punctuation */
        no_lower_sign = TRUE;
        subvec[i] = -subvec[i];
      }            /* lower sign not allowed before punctuation */
      else
        no_lower_sign = FALSE;

      i1 = subvec[i];
      k = b->typex[i1];
      k1 = b->typex[subvec[i + 1]];
      if (trans_mode == 3 && k == 19)
        k = 1;
      if ((!num) && ((k == 19)
                     || ((k == 22) && (k1 == 19))))
      {            /* start of number or decimal number */
        if (isdigit(b->replace[i1][0]) == 0 || k == 22)
          if (bwordstring[j - 1] != '#')
            strcat(bwordstring, numeric_def);
        num = TRUE;
        allcaps = join = group = pgroup = FALSE;
      }            /* start of number or decimal number */

      if (k == 19 || k == 21 || k == 23 || k == 24 || k == 25 || k == 27 || k == 28)
        strcat(bwordstring, b->replace[i1]);

      if (k == 22) /* period */
        if (k1 == 19)   /* numeric follows period */
          strcat(bwordstring, numeric_def + 2); /* decimal point */
        else
          strcat(bwordstring, b->replace[i1]);

      if (k == 26)
        /* Percent */
        if (num)
        {
          j = strpos(bwordstring, "#");
          if (!j)
            j++;
          insert(b->replace[i1], bwordstring + j - 1);
        }
        else
          strcat(bwordstring, b->replace[i1]);

      if ((k > 0 && k < 19) || k == 29 || k == 30)
      {            /* 1-19 28 */
        ptr = wordstring + i - 1;
        if (num && strcmp(b->match[i1], "-"))
        {          /* end of number */
          num = FALSE;
          /* insert ' between a number and s such as 39s */
          if (ptr[2] == '\0' && ptr[1] == 'S' && ptr[0] != '\47')
            strcat(bwordstring, "'"), j++;
          strcat(bwordstring, letter_sign);
          if (ptr[2] == '\0' && ptr[1] == 'S' && bwordstring[j - 1] == '\47')
            bwordstring[j] = '\0';
          if (ptr[0] == dash && ptr[1] > 'J')
            bwordstring[j] = '\0';      /* remove letter sign */
          if (strcmpi(ptr, "1st") == 0 || strcmpi(ptr, "2nd") == 0 ||
              strcmpi(ptr, "3rd") == 0 ||
              (strcmpi(ptr + 1, "th") == 0 && ((*ptr > '3' && *ptr <= '9') || *ptr == '0')
               || *ptr == '\0'))
            bwordstring[j] = '\0';      /* remove letter sign */
        }          /* end of number */
        if (allcaps == FALSE && capvec[i] == UPPER && xgrade != 3)
        {          /* insert capital marks */
          pgroup = FALSE;
          j = (int) strlen(bwordstring);
          strcat(bwordstring, cap_single);
          if (capvec[i + 1] == UPPER)
          {
            strcpy(bwordstring + j, cap_all);
            allcaps = TRUE;
          }
        }          /* insert capital marks */
        if (k == 6)
        {          /* to into by */
          sprintf(bword6, "%s%s", bword, b->match[subvec[i]]);
          if (!strcmp(bword6, "INTO"))
            strcpy(bword6, "9TO");
        }          /* to into by */

        if (no_lower_sign)
          strcat(bwordstring, b->match[i1]);
        else
          strcat(bwordstring, b->replace[i1]);
        if (trans_mode == 3)
        {
          if (capvec[i] == LOWER)
            strlwr(bwordstring + j);
          if (capvec[i] & UPPER)
            strupr(bwordstring + j);
          if (capvec[i] != UPPER_ALL)
            strlwr(bwordstring + j + 1);
        }
        else
        if (allcaps && k == 30 && isalpha(wordstring[0]))
          insert(letter_sign, bwordstring);
      }            /* 1-19 28 */
    }              /* add translation */
    if (trans_mode == 1 && strcmp(wordstring, "$") == 0)
    {
      strcpy(bwordstring, "4#");
      strcpy(bword6, bwordstring);
    }
  }                /* i */
  if (xgrade == 3)
  {                /* grade 3 */
    if (isdigit(wordstring[0]) == 0 && (grade_mod & BIT6))
    {              /* first character not a digit */
      for (i = 1; bwordstring[i]; i++)
        if (bwordstring[i] == 'A' && bwordstring[i + 1])
        {          /* delete */
          delete(bwordstring + i, 1);
          i--;
        }          /* delete */
    }              /* first character not a digit */
    if (isdigit(wordstring[0]) && (grade_mod & BIT7))
    {              /* number */
      i = atoi(wordstring);
      if (i > 9 && i <= 50)
      {            /* substitute grade 3 number */
        delete(bwordstring + 1, 1);
        bwordstring[1] = g3_numbers[i - 10];
      }            /* substitute grade 3 number */
    }              /* number */
  }                /* grade 3 */
}                  /* build_word */

void check_ham_call(char *wordstring)
{
  int i = 0, l = (int) strlen(wordstring), l1;
  if (ham_call == 0 || l > 6 || trans_mode == 3)
    return;        /* too long or back translating */
  strcpy(temp, wordstring);
  strupr(temp);
  if (ham_call == 1)
    if (strcmp(temp, wordstring))
      return;      /* not uppercase */
  while (call_prefix[i])
  {
    l1 = (int) strlen(call_prefix[i]);
    if (!strncmp(temp, call_prefix[i], l1))
    {              /* match */
      if (isdigit(temp[l1]) == 0 || isalpha(temp[l1 + 1]) == 0)
        return;
      xacronym = TRUE;
      total_ham_calls++;
      break;
    }              /* match */
    i++;
  }                /* while */
}                  /* check_ham_call */

void trans_word(char *wordstring)
{
  int k, l;
  lastmatch = 0;
  firstletter = count = prev_type = -1;
  l = (int) strlen(wordstring);
  if (table_stat_file && (long) total_lines > table_start_line)
    fprintf(table_stat_file, "%s\n", wordstring);
  do
  {
    count++;
    if (!(capvec[count] & NOTRANS))
    {              /* not | */
      chardec = (int) (unsigned char) wordstring[count];
      point = b->start1[chardec];       /* = first table entry for this
                                         * character */
      k = b->typex[point];
      if (!k)
        undefined_char();
      if ((k == 28 || k == 29) && xgrade > 1)
        if (b->match[point][0] == b->match[point + 1][0])
        {
          point++;
          k = b->typex[point];
        }
      if (((k <= 18 || k == 30) && trans_mode == 1) || (k <= 16 && trans_mode == 3))
        do_letter(l, wordstring);
      else
      {            /* not a letter */
        if (k == 19)
          do_number(wordstring);
        else
          do_punct(wordstring);
      }            /* not letter */
    }              /* not | */
  }
  while (wordstring[count + 1]);
}                  /* trans_word */
void undefined_char()
{
  print_error("\007Character \042%c\042 decimal %d not in %s line %ld %s\n",
              (char) chardec, chardec, active_table, total_lines,
              inf_name);
}                  /* undefined_char */

BOOL store_next_token()
{                  /* removes next token from temp storing it in token[]
                    * increment token_count */
  int retval = FALSE;
  char delim = ' ';
  char *cptr;
  trim(temp);      /* remove leading spaces */
  if (temp[0] == (char) 39 || temp[0] == (char) 34)
  {                /* ' or " */
    delim = temp[0];
    delete(temp, 1);    /* remove delim */
  }
  if (!temp[0])
    return (FALSE);/* empty line */
  if (token_count >= MAX_TOKENS)
    print_error("\7More than %d tokens on line %d\n", MAX_TOKENS,
                linecount);

/*account for quotes in option command if present*/
  if (delim == ' ' && temp[2] == '=' &&
      (temp[3] == (char) 34 || temp[3] == (char) 39))
  {
    cptr = NULL;
    goto store_token;
  }
  cptr = strchr(temp, delim);
  if (cptr == NULL)
  {                /* last token on line */
    strcpy(token[token_count], temp);
    temp[0] = 0;
  }                /* last token */
  else
  {                /* found matching delim */
    *cptr = 0;
store_token:
    strcpy(token[token_count], temp);
    if (cptr)
      strcpy(temp, cptr + 1);
    else
      temp[0] = '\0';   /* option with quotes */
    retval = TRUE;
  }                /* found delim match */
  if (strlen(token[token_count]) >= MAX_TOKEN_LEN)
    print_error("\7Token too long in line %d\n", linecount);
  token_count++;
  return (retval);
}                  /* store_next_token */

void pop_token(char *string)
{
  string[0] = 0;
  if (current_token >= token_count)
    return;        /* no more stored tokens */
  strcpy(string, token[current_token]);
  current_token++;
}                  /* pop_token */

int check_token()
{
  int i;
  if (!field[0])
    token_char = 199;
  else
  {                /* not empty */
    if (atol(field) > 0)
      token_char = 888;
    else
    {              /* word */
      strupr(field);
      i = -1;
      do
      {
        i++;
        if (!strncmp(field, tokens[i].name, strlen(tokens[i].name)))
          break;
      }
      while (tokens[i].name != NULL);
      if (tokens[i].value == 999)
        print_error("\7Unknown token %s in line %d\n", field, linecount);
      token_char = tokens[i].value;
    }              /* word */
  }                /* not empty */
  return (token_char);
}                  /* check_token */

void add_efl_data(char *string, foptype * f)
{
  int l = strlen(string);
  if (l + efl_data_offset >= MAX_EFL_DATA_BUF)
    print_error("\007efl data exceeds %d characters in line %d\n",
                MAX_EFL_DATA_BUF, linecount);
  strcpy(efl_data + efl_data_offset, string);
  f->data = efl_data + efl_data_offset;
  efl_data_offset += l + 1;
}                  /* add_efl_data */

void store_commands()
{
  int i, k;
  current_token = 0;    /* start from beginning of token list for this line */
  pop_token(field);/* get first token */
  if (!strcmpi(field, "LINE"))
  {
    pop_token(field);
    if (check_token() != 888)
      print_error("\7No line numbers specified in line %d\n", linecount);

    l[lopcount].startline = l[lopcount].endline = atoi(field);  /* starting line */
    l[lopcount].fopstart = (int) fopcount;      /* where commands start for
                                                 * this line */
    pop_token(field);
    if (check_token() == 888)
    {              /* ending line number specified */
      l[lopcount].endline = atoi(field);
      if (l[lopcount].endline < l[lopcount].startline)
        print_error("\7Ending line less than starting line in line %d\n",
                    linecount);
      pop_token(field);
    }              /* ending line number specified */
  }
  else
    l[lopcount].startline = 1, l[lopcount].endline = 32767;
  if (check_token() >= 202 && token_char <= 203)
  {                /* match */
    test_range("match");
    l[lopcount].match_active = 1 + (token_char > 202);  /* match subcommand is
                                                         * active */
    if ((unsigned int) f[fopcount].flen != strlen(field))
      print_error("\7Match data must be %d characters in line %d\n",
                  f[fopcount].flen, linecount);
    strupr(field);
    add_efl_data(field, &f[fopcount]);
    pop_token(field);
    fopcount++;
  }                /* match */
  k = fopcount;
  while (check_token() < 100)
  {                /* delete skip center indent list text page */
    if (format_char == '\r' && (token_char == 4 || token_char == 5))
      print_error("\007Cannot skip or center lines: formatting is disabled in line %d\n",
                  linecount);
    f[fopcount].fop = token_char;
    pop_token(field);
    if (token_char == 80)
    {              /* page */
      f[fopcount].fstart = atoi(field);
      pop_token(field);
    }              /* page */
    fopcount++;
  }                /* while */

  while (check_token() == 100)
  {                /* field */
    test_range("field");
    if (check_token() > 100 && token_char < 199)
    {              /* state or omit token */
      f[fopcount].fop = token_char;
      pop_token(field);
    }              /* state or omit */

    if (check_token() == 199)
    {              /* append */
      pop_token(field);
      add_efl_data(field, &f[fopcount]);
      pop_token(field);
      if (!f[fopcount].fop)
        /* there was no previous state or omit */
        f[fopcount].fop = 199;  /* indicate append operation */
    }              /* append */
    if (!f[fopcount].fop)
      print_error("\7No valid field operation specified in line %d\n",
                  linecount);
    fopcount++;    /* valid field op was found */
  }                /* while */
  if (token_char >= 301 && token_char <= 312)
  {                /* replace */
    f[fopcount].fop = token_char;
    pop_token(field);
    strupr(field); /* make case insensitive */
    if (!field[0])
      print_error("\007No search string in replace or word command in line %d\n", linecount);
    for (i = 0; i < 3; i++)
    {
      add_efl_data(field, &f[fopcount]);
      pop_token(field);
      fopcount++;
      if (i == 1 && token_char < 312)
        break;
    }              /* i */
  }                /* replace */

  if (token_char == 315 || token_char == 320 || token_char == 325)
  {                /* option or line */
    pop_token(field);
    strupr(field); /* make case insensitive */
    add_efl_data(field, &f[fopcount]);
    f[fopcount].fop = token_char;
    fopcount++;
  }                /* option or line */
  if (!f[k].fop)
    print_error("\7No command specified in line %d\n", linecount);
  l[lopcount].fopend = fopcount;
  lopcount++;
}                  /* store_commands */

void test_range(char *string)
{                  /* used in store_commands to test range of match or field
                    * entry */
  int i, fieldpos = 160, op;
  pop_token(field);
  if (check_token() != 888)
  {                /* no number specified */
    print_error("\007No %s numbers specified in line %d\n", string, linecount);
  }                /* no number specified */
  f[fopcount].fstart = (abs(atoi(field)) - 1);
  f[fopcount].flen = 1;
  pop_token(field);
  if (check_token() == 888)
  {                /* got a number */
    i = abs(atoi(field)) - 1;
    f[fopcount].flen = i - f[fopcount].fstart + 1;
    if (f[fopcount].flen < 1)
      print_error("\007Invalid %s range in line %d\n", string, linecount);
    pop_token(field);
  }                /* got number */
  if (f[fopcount].fstart + f[fopcount].flen > 160)
    print_error("\007%s specification > 160 in line %d\n", string, linecount);
  for (i = l[lopcount].fopstart; i <= fopcount; i++)
  {
    op = f[i].fop;
    if (op >= 100 && op < 200)
    {              /* field operation */
      if (f[i].fstart + f[i].flen - 1 >= fieldpos)
        print_error("\007Fields out of order in line %d\n", linecount);
      fieldpos = f[i].fstart;
    }              /* field operation */
  }                /* i */
}                  /* test_range */

void load_template()
{
  if (!(efl_mode & 1))
    return;        /* don't look for .efl file */
  lfile = fopen(efl_file, fopen_read[0]);
  if (lfile == NULL)
  {                /* not found */
    if (!(efl_mode & 2))
      return;      /* ignore error, translate without .efl file */
    print_error("\007External Format File %s not found ...\n", efl_file);
  }                /* not found */

  if (efl_mode & 4)
    fprintf(stderr, "External Format File %s\n", efl_file);

  linecount = lopcount = fopcount = efl_data_offset = 0;
  /* initialize structures */
  memset(f, 0, sizeof(foptype));
  memset(l, 0, sizeof(loptype));
  memset(efl_data, 0, sizeof(efl_data));
  while (fgets(temp, 100, lfile))
  {                /* while there are lines to read */
    temp[strlen(temp) - 1] = 0; /* get rid of lf */
    linecount++;
    if (temp[0] == ';' || temp[0] == '#')
      continue;    /* skip comment */
    token_count = 0;    /* tokens found on this line */
    while (store_next_token()); /* store tokens from entire line */
    if (token_count)
      store_commands(); /* tokens were found */
  }                /* while */
  if (lopcount)
  {
    lopactive = TRUE;
    if (efl_mode & 4)
      fprintf(stderr, "%d External Format lines were Processed\n", linecount);
  }
  else
    fprintf(stderr, "No external format lines were processed\n");
  fclose(lfile);
}                  /* load_template */

void do_translate()
{
  int subtotal_eq = 0, grade_save = xgrade;
  int hyphen_mode_save, i, j, last_flag;
  char *pointer;

  copies++;
  current_pass = 1;
  total_misspells = 0l;
  xformat_save = xformat;
  toc_line[0] = '\0';
get_next_pass:
  /* initialize variables before translation */
  memset(tabtable, 0, sizeof(tabtable));
  memset(index_pages, 0, sizeof(index_pages));
  pgroup = pjoin = join = rjoin = group = FALSE;
  xacronym = braille_page_nums = TRUE;
  xcenter = openlevel = closelevel = FALSE;
  blank_lines = djoin = toc_line_count = auto_toc_flag = auto_letter_count = 0;
  xtab = dobook = xdouble = xheading = xfooting = FALSE;
  if (current_pass == 1)
    makefoot = makehead = FALSE;
  pageset = FALSE;
  doroman = FALSE;
  pagenumlen = 4 * (dopagenum == TRUE);
  disablecol = fillit = done = FALSE;
  quotecount = 0;
  margin = setmargin = oldmargin = leftmargin;
  tabmargin = FALSE;
  bpageb = bpagec = blinec = actualpage = lineinct = inpglen = it_flag = 0;
  total_equations = toc_word = hyphen_searches = hyphen_matches = dash_searches = dash_matches = 0;
  total_ham_calls = total_breaks = total_em_pages = total_em_sheets = 0;
  total_file_pages = total_file_sheets = 0;
  hyphens_used = consec_hyphens = hyphens_skipped = dashes_used = 0;
  total_words = total_cells = total_rejoins = total_lines = total_not_rejoined = number_word = 0l;
  memset(total_dots, 0, 28);
  field[0] = 0;
  addchar[0] = 0;
  curmax = maxline;
  linelength = maxline + 1 - margin;
  plinein = &linein[0];
  fill_length = 7;
  top_of_form();
  bline[0] = oldline[0] = bline6[0] = '\0';
  if (!total_words)
  {                /* no words written yet */
    if (print_file)
    {              /* file */
      strcpy(bline, inf_name + (print_file > 1) * inf_path_len);
      if (!print_date)
        bline[curmax - 4] = '\0';
      else
        bline[20 + 12 * (print_date > 1)] = '\0';       /* leave room for date */
      strlwr(bline);
    }              /* file */
    if (print_date)
    {              /* date */
      if (bline[0])
        strcat(bline, " ");
      if (print_date > 1)
        date_string[12] = '\0';
      strcat(bline, date_string);
      date_string[12] = ' ';
    }              /* date */
    if (bline[0] && pagestart == 1)
      bpurge(-9);
    bline[0] = 0;
  }                /* no words written yet */

  if (inf_des)
    lseek(inf_des, 0l, 0);      /* start from beginning for each copy */
  bytes_in_buf = long_flag = 0; /* no bytes in iobuf */
  do
  {
    get_word();
    if (type17_table_entry)
    {              /* test for 2-3 word match */
  tab_ent0:
      type17_word_count++;      /* num times get_word was called */
      pointer = word_buf[type17_word_count];
      memset(pointer, 0, 30);   /* put nulls in buffer */
      strncpy(pointer, words, 25);
      pointer = strpbrk(pointer, trailing_punct);
      if (pointer)
      {            /* remove trailing punctuation */
        word_buf[type17_word_count][29] = *pointer;
        *pointer = '\0';
      }            /* remove trailing punctuation */
      while (!strcmpi(b->match[type17_table_entry], word_buf[0]))
      {            /* while first entry in table entry matches */
        j = b->typex[type17_table_entry];
        /* test to see if there is a second match */
        if (strcmpi(word_buf[1], b->replace[type17_table_entry]))
        {          /* no second match */
          if (table_stat_file)
            fprintf(table_stat_file, "    %s-%s %d skipped\n",
            b->match[type17_table_entry], b->replace[type17_table_entry], j);
          type17_table_entry++; /* try next table entry */
          continue;/* look for another table entry */
        }          /* no second match */
        /* two matches have been satisfied at this point */
        /* entry is guaranteed to be type 17-18 from lload_table */
/*determine how many matches are required with this next table entry*/
        j = 1 + (j == 18);
        if (j == 1)
          goto replace; /* this table entry satisfies 2-word match */
        /* we know this entry is 3-word proposed match */
        if (type17_word_count == 1)
        {          /* get next word */
          get_word();
          goto tab_ent0;
        }          /* get next word */
        if (!strcmpi(words, b->replace[type17_table_entry] + 30))
          goto replace;
        if (table_stat_file)
          fprintf(table_stat_file, "    %s-%s-%s %d skipped\n",
               b->match[type17_table_entry], b->replace[type17_table_entry],
                  b->replace[type17_table_entry] + 30, j);
        type17_table_entry++;
        continue;
      }            /* while */
      /* no 2-3 match was found */
      temp[0] = '\0';
      for (i = 0; i <= type17_word_count; i++)
      {
        sprintf(temp + strlen(temp), "%s ", word_buf[i]);
        if (word_buf[i][29] && i > 0)
          word_buf[i][strlen(word_buf[i])] = word_buf[i][29];
        translate_computer(word_buf[i], bword);
        build_line();
      }            /* i */
      strupr(temp);
      if (table_stat_file)
        fprintf(table_stat_file, "    %snot 2-3 word match\n", temp);
      type17_table_entry = 0;
      continue;
  replace:
      temp[0] = '\0';
      for (i = 0; i <= type17_word_count; i++)
        sprintf(temp + strlen(temp), "%s ", word_buf[i]);
      strupr(temp);
      pointer = b->replace[type17_table_entry] + type17_word_count * 30;
      if (table_stat_file)
        fprintf(table_stat_file, "    %s%d word replace %s\n", temp, j + 1, pointer);
      translate_computer(pointer, bword);       /* replace string from
                                                 * build_line(); add to
                                                 * pending braille line */
      type17_table_entry = 0;   /* match completed */
      continue;
    }              /* test for 2-3 word match */
    if (math_flag && xgrade > 0)
    {              /* math */
      if (is_equation())
      {            /* equation */
        subtotal_eq++;
        if (subtotal_eq == 1)
        {
          current_table_grade = -1;
          load_tables(math_table);
        }
      }            /* equation */
      else
      if (subtotal_eq)
      {
        subtotal_eq = 0;
        current_table_grade = -1;
        load_tables(table_file[0]);
      }
    }              /* math */
    if (uk_flag)
    {              /* uk */
      if (words[0] == dash)
        if (words[1] == '\0' || (words[1] == dash && words[2] == '\0'))
        {          /* dash */
          pjoin = TRUE;
          uk_flag = 2;
          goto skip_uk;
        }          /* dash */
      if (uk_flag == 2)
      {
        uk_flag = 1;
        pjoin = TRUE;
      }
    }              /* uk */
skip_uk:
    last_flag = (plinein >= eolptr && long_flag <= 0);
    if (last_flag && fillit && got_toc_page == 0 && (words[0] == currency_char || words[0] == '`'))
    {              /* last toc word */
      if (words[0] == '`')
        delete(words, 1);
      last_toc_word = TRUE;
    }              /* last toc word */
    /* process rejoined words if appropriate */
    if (hyp_dic_ptr == NULL || ((rejoin & BIT0) == 0))
      goto skip_rejoin;
    j = strlen(words);
    if (j < 2)
      goto skip_rejoin;
    hyphen_mode_save = hyphen_mode;
    if (words[j - 1] == dash)
    {              /* last character dash */
      if (last_flag || rejoin & BIT1)
      {            /* last word on line or bit 1 set */
        pointer = plinein;      /* it might have to be put back later */
        if ((ISalpha(words[j - 2]) && strpbrk(words, "*+/%()") == NULL) ||
            (trans_mode == 3 && words[0] != dash))
        {          /* proposed hyphenated word */
          strcpy(wline, words); /* save partial word */
          get_word();
          if ((ISalpha(words[0]) && ISalpha(words[1]) &&
               strchr(words, dash) == NULL) || trans_mode == 3)
          {        /* join */
            if (strlen(words) + strlen(wline) > 200)
              print_error("\7Joined word > 200 characters line %ld\n",
                          total_lines);
            strncpy_zero(wline + j - 1, words, sizeof(words) - j);
            strcpy(words, wline);
            strcpy(field, words);
            hyphen_mode = 13;
            if (trans_mode == 3)
            {      /* back translate */
              i = (strchr(lead_back_punct, field[0]) != NULL);
              translate_word(field + i, bword);
              strcpy(field, bword);
            }      /* back translate */
            i = process_rejoined_word();
            hyphen_mode = hyphen_mode_save;
            if (!i)
              goto no_rejoin;
          }        /* join */
          else
          {        /* doesn't begin with a letter so don't join */
        no_rejoin:
            if (last_flag)
              plinein = &linein[0];     /* unget the word */
            else
              plinein = pointer;
            strcpy(words, wline);       /* restore previous word */
            words[j - 1] = dash;
            words[j] = '\0';
            rjoin = TRUE;
          }        /* word doesn't begin with letter so don't join */
        }          /* proposed hyphenated word */
      }            /* last word on line or bit 1 set */
    }              /* last character dash */
    else
    if ((rejoin & BIT2) && strchr(words, dash) && trans_mode == 1)
    {              /* word has dash in middle */
      strcpy(field, words);
      remove_dashes(field, 0);
      hyphen_mode = 13;
      if (process_rejoined_word())
        remove_dashes(words, 0);
      hyphen_mode = hyphen_mode_save;
    }              /* word has dash in middle */
skip_rejoin:
    if (words[0])
    {              /* word not empty */
      total_words++;
      if (xgrade)
      {            /* grade 1-3 */
        check_ham_call(words);
        translate_word(words, bword);
      }            /* grade 1-3 */
      else
        translate_computer(words, bword);
      if (spell_dic_fileh > 0 && current_pass == 1)
        search_spell();
      if (last_flag && line_end[0])
        strcat(bword, line_end);
      build_level = 0;
      build_line();
    }              /* word not empty */
    if (keep_format && (makehead + makefoot == 0) && (last_flag || linein[0] == '\0'))
      bpurge(-10);
    if (last_flag && xformat == LINE_PARA)
    {
      flush_if_not_blank();
    }
    if (stdin_tty)
      check_keyboard(1);
  }                /* while */
  while (!done);
  if (inf_des_save != inf_des)
    close(inf_des);/* close linked file */
  inf_des = inf_des_save;
  if (ab_flag)
    return;
  if (find_toc_pages && current_pass < 2)
  {                /* end first pass with toc */
    /* look for toc entry without corresponding title */
    for (i = 0; i < MAX_TOC_ENTRY; i++)
    {
      if (toc_pages[i].braille < 0)
        print_error("\007Cannot find matching ~F%d in %s\n", i + 1, inf_name);
      if (dobook && (book_mode & 4) &&
          toc_pages[i].braille > 0 && toc_pages[i].print == 0)
        print_error("\7Ink-print page undefined for ~f%d\n", i + 1);
    }              /* i */
    if (dobook && (book_mode & 4))
      if (bpagec < pagestart_save && pagestart != 32766)
        goto delete;
    current_pass++;
    pagestart = pagestart_save;
    pageend = pageend_save;
    xformat = xformat_save;
    xgrade = grade_save;
    load_tables(table_file[0]);
    goto get_next_pass;
  }                /* end first pass with toc */
  if (bpagec < pagestart && pagestart != 32766)
  {
    if (strcmp(outf_name, prn))
    {
  delete:close(outf_des);
      unlink(outf_name);
    }
    return;
  }
  if (strcmp(bline, indent) && makefoot == FALSE)
    bpurge(-11);
  if (xfooting && blinec > 0)
  {                /* put footing on last page */
    for (i = blinec + 2; i <= linesperpage - top_margin; i++)
      if (page_in_range())
        write_string(blank_line, 1);
    write_footer();
    blinec = linesperpage;
  }                /* put footing on last page */
  if (blinec > 0)
  {                /* advance to top of next page */
    advance_page();
    /* we are at the top of bpagec+1 */
    actualpage++;
  }                /* advance to top of next page */

  if (interpoint)
  {
    if (!(actualpage & 1))
    {              /* skip interpoint even page */
      blinec = 0;  /* skip whole page */
      advance_page();
    }              /* skip interpoint even page */
    if (interpoint > 1)
      for (i = 0; i < 2; i++)
        advance_page();
  }                /* interpoint */
}                  /* do_translate */

void translate_computer(char *wordstring, char *bwordstring)
{                  /* computer braille */
  if (!grade_mod)
  {                /* grade 0 no translation */
    strcpy(bwordstring, wordstring);
    strcpy(bword6, wordstring);
    if (output_case & 2)
      strupr(bwordstring);
  }                /* grade 0 no translation */
  else
    add_case();
}                  /* translate_computer */

void translate_word(char *wordstring, char *bwordstring)
{
  vbar_fmt = cap_flag = 0;
  if (trans_mode == 3)  /* back translation */
    strupr(wordstring);
  else
    if (wordstring[1] == dash && isalpha(wordstring[0]) && xgrade && auto_letter &&
        split_word == 0 && letter_sign[0])
  {                /* insert letter sign */
    insert(string_format, wordstring);
    wordstring[0] = vbar;
    wordstring[1] = letter_sign[0];
    vbar_fmt = 1;  /* treat vbar as notrans even in formatting is disabled */
    auto_letter_count++;
  }                /* insert letter sign */
  set_vect(wordstring);
  trans_word(wordstring);
  build_word(wordstring, bwordstring);
}                  /* translate_word */

void write_stat_file(int error_mode)
{
  int i;
  if (bpagec < pagestart && (error_mode == 0))
    return;        /* no output */
  if (stat_file[0] && stat_file_ptr == NULL)
  {                /* open or append */
    if (stat_mode & BIT13)
      unlink(stat_file);
    stat_file_ptr = fopen(stat_file, "a");
  }                /* open or append */
  if (stat_file_ptr)
  {                /* open */
    time(&time2);
    time2 -= time1;
    if (stat_mode & BIT14)
      fprintf(stat_file_ptr, "NFBTRANS Version %s\n", VERSION);
    if (stat_mode & BIT0)
      fprintf(stat_file_ptr, "%s\n", date_string);
    if (stat_mode & BIT1)
    {              /* b1 */
      if (inf_name[0])
        fprintf(stat_file_ptr, "Input File: %s\n", inf_name);
      if (output_name)
        fprintf(stat_file_ptr, "Output file: %s\n", outf_name);
    }              /* b1 */
    temp[0] = init[prog_init].format;
    if (temp[0] <= '\r')
      temp[0] = ' ';
    fprintf(stat_file_ptr, "Initialization: %d %s %s %c\n", prog_init,
            init[prog_init].pre_init, init[prog_init].post_init, temp[0]);
    if (error_mode)
    {              /* error */
      fprintf(stat_file_ptr, "%s\n", iobuf);
      return;
    }              /* error */
    if (stat_mode & BIT2)
      fprintf(stat_file_ptr, "Translation time: %ld minutes %ld seconds\n", time2 / 60l, time2 % 60l);
    if (stat_mode & BIT3)
    {              /* b3 */
      fprintf(stat_file_ptr, "Input Lines: %ld\n", total_lines);
      fprintf(stat_file_ptr, "Max Line Length: %d in line %ld\n", max_input_length, max_input_line_num);
      if (total_removed_pages)
        fprintf(stat_file_ptr, "remove_page_nums = %d\n", total_removed_pages);
      if (auto_center > 1)
        fprintf(stat_file_ptr, "auto_center: %d\n", auto_center);
      fprintf(stat_file_ptr, "LinesPerPage: %d\n", linesperpage);
      if (!keep_format)
        fprintf(stat_file_ptr, "LineLength: %d\n", curmax);
    }              /* b3 */
    if ((stat_mode & BIT15) && (keep_format == 0))
      fprintf(stat_file_ptr, "Auto_blanks: %d Auto_indents: %d Auto_punct: %d\n",
              auto_blanks, auto_indents, auto_punct);
    if (stat_mode & BIT4)
    {              /* BIT4 */
      fprintf(stat_file_ptr, "Pages in file: %d\n", total_file_pages);
      if (trans_mode < 3)
      {            /* not back translation */
        fprintf(stat_file_ptr, "Pages embossed: %d\n", total_em_pages);
        fprintf(stat_file_ptr, "Sheets in file: %d\n", total_file_sheets);
        fprintf(stat_file_ptr, "Sheets embossed: %d\n", total_em_sheets);
      }            /* not back translation */
      fprintf(stat_file_ptr, "Words: %ld\n", total_words);
      if (total_misspells)
        fprintf(stat_file_ptr, "Misspells: %ld\n", total_misspells);
      if (total_file_pages)
        fprintf(stat_file_ptr, "Words per Page: %ld\n", total_words / (long) total_file_pages);
      if (time2)
      {            /* at least 1 second */
        fprintf(stat_file_ptr, "Words per Second: %ld\n", total_words / time2);
        fprintf(stat_file_ptr, "Characters per Second: %ld\n", total_cells / time2);
      }            /* at least 1 second */
      if (total_breaks)
      {
        fprintf(stat_file_ptr, "PageBreaks: %d = ", total_breaks);
        for (i = 0; i < total_breaks; i++)
          fprintf(stat_file_ptr, "%4d", page_breaks[i]);
        fprintf(stat_file_ptr, "\n");
      }
    }              /* BIT4 */
    if (current_pass == 2 && (stat_mode & BIT5))
      fprintf(stat_file_ptr, "Passes: 2\n");
    if (trans_mode != 3 && (stat_mode & BIT6))
      fprintf(stat_file_ptr, "Estimated Embossing Time: %ld Minutes\n", emboss_time);
    if (stat_mode & BIT7)
    {
      if (total_rejoins)
        fprintf(stat_file_ptr, "Rejoined words: %ld\n", total_rejoins);
      if (total_not_rejoined)
        fprintf(stat_file_ptr, "Words not rejoined: %ld\n", total_not_rejoined);
    }
    if (stat_mode & BIT8)
    {
      if (hyphen_searches)
        fprintf(stat_file_ptr, "Hyphen Searches: %d\n", hyphen_searches);
      if (dash_searches)
        fprintf(stat_file_ptr, "Dash Searches: %d\n", dash_searches);
      if (hyphen_matches)
        fprintf(stat_file_ptr, "Hyphen Matches: %d\n", hyphen_matches);
      if (hyphens_used)
        fprintf(stat_file_ptr, "Hyphens Used: %d\n", hyphens_used);
      if (dashes_used)
        fprintf(stat_file_ptr, "Dashes Used: %d\n", dashes_used);
      if (hyphens_skipped)
        fprintf(stat_file_ptr, "Hyphens Skipped: %d\n", hyphens_skipped);
    }
    if (auto_letter_count && (stat_mode & BIT9))
      fprintf(stat_file_ptr, "Auto Letter Count: %d\n", auto_letter_count);
    if (trans_mode != 3 && (stat_mode & BIT10))
    {              /* not back translate */
      if (total_equations)
        fprintf(stat_file_ptr, "equations: %d\n", total_equations);
      if (total_ham_calls)
        fprintf(stat_file_ptr, "Amateur Calls: %d\n", total_ham_calls);
      fprintf(stat_file_ptr, "Total Cells: %ld\n", total_cells);
      total = 0l;
      for (i = 0; i <= 5; i++)
        total += total_dots[i];
      fprintf(stat_file_ptr, "Total Dots: %ld\n", total);
      if (total)
        for (i = 0; i <= 5; i++)
          fprintf(stat_file_ptr, "Dot%d: %6ld = %ld%%\n", i + 1, total_dots[i],
                  total_dots[i] * 100l / total);
    }              /* not back translate */
    if (stat_mode & BIT11)
    {
      if (in_length)
        fprintf(stat_file_ptr, "Input file length: %ld\n", in_length);
      if (out_length)
        fprintf(stat_file_ptr, "Output file length: %ld\n", out_length);
      if (!ab_flag)
        if (in_length != 0l && out_length != 0l)
          fprintf(stat_file_ptr, "Output is %ld%% of input\n", 100l * out_length / in_length);
      if (ab_flag)
        fprintf(stat_file_ptr, "Aborted\n");
    }
    if (stat_mode & BIT12)
    {
      fprintf(stat_file_ptr, "Entries in table: %d\n", table_entries);
      fprintf(stat_file_ptr, "Bytes in table: %ld\n", tablebuf_offset);
    }
    fprintf(stat_file_ptr, "\n");
  }                /* open */
}                  /* write_stat_file */

void advance_page()
{
  int i;
  if (lineskips < 99)
    for (i = blinec; i < linesperpage + lineskips - top_margin; i++)
    {
      write_string(blank_line, 1);      /* do it with linefeeds */
    }
  if (lineskips == 99)
    write_char(12);/* do it with a formfeed */
  if (lineskips == 999)
    write_char(11);/* vertical tab no lf */
  if (lineskips == 9999)
  {
    write_string("\14", 0);     /* formfeed with linefeed */
    printf("\nPress Return to Continue\n");
    getch();
  }
}                  /* advance_page */

FILE *open_option_file(char *name, int open_mode)
{                  /* store file name in temp and attempt to open for reading */
  /*
   * if open_mode is 1 for writing and change file date and if = 2 then open
   * for reading
   */
SYSTEMTIME stime;
FILETIME ftime;
  char *f = field + 10;
  FILE *fptr;
  strcpy(f, name);
  if (name[0] == '/' || name[1] == ':')
    strcpy(temp, f);    /* path already specified */
  else
    sprintf(temp, "%s%s", transpath, f);
  if (open_mode < 0)
    return (NULL);
  if (open_mode > 1)
  {                /* change file date */
HANDLE fhandle = CreateFile(temp, GENERIC_WRITE, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
if (fhandle == NULL)
{
	return NULL;
}
stime.wSecond = (DWORD)time1;
SystemTimeToFileTime(&stime, &ftime);
SetFileTime(fhandle, NULL, &ftime, &ftime);
CloseHandle(fhandle);
      }                /* change file date */
  if ((fptr = fopen(temp, fopen_read[(open_mode == 1)])) == NULL)
    report_open_error(temp);
  return (fptr);
}                  /* open_option_file */

void load_tables(char *table_name)
{
  int i, j, k, error = 0, french_flag = 0, prev_type = 0;
  char match[80], prev_match[80];
  char *table_word[6];
  int lenmat, lenrep, typex;
  FILE *btable;
  char *c;
  if (current_table_grade == xgrade)
    return;        /* proper table already loaded */
  current_table_grade = xgrade;
  btable = open_option_file(table_name, 0);
  strcpy(active_table, table_name);
  table_entries = current_table_line = 0;
  memset(b, 0, sizeof(tablet)); /* fill in structure with zeros */
  tablebuf_offset = 0;
  prev_match[0] = '\0'; /* forces mismatch between match and prev_match on
                         * first line */
  while (fgets(temp, 90, btable))
  {                /* while there are lines to read */
    current_table_line++;
    trim(temp);
    if (strlen(temp) > 80)
      print_error("\7Line %d > 80 characters in %s\n", current_table_line, table_name);
    /* skip blank lines and comments starting with ; or # */
    if (temp[0] < ' ' || temp[0] == ';' || temp[0] == '#')
      continue;
    if (isalpha(temp[0]) && temp[2] == '=')
    {              /* option */
      process_options(temp, BIT2);
      continue;
    }              /* option */
    strupr(temp);
    c = strchr(temp, ' ');
    if (c)
      *c = '\0';   /* remove extra comment words after table entry */
    /* store beginning of each table word */
    j = 0;
    for (i = 0; temp[i] && j < 4; i++)
      if (temp[i] == vbar)
      {            /* vbar */
        temp[i] = '\0'; /* remove vertical bar */
        table_word[j++] = temp + i + 1;
      }            /* vbar */
    table_word[j] = NULL;
    if (table_word[0] == NULL || table_word[1] == NULL)
    {              /* error */
  no_ver_bar:error++;
      fprintf(stderr, "\7Missing | error in line %d\n",
              current_table_line);
      break;
    }              /* error */
    typex = atoi(temp);
    if (test_bracket(table_word[0], 1))
      error++;
    strcpy(match, table_word[0]);
    if (!match[0])
    {
      printf("\7NULL match string in line %d\n", current_table_line);
      error++;
      break;
    }
    lenmat = (int) strlen(match);
    k = 0;
    if (table_word[2])
    {              /* entry after replace string */
      if (trans_mode == 3 || (trans_mode < 3 && (typex < 17 || typex > 18)))
        /* allocate extra bytes */
        k = atoi(table_word[2]);
      if (trans_mode < 3 && typex >= 17 && typex <= 18)
      {            /* french */
        if (typex == 18 && table_word[3] == NULL)
          goto no_ver_bar;
        french_flag = 1, k = 100;
      }            /* french */
    }              /* entry after replace string */
    else
    if (trans_mode < 3 && typex >= 17 && typex <= 18)
      goto no_ver_bar;
    if (!table_word[1][0])
    {
      fprintf(stderr, "\7NULL replace string in line %d\n", current_table_line);
      error++;
    }
    if (test_bracket(table_word[1], 2))
      error++;
    if (error)
      break;
    lenrep = (int) strlen(table_word[1]);
    if (table_entries + 1 >= MAXTAB)
    {              /* too many entries */
      fprintf(stderr, "\7More than %d entries in table not allowed\n", MAXTAB);
      error++;
      break;
    }              /* too many entries */
    if (typex <= -32 || typex >= 64)
    {              /* bad type */
  bad_type:
      fprintf(stderr, "\7Invalid type %d in line %d\n", typex, current_table_line);
      error++;
      break;
    }              /* type error */
    if (xgrade == 3)
    {              /* grade 3 in effect */
      if (grade_mod && typex < 0)
        continue;  /* skip unwanted g2 entry */
      typex = abs(typex);
      if (typex > 32)
      {            /* grade 3 type */
        i = strlen(table_word[1]);
        if (i == 1 && typex == 34 && (grade_mod & BIT4))
          goto keep_entry;
        if (strchr("@\042^_", table_word[1][0]))
        {          /* dot 4 5 45 or 456 prefix */
          if (!(grade_mod & 15))
            continue;
          if (table_word[1][1] >= 'A' && table_word[1][1] <= 'Z' &&
              (grade_mod & BIT0))
            goto keep_entry;    /* followed by letter */
          if ((grade_mod & BIT1) && Isdigit(table_word[1][1]))
            goto keep_entry;    /* followed by digit */
          if ((grade_mod & BIT2) && strchr(g3_derivatives, table_word[1][1]) && i == 2)
            goto keep_entry;    /* followed by derivative */
          if ((grade_mod & BIT3) && i == 1 && typex == 42)
            goto keep_entry;
          continue;
        }          /* dot 4 5 45 456 prefix */
        else
        if (grade_mod & BIT5)
          goto keep_entry;
        continue;
    keep_entry:
        typex &= 31;
      }            /* grade 3 type */
    }              /* grade 3 in effect */
    else
    {              /* not grade 3 */
      typex = abs(typex);
      if (typex > 32)
        continue;  /* skip g3 entry */
    }              /* not grade 3 */
    if (typex > 30)
      goto bad_type;
    if (trans_mode == 3)
      if (typex == 5 || typex == 6 || typex == 8 || typex == 9 || typex == 11 ||
          typex == 12 || typex == 15 || typex == 16 || typex > 19)
        goto bad_type;
    if (table_entries)  /* store previous match in prev_match */
      strcpy(prev_match, b->match[table_entries]);
    table_entries++;    /* we found a valid line, store results */
    b->typex[table_entries] = typex;
    b->match[table_entries] = tablebuf + tablebuf_offset;
    tablebuf_offset += 1l + (long) lenmat;
    if (tablebuf_offset > (long) MAX_TABLE_BUF)
  table_byte_error:
      print_error("\7More than %ld bytes in %s line %ld\n", (long) MAX_TABLE_BUF,
                  table_name, current_table_line);
    strcpy(b->match[table_entries], match);
    b->replace[table_entries] = c = tablebuf + tablebuf_offset;
    if (k > lenrep)
      lenrep = k;
    tablebuf_offset += 1l + (long) lenrep;
    if (tablebuf_offset > (long) MAX_TABLE_BUF - 2l)
      goto table_byte_error;
    if (!french_flag)
      strcpy(c, table_word[1]);
    else
    {              /* 2 or 3 word match */
      french_flag = 0;
      for (i = 17; i <= typex + 1; i++)
      {
        if (strlen(table_word[i - 16]) > 25)
          print_error("\7Type %d entry > 25 characters in  line %d\n", typex,
                      current_table_line);
        strcpy(c, table_word[i - 16]);
        c += 30;
      }            /* i */
    }              /* 2 or 3 word match */
    j = (int) (match[0] - '@');
    if (j < 0 || j > 27)
      j = 0;
    if (strcmp(prev_match, match) >= 0)
      if (strncmp(prev_match, match, lenmat))
      {
        fprintf(stderr, "\007Sorting error in line %d\n", current_table_line);
        error++;
        break;
      }
    if (match[0] != prev_match[0])
    {              /* first characters differ */
      if (match[0] >= 'A' && match[0] <= 'Z')
      {            /* letter */
        if (lenmat != 1 && trans_mode < 3)
        {          /* error */
          fprintf(stderr, "\007Match should be a single character in line %d\n"
                  ,current_table_line);
          error++;
          break;
        }          /* error */
        for (i = 0; i < 27; i++)
          b->start2[j][i] = table_entries;
      }            /* letter */
      b->start1[(int) (unsigned char) (match[0])] = table_entries;
    }              /* first characters differ */
    else
    {              /* first characters same */
      if ((match[1] != prev_match[1]))
      {            /* second element of match and previous match differ */
        k = (int) (match[1] - '@');
        if (k > 0 && k < 27)
          b->start2[j][k] = table_entries;
      }            /* second elements differ */
      else
      if (prev_type > 16 && prev_type != 30 && typex < 17 && trans_mode == 1)
      {
        fprintf(stderr, "\7Type %d entry cannot match first word of type %d entry line %d\n",
                typex, prev_type, current_table_line);
        error++;
      }
    }              /* first character same */
    prev_type = typex;
  }                /* while */
  fclose(btable);
  b->match[table_entries + 1] = c = tablebuf + tablebuf_offset++;
  c[0] = '\0';
  end_table_ptr = tablebuf + tablebuf_offset;
  if (error)
    print_error("Correct error in %s and restart program.\n", table_name);
}                  /* load_tables */

int test_bracket(char *c, int x)
{
  char *bracket;
  do
  {
    bracket = strchr(c, '[');
    if (bracket && bracket[1] >= '0' && bracket[1] <= '9')
    {              /* digit */
      if (!strchr(bracket, ']'))
      {            /* no ] */
        if (x > 1)
          break;
        fprintf(stderr, "\7Unmatched bracket in line %d\n",
                current_table_line);
        return (1);
      }            /* no ] */
      bracket[0] = (unsigned char) get_integer(bracket + 1);
      delete(bracket + 1, 1);
    }              /* digit */
    else
      bracket = NULL;
  }
  while (bracket);
  return (0);
}                  /* test_bracket */

int search_hyphen_dictionary(char *string)
{                  /* returns length of word if in dictionary otherwise 0 */
  int k, l, index, string_len, leading = 0;
  char *cptr, *h, lead[4];
  char hword[MAX_DIC_LEN + 1], prev = '@';
  dash_flag = 0;
  if (hyphen_mode & BIT1)
  {                /* test for dash */
    cptr = strchr(string, dash);
    if (cptr)
    {              /* found dash */
      cptr--;
      if (isalpha(cptr[0]) && isalpha(cptr[2]))
      {            /* - between letters */
        string[MAX_DIC_LEN] = '\0';
        strcpy(hyphen_dic_line, string);
        dash_searches++;
        hyphen_line_count = 0;
        string_len = strlen(string);
        if (string[string_len - 1] == dash)
          string_len--;
        dash_flag = 1;
        return (string_len);
      }            /* - between letters */
    }              /* found dash */
  }                /* test for dash */
  if (((hyphen_mode & BIT0) == 0) || hyp_dic_ptr == NULL)
    return (0);    /* not active */
  if (hyphen_mode & BIT2)
  {                /* look for leading punctuation */
    lead[0] = lead[1] = '\0';
    if (strchr(leading_punct, string[0]))
    {
      leading = 1;
      lead[0] = string[0];
    }
  }                /* look for leading punctuation */
  hyphen_searches++;    /* for statistics file */
  index = (int) (unsigned char) string[leading] - 65;
  if (index < 0)
    return (0);    /* search word doesn't begin with a letter */
  if (hyphen_mode & BIT3)
    remove_trailing_punct(string);
  /*
   * seek to where word with this letter starts otherwise where previous
   * letter starts
   */
  while (index > 0 && hyphen_dic_start[index].pos == 0l)
    index--;
  fseek(hyp_dic_ptr, hyphen_dic_start[index].pos, 0);
  hyphen_line_count = hyphen_dic_start[index].line;
  while (fgets(hyphen_dic_line, MAX_DIC_LEN, hyp_dic_ptr))
  {
    l = strlen(hyphen_dic_line);
    hyphen_dic_line[l - 1] = 0; /* remove lf */
    hyphen_line_count++;
    if (prev != hyphen_dic_line[0])
    {              /* first word starting with this letter */
      index = (int) (unsigned char) hyphen_dic_line[0] - 65;
      if (index < 0)
      {            /* bad index */
        if (prev == '@' && isdigit(hyphen_dic_line[0]))
        {          /* remove date */
          cptr = strchr(hyphen_dic_line, 'A');
          if (!cptr)
            goto word_error;
          strcpy(hyphen_dic_line, cptr);
          index = 0;    /* for the letter A */
          hyphen_dic_start[0].pos = (long) (cptr - hyphen_dic_line);
        }          /* remove date */

        if ((unsigned char) hyphen_dic_line[0] < (unsigned char) prev)
          print_error("\7Search sorting error in line %d\n", hyphen_line_count);
        if (index)
        {          /* error */
      word_error:print_error(
                      "\7Line %d in %s must begin with letters or extended \ngraphics characters\n",
                      hyphen_line_count, hyphen_dic_name, hyphen_dic_line);
        }          /* error */
      }            /* bad index */
      if (!hyphen_dic_start[index].pos)
      {            /* store word position */
        hyphen_dic_start[index].pos = ftell(hyp_dic_ptr) - (long) (l + 1);
#ifdef unix
        hyphen_dic_start[index].pos++;  /* no cr in unix */
#endif
        hyphen_dic_start[index].line = hyphen_line_count - 1;
      }            /* store word position */
      prev = hyphen_dic_line[0];
    }              /* first word starting with this letter */
    /* build hword by removing all dashes & | from the line */
    k = 0;
    h = hyphen_dic_line;
    for (; *h; h++)
      if (*h != dash && *h != vbar)
        hword[k++] = *h;
    hword[k] = 0;
    k = strcmp(hword, string + leading);
    if (k < 0)
      continue;
    if (hyphen_mode & BIT3)
    {
      strcat(hyphen_dic_line, temp);    /* restore trailing punctuation */
      strcat(string, temp);
    }
    if (hyphen_mode & BIT2)
      insert(lead, hyphen_dic_line);
    if (k > 0)
      return (FALSE);   /* word not found */
    l = strlen(hyphen_dic_line);
    return (l - 2);
  }                /* while */
  return (FALSE);
}                  /* search_hyphen_dictionary */

int process_rejoined_word()
{
  int i;
  strupr(field);
  i = search_hyphen_dictionary(field);
  hyphen_searches--;
  if (i)
  {                /* word in dictionary */
    total_rejoins++;
    if (rejoin & BIT3)
      fprintf(rejoin_out_ptr, "%s rejoined input line %ld output page %d\n",
              field, total_lines, bpagec);
  }                /* word in dictionary */
  else
  {                /* word not in dictionary */
    total_not_rejoined++;
    if (rejoin & BIT4)
      fprintf(rejoin_out_ptr, "%s not rejoined input line %ld output page %d\n",
              field, total_lines, bpagec);
  }                /* word not in dictionary */
  return (i);
}                  /* process_rejoined_words */

void insert_hyphen_word(int date_only)
{                  /* temp contains word with dashes to insert.
                    * hyphen_dic_line contains word with dashes removed */
  /* current date is always placed on first line */
  int i, j = 0, l, lines = 0, flag = 0;
  char *cptr, *temp_hyp_file = field + 100;
  FILE *tempfile;
  /* create a temp file name and open for writing */
  strcpy(temp_hyp_file, hyphen_dic_name);
  for (i = 0; temp_hyp_file[i]; i++)
    if (temp_hyp_file[i] == '\\' || temp_hyp_file[i] == ':' ||
        temp_hyp_file[i] == '/')
      j = i + 1;
  strcpy(temp_hyp_file + j, "tmphyp.$$$");
  sprintf(end_table_ptr, "%s%s", transpath, temp_hyp_file);
  tempfile = fopen(end_table_ptr, "w");
  if (!tempfile)
    print_error("\7Cannot open %s hyphen dictionary\n", end_table_ptr);
  get_date(1);
  rewind(hyp_dic_ptr);
  if (!date_only)
  {
    hyp_words_added++;
    fprintf(stderr, "Inserting %s at line ", hyphen_dic_line);
    /* actual line number will be determined later in routine */
    l = (int) (unsigned char) hyphen_dic_line[0] - 64;
    j = strlen(temp) + 2;
#ifdef unix
    j--;           /* cr not present in unix */
#endif
/*add to start of each subsequent letter and increment line numbers*/
    for (i = l; i < 191; i++)
      if (hyphen_dic_start[i].pos)
      {            /* add */
        hyphen_dic_start[i].pos += (long) j;
        hyphen_dic_start[i].line++;
      }            /* add */
  }
  while (fgets(field, 80, hyp_dic_ptr))
  {
    l = strlen(field);
    if (field[l - 2] == '\r')
    {              /* remove cr in unix */
      if (!lines)
        fprintf(stderr, "Removing carriage returns\n");
      l--;
    }              /* remove cr in unix */
    field[l - 1] = 0;   /* remove lf */
    if (!lines)
    {              /* first line */
      fprintf(tempfile, date_string);
      cptr = strchr(field, 'A');
      if (cptr)
        strcpy(field, cptr);    /* get rid of date */
      flag = date_only;
    }              /* first line */
    lines++;
    if (!date_only)
    {
      strcpy(field + 50, field);
      remove_dashes(field + 50, 1);
      /* compare words with dashes removed to insert in proper place */
      if (strcmp(field + 50, hyphen_dic_line) > 0 && flag == 0)
      {            /* insert */
        flag++;    /* only insert one line */
        insert_report(lines, tempfile);
      }            /* insert */
    }
    if (fprintf(tempfile, "%s\n", field) < 0)
      no_space();
  }                /* while */
  if (!flag)       /* inserted at eof */
    insert_report(lines + 1, tempfile);
  fclose(tempfile);
  fclose(hyp_dic_ptr);
  sprintf(end_table_ptr + 1024, "%s%s", transpath, hyphen_dic_name);
  unlink(end_table_ptr + 1024);
  rename(end_table_ptr, end_table_ptr + 1024);
  hyp_dic_ptr = NULL;
  /* change date on file because it was modified */
  open_hyp_dictionary(hyphen_dic_name, ((hyp_words_added % hyp_int) == 0) * 2, 2);
}                  /* insert_hyphen_word */

void insert_dictionary_word()
{
  for (;;)
  {
    fprintf(stderr, "Enter dictionary word <RETURN> to skip: ");
    get_input(temp, 40);
    if (!temp[0])
      break;       /* skip, don't store as rejected */
    strupr(temp);
    if (!strchr(temp, dash))
    {
      fprintf(stderr, "\7Word must contain at least one hyphen\n");
      continue;
    }
    if (temp[0] < 'A' && temp[0] >= '\0')
    {
      fprintf(stderr, "\7Word cannot begin with %c\n", temp[0]);
      continue;
    }
    temp[MAX_DIC_LEN] = 0;
    strcpy(hyphen_dic_line, temp);
    remove_dashes(hyphen_dic_line, 1);
    if (strcmp(field, hyphen_dic_line))
    {              /* bad match */
      fprintf(stderr, "\7Word must match %s\n", field);
      continue;
    }              /* bad match */
    insert_hyphen_word(0);
    break;
  }                /* for */
}                  /* insert_dictionary_word */

void get_yn()
{
  for (;;)
  {
    get_digit();
    temp[0] = (char) tolower(temp[0]);
    if (temp[0] == 'y' || temp[0] == 'n')
      break;
#ifndef unix
    sound(1000);
    delay(50);
    nosound();
#endif
  }
  fprintf(stderr, "\n");
}                  /* get_yn */

void insert_report(int lines, FILE * tempfile)
{
  if (fprintf(tempfile, "%s\n", temp) < 0)
    no_space();
  fprintf(stderr, "%d\n", lines);
}                  /* insert_report */

void read_menu()
{
  int i, j, l, line_flag, space_flag;
  for (i = 0; i < 12; i++)
  {
    l = strlen(main_menu[i]) - 1;
    if (main_menu[i][l] == '\n')
      line_flag = 1;    /* original item ends with lf */
    else
      line_flag = 0;
    if (main_menu[i][l] == ' ')
      space_flag = 1;   /* add space to new menu item */
    else
      space_flag = 0;
    if (!fgets(temp, 80, intext))
      break;
    l = strlen(temp) - 1;
    j = menu[i + 1] - menu[i];
    if (l + 1 >= j)
      print_error("\7Menu item > %d characters in line %d\n", j, i + 1);
    if (!line_flag)
      temp[l] = '\0';   /* remove lf */
    if (space_flag)
      strcat(temp, " ");
    for (l = 0; temp[l]; l++)
      if (temp[l] == '\\')
        temp[l] = '\n';
    strcpy(menu[i], temp);
  }                /* i */
  fclose(intext);
  intext = NULL;
}                  /* read_menu */

void remove_trailing_punct(char *string)
{                  /* test for punctuation */
  int i, string_len;
  char *cptr;
  string_len = (int) (strlen(string) - 1);
  temp[0] = '\0';
  cptr = strpbrk(string + 1, trailing_punct);
  if (cptr)
  {                /* has trailing punctuation */
    for (i = 1; cptr[i]; i++)
      if (isalpha(cptr[i]) || isdigit(cptr[i]))
        return;
    strncpy_zero(temp, cptr, 5);        /* so hyphen_dic_line won't overflow */
    *cptr = '\0';
  }                /* has trailing punctuation */
}                  /* remove_trailing_punct */

void test_hyphen_dictionary(int mode)
{
  int i, l, date_found = 0;
  char *aptr;
  unsigned k, total = 0, dash_line = 0, long_line = 0, max_len = 0, max_dash = 0;
  char hword[MAX_DIC_LEN + 1], prevword[MAX_DIC_LEN + 1];
  if (mode)
    fprintf(stderr, "Checking consistency of %s Please Wait...\n", hyphen_dic_name);
  hyp_dic_tested = TRUE;
  prevword[0] = 0;
  rewind(hyp_dic_ptr);
  get_date(1);     /* get date from clock store in time1 */
  while (fgets(temp, 80, hyp_dic_ptr))
  {
    if (total == 0 && isdigit(temp[0]) > 0)
    {              /* test for date on first line */
      aptr = strchr(temp, 'A'); /* dictionary must have a word beginning with
                                 * A */
      if (!aptr)
        aptr = temp;
      date_found = (int) (aptr - temp);
      strcpy(temp, aptr);       /* remove date from line */
    }              /* test for date on first line */

    total++;
/*test for uppercase*/
    strcpy(field, temp);
    strupr(field);
    if (strcmp(field, temp))
      print_error("\7Line %u must be uppercase\n", total);
    if (!strchr(temp, dash))
      print_error("\7- not found in line %u\n", total);
    k = strlen(temp);
    if (temp[k - 2] == dash)
      print_error("\7Word ends with - line %u\n", total);
    if (k > max_len)
    {
      max_len = k;
      long_line = total;
      if (k > MAX_DIC_LEN)
        print_error("\7Word > %d characters in line %u\n", MAX_DIC_LEN, total);
    }
    /* build hword by removing all dashes from temp */
    k = l = 0;
    for (i = 0; temp[i]; i++)
      if (temp[i] != dash && temp[i] != vbar)
        hword[l++] = temp[i];
      else
        k++;
    if (k > max_dash)
    {
      max_dash = k;
      dash_line = total;
    }
    hword[l] = 0;
    l = strcmp(hword, prevword);
    if (l < 0)
      print_error("\7Sorting error in line %u\n", total);
    if (!l)
      print_error("\7Duplicate entry in line %u\n", total);
    strcpy(prevword, hword);
  }                /* while */
  if (!mode)
  {
    fprintf(stderr, "Longest word is %u characters in line %u\n", max_len, long_line);
    fprintf(stderr, "Maximum hyphens %u in line %u\n", max_dash, dash_line);
  }
  if (date_found >= 17)
  {                /* date found on first line */
    /* open for r+ */
    open_hyp_dictionary(hyphen_dic_name, 3, 1);
    rewind(hyp_dic_ptr);
    fprintf(hyp_dic_ptr, date_string);
    open_hyp_dictionary(hyphen_dic_name, 3, 2);
  }                /* date found on first line */
  else
    insert_hyphen_word(1);
}                  /* test_hyphen_dictionary */

void get_config()
{
  int i;
  char *c, *cptr;
  if (!total_files)
  {                /* first time called */
    if (paramcount > 0)
    {              /* test for cf= on command line */
      strcpy(temp, paramstr[1]);
      strupr(temp);
      if (!strncmp(temp, "CF=", 3))
      {            /* new config file */
        strcpy(config_file, paramstr[1] + 3);
        start_arg++;
      }            /* new config file on command line */
    }              /* test for cf= */
    /*
     * use nfbtrans environment variable, current directory, and then program
     * or unix path
     */
    c = (char *) getenv("NFBTRANS");
    if (c)
    {              /* nfbtrans defined */
      strcpy(transpath, c);
#ifdef DOS
      strcat(transpath, "\\");
#else
      strcat(transpath, "/");
#endif
    }              /* nfbtrans defined */
    if (!open_config_file(0))
    {              /* not in environment */
      transpath[0] = '\0';
      if (!open_config_file(0))
      {            /* not in environment or current directory */
#ifdef DOS
        strcpy(transpath, paramstr[0]);
        c = strrchr(transpath, '\\');
        if (c)
          c[1] = 0;/* we have program path */
        else
          transpath[0] = '\0';
#else
        strcpy(transpath, UNIX_PATH);
#endif
        open_config_file(1);
      }            /* not in current dir or environment */
    }              /* not in environment */
  }                /* first time called */
  else
    rewind(conf_ptr);
  config_lines = 0l;
  while (fgets(temp, 150, conf_ptr))
  {                /* while there are lines to read */
    config_lines++;
    if (strlen(temp) > 145)
      print_error("\7Line more than 145 characters in %s%s\n", transpath,
                  config_file);
    trim(temp);
    if (temp[0] == '\0' || temp[0] == ';' || temp[0] == '#')
      continue;    /* skip */
    cptr = strchr(temp, ' ');
    if (cptr)
      if (!strchr(quotes, temp[3]))
        *cptr = 0; /* allows comments starting with second word */
    process_options(temp, BIT1);
  }                /* while */
  config_lines = 0l;
  if (!total_files)
    /* process arguments on command line */
    for (i = start_arg; i <= paramcount; i++)
    {
      if (paramstr[i][2] == '=')
      {            /* xx= */
        strcpy(temp, paramstr[i]);
        process_options(temp, BIT0);
      }            /* xx= */
      else
      {            /* not xx= */
        usr_default = TRUE;
        /*
         * if input is from keyboard assume arguments are input files. Else
         * ignore
         */
        if (stdin_tty)
          input_file_arg = i;
        break;
      }            /* not xx= */
    }              /* i */
  if (!stdin_tty)
  {
    inf_des = 0;
    usr_default = TRUE;
    input_file_arg = paramcount;
  }
  if (!stdout_tty)
  {
    outf_des = 1;
#ifdef DOS
    setmode(1, O_BINARY);
#endif
    if (trans_mode == -1)
      trans_mode = trans_mode1 = 1;
  }
  if (usr_default)
  {                /* set defaults */
    display_source = FALSE;
    lastcopy = 1;
    if (pagestart <= 0)
      pagestart = 1;
    if (pageend < pagestart)
      pageend = 9999;   /* print entire document */
    if (leftmargin < 1)
      leftmargin = 1;
    if (maxline <= 1)
      maxline = 40;
    if (linesperpage <= 0)
      linesperpage = 25;
    if (lineskips < 0)
      lineskips = 99;
    if (trans_mode <= 0)
      trans_mode = 1, trans_mode1 = 2;
    display_braille = 0;
  }                /* set defaults */
}                  /* get_config */

FILE *open_config_file(int mode)
{
  sprintf(temp, "%s%s", transpath, config_file);
  conf_ptr = fopen(temp, fopen_read[0]);
  if (mode && conf_ptr == NULL)
    print_error("\007%s not found\n", config_file);
  return (conf_ptr);
}                  /* open_config_file */

void process_options(char *string, int option_mode)
{
  int j, x, y, z;
  char *cptr = string + 3, *vptr, delim, ch, *vptr1;
  string[0] = (char) toupper(string[0]);
  string[1] = (char) toupper(string[1]);
  for (j = 0; options[j].name; j++)
  {
    if (!strncmp(string, options[j].name, 2))
    {              /* option matched */
      z = options[j].value;
      if (*cptr == '\0' && z < BIT9 && (option_mode & BIT0) == 0)
        print_error("\7Improper option format: %s nothing follows =.\n", string);
      if (z & BIT8)
        return;    /* ignore this option */
      if (!(option_mode & z))   /* not allowed with this mode */
        report_option_error(option_mode, j);
      if ((z & 576) == 0)
      {            /* string option not char or int */
        vptr = NULL;
        if (*cptr)
          vptr = strchr(quotes, cptr[0]);
        if (vptr)
        {          /* quoted string */
          delim = *cptr;
          delete(cptr, 1);      /* remove first quote */
          vptr = cptr;
          vptr1 = NULL;
          while (*vptr)
          {
            if (*vptr == delim)
              vptr1 = vptr;
            vptr++;
          }
          if (vptr1 == NULL)
          {        /* unmatched */
            if (config_lines)
              total_lines = config_lines;
            print_error("\007Unmatched quoted string in line %ld",
                        total_lines);
          }        /* unmatched */
          vptr1[0] = 0;
        }          /* quoted string */
      }            /* string not char or int */
      else
      {            /* int or char */
        if (z & BIT6)
        {          /* get integer */
          if (isalpha(cptr[0]))
            print_error("\7Option %s requires integer argument\n", string);
          if (strchr(cptr, ','))
          {        /* set bits */
            x = 0;
            while (*cptr)
            {
              y = get_integer(cptr);
              x |= (1 << y);
              if (cptr[0] == ',')
                cptr++;
            }      /* while */
          }        /* set bits */
          else
            x = get_integer(cptr);      /* saves multiple calls to
                                         * get_integer */
          y = abs(x);
        }          /* get integer */
        else
        if (!cptr[0])
          *cptr = ' ';
      }            /* int or char */
      if (option_file)
        fprintf(option_file, "%s %d %04X\n", string, x, z);
      switch (j)
      {
      case 0:     /* ac */
        auto_center = x;
        if (cptr)
          strncpy(auto_center_format, cptr, sizeof(auto_center_format));
        break;
      case 1:     /* be */
#ifdef DOS
        sound(x);
        x = 0;
        if (cptr[0])
          x = get_integer(cptr + 1);
        if (!x)
          x = 500;
        delay(x);
        nosound();
#endif
        break;
      case 2:     /* bl */
        strncpy(blank_line, cptr, 7);
        break;
      case 3:     /* bm */
        book_mode = x;
        if (book_mode & 8)
          book_mode |= 4;       /* make sure book TOC bit is set */
        break;
      case 4:     /* bp */
        book_break = y;
        break;
      case 5:     /* ca */
        vptr = strchr(cptr, vbar);
        if (!vptr)
          goto no_ver_bar;
        *vptr = '\0';
        strncpy_zero(cap_single, cptr, sizeof(cap_single));
        cptr = strchr(vptr + 1, vbar);
        if (cptr)
        {
          *cptr = '\0';
          strncpy_zero(letter_sign, cptr + 1, sizeof(letter_sign));
        }
        strncpy_zero(cap_all, vptr + 1, sizeof(cap_all));
        break;
      case 6:     /* cl */
        center_length = min(y, 43);
        break;
      case 7:     /* co */
        lastcopy = y;
        break;
      case 8:     /* cs */
        charspersec = y;
        break;
      case 9:     /* cu */
        currency_char = *cptr;
        break;
      case 10:    /* db */
        display_braille = y;
        break;
      case 11:    /* de */
        emboss_delay = y;
        break;
      case 12:    /* dm */
        fprintf(stderr, "%s\n", cptr);
        break;
      case 13:    /* ds */
        display_source = y;
        break;
      case 14:    /* dv */
        divide[0] = y;
        divide[1] = get_integer(cptr + 1);
        if (divide[0] >= linesperpage
            || divide[1] > linesperpage)
          print_error("\7DV= option out of range\n");
        break;
      case 15:    /* ef */
        efl_mode = x;
        break;
      case 16:    /* em */
        if (trans_mode != x)
          print_error("\7Improper translation mode for %s\n", active_table);
        break;
      case 17:    /* et */
        expand_tab = (y > 0);
        break;
      case 18:    /* ex */
        if (total_files)
          break;   /* only get once from nfbtrans.cnf */
        for (;;)
        {
          if (prog_extension.total >= MAX_EXTENSIONS)
            print_error(
              "\007Too many extensions.  only the first %d can be stored\n",
                        MAX_EXTENSIONS);
          vptr = strchr(cptr, '=');
          if (vptr == NULL)
            break; /* no more extensions */
          *vptr = 0;    /* terminate extension */
          if (strlen(cptr) > 15)
            goto invalid;
          strcpy(prog_extension.prog_ext[prog_extension.total].ext, cptr);
          *vptr = ' ';  /* in case theres an error */
          x = (int) ((char) toupper(vptr[1]) - 48);
          if (vptr[1] == dash)
            vptr[1] = '0';
          if ((x < 0 && x != -3) || x > 21)
            goto invalid;
          prog_extension.prog_ext[prog_extension.total].init_val = x;
          cptr = vptr + 2;
          prog_extension.total++;
        }
        break;
      case 19:    /* fc */
        fill_char = *cptr;
        break;
      case 20:    /* fp */
        first_page = y;
        break;
      case 21:    /* fs */
        format_char = *cptr;
        if (format_char == ' ')
          format_char = '\r';
        break;
      case 22:    /* gd */
        guide_dots = min(y, 80);
        break;
      case 23:    /* gm */
        graphics_mode = x;
        break;
      case 24:    /* hb */
        strncpy_zero(bad_hyp_fname, cptr, sizeof(bad_hyp_fname));
        break;
      case 25:    /* hc */
        ham_call = x;
        break;
      case 26:    /* hd */
        open_hyp_dictionary(cptr, 1, 0);
        break;
      case 27:    /* hk */
        hot_key = y;
        break;
      case 28:    /* hl */
        min_hyp_len = min(y, 4);
        break;
      case 29:    /* hm */
        hyphen_mode = x;
        if (dic_out_ptr != NULL && dic_out_ptr != stderr)
          fclose(dic_out_ptr);
        dic_out_ptr = NULL;
        if (*cptr)
          dic_out_ptr = fopen(cptr, "a");
        if (!dic_out_ptr)
          dic_out_ptr = stderr;
        break;
      case 30:    /* hn */
        hyp_int = max(y, 1);
        break;
      case 31:    /* hp */
        max_hyp_page = y;
        break;
      case 32:    /* ht */
        open_hyp_dictionary(cptr, 1, 0);
        memset(hyphen_dic_start, 0, sizeof(hyphen_dic_start));
        test_hyphen_dictionary(0);
        hyphen_mode = 1;
        for (;;)
        {
          fprintf(stderr, "enter word <RETURN> to exit: ");
          get_input(field, 40);
          if (strlen(field) < 2)
            break;
          strupr(field);
          if (search_hyphen_dictionary(field))
            fprintf(stderr, "%s was found on line %u\n", field, hyphen_line_count);
          else
          {        /* not found */
            fprintf(stderr, "\7%s not found, belongs on line %u\n", field, hyphen_line_count);
            fprintf(stderr, "Insert %s in dictionary (Y/N)? ", field);
            get_yn();
            if (temp[0] == 'y')
              insert_dictionary_word();
          }        /* not found */
        }
        exit_program(0);
        break;
      case 33:    /* hx */
        max_consec_hyphens = y;
        break;
      case 34:    /* i0 */
      case 35:    /* i1 */
      case 36:    /* i2 */
      case 37:    /* i3 */
      case 38:    /* i4 */
      case 39:    /* i5 */
      case 40:    /* i6 */
      case 41:    /* i7 */
      case 42:    /* i8 */
      case 43:    /* i9 */
      case 44:    /* i: */
      case 45:    /* i; */
      case 46:    /* i< */
      case 47:    /* i= */
      case 48:    /* i> */
      case 49:    /* i@ */
      case 50:    /* ia */
      case 51:    /* ib */
      case 52:    /* ic */
      case 53:    /* id */
      case 54:    /* ie */
        options[j].value = 256; /* ignore from now on */
        x = (int) (string[1] - 48);
        vptr = strchr(cptr, vbar);
        if (!vptr)
      no_ver_bar:print_error("\007Missing | in %s\n", string);
        *vptr = 0;
        if (strlen(cptr) > MAX_INIT)
        {
      init_err:
          print_error("\7Initialization string > %d characters %s\n",
                      MAX_INIT, string);
        }
        copy_string(init[x].pre_init, cptr, MAX_INIT);
        cptr = vptr + 1;
        vptr = strchr(cptr, vbar);
        if (!vptr)
          goto no_ver_bar;
        *vptr = 0;
        if (strlen(cptr) > MAX_INIT)
          goto init_err;
        copy_string(init[x].post_init, cptr, MAX_INIT);
        init[x].format = vptr[1];
        if (!init[x].format)
          init[x].format = '\r';
        break;
      case 55:    /* IF */
        strncpy_zero(ignore_format, cptr, sizeof(ignore_format));
        break;
      case 56:    /* interpoint */
        interpoint = y;
        break;
      case 57:    /* it */
        italics[0] = *cptr;
        break;
      case 58:    /* jf */
        break;
      case 59:    /* kc */
        keep_control = y;
        break;
      case 60:    /* kf */
        keep_format = y;
        if (keep_format)
        {
          hyphen_mode = total_removed_pages = remove_page_nums = rejoin = 0;
          maxline = curmax = 99;
          dopagenum = FALSE;
          indent[0] = '\0';
        }
        break;
      case 61:    /* l0 */
        strncpy_zero(l0, cptr, sizeof(l0));
        break;
      case 62:    /* l1 */
        strncpy_zero(l1, cptr, sizeof(l1));
        break;
      case 63:    /* l2 */
        j = 0;
        do
        {
          vptr = strchr(cptr, vbar);
          if (vptr)
            *vptr = '\0';
          strcpy(l2[j], cptr);
          cptr = vptr + 1;
          j++;
        }
        while (vptr != NULL && j < 9);
        l2[j] = '\0';
        break;
      case 64:    /* l3 */
        spanish_flag = x;
        break;
      case 65:    /* lb */
        list_break = x;
        break;
      case 66:    /* le */
        if (*cptr)
        {          /* not empty */
          strncpy_zero(efl_file, cptr, sizeof(efl_file));
          j = efl_mode;
          efl_mode |= 1;
          efl_mode &= 5;
          load_template();
          if (!lfile)
          {        /* try program directory */
            efl_mode |= 2;      /* abort if not found */
            sprintf(efl_file, "%s%s", transpath, cptr);
            load_template();
          }        /* try program directory */
          efl_mode = j;
        }          /* not empty */
        else
          lopactive = FALSE;
        break;
      case 67:    /* lf */
        if (inf_des_save != inf_des)
          close(inf_des);
        strcpy(inf_name + inf_path_len, cptr);
        inf_des = open_input_file();
        bytes_in_buf = long_flag = max_input_length = 0;
        linein[0] = 0;
        break;
      case 68:    /* li */
        strncpy_zero(line_end, cptr, sizeof(line_end));
        break;
      case 69:    /* lm */
        leftmargin = min(y, 43);
        break;
      case 70:    /* lp */
        strncpy_zero(leading_punct, cptr, sizeof(leading_punct));
        break;
      case 71:    /* lr */
        auto_letter = x;
        break;
      case 72:    /* ls */
        page_sep = '\14';
        switch (toupper(cptr[0]))
        {
        case 'F':
          lineskips = 99;
          break;
        case 'V':
          lineskips = 100;
          page_sep = '\13';
          break;
        case 'P':
          lineskips = 101;
          break;
        default:
          lineskips = get_integer(cptr);
          if (lineskips < 0 || (lineskips > 10 && lineskips < 99))
            lineskips = 0;
        }          /* switch */
        break;
      case 73:    /* lt */
        eol_term = min(y, 3);
        break;
      case 74:    /* m3 */
        default_g3_mod = x;
        break;
      case 75:    /* ma */
        math_flag = 0;
        if (!Isdigit(cptr[0]) && trans_mode < 3)
        {          /* not a digit */
          current_table_grade = 0;
          if (*cptr == '+')
            load_tables(math_table);
          else
            load_tables(table_file[0]);
        }          /* not a digit */
        else
          math_flag = x;
        break;
      case 76:    /* mf */
        options[j].value = 256; /* ignore from now on */
        intext = open_option_file(cptr, 0);
        read_menu();
        break;
      case 77:    /* ms */
        strncpy_zero(math_symbols, cptr, sizeof(math_symbols));
        break;
      case 78:    /* mt */
        strncpy_zero(math_table, cptr, sizeof(math_table));
        break;
      case 79:    /* nc */
        no_copyright = y;
        break;
      case 80:    /* ns */
        numeric_def[0] = *cptr;
        numeric_def[2] = cptr[1];
        break;
      case 81:    /* ob */
        strupr(cptr);
        for (x = 0; options[x].name; x++)
          if (!strncmp(options[x].name, cptr, 2))
          {
            options[x].value = atoi(cptr + 3);
            return;
          }
        goto invalid;
        break;
      case 82:    /* oc */
        output_case = y;
        break;
      case 83:    /* od */
        strncpy(output_dir, cptr, 126);
        y = strlen(output_dir) - 1;
        if (y && (output_dir[y] == '\\' || output_dir[y] == '/'))
          output_dir[y] = '\0';
#ifdef DOS
        strcat(output_dir, "\\");
#else
        strcat(output_dir, "/");
#endif
        break;
      case 84:    /* of */
        option_file = fopen(cptr, "w");
        break;
      case 85:    /* on */
        output_name = y;
        break;
      case 86:    /* ow */
        over_write = y;
        break;
      case 87:    /* pa */
        pause_time = y;
        spool = 0;
        display_braille = display_source = FALSE;
        break;
      case 88:    /* pd */
        print_date = y;
        break;
      case 89:    /* pe */
        pageend = y;
        break;
      case 90:    /* pf */
        print_file = y;
        break;
      case 91:    /* pl */
        linesperpage = max(y, 5);
        break;
      case 92:    /* pm */
        page_min = min(5, y);
        break;
      case 93:    /* pn */
        strncpy_zero(prn, cptr, sizeof(prn));
        break;
      case 94:    /* ps */
        pagestart = y;
        break;
      case 95:    /* pw */
        y &= 127;
        maxline = curmax = max(y, 10);
        if (total_words)
          flush_if_not_blank();
        break;
      case 96:    /* qm */
        quiet_mode = y;
        break;
      case 97:    /* rc */
        delim = format_char;
        format_char = '~';
        strcpy(words, cptr);
        do_commands(8);
        format_char = delim;
        break;
      case 98:    /* rf */
        strcpy(temp, cptr);
        for (x = 0; temp[x]; x++)
          if (temp[x] == '%')
          {        /* % */
            ch = temp[x + 1];
            delete(temp + x, 2);
            cptr = NULL;
            switch (ch)
            {
            case 'i':
              cptr = inf_name;
              break;
            case 'o':
              cptr = outf_name;
              break;
            }      /* switch */
            if (cptr)
              insert(cptr, temp + x);
          }        /* % */
        system(temp);
        break;
      case 99:    /* rp */
        remove_page_nums = y;
        break;
      case 100:   /* rw */
        rejoin = x;
        if (rejoin_out_ptr != NULL && rejoin_out_ptr != stderr)
          fclose(rejoin_out_ptr);
        rejoin_out_ptr = NULL;
        if (*cptr)
          rejoin_out_ptr = fopen(cptr, "a");
        if (!rejoin_out_ptr)
          rejoin_out_ptr = stderr;
        break;
      case 101:   /* s0 */
        strncpy_zero(s0_init, cptr, sizeof(s0_init));
        break;
      case 102:   /* sc */
        open_spell(cptr);
        break;
      case 103:   /* sd */
        if (y < 2 | y > 30)
          y = 2;
        min_spell_len = y;
        break;
      case 104:   /* si */
        strncpy_zero(stdin_name, cptr, sizeof(stdin_name));
        break;
      case 105:   /* sl */
        scan_lines = y;
        break;
      case 106:   /* sm */
        stat_mode = x;
        break;
      case 107:   /* sn */
        strncpy(bad_spell_name, cptr, 63);
        break;
      case 108:   /* so */
        make_sound = y;
        break;
      case 109:   /* sp */
        spool = y;
        pause_time = 0;
        break;
      case 110:   /* st */
        strncpy_zero(stat_file, cptr, sizeof(stat_file));
        break;
      case 111:   /* tc */
        j = 0;
        do
        {
          vptr = strchr(cptr, vbar);
          if (vptr)
            *vptr = '\0';
          strcpy(t1[j], cptr);
          cptr = vptr + 1;
          j++;
        }
        while (vptr != NULL && j < 6);
        break;
      case 112:   /* td */
        strncpy_zero(table_definition, cptr, 40);
        process_table_definition();
        break;
      case 113:   /* te */
        vptr = strchr(cptr, ' ');
        if (vptr)
          *vptr++ = '\0';
        else
          vptr = cptr + strlen(cptr);
        x = 0;
        while (x < table_entries)
        {
          if (!strcmp(b->match[x], cptr))
          {        /* found entry */
            j = (int) (b->match[x + 1] - b->replace[x] - 1l);
            if ((int) strlen(vptr) > j)
              print_error("\7Table entry %s > %d characters in line %ld\n", vptr, j, total_lines);
            strcpy(b->replace[x], vptr);
            break;
          }        /* found entry */
          x++;
        }          /* while */
        if (x == table_entries)
          print_error("\7No match for te= option line %ld\n", total_lines);
        break;
      case 114:   /* tf */
        vptr = strchr(cptr, ';');
        if (vptr)
        {          /* back trans table specified */
          *vptr = '\0';
          strcpy(table_file[1], vptr + 1);
        }          /* back trans table specified */
        strcpy(table_file[0], cptr);
        current_table_grade = -1;
        load_tables(table_file[trans_mode > 2]);
        break;
      case 115:   /* tm */
        trans_mode = (y % 10);
        trans_mode1 = (y / 10);
        break;
      case 116:   /* tn */
        trans_default = y;
        break;
      case 117:   /* to */
        strncpy_zero(toc_format, cptr, sizeof(toc_format));
        break;
      case 118:   /* tp */
        strncpy_zero(trailing_punct, cptr, sizeof(trailing_punct));
        break;
      case 119:   /* ts */
        if (table_stat_file)
          fclose(table_stat_file);
        table_stat_file = fopen(cptr, "w");
        break;
      case 120:   /* tt */
        table_start_line = (long) y;
        break;
      case 121:   /* tv */
        timer = y;
        break;
      case 122:   /* uk */
        uk_flag = y;
        if (y)
          numeric_def[2] = '1';
        break;
      case 123:   /* vc */
        vptr = strchr(cptr, vbar);
        if (!vptr)
          goto no_ver_bar;
        *vptr = '\0';
        strncpy_zero(vowels, cptr, sizeof(vowels));
        strncpy_zero(consonants, vptr + 1, sizeof(consonants));
        break;
      case 124:   /* ve */
        if (strcmp(cptr, VERSION) < 0)
        {
          fprintf(stderr, "\7Please use the latest .tab files supplied with this program.\n");
          fprintf(stderr, "Press any key to continue...\n");
          getch();
        }
      }            /* switch */
      break;
    }              /* option matched */
  }                /* j */
  if (options[j].name == NULL)
  {                /* invalid */
invalid:
    sprintf(iobuf, "\7Invalid option %s", string);
    if (total_lines)
      sprintf(iobuf + strlen(iobuf), " in line %ld", total_lines);
    for (j = 0; j < 6; j++)
      if (option_mode & (1 << j))
        sprintf(iobuf + strlen(iobuf), " %s\n", option_types[j]);
    print_error(iobuf);
  }                /* invalid */
}                  /* process_options */

void report_option_error(int mode, int number)
{
  int i;
  for (i = 0; i < 8; i++)
    if (mode & (1 << i))
      print_error("\7option %s= not allowed %s\n", options[number].name,
                  option_types[i]);
}                  /* report_option_error */

void open_hyp_dictionary(char *string, int do_check, int open_mode)
{
  int i, j = 0, times[6];
  if (strcmp(hyphen_dic_name, string) == 0 && hyp_dic_ptr != NULL &&
      do_check < 1)
    return;        /* don't close & reopen unless names are different */
  strncpy_zero(hyphen_dic_name, string, sizeof(hyphen_dic_name));
  if (hyp_dic_ptr)
  {                /* close hyphen dictionary */
    fclose(hyp_dic_ptr);
    hyp_dic_ptr = NULL;
  }                /* close hyphen dictionary */
  if (string[0])
  {                /* open hyphen dictionary */
    hyp_dic_ptr = open_option_file(string, open_mode);
    if (do_check)  /* initialize dictionary */
      memset(hyphen_dic_start, 0, sizeof(hyphen_dic_start));
    hyphen_mode |= BIT0;        /* dictionary active */
    hyp_dic_tested = 0;
    if (do_check < 3)
      stat(temp, &hypfilestat); /* get current file date */
    if (hyphen_mode > 0 && do_check)
    {              /* do consistency check */
      if (do_check >= 3)
        return;
      fgets(temp, 80, hyp_dic_ptr);     /* get first line of file */
      for (i = 0; i < 6; i++)
        times[i] = atoi(temp + 3 * i);
      tm = localtime(&hypfilestat.st_mtime);    /* file date in mmddyy */
      if (times[0] != tm->tm_mon + 1 || times[1] != tm->tm_mday ||
          times[2] != (tm->tm_year % 100) || times[3] != tm->tm_hour ||
          times[4] != tm->tm_min || times[5] - tm->tm_sec >= 2l ||
          do_check == 2)
      {            /* check consistency */
        test_hyphen_dictionary(1);
      }            /* test consistency */
    }              /* do consistency check */
  }                /* open hyphen dictionary */
  else
    hyp_dic_ptr = NULL;
}                  /* open_hyp_dictionary */

void open_spell(char *name)
{                  /* opens dictionary file, read in letter offsets verify
                    * format */
  int i;
  char *ptr = strchr(name, '.');
  unsigned short *ibuf;
  long sum = 102;
  if (!name[0])
  {                /* close */
    if (spell_dic_fileh > 0)
      close(spell_dic_fileh);
    if (spell_buffer)
      free(spell_buffer);
    spell_buffer = NULL;
    spell_dic_fileh = 0;
    return;
  }                /* close */
  if (!ptr)
  {                /* no extension */
    strcat(name, ".dat");
  }                /* no extension */
  open_option_file(name, -1);
  if (!bad_spell_name[0])
    strcpy(bad_spell_name, temp);
  spell_dic_fileh = open(temp, (int) (O_BINARY | O_RDONLY));
  if (spell_dic_fileh < 0)
    print_error("\7%s not found\n", temp);
  spell_buffer = malloc(50000);
  if (!spell_buffer)
    print_error("\7Unable to allocate memory for spell dictionary\n");
  ibuf = (unsigned short *) spell_buffer;
  lseek(spell_dic_fileh, 50l, 0);
  read(spell_dic_fileh, ibuf, 52);
  /* calculate absolute position for start of each letter */
  for (i = 0; i < 27; i++)
  {
    letter_offset[i] = sum;
    sum += (long) ibuf[i];
  }
  if (letter_offset[26] != filelength(spell_dic_fileh))
    print_error("\7Invalid spelling dictionary format\n");
  strcpy(bad_spell_name + strlen(bad_spell_name) - 3, "dic");
  spell_dic_file_ptr = fopen(bad_spell_name, "a+");
  if (!spell_dic_file_ptr)
    print_error("\7Can't create %s\n", name);
}                  /* open_spell */

int search_spell()
{
  char *dash_ptr, *space_ptr, *ptr;
  char search_word[40], word_buf[40], *word_ptr;
  int i, l, letter, prev_length;
  unsigned int total = 0, bytes_to_read;
  if (strlen(words) > 30)
    return (1);
  strcpy(search_word, words);
  strupr(search_word);
  do
  {
    while (search_word[0])
    {              /* while not empty */
      letter = (int) (search_word[0]) - 65;
      if (letter < 0 || letter > 25)
      {            /* remove char */
        delete(search_word, 1);
        continue;  /* check leading character again */
      }            /* remove char */
      break;       /* word either empty or begins with a letter */
    }              /* while */
/*word begins with lettor or is nul*/
    dash_ptr = strchr(search_word, dash);
    if (dash_ptr)
      dash_ptr[0] = '\0';
    if (strpbrk(search_word, "0123456789/.=:_"))
      goto retest;
    l = strlen(search_word);
    while (l--)
    {
      if (search_word[l] >= 'A' && search_word[l] <= 'Z')
        break;
      search_word[l] = '\0';
    }              /* while */
    if (search_word[l] == 'S' && (search_word[l - 1] == '\47' || search_word[l - 1] == '('))
      search_word[l - 1] = '\0';
    if ((int) strlen(search_word) < min_spell_len)
      goto retest;
    ptr = spell_buffer;
    lseek(spell_dic_fileh, letter_offset[letter], 0);
    bytes_to_read = (unsigned int) (letter_offset[letter + 1] - letter_offset[letter]);
    read(spell_dic_fileh, spell_buffer, bytes_to_read);
    do
    {
      prev_length = (int) ptr[0];       /* always zero for first word of
                                         * given letter */
      word_ptr = word_buf + prev_length - 1;
      i = 0;
      while (ptr[++i] > 0)
        word_ptr[i] = ptr[i];
      word_ptr[i] = (char) ((int) ptr[i] & 127);
      word_ptr[i + 1] = '\0';
      l = strcmpi(search_word, word_buf);
      if (!l)
        break;     /* word found */
      if (l < 0)
      {            /* word not in dictionary */
        total_misspells++;
        rewind(spell_dic_file_ptr);
        while (fgets(temp, 30, spell_dic_file_ptr))
        {
          temp[strlen(temp) - 1] = '\0';
          space_ptr = strchr(temp, ' ');
          if (space_ptr)
            *space_ptr = '\0';
          l = strcmpi(temp, search_word);
          if (!l)
            break;
        }          /* while */
        if (l)
          fprintf(spell_dic_file_ptr, "%s %ld\n", search_word, total_lines);
        break;     /* word not found, don't continue the search */
      }            /* word not in dictionary */
      ptr += i + 1;
      total += i + 1;
    }              /* while */
    while (total < bytes_to_read);
retest:
    if (dash_ptr)
      strcpy(search_word, dash_ptr + 1);
  }
  while (dash_ptr);
}                  /* search_spell */

void process_table_definition()
{
  int j;
  if (!total_lines)
    print_error("\007td option not allowed on command line or nfbtrans.cnf\n");
  if (!table_definition[0])
    return;
  strlwr(table_definition);
  cols_in_table = chars_in_table = 0;
  strcpy(temp, table_definition);
  for (j = 0; temp[j]; j++)
  {
    if (temp[j] == '%')
    {              /* percent */
      if ((isdigit(temp[j + 1]) || ((temp[j + 1] == dash) &&
                                    isdigit(temp[j + 2]))))
      {            /* field spec */
        if (cols_in_table >= 8)
          print_error("\007> 8 columns in line %ld\n",
                      total_lines);
        j++;       /* index digit */
        field_width[cols_in_table] = abs(get_integer(temp + j));
        if (field_width[cols_in_table] > MAX_COL_WIDTH)
          print_error("\007> %d characters in field %d of table definition in line %ld\n",
                      MAX_COL_WIDTH, cols_in_table + 1, total_lines);
        chars_in_table += field_width[cols_in_table];
        if (temp[j] != 's')
          print_error("\007Invalid field specifier in %s\n",
                      table_definition);
        cols_in_table++;
        continue;
      }            /* field spec */
      if (temp[j + 1] == '*')
        print_error("%s\n", "\007%* not allowed in table definition");
    }              /* percent */
    chars_in_table++;
  }                /* j */
  if (chars_in_table > curmax + margin - 1)
    print_error("\007Table definition > %d characters in line %ld\n",
                curmax + margin - 1, total_lines);
  flush_if_not_blank();
}                  /* process_table_definition */

void trim(char *string)
{
  int i, l = strlen(string);
/*remove trailing cr lf and spaces*/
  for (i = l - 1; i >= 0; i--)
    if (string[i] == ' ' || string[i] == '\n' || string[i] == '\15')
      string[i] = '\0';
    else
      break;
  i = 0;
  while (string[i] == ' ')
    i++;
  if (i)
    delete(string, i);
}                  /* trim */

int strpos(char *string, char *substring)
{
  int i = 0, j, stringlen, sublen;
  stringlen = strlen(string);
  sublen = strlen(substring);
  if ((!stringlen) || (!sublen) || (sublen > stringlen))
    return (0);
  do
  {
    for (j = 0; j < sublen; j++)
      if (string[i + j] != substring[j])
        goto retry;
    return (i + 1);
retry:;
  }
  while (++i < stringlen);
  return (0);
}                  /* strpos */

void insert(char *ins, char *string)
{
  char buf[256];
  sprintf(buf, "%s%s", ins, string);
  strcpy(string, buf);
}                  /* insert */

void delete(char *string, int bytes)
{
  strcpy(string, string + bytes);
}                  /* delete */

void strncpy_zero(char *dest, char *source, int bytes)
{
  strncpy(dest, source, bytes);
  dest[bytes - 1] = '\0';
}                  /* strncpy_zero */

void move(char *string1, char *string2, int bytes)
{
  while (bytes >= 0)
  {
    string2[bytes] = string1[bytes];
    bytes--;
  }                /* while */
}                  /* move */

#ifdef DOS
void sort_names()
{                  /* sorts names in file_names */
  int i, j;
  if (file_count < 2)
    return;        /* nothing to sort */
  for (i = 0; i < file_count; i++)
    for (j = i + 1; j < file_count; j++)
      if (strcmp(file_name[i], file_name[j]) > 0)
      {            /* out of order */
        strcpy(temp, file_name[i]);
        strcpy(file_name[i], file_name[j]);
        strcpy(file_name[j], temp);
      }            /* out of order */
}                  /* sort_names */

#endif             /* DOS */

void translate_file()
{
WIN32_FIND_DATA fdata;
HANDLE findhandle;
  int i, j, input_mode = 0, startfile, create_output;
  char *cptr, c;
  FILE *indirect_ptr = NULL;
  intext = NULL;   /* for statistics file */
  do
  {
    if (!stdin_tty)
      strcpy(inf_name, stdin_name);
    else
    {              /* input filename required */
      if (!input_file_arg)
      {            /* no file args */
        if (!indirect_ptr)
        {
          fprintf(stderr, menu[10]);
          get_input(temp, 80);
          if (!temp[0])
            exit_program(0);
          strcpy(inf_name, temp);
          input_file_arg = paramcount;
        }
      }            /* no file args */
      else
      if (!indirect_ptr)
        strcpy(inf_name, paramstr[input_file_arg]);
      input_mode = 1;
      if (inf_name[0] == '@' && indirect_ptr == NULL)
      {            /* indirect mode */
        delete(inf_name, 1);    /* remove @ */
        indirect_ptr = fopen(inf_name, "r");
        if (!indirect_ptr)
          report_open_error(inf_name);
        pagestart = 1;
      }            /* indirect mode */

      if (indirect_ptr)
        if (fgets(inf_name, 128, indirect_ptr))
        {          /* not eof */
          i = (int) (strlen(inf_name) - 1);
          if (i >= 0 && inf_name[i] == '\n')
            inf_name[i] = '\0';
          if (!i)
            continue;   /* skip blank lines */
        }          /* not eof */
        else
        {          /* eof */
          fclose(indirect_ptr);
          indirect_ptr = NULL;
          input_file_arg++;
          continue;
        }          /* eof */

      inf_path_len = get_path_component(inf_name, 0);
      file_count = current_file = 0;
      startfile = 1;
#ifdef DOS
      if (strpbrk(inf_name, "?*"))
      {            /* wildcard chars */
        input_mode = 2; /* wildcard chars specified */
        pagestart = 1;
        cptr = strpbrk(inf_name, ",;");
        if (cptr)
        {          /* find startfile */
          *cptr = '\0'; /* remove startfile from filename */
          startfile = atoi(cptr + 1);
          if (startfile < 1)
            startfile = 1;
        }          /* find startfile */
      }            /* wildcard chars */
findhandle = FindFirstFile(inf_name, &fdata);
      if (!findhandle)
        do
        {          /* store and sort matching file names */
          if (test_extension(fdata.cFileName) < 0)
            continue;   /* extension was excluded */
          strcpy(file_name[file_count++], fdata.cFileName);
          if (file_count >= MAX_FILES)
            break; /* don't store any more */
        }
        while(!FindNextFile(findhandle, &fdata));
		      if (!file_count)
/*no file was found in search but store name anyway*/
        strcpy(file_name[file_count++], inf_name + inf_path_len);
      sort_names();
      current_file = startfile - 1;
      if (current_file > file_count)
        break;
  get_next_file:
      strcpy(inf_name + inf_path_len, file_name[current_file++]);
if (findhandle)
{
	FindClose(findhandle);
}

#endif
   }              /* input filename required */
    total_lines = 0l;
    if (total_files)
    {              /* not first file */
      lopactive = FALSE;
      l0[0] = l1[0] = '\0';
      ignore_format[0] = '\0';
      get_config();
    }              /* not first file */
    prog_init = test_extension(inf_name);
    if (prog_init < 0)
      print_error("\7%s: Excluded by ex= option\n", inf_name);
    c = format_char;
    format_char = '~';
    strcpy(words, init[prog_init].pre_init);
    do_commands(32);
    if (stdin_tty)
    {              /* input filename required */
      inf_des = open_input_file();
      inf_des_save = inf_des;   /* in case the lf option is encountered */
      if (spool > 1 && total_files > 0 && stdout_tty > 0)
        pause_program();
      if (ab_flag)
        break;
      if (skip_output == 0 && (indirect_ptr != NULL || input_mode == 2
                               || total_files > 0))
        fprintf(stderr, "Translating %s\n", inf_name);
      total_files++;
      fstat(inf_des, &infilestat);
      if (efl_mode && (infilestat.st_mode & IFCHR) == 0 && lopactive == FALSE)
      {            /* process .efl file */
        strcpy(efl_file, inf_name);
        cptr = strchr(efl_file, '.');
        if (!cptr)
          cptr = efl_file + strlen(efl_file);
        strlwr(cptr);
        if (strcmp(cptr, ".efl"))
        {          /* load */
          strcpy(cptr, ".efl");
          if (trans_mode < 3)
            format_char = init[prog_init].format;
          load_template();
        }          /* load */
      }            /* process .efl file */
    }              /* input file required */
    get_date(0);   /* put date in date_string */
    if (!leftmargin)
    {              /* get left margin */
      leftmargin = 1;
      fprintf(stderr, "Enter Number of spaces before Left Margin of source File \n");
      fprintf(stderr, "     (usually 1)?");
      backspace_int(leftmargin);
      get_input(temp, 4);
      leftmargin = atoi(temp);
      if (leftmargin <= 0)
        leftmargin = 1;
    }              /* get left margin */
    if (maxline <= 0)
    {              /* get maxline */
      maxline = 40;
      fprintf(stderr, "Enter Number of braille cells to emboss\n");
      fprintf(stderr, "  on a Line (usually 40)?");
      backspace_int(maxline);
      get_input(temp, 3);
      maxline = atoi(temp);
      if (maxline <= 0)
        maxline = 40;
    }              /* get maxline */
    get_page_range();
    if (display_source < 0)
    {              /* display source? */
      fprintf(stderr, "\nDisplay Source Text (Y/N)? N\010");
      get_digit();
      if (temp[0] == 'y' || temp[0] == 'Y')
        display_source = TRUE;
      else
        display_source = FALSE;
    }              /* display source? */
    if (display_braille < 0)
    {              /* display braille? */
      fprintf(stderr, "\nDisplay Translated Text (Y/N)? Y\010");
      get_digit();
      if (temp[0] == 'n' || temp[0] == 'N')
        display_braille = FALSE;
    }              /* display braille? */
    if (linesperpage <= 0)
    {
      linesperpage = 25;
      fprintf(stderr, "\nNumber of Lines per Page?");
      backspace_int(linesperpage);
      get_input(temp, 5);
      if (temp[0])
        linesperpage = atoi(temp);
      if (linesperpage <= 0)
        linesperpage = 25;
    }
    if (lineskips < 0)
    {
      lineskips = 99;
      fprintf(stderr, "Line Skips between Pages (99-FF, 999-VT)?");
      backspace_int(lineskips);
      get_input(temp, 5);
      if (temp[0])
        lineskips = atoi(temp);
      if (lineskips < 0)
        lineskips = 99;
    }
    if (current_file < 2)
      printit = TRUE;
    if (trans_mode == 3)
      trans_mode1 = 1;
#ifdef unix
    trans_mode1 = 1;    /* translate and store or write to stdout */
    if (!stdin_tty)
    {
      stdout_tty = 0;
      outf_des = 1;
    }
    printit = FALSE;
#endif
    if (!trans_mode1)
    {              /* get secondary translation mode */
      i = (trans_default / 10);
      fprintf(stderr, menu[0]);
      for (j = 5; j < 8; j++)
        fprintf(stderr, menu[j]);
      backspace_int(i);
      do
      {
        get_digit();
        if (!temp[0])
          i = (trans_default / 10);
        else
          i = atoi(temp);
      }
      while (i < 1 || i > 2);
    }              /* get secondary translation mode */
    else
      i = trans_mode1;
    trans_mode1 = i;
    if (i == 1)
    {              /* translate and store */
      printit = FALSE;
      if (!outf_name[0] && stdout_tty != 0)
      {            /* output file required */
        create_output = output_name & ((trans_mode == 1) + 2 * (trans_mode == 3));
        if (create_output)
        {          /* construct output name */
          strcpy(outf_name, inf_name);
          if ((output_name & 4) == 0)
            get_path_component(outf_name, 1);   /* only keep file name */
          cptr = strchr(outf_name, '.');
          if (cptr)
            *cptr = '\0';
          strcat(outf_name, output_extension[(trans_mode > 1)]);
        }          /* construct output name */
        else
        {          /* prompt */
          fprintf(stderr, menu[11]);
          get_input(outf_name, MAXPATHLEN - 1);
        }          /* prompt */
        if (!outf_name[0])
          exit_program(0);
        if (output_dir[0] && strpbrk(outf_name, "\\/:") == NULL)
          insert(output_dir, outf_name);
      }            /* output file required */
    }              /* translate and store */
    if (!rejoin)
      rejoin = TRUE;
    xgrade = 2;
    xformat = TEXT;
    find_toc_pages = FALSE;
    memset(toc_pages, 0, sizeof(toctype) * MAX_TOC_ENTRY);
    current_pass = 1;
    if (spool > 0 && file_count >= 9 && printit)
      spool++;     /* pause between files so queue won't overflow */
    if ((printit && !(usr_default)))
      get_page_range();
    get_copies();
    if (printit == FALSE && stdout_tty != 0)
    {              /* disk file */
      if (test_file_exist())
        return;
      if (stdout_tty)
        printf("Writing to file - %s\n", outf_name);
    }              /* disk file */
    else
    {
      get_printer_file_name();
      if (total_files == 1)
        do_pause();
    }
    if (ab_flag)
      return;
    time(&time1);
    if (outf_name[0] && outf_des == 0)
      outf_des = open(outf_name, (int) (O_BINARY | O_CREAT | O_TRUNC | O_WRONLY), STD_OPEN);
    keep_control = top_margin = 0;
    keep_together = keep_together_save = max_input_length = 0;
    total_lines = 0l;
    switch (trans_mode)
    {
    case 1:
      load_tables(table_file[0]);
      break;
    case 3:
      current_table_grade = -1;
      load_tables(table_file[1]);
      if (remove_page_nums)
        get_paragraph_type(1);
    }              /* switch */
    if (trans_mode < 3)
      format_char = init[prog_init].format;
    if (xformat == AUTO_FORMAT || auto_center)
      get_paragraph_type(auto_center);
    if (printit && spool)
    {              /* printer ignore formfeed */
      format_char = '~';
      strcpy(words, s0_init);
      do_commands(32);
      format_char = init[prog_init].format;
    }              /* printer ignore formfeed */
#ifdef unix
    unbuf_stdin(); /* so we can check for esc */
#endif
    in_length = filelength(inf_des);
    if (infilestat.st_mode & IFCHR)
      in_length = 0l;

    do
    {
      do_translate();
      if (toc_file_ptr)
      {            /* close */
        fclose(toc_file_ptr);
        unlink(toc_file_name);
        toc_file_ptr = NULL;
      }            /* close */
    }
    while (copies < lastcopy && ab_flag == 0);
    out_length = filelength(outf_des);
    emboss_time = total_cells / (60l * (long) charspersec);
    write_stat_file(0);
    format_char = '~';
    strcpy(words, init[prog_init].post_init);
    do_commands(32);
    if (trans_mode == 3)
      format_char = c;
    close(inf_des);
#ifdef DOS
    if (printit && spool && ab_flag == 0)
    {              /* spool */
      if (stdout_tty)
      {            /* output not redirected */
        if (emboss_time > 1l)
          printf("Estimated embossing time: %ld minutes\n",
                 emboss_time);
      }            /* output not redirected */
      close(outf_des);
      outf_des = 0;
      spool_file();
    }              /* spool */
    if (ab_flag)
      break;
    if (input_mode)
    {
      copies = 0;
      if (current_file < file_count)
      {
        if (create_output)
        {          /* force next create */
          if (outf_des > 1)
            close(outf_des);
          outf_des = 0;
          outf_name[0] = '\0';
        }          /* force next create */
        goto get_next_file;
      }
    }
#endif             /* DOS */
    if (input_mode == 2)
    {              /* wildcards */
      input_mode = 1;
    }              /* wildcards */
    if (indirect_ptr)
      continue;
    input_file_arg++;
  }
  while (input_file_arg <= paramcount);
  if (outf_des > 1)
    close(outf_des);
  if (intext)
    fclose(intext);
  fclose(conf_ptr);
}                  /* translate_file */

int get_path_component(char *string, int mode)
{                  /* returns length of path component of string. If mode is
                    * 1 then path in string is deleted, leaving the file name */
  int i, retval = 0;
  for (i = 0; string[i]; i++)
    if (string[i] == '\\' || string[i] == '/' || string[i] == ':')
      retval = i + 1;
  if (mode && retval)
    strcpy(string, string + retval);
  return (retval);
}                  /* get_path_component */

int open_input_file()
{
  int h;
  h = open(inf_name, O_BINARY | O_RDONLY);
  if (h < 0)
    report_open_error(inf_name);
  return (h);
}                  /* open_input_file */

int test_file_exist()
{
  int i, retval = 0;
  if (!access(outf_name, 0))
  {                /* exists */
    i = get_path_component(outf_name, 0);
    stat(outf_name, &outfilestat);
    if (infilestat.st_mtime == outfilestat.st_mtime &&
        infilestat.st_size == outfilestat.st_size &&
        strcmp(inf_name + inf_path_len, outf_name + i) == 0)
      print_error("\7Cannot translate a file on to itself\n");
    if (over_write == 0)
    {              /* file already exists */
      printf("\007File %s exists - Overwrite (Y/N)? Y\010", outf_name);
      get_digit();
      if (temp[0] == 'n' || temp[0] == 'N')
        retval = 1;
    }              /* file already exist */
  }                /* exists */
  return (retval);
}                  /* test_file_exist */

void report_open_error(char *string)
{
  if (errno == EACCES)
    print_error("\007%s is a directory\n", string);
  else
    print_error("\7%s not found.\n", string);
}                  /* report_open_error */

void spool_file()
{
  sprintf(temp, "print %s", outf_name);
  if (system(temp))
    print_error("\007Could not execute %s\n", temp);
}                  /* spool_file */

int test_extension(char *string)
{
  int i, j = 0;
  char *cptr;
  /* get file extension */
  cptr = strchr(string, '.');
  if (cptr == NULL)
  {                /* no extension */
    for (i = 0; string[i]; i++)
      if (string[i] == ':' || string[i] == '\\' || string[i] == '/')
        j = i + 1;
    /* copy the filename without path component */
    strncpy_zero(inf_name_ext, string + j, 9);
    j = search_extensions(inf_name_ext);
    if (strlen(inf_name_ext) <= 3)
      j = 0;
    if (j)
      goto done_test;
    cptr = string + strlen(string) - 1;
  }                /* no extension */
  strcpy(inf_name_ext, cptr + 1);
  /* find out if extension is in an ex= option */
  j = search_extensions(inf_name_ext);
done_test:
  if (j)
    return (prog_extension.prog_ext[j - 1].init_val);
  return (0);      /* for extensions not in ex= */
}                  /* test_extension */

int search_extensions(char *string)
{
  int i, j = 0;
  for (i = 0; i < prog_extension.total; i++)
#ifdef unix
    if (!strcmp(string, prog_extension.prog_ext[i].ext))
#else
    if (!strcmpi(string, prog_extension.prog_ext[i].ext))
#endif
      j = i + 1;   /* extension was in list */
  return (j);
}                  /* search_extensions */

void emboss_file()
{
  char ch, *cptr;
  if (input_file_arg)
    strcpy(inf_name, paramstr[input_file_arg]);
  if (!inf_name[0])
  {                /* get filename */
    printf("\nEnter the name of the already Translated File <RETURN> to exit: ");
    get_input(temp, 48);
    if (!temp[0])
      exit_program(0);
    strcpy(inf_name, temp);
  }                /* get filename */
  if ((intext = fopen(inf_name, fopen_read[0])) == NULL)
    print_error("\7%s does not exist\n", inf_name);
  get_copies();
  get_page_range();
  usr_default = FALSE;
  printit = TRUE;
  get_date(0);
  get_printer_file_name();
  do_pause();
  if (ab_flag)
    return;
  outf_des = open(outf_name, O_BINARY | O_CREAT | O_TRUNC | O_WRONLY, S_IWRITE);
  do
  {                /* while there are copies to emboss */
    copies++;
    actualpage = 0;
    bpagec = 1;    /* current braille page */
    lineinct = 0;
    rewind(intext);/* start from beginning of file for this copy */
    while ((fgets(field, 80, intext)) && bpagec <= pageend)
    {              /* while there are lines to read and not passed ending
                    * page */
      if (check_keyboard(0) < 0)
        break;
      lineinct++;
      linecount = (int) strlen(field);
      field[linecount - 1] = 0; /* remove \n */
      if ((cptr = strpbrk(field, "\013\014")) != NULL ||
          (int) lineinct > linesperpage)
      {            /* new page */
        bpagec++;  /* ff or vt */
        page_beep();
        if (!skip_output)
          fprintf(stderr, "Embossing page %d\n", bpagec);
        lineinct = 1;   /* first line on new page */
        if (bpagec > pageend)
          break;   /* done */
        if (display_braille > 0 && page_in_range() && skip_output == 0)
        {          /* display braille */
          fprintf(stderr, "%s\n", field);
          if (linecount >= 20)
            fprintf(stderr, "       Copy %d of %d\n", copies, lastcopy);
        }          /* display_braille */
      }            /* new page */
      if (page_in_range())
      {            /* process page */
        if (cptr)
        {          /* vt or ff */
          ch = *cptr;
          if (!skip_output)
            fprintf(stderr, "%s\n", field);
          strcpy(field, cptr + 1);
          if (actualpage)
            write_char(ch);
          actualpage++;
          delay(emboss_delay);
          if (field[0])
            write_string(field, 0);
        }          /* ff or vt */
        else
        {          /* no ff or vt */
          write_string(field, 1);
        }          /* no f or vt */
      }            /* process page */
    }              /* while */
  }
  while (copies < lastcopy);
  fclose(intext);
  close(outf_des);
  if (spool)
    spool_file();
}                  /* emboss_file */

void get_page_range()
{
  int i;
  char *cptr;
  if (trans_mode == 3)
    pagestart = 1, pageend = 9999;
  if (pagestart <= 0)
  {                /* get starting page */
    pagestart_roman = arabic_flag = 1;
    pageend_roman = 9999;
    roman_flag = 0;
    fprintf(stderr, "%s 1\010", menu[8]);
    get_input(temp, 80);
    if (!temp[0])
      pagestart = 1;
    cptr = strpbrk(temp, "-, ");
    if (cptr)
      *cptr = '\0';
    i = get_integer(temp);
    /* if interpoint then page should be odd */
    if (interpoint && (i & 1) == 0)
      i--;
    switch (temp[0])
    {
    case 'r':
      pagestart_roman = i;
      roman_flag = 1;
      break;
    case 'a':
      roman_flag = 1;
      pagestart_roman = 9999;
    default:
      pagestart = i;
    }              /* switch */
    if (cptr)
      get_end_page(cptr + 1);
    if (pagestart <= 0)
      pagestart = 1;
  }                /* get starting page */
  if (pageend < pagestart)
  {                /* pageend */
    pageend = 9999;
    fprintf(stderr, menu[9]);
    backspace_int(pageend);
    get_input(temp, 5);
    get_end_page(temp);
  }                /* pageend */
  pagestart_save = pagestart;
  pageend_save = pageend;
}                  /* get_page_range */

void get_end_page(char *string)
{
  int i;
  i = atoi(string);
  if (!string[0])
    i = 9999;
  if (interpoint && (i & 1))
    i++;           /* make even */
  if (strchr(string, 'r'))
  {                /* roman */
    pageend_roman = i;
    pagestart = pageend = 32766;
    arabic_flag = 0;
  }                /* roman */
  else
    pageend = i;
  if (pageend < 1)
    pageend = 1000;
}                  /* get_end_page */

int page_in_range()
{
  int retval = 0;
  if (doroman)
  {                /* roman page number */
    if (roman_flag)
    {              /* roman range specified */
      if (bpagec >= pagestart_roman)
        retval = bpagec + 1 - pagestart_roman;
      if (bpagec > pageend_roman)
        retval = 0;
    }              /* roman range specified */
    else
    if (bpagec >= pagestart)
      retval = bpagec + 1 - pagestart;
    if (pagestart == 32767)
      retval = 0;  /* no output pass 1 of 2 */
  }                /* roman page number */
  else
  if (bpagec >= pagestart)
    retval = bpagec + 1 - pagestart;
  return (retval);
}                  /* page_in_range */

void get_copies()
{
  if (lastcopy <= 0)
  {
    lastcopy = 1;
    printf("\nNumber of Copies? %d\010", lastcopy);
    get_input(temp, 80);
    lastcopy = atoi(temp);
    if (lastcopy <= 0)
      lastcopy = 1;
  }
}                  /* get_copies */

void get_printer_file_name()
{
  int tries = 0;
  char *cptr;
  if (printit)
    if (stdout_tty)
    {              /* stdout is tty */
      if (!spool)
        strcpy(outf_name, prn);
      else
      {            /* spool */
        /* get location of temp file */
        cptr = (char *) getenv("TMP");
        if (cptr)
          strcpy(temp, cptr);
        else
          temp[0] = 0;
        do
        {
          tries++;
          sprintf(outf_name, "%s%02d%02d%02d", temp, tm->tm_hour, tm->tm_min,
                  tm->tm_sec);
          if (!access(outf_name, 0))
            tm->tm_sec++;       /* file exists so use a different name */
          else
            break;
        }
        while (tries < 10);

        if (interpoint > 1)
          interpoint = 1;       /* don't eject blank page */
        printf("Creating %s\n", outf_name);
      }            /* spool */
    }              /* stdout is tty */
}                  /* get_printer_file_name */

void do_pause()
{
  int i;
  if (pause_time && printit)
  {                /* pause */
    for (i = 0; i < pause_time; i++)
    {
      if (make_sound & BIT1)
#ifdef DOS
        sound(440);
      delay(80);
      nosound();
#else              /* unix */
        beep(1);
#endif             /* DOS */
      delay(800);
      if (stdin_tty)
        if (check_keyboard(0) > 0)
          getch();
    }              /* i */
    skip_output = TRUE;
  }                /* pause */
}                  /* do_pause */

void pause_program()
{
  if (emboss_time)
  {                /* pause */
    printf("Press any key to continue or wait %ld minutes for embossing\n",
           emboss_time);
    time1 = (long) time(NULL);
    do
    {
      if (check_keyboard(0))
        break;
    }
    while ((long) time(NULL) - time1 < emboss_time * 60l);
  }                /* pause */
}                  /* pause_program */

int check_keyboard(int toggle)
{                  /* returns -1 for esc 0 for no key and 1 otherwise */
  int retval = 0;
  long pos;
  if (kbhit())
  {                /* get key */
    switch (getch())
    {
    case 32:
      if (toggle)
        make_sound ^= 1;
      break;
    case 27:
      done = TRUE; /* abort */
      ab_flag = 1;
      retval = -1;
      break;
    case 70:
    case 102:
      pos = lseek(inf_des, 0l, 1) + (long) (ioptr - iobuf) - (long) BUFSIZE;
      fprintf(stderr, "%s %d %d = %ld%%\n", inf_name, inf_des, bpagec,
              (100l * pos) / in_length);
    default:
      retval = 1;
    }              /* switch */
  }                /* get key */
  return (retval);
}                  /* check_keyboard */

void write_string(char *string, int display)
{
  int l;
  strcpy(temp, string);
  strcat(temp, lt_string[eol_term]);
  l = strlen(temp);
  if (!output_case)
    strlwr(temp);
  if (write(outf_des, temp, l) < 0)
    no_space();
  if (printit)
    delay(emboss_delay);
  if (display)
    if (display_braille && skip_output == FALSE)
      fprintf(stderr, string_format, temp);
  if (stat_file[0])
    add_dots(string);
}                  /* write_string */

void write_toc_header()
{
  int i;
  char string[16];
  if (toc_entry == 0 || (book_mode & 8) == 0)
    return;
  i = 2 + (toc_word == 0);
  if (blinec + i > linesperpage - top_margin)
  {
    top_of_form();
    blinec = i;
  }
  else
    blinec += i;
  if (page_in_range())
  {
    sprintf(string, "%%-%ds%%5s %%5s", curmax - fill_length + 1);
    sprintf(field, string, t1[0], t1[1], t1[2]);
    write_string(field, 0);
    sprintf(field, string, t1[3], t1[4], t1[5]);
    write_string(field, 0);
    if (!toc_word)
      write_string(blank_line, 0);
  }
  field[0] = 0;
}                  /* write_toc_header */

void write_char(char ch)
{
  if (write(outf_des, &ch, 1) <= 0)
    no_space();
}                  /* write_char */

void no_space()
{
  int i;
  if (make_sound & BIT2)
    for (i = 0; i < 5; i++)
    {
#ifdef DOS
      sound(750);
      delay(100);
      sound(1500);
      delay(100);
#else              /* unix */
      beep(2);
#endif             /* DOS */
    }
#ifdef DOS
  nosound();
#endif             /* DOS */
  print_error("Insufficient disk space or bad output file\n");
}                  /* no_space */

void copy_string(char *string1, char *string2, int maxlen)
{                  /* copies at most length bytes of string2 into string1
                    * accounting for escaped characters */
  int i, j = 0, k;
  char c;
  for (i = 0; (string2[i] != '\0' && j < maxlen); i++)
    if (string2[i] != '\\')
      string1[j++] = string2[i];
    else
    {              /* escape */
      i++;         /* skip \ */
      if (isdigit(string2[i]))
      {            /* digit */
        k = i++;   /* indexes first digit */
        if (isdigit(string2[i]))
          i++;
        if (isdigit(string2[i]))
          i++;
        c = string2[i]; /* character after last digit */
        string2[i] = '\0';
        string1[j++] = (char) atoi(string2 + k);
        string2[i--] = c;
      }            /* digit */
    }              /* escape */
  string1[j] = '\0';
}                  /* copy_string */

void cleanup(int x)
{
#ifdef DOS
  setmode(1, O_TEXT);
#else              /* unix */
  restore_stdin();
#endif
  if (x == SIGINT)
    exit(0);
}                  /* cleanup */

void exit_program(int x)
{
  cleanup(-1);
  exit(x);
}                  /* exit_program */

void print_error(char *message,...)
{                  /* prints message to stderr and exits program */
#ifdef unix
  int ip[MAXARGS + 1];
  int i;
  va_list ap;
  va_start(ap, message);
  for (i = 1; i <= MAXARGS; i++)
    ip[i] = va_arg(ap, int);
  va_end(ap);
#else
  long *ip = (long *) &message;
#endif
  sprintf(iobuf, message, ip[1], ip[2], ip[3], ip[4], ip[5], ip[6]);
  fprintf(stderr, "%s", iobuf);
  write_stat_file(1);
  exit_program(1);
}                  /* print_error */

int ISalpha(char c)
{
  if (c < '\0')
  {
    if (rejoin & 8)
      return (1);
    return (0);
  }
  return (isalpha(c));
}                  /* ISalpha */

int Isdigit(char c)
{
  if (c <= '\0')
    return (0);
  return (isdigit(c));
}                  /* Isdigit */

int main(int argc, char *argv[])
{
  int i, j;
  char *cptr;
  paramcount = argc - 1;
  paramstr = argv;
  /* initialize structures */
  b = (tablet *) malloc(sizeof(tablet));
  tablebuf = end_table_ptr = (char *) malloc(MAX_TABLE_BUF);
  if (b == NULL || tablebuf == NULL)
    print_error("\7Unable to allocate memory for table\n");
  memset(init, 0, sizeof(init_t));
  memset(&prog_extension.total, 0, sizeof(prog_ext_t));
  stdin_tty = isatty(0);
  stdout_tty = isatty(1);
#ifdef unix
  save_stdin();
  if (stdout_tty)
    setbuf(stdout, NULL);       /* so printf always prints */
#endif             /* unix */
  cptr = menu_buf;
  for (i = 0; i < 12; i++)
  {
    menu[i] = cptr;
    strcpy(cptr, main_menu[i]);
    j = strlen(cptr) * 3;
    j = min(j, 80);
    cptr += j;
    if (cptr - menu[0] > MAX_MENU_BUF)
      print_error("\7menu > %d characters\n", MAX_MENU_BUF);
  }                /* i */
  menu[12] = cptr;
  get_config();
  signal(SIGINT, cleanup);
  if (no_copyright == FALSE)
  {                /* display message */
    fprintf(stderr, "\nNFBTRANS Grade Two Braille Translator - Release Version %s\n%s\n",
            VERSION, DATE);
    fprintf(stderr, "%s\n\n", COPYRIGHT);
  }                /* display message */

  copies = 0;
  if (usr_default == FALSE && pagestart <= 1 && trans_mode <= 0)
  {                /* choose mode */
    j = (trans_default % 10);
    for (i = 0; i < 4; i++)
      fprintf(stderr, menu[i]);
    while (trans_mode < 1 || trans_mode > 3)
    {
      fprintf(stderr, "\n%s", menu[4]);
      backspace_int(j);
#ifdef DOS
      if (make_sound & BIT3)
      {
        sound(440);
        delay(20);
        sound(1760);
        delay(20);
        nosound();
      }
#endif             /* DOS */
      get_digit();
      if (!temp[0])
        trans_mode = j;
      else
        trans_mode = atoi(temp);
    }              /* while */
  }                /* choose mode */
  if (trans_mode == 1 || trans_mode == 3)
    translate_file();
  else
  if (trans_mode == 2)
    emboss_file();
  if (ab_flag)
    fprintf(stderr, "Program aborted\n");
  if (skip_output == TRUE && stdout_tty == TRUE)
  {
    do_pause();
    getch();
  }
  exit_program(0);
  return (0);
}                  /* main */

void sound(int freq)
{
if (nsound == 0)
Beep(freq, 250);
}
void nosound()
{
	nsound = 1;
}
void delay(int d)
{
	Sleep(d);
}