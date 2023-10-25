using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyRoomRental.API.Services.Interfaces;
using StudyRoomRental.BusinessTier.Constants;
using StudyRoomRental.BusinessTier.Enums;
using StudyRoomRental.BusinessTier.Payload.Account;
using StudyRoomRental.BusinessTier.Payload.Login;
using StudyRoomRental.BusinessTier.Validators;

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
    }
}
