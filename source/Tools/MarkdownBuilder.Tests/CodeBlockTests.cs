// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

namespace Pihrtsoft.Markdown.Tests
{
    public class CodeBlockTests
    {
        [Fact]
        public void CodeBlock_Equals()
        {
            FencedCodeBlock block = CreateCodeBlock();

            Assert.True(block.Equals((object)block));
        }

        [Fact]
        public void CodeBlock_NotEquals()
        {
            FencedCodeBlock block = CreateCodeBlock();
            FencedCodeBlock block2 = block.Modify();

            Assert.False(block.Equals((object)block2));
        }

        [Fact]
        public void CodeBlock_IEquatableEquals()
        {
            FencedCodeBlock block = CreateCodeBlock();
            FencedCodeBlock block2 = block;
            IEquatable<FencedCodeBlock> equatable = block;

            Assert.True(equatable.Equals(block2));
        }

        [Fact]
        public void CodeBlock_IEquatableNotEquals()
        {
            FencedCodeBlock block = CreateCodeBlock();
            FencedCodeBlock block2 = CreateCodeBlock().Modify();
            IEquatable<FencedCodeBlock> equatable = block;

            Assert.False(block.Equals(block2));
        }

        [Fact]
        public void CodeBlock_GetHashCode_Equal()
        {
            FencedCodeBlock block = CreateCodeBlock();

            Assert.Equal(block.GetHashCode(), block.GetHashCode());
        }

        [Fact]
        public void CodeBlock_GetHashCode_NotEqual()
        {
            FencedCodeBlock block = CreateCodeBlock();
            FencedCodeBlock block2 = block.Modify();

            Assert.NotEqual(block.GetHashCode(), block2.GetHashCode());
        }

        [Fact]
        public void CodeBlock_OperatorEquals()
        {
            FencedCodeBlock block = CreateCodeBlock();
            FencedCodeBlock block2 = block;

            Assert.True(block == block2);
        }

        [Fact]
        public void CodeBlock_OperatorNotEquals()
        {
            FencedCodeBlock block = CreateCodeBlock();
            FencedCodeBlock block2 = block.Modify();

            Assert.True(block != block2);
        }

        [Fact]
        public void CodeBlock_Constructor_AssignText()
        {
            string text = CodeBlockText();
            var block = new FencedCodeBlock(text: text, info: CodeBlockInfo());

            Assert.Equal(text, block.Text);
        }

        [Fact]
        public void CodeBlock_WithText()
        {
            string text = CodeBlockText();

            Assert.Equal(text, CreateCodeBlock().WithText(text).Text);
        }

        [Fact]
        public void CodeBlock_Constructor_AssignInfo()
        {
            string info = CodeBlockInfo();
            var block = new FencedCodeBlock(text: CodeBlockText(), info: info);

            Assert.Equal(info, block.Info);
        }

        [Fact]
        public void CodeBlock_WithInfo()
        {
            FencedCodeBlock block = CreateCodeBlock();
            string info = block.Info.Modify();

            Assert.Equal(info, block.WithInfo(info).Info);
        }
    }
}
