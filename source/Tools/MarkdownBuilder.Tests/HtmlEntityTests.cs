// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
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
            CharReference htmlEntity = CreateHtmlEntity();

            Assert.True(htmlEntity.Equals((object)htmlEntity));
        }

        [Fact]
        public void HtmlEntity_NotEquals()
        {
            CharReference htmlEntity = CreateHtmlEntity();
            CharReference htmlEntity2 = htmlEntity.Modify();

            Assert.False(htmlEntity.Equals((object)htmlEntity2));
        }

        [Fact]
        public void HtmlEntity_IEquatableEquals()
        {
            CharReference htmlEntity = CreateHtmlEntity();
            CharReference htmlEntity2 = htmlEntity;
            IEquatable<CharReference> equatable = htmlEntity;

            Assert.True(equatable.Equals(htmlEntity2));
        }

        [Fact]
        public void HtmlEntity_IEquatableNotEquals()
        {
            CharReference htmlEntity = CreateHtmlEntity();
            CharReference htmlEntity2 = CreateHtmlEntity().Modify();
            IEquatable<CharReference> equatable = htmlEntity;

            Assert.False(htmlEntity.Equals(htmlEntity2));
        }

        [Fact]
        public void HtmlEntity_GetHashCode_Equal()
        {
            CharReference htmlEntity = CreateHtmlEntity();

            Assert.Equal(htmlEntity.GetHashCode(), htmlEntity.GetHashCode());
        }

        [Fact]
        public void HtmlEntity_GetHashCode_NotEqual()
        {
            CharReference htmlEntity = CreateHtmlEntity();
            CharReference htmlEntity2 = htmlEntity.Modify();

            Assert.NotEqual(htmlEntity.GetHashCode(), htmlEntity2.GetHashCode());
        }

        [Fact]
        public void HtmlEntity_OperatorEquals()
        {
            CharReference htmlEntity = CreateHtmlEntity();
            CharReference htmlEntity2 = htmlEntity;

            Assert.True(htmlEntity == htmlEntity2);
        }

        [Fact]
        public void HtmlEntity_OperatorNotEquals()
        {
            CharReference htmlEntity = CreateHtmlEntity();
            CharReference htmlEntity2 = htmlEntity.Modify();

            Assert.True(htmlEntity != htmlEntity2);
        }

        [Fact]
        public void HtmlEntity_Constructor_AssignNumber()
        {
            int number = HtmlEntityNumber();
            var htmlEntity = new CharReference(number: number);

            Assert.Equal(number, htmlEntity.Number);
        }

        [Fact]
        public void HtmlEntity_WithNumber()
        {
            int number = HtmlEntityNumber();

            Assert.Equal(number, CreateHtmlEntity().WithNumber(number).Number);
        }
    }
}
