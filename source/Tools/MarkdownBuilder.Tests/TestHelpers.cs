// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;

namespace Pihrtsoft.Markdown.Tests
{
    internal static class TestHelpers
    {
        private static readonly Random _random = new Random();

        public const string Chars = @"\ ` * _ } { ] [ ) ( # + - . ! > < "" '";

        public const string CharsWithoutSpaces = @"\`*_}{][)(#+-.!><""'";

        public const string CharsEscaped = @"\\ \` \* \_ \} \{ \] \[ \) \( \# \+ \- \. \! > \< "" '";

        public const string CharsSquareBracketsBacktickEscaped = @"\ \` * _ } { \] \[ ) ( # + - . ! > < "" '";

        public const string CharsWithoutSpacesParenthesesEscaped = @"\`*_}{][\)\(#+-.!><""'";

        public const string CharsDoubleQuoteEscaped = @"\ ` * _ } { ] [ ) ( # + - . ! > < \"" '";

        public const string CharsEnclosedWithBacktick = @"` \ * _ } { ] [ ) ( # + - . ! > <  "" ' `";

        public const string CharsEnclosedWithBacktickDoubled = @"`` \ * _ } { ] [ ) ( # + - . ! > <  "" ' ``";

        public const string NewLine = "\r\n";

        public const string DefaultText = "Text";

        public static string NewLine2 { get; } = NewLine + NewLine;

        public static string Backtick { get; } = "`";

        public static CodeBlock CreateCodeBlock()
        {
            return new CodeBlock(CodeBlockText(), CodeBlockLanguage());
        }

        public static string CodeBlockText()
        {
            return StringValue();
        }

        public static string CodeBlockLanguage()
        {
            return StringValue();
        }

        public static Heading CreateHeading()
        {
            return new Heading(HeadingText(), HeadingLevel());
        }

        public static string HeadingText()
        {
            return StringValue();
        }

        public static int HeadingLevel()
        {
            return IntValue(1, 6);
        }

        public static HorizontalRule CreateHorizontalRule()
        {
            return new HorizontalRule(HorizontalRuleStyle(), HorizontalRuleCount(), HorizontalRuleAddSpaces());
        }

        public static HorizontalRuleStyle HorizontalRuleStyle()
        {
            return (HorizontalRuleStyle)IntValue(0, 2);
        }

        public static int HorizontalRuleCount()
        {
            return IntValue(3, 10);
        }

        public static bool HorizontalRuleAddSpaces()
        {
            return BoolValue();
        }

        public static Image CreateImage()
        {
            return new Image(LinkText(), LinkUrl(), LinkTitle());
        }

        public static Link CreateLink()
        {
            return new Link(LinkText(), LinkUrl(), LinkTitle());
        }

        public static string LinkText()
        {
            return StringValue();
        }

        public static string LinkUrl()
        {
            return StringValue();
        }

        public static string LinkTitle()
        {
            return StringValue();
        }

        public static ListItem CreateListItem()
        {
            return new ListItem(ListItemText());
        }

        public static string ListItemText()
        {
            return StringValue();
        }

        public static MarkdownText CreateMarkdownText()
        {
            return new MarkdownText(MarkdownTextText(), MarkdownTextOptions());
        }

        public static string MarkdownTextText()
        {
            return StringValue();
        }

        public static RawText CreateRawText()
        {
            return new RawText(RawTextText());
        }

        public static string RawTextText()
        {
            return StringValue();
        }

        public static EmphasisOptions MarkdownTextOptions()
        {
            return (EmphasisOptions)IntValue(0, 8);
        }

        public static OrderedListItem CreateOrderedListItem()
        {
            return new OrderedListItem(OrderedListItemNumber(), ListItemText());
        }

        public static int OrderedListItemNumber()
        {
            return IntValue(0, 9);
        }

        public static QuoteBlock CreateQuoteBlock()
        {
            return new QuoteBlock(QuoteBlockText());
        }

        public static string QuoteBlockText()
        {
            return StringValue();
        }

        public static TableColumn CreateTableColumn()
        {
            return new TableColumn(TableColumnName(), TableColumnAlignment());
        }

        public static string TableColumnName()
        {
            return StringValue();
        }

        public static Alignment TableColumnAlignment()
        {
            return (Alignment)IntValue(0, 2);
        }

        public static TaskListItem CreateTaskListItem()
        {
            return new TaskListItem(ListItemText(), TaskListItemIsCompleted());
        }

        public static bool TaskListItemIsCompleted()
        {
            return BoolValue();
        }

        public static MarkdownBuilder CreateBuilder(MarkdownSettings settings = null)
        {
            return CreateBuilder(new StringBuilder(), settings);
        }

        public static MarkdownBuilder CreateBuilderWithCodeBlockOptions(CodeBlockOptions options)
        {
            return CreateBuilder(new MarkdownSettings(codeBlockOptions: options));
        }

        public static MarkdownBuilder CreateBuilderWithBoldStyle(EmphasisStyle? boldStyle)
        {
            return CreateBuilder((boldStyle != null) ? new MarkdownSettings(boldStyle: boldStyle.Value) : null);
        }

        public static MarkdownBuilder CreateBuilderWithItalicStyle(EmphasisStyle? italicStyle)
        {
            return CreateBuilder((italicStyle != null) ? new MarkdownSettings(italicStyle: italicStyle.Value) : null);
        }

        public static MarkdownBuilder CreateBuilderWithHeadingOptions(HeadingOptions? headingOptions)
        {
            return CreateBuilder((headingOptions != null) ? new MarkdownSettings(headingOptions: headingOptions.Value) : null);
        }

        public static MarkdownBuilder CreateBuilderWithListItemStyle(ListItemStyle? style)
        {
            return CreateBuilder((style != null) ? new MarkdownSettings(listItemStyle: style.Value) : null);
        }

        public static MarkdownBuilder CreateBuilder(StringBuilder sb, MarkdownSettings settings = null)
        {
            return new MarkdownBuilder(sb, settings);
        }

        public static CodeMarkdownBuilder CreateCodeBuilder(MarkdownSettings settings = null)
        {
            return CreateCodeBuilder(new StringBuilder(), settings);
        }

        public static CodeMarkdownBuilder CreateCodeBuilder(StringBuilder sb, MarkdownSettings settings = null)
        {
            return new CodeMarkdownBuilder(sb, settings);
        }

        public static int IntValue()
        {
            return _random.Next();
        }

        public static int IntValue(int maxValue)
        {
            return _random.Next(maxValue + 1);
        }

        public static int IntValue(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue + 1);
        }

        public static string StringValue(int length = 3)
        {
            var chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = (char)IntValue(97, 122);
            }

            return new string(chars);
        }

        public static bool BoolValue()
        {
            return IntValue(0, 1) == 1;
        }
    }
}
