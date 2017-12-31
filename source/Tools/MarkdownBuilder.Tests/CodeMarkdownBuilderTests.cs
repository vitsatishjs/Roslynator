// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Pihrtsoft.Markdown.Tests.TestHelper;

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class CodeMarkdownBuilderTests
    {
        [TestMethod]
        public void CodeMarkdownBuilderTest1()
        {
            MarkdownBuilder mb = CreateCodeBuilder();

            const string x = SpecialCharsEnclosedWithBacktick;
            const string y = SpecialCharsEnclosedWithBacktickDoubled;

            Assert.AreEqual(y, mb.Append(x).ToStringAndClear());
            Assert.AreEqual(y, mb.Append(x, MarkdownEscaper.ShouldBeEscaped, '\\').ToStringAndClear());
        }
    }
}
