using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using SSEStockPrice.Interfaces;
using SSEStockPrice.Models;
using SSEStockPrice.Models.Enums;

namespace SSEStockPrice.Infrastructure.ExternalApis
{
    public class AcsEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailClient _emailClient;
        public AcsEmailService(IConfiguration config, EmailClient emailClient   )
        {
            _configuration = config;
            _emailClient = emailClient;
        }
        public async Task SendEmailAlertAsync(PriceAlertMessage priceAlertMessage, CancellationToken cancellationToken = default)
        {
            var senderAddr = _configuration["AcsSenderAddress"];
            var directionText = priceAlertMessage.Direction == AlertDirection.Above ? "above" : "below";
            var body = $@"Hi {priceAlertMessage.ColleagueName},

You are recieving this alert based on your setup for {priceAlertMessage.Symbol}.
The current price is {priceAlertMessage.CurrentPrice:C}, which is {directionText} your target of {priceAlertMessage.TargetPrice:C}.
Thank you for using SSE Stock Price Alerts! If you have any question please email me at nirdesh.acharya@sse.com
Nirdesh";
            var subject = $"Alert: Stock Prices have gone {directionText} {priceAlertMessage.TargetPrice:C}";
            var emailContent = new EmailContent(subject)
            {
                PlainText = body
            };
            var emailMsg = new EmailMessage(senderAddr, priceAlertMessage.Email, emailContent);

            var result = await _emailClient.SendAsync(Azure.WaitUntil.Completed ,emailMsg, cancellationToken);

            if (result.Value.Status == EmailSendStatus.Failed) throw new InvalidOperationException($"Email failed to send to {priceAlertMessage.Email}");
        }
    }
}
