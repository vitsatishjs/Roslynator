// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Pihrtsoft.Markdown.Linq;

namespace Pihrtsoft.Markdown
{
    internal class MarkdownTextWriter : MarkdownWriter
    {
        private const int BufferSize = 1024 * 6;
        private const int BufferOverflow = 32;

        private TextWriter _writer;
        private readonly char[] _bufChars;
        //private int _bufPos;
        protected int _bufLen = BufferSize;
        private bool _skipWrite;

        public MarkdownTextWriter(TextWriter writer, MarkdownWriterSettings settings)
            : base(settings)
        {
            Debug.Assert(writer != null);

            _writer = writer;

            _bufChars = new char[_bufLen + BufferOverflow];
        }

        protected internal override int Length { get; set; }

        protected override void WriteValue(char value)
        {
            _writer.Write(value);
            Length++;
        }

        protected override void WriteString(string value)
        {
            _writer.Write(value);

            if (value != null)
                Length += value.Length;
        }

        public override void WriteString(string value, int startIndex, int count)
        {
            if (value == null)
                return;

            _writer.Write(value.ToCharArray(), startIndex, count);
            Length += count;
        }

        public override void WriteValue(int value)
        {
            _writer.Write(value);
        }

        public override void WriteValue(long value)
        {
            _writer.Write(value);
        }

        public override void WriteValue(float value)
        {
            _writer.Write(value);
        }

        public override void WriteValue(double value)
        {
            _writer.Write(value);
        }

        public override void WriteValue(decimal value)
        {
            _writer.Write(value);
        }

        public override void Flush()
        {
            FlushBuffer();

            _writer?.Flush();
        }

        protected virtual void FlushBuffer()
        {
            try
            {
                if (!_skipWrite)
                {
                    //_writer.Write(_bufChars, 0, _bufPos);
                }
            }
            catch
            {
                _skipWrite = true;
                throw;
            }
            finally
            {
                //_bufPos = 0;
            }
        }

        public override void Close()
        {
            try
            {
                FlushBuffer();
            }
            finally
            {
                _skipWrite = true;

                if (_writer != null)
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
                                _writer.Dispose();
                        }
                        finally
                        {
                            _writer = null;
                        }
                    }
                }
            }
        }

        protected internal override IReadOnlyList<TableColumnInfo> AnalyzeTable(IEnumerable<MElement> rows)
        {
            return TableAnalyzer.Analyze(rows, Settings, _writer.FormatProvider)?.AsReadOnly();
        }
    }
}