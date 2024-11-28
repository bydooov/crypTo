using CrypTo.Infrastructure.Contracts;
using CrypTo.Infrastructure.Contracts.Wallets;
using CrypTo.Infrastructure.Exceptions;
using CrypTo.Infrastructure.Services.Wallets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CrypTo.Api.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateWalletContract>> CreateWalletAsync()
        {
            try
            {
                var wallet = await _walletService.CreateWalletAsync().ConfigureAwait(false);

                return Ok(wallet);
            }
            catch (DatabaseException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Create Wallet Failed"
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
                    Title = "Create Wallet Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpPost, Route("signature")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<WalletSignatureContract> CreateWalletSignature([FromBody, BindRequired] GetWalletSignatureRequest request)
        {
            try
            {
                var signature = _walletService.CreateWalletSignature(request.PrivateKey!, request.Message!);

                return Ok(signature);
            }
            catch (Exception ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Create Signature Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpPost, Route("{walletAddress}/deposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<WalletContract>> DepositWalletMoneyAsync([FromRoute] string walletAddress, [FromBody, BindRequired] DepositWalletMoneyRequest request)
        {
            try
            {
                string decodedWalletAddress = Uri.UnescapeDataString(walletAddress);

                var wallet = await _walletService.DepositWalletMoneyAsync(decodedWalletAddress, request).ConfigureAwait(false);

                return Ok(wallet);
            }
            catch (DatabaseException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Deposit Wallet Money Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            catch (NotFoundException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Details = ex.Message,
                    Title = "Deposit Wallet Money Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            catch (BadRequestException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Details = ex.Message,
                    Title = "Deposit Wallet Money Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            catch (Exception ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Deposit Wallet Money Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpGet, Route("{walletAddress}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<WalletContract>> GetWalletByWalletAddressAsync([FromRoute] string walletAddress)
        {
            try
            {
                string decodedWalletAddress = Uri.UnescapeDataString(walletAddress);

                var wallet = await _walletService.GetWalletAsync(decodedWalletAddress).ConfigureAwait(false);

                return Ok(wallet);
            }
            catch (DatabaseException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Get Wallet Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            catch (NotFoundException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Details = ex.Message,
                    Title = "Get Wallet Failed"
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
                    Title = "Get Wallet Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpGet, Route("{walletAddress}/transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<WalletContract>> GetWalletTransactionsByWalletAddressAsync([FromRoute] string walletAddress)
        {
            try
            {
                string decodedWalletAddress = Uri.UnescapeDataString(walletAddress);

                var wallet = await _walletService.GetWalletTransactionsAsync(decodedWalletAddress).ConfigureAwait(false);

                return Ok(wallet);
            }
            catch (DatabaseException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Get Wallet Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            catch (NotFoundException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Details = ex.Message,
                    Title = "Get Wallet Failed"
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
                    Title = "Get Wallet Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpDelete, Route("{walletAddress}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteWalletByWalletAddressAsync([FromRoute] string walletAddress, [FromBody] WalletSignatureContract signature)
        {
            try
            {
                string decodedWalletAddress = Uri.UnescapeDataString(walletAddress);

                await _walletService.DeleteWalletAsync(decodedWalletAddress, signature).ConfigureAwait(false);

                return NoContent();
            }
            catch (DatabaseException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Delete Wallet Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            catch (NotFoundException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Details = ex.Message,
                    Title = "Delete Wallet Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            catch (BadRequestException ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Details = ex.Message,
                    Title = "Delete Wallet Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            catch (Exception ex)
            {
                var errorContract = new ErrorContract
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Details = ex.Message,
                    Title = "Delete Wallet Failed"
                };

                return new ObjectResult(errorContract)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
