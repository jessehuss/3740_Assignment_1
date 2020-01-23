using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RML_Parser
{
    class Program
    {
        public static List<int> _registers {get;set;}
        public static bool _done = false;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Jesse's RML Emulator!\n\n" +
                "This program will allow you to import an RML program in the form of a .rml file.\n" +
                "Or use on of our pre-defined expamples.\n" +
                "An example of acceptable formatting is as follows:\n\t" +
                "DEB 1 2 3\n\t" +
                "INC 2 3\n\t" +
                "HALT\n" +
                "Now without further ado, let us begin!\n\n");

            var inputAccepted = false;            

            // Command
            while (!_done)
            {
                _registers = new List<int>();
                var program = ImportAndValidateRML(inputAccepted);
                if (_done)
                    break;
                inputAccepted = false;
                ExecuteProgram(program);
            }            
        }
        static void InitializeRegisters(List<List<int>> program)
        {
            // GET MAX HIGHEST REGISTER USED SO WE CAN INITIALIZE ONLY THAT MANY
            var maxReg = program.OrderByDescending(x => x[1]).First()[1];

            for (int i = 0; i < maxReg + 1; i++)
            {
                var validInput = false;
                while (!validInput)
                {
                    Console.Write($"Enter a value to initialize register {i + 1} (default = 0):");
                    var resp = Console.ReadLine();
                    if (int.TryParse(resp, out int regVal))
                    {
                        _registers.Add(regVal);
                        validInput = true;
                    }
                    else if (string.IsNullOrEmpty(resp))
                    {
                        _registers.Add(0);
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid input...\n");
                    }
                }
            }
        }
        // PRETTIFY
        static void PrintRegisters(bool init = false)
        {
            if (init)
                Console.WriteLine();
            for (int i = 0; i < _registers.Count; i++)
            {
                if (init)
                    Console.Write($"Reg {i + 1}\t");
                else
                    Console.Write($"{_registers[i]}\t");
            }
            Console.WriteLine();
        }

        static void ExecuteProgram(List<List<int>> program)
        {
            var PC = 0;
            PrintRegisters(true);

            while (program[PC].First() != (int)Instruction.HALT)
            {
                // INSTRUCTION EXECUTION
                switch (program[PC].First())
                {
                    case (int)Instruction.INC:
                        PrintRegisters();
                        _registers[program[PC][1]]++;
                        PC = program[PC][2];
                        break;
                    case (int)Instruction.DEB:
                        if (_registers[program[PC][1]] > 0)
                        {
                            PrintRegisters();
                            _registers[program[PC][1]]--;
                            PC = program[PC][2];
                        }
                        else
                        {
                            PrintRegisters();
                            PC = program[PC][3];
                        }
                        break;
                    default:
                        break;
                }
            }
            PrintRegisters();
        }
        static List<List<int>> ConvertRMLToNumberSequence(List<string> programAsStrings)
        {
            // OUTER LIST ACTS AS PROGRAM, INNER LIST ACTS AS REGISTER STORING INSTRUCTIONS
            var programAsNumberSequence = new List<List<int>>();

            foreach (var line in programAsStrings)
            {
                var register = new List<int>();
                var lineSplit = line.Split(' ');

                Enum.TryParse(lineSplit.First(), out Instruction instruction);
                register.Add((int)instruction);

                for (int i = 1; i < 4; i++)
                    register.Add(i < lineSplit.Length ? int.Parse(lineSplit[i]) - 1 : 0);

                programAsNumberSequence.Add(register);
            }

            return programAsNumberSequence;
        }
        static List<List<int>> ImportAndValidateRML(bool inputAccepted)
        {
            var program = new List<string>();

            while (!inputAccepted)
            {
                Console.Write("\nPlease provide input in the form of the following\n" +
                    "MULT {int} {int}\n" +
                    "DIV {int} {int} \n" +
                    "CNT {int}\n" +
                    "Or in the form of a filepath\n" +
                    "Or quit to Quit the emulator\n" +
                    "Input:");
                var input = Console.ReadLine();
                Console.WriteLine();
                if (input.ToUpper().StartsWith("MULT"))
                {
                    var args = input.Split(' ');
                    if (args.Length != 3)
                    {
                        Console.WriteLine("Invalid number of arguments...");
                        inputAccepted = false;
                    }
                    else
                    {
                        if (!int.TryParse(args[1], out int r1) || !int.TryParse(args[2], out int r2))
                        {
                            Console.WriteLine("Invalid: 2nd and 3rd arguments must be integer values");
                            inputAccepted = false;
                        }
                        else
                        {
                            _registers.Add(r1);
                            _registers.Add(r2);
                            _registers.Add(0);
                            _registers.Add(0);
                            program = ReadInFile("MULT");
                            inputAccepted = true;
                        }
                    }
                }
                else if (input.ToUpper().StartsWith("DIV"))
                {
                    var args = input.Split(' ');
                    if (args.Length != 3)
                    {
                        Console.WriteLine("Invalid number of arguments...");
                        inputAccepted = false;
                    }
                    else
                    {
                        if (!int.TryParse(args[1], out int r1) || !int.TryParse(args[2], out int r2))
                        {
                            Console.WriteLine("Invalid: 2nd and 3rd arguments must be integer values");
                            inputAccepted = false;
                        }
                        else
                        {
                            _registers.Add(r1);
                            _registers.Add(r2);
                            _registers.Add(0);
                            _registers.Add(0);
                            _registers.Add(0);
                            _registers.Add(0);
                            program = ReadInFile("DIV");
                            inputAccepted = true;
                        }
                    }
                }
                else if (input.ToUpper().StartsWith("CNT"))
                {
                    var args = input.Split(' ');
                    if (args.Length != 2)
                    {
                        Console.WriteLine("Invalid number of arguments...");
                        inputAccepted = false;
                    }
                    else
                    {
                        if (!int.TryParse(args[1], out int r1))
                        {
                            Console.WriteLine("Invalid: 2nd argument must be an integer value");
                            inputAccepted = false;
                        }
                        else
                        {
                            _registers.Add(0);
                            _registers.Add(r1);
                            _registers.Add(0);
                            _registers.Add(0);
                            _registers.Add(0);
                            program = ReadInFile("CNT");
                            inputAccepted = true;
                        }
                    }
                }
                else if (input.ToUpper() == "QUIT")
                {
                    Console.WriteLine("Goodbye...");
                    inputAccepted = true;
                    _done = true;
                    return null;
                }
                else
                {
                    program = ManualFile(input);
                    inputAccepted = program != null;
                }
            }

            var programAsNumberSequence = ConvertRMLToNumberSequence(program);
            if (_registers.All(x => x == 0))
                InitializeRegisters(programAsNumberSequence);

            return programAsNumberSequence;
        }
        static List<string> ReadInFile(string input)
        {
            var path = "";
            switch (input.ToUpper())
            {
                case "MULT":
                    path = "..\\..\\RML\\Multiply.rml";
                    break;
                case "DIV":
                    path = "..\\..\\RML\\Divide.rml";
                    break;
                case "CNT":
                    path = "..\\..\\RML\\CNT.rml";
                    break;
                default:
                    break;
            }
            var program = new List<string>();
            using (var sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    program.Add(line);
                }
            }
            return program;
        }
        static List<string> ManualFile(string input)
        {
            var program = new List<string>();
            if (File.Exists(input) && input.EndsWith(".rml"))
            {
                using (var sr = new StreamReader(input))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        program.Add(line);
                    }
                }
                var programValid = true;
                foreach (var instruction in program)
                {
                    Console.Write(instruction);

                    var inst = instruction.Split(' ');
                    // VALIDATE PROGRAM INSTRUCTIONS: MAX 4 PARTS PER INSTRUCTION, 
                    // FIRST MUST BE A STRING OF 3-4 CHARS, REMAINING MUST BE INT PARSABLE
                    if (!(inst.Length > 0 && inst.Length < 5))
                    {
                        programValid = false;
                        Console.Write($" < Error: Incorrect number of arguments at line: {program.IndexOf(instruction) + 1}\n");
                        continue;
                    }
                    if (!new List<string>() { "HALT", "DEB", "INC" }.Contains(inst.First().ToUpper()))
                    {
                        programValid = false;
                        Console.Write($" < Error: Invalid instruction at line: {program.IndexOf(instruction) + 1}\n");
                        continue;
                    }
                    for (int i = 1; i < inst.Length; i++)
                    {
                        if (!int.TryParse(inst[i], out int n))
                        {
                            programValid = false;
                            Console.Write($" < Error: Invalid register or instruction line at line: {program.IndexOf(instruction) + 1}, position: {i + 1}");
                            continue;
                        }
                    }
                    Console.WriteLine();
                }
                if (programValid)
                    return program;
                else
                    Console.WriteLine("\nRML Program error... Your code will not execute. Please fix any errors and try again...\n");
            }
            else
            {
                Console.WriteLine("Invalid File. Please try again...\n");
            }
            return null;
        }
    }

    enum Instruction
    {
        HALT = 0,
        INC = 1,
        DEB = 2
    }
}
