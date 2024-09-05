using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WEBCodigo.Models;

namespace WEBCodigo.Controllers
{
    public class CustomersController : Controller
    {
        private readonly HttpClient _httpClient;

        public CustomersController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            string url = "https://localhost:7227/api/Customers/GetByFilters";
            List<Customer> customers = new List<Customer>();

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
    }
}
