// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Pihrtsoft.Markdown.Linq;
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
            MRaw rawText = CreateRawText();

            Assert.True(rawText.Equals((object)rawText));
        }

        [Fact]
        public void RawText_NotEquals()
        {
            MRaw rawText = CreateRawText();
            MRaw rawText2 = rawText.Modify();

            Assert.False(rawText.Equals((object)rawText2));
        }

        [Fact]
        public void RawText_GetHashCode_Equal()
        {
            MRaw rawText = CreateRawText();

            Assert.Equal(rawText.GetHashCode(), rawText.GetHashCode());
        }

        [Fact]
        public void RawText_GetHashCode_NotEqual()
        {
            MRaw rawText = CreateRawText();
            MRaw rawText2 = rawText.Modify();

            Assert.NotEqual(rawText.GetHashCode(), rawText2.GetHashCode());
        }

        [Fact]
        public void RawText_OperatorEquals()
        {
            MRaw rawText = CreateRawText();
            MRaw rawText2 = rawText;

            Assert.True(rawText == rawText2);
        }

        [Fact]
        public void RawText_OperatorNotEquals()
        {
            MRaw rawText = CreateRawText();
            MRaw rawText2 = rawText.Modify();

            Assert.True(rawText != rawText2);
        }
    }
}
