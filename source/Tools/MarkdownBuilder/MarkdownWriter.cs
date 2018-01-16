// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Pihrtsoft.Markdown.Linq;
using static Pihrtsoft.Markdown.Linq.MFactory;
using static Pihrtsoft.Markdown.TextUtility;

#pragma warning disable CA1814

namespace Pihrtsoft.Markdown
{
    public abstract class MarkdownWriter : IDisposable
    {
        private bool _disposed;
        private bool _startOfLine = true;
        private bool _emptyLine;
        private bool _pendingEmptyLine;

        private int _headingPosition = -1;
        private int _headingLevel = -1;

        private readonly Stack<MarkdownKind> _containers = new Stack<MarkdownKind>();

        private static readonly int[,] _states = new int[32, 32] {
        //                       None                       Text                       Raw                        Link                       LinkReference              Image                      ImageReference             Autolink                   InlineCode                 CharReference              EntityReference            Comment                    FencedCodeBlock            IndentedCodeBlock          HorizontalRule             Label                      InlineContainer            Bold                       Italic                     Strikethrough              Heading                    Table                      TableRow                   TableColumn                Document                   BlockQuote                 BulletList                 BulletItem                 OrderedList                OrderedItem                TaskList                   TaskItem
        /* None              */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Text              */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Raw               */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Link              */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* LinkReference     */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Image             */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* ImageReference    */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Autolink          */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* InlineCode        */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* CharReference     */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* EntityReference   */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Comment           */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* FencedCodeBlock   */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* IndentedCodeBlock */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* HorizontalRule    */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Label             */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* InlineContainer   */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Bold              */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Italic            */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Strikethrough     */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Heading           */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Table             */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* TableRow          */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* TableColumn       */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* Document          */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* BlockQuote        */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* BulletList        */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* BulletItem        */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* OrderedList       */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* OrderedItem       */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* TaskList          */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 },
        /* TaskItem          */ {0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0, /*                   */ 0 }
        };

        protected MarkdownWriter(MarkdownWriterSettings settings = null)
        {
            Settings = settings ?? MarkdownWriterSettings.Default;
        }

        public virtual MarkdownWriterSettings Settings { get; internal set; }

        public MarkdownFormat Format
        {
            get { return Settings.Format; }
        }

        private MarkdownKind CurrentKind
        {
            get { return (_containers.Count > 0) ? _containers.Peek() : MarkdownKind.None; }
        }

        public int QuoteLevel { get; private set; }

        internal int ListLevel { get; private set; }

        protected internal abstract int Length { get; set; }

