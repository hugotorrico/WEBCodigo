using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using WEBCodigo.Models;

namespace WEBCodigo.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController()
        {
            _httpClient = new HttpClient();
        }

        // Acción para mostrar la página de login
        public IActionResult Login()
        {
            return View();
        }

        // Acción para procesar el formulario de login
        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                
                string token = await GetJwtTokenAsync( model.UserName, model.Password);

                if (!string.IsNullOrEmpty(token))
                {
                    // Guardar el token en la sesión
                  
                    HttpContext.Session.SetString("JwtToken", token);
                   

                    // Redirigir a una página segura (por ejemplo, el dashboard)
                    return RedirectToAction("Index", "Customers");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View(model);
        }

        // Función para obtener el token JWT desde el API
        private async Task<string> GetJwtTokenAsync(string username, string password)
        {
            string url = "https://localhost:7227/security/createToken";
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
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<string>(responseContent);
                return tokenData;
            }

            return null;
        }

       

       
    }
}
