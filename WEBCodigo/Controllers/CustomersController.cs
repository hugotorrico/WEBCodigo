using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WEBCodigo.Models;

namespace WEBCodigo.Controllers
{
    [JwtAuthorize]
    public class CustomersController : Controller
    {
        private readonly HttpClient _httpClient;

        public CustomersController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {            


                string sessionToken = HttpContext.Session.GetString("JwtToken");   
                string url = "https://localhost:7227/api/Customers/GetByFilters";
                List<Customer> customers = new List<Customer>();

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionToken);

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    customers = await response.Content.ReadFromJsonAsync<List<Customer>>();

                }
                else
                {
                    ViewBag.Error = $"Error: {response.StatusCode}";
                }


                return View(customers);
           
        }
    
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            string url = "https://localhost:7227/api/Customers/Insert";

            //Convert object to json
            var json = JsonSerializer.Serialize(customer);
            //Preparo el contenido
            var content = new StringContent(json, Encoding.UTF8, "application/json");
          
            // Realizar la solicitud POST
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Customer inserted successfully.";
            }
            else
            {
                ViewBag.Error = $"Error: {response.StatusCode}";
            }

            return View();

        }


        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public void Update(Customer customer)
        {

        }


        public async Task<IActionResult> IndexToken()
        {

            List<Customer> customers = new List<Customer>();
           //Obtener el token
            string token = await GetJwtToken("admin1","admin123");
           
            //Validar si el token es correro
            if (!string.IsNullOrEmpty(token))
            {

                string url = "https://localhost:7227/api/Customers/GetByFilters";

                //Asignar el token al httplcient
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //Llamar al API
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    customers = await response.Content.ReadFromJsonAsync<List<Customer>>();
                }
                else
                {
                    ViewBag.Error = $"Error: {response.StatusCode}";
                }

            }
            else
            {
                ViewBag.Error = "Failed to obtain JWT token.";

            }              
            return View(customers);
        }


        private  async Task<string> GetJwtToken(string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                // URL del servicio
                string url = "https://localhost:7227/security/createToken";

                var loginData = new
                {
                    userName = username,
                    password = password
                };

                // Serializar el objeto a JSON
                var json = JsonSerializer.Serialize(loginData);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                // Verificar si la respuesta fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenData = JsonSerializer.Deserialize<string>(responseContent);                   
                    return tokenData;
                }
                else
                {
                    return "";
                }

            }
        }


        private async Task<string> GetJwtTokenAsync(string url, string username, string password)
        {
            var loginData = new
            {
                userName = username,
                password = password
            };

            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                // Aquí puedes deserializar la respuesta JSON para obtener el token
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<string>(responseContent);
                return tokenData;
            }

            return null;
        }
    }
}
