﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            HeadingStyle headingStyle = HeadingStyle.NumberSign,
            HeadingOptions headingOptions = HeadingOptions.EmptyLineBeforeAndAfter,
            TableOptions tableOptions = TableOptions.FormatHeader | TableOptions.OuterDelimiter | TableOptions.Padding,
            CodeFenceStyle codeFenceStyle = CodeFenceStyle.Backtick,
            CodeBlockOptions codeBlockOptions = CodeBlockOptions.EmptyLineBeforeAndAfter,
            CharReferenceFormat charReferenceFormat = CharReferenceFormat.Hexadecimal,
            HorizontalRuleFormat? horizontalRuleFormat = null)
        {
            BoldStyle = boldStyle;
            ItalicStyle = italicStyle;
            ListItemStyle = listItemStyle;
            OrderedListItemStyle = orderedListItemStyle;
            HeadingStyle = headingStyle;
            HeadingOptions = headingOptions;
            TableOptions = tableOptions;
            CodeFenceStyle = codeFenceStyle;
            CodeBlockOptions = codeBlockOptions;
            CharReferenceFormat = charReferenceFormat;
            HorizontalRuleFormat = horizontalRuleFormat ?? HorizontalRuleFormat.Default;
        }

        public static MarkdownFormat Default { get; } = new MarkdownFormat();

        internal static MarkdownFormat DebugFormat { get; } = new MarkdownFormat(
            Default.BoldStyle,
            Default.ItalicStyle,
            Default.ListItemStyle,
            Default.OrderedListItemStyle,
            Default.HeadingStyle,
            HeadingOptions.None,
            TableOptions.OuterDelimiter,
            Default.CodeFenceStyle,
            CodeBlockOptions.None,
            Default.CharReferenceFormat,
            Default.HorizontalRuleFormat);

        public EmphasisStyle BoldStyle { get; }

        public EmphasisStyle AlternativeBoldStyle => GetAlternativeEmphasisStyle(BoldStyle);

        public EmphasisStyle ItalicStyle { get; }

        public EmphasisStyle AlternativeItalicStyle => GetAlternativeEmphasisStyle(ItalicStyle);

        public ListItemStyle ListItemStyle { get; }

        public OrderedListItemStyle OrderedListItemStyle { get; }

        public HorizontalRuleFormat HorizontalRuleFormat { get; }

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

        public CharReferenceFormat CharReferenceFormat { get; }

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
                && HeadingStyle == other.HeadingStyle
                && HeadingOptions == other.HeadingOptions
                && CodeFenceStyle == other.CodeFenceStyle
                && CodeBlockOptions == other.CodeBlockOptions
                && TableOptions == other.TableOptions
                && CharReferenceFormat == other.CharReferenceFormat
                && HorizontalRuleFormat == other.HorizontalRuleFormat;
        }

        public override int GetHashCode()
        {
            int hashCode = Hash.OffsetBasis;
            hashCode = Hash.Combine((int)BoldStyle, hashCode);
            hashCode = Hash.Combine((int)ItalicStyle, hashCode);
            hashCode = Hash.Combine((int)ListItemStyle, hashCode);
            hashCode = Hash.Combine((int)OrderedListItemStyle, hashCode);
            hashCode = Hash.Combine((int)HeadingStyle, hashCode);
            hashCode = Hash.Combine((int)HeadingOptions, hashCode);
            hashCode = Hash.Combine((int)CodeFenceStyle, hashCode);
            hashCode = Hash.Combine((int)CodeBlockOptions, hashCode);
            hashCode = Hash.Combine((int)TableOptions, hashCode);
            hashCode = Hash.Combine((int)CharReferenceFormat, hashCode);
            hashCode = Hash.Combine(HorizontalRuleFormat.GetHashCode(), hashCode);
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
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharReferenceFormat,
                HorizontalRuleFormat);
        }

        public MarkdownFormat WithItalicStyle(EmphasisStyle italicStyle)
        {
            return new MarkdownFormat(
                BoldStyle,
                italicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharReferenceFormat,
                HorizontalRuleFormat);
        }

        public MarkdownFormat WithListItemStyle(ListItemStyle listItemStyle)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                listItemStyle,
                OrderedListItemStyle,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharReferenceFormat,
                HorizontalRuleFormat);
        }

        public MarkdownFormat WithOrderedListItemStyle(OrderedListItemStyle orderedListItemStyle)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                orderedListItemStyle,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharReferenceFormat,
                HorizontalRuleFormat);
        }

        public MarkdownFormat WithHorizontalRuleFormat(HorizontalRuleFormat horizontalRuleFormat)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharReferenceFormat,
                horizontalRuleFormat);
        }

        public MarkdownFormat WithHeadingOptions(HeadingStyle headingStyle)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                headingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharReferenceFormat,
                HorizontalRuleFormat);
        }

        public MarkdownFormat WithHeadingOptions(HeadingOptions headingOptions)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HeadingStyle,
                headingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharReferenceFormat,
                HorizontalRuleFormat);
        }

        public MarkdownFormat WithTableOptions(TableOptions tableOptions)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HeadingStyle,
                HeadingOptions,
                tableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                CharReferenceFormat,
                HorizontalRuleFormat);
        }

        public MarkdownFormat WithCodeFenceStyle(CodeFenceStyle codeFenceStyle)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                codeFenceStyle,
                CodeBlockOptions,
                CharReferenceFormat,
                HorizontalRuleFormat);
        }

        public MarkdownFormat WithCodeBlockOptions(CodeBlockOptions codeBlockOptions)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                codeBlockOptions,
                CharReferenceFormat,
                HorizontalRuleFormat);
        }

        public MarkdownFormat WithCharReferenceFormat(CharReferenceFormat charReferenceFormat)
        {
            return new MarkdownFormat(
                BoldStyle,
                ItalicStyle,
                ListItemStyle,
                OrderedListItemStyle,
                HeadingStyle,
                HeadingOptions,
                TableOptions,
                CodeFenceStyle,
                CodeBlockOptions,
                charReferenceFormat,
                HorizontalRuleFormat);
        }
    }
}