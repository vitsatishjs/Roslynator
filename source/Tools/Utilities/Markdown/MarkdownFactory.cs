// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.Utilities.Markdown
{
    //TODO: UnicodeChar
    public static class MarkdownFactory
    {
        public static MarkdownText EmptyText
        {
            get { return MarkdownText.Empty; }
        }

        public static MarkdownText Text(string text)
        {
            return new MarkdownText(text);
        }

        public static RawText RawText(string text)
        {
            return new RawText(text);
        }

        public static ItalicText Italic(string text)
        {
            return new ItalicText(text);
        }

        public static BoldText Bold(string text)
        {
            return new BoldText(text);
        }

        public static StrikethroughText Strikethrough(string text)
        {
            return new StrikethroughText(text);
        }

        public static MarkdownTableHeader TableHeader(string name, Alignment alignment = Alignment.Left)
        {
            return new MarkdownTableHeader(name, alignment);
        }

        internal static string HeaderStart(HeaderLevel level = HeaderLevel.Header1)
        {
            switch (level)
            {
                case HeaderLevel.Header1:
                    return "# ";
                case HeaderLevel.Header2:
                    return "## ";
                case HeaderLevel.Header3:
                    return "### ";
                case HeaderLevel.Header4:
                    return "#### ";
                case HeaderLevel.Header5:
                    return "##### ";
                case HeaderLevel.Header6:
                    return "###### ";
                default:
                    throw new ArgumentException("", nameof(level));
            }
        }

        public static MarkdownHeader Header(string text, HeaderLevel level)
        {
            return new MarkdownHeader(text, level);
        }

        public static MarkdownHeader Header1(string text = null)
        {
            return new MarkdownHeader(text, HeaderLevel.Header1);
        }

        public static MarkdownHeader Header2(string text = null)
        {
            return new MarkdownHeader(text, HeaderLevel.Header2);
        }

        public static MarkdownHeader Header3(string text = null)
        {
            return new MarkdownHeader(text, HeaderLevel.Header3);
        }

        public static MarkdownHeader Header4(string text = null)
        {
            return new MarkdownHeader(text, HeaderLevel.Header4);
        }

        public static MarkdownHeader Header5(string text = null)
        {
            return new MarkdownHeader(text, HeaderLevel.Header5);
        }

        public static MarkdownHeader Header6(string text = null)
        {
            return new MarkdownHeader(text, HeaderLevel.Header6);
        }

        //TODO: 
        //public static UnorderedListItem UnorderedListItem1(string value = null, string indentation = "\t")
        //{
        //    return UnorderedListItem(sb, value, indentation, 2);
        //}

        //public static UnorderedListItem UnorderedListItem2(string value = null, string indentation = "\t")
        //{
        //    return UnorderedListItem(sb, value, indentation, 2);
        //}

        //public static UnorderedListItem UnorderedListItem3(string value = null, string indentation = "\t")
        //{
        //    return UnorderedListItem(sb, value, indentation, 3);
        //}

        //public static UnorderedListItem UnorderedListItem(string value = null, string indentation = "\t", int level = 1)
        //{
        //}

        public static MarkdownLink Link(string text, string url)
        {
            return new MarkdownLink(text, url);
        }

        public static MarkdownImage Image(string text, string url)
        {
            return new MarkdownImage(text, url);
        }

        public static HorizontalRule HorizonalRule(HorizontalRuleCharacter character = HorizontalRule.DefaultCharacter, int length = HorizontalRule.DefaultLength)
        {
            return new HorizontalRule(character, length);
        }

        public static InlineCode InlineCode(string text)
        {
            return new InlineCode(text);
        }

        public static CodeBlock CSharpCodeBlock(string text)
        {
            return CodeBlock(text, LanguageIdentifiers.CSharp);
        }

        public static CodeBlock CodeBlock(string text, string language = null)
        {
            return new CodeBlock(text, language);
        }
    }
}
