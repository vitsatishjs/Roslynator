// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Pihrtsoft.Markdown
{
    public class MarkdownFormat : IEquatable<MarkdownFormat>
    {
        public MarkdownFormat(
            EmphasisStyle boldStyle = EmphasisStyle.Asterisk,
            EmphasisStyle italicStyle = EmphasisStyle.Asterisk,
            ListItemStyle listItemStyle = ListItemStyle.Asterisk,
            OrderedListItemStyle orderedListItemStyle = OrderedListItemStyle.Dot,
            HorizontalRule horizontalRule = default(HorizontalRule),
            HeadingStyle headingStyle = HeadingStyle.NumberSign,
            HeadingOptions headingOptions = HeadingOptions.EmptyLineBeforeAndAfter,
            TableOptions tableOptions = TableOptions.FormatHeader | TableOptions.OuterDelimiter | TableOptions.Padding,
            CodeFenceStyle codeFenceStyle = CodeFenceStyle.Backtick,
            CodeBlockOptions codeBlockOptions = CodeBlockOptions.EmptyLineBeforeAndAfter,
            CharacterReferenceFormat characterReferenceFormat = CharacterReferenceFormat.Hexadecimal)
        {
            BoldStyle = boldStyle;
            ItalicStyle = italicStyle;
            ListItemStyle = listItemStyle;
            OrderedListItemStyle = orderedListItemStyle;
            HorizontalRule = horizontalRule;
            HeadingStyle = headingStyle;
            HeadingOptions = headingOptions;
            CodeFenceStyle = codeFenceStyle;
            CodeBlockOptions = codeBlockOptions;
            TableOptions = tableOptions;
            CharacterReferenceFormat = characterReferenceFormat;
        }

        public static MarkdownFormat Default { get; } = new MarkdownFormat();

        internal static MarkdownFormat DefaultWithTableOuterDelimiter { get; } = Default.WithTableOptions(Default.TableOptions | TableOptions.OuterDelimiter);

        public EmphasisStyle BoldStyle { get; }

        public EmphasisStyle AlternativeBoldStyle => GetAlternativeEmphasisStyle(BoldStyle);

        public EmphasisStyle ItalicStyle { get; }

        public EmphasisStyle AlternativeItalicStyle => GetAlternativeEmphasisStyle(ItalicStyle);

        public ListItemStyle ListItemStyle { get; }

        public OrderedListItemStyle OrderedListItemStyle { get; }

        public HorizontalRule HorizontalRule { get; }

        public HeadingStyle HeadingStyle { get; }

        public HeadingOptions HeadingOptions { get; }

        internal bool EmptyLineBeforeHeading => (HeadingOptions & HeadingOptions.EmptyLineBefore) != 0;

        internal bool EmptyLineAfterHeading => (HeadingOptions & HeadingOptions.EmptyLineAfter) != 0;

        public CodeFenceStyle CodeFenceStyle { get; }

        public CodeBlockOptions CodeBlockOptions { get; }

        internal bool EmptyLineBeforeCodeBlock => (CodeBlockOptions & CodeBlockOptions.EmptyLineBefore) != 0;

        internal bool EmptyLineAfterCodeBlock => (CodeBlockOptions & CodeBlockOptions.EmptyLineAfter) != 0;

        public TableOptions TableOptions { get; }

        internal bool TablePadding => (TableOptions & TableOptions.Padding) != 0;

        internal bool TableOuterDelimiter => (TableOptions & TableOptions.OuterDelimiter) != 0;

        internal bool UnderlineHeading => (HeadingOptions & HeadingOptions.Underline) != 0;

        internal bool UnderlineHeading1 => (HeadingOptions & HeadingOptions.UnderlineH1) != 0;

        internal bool UnderlineHeading2 => (HeadingOptions & HeadingOptions.UnderlineH2) != 0;

        internal bool CloseHeading => (HeadingOptions & HeadingOptions.Close) != 0;

        public CharacterReferenceFormat CharacterReferenceFormat { get; }

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
            return Equals(obj as MarkdownFormat);
        }

        public bool Equals(MarkdownFormat other)
        {
            return other != null
                && BoldStyle == other.BoldStyle
                && ItalicStyle == other.ItalicStyle
                && ListItemStyle == other.ListItemStyle
                && OrderedListItemStyle == other.OrderedListItemStyle
                && HorizontalRule == other.HorizontalRule
                && HeadingStyle == other.HeadingStyle
                && HeadingOptions == other.HeadingOptions
                && CodeFenceStyle == other.CodeFenceStyle
                && CodeBlockOptions == other.CodeBlockOptions
                && TableOptions == other.TableOptions
                && CharacterReferenceFormat == other.CharacterReferenceFormat;
        }

        public override int GetHashCode()
        {
            int hashCode = Hash.OffsetBasis;
            hashCode = Hash.Combine((int)BoldStyle, hashCode);
            hashCode = Hash.Combine((int)ItalicStyle, hashCode);
            hashCode = Hash.Combine((int)ListItemStyle, hashCode);
            hashCode = Hash.Combine((int)OrderedListItemStyle, hashCode);
            hashCode = Hash.Combine(HorizontalRule.GetHashCode(), hashCode);
            hashCode = Hash.Combine((int)HeadingStyle, hashCode);
            hashCode = Hash.Combine((int)HeadingOptions, hashCode);
            hashCode = Hash.Combine((int)CodeFenceStyle, hashCode);
            hashCode = Hash.Combine((int)CodeBlockOptions, hashCode);
            hashCode = Hash.Combine((int)TableOptions, hashCode);
            hashCode = Hash.Combine((int)CharacterReferenceFormat, hashCode);
            return hashCode;
        }

        public static bool operator ==(MarkdownFormat format1, MarkdownFormat format2)
        {
            return EqualityComparer<MarkdownFormat>.Default.Equals(format1, format2);
        }

        public static bool operator !=(MarkdownFormat format1, MarkdownFormat format2)
        {
            return !(format1 == format2);
        }

        public MarkdownFormat WithBoldStyle(EmphasisStyle boldStyle)
        {
            return new MarkdownFormat(
                boldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HorizontalRule,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharacterReferenceFormat);
        }

        public MarkdownFormat WithItalicStyle(EmphasisStyle italicStyle)
        {
            return new MarkdownFormat(
                BoldStyle,
                italicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HorizontalRule,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharacterReferenceFormat);
        }

        public MarkdownFormat WithListItemStyle(ListItemStyle listItemStyle)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                listItemStyle,
                OrderedListItemStyle,
                HorizontalRule,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharacterReferenceFormat);
        }

        public MarkdownFormat WithOrderedListItemStyle(OrderedListItemStyle orderedListItemStyle)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                orderedListItemStyle,
                HorizontalRule,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharacterReferenceFormat);
        }

        public MarkdownFormat WithHorizontalRule(HorizontalRule horizontalRule)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                horizontalRule,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharacterReferenceFormat);
        }

        public MarkdownFormat WithHeadingOptions(HeadingStyle headingStyle)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HorizontalRule,
                headingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharacterReferenceFormat);
        }

        public MarkdownFormat WithHeadingOptions(HeadingOptions headingOptions)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HorizontalRule,
                HeadingStyle,
                headingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharacterReferenceFormat);
        }

        public MarkdownFormat WithTableOptions(TableOptions tableOptions)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HorizontalRule,
                HeadingStyle,
                HeadingOptions,
                tableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharacterReferenceFormat);
        }

        public MarkdownFormat WithCodeFenceStyle(CodeFenceStyle codeFenceStyle)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HorizontalRule,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                codeFenceStyle,
                CodeBlockOptions,
                CharacterReferenceFormat);
        }

        public MarkdownFormat WithCodeBlockOptions(CodeBlockOptions codeBlockOptions)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HorizontalRule,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                codeBlockOptions,
                CharacterReferenceFormat);
        }

        public MarkdownFormat WithCharacterReferenceFormat(CharacterReferenceFormat characterReferenceFormat)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HorizontalRule,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                characterReferenceFormat);
        }
    }
}
