# 3740_Assignment_1
RML Emulator

## Description
- Emulator will read in an rml file of a RML program or use one of three predefined .rml files (MULT, DIV, CNT)
- The emulator verifies the syntax and will report any errors that would cause the RML program to not run
- The emulator dynamically defines the appropriate number of registers
- The emulator asks the user to initialize the registers with starting values: Default is 0 unless a predefined .rml was chosed.
- Upon execution of the RML program the emulator outputs step by step register data.

## Functionality
Prebuilt fucntionality allows for the user to test the following:
```bash
MULT {int} {int} #Multiply Function
DIV {int} {int} #Divide Function
CNT {int} #Reg 1 contains number of 1's in binary expansion
//filepath #Allows users to test their own .rml
QUIT #Quits the program
```
## Usage
For Windows:
- Within the project directory navigate to obj/Release
```bash
cd RML Parser/obj/Release
"RML Parser.exe"
```
For Linux:
- Has not been thoroughly tested as there was no requirement for the program to run on Linux but should be able to run by doing the following
```bash
wine /path/to/application.exe
```
## Contents
- RML Parser/RML Parser/obj/Release/RML Parser.exe #Executable
- RML Parser/RML Parser/Program.cs #Code
- RML Files/Divide.rml
- RML Files/Multiply.rml
- RML Files/CNT.rml 
- README.md
- RML_A1_Report.pdf
## Contributors
Jesse Huss

## License
[UNLICENCE](https://choosealicense.com/licenses/unlicense/)
