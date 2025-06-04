using System;

namespace FollwUp.API.Constants;

public static class Keys
{
    public static string EmailApiKey = Environment.GetEnvironmentVariable("EMAIL_API_KEY") 
        ?? throw new InvalidOperationException("EMAIL_API_KEY environment variable is not set.");
}
