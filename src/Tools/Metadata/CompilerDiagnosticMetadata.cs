﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.Metadata
{
    public class CompilerDiagnosticMetadata
    {
        public CompilerDiagnosticMetadata(
            string id,
            string identifier,
            string title,
            string messageFormat,
            string severity,
            string helpUrl)
        {
            Id = id;
            Identifier = identifier;
            Title = title;
            MessageFormat = messageFormat;
            Severity = severity;
            HelpUrl = helpUrl;
        }

        public string Id { get; }

        public string Identifier { get; }

        public string Title { get; }

        public string MessageFormat { get; }

        public string Severity { get; }

        public string HelpUrl { get; }
    }
}
