// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Pihrtsoft.Markdown.Linq;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class MarkdownTextTests
    {
        [Fact]
        public void MarkdownText_Equals()
        {
            MText markdownText = CreateMarkdownText();

            Assert.True(markdownText.Equals((object)markdownText));
        }

        [Fact]
        public void MarkdownText_GetHashCode_Equal()
        {
            MText markdownText = CreateMarkdownText();

            Assert.Equal(markdownText.GetHashCode(), markdownText.GetHashCode());
        }

        [Fact]
        public void MarkdownText_OperatorEquals()
        {
            MText markdownText = CreateMarkdownText();
            MText markdownText2 = markdownText;

            Assert.True(markdownText == markdownText2);
        }

        [Fact]
        public void MarkdownText_Constructor_AssignText()
        {
            string text = MarkdownTextText();
            var markdownText = new MText(value: text);

            Assert.Equal(text, markdownText.Value);
        }
    }
}
