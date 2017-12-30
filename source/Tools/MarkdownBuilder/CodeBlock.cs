// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq} Language = {LanguageDebuggerDisplay}")]
    public struct CodeBlock : IMarkdown, IEquatable<CodeBlock>
    {
        internal CodeBlock(string text, string language = null)
        {
            Text = text;
            Language = language;
        }

        public string Text { get; }

        public string Language { get; }

        private string LanguageDebuggerDisplay => Language ?? "None";

        public CodeBlock WithText(string text)
        {
            return new CodeBlock(text, Language);
        }

        public CodeBlock WithLanguage(string language)
        {
            return new CodeBlock(Text, language);
        }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendCodeBlock(Text, Language);
        }

        public override bool Equals(object obj)
        {
            return (obj is CodeBlock other)
                && Equals(other);
        }

        public bool Equals(CodeBlock other)
        {
            return string.Equals(Text, other.Text, StringComparison.Ordinal)
                && string.Equals(Language, other.Language, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Text, Hash.Create(Language));
        }

        public static bool operator ==(CodeBlock left, CodeBlock right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CodeBlock left, CodeBlock right)
        {
            return !(left == right);
        }
    }
}
