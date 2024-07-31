using HospitalApi.Service.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Tenge.WebApi.Configurations;

namespace Tenge.Service.Helpers;

public class AuthHelper
{
    public static string GenerateToken(string phone)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(EnvironmentHelper.JWTKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                 new Claim("Phone", phone)
            }),
            Expires = DateTime.UtcNow.AddYears(Convert.ToInt32(EnvironmentHelper.TokenLifeTimeInYears)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async static Task<long> SendCodeToPhone(string phone)
    {
        Random random = new Random();
        var code = random.Next(1000, 9999);
        using (HttpClient client = new HttpClient())
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MjQ5MjU3NTEsImlhdCI6MTcyMjMzMzc1MSwicm9sZSI6InVzZXIiLCJzaWduIjoiM2FiOTcwN2Q4ODRhNTIzMGQwOTUzZDE5ZTMwZGU2NWEyMjgyMzQxNjQ4MjMzOTIwNTNkOTQ5OGMzMWUwOTdmNyIsInN1YiI6Ijc5NzMifQ.Zg-UjfLrHgXHYMlVrmUbbWg24d2KV4tNg3kYZosbMsA";
           client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestContent = new MultipartFormDataContent();
            requestContent.Add(new StringContent(phone.Substring(1)), "mobile_phone");
            requestContent.Add(new StringContent($"Код верификации для входа к мобильному приложению Find Sports: {code.ToString()}"), "message");
            requestContent.Add(new StringContent("4546"), "from");
            //requestContent.Add(new StringContent("Authorization"), $"Bearer {token}");

            HttpResponseMessage response = await client.PostAsync("https://notify.eskiz.uz/api/message/sms/send", requestContent);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new ArgumentIsNotValidException("Failed to Send SMS");
            }
            
        }
        return code;
    }   
}
