// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Pihrtsoft.Markdown.Tests.TestHelper;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class CodeBlockTests
    {
        [TestMethod]
        public void CodeBlockTest1()
        {
            var x = new CodeBlock(StringValue, LanguageIdentifiers.CSharp);

            string text = x.Text;
            string language = x.Language;

            string text2 = x.Text.Modify();
            string language2 = x.Language.Modify();

            Assert.AreNotEqual(text, text2);
            Assert.AreNotEqual(language, language2);

            TestEquality(x, x.WithText(text2));
            TestEquality(x, x.WithLanguage(language2));

            Assert.AreEqual(text2, x.WithText(text2).Text);
            Assert.AreEqual(language2, x.WithLanguage(language2).Language);

            Assert.AreEqual(x, x.WithText(text));
            Assert.AreEqual(x, x.WithLanguage(language));

            Assert.AreNotEqual(x, x.WithText(text2));
            Assert.AreNotEqual(x, x.WithLanguage(language2));
        }

        private static void TestEquality(CodeBlock x, CodeBlock y)
        {
            Assert.AreEqual(x, x);
            Assert.IsTrue(x == x);
            Assert.IsFalse(x != x);

            Assert.AreNotEqual(x, y);
            Assert.IsFalse(x == y);
            Assert.IsTrue(x != y);
            Assert.IsFalse(x.GetHashCode() == y.GetHashCode());
        }
    }
}
