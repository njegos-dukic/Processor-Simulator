using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessorSimulator
{
    class Memory
    {
        private static ulong nextAddress = 1;
        public static List<Memory> takenMemoryLocations = new List<Memory>();

        public ulong Address { get; private set; }
        public string Identifier { get; private set; }
        public long Value { get; set; }

        public Memory(string identifier, long value = 0)
        {
            this.Identifier = identifier;
            this.Value = value;

            this.Address = nextAddress;
            nextAddress++;

            takenMemoryLocations.Add(this);
        }

        public Memory(long value)
        {
            this.Value = value;
            this.Address = 0;
        }

        public static bool IsMemoryLocation(string identifier)
        {
            return takenMemoryLocations.Exists(m => m.Identifier == identifier);
        }

        public static Memory GetMemoryLocation(string identifier)
        {
            return takenMemoryLocations.First(m => m.Identifier == identifier);
        }

        public static void UpdateMemoryLocation(string identifier, long value)
        {
            takenMemoryLocations.First(m => m.Identifier == identifier).Value = value;
        }

        public static void ShowAllMemoryLocations()
        {
            Console.WriteLine("Memory: ");

            foreach (var memory in takenMemoryLocations)
                Console.WriteLine(memory.Address + " " + memory.Identifier + " " + memory.Value);

            Console.WriteLine();
        }
    }
}
