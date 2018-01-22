// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Pihrtsoft.Markdown.Linq;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

namespace Pihrtsoft.Markdown.Tests
{
    public class IndentedCodeBlockTests
    {
        [Fact]
        public void IndentedCodeBlock_Equals()
        {
            MIndentedCodeBlock block = CreateIndentedCodeBlock();

            Assert.True(block.Equals((object)block));
        }

        [Fact]
        public void IndentedCodeBlock_NotEquals()
        {
            MIndentedCodeBlock block = CreateIndentedCodeBlock();
            MIndentedCodeBlock block2 = block.Modify();

            Assert.False(block.Equals((object)block2));
        }

        [Fact]
        public void IndentedCodeBlock_GetHashCode_Equal()
        {
            MIndentedCodeBlock block = CreateIndentedCodeBlock();

            Assert.Equal(block.GetHashCode(), block.GetHashCode());
        }

        [Fact]
        public void IndentedCodeBlock_GetHashCode_NotEqual()
        {
            MIndentedCodeBlock block = CreateIndentedCodeBlock();
            MIndentedCodeBlock block2 = block.Modify();

            Assert.NotEqual(block.GetHashCode(), block2.GetHashCode());
        }

        [Fact]
        public void IndentedCodeBlock_OperatorEquals()
        {
            MIndentedCodeBlock block = CreateIndentedCodeBlock();
            MIndentedCodeBlock block2 = block;

            Assert.True(block == block2);
        }

        [Fact]
        public void IndentedCodeBlock_OperatorNotEquals()
        {
            MIndentedCodeBlock block = CreateIndentedCodeBlock();
            MIndentedCodeBlock block2 = block.Modify();

            Assert.True(block != block2);
        }

        [Fact]
        public void IndentedCodeBlock_Constructor_AssignText()
        {
            string text = IndentedCodeBlockText();
            var block = new MIndentedCodeBlock(text: text);

            Assert.Equal(text, block.Text);
        }

        [Fact]
        public void IndentedCodeBlock_WithText()
        {
            string text = IndentedCodeBlockText();

            Assert.Equal(text, CreateIndentedCodeBlock().WithText(text).Text);
        }
    }
}
