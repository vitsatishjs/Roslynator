// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq} Url = {Url,nq} Title = {Title,nq}")]
    public struct LinkInfo : IEquatable<LinkInfo>
    {
        public LinkInfo(string text, string url, string title = null)
        {
            Text = text;
            Url = url;
            Title = title;
        }

        public string Text { get; }

        public string Url { get; }

        public string Title { get; }

        public override bool Equals(object obj)
        {
            return (obj is LinkInfo other)
                && Equals(other);
        }

        public bool Equals(LinkInfo other)
        {
            return Text == other.Text
                   && Url == other.Url
                   && Title == other.Title;
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Text, Hash.Combine(Url, Hash.Create(Title)));
        }

        public static bool operator ==(LinkInfo info1, LinkInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(LinkInfo info1, LinkInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
