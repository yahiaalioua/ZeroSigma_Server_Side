using AngularAuthApi.DcfCalculator.Abstract;
using AngularAuthApi.DcfCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace AngularAuthApi.DcfCalculator.HttpClient
{
    public class FinancialPrepHttpCalls : IFinancialPrepHttpCalls
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FinancialPrepHttpCalls(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetIncomeStatements(string Ticker)
        {
            var client=_httpClientFactory.CreateClient("FinancialApi");
            
            return await client.GetStringAsync($"income-statement/{Ticker}?limit=5&apikey=9e79e541240a16005f1940b4b6984242");
        }
        public async Task<string> GetBalanceSheet(string Ticker)
        {
            var client = _httpClientFactory.CreateClient("FinancialApi");

            return await client.GetStringAsync($"balance-sheet-statement/{Ticker}?limit=5&apikey=9e79e541240a16005f1940b4b6984242");
        }
        public async Task<string> GetCashFlowStatements(string Ticker)
        {
            var client = _httpClientFactory.CreateClient("FinancialApi");

            return await client.GetStringAsync($"cash-flow-statement/{Ticker}?limit=5&apikey=9e79e541240a16005f1940b4b6984242");
        }

        

    }
}
