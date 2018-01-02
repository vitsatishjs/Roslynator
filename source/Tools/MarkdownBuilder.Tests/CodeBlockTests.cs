// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using static Pihrtsoft.Markdown.MarkdownFactory;
using static Pihrtsoft.Markdown.Tests.MarkdownSamples;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

namespace Pihrtsoft.Markdown.Tests
{
    public class CodeBlockTests
    {
        [Fact]
        public void CodeBlock_Equals()
        {
            CodeBlock codeBlock = CreateCodeBlock();

            Assert.True(codeBlock.Equals((object)codeBlock));
        }

        [Fact]
        public void CodeBlock_NotEquals()
        {
            CodeBlock codeBlock = CreateCodeBlock();
            CodeBlock codeBlock2 = codeBlock.Modify();

            Assert.False(codeBlock.Equals((object)codeBlock2));
        }

        [Fact]
        public void CodeBlock_IEquatableEquals()
        {
            CodeBlock codeBlock = CreateCodeBlock();
            CodeBlock codeBlock2 = codeBlock;
            IEquatable<CodeBlock> equatable = codeBlock;

            Assert.True(equatable.Equals(codeBlock2));
        }

        [Fact]
        public void CodeBlock_IEquatableNotEquals()
        {
            CodeBlock codeBlock = CreateCodeBlock();
            CodeBlock codeBlock2 = CreateCodeBlock().Modify();
            IEquatable<CodeBlock> equatable = codeBlock;

            Assert.False(codeBlock.Equals(codeBlock2));
        }

        [Fact]
        public void CodeBlock_GetHashCode_Equal()
        {
            CodeBlock codeBlock = CreateCodeBlock();

            Assert.Equal(codeBlock.GetHashCode(), codeBlock.GetHashCode());
        }

        [Fact]
        public void CodeBlock_GetHashCode_NotEqual()
        {
            CodeBlock codeBlock = CreateCodeBlock();
            CodeBlock codeBlock2 = codeBlock.Modify();

            Assert.NotEqual(codeBlock.GetHashCode(), codeBlock2.GetHashCode());
        }

        [Fact]
        public void CodeBlock_OperatorEquals()
        {
            CodeBlock codeBlock = CreateCodeBlock();
            CodeBlock codeBlock2 = codeBlock;

            Assert.True(codeBlock == codeBlock2);
        }

        [Fact]
        public void CodeBlock_OperatorNotEquals()
        {
            CodeBlock codeBlock = CreateCodeBlock();
            CodeBlock codeBlock2 = codeBlock.Modify();

            Assert.True(codeBlock != codeBlock2);
        }

        [Fact]
        public void CodeBlock_Constructor_AssignText()
        {
            string text = CodeBlockText();
            var codeBlock = new CodeBlock(text: text, language: CodeBlockLanguage());

            Assert.Equal(text, codeBlock.Text);
        }

        [Fact]
        public void CodeBlock_WithText()
        {
            string text = CodeBlockText();

            Assert.Equal(text, CreateCodeBlock().WithText(text).Text);
        }

        [Fact]
        public void CodeBlock_Constructor_AssignLanguage()
        {
            string language = CodeBlockLanguage();
            var codeBlock = new CodeBlock(text: CodeBlockText(), language: language);

            Assert.Equal(language, codeBlock.Language);
        }

        [Fact]
        public void CodeBlock_WithLanguage()
        {
            CodeBlock codeBlock = CreateCodeBlock();
            string language = codeBlock.Language.Modify();

            Assert.Equal(language, codeBlock.WithLanguage(language).Language);
        }

        [Fact]
        public void CodeBlock_Append_CodeBlockOptionsNone()
        {
            MarkdownBuilder mb = CreateBuilderWithCodeBlockOptions(CodeBlockOptions.None);

            CodeBlock cb = CodeBlock(Chars, DefaultText);

            Assert.Equal(
                DefaultText + NewLine + CodeBlockMarkdown + CodeBlockMarkdown + DefaultText,
                mb.Append(DefaultText).Append(cb).Append(cb).Append(DefaultText).ToStringAndClear());
        }

        [Fact]
        public void CodeBlock_Append_CodeBlockOptionsEmptyLineBefore()
        {
            MarkdownBuilder mb = CreateBuilderWithCodeBlockOptions(CodeBlockOptions.EmptyLineBefore);

            CodeBlock cb = CodeBlock(Chars, DefaultText);

            Assert.Equal(
                DefaultText + NewLine2 + CodeBlockMarkdown + NewLine + CodeBlockMarkdown + DefaultText,
                mb.Append(DefaultText).Append(cb).Append(cb).Append(DefaultText).ToStringAndClear());
        }

        [Fact]
        public void CodeBlock_Append_CodeBlockOptionsEmptyLineAfter()
        {
            MarkdownBuilder mb = CreateBuilderWithCodeBlockOptions(CodeBlockOptions.EmptyLineAfter);

            CodeBlock cb = CodeBlock(Chars, DefaultText);

            Assert.Equal(
                DefaultText + NewLine + CodeBlockMarkdown + NewLine + CodeBlockMarkdown + NewLine + DefaultText,
                mb.Append(DefaultText).Append(cb).Append(cb).Append(DefaultText).ToStringAndClear());
        }

        [Fact]
        public void CodeBlock_Append_CodeBlockOptionsEmptyLineBeforeAndAfter()
        {
            MarkdownBuilder mb = CreateBuilderWithCodeBlockOptions(CodeBlockOptions.EmptyLineBeforeAndAfter);

            CodeBlock cb = CodeBlock(Chars, DefaultText);

            Assert.Equal(
                DefaultText + NewLine2 + CodeBlockMarkdown + NewLine + CodeBlockMarkdown + NewLine + DefaultText,
                mb.Append(DefaultText).Append(cb).Append(cb).Append(DefaultText).ToStringAndClear());
        }
    }
}
