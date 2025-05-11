using System;
using System.Threading;

namespace Task3.Models
{
    public class MillingOperator
    {
        public event Action<string>? OperationStatusChanged;

        public string Name { get; }
        public bool IsBusy { get; private set; }

        public MillingOperator(string name) => Name = name;

        public void ProcessPart(Part part)
        {
            if (part == null) return;

            IsBusy = true;
            OperationStatusChanged?.Invoke($"{Name}: Начал обработку {part}");

            Thread.Sleep(500); // Имитация обработки

            OperationStatusChanged?.Invoke($"{Name}: Завершил обработку {part}");
            IsBusy = false;
        }
    }
}