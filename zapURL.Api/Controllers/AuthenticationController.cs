using Microsoft.AspNetCore.Mvc;
using zapURL.Api.Contracts.Authentication;
using zapURL.Api.Services.AuthenticationService;

namespace zapURL.Api.Controllers;

[Route("api/[controller]")]
public class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly Uri _callBackUrl = new("http://localhost:5000/handler/email-verification");

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(LoginRequest request)
    {
        var result = await _authenticationService.SignInAsync(request.Email, request.Password);
        return result.Match(
            Ok,
            Problem
        );
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(RegisterRequest request)
    {
        var result = await _authenticationService.SignUpAsync(request.Email, request.Password, _callBackUrl.ToString());
        return result.Match(
            Ok,
            Problem
        );
    }
}