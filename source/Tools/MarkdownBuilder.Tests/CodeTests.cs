// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Pihrtsoft.Markdown.Tests.TestHelper;

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class CodeTests
    {
        [TestMethod]
        public void CodeTest1()
        {
            MarkdownBuilder mb = CreateBuilder();
            Assert.AreEqual("` `` `", mb.AppendCode("`").ToStringAndClear());
            Assert.AreEqual("` `` `", mb.AppendCode('`').ToStringAndClear());
            Assert.AreEqual("` ```` `", mb.AppendCode('`', '`').ToStringAndClear());
        }
    }
}
