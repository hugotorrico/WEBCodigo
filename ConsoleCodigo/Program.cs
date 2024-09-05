using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using ConsoleCodigo;

class Program
{
    static async Task Main(string[] args)
    {
        await GetCustomers();
        Console.Read();

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
                Console.WriteLine("Llamada con error");

            }
        }
    }

}
