using HospitalApi.Service.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Tenge.Service.Helpers;

namespace SMSAuthApi.ApiServices;

public class SMSService(IMemoryCache cache) : ISMSService
{
    public async Task<bool> SendSMSCodeAsync(string phone)
    {
        if(!ValidationHelper.IsPhoneValid(phone))
            throw new ArgumentIsNotValidException(nameof(phone)+ "is not valid");
        var code = await AuthHelper.SendCodeToPhone(phone);
        cache.Set(phone, code, TimeSpan.FromMinutes(5));

        return await Task.FromResult(true);
    }

    public async Task<string> VerifySMSCode(string phone, long code)
    {
        if (cache.TryGetValue(phone, out long storedCode))
        {
            if (storedCode == code)
            {
                cache.Remove(phone);
                var token = AuthHelper.GenerateToken(phone);
                return token;
            }
            else
            {
                throw new ArgumentIsNotValidException("Invalid code");
            }
        }
        else
        {
            throw new NotFoundException("Code not found or expired");
        }
    }
}
    
