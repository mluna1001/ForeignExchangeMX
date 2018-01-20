namespace ForeignExchangeMX.Services
{
    using System;
    using System.Threading.Tasks;
    using Helpers;
    using Xamarin.Forms;

    public class DialogService
    {
        public async Task ShowMessage(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                Lenguages.Accept);
        }
    }
}
