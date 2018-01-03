// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class RawTextTests
    {
        [Fact]
        public void RawText_Equals()
        {
            RawText rawText = CreateRawText();

            Assert.True(rawText.Equals((object)rawText));
        }

        [Fact]
        public void RawText_NotEquals()
        {
            RawText rawText = CreateRawText();
            RawText rawText2 = rawText.Modify();

            Assert.False(rawText.Equals((object)rawText2));
        }

        [Fact]
        public void RawText_IEquatableEquals()
        {
            RawText rawText = CreateRawText();
            RawText rawText2 = rawText;
            IEquatable<RawText> equatable = rawText;

            Assert.True(equatable.Equals(rawText2));
        }

        [Fact]
        public void RawText_IEquatableNotEquals()
        {
            RawText rawText = CreateRawText();
            RawText rawText2 = CreateRawText().Modify();
            IEquatable<RawText> equatable = rawText;

            Assert.False(rawText.Equals(rawText2));
        }

        [Fact]
        public void RawText_GetHashCode_Equal()
        {
            RawText rawText = CreateRawText();

            Assert.Equal(rawText.GetHashCode(), rawText.GetHashCode());
        }

        [Fact]
        public void RawText_GetHashCode_NotEqual()
        {
            RawText rawText = CreateRawText();
            RawText rawText2 = rawText.Modify();

            Assert.NotEqual(rawText.GetHashCode(), rawText2.GetHashCode());
        }

        [Fact]
        public void RawText_OperatorEquals()
        {
            RawText rawText = CreateRawText();
            RawText rawText2 = rawText;

            Assert.True(rawText == rawText2);
        }

        [Fact]
        public void RawText_OperatorNotEquals()
        {
            RawText rawText = CreateRawText();
            RawText rawText2 = rawText.Modify();

            Assert.True(rawText != rawText2);
        }

        [Fact]
        public void RawText_Constructor_AssignText()
        {
            string text = RawTextText();
            var rawText = new RawText(text: text);

            Assert.Equal(text, rawText.Text);
        }

        [Fact]
        public void RawText_WithText()
        {
            string text = RawTextText();

            Assert.Equal(text, CreateRawText().WithText(text).Text);
        }
    }
}
