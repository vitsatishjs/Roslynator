// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public class Image : Link
    {
        internal Image(string text, string url, string title = null)
            : base(text, url, title)
        {
        }

        public Image(Image other)
            : base(other)
        {
        }

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendImage(Text, Url, Title);
        }

        internal override MElement Clone()
        {
            return new Image(this);
        }
    }
}
