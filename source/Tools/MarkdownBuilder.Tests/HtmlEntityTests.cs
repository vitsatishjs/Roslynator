// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Markdown.Linq;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class HtmlEntityTests
    {
        [Fact]
        public void HtmlEntity_Equals()
        {
            MCharEntity htmlEntity = CreateHtmlEntity();

            Assert.True(htmlEntity.Equals((object)htmlEntity));
        }

        [Fact]
        public void HtmlEntity_NotEquals()
        {
            MCharEntity htmlEntity = CreateHtmlEntity();
            MCharEntity htmlEntity2 = htmlEntity.Modify();

            Assert.False(htmlEntity.Equals((object)htmlEntity2));
        }

        [Fact]
        public void HtmlEntity_GetHashCode_Equal()
        {
            MCharEntity htmlEntity = CreateHtmlEntity();

            Assert.Equal(htmlEntity.GetHashCode(), htmlEntity.GetHashCode());
        }

        [Fact]
        public void HtmlEntity_GetHashCode_NotEqual()
        {
            MCharEntity htmlEntity = CreateHtmlEntity();
            MCharEntity htmlEntity2 = htmlEntity.Modify();

            Assert.NotEqual(htmlEntity.GetHashCode(), htmlEntity2.GetHashCode());
        }

        [Fact]
        public void HtmlEntity_OperatorEquals()
        {
            MCharEntity htmlEntity = CreateHtmlEntity();
            MCharEntity htmlEntity2 = htmlEntity;

            Assert.True(htmlEntity == htmlEntity2);
        }

        [Fact]
        public void HtmlEntity_OperatorNotEquals()
        {
            MCharEntity htmlEntity = CreateHtmlEntity();
            MCharEntity htmlEntity2 = htmlEntity.Modify();

            Assert.True(htmlEntity != htmlEntity2);
        }

        [Fact]
        public void HtmlEntity_Constructor_AssignNumber()
        {
            char ch = CharEntityChar();
            var htmlEntity = new MCharEntity(value: ch);

            Assert.Equal(ch, htmlEntity.Value);
        }
    }
}
