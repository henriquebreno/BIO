using BIO_API_DATA.API_Client.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace BIO_API_DATA.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopLevelCustomerController : ControllerBase
    {
        private readonly ITimeSeriesLogic _timeSeriesLogic;
        private readonly ILogger<TopLevelCustomerController> _logger;
        public TopLevelCustomerController(ITimeSeriesLogic timeSeriesLogic,ILogger<TopLevelCustomerController> logger)
        {
            _timeSeriesLogic = timeSeriesLogic;
            _logger = logger;
        }
        [HttpPost(Name = "Import")]
        public async Task<IActionResult> ImportTopCustomerJob()
        {
            _logger.LogDebug("ImportTopCustomerJob Import");
            await _timeSeriesLogic.AddTimeSeriesAsync();
            return Ok();
        }
    }
}
