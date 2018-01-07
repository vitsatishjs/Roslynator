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

        public static string ModifySpaces(this string value, int minValue, int maxValue)
        {
            string newValue = null;

            do
            {
                newValue = Spaces(minValue, maxValue);

            } while (newValue == value);

            return newValue;
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

        public static FencedCodeBlock Modify(this FencedCodeBlock block)
        {
            return new FencedCodeBlock(block.Text.Modify(), block.Info.Modify());
        }

        public static IndentedCodeBlock Modify(this IndentedCodeBlock block)
        {
            return new IndentedCodeBlock(block.Text.Modify());
        }

        public static Heading Modify(this Heading heading)
        {
            return new Heading(heading.Text.Modify(), heading.Level.Modify(1, 6));
        }

        public static HorizontalRule Modify(this HorizontalRule horizontalRule)
        {
            return new HorizontalRule(horizontalRule.Style.Modify(), horizontalRule.Count.Modify(3, 10), horizontalRule.Space.Modify());
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

        public static Emphasis Modify(this Emphasis text)
        {
            return new Emphasis(text.Text.Modify(), text.Option.Modify());
        }

        public static RawText Modify(this RawText text)
        {
            return new RawText(text.Text.Modify());
        }

        public static OrderedListItem Modify(this OrderedListItem item)
        {
            return new OrderedListItem(item.Number.Modify(0, 10), item.Text.Modify());
        }

        public static BlockQuote Modify(this BlockQuote quoteBlock)
        {
            return new BlockQuote(quoteBlock.Text.Modify());
        }

        public static TableColumn Modify(this TableColumn column)
        {
            return new TableColumn(column.Name.Modify(), column.Alignment.Modify());
        }

        public static TaskListItem Modify(this TaskListItem item)
        {
            return new TaskListItem(item.Text.Modify(), item.IsCompleted.Modify());
        }

        public static MarkdownFormat Modify(this MarkdownFormat x)
        {
            return new MarkdownFormat(
                x.BoldStyle.Modify(),
                x.ItalicStyle.Modify(),
                x.ListItemStyle.Modify(),
                x.HorizontalRule.Modify(),
                x.HeadingStyle.Modify(),
                x.HeadingOptions.Modify(),
                x.TableOptions.Modify(),
                x.CodeFenceStyle.Modify(),
                x.CodeBlockOptions.Modify(),
                x.CharacterReferenceFormat.Modify());
        }

        public static ListItemStyle Modify(this ListItemStyle style)
        {
            switch (style)
            {
                case ListItemStyle.Asterisk:
                    return ListItemStyle.Plus;
                case ListItemStyle.Plus:
                    return ListItemStyle.Minus;
                case ListItemStyle.Minus:
                    return ListItemStyle.Asterisk;
                default:
                    throw new ArgumentException(style.ToString(), nameof(style));
            }
        }

        public static HeadingStyle Modify(this HeadingStyle style)
        {
            switch (style)
            {
                case HeadingStyle.NumberSign:
                    return HeadingStyle.NumberSign;
                default:
                    throw new ArgumentException(style.ToString(), nameof(style));
            }
        }

        public static HeadingOptions Modify(this HeadingOptions options)
        {
            switch (options)
            {
                case HeadingOptions.None:
                    return HeadingOptions.EmptyLineBefore;
                default:
                    return HeadingOptions.None;
            }
        }

        public static TableOptions Modify(this TableOptions options)
        {
            switch (options)
            {
                case TableOptions.None:
                    return TableOptions.FormatHeader;
                default:
                    return TableOptions.None;
            }
        }

        public static CodeFenceStyle Modify(this CodeFenceStyle style)
        {
            switch (style)
            {
                case CodeFenceStyle.Backtick:
                    return CodeFenceStyle.Tilde;
                case CodeFenceStyle.Tilde:
                    return CodeFenceStyle.Backtick;
                default:
                    throw new ArgumentException(style.ToString(), nameof(style));
            }
        }

        public static CodeBlockOptions Modify(this CodeBlockOptions options)
        {
            switch (options)
            {
                case CodeBlockOptions.None:
                    return CodeBlockOptions.EmptyLineBefore;
                default:
                    return CodeBlockOptions.None;
            }
        }

        public static EmphasisStyle Modify(this EmphasisStyle style)
        {
            switch (style)
            {
                case EmphasisStyle.Asterisk:
                    return EmphasisStyle.Underscore;
                case EmphasisStyle.Underscore:
                    return EmphasisStyle.Asterisk;
                default:
                    throw new ArgumentException(style.ToString(), nameof(style));
            }
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
                    throw new ArgumentException(alignment.ToString(), nameof(alignment));
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
                    throw new ArgumentException(style.ToString(), nameof(style));
            }
        }

        public static EmphasisOption Modify(this EmphasisOption options)
        {
            switch (options)
            {
                case EmphasisOption.None:
                    return EmphasisOption.Bold;
                default:
                    return EmphasisOption.None;
            }
        }

        public static CharacterReference Modify(this CharacterReference htmlEntity)
        {
            return new CharacterReference(htmlEntity.Number.Modify(1, 0xFFFF));
        }

        public static CharacterReferenceFormat Modify(this CharacterReferenceFormat format)
        {
            switch (format)
            {
                case CharacterReferenceFormat.Hexadecimal:
                    return CharacterReferenceFormat.Decimal;
                case CharacterReferenceFormat.Decimal:
                    return CharacterReferenceFormat.Hexadecimal;
                default:
                    throw new ArgumentException(format.ToString(), nameof(format));
            }
        }
    }
}
