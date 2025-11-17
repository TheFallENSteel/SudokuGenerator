# Overview
Sudoku Generator Application is a console and WPF application that allows user to generate, solve and export to PDF Sudoku.

Application is divided into four separate projects, namely 
Sudoku Generator, SudokuSolver and PDFGenerator and GraphicalInterface each serving different purpose. The core of the app is either Sudoku Generator which is the entry point of the Console app or GraphicalInterface for WPF app. It contains the user interface and references the other two projects when necessary. The PDFGenerator is responsible for exporting the sudoku in PDF format for users to print and solve on paper. SudokuSolver is the main project that deals with issues actually related to sudoku solving. 

Project is built with C# and uses PDFSharp library.

# Program Structure

Program is divided into four projects, each serving different purpose.

# Project PDF Generator
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



# Project Sudoku Solver
Sudoku solver, and generator. Generates sudokus by first generating valid solved sudoku and then unsolving it with allowed techniques (based on user specified difficulty).

## Cell.cs
Represents current status of cells in sudoku with corresponding possibilities (known by generator).

## Container.cs
Represents current status of container (rows, columns and squares) and performs checks for hidden values in them. Defines operations like intersection and exclusion and is used to mass manipulate all cells that it contains.

## SudokuData.cs
Holds data of sudoku (cells, containers). Can check for hidden values, pointing and claiming. Can validate values in sudoku, determine whether it is solved, count empty cells and find cell with least possible values and render sudoku values into string (formatted for reading/unformatted for further use in other apps).

## Sudoku.cs
Holds intermediate values for the solver. Can generate random solved sudoku, unsolve or solve it.



# Project SudokuGenerator (CLI)
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



# Project GraphicalInterface (GUI)
Entrypoint for graphical interface of the app.

## App
Class representing the graphical application.

## ButtonBox.cs
Communicates with the Sudoku Browser and holds all the buttons and difficulty combobox.

## MainWindow.cs
Represents the window of the application and shows ButtonBox and SudokuBrowser.

## Sudoku.cs
Implements INotifyPropertyChanged and holds data about the Sudoku.

## SudokuBrowser.cs
Real core of the App. Displays Sudoku and request sudoku operations from SudokuSolver class. Also uses background thread to pre-generate solved sudokus.

## Sudoku Wrapper
Wraps Sudoku from Sudoku solver into IPrintableSudoku object.



# Difficulty

Technique | Difficulty
--------- | ----------
No Technique | 0
Hidden Values | 1
Naked Values | 2
Pointing | 3
Claiming | 4



# Notable Used Algorithms
All used sudoku solving algorithms copy techniques that are commonly used by human sudoku solvers. I learned them on website [Sudoku Rules](https://sudoku.com/sudoku-rules/).

## Sudoku Techniques algorithms

### Naked Values
Checks all remaining possibilities in each cell and if only one is remaining, value is set to it.

### Hidden Values
Checks for all possibilities in all containers (row, column, box) whether it contains only one possible cell that can have specified value.

### Pointing/Claiming
For human solvers those are two different techniques. But for algorithms they are same with exception of which containers are used as primary and secondary (for human solver square differ a lot from rows and columns).

For pointing squares are primary containers while for claiming they are secondary. Row and Column containers are merged into one. Otherwise the algorithm is the same.


We go through each combination of primary and secondary algorithm, and for each of those we find: 
1) Intersection cells (in primary and in secondary containers),
2) Exclusion cells (in primary and not in secondary)

and take all possibilities in those cells. From those we want to take those that are in intersection case and not in exclusion cells. The techniques tells us that those cannot be outside of the intersection cells, so we remove them.

Before applying the technique (Pointing):

![PointingBefore](https://sudoku.com/img/post-images/1646982767-11.%20Pointing%20pairs_1.png)

After applying the technique (Pointing):

![PointingAfter](https://sudoku.com/img/post-images/1646982767-11.%20Pointing%20pairs_1.png)

## Sudoku Generation Algorithms

Sudoku generation is done by first generating a valid solved sudoku and then unsolving it with allowed techniques (based on user specified difficulty). First we generate a solved sudoku by using naked values and hidden values techniques (they are fast). Then we unsolve it with techniques allowed by difficulty.

## CLI Command Algorithms

Commands in the Command line use command pattern. There is a CommandContainer which accept string commands (from the CLI) and then looks available commands in dictionary, where the commands are. First positional commands are tested (those that can be in other positions than start of a line) and then normal commands. 
Commands have multiple aliases so all are tested. All commands hold their name, long and short descriptions (invoked with positional help command). Due to small scope of program, all commands are added by ModuleInitializer.

## Sudoku Transformations (GraphicalInterface only)

Sudoku transformations are used to perform random transformations on sudoku that doesn't change solvability (mirror, flip, change digits, change row blocks, column blocks). They are used to change sudokus to form sudoku different looking from the original one. Row and column blocks are done by randomly swapping rows and columns in the same block. 

# Notable Data Structures

## Sudoku
All sudoku representations in all three projects are represented by int array of 81 numbers (0-9). 0 represents empty cell. They are accompanied by methods needed in each project. Important methods are described in Notable Used Algorithms section.
### Cell
In sudoku solver, where simple int array is not enough, we use Cell class to represent each cell in sudoku. It holds the value of the cell and all possible values that can be in it. It is used to check for naked and hidden values.
### Container
In sudoku solver, Container class represents rows, columns and squares. It holds all cells in it and is used to check for hidden values and perform intersection and exclusion operations on cells.
### SudokuString
SudokuString is string representation of sudoku. It is a string of 81 numbers (0-9). It is used to parse inputted sudoku from string to int array. And it can be copied to clipboard.
### SudokuSaveFile
It is a file that holds multiple sudokus in it. It is used to save and load multiple sudokus at once. It is a text file with each sudoku on a new line. Each sudoku is represented by SudokuString.
### Command
Command class represents commands in the CLI. It holds values of its method, aliases and info. It is used to parse inputted commands from string to Command object. It is used to execute commands in the CLI. 

# Discussion

In this section, we will discuss the design choices made in the program, the challenges faced during development, and potential future improvements.

## Design Choices
The program consists of four separate projects, which was meant to separate programs into logical parts, but looking back, it might have been an overkill for such a small project. The enlargement of project with graphical interface may have been done in a hurry and made the code less readable as there should have been united interface for both CLI and GUI.

## Testing
The program might benefit from more extensive unit testing. Currently, the testing is done manually.

## Future Improvements
Potential future improvements include optimizing the Sudoku generation process as most algorithms are not optimized and use brute force. Also, the GUI could be improved to make it more user-friendly and faster as updating the sudoku in the GUI is slow and bottleneck to the program. Additionally, more advanced Sudoku solving techniques could be implemented to improve the solving capabilities of the program.

## Conclusion
In conclusion, the program is a complete and functional Sudoku generator and solver. It provides CLI and GUI for users to interact with the program. It is a good starting point for anyone interested in Sudoku generation and solving. Project can be further improved and optimized to be more efficient, use more solving techniques and be more user-friendly.
