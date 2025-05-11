using System;

namespace Task3.Models
{
    public class Part
    {
        public int Id { get; }
        public string Name { get; }
        public DateTime ProductionTime { get; }

        public Part(int id, string name)
        {
            Id = id;
            Name = name;
            ProductionTime = DateTime.Now;
        }

        public override string ToString() =>
            $"{Name} #{Id} (произведена {ProductionTime:HH:mm:ss})";
    }
}