// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Pihrtsoft.Markdown.Linq;

namespace Pihrtsoft.Markdown
{
    internal class MarkdownTextWriter : MarkdownWriter
    {
        private const int BufferSize = 1024;

        private TextWriter _writer;
        private readonly char[] _buffer;

        public MarkdownTextWriter(TextWriter writer, MarkdownWriterSettings settings)
            : base(settings)
        {
            Debug.Assert(writer != null);

            _writer = writer;

            _buffer = new char[BufferSize];
        }

        protected internal override int Length { get; set; }

        protected override void WriteString(string value)
        {
            _writer.Write(value);

            if (value != null)
                Length += value.Length;
        }

        // https://github.com/dotnet/corefx/issues/1571
        protected override void WriteString(string value, int startIndex, int count)
        {
            if (value == null)
                return;

            while (count > 0)
            {
                int charCount = Math.Min(BufferSize, count);

                for (int i = 0; i < charCount; i++)
                    _buffer[i] = value[i + startIndex];

                _writer.Write(_buffer, 0, charCount);
                Length += charCount;
                count -= charCount;
                startIndex += charCount;
            }
        }

        protected override void WriteValue(char value)
        {
            _writer.Write(value);
            Length++;
        }

        protected override void WriteValue(int value)
        {
            _writer.Write(value);
        }

        protected override void WriteValue(uint value)
        {
            _writer.Write(value);
        }

        protected override void WriteValue(long value)
        {
            _writer.Write(value);
        }

        protected override void WriteValue(ulong value)
        {
            _writer.Write(value);
        }

        protected override void WriteValue(float value)
        {
            _writer.Write(value);
        }

        protected override void WriteValue(double value)
        {
            _writer.Write(value);
        }

        protected override void WriteValue(decimal value)
        {
            _writer.Write(value);
        }

        public override void Close()
        {
            try
            {
                _writer.Flush();
            }
            finally
            {
                try
                {
                    if (Settings.CloseOutput)
                    {
                        _writer.Dispose();
                    }
                }
                finally
                {
                    _writer = null;
                }
            }
        }

        protected override List<TableColumnInfo> AnalyzeTable(IEnumerable<MElement> rows)
        {
            return TableAnalyzer.Analyze(rows, Settings, _writer.FormatProvider);
        }
    }
}