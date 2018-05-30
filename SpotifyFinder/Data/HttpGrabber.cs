using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace SpotifyFinder.Data
{
    class HttpGrabber
    {

        private string BaseAddress = "https://api.spotify.com/v1/";
        public async Task MakeString()
        {
            var getData = JsonConvert.DeserializeObject<List<SpotifyData>>(await TestGet());
        }
        //metoda asynchroniczna - aplikacja wielowątkowa
        //przerzucamy na inny wątek procesora
        public async Task<string> TestGet()
        {
            string testRequest = "";

            try {
                    var request = HttpWebRequest.CreateHttp(BaseAddress +"?q = SzukanyUtwor & type = track");
                    //metoda jaką wykonamy
                    request.Method = WebRequestMethods.Http.Get;
                    //jaki typ zostanie zaakceptowany
                    //sprawdzamy to wchodząc w F12 w przeglądarce->Network
                    request.ContentType = "application / json; charset = utf - 8";
                    //Czekamy
                    await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse,
                    request.EndGetResponse, null)
                    .ContinueWith(task =>
                    {
                        var response = (HttpWebResponse)task.Result;
                        //jeśli strona zwróciła dane w odpowiednim rodzaju
                        if(response.StatusCode == HttpStatusCode.OK)
                        {
                            //wrzucamy do swojego stringa
                            //pobierając stream, dekodując go na UTF8 i dodając do stringa
                            StreamReader responseReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                            string responseData = responseReader.ReadToEnd();
                            testRequest = responseData.ToString();
                            //zamknięcie strumienia
                            responseReader.Close();
                        }
                        response.Close();
                    });
                }
            catch (Exception)
            {
                //just pass
            }
            return testRequest;
        }
           
            
        
    }
}
