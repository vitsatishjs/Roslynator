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
            CharacterReference htmlEntity = CreateHtmlEntity();

            Assert.True(htmlEntity.Equals((object)htmlEntity));
        }

        [Fact]
        public void HtmlEntity_NotEquals()
        {
            CharacterReference htmlEntity = CreateHtmlEntity();
            CharacterReference htmlEntity2 = htmlEntity.Modify();

            Assert.False(htmlEntity.Equals((object)htmlEntity2));
        }

        [Fact]
        public void HtmlEntity_IEquatableEquals()
        {
            CharacterReference htmlEntity = CreateHtmlEntity();
            CharacterReference htmlEntity2 = htmlEntity;
            IEquatable<CharacterReference> equatable = htmlEntity;

            Assert.True(equatable.Equals(htmlEntity2));
        }

        [Fact]
        public void HtmlEntity_IEquatableNotEquals()
        {
            CharacterReference htmlEntity = CreateHtmlEntity();
            CharacterReference htmlEntity2 = CreateHtmlEntity().Modify();
            IEquatable<CharacterReference> equatable = htmlEntity;

            Assert.False(htmlEntity.Equals(htmlEntity2));
        }

        [Fact]
        public void HtmlEntity_GetHashCode_Equal()
        {
            CharacterReference htmlEntity = CreateHtmlEntity();

            Assert.Equal(htmlEntity.GetHashCode(), htmlEntity.GetHashCode());
        }

        [Fact]
        public void HtmlEntity_GetHashCode_NotEqual()
        {
            CharacterReference htmlEntity = CreateHtmlEntity();
            CharacterReference htmlEntity2 = htmlEntity.Modify();

            Assert.NotEqual(htmlEntity.GetHashCode(), htmlEntity2.GetHashCode());
        }

        [Fact]
        public void HtmlEntity_OperatorEquals()
        {
            CharacterReference htmlEntity = CreateHtmlEntity();
            CharacterReference htmlEntity2 = htmlEntity;

            Assert.True(htmlEntity == htmlEntity2);
        }

        [Fact]
        public void HtmlEntity_OperatorNotEquals()
        {
            CharacterReference htmlEntity = CreateHtmlEntity();
            CharacterReference htmlEntity2 = htmlEntity.Modify();

            Assert.True(htmlEntity != htmlEntity2);
        }

        [Fact]
        public void HtmlEntity_Constructor_AssignNumber()
        {
            int number = HtmlEntityNumber();
            var htmlEntity = new CharacterReference(number: number);

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
