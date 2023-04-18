using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace WeatherForecast_Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly string apiKey = "7df1d178ee2ca256eb45382ff4d6dd0b"; // OpenWeatherMap API key

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetTemperature(string city)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={apiKey}";
                    var response = await client.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<OpenWeatherMapResult>(content);
                    if (result.Main != null)
                    {
                        ViewBag.Temperature = result.Main.Temp;
                        ViewBag.FeelsLike = result.Main.FeelsLike;
                        ViewBag.Speed = result.Wind.Speed;
                    }
                    else
                    {
                        ViewBag.Error = "Could not retrieve temperature.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View("Index");
        }
    }

    public class OpenWeatherMapResult
    {
        [JsonProperty("main")]
        public Main Main { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }
    }

    public class Main
    {
        [JsonProperty("temp")]
        public decimal Temp { get; set; }

        [JsonProperty("feels_like")]
        public decimal FeelsLike { get; set; }
    }

    public class Wind
    {
        [JsonProperty("speed")]
        public decimal Speed { get; set; }

        //[JsonProperty("deg")]
        //public decimal Direction { get; set; }
    }
}
