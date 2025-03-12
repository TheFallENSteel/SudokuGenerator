# Overview
Sudoku Generator Application is a console application that allows user to generate, solve and export to PDF Sudoku.

Application is divided into three separate projects, namely 
Sudoku Generator, SudokuSolver and PDFGenerator each serving different purpose. The core of the app is Sudoku Generator which is the entry point of the app. It contains the user interface and references the other two projects when necessary. The PDFGenerator is responsible for exporting the sudoku in PDF format for users to print and solve on paper. SudokuSolver is the main project that deals with issues actually related to sudoku solving. 

Project is built with C# and uses PDFSharp library.



# PDF Generator
PDF generator uses PDFSharp library to generate PDF files for user to print and solve on paper.

## DocumentSetting.cs
Class that handles how is PDF file generated.

## IPrintableSudoku.cs
Interface for sudokus that should be generated.

## PrintableSudokuDrawer.cs
Class specifying the paging and spacing of sudokus.

## SudokuGrid.cs
Generates PrintableSudoku drawer.

## PDFGenerator.cs
Entry file. Comprises of methods that creates the PDFFile based on user specifications. Calculates all necessary values and interacts with PDFSharp library.



# Sudoku Solver
Sudoku solver, and generator. Generates sudokus by first generating valid solved sudoku and then unsolving it with allowed techniques (based on user specified difficulty).

## Cell.cs
Represents current status of cells in sudoku with corresponding possibilities (known by generator).

## Container.cs
Represents current status of container (rows, columns and squares) and performs checks for hidden values in them. Defines operations like intersection and exclusion and is used to mass manipulate all cells that it contains.

## SudokuData.cs
Holds data of sudoku (cells, containers). Can check for hidden values, pointing and claiming. Can validate values in sudoku, determine whether it is solved, count empty cells and find cell with least possible values and render sudoku values into string (formatted for reading/unformatted for further use in other apps).

## Sudoku.cs
Holds intermediate values for the solver. Can generate random solved sudoku, unsolve or solve it.



# SudokuGenerator.cs
It is the entry project that connects the others together. It includes the CLI interface.

## Command.cs
Class representing commands in the CLI. Holds values of it's aliases and info.

### CommandInfo.cs
Holds information about commands (name short description and long description).

### CommandArgs.cs
Class that represents arguments of CLI commands and parses them to specified data type.

### PositionalCommand.cs
Command that can be used in places of arguments (currently only the help command). and uses the rest of the command as its' parameter.

## CommandContainer.cs
Handle the using in command. Allows adding new Commands, listing all current commands

## Program.cs
Is home of the Main method (entry of program).

## ProgramState.cs
Holds global program objects relating to the status of the program.

## SudokuLoader.cs
Parses inputted sudoku from string to int array.

## SudokuWrapper.cs
Wraps Sudoku from Sudoku solver into IPrintableSudoku object.

# Difficulty

Technique | Difficulty
--------- | ----------
No Technique | 0
Hidden Values | 1
Naked Values | 2
Pointing | 3
Claiming | 4