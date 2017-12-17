// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.Utilities.Markdown
{
    public interface IMarkdownWriter
    {
        void WriteBlockQuote(string value = null);
        void WriteBold(string value);
        void WriteCodeBlock(string code, string language = null);
        void WriteCompletedTaskListItem(string value = null);
        void WriteHeader(MarkdownHeader header);
        void WriteHeader(string value, int level);
        void WriteHeader1(string value = null);
        void WriteHeader2(string value = null);
        void WriteHeader3(string value = null);
        void WriteHeader4(string value = null);
        void WriteHeader5(string value = null);
        void WriteHeader6(string value = null);
        void WriteHorizonalRule();
        void WriteImage(string text, string url);
        void WriteInlineCode(string code);
        void WriteItalic(string value);
        void WriteLink(string text, string url);
        void WriteListItem(string value = null);
        void WriteMarkdown(BoldText text);
        void WriteMarkdown(CodeBlock codeBlock);
        void WriteMarkdown(InlineCode  inlineCode);
        void WriteMarkdown(ItalicText text);
        void WriteMarkdown(ListItem item);
        void WriteMarkdown(MarkdownHeader header);
        void WriteMarkdown(MarkdownImage image);
        void WriteMarkdown(MarkdownLink link);
        void WriteMarkdown(MarkdownText text);
        void WriteMarkdown(OrderedListItem item);
        void WriteMarkdown(StrikethroughText text);
        void WriteMarkdown(TaskListItem item);
        void WriteOrderedListItem(int number, string value = null);
        void WriteStrikethrough(string value);
        void WriteTableHeader(params MarkdownTableHeader[] headers);
        void WriteTableRow(params object[] values);
        void WriteTaskListItem(string value = null, bool isCompleted = false);
    }
}