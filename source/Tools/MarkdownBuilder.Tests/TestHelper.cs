// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace Pihrtsoft.Markdown.Tests
{
    internal static class TestHelper
    {
        public static MarkdownBuilder CreateBuilder(MarkdownSettings settings = null)
        {
            return CreateBuilder(new StringBuilder(), settings);
        }

        public static MarkdownBuilder CreateBuilder(StringBuilder sb, MarkdownSettings settings = null)
        {
            return new MarkdownBuilder(sb, settings);
        }
    }
}
