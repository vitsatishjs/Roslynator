// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public enum MarkdownKind
    {
        None = 0,

        Text = 1,
        Raw = 2,
        Link = 3,
        LinkReference = 4,
        Image = 5,
        ImageReference = 6,
        Autolink = 7,
        InlineCode = 8,
        CharReference = 9,
        EntityRef = 10,
        Comment = 11,

        FencedCodeBlock = 12,
        IndentedCodeBlock = 13,
        HorizontalRule = 14,
        Label = 15,

        Inline = 16,
        Bold = 17,
        Italic = 18,
        Strikethrough = 19,

        Heading = 20,

        Table = 21,
        TableRow = 22,
        TableColumn = 23,

        Document = 24,
        BlockQuote = 25,
        BulletList = 26,
        BulletItem = 27,
        OrderedList = 28,
        OrderedItem = 29,
        TaskList = 30,
        TaskItem = 31,
    }
}