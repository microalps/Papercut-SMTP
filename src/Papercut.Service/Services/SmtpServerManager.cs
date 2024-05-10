﻿// Papercut
// 
// Copyright © 2008 - 2012 Ken Robertson
// Copyright © 2013 - 2021 Jaben Cargman
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Papercut.Service.Services
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;

    using Autofac;

    using Papercut.Common.Domain;
    using Papercut.Core.Annotations;
    using Papercut.Core.Domain.Network;
    using Papercut.Core.Domain.Network.Smtp;
    using Papercut.Core.Domain.Settings;
    using Papercut.Core.Infrastructure.Lifecycle;
    using Papercut.Infrastructure.Smtp;
    using Papercut.Service.Helpers;

    using Serilog;

    public class SmtpServerManager : IEventHandler<SmtpServerBindEvent>, IEventHandler<PapercutServiceReadyEvent>
    {
        private readonly ILogger _logger;

        private readonly PapercutServiceSettings _serviceSettings;

        private readonly PapercutSmtpServer _smtpServer;

        public SmtpServerManager(PapercutSmtpServer smtpServer,
            PapercutServiceSettings serviceSettings,
            ILogger logger)
        {
            this._smtpServer = smtpServer;
            this._serviceSettings = serviceSettings;
            this._logger = logger;
        }

        public async Task HandleAsync(SmtpServerBindEvent @event, CancellationToken token = default)
        {
            this._logger.Information(
                "Received New Smtp Server Binding Settings from UI {@Event}",
                @event);

            // update settings...
            this._serviceSettings.IP = @event.IP;
            this._serviceSettings.Port = @event.Port;
            this._serviceSettings.Save();

            // rebind the server...
            await this.BindSMTPServer();
        }

        public async Task HandleAsync(PapercutServiceReadyEvent @event, CancellationToken token = default)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500), token);
            await BindSMTPServer();
        }

        async Task BindSMTPServer()
        {
            try
            {
                await this._smtpServer.StopAsync();
                var endpoint = string.IsNullOrEmpty(_serviceSettings.CertificateFindValue)
                    ? new EndpointDefinition(_serviceSettings.IP, _serviceSettings.Port)
                    : new EndpointDefinition(_serviceSettings.IP, _serviceSettings.Port, (X509FindType)Enum.Parse(typeof(X509FindType), _serviceSettings.CertificateFindType, true), _serviceSettings.CertificateFindValue);
                await this._smtpServer.StartAsync(endpoint);
            }
            catch (Exception ex)
            {
                this._logger.Warning(
                    ex,
                    "Unable to Create SMTP Server Listener on {IP}:{Port}. After 5 Retries. Failing",
                    this._serviceSettings.IP,
                    this._serviceSettings.Port);
            }
        }
    }
}