using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Account;
using StudyRoomRental.BusinessTier.Payload.Login;
using StudyRoomRental.BusinessTier.Validators;
using StudyRoomRental.DataTier.Paginate;

namespace StudyRoomRental.API.Controllers
{
    [ApiController]
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService) : base(logger)
        {
            _accountService = accountService;
        }

        [HttpPost(ApiEndPointConstant.Authentication.Login)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var loginResponse = await _accountService.Login(loginRequest);
            if (loginResponse == null)
                throw new BadHttpRequestException(MessageConstant.LoginMessage.InvalidUsernameOrPassword);
            if (loginResponse.Status == AccountStatus.Deactivate)
                throw new BadHttpRequestException(MessageConstant.LoginMessage.DeactivatedAccount);
            return Ok(loginResponse);
        }

        // [CustomAuthorize(RoleEnum.Admin)]
        [HttpPost(ApiEndPointConstant.Account.AccountsEndpoint)]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateAccount(AccountRequest createNewAccountRequest)
        {
            var response = await _accountService.CreateNewAccount(createNewAccountRequest);
            return Ok(response);
        }

        // [CustomAuthorize(RoleEnum.Admin)]
        [HttpPut(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateAccountInformation(int id, [FromBody] UpdateAccountRequest updateAccountRequest)
        {
            var isSuccessful = await _accountService.UpdateAccountInformation(id, updateAccountRequest);
            if (!isSuccessful) return Ok(MessageConstant.Account.UpdateAccountFailedMessage);
            return Ok(MessageConstant.Account.UpdateAccountSuccessfulMessage);
        }

        [HttpGet(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(GetAccountResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetAccountDetail(int id)
        {
            var accountDetails = await _accountService.GetAccountDetail(id);
            return Ok(accountDetails);
        }

        // [CustomAuthorize(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateAccountStatus(int id)
        {
            var isSuccessful = await _accountService.UpdateAccountStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Account.UpdateAccountStatusFailedMessage);
            return Ok(MessageConstant.Account.UpdateAccountStatusSuccessfulMessage);
        }
         
        // [CustomAuthorize(RoleEnum.Admin)]
        [HttpPut(ApiEndPointConstant.Account.AccountUpdateEndpoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateLandlordRole(int id)
        {
            var isSuccessful = await _accountService.UpdateAccountRole(id);
            if (!isSuccessful) return Ok(MessageConstant.Account.UpdateAccountRoleFailedMessage);
            return Ok(MessageConstant.Account.UpdateAccountRoleSuccessfulMessage);
        }

        // [CustomAuthorize(RoleEnum.Admin)]
        [HttpGet(ApiEndPointConstant.Account.AccountsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<GetAccountResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllAccount([FromQuery] string? username, [FromQuery] RoleEnum? role, AccountStatus? status, [FromQuery] int page, [FromQuery] int size)
        {
            var accounts = await _accountService.GetAccounts(username, role, status, page, size);
            return Ok(accounts);
        }
    }
}
