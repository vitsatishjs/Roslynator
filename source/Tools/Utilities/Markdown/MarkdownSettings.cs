// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.Utilities.Markdown
{
    //TODO: Escaper
    //TODO: NewLine
    public class MarkdownSettings
    {
        public const EmphasisDelimiter DefaultEmphasisDelimiter = EmphasisDelimiter.Asterisk;

        public const string UnorderedListItemText = "*";

        //TODO: ?
        public const string InlineCodeDelimiterText = "`";

        public static MarkdownSettings Default { get; } = new MarkdownSettings();

        public EmphasisDelimiter BoldDelimiter { get; }

        public string BoldDelimiterText
        {
            get { return (BoldDelimiter == EmphasisDelimiter.Asterisk) ? "**" : "__"; }
        }

        public EmphasisDelimiter ItalicDelimiter { get; }

        public string ItalicDelimiterText
        {
            get { return (ItalicDelimiter == EmphasisDelimiter.Asterisk) ? "*" : "_"; }
        }

        public string Indentation { get; } = "\t";

        public HorizontalRule HorizontalRule { get; } = HorizontalRule.Default;

        //TODO: GetIndentation
        internal char GetIndentation(byte level)
        {
            throw new NotImplementedException();
        }
    }
}
