// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;

namespace Pihrtsoft.Markdown
{
    internal class MarkdownStringWriter : MarkdownWriter
    {
        private bool _isOpen;

        public MarkdownStringWriter(StringBuilder sb = null, MarkdownWriterSettings settings = null)
            : base(settings)
        {
            StringBuilder = sb ?? new StringBuilder();
            _isOpen = true;
        }

        public StringBuilder StringBuilder { get; }

        protected override int Length
        {
            get { return StringBuilder.Length; }
            set { StringBuilder.Length = value; }
        }

        protected override void WriteCore(string value)
        {
            ThrowIfClosed();

            StringBuilder.Append(value);
        }

        protected override void WriteCore(string value, int startIndex, int count)
        {
            ThrowIfClosed();

            StringBuilder.Append(value, startIndex, count);
        }

        protected override void WriteCore(char value)
        {
            ThrowIfClosed();

            StringBuilder.Append(value);
        }

        private void ThrowIfClosed()
        {
            if (!_isOpen)
                throw new ObjectDisposedException(null, "Cannot write to a closed TextWriter.");
        }

        public override void Close()
        {
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            _isOpen = false;
            base.Dispose(disposing);
        }

        public void Clear()
        {
            StringBuilder.Clear();
            ResetState();
        }
    }
}