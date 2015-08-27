@echo off
cls
rem set path=C:\Program Files\MiKTeX 2.6\scripts\tex4ht\bat;%path%
set latex=--quiet --aux-dir=%temp%

        latex %5 \makeatletter\def\HCode{\futurelet\HCode\HChar}\def\HChar{\ifx"\HCode\def\HCode"##1"{\Link##1}\expandafter\HCode\else\expandafter\Link\fi}\def\Link#1.a.b.c.{\g@addto@macro\@documentclasshook{\RequirePackage[#1,xhtml,mozilla]{tex4ht}}\let\HCode\documentstyle\def\documentstyle{\let\documentstyle\HCode\expandafter\def\csname tex4ht\endcsname{#1,xhtml,mozilla}\def\HCode####1{\documentstyle[tex4ht,}\@ifnextchar[{\HCode}{\documentstyle[tex4ht]}}}\makeatother\HCode %2.a.b.c.\input  %1
        latex %5 \makeatletter\def\HCode{\futurelet\HCode\HChar}\def\HChar{\ifx"\HCode\def\HCode"##1"{\Link##1}\expandafter\HCode\else\expandafter\Link\fi}\def\Link#1.a.b.c.{\g@addto@macro\@documentclasshook{\RequirePackage[#1,xhtml,mozilla]{tex4ht}}\let\HCode\documentstyle\def\documentstyle{\let\documentstyle\HCode\expandafter\def\csname tex4ht\endcsname{#1,xhtml,mozilla}\def\HCode####1{\documentstyle[tex4ht,}\@ifnextchar[{\HCode}{\documentstyle[tex4ht]}}}\makeatother\HCode %2.a.b.c.\input  %1
        latex %5 \makeatletter\def\HCode{\futurelet\HCode\HChar}\def\HChar{\ifx"\HCode\def\HCode"##1"{\Link##1}\expandafter\HCode\else\expandafter\Link\fi}\def\Link#1.a.b.c.{\g@addto@macro\@documentclasshook{\RequirePackage[#1,xhtml,mozilla]{tex4ht}}\let\HCode\documentstyle\def\documentstyle{\let\documentstyle\HCode\expandafter\def\csname tex4ht\endcsname{#1,xhtml,mozilla}\def\HCode####1{\documentstyle[tex4ht,}\@ifnextchar[{\HCode}{\documentstyle[tex4ht]}}}\makeatother\HCode %2.a.b.c.\input  %1
        tex4ht \dirchar %1  -ic:\tex4ht\texmf\tex4ht\ht-fonts\%3 -ec:\tex4ht\texmf\tex4ht\base\win32\tex4ht.env -cmozhtf
        t4ht \dirchar %1 %4 -ec:\tex4ht\texmf\tex4ht\base\win32\tex4ht.env -cvalidate 



