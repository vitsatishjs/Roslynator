// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("Encoding = {Encoding.WebName,nq} CloseOutput = {CloseOutput}")]
    public class MarkdownWriterSettings : IEquatable<MarkdownWriterSettings>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as MarkdownWriterSettings);
        }

        public bool Equals(MarkdownWriterSettings other)
        {
            return other != null
                && EqualityComparer<MarkdownFormat>.Default.Equals(Format, other.Format)
                && EqualityComparer<Encoding>.Default.Equals(Encoding, other.Encoding)
                && NewLineChars == other.NewLineChars
                && NewLineHandling == other.NewLineHandling
                && CloseOutput == other.CloseOutput;
        }

        public override int GetHashCode()
        {
            int hash = Hash.Create(Format);
            hash = Hash.Combine(Encoding, hash);
            hash = Hash.Combine(NewLineChars, hash);
            hash = Hash.Combine((int)NewLineHandling, hash);
            return Hash.Combine(CloseOutput, hash);
        }

        public static bool operator ==(MarkdownWriterSettings settings1, MarkdownWriterSettings settings2)
        {
            return EqualityComparer<MarkdownWriterSettings>.Default.Equals(settings1, settings2);
        }

        public static bool operator !=(MarkdownWriterSettings settings1, MarkdownWriterSettings settings2)
        {
            return !(settings1 == settings2);
        }
    }
}
