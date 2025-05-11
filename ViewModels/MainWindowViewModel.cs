using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using CommunityToolkit.Mvvm.Input;
using Task3.Models;

namespace Task3.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly SynchronizationContext? _syncContext;
        private string _status = "Готов к работе";
        private int _machineSpeed = 1000;
        private int _machineCounter = 1;
        private int _operatorCounter = 1;
        private int _loaderCounter = 1;

        public ObservableCollection<MachineViewModel> Machines { get; } = new();
        public ObservableCollection<MillingOperator> Operators { get; } = new();
        public ObservableCollection<Loader> Loaders { get; } = new();
        public ObservableCollection<string> Logs { get; } = new();

        public IRelayCommand AddMachineCommand { get; }
        public IRelayCommand AddOperatorCommand { get; }
        public IRelayCommand AddLoaderCommand { get; }
        public IRelayCommand StartAllCommand { get; }
        public IRelayCommand StopAllCommand { get; }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public int MachineSpeed
        {
            get => _machineSpeed;
            set => SetProperty(ref _machineSpeed, value);
        }

        public MainWindowViewModel()
        {
            _syncContext = SynchronizationContext.Current;

            AddMachineCommand = new RelayCommand(AddMachine);
            AddOperatorCommand = new RelayCommand(AddOperator);
            AddLoaderCommand = new RelayCommand(AddLoader);
            StartAllCommand = new RelayCommand(StartAllMachines);
            StopAllCommand = new RelayCommand(StopAllMachines);
        }

        private void AddMachine()
        {
            var name = $"Станок {_machineCounter++}";
            var machine = new Machine(name);
            var vm = new MachineViewModel(machine);

            machine.OperationStatusChanged += msg => AddLog(msg);
            machine.PartProduced += part =>
            {
                foreach (var op in Operators)
                {
                    if (!op.IsBusy)
                    {
                        op.ProcessPart(part);
                        foreach (var loader in Loaders)
                        {
                            loader.LoadPart(part);
                        }
                        break;
                    }
                }
            };

            Machines.Add(vm);
            AddLog($"Добавлен {name}");
        }

        private void AddOperator()
        {
            var name = $"Фрезеровщик {_operatorCounter++}";
            var op = new MillingOperator(name);
            op.OperationStatusChanged += msg => AddLog(msg);
            Operators.Add(op);
            AddLog($"Добавлен {name}");
        }

        private void AddLoader()
        {
            var name = $"Погрузчик {_loaderCounter++}";
            var loader = new Loader(name);
            loader.OperationStatusChanged += msg => AddLog(msg);
            Loaders.Add(loader);
            AddLog($"Добавлен {name}");
        }

        private void StartAllMachines()
        {
            foreach (var vm in Machines)
            {
                vm.Machine.SetProductionSpeed(MachineSpeed);
                vm.Machine.StartWorking();
            }
            Status = "Все станки запущены";
            AddLog(Status);
        }

        private void StopAllMachines()
        {
            foreach (var vm in Machines)
            {
                vm.Machine.StopWorking();
            }
            Status = "Все станки остановлены";
            AddLog(Status);
        }

        private void AddLog(string message)
        {
            _syncContext?.Post(_ =>
            {
                var timestamp = DateTime.Now.ToString("HH:mm:ss");
                Logs.Insert(0, $"[{timestamp}] {message}");

                if (Logs.Count > 50) Logs.RemoveAt(Logs.Count - 1);
            }, null);
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}