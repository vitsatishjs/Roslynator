// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace Roslynator.Utilities.Markdown
{
    public struct CodeBlock : IAppendable
    {
        internal CodeBlock(string text, string language = null)
        {
            OriginalText = text;
            Language = language;
        }

        public string OriginalText { get; }

        public string Language { get; }

        public StringBuilder Append(StringBuilder sb, MarkdownSettings settings = null)
        {
            return sb
                .Append("```")
                .AppendLine(Language)
                .Append(OriginalText)
                .AppendLineIf(!OriginalText.EndsWith("\n"))
                .AppendLine("```");
        }

        //TODO: 
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
