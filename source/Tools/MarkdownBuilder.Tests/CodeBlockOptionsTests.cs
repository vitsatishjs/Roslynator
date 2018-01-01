// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Pihrtsoft.Markdown.MarkdownFactory;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class CodeBlockOptionsTests
    {
        [TestMethod]
        public void CodeBlockOptionsTest1()
        {
            MarkdownBuilder mb = CreateBuilder();

            const string x = "x";
            string y = "```csharp" + NewLine + Chars + NewLine + "```" + NewLine;
            CodeBlock cb = CodeBlock(Chars, CSharpLanguage);

            mb.WithSettings(mb.Settings.WithCodeBlockOptions(CodeBlockOptions.None));

            Assert.AreEqual(x + NewLine + y + y + x, mb.Append(x).Append(cb).Append(cb).Append(x).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithCodeBlockOptions(CodeBlockOptions.EmptyLineBefore));
            Assert.AreEqual(x + NewLine2 + y + NewLine + y + x, mb.Append(x).Append(cb).Append(cb).Append(x).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithCodeBlockOptions(CodeBlockOptions.EmptyLineAfter));
            Assert.AreEqual(x + NewLine + y + NewLine + y + NewLine + x, mb.Append(x).Append(cb).Append(cb).Append(x).ToStringAndClear());

            mb.WithSettings(mb.Settings.WithCodeBlockOptions(CodeBlockOptions.EmptyLineBeforeAndAfter));
            Assert.AreEqual(x + NewLine2 + y + NewLine + y + NewLine + x, mb.Append(x).Append(cb).Append(cb).Append(x).ToStringAndClear());
        }
    }
}
