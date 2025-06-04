using System;
using brevo_csharp.Api;
using brevo_csharp.Client;
using FollwUp.API.Constants;
using FollwUp.API.Interfaces;
using RestSharp;

namespace FollwUp.API.Services;

public class EmailService : IEmailService
{
    private readonly string _emailApiKey = Keys.EmailApiKey;

    public async Task<bool> SendEmailAsync(string senderName, string senderEmail, string toEmail, string message, string? subject = null)
    {
        var client = new RestClient("https://api.brevo.com/v3/smtp/email");
        var request = new RestRequest("", Method.Post);
        request.AddHeader("accept", "application/json");
        request.AddHeader("api-key", _emailApiKey);
        request.AddHeader("Content-Type", "application/json");

            var body = new
            {
                sender = new { name = senderName, email = senderEmail},
                to = new[] { new { email = toEmail} },
                subject = subject, // TODO::: test with a null subject
                htmlContent = message
            };

        request.AddJsonBody(body);
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            // Handle error logging or retry
            Console.WriteLine(response.Content); // TODO::: push this error to trace
            return false;
        }

        return true;
    }

    // public async Task<bool> SendEmailAsync(string senderName, string senderEmail, string toEmail, string message, string? subject = null)
    // {
    //     var client = new RestClient("https://api.brevo.com/v3/smtp/email");
    //     var request = new RestRequest("POST");
    //     request.AddHeader("api-key", _brevoApiKey);
    //     request.AddHeader("Content-Type", "application/json");

    //         var body = new
    //         {
    //             sender = new { name = senderName, email = senderEmail},
    //             to = new[] { new { email = toEmail } },
    //             subject = subject, // TODO::: test with a null subject
    //             htmlContent = message
    //         };

    //     request.AddJsonBody(body);
    //     var response = await client.ExecuteAsync(request);

    //     if (!response.IsSuccessful)
    //     {
    //         // Handle error logging or retry
    //         Console.WriteLine(response.Content); // TODO::: push this error to trace
    //         return false;
    //     }

    //     return true;
    // }
}
