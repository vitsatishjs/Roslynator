// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Tests
{
    internal static class MarkdownBuilderExtensions
    {
        public static string ToStringAndClear(this MarkdownBuilder mb)
        {
            string s = mb.ToString();
            mb.Clear();
            return s;
        }
    }
}
