namespace ForeignExchangeMX.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using ForeignExchangeMX.Models;
    using GalaSoft.MvvmLight.Command;

    public class MainViewModel
    {
        #region Properties
        public string Amount
        {
            get;
            set;
        }

        public ObservableCollection<Rate> Rates
        {
            get;
            set;
        }

        public Rate SourceRate
        {
            get;
            set;
        }

        public Rate TargetRate
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get;
            set;
        }

        public string Result
        {
            get;
            set;
        }

        #endregion


        public MainViewModel()
        {
        }

        #region Commands
        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }
        }

        private void Convert()
        {
            
        }
        #endregion
    }
}
