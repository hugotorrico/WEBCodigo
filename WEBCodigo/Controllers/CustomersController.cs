using Microsoft.AspNetCore.Mvc;
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
    }
}
