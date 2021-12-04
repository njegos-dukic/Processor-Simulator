using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ProcessorSimulator
{
    class Interpreter
    {
        static void Main(string[] args)
        {
            string[] program = null;
            string programPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar + "program.txt";
            if (File.Exists(programPath))
                program = File.ReadAllLines(programPath);

            bool active = true;

            if (program != null && program.Length > 0)
            {
                Console.WriteLine("Press enter to continue or type in \"show\" to inspect memory.\n");

                for (int i = 0; i < program.Length; i++)
                {
                    string interpreted = InterpretProgram(program[i].Trim().ToLower());
                    if (interpreted == "")
                    {
                        Console.Write("#" + (i + 1) + " >> ");
                        var debug = Console.ReadLine().Trim().ToLower();

                        if ("show".Equals(debug))
                        {
                            Processor.ShowAllRegistries();
                            Memory.ShowAllMemoryLocations();
                        }
                    }

                    else
                    {
                        for (int j = 0; j < program.Length; j++)
                            if (program[j].Trim().ToLower() == interpreted.Trim().ToLower())
                                i = j;
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine("Interpreter started.\n");

            while (active)
            {
                System.Console.Write(">> ");
                string instruction = System.Console.ReadLine().Trim().ToLower();
               
                if ("end".Equals(instruction))
                    active = false;

                else
                    Interpret(instruction);
            }
        }

        private static void Interpret(string instruction)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            instruction = regex.Replace(instruction, " ");
            string[] parameters = instruction.Split(' ');

            if ("declare".Equals(parameters[0]) && parameters.Length == 3)
            {
                if (long.TryParse(parameters[2], out long n))
                {
                    if (Memory.IsMemoryLocation(parameters[1]))
                        Memory.UpdateMemoryLocation(parameters[1], n);
                    else
                        _ = new Memory(parameters[1], n);
                }
            }

            else if ("read".Equals(parameters[0]) && (IsRegistry(parameters[1]) || Memory.IsMemoryLocation(parameters[1])))
            {
                if (IsRegistry(parameters[1]))
                    Console.WriteLine(parameters[1] + ": " + GetRegistryFromID(parameters[1]).Value);
                else
                    Console.WriteLine(parameters[1] + ": " + Memory.GetMemoryLocation(parameters[1]).Value);

                Console.WriteLine();
            }

            else if ("load".Equals(parameters[0]) && parameters.Length == 3 && IsRegistry(parameters[1]) && (IsRegistry(parameters[2]) || Memory.IsMemoryLocation(parameters[2]) || long.TryParse(parameters[2], out long _)))
            {
                Registry registry = GetRegistryFromID(parameters[1]);

                if (registry != null)
                {
                    if (IsRegistry(parameters[2]))
                        Processor.Load(registry, GetRegistryFromID(parameters[2]));

                    else if (Memory.IsMemoryLocation(parameters[2]))
                        Processor.Load(registry, Memory.GetMemoryLocation(parameters[2]));
                    else
                    {
                        long.TryParse(parameters[2], out long n);
                        Processor.Load(registry, new Memory(n));
                    }
                }
            }

            else if ("add".Equals(parameters[0]) && parameters.Length == 3 && IsRegistry(parameters[1]) && (IsRegistry(parameters[2]) || Memory.IsMemoryLocation(parameters[2]) || long.TryParse(parameters[2], out long _)))
            {
                Registry registry = GetRegistryFromID(parameters[1]);

                if (registry != null)
                {
                    if (IsRegistry(parameters[2]))
                        Processor.Add(registry, GetRegistryFromID(parameters[2]));

                    else if (Memory.IsMemoryLocation(parameters[2]))
                        Processor.Add(registry, Memory.GetMemoryLocation(parameters[2]));
                    else
                    {
                        long.TryParse(parameters[2], out long n);
                        Processor.Add(registry, new Memory(n));
                    }
                }
            }

            else if ("subtract".Equals(parameters[0]) && parameters.Length == 3 && IsRegistry(parameters[1]) && (IsRegistry(parameters[2]) || Memory.IsMemoryLocation(parameters[2]) || long.TryParse(parameters[2], out long _)))
            {
                Registry registry = GetRegistryFromID(parameters[1]);

                if (registry != null)
                {
                    if (IsRegistry(parameters[2]))
                        Processor.Subtract(registry, GetRegistryFromID(parameters[2]));

                    else if (Memory.IsMemoryLocation(parameters[2]))
                        Processor.Subtract(registry, Memory.GetMemoryLocation(parameters[2]));
                    else
                    {
                        long.TryParse(parameters[2], out long n);
                        Processor.Subtract(registry, new Memory(n));
                    }
                }
            }

            else if ("and".Equals(parameters[0]) && parameters.Length == 3 && IsRegistry(parameters[1]) && IsRegistry(parameters[2]))
            {
                Registry registry1 = GetRegistryFromID(parameters[1]);
                Registry registry2 = GetRegistryFromID(parameters[2]);

                if (registry1 != null && registry2 != null)
                    Processor.And(registry1, registry2);
            }

            else if ("or".Equals(parameters[0]) && parameters.Length == 3 && IsRegistry(parameters[1]) && IsRegistry(parameters[2]))
            {
                Registry registry1 = GetRegistryFromID(parameters[1]);
                Registry registry2 = GetRegistryFromID(parameters[2]);

                if (registry1 != null && registry2 != null)
                    Processor.Or(registry1, registry2);
            }

            else if ("not".Equals(parameters[0]) && parameters.Length == 2 && IsRegistry(parameters[1]))
            {
                Registry registry = GetRegistryFromID(parameters[1]);

                if (registry != null)
                    Processor.Not(registry);
            }
        }

        public static string InterpretProgram(string instruction)
        {
            Interpret(instruction);

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            instruction = regex.Replace(instruction, " ");
            string[] parameters = instruction.Split(' ');

            if ("je".Equals(parameters[0]) && parameters.Length == 4 && IsRegistry(parameters[1]) && IsRegistry(parameters[2]))
            {
                Registry registry1 = GetRegistryFromID(parameters[1]);
                Registry registry2 = GetRegistryFromID(parameters[2]);

                if (registry1 != null && registry2 != null)
                    Processor.JE(registry1, registry2);

                if (Processor.JumpValue)
                    return parameters[3];
            }

            else if ("jne".Equals(parameters[0]) && parameters.Length == 4 && IsRegistry(parameters[1]) && IsRegistry(parameters[2]))
            {
                Registry registry1 = GetRegistryFromID(parameters[1]);
                Registry registry2 = GetRegistryFromID(parameters[2]);

                if (registry1 != null && registry2 != null)
                    Processor.JNE(registry1, registry2);

                if (Processor.JumpValue)
                    return parameters[3];
            }

            else if ("jg".Equals(parameters[0]) && parameters.Length == 4 && IsRegistry(parameters[1]) && IsRegistry(parameters[2]))
            {
                Registry registry1 = GetRegistryFromID(parameters[1]);
                Registry registry2 = GetRegistryFromID(parameters[2]);

                if (registry1 != null && registry2 != null)
                    Processor.JG(registry1, registry2);

                if (Processor.JumpValue)
                    return parameters[3];
            }

            else if ("jl".Equals(parameters[0]) && parameters.Length == 4 && IsRegistry(parameters[1]) && IsRegistry(parameters[2]))
            {
                Registry registry1 = GetRegistryFromID(parameters[1]);
                Registry registry2 = GetRegistryFromID(parameters[2]);

                if (registry1 != null && registry2 != null)
                    Processor.JL(registry1, registry2);

                if (Processor.JumpValue)
                    return parameters[3];
            }

            return "";
        }

        private static bool IsRegistry(string identifier)
        {
            return ("r1".Equals(identifier) || "r2".Equals(identifier) || "r3".Equals(identifier) || "r4".Equals(identifier));
        }

        private static Registry GetRegistryFromID(string identifier)
        {
            switch (identifier)
            {
                case "r1":
                    return Processor.R1;
                case "r2":
                    return Processor.R2;
                case "r3":
                    return Processor.R3;
                case "r4":
                    return Processor.R4;
                default:
                    return null;
            }
        }
    }
}
