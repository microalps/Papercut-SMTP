using Papercut.Core.Domain.Network;
using SmtpServer;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Papercut.Infrastructure.Smtp
{
    internal static class EndpointDefinitionBuilderExtensions
    {
        public static EndpointDefinitionBuilder WithEndpoint(this EndpointDefinitionBuilder builder, EndpointDefinition smtpEndpoint)
        {
            builder = builder.Endpoint(smtpEndpoint.ToIPEndPoint());
            if (smtpEndpoint.Certificate != null)
                builder = builder.Certificate(smtpEndpoint.Certificate);
            return builder;
        }
    }
}
