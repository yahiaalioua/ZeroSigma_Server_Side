using AngularAuthApi.DcfCalculator.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularAuthApi.DcfCalculator.Controller
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
        public async Task<IActionResult> test(string ticker, double sharePrice)
        {
            var response = await _coreDcfService.ExpectedFcff(ticker);
            return Ok(response);
        }
    }
}
