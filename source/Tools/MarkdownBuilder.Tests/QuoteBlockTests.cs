// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Pihrtsoft.Markdown.Linq;
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
            MBlockQuote quoteBlock = CreateBlockQuote();

            Assert.True(quoteBlock.Equals((object)quoteBlock));
        }

        [Fact]
        public void QuoteBlock_GetHashCode_Equal()
        {
            MBlockQuote quoteBlock = CreateBlockQuote();

            Assert.Equal(quoteBlock.GetHashCode(), quoteBlock.GetHashCode());
        }

        [Fact]
        public void QuoteBlock_OperatorEquals()
        {
            MBlockQuote quoteBlock = CreateBlockQuote();
            MBlockQuote quoteBlock2 = quoteBlock;

            Assert.True(quoteBlock == quoteBlock2);
        }
    }
}
