using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using ScooterBear.GTD.Application.Services.Email;
using ScooterBear.GTD.Patterns.CQRS;

namespace ScooterBear.GTD.AWS.SimpleEmailService
{
    public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand>
    {
        private readonly ILogger<SendEmailCommandHandler> _logger;

        public SendEmailCommandHandler(ILogger<SendEmailCommandHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Run(SendEmailCommand command)
        {
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.APEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = command.FromEmail,
                    Destination = new Destination
                    {
                        ToAddresses =
                            new List<string> { command.ToEmail }
                    },
                    Message = new Message
                    {
                        Subject = new Content(command.Subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = command.HtmlBody
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = command.TextBody
                            }
                        }
                    }
                };
                if (!string.IsNullOrEmpty(command.ConfigSetName))
                    sendRequest.ConfigurationSetName = command.ConfigSetName;

                try
                {
                    //TODO:  Come back we should have an exponential back off and retry
                    //TODO:  Fall back to a queue that will run this once the circuit breaker is turned back on.
                    //RESEARCH:  Does the .Net Framework use Polly or do I have to implement it?
                    await client.SendEmailAsync(sendRequest, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    _logger.LogError(new EventId(300), ex, "Error Sending email.");
                }
            }
        }
    }
}
