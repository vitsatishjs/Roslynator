// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;

namespace Pihrtsoft.Markdown.Tests
{
    internal static class TestHelpers
    {
        private static readonly Random _random = new Random();

        public const string Chars = @"! "" # $ % & ' ) ( * + , - . / : ; < = > ? @ ] [ \ ^ _ ` } { | ~";

        public const string CharsWithoutSpaces = @"!""#$%&')(*+,-./:;<=>?@][\^_`}{|~";

        public const string CharsEscaped = @"\! "" \# $ % & ' \) \( \* \+ , \- \. / : ; \< = > ? @ \] \[ \\ ^ \_ \` \} \{ \| \~";

        public const string CharsSquareBracketsBacktickEscaped = @"! "" # $ % & ' ) ( * + , - . / : ; < = > ? @ \] \[ \ ^ _ \` } { | ~";

        public const string CharsWithoutSpacesParenthesesEscaped = @"!""#$%&'\)\(*+,-./:;<=>?@][\^_`}{|~";

        public const string CharsDoubleQuoteEscaped = @"! \"" # $ % & ' ) ( * + , - . / : ; < = > ? @ ] [ \ ^ _ ` } { | ~";

        public const string CharsEnclosedWithBacktick = @"` ! "" # $ % & ' ) ( * + , - . / : ; < = > ? @ ] [ \ ^ _ } { | ~ `";

        public const string CharsEnclosedWithBacktickDoubled = @"`` ! "" # $ % & ' ) ( * + , - . / : ; < = > ? @ ] [ \ ^ _ } { | ~ ``";

        public const string NewLine = "\r\n";

        public const string DefaultText = "Text";

        public static string NewLine2 { get; } = NewLine + NewLine;

        public static string Backtick { get; } = "`";

        public static FencedCodeBlock CreateCodeBlock()
        {
            return new FencedCodeBlock(CodeBlockText(), CodeBlockInfo());
        }

        public static string CodeBlockText()
        {
            return StringValue();
        }

        public static IndentedCodeBlock CreateIndentedCodeBlock()
        {
            return new IndentedCodeBlock(IndentedCodeBlockText());
        }

        public static string IndentedCodeBlockText()
        {
            return StringValue();
        }

        public static string CodeBlockInfo()
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
            return new HorizontalRule(HorizontalRuleStyle(), HorizontalRuleCount(), HorizontalRuleSpace());
        }

        public static HorizontalRuleStyle HorizontalRuleStyle()
        {
            return (HorizontalRuleStyle)IntValue(0, 2);
        }

        public static int HorizontalRuleCount()
        {
            return IntValue(3, 10);
        }

        public static string HorizontalRuleSpace()
        {
            return Spaces(0, 2);
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

        public static Emphasis CreateMarkdownText()
        {
            return new Emphasis(MarkdownTextText(), MarkdownTextOptions());
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

        public static EmphasisOption MarkdownTextOptions()
        {
            return (EmphasisOption)IntValue(0, 8);
        }

        public static OrderedListItem CreateOrderedListItem()
        {
            return new OrderedListItem(OrderedListItemNumber(), ListItemText());
        }

        public static int OrderedListItemNumber()
        {
            return IntValue(0, 9);
        }

        public static BlockQuote CreateQuoteBlock()
        {
            return new BlockQuote(QuoteBlockText());
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

        public static MarkdownFormat CreateMarkdownFormat()
        {
            return new MarkdownFormat(
                BoldStyle(),
                ItalicStyle(),
                ListItemStyle(),
                CreateHorizontalRule(),
                HeadingOptions(),
                TableOptions(),
                CodeFenceStyle(),
                CodeBlockOptions(),
                HtmlEntityFormat(),
                horizontalRuleFormat: HeadingStyle());
        }

        public static EmphasisStyle BoldStyle()
        {
            return (EmphasisStyle)IntValue(0, 1);
        }

        public static EmphasisStyle ItalicStyle()
        {
            return (EmphasisStyle)IntValue(0, 1);
        }

        public static ListStyle ListItemStyle()
        {
            return (ListStyle)IntValue(0, 2);
        }

        public static HeadingStyle HeadingStyle()
        {
            return (HeadingStyle)IntValue(0, 0);
        }

        public static HeadingOptions HeadingOptions()
        {
            return (HeadingOptions)IntValue(0, 16);
        }

        public static TableOptions TableOptions()
        {
            return (TableOptions)IntValue(0, 8);
        }

        public static CodeFenceStyle CodeFenceStyle()
        {
            return (CodeFenceStyle)IntValue(0, 1);
        }

        public static CodeBlockOptions CodeBlockOptions()
        {
            return (CodeBlockOptions)IntValue(0, 3);
        }

        public static CharReference CreateHtmlEntity()
        {
            return new CharReference(IntValue(1, 0xFFFF));
        }

        public static int HtmlEntityNumber()
        {
            return IntValue(1, 0xFFFF);
        }

        public static CharReferenceFormat HtmlEntityFormat()
        {
            return (CharReferenceFormat)IntValue(0, 1);
        }

        public static MarkdownBuilder CreateBuilder(MarkdownFormat format = null)
        {
            return CreateBuilder(new StringBuilder(), format);
        }

        public static MarkdownBuilder CreateBuilderWithHtmlEntityFormat(CharReferenceFormat? format)
        {
            return CreateBuilder((format != null) ? new MarkdownFormat() : null);
        }

        public static MarkdownBuilder CreateBuilderWithCodeBlockOptions(CodeBlockOptions options)
        {
            return CreateBuilder(new MarkdownFormat(codeBlockOptions: options));
        }

        public static MarkdownBuilder CreateBuilderWithCodeFenceOptions(CodeFenceStyle? style)
        {
            return CreateBuilder((style != null) ? new MarkdownFormat(codeFenceStyle: style.Value) : null);
        }

        public static MarkdownBuilder CreateBuilderWithBoldStyle(EmphasisStyle? boldStyle)
        {
            return CreateBuilder((boldStyle != null) ? new MarkdownFormat(boldStyle: boldStyle.Value) : null);
        }

        public static MarkdownBuilder CreateBuilderWithItalicStyle(EmphasisStyle? italicStyle)
        {
            return CreateBuilder((italicStyle != null) ? new MarkdownFormat(italicStyle: italicStyle.Value) : null);
        }

        public static MarkdownBuilder CreateBuilderWithHeadingOptions(HeadingOptions? headingOptions)
        {
            return CreateBuilder((headingOptions != null) ? new MarkdownFormat(headingOptions: headingOptions.Value) : null);
        }

        public static MarkdownBuilder CreateBuilderWithListItemStyle(ListStyle? style)
        {
            return CreateBuilder((style != null) ? new MarkdownFormat(listStyle: style.Value) : null);
        }

        public static MarkdownBuilder CreateBuilder(StringBuilder sb, MarkdownFormat format = null)
        {
            return new MarkdownBuilder(sb, format);
        }

        public static CodeMarkdownBuilder CreateCodeBuilder(MarkdownFormat format = null)
        {
            return CreateCodeBuilder(new StringBuilder(), format);
        }

        public static CodeMarkdownBuilder CreateCodeBuilder(StringBuilder sb, MarkdownFormat format = null)
        {
            return new CodeMarkdownBuilder(sb, format);
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

        public static string Spaces(int minValue, int maxValue)
        {
            return new string(' ', IntValue(minValue, maxValue));
        }

        public static bool BoolValue()
        {
            return IntValue(0, 1) == 1;
        }
    }
}
