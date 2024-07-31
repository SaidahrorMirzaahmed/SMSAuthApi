namespace SMSAuthApi.ApiServices;

public interface ISMSService
{
    Task<bool> SendSMSCodeAsync(string phone);
    Task<string> VerifySMSCode(string phone, long code);
}

