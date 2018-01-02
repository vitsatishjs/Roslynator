// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class HeadingTests
    {
        [Fact]
        public void Heading_Equals()
        {
            Heading heading = CreateHeading();

            Assert.True(heading.Equals((object)heading));
        }

        [Fact]
        public void Heading_NotEquals()
        {
            Heading heading = CreateHeading();
            Heading heading2 = heading.Modify();

            Assert.False(heading.Equals((object)heading2));
        }

        [Fact]
        public void Heading_IEquatableEquals()
        {
            Heading heading = CreateHeading();
            Heading heading2 = heading;
            IEquatable<Heading> equatable = heading;

            Assert.True(heading.Equals(heading2));
        }

        [Fact]
        public void Heading_IEquatableNotEquals()
        {
            Heading heading = CreateHeading();
            Heading heading2 = CreateHeading().Modify();
            IEquatable<Heading> equatable = heading;

            Assert.False(equatable.Equals(heading2));
        }

        [Fact]
        public void Heading_GetHashCode_Equal()
        {
            Heading heading = CreateHeading();

            Assert.Equal(heading.GetHashCode(), heading.GetHashCode());
        }

        [Fact]
        public void Heading_GetHashCode_NotEqual()
        {
            Heading heading = CreateHeading();
            Heading heading2 = heading.Modify();

            Assert.NotEqual(heading.GetHashCode(), heading2.GetHashCode());
        }

        [Fact]
        public void Heading_OperatorEquals()
        {
            Heading heading = CreateHeading();
            Heading heading2 = heading;

            Assert.True(heading == heading2);
        }

        [Fact]
        public void Heading_OperatorNotEquals()
        {
            Heading heading = CreateHeading();
            Heading heading2 = heading.Modify();

            Assert.True(heading != heading2);
        }

        [Fact]
        public void Heading_Constructor_AssignText()
        {
            string text = HeadingText();
            var heading = new Heading(text: text, level: HeadingLevel());

            Assert.Equal(text, heading.Text);
        }

        [Fact]
        public void Heading_WithText()
        {
            string text = HeadingText();

            Assert.Equal(text, CreateHeading().WithText(text).Text);
        }

        [Fact]
        public void Heading_Constructor_AssignLevel()
        {
            int level = HeadingLevel();

            var heading = new Heading(text: HeadingText(), level: level);

            Assert.Equal(level, heading.Level);
        }

        [Fact]
        public void Heading_WithLevel()
        {
            int level = HeadingLevel();

            Assert.Equal(level, CreateHeading().WithLevel(level).Level);
        }
    }
}
