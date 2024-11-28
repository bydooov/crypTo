using CrypTo.Infrastructure.Contracts;
using CrypTo.Infrastructure.Exceptions;
using CrypTo.Infrastructure.Services.Mine;
using Microsoft.AspNetCore.Mvc;

namespace CrypTo.Api.Controllers
{
    [Route("api/mining")]
    [ApiController]
    public class MiningController : ControllerBase
    {
        private readonly IMiningService _miningService;

        public MiningController(IMiningService miningService)
        {
            _miningService = miningService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> MineBlockAsync()
        {
            try
            {
                await _miningService.MineAsync();

                return Ok();
            }
            catch (DatabaseException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Mining Block Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            catch (Exception ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Mining Block Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
