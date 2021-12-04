namespace ProcessorSimulator
{
    class Registry
    {
        public string Name { get; private set; }
        public long Value { get; set; }

        public Registry(string name)
        {
            this.Name = name;
            this.Value = 0;
        }
    }
}
