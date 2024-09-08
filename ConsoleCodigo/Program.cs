using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using ConsoleCodigo;
using System.Net.Http.Headers;

class Program
{
    static async Task Main(string[] args)
    {

      
        string token= await GetJwtToken("admin1", "admin123");
        if (!string.IsNullOrEmpty(token))
        {
            Console.WriteLine("Token obtained successfully.");
            await GetCustomersWithToken(token);
        }
        else
        {
            Console.WriteLine("Failed to obtain token.");
        }

        //await GetCustomers();
        //await GetProducts();
        //await InsertCustomer();

        Console.Read();

    }

    private static async Task InsertCustomer()
    {
        using (HttpClient client = new HttpClient())
        {
            // URL del servicio
            string url = "https://localhost:7227/api/Customers/Insert";

            var customer = new
            {
                customerID = 0,
                name = "string2",
                documentNumber = "string2",
                documentType = "string2",
                isActive = true
            };

            // Serializar el objeto a JSON
            var json = JsonSerializer.Serialize(customer);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);



            // Verificar si la respuesta fue exitosa
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Customer inserted successfully.");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }

        }

    }


    private static async Task GetCustomers()
    {
        using (HttpClient client = new HttpClient())
        {
            // URL del servicio
            string url = "https://localhost:7227/api/Customers/GetByFilters";

            //Guardar response
            HttpResponseMessage response = await client.GetAsync(url);

            // Verificar si la respuesta fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Leer y deserializar el contenido de la respuesta
                List<Customer> customers = await response.Content.ReadFromJsonAsync<List<Customer>>();

                foreach (var customer in customers)
                {
                    Console.WriteLine($"ID: {customer.CustomerID}, Name: {customer.Name}, Document: {customer.DocumentNumber}, Type: {customer.DocumentType}, Active: {customer.IsActive}");
                }

            }
            else
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine("Llamada con error");

            }
        }
    }

    private static async Task GetProducts()
    {
        using (HttpClient client = new HttpClient())
        {
            // URL del servicio
            string url = "https://localhost:7227/api/Products/GetByFilters";

            //Guardar response
            HttpResponseMessage response = await client.GetAsync(url);

            // Verificar si la respuesta fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Leer y deserializar el contenido de la respuesta
                List<Product> products = await response.Content.ReadFromJsonAsync<List<Product>>();

                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.ProductID}, Name: {product.Name}, Price: {product.Price}");
                }

            }
            else
            {
                Console.WriteLine("Llamada con error");

            }
        }
    }

    private static async Task GetCustomersWithToken(string jwtToken)
    {
        //string jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjJkODA1MGY1LWQ0MTYtNDY0ZS1hMWJlLTI5MGNiZjkyNGZmNSIsInN1YiI6ImFkbWluIiwiZW1haWwiOiJhZG1pbiIsImp0aSI6ImQ5ZjFiNDNhLTg4NzktNGZmNy1iZDNiLTUzMmM2MWIxMGU4NSIsInJvbGUiOiJBZG1pbmlzdHJhdG9yIiwibmJmIjoxNzI1NzMzODgwLCJleHAiOjE3MjU3MzQxODAsImlhdCI6MTcyNTczMzg4MCwiaXNzIjoiaHR0cHM6Ly9odWdvdG9ycmljby8iLCJhdWQiOiJodHRwczovL2FsdW1ub3MvIn0.0K3WqFoQAhfiodUP3xXGLjH8AxZyLy4kJSEp2Nva57MXTfIEeTW1A0AluSujosI8hkhb-CbNc9uuzBumEMvnTQ";
        using (HttpClient client = new HttpClient())
        {
            // URL del servicio
            string url = "https://localhost:7227/api/Customers/GetByFilters";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            //Guardar response
            HttpResponseMessage response = await client.GetAsync(url);

            // Verificar si la respuesta fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Leer y deserializar el contenido de la respuesta
                List<Customer> customers = await response.Content.ReadFromJsonAsync<List<Customer>>();

                foreach (var customer in customers)
                {
                    Console.WriteLine($"ID: {customer.CustomerID}, Name: {customer.Name}, Document: {customer.DocumentNumber}, Type: {customer.DocumentType}, Active: {customer.IsActive}");
                }

            }
            else
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine("Llamada con error");

            }
        }
    }

    public static async Task<string> GetJwtToken(string username, string password)
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
                Console.WriteLine(tokenData);
                return tokenData;               
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return "";
            }

        }
    }

}

