// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace Pihrtsoft.Markdown
{
    internal class MarkdownStringWriter : MarkdownWriter
    {
        public MarkdownStringWriter(StringBuilder sb = null, MarkdownWriterSettings settings = null)
            : base(settings)
        {
            StringBuilder = sb ?? new StringBuilder();
        }

        public StringBuilder StringBuilder { get; }

        protected override int Length
        {
            get { return StringBuilder.Length; }
            set { StringBuilder.Length = value; }
        }

        protected override void WriteCore(string value)
        {
            StringBuilder.Append(value);
        }

        protected override void WriteCore(string value, int startIndex, int count)
        {
            StringBuilder.Append(value, startIndex, count);
        }

        protected override void WriteCore(char value)
        {
            StringBuilder.Append(value);
        }

        protected override void WriteCore(char value, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
                StringBuilder.Append(value);
        }

        public void Clear()
        {
            StringBuilder.Clear();
            ResetState();
        }
    }
}