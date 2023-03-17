using AngularAuthApi.Core.DcfCalculator.Abstract;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("intrinsic-value")]
        public async Task<IActionResult>GetIntristicValue(string ticker)
        {
            double intristicValue=await _coreDcfService.IntristicShareValue(ticker);
            return Ok(intristicValue);
        }

    }
}
