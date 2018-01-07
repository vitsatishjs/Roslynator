// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public enum MarkdownKind
    {
        None,
        Document,
        Text,
        Emphasis,
        InlineCode,
        Heading,
        ListItem,
        List,
        OrderedListItem,
        OrderedList,
        TaskListItem,
        TaskList,
        Link,
        LinkReference,
        Autolink,
        Image,
        ImageReference,
        Label,
        FencedCodeBlock,
        IndentedCodeBlock,
        BlockQuote,
        HorizontalRule,
        Table,
        TableRow,
        TableColumn,
        Comment,
        CharacterReference,
        Container,
        EntityReference,
    }
}