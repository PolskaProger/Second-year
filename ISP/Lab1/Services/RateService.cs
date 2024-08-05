using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class RateService : IRateService
    {
        private HttpClient client;

        public RateService(HttpClient client )
        {
            this.client = client;
            Debug.Write($"------------> {client.BaseAddress.AbsoluteUri}");
        }
        public async Task<IEnumerable<Rate>> GetRates(DateTime date)
        {
            //var client = new HttpClient();
            var response = await client.GetAsync($"{client.BaseAddress}?ondate={date.ToString("yyyy-MM-dd")}&periodicity=0");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Rate>>(responseBody);
        }
    }
}
