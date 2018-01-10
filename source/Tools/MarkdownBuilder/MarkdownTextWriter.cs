// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.IO;

namespace Pihrtsoft.Markdown
{
    internal class MarkdownTextWriter : MarkdownWriter
    {
        private TextWriter _writer;

        public MarkdownTextWriter(TextWriter writer, MarkdownWriterSettings settings)
            : base(settings)
        {
            Debug.Assert(writer != null);

            _writer = writer;
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

        protected override void WriteCore(string value)
        {
            _writer.Write(value);

            if (value != null)
                Length += value.Length;
        }

        protected override void WriteCore(string value, int startIndex, int count)
        {
            //TODO: optimize
            _writer.Write(value.ToCharArray(), startIndex, count);
            Length += count;
        }

        protected override void WriteCore(char value)
        {
            _writer.Write(value);
            Length++;
        }

        protected override void WriteCore(char value, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
                _writer.Write(value);

            Length += repeatCount;
        }
    }
}