// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Pihrtsoft.Markdown.Linq;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

namespace Pihrtsoft.Markdown.Tests
{
    public class CodeBlockTests
    {
        [Fact]
        public void CodeBlock_Equals()
        {
            MFencedCodeBlock block = CreateCodeBlock();

            Assert.True(block.Equals((object)block));
        }

        [Fact]
        public void CodeBlock_NotEquals()
        {
            MFencedCodeBlock block = CreateCodeBlock();
            MFencedCodeBlock block2 = block.Modify();

            Assert.False(block.Equals((object)block2));
        }

        [Fact]
        public void CodeBlock_GetHashCode_Equal()
        {
            MFencedCodeBlock block = CreateCodeBlock();

            Assert.Equal(block.GetHashCode(), block.GetHashCode());
        }

        [Fact]
        public void CodeBlock_GetHashCode_NotEqual()
        {
            MFencedCodeBlock block = CreateCodeBlock();
            MFencedCodeBlock block2 = block.Modify();

            Assert.NotEqual(block.GetHashCode(), block2.GetHashCode());
        }

        [Fact]
        public void CodeBlock_OperatorEquals()
        {
            MFencedCodeBlock block = CreateCodeBlock();
            MFencedCodeBlock block2 = block;

            Assert.True(block == block2);
        }

        [Fact]
        public void CodeBlock_OperatorNotEquals()
        {
            MFencedCodeBlock block = CreateCodeBlock();
            MFencedCodeBlock block2 = block.Modify();

            Assert.True(block != block2);
        }

        [Fact]
        public void CodeBlock_Constructor_AssignText()
        {
            string text = CodeBlockText();
            var block = new MFencedCodeBlock(text: text, info: CodeBlockInfo());

            Assert.Equal(text, block.Text);
        }

        [Fact]
        public void CodeBlock_Constructor_AssignInfo()
        {
            string info = CodeBlockInfo();
            var block = new MFencedCodeBlock(text: CodeBlockText(), info: info);

            Assert.Equal(info, block.Info);
        }
    }
}
