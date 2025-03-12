# Overview
Sudoku solver allows you to generate unlimited number of sudoku puzzles for you to solve, based on the level of techniques you know, so you can solve the sudoku comfortably (or not). 

App can be used easily even without this guide, because the CLI has built in help command
```
help [command]
```
or positional ?help command to find out about argument at that position

```
[command] [args] ... ?help ... [args]
```
and quick start command for introduction
```
quickstart
```

# Commands
You have many useful commands that you can use in the command line.
## Add Sudoku
*Aliases*: *add*, *+*, *append*

Adds sudoku to your working buffer (ex. you want to solve it).
```
add [SudokuString]
```
Sudoku string is sequence of 81 numbers (spaces are ignored)

## Clear
*Aliases*: *clear*, *clr*, *cl*, *delete*, *del*, *flush*

Clears the sudoku buffer
```
clear [SudokuString]
```
## Exit
*Aliases*: *exit*, *ex*, *e*, *stop*, *terminate*, *end*, *quit*, *leave*, *q*

Used to close program (asks user).
```
exit
```

## Generate Sudoku
*Aliases*: *generate*, *gen*, *g*

Generates Sudoku based on parameters
```
generate [max difficulty] [sudokuCount]
```

## Help and QuickStart
*Aliases*: *help*, *h* / *quickstart*
```
help [command]
```
Shows description of command

```
[command] [args] ... ?help ... [args]
```
Shows positional ?help command to find out about argument at that position
```
quickstart
```
Shows introduction to program

## Print Sudoku String
*Aliases*: *print*, *pr*, *data*, *d*

Prints text representation of sudoku in the console for further use.
```
print [raw]
```
If raw is *true*, *t*, *1* returns raw representation of sudoku, 81 consecutives numbers (for pasting to other applications, saving and sharing). Otherwise it returns readable text in console with new lines and spaces (to view the sudoku).

## Solve Sudoku
*Aliases*: *solve*

Tries to solve sudoku with methods based on difficulty.
```
solve [difficulty]
```

## Save PDF
*Aliases*: *pdf*, *export*, *save*, *s*, *output*, *out*

Saves sudokus into the PDF file
```
pdf [file name] [file directory] [page format] [rows] [columns]
```
Page format ex. A4, A5,...
Rows, columns specifies number of rows of sudokus on one page.