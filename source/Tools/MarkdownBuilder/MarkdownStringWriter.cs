// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Pihrtsoft.Markdown.Linq;

namespace Pihrtsoft.Markdown
{
    public class MarkdownStringWriter : MarkdownWriter
    {
        private readonly StringBuilder _sb;
        private readonly IFormatProvider _formatProvider;
        private bool _isOpen;

        public MarkdownStringWriter(MarkdownWriterSettings settings = null)
            : this(new StringBuilder(), settings)
        {
        }

        public MarkdownStringWriter(StringBuilder sb, MarkdownWriterSettings settings = null)
            : this(sb, CultureInfo.CurrentCulture, settings)
        {
        }

        public MarkdownStringWriter(IFormatProvider formatProvider, MarkdownWriterSettings settings = null)
            : this(new StringBuilder(), formatProvider, settings)
        {
        }

        public MarkdownStringWriter(StringBuilder sb, IFormatProvider formatProvider, MarkdownWriterSettings settings = null)
            : base(settings)
        {
            _sb = sb ?? throw new ArgumentNullException(nameof(sb));
            _formatProvider = formatProvider;
            _isOpen = true;
        }

        internal virtual StringBuilder GetStringBuilder()
        {
            return _sb;
        }

        public virtual IFormatProvider FormatProvider
        {
            get { return _formatProvider ?? CultureInfo.CurrentCulture; }
        }

        protected internal override int Length
        {
            get { return _sb.Length; }
            set { _sb.Length = value; }
        }

        public override MarkdownWriter WriteString(string text)
        {
            ThrowIfClosed();

            if (string.IsNullOrEmpty(text))
                return this;

            int length = text.Length;

            int prev = 0;

            int i = 0;
            while (i < length)
            {
                char ch = text[i];

                if (ch == 10)
                {
                    OnBeforeWriteLine();

                    if (NewLineHandling == NewLineHandling.Replace)
                    {
                        WriteString(text, prev, i - prev);
                        WriteNewLine();
                    }
                    else if (NewLineHandling == NewLineHandling.None)
                    {
                        WriteString(text, prev, i + 1 - prev);
                    }

                    OnAfterWriteLine();
                    prev = ++i;
                }
                else if (ch == 13)
                {
                    OnBeforeWriteLine();

                    if (i < length - 1
                        && text[i + 1] == 10)
                    {
                        if (NewLineHandling == NewLineHandling.Replace)
                        {
                            WriteString(text, prev, i - prev);
                            WriteNewLine();
                        }
                        else if (NewLineHandling == NewLineHandling.None)
                        {
                            WriteString(text, prev, i + 2 - prev);
                        }

                        i++;
                    }
                    else if (NewLineHandling == NewLineHandling.Replace)
                    {
                        WriteString(text, prev, i - prev);
                        WriteNewLine();
                    }
                    else if (NewLineHandling == NewLineHandling.None)
                    {
                        WriteString(text, prev, i + 1 - prev);
                    }

                    OnAfterWriteLine();
                    prev = ++i;
                }
                else if (ShouldBeEscaped(ch))
                {
                    WriteString(text, prev, i - prev);
                    WriteChar(EscapingChar);
                    WriteChar(ch);
                    prev = ++i;
                }
                else
                {
                    i++;
                }
            }

            WriteString(text, prev, text.Length - prev);
            return this;
        }

        private void WriteString(string value, int startIndex, int count)
        {
            ThrowIfClosed();
            _sb.Append(value, startIndex, count);
        }

        private void WriteChar(char ch)
        {
            ThrowIfClosed();
            _sb.Append(ch);
        }

        private void WriteNewLine()
        {
            ThrowIfClosed();
            _sb.Append(NewLineChars);
        }

        public override MarkdownWriter WriteRaw(string data)
        {
            ThrowIfClosed();
            _sb.Append(data);
            return this;
        }

        public override MarkdownWriter WriteLine()
        {
            OnBeforeWriteLine();
            ThrowIfClosed();
            _sb.Append(NewLineChars);
            OnAfterWriteLine();
            return this;
        }

        public override void WriteValue(int value)
        {
            ThrowIfClosed();
            _sb.Append(value.ToString(FormatProvider));
        }

        public override void WriteValue(long value)
        {
            ThrowIfClosed();
            _sb.Append(value.ToString(FormatProvider));
        }

        public override void WriteValue(float value)
        {
            ThrowIfClosed();
            _sb.Append(value.ToString(FormatProvider));
        }

        public override void WriteValue(double value)
        {
            ThrowIfClosed();
            _sb.Append(value.ToString(FormatProvider));
        }

        public override void WriteValue(decimal value)
        {
            ThrowIfClosed();
            _sb.Append(value.ToString(FormatProvider));
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        protected internal override IReadOnlyList<TableColumnInfo> AnalyzeTable(IEnumerable<MElement> rows)
        {
            return TableAnalyzer.Analyze(rows, Settings, FormatProvider)?.AsReadOnly();
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
                throw new ObjectDisposedException(null, "Cannot write to a closed writer.");
        }
    }
}