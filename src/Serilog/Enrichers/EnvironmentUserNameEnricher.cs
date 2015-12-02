﻿// Copyright 2013-2015 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#if !PROFILE259

using System;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers
{
    /// <summary>
    /// Enriches log events with an EnvironmentUserName property containing [<see cref="Environment.UserDomainName"/>\]<see cref="Environment.UserName"/>.
    /// </summary>
    public class EnvironmentUserNameEnricher : ILogEventEnricher
    {
        LogEventProperty _cachedProperty;

        /// <summary>
        /// The property name added to enriched log events.
        /// </summary>
        public const string EnvironmentUserNamePropertyName = "EnvironmentUserName";

        /// <summary>
        /// Enrich the log event.
        /// </summary>
        /// <param name="logEvent">The log event to enrich.</param>
        /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            _cachedProperty = _cachedProperty ?? propertyFactory.CreateProperty(EnvironmentUserNamePropertyName, GetEnvironmentUserName());
            logEvent.AddPropertyIfAbsent(_cachedProperty);
        }

        private static string GetEnvironmentUserName()
        {
#if !DOTNET5_4
            var userDomainName = Environment.UserDomainName;
            var userName = Environment.UserName;
#else
            var userDomainName = Environment.GetEnvironmentVariable("USERNAME");
            var userName = Environment.GetEnvironmentVariable("USERDOMAIN");
#endif
            return !string.IsNullOrWhiteSpace(userDomainName) ? $@"{userDomainName}\{userName}" : userName;
        }
    }
}

#endif
