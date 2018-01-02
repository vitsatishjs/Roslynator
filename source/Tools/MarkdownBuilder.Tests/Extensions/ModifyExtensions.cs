// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

namespace Pihrtsoft.Markdown.Tests
{
    internal static class ModifyExtensions
    {
        public static string Modify(this string value)
        {
            return value + "x";
        }

        public static int Modify(this int value)
        {
            int newValue = 0;

            do
            {
                newValue = IntValue();

            } while (newValue == value);

            return newValue;
        }

        public static int Modify(this int value, int maxValue)
        {
            int newValue = 0;

            do
            {
                newValue = IntValue(maxValue);

            } while (newValue == value);

            return newValue;
        }

        public static int Modify(this int value, int minValue, int maxValue)
        {
            int newValue = 0;

            do
            {
                newValue = IntValue(minValue, maxValue);

            } while (newValue == value);

            return newValue;
        }

        public static bool Modify(this bool value)
        {
            return !value;
        }

        public static CodeBlock Modify(this CodeBlock codeBlock)
        {
            return new CodeBlock(codeBlock.Text.Modify(), codeBlock.Language.Modify());
        }

        public static Heading Modify(this Heading heading)
        {
            return new Heading(heading.Text.Modify(), heading.Level.Modify(1, 6));
        }

        public static HorizontalRule Modify(this HorizontalRule horizontalRule)
        {
            return new HorizontalRule(horizontalRule.Style.Modify(), horizontalRule.Count.Modify(3, 10), horizontalRule.AddSpaces.Modify());
        }

        public static Image Modify(this Image image)
        {
            return new Image(image.Text.Modify(), image.Url.Modify(), image.Title.Modify());
        }

        public static Link Modify(this Link link)
        {
            return new Link(link.Text.Modify(), link.Url.Modify(), link.Title.Modify());
        }

        public static ListItem Modify(this ListItem item)
        {
            return new ListItem(item.Text.Modify());
        }

        public static MarkdownText Modify(this MarkdownText text)
        {
            return new MarkdownText(text.Text.Modify(), text.Options.Modify(), text.Escape.Modify());
        }

        public static OrderedListItem Modify(this OrderedListItem item)
        {
            return new OrderedListItem(item.Number.Modify(0, 10), item.Text.Modify());
        }

        public static QuoteBlock Modify(this QuoteBlock quoteBlock)
        {
            return new QuoteBlock(quoteBlock.Text.Modify());
        }

        public static TableColumn Modify(this TableColumn column)
        {
            return new TableColumn(column.Name.Modify(), column.Alignment.Modify());
        }

        public static TaskListItem Modify(this TaskListItem item)
        {
            return new TaskListItem(item.Text.Modify(), item.IsCompleted.Modify());
        }

        public static Alignment Modify(this Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Left:
                    return Alignment.Center;
                case Alignment.Center:
                    return Alignment.Right;
                case Alignment.Right:
                    return Alignment.Left;
                default:
                    throw new ArgumentException("", nameof(alignment));
            }
        }

        public static HorizontalRuleStyle Modify(this HorizontalRuleStyle style)
        {
            switch (style)
            {
                case HorizontalRuleStyle.Hyphen:
                    return HorizontalRuleStyle.Asterisk;
                case HorizontalRuleStyle.Asterisk:
                    return HorizontalRuleStyle.Underscore;
                case HorizontalRuleStyle.Underscore:
                    return HorizontalRuleStyle.Hyphen;
                default:
                    throw new ArgumentException("", nameof(style));
            }
        }

        public static EmphasisOptions Modify(this EmphasisOptions options)
        {
            switch (options)
            {
                case EmphasisOptions.None:
                    return EmphasisOptions.Bold;
                default:
                    return EmphasisOptions.None;
            }
        }
    }
}
