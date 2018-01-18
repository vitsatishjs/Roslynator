// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public override MarkdownWriter WriteString(string value)
        {
            ThrowIfClosed();

            if (string.IsNullOrEmpty(value))
                return this;

            RawText(value);
            return this;
        }

        protected void RawText(string value)
        {
            Debug.Assert(value != null);

            int length = value.Length;

            int prev = 0;

            int i = 0;
            while (i < length)
            {
                char ch = value[i];

                if (ch == 10)
                {
                    OnBeforeWrite();

                    if (Settings.NewLineHandling == NewLineHandling.Replace)
                    {
                        WriteString(value, prev, i - 1 - prev);
                        WriteLine();
                    }
                    else if (Settings.NewLineHandling == NewLineHandling.None)
                    {
                        WriteString(value, prev, i - prev);
                        OnAfterWriteLine();
                    }

                    prev = ++i;
                }
                else if (ch == 13)
                {
                    if (i == length - 1
                        || value[i + 1] != 10)
                    {
                        OnBeforeWrite();

                        if (Settings.NewLineHandling == NewLineHandling.Replace)
                        {
                            WriteString(value, prev, i - 1 - prev);
                            WriteLine();
                        }
                        else if (Settings.NewLineHandling == NewLineHandling.None)
                        {
                            WriteString(value, prev, i - prev);
                            OnAfterWriteLine();
                        }

                        prev = ++i;
                    }
                }
                else if (_shouldBeEscaped(ch))
                {
                    OnBeforeWrite();
                    WriteString(value, prev, i - prev);
                    WriteChar((_state == State.InlineCodeText) ? '`' : '\\');
                    WriteChar(ch);
                    prev = ++i;
                }
                else
                {
                    i++;
                }
            }

            OnBeforeWrite();
            WriteString(value, prev, value.Length - prev);
        }

        private void WriteChar(char ch)
        {
            ThrowIfClosed();
            StringBuilder.Append(ch);
        }

        private void WriteString(string value, int startIndex, int count)
        {
            ThrowIfClosed();
            StringBuilder.Append(value, startIndex, count);
        }

        public override void WriteValue(int value)
        {
            ThrowIfClosed();
            StringBuilder.Append(value.ToString(_formatProvider));
        }

        public override void WriteValue(long value)
        {
            ThrowIfClosed();
            StringBuilder.Append(value.ToString(_formatProvider));
        }

        public override void WriteValue(float value)
        {
            ThrowIfClosed();
            StringBuilder.Append(value.ToString(_formatProvider));
        }

        public override void WriteValue(double value)
        {
            ThrowIfClosed();
            StringBuilder.Append(value.ToString(_formatProvider));
        }

        public override void WriteValue(decimal value)
        {
            ThrowIfClosed();
            StringBuilder.Append(value.ToString(_formatProvider));
        }

        protected internal override IReadOnlyList<TableColumnInfo> AnalyzeTable(IEnumerable<MElement> rows)
        {
            throw new InvalidOperationException();
        }

        public override void Flush()
        {
        }

        public override void Close()
        {
            _isOpen = false;
        }

        private void ThrowIfClosed()
        {
            if (!_isOpen)
                throw new ObjectDisposedException(null, "Cannot write to a closed TextWriter.");
        }
    }
}