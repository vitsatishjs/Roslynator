// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using static Roslynator.Markdown.MarkdownFactory;

namespace Roslynator.Markdown
{
    internal class CodeMarkdownBuilder : MarkdownBuilder
    {
        internal override MarkdownBuilder Append(string value, Func<char, bool> shouldBeEscaped, char escapingChar)
        {
            return base.Append(value, ch => ch == CodeDelimiterChar, CodeDelimiterChar);
        }
    }
}