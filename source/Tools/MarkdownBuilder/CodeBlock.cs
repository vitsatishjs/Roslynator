// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq} Language = {LanguageDebuggerDisplay}")]
    public struct CodeBlock : IMarkdown
    {
        internal CodeBlock(string text, string language = null)
        {
            Text = text;
            Language = language;
        }

        public string Text { get; }

        public string Language { get; }

        private string LanguageDebuggerDisplay => Language ?? "None";

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendCodeBlock(Text, Language);
        }
    }
}
