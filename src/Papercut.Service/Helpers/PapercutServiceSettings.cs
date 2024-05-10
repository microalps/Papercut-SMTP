﻿// Papercut
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

namespace Papercut.Service.Helpers
{
    using Papercut.Core.Domain.Settings;

    public class PapercutServiceSettings : ISettingsTyped
    {
        public ISettingStore Settings { get; set; }

        public string CertificateFindType
        {
            get => Settings.GetOrSet("CertificateFindType", "FindByThumbprint", "Certificate Find Type.");
            set { if (CertificateFindType != value) Settings.Set("CertificateFindType", value); }
        }

        public string CertificateFindValue
        {
            get => Settings.GetOrSet("CertificateFindValue", "", "Certificate Find Value.");
            set { if (CertificateFindValue != value) Settings.Set("CertificateFindValue", value); }
        }

        public string IP
        {
            get => Settings.GetOrSet("IP", "Any", "SMTP Server listening IP. 'Any' is the default and it means '0.0.0.0'.");
            set { if (IP != value) Settings.Set("IP", value); }
        }

        public int Port
        {
            get => Settings.GetOrSet("Port", 25, "SMTP Server listening Port. Default is 25.");
            set { if (Port != value) Settings.Set("Port", value); }
        }

        public string MessagePath
        {
            get => Settings.GetOrSet<string>("MessagePath", @"%DataDirectory%\Incoming;%BaseDirectory%\Incoming", "Base path where incoming emails are written.");
            set { if (MessagePath != value) Settings.Set("MessagePath", value); }
        }

        public string LoggingPath
        {
            get => Settings.GetOrSet<string>("LoggingPath", @"%DataDirectory%\Logs;%BaseDirectory%\Logs", "Base path where logs are written.");
            set { if (LoggingPath != value) Settings.Set("LoggingPath", value); }
        }

        public string SeqEndpoint
        {
            get => Settings.GetOrSet<string>("SeqEndpoint", @"", "Populate with a endpoint if you want to enable SEQ (https://getseq.net/) logging.");
            set { if (SeqEndpoint != value) Settings.Set("SeqEndpoint", value); }
        }
    }
}