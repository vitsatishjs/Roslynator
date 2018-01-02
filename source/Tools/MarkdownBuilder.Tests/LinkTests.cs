// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class LinkTests
    {
        [Fact]
        public void Link_Equals()
        {
            Link link = CreateLink();

            Assert.True(link.Equals((object)link));
        }

        [Fact]
        public void Link_NotEquals()
        {
            Link link = CreateLink();
            Link link2 = link.Modify();

            Assert.False(link.Equals((object)link2));
        }

        [Fact]
        public void Link_IEquatableEquals()
        {
            Link link = CreateLink();
            Link link2 = link;
            IEquatable<Link> equatable = link;

            Assert.True(equatable.Equals(link2));
        }

        [Fact]
        public void Link_IEquatableNotEquals()
        {
            Link link = CreateLink();
            Link link2 = CreateLink().Modify();
            IEquatable<Link> equatable = link;

            Assert.False(link.Equals(link2));
        }

        [Fact]
        public void Link_GetHashCode_Equal()
        {
            Link link = CreateLink();

            Assert.Equal(link.GetHashCode(), link.GetHashCode());
        }

        [Fact]
        public void Link_GetHashCode_NotEqual()
        {
            Link link = CreateLink();
            Link link2 = link.Modify();

            Assert.NotEqual(link.GetHashCode(), link2.GetHashCode());
        }

        [Fact]
        public void Link_OperatorEquals()
        {
            Link link = CreateLink();
            Link link2 = link;

            Assert.True(link == link2);
        }

        [Fact]
        public void Link_OperatorNotEquals()
        {
            Link link = CreateLink();
            Link link2 = link.Modify();

            Assert.True(link != link2);
        }

        [Fact]
        public void Link_Constructor_AssignText()
        {
            string text = LinkText();
            var link = new Link(text: text, url: LinkUrl(), title: LinkTitle());

            Assert.Equal(text, link.Text);
        }

        [Fact]
        public void Link_WithText()
        {
            string text = LinkText();

            Assert.Equal(text, CreateLink().WithText(text).Text);
        }

        [Fact]
        public void Link_Constructor_AssignUrl()
        {
            string url = LinkUrl();
            var link = new Link(text: LinkText(), url: url, title: LinkTitle());

            Assert.Equal(url, link.Url);
        }

        [Fact]
        public void Link_WithUrl()
        {
            Link link = CreateLink();
            string url = link.Url.Modify();

            Assert.Equal(url, link.WithUrl(url).Url);
        }

        [Fact]
        public void Link_Constructor_AssignTitle()
        {
            string title = LinkTitle();
            var link = new Link(text: LinkText(), url: LinkUrl(), title: title);

            Assert.Equal(title, link.Title);
        }

        [Fact]
        public void Link_WithTitle()
        {
            Link link = CreateLink();
            string title = link.Title.Modify();

            Assert.Equal(title, link.WithTitle(title).Title);
        }
    }
}
