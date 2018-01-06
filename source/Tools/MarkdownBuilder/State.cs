// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

#pragma warning disable CA1714 // Flags enums should have plural names

namespace Pihrtsoft.Markdown
{
    [Flags]
    internal enum State
    {
        None = 0,
        Text = 1,
        Bold = 2,
        Italic = 4,
        Strikethrough = 8,
        Code = 16,
        Heading = 32,
        ListItem = 64,
        OrderedListItem = 128,
        TaskListItem = 256,
        Link = 512,
        LinkReference = 1024,
        Autolink = 2048,
        Image = 4096,
        ImageReference = 8192,
        Label = 16384,
        FencedCodeBlock = 32768,
        IndentedCodeBlock = 65536,
        QuoteBlock = 131072,
        HorizontalRule = 262144,
        Table = 524288,
        DoubleQuotes = 1048576,
        Parentheses = 2097152,
        AngleBrackets = 4194304,
        SquareBrackets = 8388608,
        Comment = 16777216,
        StartOfLine = 33554432,
        PendingEmptyLine = 67108864,
    }
}
