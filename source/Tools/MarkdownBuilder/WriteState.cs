// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

#pragma warning disable CA1714 // Flags enums should have plural names

namespace Pihrtsoft.Markdown
{
    [Flags]
    internal enum WriteState
    {
        None = 0,
        Text = 1,
        Bold = 2,
        Italic = 4,
        Strikethrough = 8,
        Code = 16,
        Heading = 32,
        BulletItem = 64,
        OrderedItem = 128,
        TaskItem = 256,
        Link = 512,
        LinkReference = 1024,
        Autolink = 2048,
        Image = 4096,
        ImageReference = 8192,
        Label = 16384,
        FencedCodeBlock = 32768,
        IndentedCodeBlock = 65536,
        BlockQuote = 131072,
        HorizontalRule = 262144,
        Table = 524288,
        Comment = 1048576,
        StartOfDocument = 2097152,
        StartOfLine = 4194304,
        EmptyLine = 8388608,
        PendingEmptyLine = 16777216,
    }
}
