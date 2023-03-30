# FocasSimple说明
简易 FOCAS API 程序。

## 功能
1、演示读取CNC系统坐标、用户宏变量，以及PMC数值。

2、演示写入用户宏变量、PMC数值。

## 用法
需要用到FANUC FOCAS API函数库，请自行准备相关dll文件。

## 涉及的API函数
cnc_allclibhndl3.........Get the library handle(for Ethernet)

cnc_freelibhndl.........Free library handle

cnc_absolute2.........Read absolute axis position 2

cnc_machine.........Read machine axis position

cnc_rdmacro.........Read custom macro variable

cnc_wrmacro.........Write custom macro variable

pmc_rdpmcrng.........Read PMC data(area specified)

pmc_wrpmcrng.........Write PMC data(area specified)

## 其他
由于时间关系，部分内容做了精简，待有时间再补充吧。
