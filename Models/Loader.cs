using System;
using System.Threading;

namespace Task3.Models
{
    public class Loader
    {
        public event Action<string>? OperationStatusChanged;

        public string Name { get; }
        public int LoadedPartsCount { get; private set; }

        public Loader(string name) => Name = name;

        public void LoadPart(Part part)
        {
            if (part == null) return;

            OperationStatusChanged?.Invoke($"{Name}: Начал погрузку {part}");

            Thread.Sleep(300); // Имитация погрузки

            LoadedPartsCount++;
            OperationStatusChanged?.Invoke($"{Name}: Загрузил {part} (всего: {LoadedPartsCount})");
        }
    }
}