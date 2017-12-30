﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using static Pihrtsoft.Markdown.MarkdownFactory;

namespace Pihrtsoft.Markdown
{
    internal class CodeMarkdownBuilder : MarkdownBuilder
    {
        public CodeMarkdownBuilder(MarkdownSettings settings = null)
            : base(settings)
        {
        }

        public CodeMarkdownBuilder(System.Text.StringBuilder sb, MarkdownSettings settings = null)
            : base(sb, settings)
        {
        }

        internal override MarkdownBuilder Append(string value, Func<char, bool> shouldBeEscaped, char escapingChar)
        {
            return base.Append(value, ch => ch == CodeDelimiterChar, CodeDelimiterChar);
        }
    }
}