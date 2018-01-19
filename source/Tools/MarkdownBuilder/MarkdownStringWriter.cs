// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Pihrtsoft.Markdown.Linq;

namespace Pihrtsoft.Markdown
{
    //TODO: public
    internal class MarkdownStringWriter : MarkdownWriter
    {
        private readonly StringBuilder _sb;
        private readonly IFormatProvider _formatProvider;
        private bool _isOpen;

        public MarkdownStringWriter(MarkdownWriterSettings settings = null)
            : this(new StringBuilder(), settings)
        {
        }

        public MarkdownStringWriter(StringBuilder sb, MarkdownWriterSettings settings = null)
            : this(sb, CultureInfo.InvariantCulture, settings)
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
            _formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
            _isOpen = true;
        }

        public virtual StringBuilder GetStringBuilder()
        {
            return _sb;
        }

        protected internal override int Length
        {
            get { return _sb.Length; }
            set { _sb.Length = value; }
        }

        public override MarkdownWriter WriteString(string value)
        {
            ThrowIfClosed();

            if (string.IsNullOrEmpty(value))
                return this;

            int length = value.Length;

            int prev = 0;

            int i = 0;
            while (i < length)
            {
                char ch = value[i];

                if (ch == 10)
                {
                    if (NewLineHandling == NewLineHandling.Replace)
                    {
                        WriteString(value, prev, i - prev);
                        WriteLine();
                    }
                    else if (NewLineHandling == NewLineHandling.None)
                    {
                        WriteString(value, prev, i + 1 - prev);
                        OnAfterWriteLine();
                    }

                    prev = ++i;
                }
                else if (ch == 13)
                {
                    if (i < length - 1
                        && value[i + 1] == 10)
                    {
                        if (NewLineHandling == NewLineHandling.Replace)
                        {
                            WriteString(value, prev, i - prev);
                            WriteLine();
                        }
                        else if (NewLineHandling == NewLineHandling.None)
                        {
                            WriteString(value, prev, i + 2 - prev);
                            OnAfterWriteLine();
                        }

                        i++;
                    }
                    else if (NewLineHandling == NewLineHandling.Replace)
                    {
                        WriteString(value, prev, i - prev);
                        WriteLine();
                    }
                    else if (NewLineHandling == NewLineHandling.None)
                    {
                        WriteString(value, prev, i + 1 - prev);
                        OnAfterWriteLine();
                    }

                    prev = ++i;
                }
                else if (ShouldBeEscaped(ch))
                {
                    WriteString(value, prev, i - prev);
                    WriteChar(EscapingChar);
                    WriteChar(ch);
                    prev = ++i;
                }
                else
                {
                    i++;
                }
            }

            WriteString(value, prev, value.Length - prev);
            return this;
        }

        private void WriteString(string value, int startIndex, int count)
        {
            ThrowIfClosed();
            OnBeforeWrite();
            _sb.Append(value, startIndex, count);
        }

        private void WriteChar(char ch)
        {
            ThrowIfClosed();
            OnBeforeWrite();
            _sb.Append(ch);
        }

        public override MarkdownWriter WriteRaw(string value)
        {
            ThrowIfClosed();
            OnBeforeWrite();
            _sb.Append(value);
            return this;
        }

        public override MarkdownWriter WriteLine()
        {
            ThrowIfClosed();
            _sb.Append(Settings.NewLineChars);
            OnAfterWriteLine();
            return this;
        }

        public override void WriteValue(int value)
        {
            ThrowIfClosed();
            _sb.Append(value.ToString(_formatProvider));
        }

        public override void WriteValue(long value)
        {
            ThrowIfClosed();
            _sb.Append(value.ToString(_formatProvider));
        }

        public override void WriteValue(float value)
        {
            ThrowIfClosed();
            _sb.Append(value.ToString(_formatProvider));
        }

        public override void WriteValue(double value)
        {
            ThrowIfClosed();
            _sb.Append(value.ToString(_formatProvider));
        }

        public override void WriteValue(decimal value)
        {
            ThrowIfClosed();
            _sb.Append(value.ToString(_formatProvider));
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        protected internal override IReadOnlyList<TableColumnInfo> AnalyzeTable(IEnumerable<MElement> rows)
        {
            return TableAnalyzer.Analyze(rows, Settings, _formatProvider)?.AsReadOnly();
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