// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public enum MarkdownKind
    {
        None,
        Document,
        Text,
        Emphasis,
        Code,
        Heading,
        ListItem,
        OrderedListItem,
        OrderedList,
        TaskListItem,
        Link,
        LinkReference,
        Autolink,
        Image,
        ImageReference,
        Label,
        FencedCodeBlock,
        IndentedCodeBlock,
        QuoteBlock,
        HorizontalRule,
        Table,
        TableRow,
        TableColumn,
        Comment,
        HtmlEntity,
        Container
    }
}