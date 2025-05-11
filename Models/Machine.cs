using System;
using System.Collections.ObjectModel;
using System.Threading;
using Task3.Models;

namespace Task3.Models
{
    public class Machine
    {
        public event Action<Part>? PartProduced;
        public event Action<string>? OperationStatusChanged;

        private readonly Random _random = new();
        private bool _isWorking;
        private int _productionSpeed = 1000;
        private int _partCounter;

        public string Name { get; }
        public ObservableCollection<Part> ProducedParts { get; } = new();

        public Machine(string name)
        {
            Name = name;
        }

        public void SetProductionSpeed(int speedMs) => _productionSpeed = speedMs;

        public void StartWorking()
        {
            if (_isWorking) return;

            _isWorking = true;
            ThreadPool.QueueUserWorkItem(_ => ProductionProcess());
        }

        public void StopWorking() => _isWorking = false;

        private void ProductionProcess()
        {
            OperationStatusChanged?.Invoke($"{Name}: Станок запущен");

            while (_isWorking)
            {
                try
                {
                    Thread.Sleep(_productionSpeed + _random.Next(-200, 200));

                    if (!_isWorking) break;

                    var part = new Part(++_partCounter, $"Деталь от {Name}");
                    ProducedParts.Add(part);
                    OperationStatusChanged?.Invoke($"{Name}: Произведена {part}");
                    PartProduced?.Invoke(part);
                }
                catch (Exception ex)
                {
                    OperationStatusChanged?.Invoke($"{Name}: Ошибка - {ex.Message}");
                }
            }

            OperationStatusChanged?.Invoke($"{Name}: Станок остановлен");
        }
    }
}