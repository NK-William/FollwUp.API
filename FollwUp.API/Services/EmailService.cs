using System;
using FollwUp.API.Interfaces;
using RestSharp;

namespace FollwUp.API.Services;

public class EmailService : IEmailService
{
    private readonly string _brevoApiKey = "your-brevo-api-key";

    public async Task<bool> SendEmailAsync(string senderName, string senderEmail, string toEmail, string message, string? subject = null)
    {
        var client = new RestClient("https://api.brevo.com/v3/smtp/email");
        var request = new RestRequest("POST");
        request.AddHeader("api-key", _brevoApiKey);
        request.AddHeader("Content-Type", "application/json");

        var frontendUrl = $"https://your-frontend-domain.com/details/{taskId}"; //TODO::: should not be in here, should be part of message
            var body = new
            {
                sender = new { name = "YourApp", email = "your@sender.com" }, // senderName and senderEmail should be used here
                to = new[] { new { email = toEmail } },
                subject = "Your Unique Link", // subject should be used here
                htmlContent = $"<p>Click the link below to view your data:</p><a href='{frontendUrl}'>Open Link</a>" // message here
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
}
