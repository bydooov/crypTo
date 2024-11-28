using CrypTo.Infrastructure.Contracts;
using CrypTo.Infrastructure.Contracts.Transactions;
using CrypTo.Infrastructure.Exceptions;
using CrypTo.Infrastructure.Services.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CrypTo.Api.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateTransactionAsync([BindRequired, FromBody] CreateTransactionRequest request)
        {
            try
            {
                await _transactionService.CreateTransactionAsync(request).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Create Transaction Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpPost, Route("process")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ProcessTransactionAsync([BindRequired, FromBody] List<ProcessTransactionRequest> request)
        {
            try
            {
                await _transactionService.ProcessTransactionsAsync(request).ConfigureAwait(false);  

                return Ok();
            }
            catch (Exception ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Process Transaction Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpGet, Route("{transactionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransactionContract>> GetTransactionAsync([BindRequired, FromRoute] string transactionId)
        {
            try
            {
                return Ok(await _transactionService.GetTransactionAsync(transactionId).ConfigureAwait(false));  
            }
            catch (NotFoundException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Details = ex.Message,
                    Title = "Get Transaction Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            catch (Exception ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Get Transaction Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
