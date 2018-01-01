// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Pihrtsoft.Markdown
{
    //TODO: MarkdownFormat
    public class MarkdownSettings : IEquatable<MarkdownSettings>
    {
        public MarkdownSettings(
            EmphasisStyle boldStyle = EmphasisStyle.Asterisk,
            EmphasisStyle italicStyle = EmphasisStyle.Asterisk,
            ListItemStyle listItemStyle = ListItemStyle.Asterisk,
            HorizontalRule horizontalRule = default(HorizontalRule),
            HeadingOptions headingOptions = HeadingOptions.EmptyLineBeforeAndAfter,
            TableOptions tableOptions = TableOptions.FormatHeader | TableOptions.OuterPipe | TableOptions.Padding,
            CodeBlockOptions codeBlockOptions = CodeBlockOptions.EmptyLineBeforeAndAfter,
            string indentChars = "  ")
        {
            BoldStyle = boldStyle;
            ItalicStyle = italicStyle;
            ListItemStyle = listItemStyle;
            HorizontalRule = horizontalRule;
            HeadingOptions = headingOptions;
            CodeBlockOptions = codeBlockOptions;
            TableOptions = tableOptions;
            IndentChars = indentChars;
        }

        public static MarkdownSettings Default { get; } = new MarkdownSettings();

        public EmphasisStyle BoldStyle { get; }

        public EmphasisStyle AlternativeBoldStyle => GetAlternativeEmphasisStyle(BoldStyle);

        public EmphasisStyle ItalicStyle { get; }

        public EmphasisStyle AlternativeItalicStyle => GetAlternativeEmphasisStyle(ItalicStyle);

        public ListItemStyle ListItemStyle { get; }

        public HorizontalRule HorizontalRule { get; }

        public HeadingOptions HeadingOptions { get; }

        internal bool EmptyLineBeforeHeading => (HeadingOptions & HeadingOptions.EmptyLineBefore) != 0;

        internal bool EmptyLineAfterHeading => (HeadingOptions & HeadingOptions.EmptyLineAfter) != 0;

        public CodeBlockOptions CodeBlockOptions { get; }

        internal bool EmptyLineBeforeCodeBlock => (CodeBlockOptions & CodeBlockOptions.EmptyLineBefore) != 0;

        internal bool EmptyLineAfterCodeBlock => (CodeBlockOptions & CodeBlockOptions.EmptyLineAfter) != 0;

        public TableOptions TableOptions { get; }

        internal bool TablePadding => (TableOptions & TableOptions.Padding) != 0;

        internal bool TableOuterPipe => (TableOptions & TableOptions.OuterPipe) != 0;

        internal bool UnderlineHeading => (HeadingOptions & HeadingOptions.Underline) != 0;

        internal bool UnderlineHeading1 => (HeadingOptions & HeadingOptions.UnderlineH1) != 0;

        internal bool UnderlineHeading2 => (HeadingOptions & HeadingOptions.UnderlineH2) != 0;

        internal bool CloseHeading => (HeadingOptions & HeadingOptions.Close) != 0;

        public string IndentChars { get; }

        private static EmphasisStyle GetAlternativeEmphasisStyle(EmphasisStyle style)
        {
            if (style == EmphasisStyle.Asterisk)
                return EmphasisStyle.Underscore;

            if (style == EmphasisStyle.Underscore)
                return EmphasisStyle.Asterisk;

            throw new ArgumentException(ErrorMessages.UnknownEnumValue(style), nameof(style));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MarkdownSettings);
        }

        public bool Equals(MarkdownSettings other)
        {
            return other != null
                && BoldStyle == other.BoldStyle
                && ItalicStyle == other.ItalicStyle
                && ListItemStyle == other.ListItemStyle
                && HorizontalRule == other.HorizontalRule
                && HeadingOptions == other.HeadingOptions
                && CodeBlockOptions == other.CodeBlockOptions
                && TableOptions == other.TableOptions
                && IndentChars == other.IndentChars;
        }

        public override int GetHashCode()
        {
            int hashCode = Hash.OffsetBasis;
            hashCode = Hash.Combine((int)BoldStyle, hashCode);
            hashCode = Hash.Combine((int)ItalicStyle, hashCode);
            hashCode = Hash.Combine((int)ListItemStyle, hashCode);
            hashCode = Hash.Combine(HorizontalRule.GetHashCode(), hashCode);
            hashCode = Hash.Combine((int)HeadingOptions, hashCode);
            hashCode = Hash.Combine((int)CodeBlockOptions, hashCode);
            hashCode = Hash.Combine((int)TableOptions, hashCode);
            hashCode = Hash.Combine(IndentChars, hashCode);
            return hashCode;
        }

        public static bool operator ==(MarkdownSettings settings1, MarkdownSettings settings2)
        {
            return EqualityComparer<MarkdownSettings>.Default.Equals(settings1, settings2);
        }

        public static bool operator !=(MarkdownSettings settings1, MarkdownSettings settings2)
        {
            return !(settings1 == settings2);
        }

        public MarkdownSettings WithBoldStyle(EmphasisStyle boldStyle)
        {
            return new MarkdownSettings(
                boldStyle,
                ItalicStyle,
                ListItemStyle,
                HorizontalRule,
                HeadingOptions,
                TableOptions,
                CodeBlockOptions,
                IndentChars);
        }

        public MarkdownSettings WithItalicStyle(EmphasisStyle italicStyle)
        {
            return new MarkdownSettings(
                BoldStyle,
                italicStyle,
                ListItemStyle,
                HorizontalRule,
                HeadingOptions,
                TableOptions,
                CodeBlockOptions,
                IndentChars);
        }

        public MarkdownSettings WithListItemStyle(ListItemStyle listItemStyle)
        {
            return new MarkdownSettings(
                BoldStyle,
                ItalicStyle,
                listItemStyle,
                HorizontalRule,
                HeadingOptions,
                TableOptions,
                CodeBlockOptions,
                IndentChars);
        }

        public MarkdownSettings WithHorizontalRule(HorizontalRule horizontalRule)
        {
            return new MarkdownSettings(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                horizontalRule,
                HeadingOptions,
                TableOptions,
                CodeBlockOptions,
                IndentChars);
        }

        public MarkdownSettings WithHeadingOptions(HeadingOptions headingOptions)
        {
            return new MarkdownSettings(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                HorizontalRule,
                headingOptions,
                TableOptions,
                CodeBlockOptions,
                IndentChars);
        }

        public MarkdownSettings WithTableOptions(TableOptions tableOptions)
        {
            return new MarkdownSettings(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                HorizontalRule,
                HeadingOptions,
                tableOptions,
                CodeBlockOptions,
                IndentChars);
        }

        public MarkdownSettings WithCodeBlockOptions(CodeBlockOptions codeBlockOptions)
        {
            return new MarkdownSettings(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                HorizontalRule,
                HeadingOptions,
                TableOptions,
                codeBlockOptions,
                IndentChars);
        }

        public MarkdownSettings WithIndentChars(string indentChars)
        {
            return new MarkdownSettings(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                HorizontalRule,
                HeadingOptions,
                TableOptions,
                CodeBlockOptions,
                indentChars);
        }
    }
}
