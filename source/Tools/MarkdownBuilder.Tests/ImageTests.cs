// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void ImageTest1()
        {
            Image x = MarkdownFactory.Image("ImageText", "ImageUrl", "ImageTitle");

            string text = x.Text;
            string url = x.Url;
            string title = x.Title;

            string text2 = text.Modify();
            string url2 = url.Modify();
            string title2 = title.Modify();

            Assert.AreNotEqual(text, text2);
            Assert.AreNotEqual(url, url2);
            Assert.AreNotEqual(title, title2);

            TestEquality(x, x.WithText(text2));
            TestEquality(x, x.WithUrl(url2));
            TestEquality(x, x.WithTitle(title2));

            Assert.AreEqual(text2, x.WithText(text2).Text);
            Assert.AreEqual(url2, x.WithUrl(url2).Url);
            Assert.AreEqual(title2, x.WithTitle(title2).Title);

            Assert.AreEqual(x, x.WithText(text));
            Assert.AreEqual(x, x.WithUrl(url));
            Assert.AreEqual(x, x.WithTitle(title));

            Assert.AreNotEqual(x, x.WithText(text2));
            Assert.AreNotEqual(x, x.WithUrl(url2));
            Assert.AreNotEqual(x, x.WithTitle(title2));
        }

        private static void TestEquality(Image x, Image y)
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
