// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class QuoteBlockTests
    {
        [Fact]
        public void QuoteBlock_Equals()
        {
            QuoteBlock quoteBlock = CreateQuoteBlock();

            Assert.True(quoteBlock.Equals((object)quoteBlock));
        }

        [Fact]
        public void QuoteBlock_NotEquals()
        {
            QuoteBlock quoteBlock = CreateQuoteBlock();
            QuoteBlock quoteBlock2 = quoteBlock.Modify();

            Assert.False(quoteBlock.Equals((object)quoteBlock2));
        }

        [Fact]
        public void QuoteBlock_IEquatableEquals()
        {
            QuoteBlock quoteBlock = CreateQuoteBlock();
            QuoteBlock quoteBlock2 = quoteBlock;
            IEquatable<QuoteBlock> equatable = quoteBlock;

            Assert.True(equatable.Equals(quoteBlock2));
        }

        [Fact]
        public void QuoteBlock_IEquatableNotEquals()
        {
            QuoteBlock quoteBlock = CreateQuoteBlock();
            QuoteBlock quoteBlock2 = CreateQuoteBlock().Modify();
            IEquatable<QuoteBlock> equatable = quoteBlock;

            Assert.False(quoteBlock.Equals(quoteBlock2));
        }

        [Fact]
        public void QuoteBlock_GetHashCode_Equal()
        {
            QuoteBlock quoteBlock = CreateQuoteBlock();

            Assert.Equal(quoteBlock.GetHashCode(), quoteBlock.GetHashCode());
        }

        [Fact]
        public void QuoteBlock_GetHashCode_NotEqual()
        {
            QuoteBlock quoteBlock = CreateQuoteBlock();
            QuoteBlock quoteBlock2 = quoteBlock.Modify();

            Assert.NotEqual(quoteBlock.GetHashCode(), quoteBlock2.GetHashCode());
        }

        [Fact]
        public void QuoteBlock_OperatorEquals()
        {
            QuoteBlock quoteBlock = CreateQuoteBlock();
            QuoteBlock quoteBlock2 = quoteBlock;

            Assert.True(quoteBlock == quoteBlock2);
        }

        [Fact]
        public void QuoteBlock_OperatorNotEquals()
        {
            QuoteBlock quoteBlock = CreateQuoteBlock();
            QuoteBlock quoteBlock2 = quoteBlock.Modify();

            Assert.True(quoteBlock != quoteBlock2);
        }

        [Fact]
        public void QuoteBlock_Constructor_AssignText()
        {
            string text = QuoteBlockText();
            var quoteBlock = new QuoteBlock(text: text);

            Assert.Equal(text, quoteBlock.Text);
        }

        [Fact]
        public void QuoteBlock_WithText()
        {
            string text = QuoteBlockText();

            Assert.Equal(text, CreateQuoteBlock().WithText(text).Text);
        }
    }
}
