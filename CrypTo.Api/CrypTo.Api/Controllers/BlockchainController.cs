using CrypTo.Infrastructure.Contracts;
using CrypTo.Infrastructure.Contracts.Blockchain;
using CrypTo.Infrastructure.Exceptions;
using CrypTo.Infrastructure.Services.Blockchain;
using Microsoft.AspNetCore.Mvc;

namespace CrypTo.Api.Controllers
{
    [Route("api/blockchain")]
    [ApiController]
    public class BlockchainController : ControllerBase
    {
        private readonly IBlockChainService _blockChainService;

        public BlockchainController(IBlockChainService blockChainService)
        {
            _blockChainService = blockChainService;
        }

        [HttpGet, Route("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BlockChainStatusContract>> GetBlockChainStatusAsync()
        {
            try
            {
                var blockchainStatus = await _blockChainService.GetBlockcahinStatusAsync().ConfigureAwait(false);

                return Ok(blockchainStatus);
            }
            catch (DatabaseException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Get Blockchain Status Failed"
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
                    Title = "Get Blockchain Status Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BlockChainContract>> GetBlockChainAsync()
        {
            try
            {
                var blockchain = await _blockChainService.GetBlockchainAsync().ConfigureAwait(false);

                return Ok(blockchain);
            }
            catch (DatabaseException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Get Blockchain Failed"
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
                    Title = "Get Blockchain Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        [HttpGet, Route("{index}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BlockContract>> GetBlockAsync([FromRoute] int index)
        {
            try
            {
                var block = await _blockChainService.GetBlockAsync(index).ConfigureAwait(false);

                return Ok(block);
            }
            catch (NotFoundException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Details = ex.Message,
                    Title = "Get Block Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            catch (DatabaseException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Get Block Failed"
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
                    Title = "Get Block Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
