// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Pihrtsoft.Markdown.Linq;

namespace Pihrtsoft.Markdown
{
    internal class MarkdownStringWriter : MarkdownWriter
    {
        private bool _isOpen;
        private readonly IFormatProvider _formatProvider;

        public MarkdownStringWriter(IFormatProvider formatProvider, StringBuilder stringBuilder = null, MarkdownWriterSettings settings = null)
            : base(settings)
        {
            _formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
            StringBuilder = stringBuilder ?? new StringBuilder();
            _isOpen = true;
        }

        public StringBuilder StringBuilder { get; }

        protected internal override int Length
        {
            get { return StringBuilder.Length; }
            set { StringBuilder.Length = value; }
        }

        protected override void WriteString(string value)
        {
            ThrowIfClosed();

            StringBuilder.Append(value);
        }

        protected override void WriteString(string value, int startIndex, int count)
        {
            ThrowIfClosed();

            StringBuilder.Append(value, startIndex, count);
        }

        protected override void WriteValue(char value)
        {
            ThrowIfClosed();

            StringBuilder.Append(value);
        }

        protected override void WriteValue(int value)
        {
            StringBuilder.Append(value.ToString(_formatProvider));
        }

        protected override void WriteValue(uint value)
        {
            StringBuilder.Append(value.ToString(_formatProvider));
        }

        protected override void WriteValue(long value)
        {
            StringBuilder.Append(value.ToString(_formatProvider));
        }

        protected override void WriteValue(ulong value)
        {
            StringBuilder.Append(value.ToString(_formatProvider));
        }

        protected override void WriteValue(float value)
        {
            StringBuilder.Append(value.ToString(_formatProvider));
        }

        protected override void WriteValue(double value)
        {
            StringBuilder.Append(value.ToString(_formatProvider));
        }

        protected override void WriteValue(decimal value)
        {
            StringBuilder.Append(value.ToString(_formatProvider));
        }

        protected internal override IReadOnlyList<TableColumnInfo> AnalyzeTable(IEnumerable<MElement> rows)
        {
            throw new InvalidOperationException();
        }

        private void ThrowIfClosed()
        {
            if (!_isOpen)
                throw new ObjectDisposedException(null, "Cannot write to a closed TextWriter.");
        }

        public override void Close()
        {
            _isOpen = false;
        }

        internal override void Reset()
        {
            StringBuilder.Clear();
            base.Reset();
        }
    }
}