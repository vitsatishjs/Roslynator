// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

namespace Pihrtsoft.Markdown.Tests
{
    public class CodeMarkdownBuilderTests
    {
        private const string x = CharsEnclosedWithBacktick;

        private const string e = CharsEnclosedWithBacktickDoubled;

        [Fact]
        public void CodeMarkdownBuilder_Append_WithDefaultParameters()
        {
            MarkdownBuilder mb = CreateCodeBuilder();

            Assert.Equal(e, mb.Append(x).ToStringAndClear());
        }

        [Fact]
        public void CodeMarkdownBuilder_Append_WithParameters()
        {
            MarkdownBuilder mb = CreateCodeBuilder();

            Assert.Equal(e, mb.Append(x, MarkdownEscaper.ShouldBeEscaped, '\\').ToStringAndClear());
        }
    }
}
