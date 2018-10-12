using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CalculatorServices.Services;
using CalculatorServices.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly IAdditionService _additionService;
        private readonly ISubtractionService _subtractionService;
        private readonly IHistoryService _historyService;

        public CalculatorController(IAdditionService additionService,
            ISubtractionService subtractionService,
            IHistoryService historyService)
        {
            _additionService = additionService;
            _subtractionService = subtractionService;
            _historyService = historyService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<CalculatorResultViewModel>> Add(int? id, decimal? value, CancellationToken cancellationToken)
        {
            if (!value.HasValue)
            {
                return BadRequest("Must provide value");
            }

            return Ok(await _additionService.Add(id, value.Value, cancellationToken));
        }

        [HttpPost("subtract")]
        public async Task<ActionResult<CalculatorResultViewModel>> Subtract(int? id, decimal? value, CancellationToken cancellationToken)
        {
            if (!value.HasValue)
            {
                return BadRequest("Must provide value");
            }

            return Ok(await _subtractionService.Subtract(id, value.Value, cancellationToken));
        }

        [HttpGet("history/{id}")]
        public async Task<ActionResult<CalculatorHistoryViewModel>> History(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("CalculatorController.History needs to be implemented using the service you build.");
        }
    }
}