        public static MarkdownWriter Create(StringBuilder output, MarkdownWriterSettings settings = null)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            return new MarkdownTextWriter(new StringWriter(output, CultureInfo.InvariantCulture), settings);
        }

        public static MarkdownWriter Create(TextWriter output, MarkdownWriterSettings settings = null)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            return new MarkdownTextWriter(output, settings);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    Close();

                _disposed = true;
            }
        }

        public virtual void Close()
        {
            Dispose();
        }

        internal virtual void Reset()
        {
            Length = 0;
            _startOfLine = true;
        }

        private void PushCheck(MarkdownKind kind)
        {
            Check(kind);
            _containers.Push(kind);
        }

        private void Pop(MarkdownKind kind)
        {
            _containers.Pop();
        }

        internal void Check(MarkdownKind kind)
        {
        }

        public MarkdownWriter WriteBoldStart()
        {
            PushCheck(MarkdownKind.Bold);
            WriteSyntax(BoldDelimiter(Format.BoldStyle));
            return this;
        }

        public MarkdownWriter WriteBoldEnd()
        {
            WriteSyntax(BoldDelimiter(Format.BoldStyle));
            Pop(MarkdownKind.Bold);
            return this;
        }

        public MarkdownWriter WriteBold(string content)
        {
            WriteBoldStart();
            Write(content);
            WriteBoldEnd();
            return this;
        }

        public MarkdownWriter WriteItalicStart()
        {
            PushCheck(MarkdownKind.Italic);
            WriteSyntax(ItalicDelimiter(Format.ItalicStyle));
            return this;
        }

        public MarkdownWriter WriteItalicEnd()
        {
            WriteSyntax(ItalicDelimiter(Format.ItalicStyle));
            Pop(MarkdownKind.Italic);
            return this;
        }

        public MarkdownWriter WriteItalic(string content)
        {
            WriteItalicStart();
            Write(content);
            WriteItalicEnd();
            return this;
        }

        public MarkdownWriter WriteStrikethroughStart()
        {
            PushCheck(MarkdownKind.Strikethrough);
            WriteSyntax(StrikethroughDelimiter);
            return this;
        }

        public MarkdownWriter WriteStrikethroughEnd()
        {
            WriteSyntax(StrikethroughDelimiter);
            Pop(MarkdownKind.Strikethrough);
            return this;
        }

        public MarkdownWriter WriteStrikethrough(string content)
        {
            WriteStrikethroughStart();
            Write(content);
            WriteStrikethroughEnd();
            return this;
        }

        public MarkdownWriter WriteInlineCode(string value)
        {
            Check(MarkdownKind.InlineCode);
            WriteSyntax(CodeDelimiter);

            if (!string.IsNullOrEmpty(value))
            {
                if (value[0] == CodeDelimiterChar)
                    WriteSpace();

                Write(value, ch => ch == CodeDelimiterChar, CodeDelimiterChar);

                if (value[value.Length - 1] == CodeDelimiterChar)
                    WriteSpace();
            }

            WriteSyntax(CodeDelimiter);
            return this;
        }

        public MarkdownWriter WriteHeadingStart(int level)
        {
            Error.ThrowOnInvalidHeadingLevel(level);

            PushCheck(MarkdownKind.Heading);

            bool underline = (level == 1 && Format.UnderlineHeading1)
                || (level == 2 && Format.UnderlineHeading2);

            WriteLineIfNecessary();
            WriteEmptyLineIf(Format.EmptyLineBeforeHeading);

            if (!underline)
            {
                WriteRaw(HeadingStartChar(Format.HeadingStyle), level);
                WriteSpace();
            }

            return this;
        }

        public MarkdownWriter WriteHeadingEnd()
        {
            int level = _headingLevel;
            _headingLevel = -1;

            bool underline = (level == 1 && Format.UnderlineHeading1)
                || (level == 2 && Format.UnderlineHeading2);

            if (!underline
                && Format.CloseHeading)
            {
                WriteSpace();
                WriteRaw(HeadingStartChar(Format.HeadingStyle), level);
            }

            WriteLineIfNecessary();

            if (underline)
            {
                WriteRaw((level == 1) ? '=' : '-', Length - _headingPosition);
                _headingPosition = -1;
                WriteLine();
            }

            PendingLineIf(Format.EmptyLineAfterHeading);
            return this;
        }

        public MarkdownWriter WriteHeading1(string content)
        {
            return WriteHeading(1, content);
        }

        public MarkdownWriter WriteHeading2(string content)
        {
            return WriteHeading(2, content);
        }

        public MarkdownWriter WriteHeading3(string content)
        {
            return WriteHeading(3, content);
        }

        public MarkdownWriter WriteHeading4(string content)
        {
            return WriteHeading(4, content);
        }

        public MarkdownWriter WriteHeading5(string content)
        {
            return WriteHeading(5, content);
        }

        public MarkdownWriter WriteHeading6(string content)
        {
            return WriteHeading(6, content);
        }

        public MarkdownWriter WriteHeading(int level, string content)
        {
            WriteHeadingStart(level);
            Write(content);
            WriteHeadingEnd();
            return this;
        }

        public MarkdownWriter WriteBulletItemStart()
        {
            PushCheck(MarkdownKind.BulletItem);
            WriteLineIfNecessary();
            WriteSyntax(BulletItemStart(Format.BulletListStyle));
            ListLevel++;
            return this;
        }

        public MarkdownWriter WriteBulletItemEnd()
        {
            Pop(MarkdownKind.BulletItem);
            WriteLineIfNecessary();
            ListLevel--;
            return this;
        }

        public MarkdownWriter WriteBulletItem(string content)
        {
            WriteBulletItemStart();
            Write(content);
            WriteBulletItemEnd();
            return this;
        }

        public MarkdownWriter WriteOrderedItemStart(int number)
        {
            Error.ThrowOnInvalidItemNumber(number);
            PushCheck(MarkdownKind.OrderedItem);
            WriteLineIfNecessary();
            WriteValue(number);
            WriteSyntax(OrderedItemStart(Format.OrderedListStyle));
            ListLevel++;
            return this;
        }

        public MarkdownWriter WriteOrderedItemEnd()
        {
            Pop(MarkdownKind.OrderedItem);
            WriteLineIfNecessary();
            ListLevel--;
            return this;
        }

        public MarkdownWriter WriteOrderedItem(int number, string content)
        {
            Error.ThrowOnInvalidItemNumber(number);

            WriteOrderedItemStart(number);
            Write(content);
            WriteOrderedItemEnd();
            return this;
        }

        public MarkdownWriter WriteTaskItemStart(bool isCompleted = false)
        {
            PushCheck(MarkdownKind.TaskItem);
            WriteLineIfNecessary();
            WriteSyntax(TaskItemStart(isCompleted: isCompleted));
            ListLevel++;
            return this;
        }

        public MarkdownWriter WriteCompletedTaskItemStart()
        {
            return WriteTaskItemStart(isCompleted: true);
        }

        public MarkdownWriter WriteTaskItemEnd()
        {
            Pop(MarkdownKind.TaskItem);
            WriteLineIfNecessary();
            ListLevel--;
            return this;
        }

        public MarkdownWriter WriteTaskItem(string content)
        {
            WriteTaskItemStart();
            Write(content);
            WriteTaskItemEnd();
            return this;
        }

        public MarkdownWriter WriteCompletedTaskItem(string content)
        {
            WriteCompletedTaskItemStart();
            Write(content);
            WriteTaskItemEnd();
            return this;
        }

        public MarkdownWriter WriteImage(string text, string url, string title = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            Error.ThrowOnInvalidUrl(url);

            Check(MarkdownKind.Image);
            WriteSyntax("!");
            WriteLinkCore(text, url, title);
            return this;
        }

        public MarkdownWriter WriteLink(string text, string url, string title = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            Error.ThrowOnInvalidUrl(url);

            Check(MarkdownKind.Link);
            WriteLinkCore(text, url, title);
            return this;
        }

        public MarkdownWriter WriteLinkOrText(string text, string url = null, string title = null)
        {
            if (!string.IsNullOrEmpty(url))
            {
                WriteLink(text, url, title);
            }
            else
            {
                Write(text);
            }

            return this;
        }

        private MarkdownWriter WriteLinkCore(string text, string url, string title)
        {
            WriteSquareBrackets(text);
            WriteSyntax("(");
            Write(url, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkUrl);
            WriteLinkTitle(title);
            WriteSyntax(")");
            return this;
        }

        public MarkdownWriter WriteAutolink(string url)
        {
            Error.ThrowOnInvalidUrl(url);

            Check(MarkdownKind.Autolink);
            WriteAngleBrackets(url);
            return this;
        }

        public MarkdownWriter WriteImageReference(string text, string label)
        {
            Check(MarkdownKind.ImageReference);
            WriteSyntax("!");
            WriteLinkReferenceCore(text, label);
            return this;
        }

        public MarkdownWriter WriteLinkReference(string text, string label = null)
        {
            Check(MarkdownKind.LinkReference);
            WriteLinkReferenceCore(text, label);
            return this;
        }

        private MarkdownWriter WriteLinkReferenceCore(string text, string label = null)
        {
            WriteSquareBrackets(text);
            WriteSquareBrackets(label);
            return this;
        }

        public MarkdownWriter WriteLabel(string label, string url, string title = null)
        {
            Error.ThrowOnInvalidUrl(url);

            Check(MarkdownKind.Label);
            WriteLineIfNecessary();
            WriteSquareBrackets(label);
            WriteSyntax(": ");
            WriteAngleBrackets(url);
            WriteLinkTitle(title);
            WriteLineIfNecessary();
            return this;
        }

        private void WriteLinkTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                WriteSpace();

                WriteSyntax("\"");
                Write(title, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkTitle);
                WriteSyntax("\"");
            }
        }

        private void WriteSquareBrackets(string value)
        {
            WriteSyntax("[");
            Write(value, shouldBeEscaped: MarkdownEscaper.ShouldBeEscapedInLinkText);
            WriteSyntax("]");
        }

        private void WriteAngleBrackets(string value)
        {
            WriteSyntax("<");
            Write(value, shouldBeEscaped: ch => ch == '<' || ch == '>');
            WriteSyntax(">");
        }

        public MarkdownWriter WriteIndentedCodeBlock(string text)
        {
            PushCheck(MarkdownKind.IndentedCodeBlock);
            WriteLineIfNecessary();
            WriteEmptyLineIf(Format.EmptyLineBeforeCodeBlock);
            WriteRaw(text);
            WriteLineIfNecessary();
            Pop(MarkdownKind.IndentedCodeBlock);
            PendingLineIf(Format.EmptyLineAfterCodeBlock);

            return this;
        }

        public MarkdownWriter WriteFencedCodeBlock(string text, string info = null)
        {
            Check(MarkdownKind.FencedCodeBlock);
            WriteLineIfNecessary();
            WriteEmptyLineIf(Format.EmptyLineBeforeCodeBlock);

            WriteCodeFence();
            WriteSyntax(info);
            WriteLine();

            WriteRaw(text);
            WriteLineIfNecessary();

            WriteCodeFence();
            WriteLine();

            PendingLineIf(Format.EmptyLineAfterCodeBlock);

            return this;

            MarkdownWriter WriteCodeFence()
            {
                switch (Format.CodeFenceStyle)
                {
                    case CodeFenceStyle.Backtick:
                        return WriteSyntax("```");
                    case CodeFenceStyle.Tilde:
                        return WriteSyntax("~~~");
                    default:
                        throw new InvalidOperationException(ErrorMessages.UnknownEnumValue(Format.CodeFenceStyle));
                }
            }
        }

        public void WriteBlockQuoteStart()
        {
            PushCheck(MarkdownKind.BlockQuote);
            WriteLineIfNecessary();
            QuoteLevel++;
        }

        public void WriteBlockQuoteEnd()
        {
            WriteLineIfNecessary();
            QuoteLevel--;
            Pop(MarkdownKind.BlockQuote);
        }

        public MarkdownWriter WriteBlockQuote(string content)
        {
            QuoteLevel++;
            Write(content);
            WriteLineIfNecessary();
            QuoteLevel--;
            return this;
        }

        public MarkdownWriter WriteHorizontalRule(HorizontalRuleStyle style = HorizontalRuleStyle.Hyphen, int count = 3, string space = " ")
        {
            Error.ThrowOnInvalidHorizontalRuleCount(count);

            Check(MarkdownKind.HorizontalRule);

            WriteLineIfNecessary();

            char ch = HorizontalRuleChar(style);

            bool isFirst = true;

            for (int i = 0; i < count; i++)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    WriteRaw(space);
                }

                WriteRaw(ch);
            }

            WriteLine();
            return this;
        }

        internal MarkdownWriter WriteTable(IEnumerable<MElement> rows)
        {
            List<TableColumnInfo> columns = AnalyzeTable(rows);

            if (columns == null)
                return this;

            using (IEnumerator<MElement> en = rows.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    WriteTableHeader(en.Current, columns);

                    while (en.MoveNext())
                        WriteTableRow(en.Current, columns);
                }
            }

            return this;
        }

        protected internal abstract List<TableColumnInfo> AnalyzeTable(IEnumerable<MElement> rows);

        internal void WriteTableHeader(MElement content, IList<TableColumnInfo> columns)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            bool isFirst = true;
            bool isLast = false;

            int i = 0;

            if (content is MContainer container)
            {
                using (IEnumerator<MElement> en = container.Elements().GetEnumerator())
                {
                    if (en.MoveNext())
                    {
                        MElement curr = en.Current;

                        isLast = !en.MoveNext();

                        WriteCell(curr);

                        if (!isLast)
                        {
                            isFirst = false;

                            do
                            {
                                curr = en.Current;
                                isLast = !en.MoveNext();
                                i++;
                                WriteCell(curr);
                            }
                            while (!isLast);
                        }
                    }
                }
            }
            else
            {
                isLast = true;
                WriteCell(content);
            }

            WriteLine();
            WriteTableHeaderSeparator(columns);

            void WriteCell(MElement cellContent)
            {
                Alignment alignment = columns[i].Alignment;

                WriteTableCellStart(isFirst, isLast, columns[i].IsWhiteSpace);

                if (Format.TablePadding)
                {
                    WriteSpace();
                }
                else if (Settings.FormatTableHeader
                    && alignment == Alignment.Center)
                {
                    WriteSpace();
                }

                int width = WriteGetLength(cellContent);

                if (Settings.FormatTableHeader)
                {
                    int minimalWidth = Math.Max(width, 3);

                    WritePadRight(width, columns[i].Width, minimalWidth);

                    if (!Format.TablePadding
                        && alignment != Alignment.Left)
                    {
                        WriteSpace();
                    }
                }

                WriteTableCellEnd(isLast, columns[i].IsWhiteSpace);
            }
        }

        private void WriteTableHeaderSeparator(IList<TableColumnInfo> columns)
        {
            int count = columns.Count;

            for (int i = 0; i < count; i++)
            {
                TableColumnInfo column = columns[i];

                bool isLast = i == count - 1;

                WriteTableCellStart(i == 0, isLast, column.IsWhiteSpace);

                if (column.Alignment == Alignment.Center)
                {
                    WriteSyntax(":");
                }
                else
                {
                    WriteTablePadding();
                }

                WriteSyntax("---");

                if (Settings.FormatTableHeader)
                    WritePadRight(3, columns[i].Width, 3, '-');

                if (column.Alignment != Alignment.Left)
                {
                    WriteSyntax(":");
                }
                else
                {
                    WriteTablePadding();
                }

                if (isLast)
                {
                    if (Format.TableOuterDelimiter
                        || columns[i].IsWhiteSpace)
                    {
                        WriteSyntax(TableDelimiter);
                    }
                }
            }

            WriteLine();
        }

        //TODO: columns = null
        internal MarkdownWriter WriteTableRow(MElement content, List<TableColumnInfo> columns = null)
        {
            bool isFirst = true;
            bool isLast = false;

            int i = 0;

            if (content is MContainer container)
            {
                using (IEnumerator<MElement> en = container.Elements().GetEnumerator())
                {
                    if (en.MoveNext())
                    {
                        MElement curr = en.Current;

                        isLast = !en.MoveNext();

                        WriteCell(curr);

                        if (!isLast)
                        {
                            isFirst = false;

                            do
                            {
                                curr = en.Current;
                                isLast = !en.MoveNext();
                                i++;
                                WriteCell(curr);
                            }
                            while (!isLast);
                        }
                    }
                }
            }
            else
            {
                isLast = true;
                WriteCell(content);
            }

            WriteLine();
            return this;

            void WriteCell(MElement cell)
            {
                WriteTableCellStart(isFirst, isLast);
                WriteTablePadding();

                int length = WriteGetLength(cell);

                if (Settings.FormatTableContent)
                    WritePadRight(length, columns[i].Width);

                WriteTableCellEnd(isLast);
            }
        }

        private void WriteTableCellStart(bool isFirst, bool isLast, bool isWhiteSpace = false)
        {
            if (isFirst)
            {
                WriteLineIfNecessary();

                if (Format.TableOuterDelimiter
                    || isLast
                    || isWhiteSpace)
                {
                    WriteSyntax(TableDelimiter);
                }
            }
            else
            {
                WriteSyntax(TableDelimiter);
            }
        }

        private void WriteTableCellEnd(bool isLast, bool isWhiteSpace = false)
        {
            if (isLast)
            {
                if (Format.TableOuterDelimiter)
                {
                    WriteTablePadding();
                    WriteSyntax(TableDelimiter);
                }
                else if (isWhiteSpace)
                {
                    WriteSyntax(TableDelimiter);
                }
            }
            else
            {
                WriteTablePadding();
            }
        }

        private void WriteTablePadding()
        {
            if (Format.TablePadding)
                WriteSpace();
        }

        private void WritePadRight(int width, int? proposedWidth, int minimalWidth = 0, char paddingChar = ' ')
        {
            int totalWidth = Math.Max(proposedWidth ?? width, minimalWidth);

            for (int j = width; j < totalWidth; j++)
                WriteRaw(paddingChar);
        }

        public MarkdownWriter WriteCharReference(int number)
        {
            Check(MarkdownKind.CharReference);
            WriteSyntax("&#");

            if (Format.CharReferenceFormat == CharReferenceFormat.Hexadecimal)
            {
                WriteSyntax("x");
                WriteRaw(number.ToString("x", CultureInfo.InvariantCulture));
            }
            else if (Format.CharReferenceFormat == CharReferenceFormat.Decimal)
            {
                WriteRaw(number.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                throw new InvalidOperationException(ErrorMessages.UnknownEnumValue(Format.CharReferenceFormat));
            }

            WriteSyntax(";");
            return this;
        }

        public MarkdownWriter WriteEntityReference(string name)
        {
            Check(MarkdownKind.EntityReference);
            WriteSyntax("&");
            WriteSyntax(name);
            WriteSyntax(";");
            return this;
        }

        public MarkdownWriter WriteComment(string value)
        {
            PushCheck(MarkdownKind.Comment);
            WriteSyntax("<!-- ");
            WriteRaw(value);
            WriteSyntax(" -->");
            Pop(MarkdownKind.Comment);
            return this;
        }

        private int WriteGetLength(object content)
        {
            int length = Length;
            Write(content);
            return Length - length;
        }

        public MarkdownWriter Write(char value)
        {
            return Write(value, escape: true);
        }

        public MarkdownWriter Write(char value, bool escape)
        {
            if (escape
                && MarkdownEscaper.ShouldBeEscaped(value))
            {
                return WriteRaw('\\');
            }

            return WriteRaw(value);
        }

        public MarkdownWriter Write(string value)
        {
            return Write(value, MarkdownEscaper.ShouldBeEscaped);
        }

        internal MarkdownWriter Write(string value, Func<char, bool> shouldBeEscaped, char escapingChar = '\\')
        {
            if (value == null)
                return this;

            BeforeWrite();

            int length = value.Length;

            bool f = false;

            for (int i = 0; i < length; i++)
            {
                char ch = value[i];

                if (ch == 10)
                {
                    WriteLine(0, i);
                    f = true;
                }
                else if (ch == 13
                    && (i == length - 1 || value[i + 1] != 10))
                {
                    WriteLine(0, i);
                    f = true;
                }
                else if (shouldBeEscaped(ch))
                {
                    WriteEscapedChar(0, i, ch);
                    f = true;
                }

                if (f)
                {
                    f = false;

                    i++;
                    int lastIndex = i;

                    while (i < value.Length)
                    {
                        ch = value[i];

                        if (ch == 10)
                        {
                            WriteLine(lastIndex, i);
                            f = true;
                        }
                        else if (ch == 13
                            && (i == length - 1 || value[i + 1] != 10))
                        {
                            WriteLine(0, i);
                            f = true;
                        }
                        else if (shouldBeEscaped(ch))
                        {
                            WriteEscapedChar(lastIndex, i, ch);
                            f = true;
                        }

                        if (f)
                        {
                            f = false;
                            i++;
                            lastIndex = i;
                        }
                        else
                        {
                            i++;
                        }
                    }

                    BeforeWrite();
                    WriteString(value, lastIndex, value.Length - lastIndex);
                    return this;
                }
            }

            BeforeWrite();
            WriteString(value);
            return this;

            void WriteLine(int startIndex, int index)
            {
                if (Settings.NewLineHandling == NewLineHandling.Replace)
                {
                    index--;
                    if (index > 0
                        && value[index - 1] == '\r')
                    {
                        index--;
                    }
                }

                BeforeWrite();
                WriteString(value, startIndex, index - startIndex);

                if (Settings.NewLineHandling == NewLineHandling.Replace)
                {
                    this.WriteLine();
                }
                else
                {
                    AfterWriteLine();
                }
            }

            void WriteEscapedChar(int startIndex, int index, char ch)
            {
                BeforeWrite();
                WriteString(value, startIndex, index - startIndex);
                WriteValue(escapingChar);
                WriteValue(ch);
            }
        }

        internal MarkdownWriter Write(object value)
        {
            if (value == null)
                return this;

            if (value is MElement element)
                return element.WriteTo(this);

            if (value is string s)
                return Write(s);

            if (value is object[] arr)
            {
                foreach (object item in arr)
                    Write(item);

                return this;
            }

            if (value is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                    Write(item);

                return this;
            }

            return Write(value.ToString());
        }

        public MarkdownWriter WriteLine(string value)
        {
            Write(value);
            WriteLine();
            return this;
        }

        internal void WriteLineIfNecessary()
        {
            if (!_startOfLine)
                WriteLine();
        }

        private void PendingLineIf(bool condition)
        {
            if (condition)
                _pendingEmptyLine = true;
        }

        private void WriteEmptyLineIf(bool condition)
        {
            if (condition)
                WriteEmptyLine();
        }

        private void WriteEmptyLine()
        {
            if (Length > 0
                && !_emptyLine)
            {
                WriteLine();
            }
        }

        private MarkdownWriter WriteRaw(char value)
        {
            Debug.Assert(!IsCarriageReturnOrLinefeed(value), value.ToString());

            BeforeWrite();
            WriteValue(value);
            return this;
        }

        private MarkdownWriter WriteRaw(char value, int repeatCount)
        {
            Debug.Assert(!IsCarriageReturnOrLinefeed(value), value.ToString());

            BeforeWrite();

            for (int i = 0; i < repeatCount; i++)
                WriteValue(value);

            return this;
        }

        private MarkdownWriter WriteSyntax(string value)
        {
            BeforeWrite();
            WriteString(value);
            _emptyLine = false;
            _startOfLine = false;
            return this;
        }

        public MarkdownWriter WriteRaw(string value)
        {
            Write(value, _ => false);
            return this;
        }

        public MarkdownWriter WriteRaw(int value)
        {
            BeforeWrite();
            WriteValue(value);
            return this;
        }

        public virtual MarkdownWriter WriteLine()
        {
            WriteString(Settings.NewLineChars);
            AfterWriteLine();
            return this;
        }

        private void AfterWriteLine()
        {
            _pendingEmptyLine = false;

            if (_startOfLine)
            {
                _emptyLine = true;
            }
            else
            {
                _startOfLine = true;
            }
        }

        private void BeforeWrite()
        {
            if (_pendingEmptyLine)
            {
                WriteIndentation();
                WriteLine();
            }
            else if (_startOfLine)
            {
                WriteIndentation();
                _startOfLine = false;
                _emptyLine = false;
            }
        }

        private void WriteIndentation()
        {
            if (CurrentKind == MarkdownKind.Comment)
                return;

            for (int i = 0; i < QuoteLevel; i++)
                WriteString(BlockQuoteStart);

            for (int i = 0; i < ListLevel; i++)
                WriteString("  ");

            if (CurrentKind == MarkdownKind.IndentedCodeBlock)
                WriteString("    ");
        }

        internal MarkdownWriter WriteSpace()
        {
            return WriteSyntax(" ");
        }

        protected abstract void WriteString(string value);

        protected abstract void WriteString(string value, int startIndex, int count);

        protected abstract void WriteValue(char value);

        protected abstract void WriteValue(int value);

        protected abstract void WriteValue(uint value);

        protected abstract void WriteValue(long value);

        protected abstract void WriteValue(ulong value);

        protected abstract void WriteValue(float value);

        protected abstract void WriteValue(double value);

        protected abstract void WriteValue(decimal value);
    }
}