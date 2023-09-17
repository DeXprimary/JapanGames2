using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JapanGames2.ViewModel
{
    internal class MainMenuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand startSudokuGameCommand { get; }

        private string someString;
        public string SomeString
        {
            get => someString;

            set
            {
                someString = value;

                OnPropertyChanged("SomeString");
            }
        }

        private void AddToCounter() => counter += 50;

        internal MainMenuViewModel()
        {
            startSudokuGameCommand = new Command(AddToCounter);

            DoSome();

            
        }

        int counter = 0;

        async void DoSome()
        {
            

            while (true)
            {
                await Task.Delay(1000);

                counter++;

                SomeString = counter.ToString();
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
