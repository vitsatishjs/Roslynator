// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Text;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("Encoding = {Encoding.EncodingName,nq} CloseOutput = {CloseOutput}")]
    public class MarkdownWriterSettings
    {
        public MarkdownWriterSettings(
            MarkdownFormat format = null,
            Encoding encoding = null,
            string newLineChars = null,
            NewLineHandling newLineHandling = NewLineHandling.Replace,
            bool closeOutput = false)
        {
            Format = format ?? MarkdownFormat.Default;
            Encoding = encoding ?? Encoding.UTF8;
            NewLineChars = newLineChars ?? Environment.NewLine;
            NewLineHandling = newLineHandling;
            CloseOutput = closeOutput;
        }

        public static MarkdownWriterSettings Default { get; } = new MarkdownWriterSettings();

        internal static MarkdownWriterSettings Debugging { get; } = new MarkdownWriterSettings(MarkdownFormat.Debugging);

        public MarkdownFormat Format { get; }

        public Encoding Encoding { get; }

        public string NewLineChars { get; }

        public NewLineHandling NewLineHandling { get; }

        public bool CloseOutput { get; }

        internal static MarkdownWriterSettings From(MarkdownFormat format)
        {
            if (format == null
                || object.ReferenceEquals(format, MarkdownFormat.Default))
            {
                return Default;
            }

            return new MarkdownWriterSettings(format);
        }
    }
}
