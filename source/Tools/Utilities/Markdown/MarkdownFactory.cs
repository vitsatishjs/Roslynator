// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.Utilities.Markdown
{
    public static class MarkdownFactory
    {
        public static MarkdownText EmptyText
        {
            get { return Markdown.MarkdownText.Empty; }
        }

        public static MarkdownText MarkdownText(string text)
        {
            return new MarkdownText(text, escape: true);
        }

        public static MarkdownText RawText(string text)
        {
            return new MarkdownText(text, escape: false);
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

        internal static string HeaderStart(int level = 1)
        {
            switch (level)
            {
                case 1:
                    return "# ";
                case 2:
                    return "## ";
                case 3:
                    return "### ";
                case 4:
                    return "#### ";
                case 5:
                    return "##### ";
                case 6:
                    return "###### ";
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, "Header level cannot be less than 1 or greater than 6");
            }
        }

        public static MarkdownHeader Header(string text, int level)
        {
            return new MarkdownHeader(text, level);
        }

        public static MarkdownHeader Header1(string text = null)
        {
            return new MarkdownHeader(text, 1);
        }

        public static MarkdownHeader Header2(string text = null)
        {
            return new MarkdownHeader(text, 2);
        }

        public static MarkdownHeader Header3(string text = null)
        {
            return new MarkdownHeader(text, 3);
        }

        public static MarkdownHeader Header4(string text = null)
        {
            return new MarkdownHeader(text, 4);
        }

        public static MarkdownHeader Header5(string text = null)
        {
            return new MarkdownHeader(text, 5);
        }

        public static MarkdownHeader Header6(string text = null)
        {
            return new MarkdownHeader(text, 6);
        }

        public static ListItem ListItem(string value = null)
        {
            return new ListItem(value);
        }

        public static TaskListItem TaskListItem(string value = null)
        {
            return new TaskListItem(value);
        }

        public static MarkdownLink Link(string text, string url)
        {
            return new MarkdownLink(text, url);
        }

        public static MarkdownImage Image(string text, string url)
        {
            return new MarkdownImage(text, url);
        }

        public static MarkdownText HorizontalRule()
        {
            return RawText(MarkdownSettings.DefaultHorizontalRule);
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
