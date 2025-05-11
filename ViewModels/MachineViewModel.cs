using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using Task3.Models;
using System.Runtime.CompilerServices;

namespace Task3.ViewModels
{
    public class MachineViewModel : INotifyPropertyChanged
    {
        public Machine Machine { get; }
        public string Name => Machine.Name;
        public int ProducedPartsCount => Machine.ProducedParts.Count;

        public MachineViewModel(Machine machine)
        {
            Machine = machine;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}