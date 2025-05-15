using System;

namespace FollwUp.API.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string senderName, string senderEmail, string toEmail, string message, string? subject = null);
}
