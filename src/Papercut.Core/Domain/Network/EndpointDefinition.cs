// Papercut
// 
// Copyright © 2008 - 2012 Ken Robertson
// Copyright © 2013 - 2020 Jaben Cargman
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


namespace Papercut.Core.Domain.Network
{
    using System;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;

    public class EndpointDefinition
    {
        public EndpointDefinition(string address, int port)
        {
            Address = ParseIpAddress(address);
            Port = port;
        }

        public EndpointDefinition(string address, int port, X509FindType certificateFindType, string certificateFindValue) : this(address, port)
        {
            X509Store store = new X509Store("MY", StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            var certificates = store.Certificates.Find(certificateFindType, certificateFindValue, false);
            if (certificates.Count != 1)
            {
                throw new Exception("Certificate Not Found");
            }
            Certificate = certificates[0];
            store.Close();
        }

        public IPAddress Address { get; }
        public int Port { get; }
        public X509Certificate Certificate { get; }

        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(this.Address, this.Port);
        }

        private IPAddress ParseIpAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || string.Equals(value, "any", StringComparison.OrdinalIgnoreCase))
            {
                return IPAddress.Any;
            }

            return IPAddress.Parse(value);
        }

        public override string ToString()
        {
            return $"{Address}:{Port}";
        }
    }
}