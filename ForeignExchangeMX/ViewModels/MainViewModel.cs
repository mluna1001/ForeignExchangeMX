namespace ForeignExchangeMX.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using ForeignExchangeMX.Helpers;
    using ForeignExchangeMX.Models;
    using ForeignExchangeMX.Services;
    using GalaSoft.MvvmLight.Command;

    public class MainViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiSevice;
        DialogService dialogService;
        DataService dataService;
        #endregion

        #region Attributes
        Rate _sourceRate;
        Rate _targetRate;
        bool _isEnabled;
        bool _isRunning;
        string _result;
        string _status;
        ObservableCollection<Rate> _rates;
        List<Rate> rates;
        #endregion

        #region Properties
        public string Amount
        {
            get;
            set;
        }

        public ObservableCollection<Rate> Rates
        {
            get
            {
                return _rates;
            }
            set
            {
                if (_rates != value)
                {
                    _rates = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Rates)));
                }
            }
        }

        public Rate SourceRate
        {
            get
            {
                return _sourceRate;
            }
            set
            {
                if (_sourceRate != value)
                {
                    _sourceRate = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(SourceRate)));
                }
            }
        }

        public Rate TargetRate
        {
            get
            {
                return _targetRate;
            }
            set
            {
                if (_targetRate != value)
                {
                    _targetRate = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(TargetRate)));
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                if (_result != value)
                {
                    _result = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Status)));
                }
            }
        }

        #endregion

        #region Constructors
        public MainViewModel()
        {
            apiSevice = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            LoadRates();
        }
        #endregion

        #region Methods
        async void LoadRates()
        {
            IsRunning = true;
            Result = Lenguages.Loading;

            var connection = await apiSevice.CheckConnection();

            if (!connection.IsSucess)
            {
                //IsRunning = false;
                //Result = connection.Message;
                //return;
                LoadLocalData();
            }
            else
            {
                await LoadDataFromAPI();
            }

            if (rates.Count == 0)
            {
                IsRunning = false;
                IsEnabled = false;
                Result = Lenguages.ErrorLoad;
            }

            Rates = new ObservableCollection<Rate>(rates);
            IsRunning = false;
            IsEnabled = true;
            Result = Lenguages.Ready;
            //Status = Lenguages.Status;

        }

        void LoadLocalData()
        {
            rates = dataService.Get<Rate>(false);
            Status = Lenguages.LocalDataResult;
        }

        async Task LoadDataFromAPI()
        {
            //var url = Application.Current.Resources["URLAPI"].ToString();
            var url = "http://apiexchangerates.azurewebsites.net";

            var response = await apiSevice.GetList<Rate>(
                url,
                "api/rates");

            if (!response.IsSucess)
            {
                //IsRunning = false;
                //Result = response.Message;
                LoadLocalData();
                return;
            }

            //Storage data local
            rates = (List<Rate>)response.Result;
            dataService.DeleteAll<Rate>();
            dataService.Save(rates);
            Status = Lenguages.APIDataResult;
        }
        #endregion

        #region Commands
        public ICommand SwitchCommand
        {
            get
            {
                return new RelayCommand(Switch);
            }
        }

        void Switch()
        {
            var aux = SourceRate;
            SourceRate = TargetRate;
            TargetRate = aux;
            Convert();
        }

        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }
        }

        async void Convert()
        {
            decimal amount = 0;

            if (string.IsNullOrEmpty(Amount))
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.AmmountValidation
                );

                /*await Application.Current.MainPage.DisplayAlert(
                    Lenguages.Error,
                    Lenguages.AmmountValidation,
                    Lenguages.Accept
                );*/
                return;
            }
            if (!decimal.TryParse(Amount, out amount))
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.AmountNumericValidation
                );
                return;
            }
            if (SourceRate == null)
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.SourceRateValidation
                );
                return;
            }
            if (TargetRate == null)
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.TargetRateValidation
                );
                return;
            }

            var amountConverted = amount /
                    (decimal)SourceRate.TaxRate * (decimal)TargetRate.TaxRate;

            Result = string.Format("{0} {1:C2} = {2} {3:C2}",
                                   SourceRate.Code,
                                   amount,
                                   TargetRate.Code,
                                   amountConverted);

        }
        #endregion
    }
}
