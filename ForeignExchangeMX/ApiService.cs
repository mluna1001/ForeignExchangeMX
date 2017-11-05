namespace ForeignExchangeMX
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using ForeignExchangeMX.Helpers;
    using ForeignExchangeMX.Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;

    public class ApiService
    {
        public async Task<Response> CheckConnection(){
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = Lenguages.SettingInternetError
                };
            }

            var response = await CrossConnectivity.Current.IsRemoteReachable("http://www.google.com");
            if (!response)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = Lenguages.ConnectionInternetError
                };
            }

            return new Response
            {
                IsSucess = true
            };
        }

        public async Task<Response> GetList<T>(string UrlBase, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new
                    Uri(UrlBase);
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSucess = false,
                        Message = result
                    };
                }

                var list = JsonConvert.DeserializeObject<List<Rate>>(result);

                return new Response
                {
                    IsSucess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
