using System;

namespace ProcessorSimulator
{
    class Processor
    {
        public static Registry R1 = new Registry("R1");
        public static Registry R2 = new Registry("R2");
        public static Registry R3 = new Registry("R3");
        public static Registry R4 = new Registry("R4");

        public static bool LogicValue = false;
        public static bool JumpValue = false;

        public static void Load(Registry r1, Registry r2)
        {
            r1.Value = r2.Value;
        }

        public static void Load(Registry r, Memory m)
        {
            r.Value = m.Value;
        }

        public static void Store(Registry r, Memory m)
        {
            m.Value = r.Value;
        }

        public static void Add(Registry r1, Registry r2)
        {
            r1.Value = r1.Value + r2.Value;
        }

        public static void Add(Registry r, Memory m) 
        {
            r.Value = r.Value + m.Value;
        }

        public static void Subtract(Registry r1, Registry r2)
        {
            r1.Value = r1.Value - r2.Value;
        }

        public static void Subtract(Registry r, Memory m)
        {
            r.Value = r.Value - m.Value;
        }

        public static void And(Registry r1, Registry r2)
        {
            LogicValue = r1.Value != 0 && r2.Value != 0;
            Console.WriteLine("Result: " + LogicValue);
            Console.WriteLine();
        }

        public static void Or(Registry r1, Registry r2)
        {
            LogicValue = r1.Value != 0 || r2.Value != 0;
            Console.WriteLine("Result: " + LogicValue);
            Console.WriteLine();
        }

        public static void Not(Registry r)
        {
            LogicValue = (r.Value == 0);
            Console.WriteLine("Result: " + LogicValue);
            Console.WriteLine();
        }
    
        public static void JE(Registry r1, Registry r2)
        {
            JumpValue = r1.Value == r2.Value;
        }

        public static void JNE(Registry r1, Registry r2)
        {
            JumpValue = r1.Value != r2.Value;
        }

        public static void JG(Registry r1, Registry r2)
        {
            JumpValue = r1.Value > r2.Value;
        }

        public static void JL(Registry r1, Registry r2)
        {
            JumpValue = r1.Value < r2.Value;
        }

        public static void ShowAllRegistries()
        {
            Console.WriteLine("\nRegistries: ");
            Console.WriteLine("R1: " + R1.Value);
            Console.WriteLine("R2: " + R2.Value);
            Console.WriteLine("R3: " + R3.Value);
            Console.WriteLine("R4: " + R4.Value);
            Console.WriteLine();
        }
    }
}
