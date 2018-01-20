using Xamarin.Forms;

[assembly: Dependency(typeof(ForeignExchangeMX.Droid.Implementations.Config))]
namespace ForeignExchangeMX.Droid.Implementations
{
    using System;
    using Interfaces;
    using SQLite.Net.Interop;

    public class Config : IConfig
    {
        private string directoryDB;
        private ISQLitePlatform platform;

        public string DirectoryDB
        {
            get
            {
                if (string.IsNullOrEmpty(directoryDB))
                {
                    directoryDB = Environment.GetFolderPath(
                        Environment.SpecialFolder.Personal);
                }
                return directoryDB;
            }
        }

        public ISQLitePlatform Platform
        {
            get
            {
                if (platform == null)
                {
                    platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
                }
                return platform;
            }
        }
    }
}
