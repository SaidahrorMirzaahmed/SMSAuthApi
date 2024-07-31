using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SMSAuthApi.ApiServices;
using SMSAuthApi.Models;

namespace SMSAuthApi.Controllers;

[EnableCors("AllowSpecificOrigin")]
[Route("api/[controller]")]
[ApiController]
public class SMSController(ISMSService service) : ControllerBase
{
    [HttpGet("send-code")]
    public async Task<IActionResult> SendCode(string phone)
    {
        return Ok(new Response
        {
            StatusCode = 200,
            Message= "Ok",
            Data = await service.SendSMSCodeAsync(phone)
        });
    }

    [HttpGet("verify")]
    public async Task<IActionResult> VerifyCode(string phone, long code)
    {
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Ok",
            Data = await service.VerifySMSCode(phone, code)
        });
    }
}
