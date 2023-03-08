using AngularAuthApi.Core.DcfCalculator.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularAuthApi.Core.DcfCalculator.Controller
{
    [Route("api/")]
    [ApiController]
    public class FinancialDataController : ControllerBase
    {
        private readonly IFinancialPrepHttpCalls _calls;
        private readonly ICoreDcfService _coreDcfService;

        public FinancialDataController(IFinancialPrepHttpCalls calls, ICoreDcfService coreDcfService)
        {
            _calls = calls;
            _coreDcfService = coreDcfService;
        }

        [HttpGet("income-statements/{Ticker}")]
        public async Task<IActionResult> GetIncomeStatements(string Ticker)
        {
            var response = await _calls.GetIncomeStatements(Ticker);
            return Ok(response);
        }
        [HttpGet("balance-sheet/{Ticker}")]
        public async Task<IActionResult> GetBalanceSheet(string Ticker)
        {
            var response = await _calls.GetBalanceSheet(Ticker);
            return Ok(response);
        }
        [HttpGet("cash-flow-statements/{Ticker}")]
        public async Task<IActionResult> GetCashFlowStatements(string Ticker)
        {
            var response = await _calls.GetCashFlowStatements(Ticker);
            return Ok(response);
        }
        [HttpGet("tester")]
        [ResponseCache(Duration = 80000, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> test(string ticker, double sharePrice)
        {
            var response1 = await _coreDcfService.Fcff(ticker);
            var response2 = await _coreDcfService.Wacc(ticker,sharePrice);
            var response3 = await _coreDcfService.ExpectedFcff(ticker);
            var response4 = await _coreDcfService.TerminalValue(ticker,sharePrice);
            var response5 = await _coreDcfService.PvFcff(ticker, sharePrice);
            var response = await _coreDcfService.result(ticker,sharePrice);
            return Ok(response);
        }
        
    }
}
