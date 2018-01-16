// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Text;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("Encoding = {Encoding.WebName,nq} CloseOutput = {CloseOutput}")]
    public class MarkdownWriterSettings
    {
        public MarkdownWriterSettings(
            MarkdownFormat format = null,
            Encoding encoding = null,
            string newLineChars = null,
            NewLineHandling newLineHandling = NewLineHandling.Replace,
            bool closeOutput = false)
        {
            Format = format ?? MarkdownFormat.Default;
            Encoding = encoding ?? Encoding.UTF8;
            NewLineChars = newLineChars ?? Environment.NewLine;
            NewLineHandling = newLineHandling;
            CloseOutput = closeOutput;
        }

        public static MarkdownWriterSettings Default { get; } = new MarkdownWriterSettings();

        internal static MarkdownWriterSettings Debugging { get; } = new MarkdownWriterSettings(MarkdownFormat.Debugging);

        public MarkdownFormat Format { get; }

        public Encoding Encoding { get; }

        public string NewLineChars { get; }

        public NewLineHandling NewLineHandling { get; }

        public bool CloseOutput { get; }

        internal BulletListStyle BulletListStyle => Format.BulletListStyle;

        internal OrderedListStyle OrderedListStyle => Format.OrderedListStyle;

        internal bool AddEmptyLineBeforeHeading => Format.EmptyLineBeforeHeading;

        internal bool AddEmptyLineAfterHeading => Format.EmptyLineAfterHeading;

        internal bool AddEmptyLineBeforeCodeBlock => Format.EmptyLineBeforeCodeBlock;

        internal bool AddEmptyLineAfterCodeBlock => Format.EmptyLineAfterCodeBlock;

        internal TableOptions TableOptions => Format.TableOptions;

        internal bool FormatTableHeader => Format.FormatTableHeader;

        internal bool FormatTableContent => Format.FormatTableContent;

        internal bool AddEmptyLineBeforeTable => Format.EmptyLineBeforeTable;

        internal bool AddEmptyLineAfterTable => Format.EmptyLineAfterTable;

        internal bool TableOuterDelimiter => Format.TableOuterDelimiter;

        internal bool TablePadding => Format.TablePadding;

        internal bool UnderlineHeading1 => Format.UnderlineHeading1;

        internal bool UnderlineHeading2 => Format.UnderlineHeading2;

        internal bool CloseHeading => Format.CloseHeading;

        internal HeadingStyle HeadingStyle => Format.HeadingStyle;

        internal static MarkdownWriterSettings From(MarkdownFormat format)
        {
            if (format == null
                || object.ReferenceEquals(format, MarkdownFormat.Default))
            {
                return Default;
            }

            return new MarkdownWriterSettings(format);
        }
    }
}
