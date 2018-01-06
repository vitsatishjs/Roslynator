// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
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
            Emphasis markdownText = CreateMarkdownText();

            Assert.True(markdownText.Equals((object)markdownText));
        }

        [Fact]
        public void MarkdownText_NotEquals()
        {
            Emphasis markdownText = CreateMarkdownText();
            Emphasis markdownText2 = markdownText.Modify();

            Assert.False(markdownText.Equals((object)markdownText2));
        }

        [Fact]
        public void MarkdownText_IEquatableEquals()
        {
            Emphasis markdownText = CreateMarkdownText();
            Emphasis markdownText2 = markdownText;
            IEquatable<Emphasis> equatable = markdownText;

            Assert.True(equatable.Equals(markdownText2));
        }

        [Fact]
        public void MarkdownText_IEquatableNotEquals()
        {
            Emphasis markdownText = CreateMarkdownText();
            Emphasis markdownText2 = CreateMarkdownText().Modify();
            IEquatable<Emphasis> equatable = markdownText;

            Assert.False(markdownText.Equals(markdownText2));
        }

        [Fact]
        public void MarkdownText_GetHashCode_Equal()
        {
            Emphasis markdownText = CreateMarkdownText();

            Assert.Equal(markdownText.GetHashCode(), markdownText.GetHashCode());
        }

        [Fact]
        public void MarkdownText_GetHashCode_NotEqual()
        {
            Emphasis markdownText = CreateMarkdownText();
            Emphasis markdownText2 = markdownText.Modify();

            Assert.NotEqual(markdownText.GetHashCode(), markdownText2.GetHashCode());
        }

        [Fact]
        public void MarkdownText_OperatorEquals()
        {
            Emphasis markdownText = CreateMarkdownText();
            Emphasis markdownText2 = markdownText;

            Assert.True(markdownText == markdownText2);
        }

        [Fact]
        public void MarkdownText_OperatorNotEquals()
        {
            Emphasis markdownText = CreateMarkdownText();
            Emphasis markdownText2 = markdownText.Modify();

            Assert.True(markdownText != markdownText2);
        }

        [Fact]
        public void MarkdownText_Constructor_AssignText()
        {
            string text = MarkdownTextText();
            var markdownText = new Emphasis(text: text, options: MarkdownTextOptions());

            Assert.Equal(text, markdownText.Text);
        }

        [Fact]
        public void MarkdownText_WithText()
        {
            string text = MarkdownTextText();

            Assert.Equal(text, CreateMarkdownText().WithText(text).Text);
        }

        [Fact]
        public void MarkdownOptions_Constructor_AssignOptions()
        {
            EmphasisOption options = MarkdownTextOptions();
            var markdownOptions = new Emphasis(text: MarkdownTextText(), options: options);

            Assert.Equal(options, markdownOptions.Option);
        }

        [Fact]
        public void MarkdownText_WithOptions()
        {
            Emphasis markdownText = CreateMarkdownText();
            EmphasisOption options = markdownText.Option.Modify();

            Assert.Equal(options, markdownText.WithOptions(options).Options);
        }
    }
}
