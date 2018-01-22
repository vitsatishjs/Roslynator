// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Pihrtsoft.Markdown.Linq;
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
            MHeading heading = CreateHeading();

            Assert.True(heading.Equals((object)heading));
        }

        [Fact]
        public void Heading_GetHashCode_Equal()
        {
            MHeading heading = CreateHeading();

            Assert.Equal(heading.GetHashCode(), heading.GetHashCode());
        }

        [Fact]
        public void Heading_OperatorEquals()
        {
            MHeading heading = CreateHeading();
            MHeading heading2 = heading;

            Assert.True(heading == heading2);
        }

        [Fact]
        public void Heading_Constructor_AssignText()
        {
            string text = HeadingText();
            var heading = new MHeading(level: HeadingLevel(), content: text);

            Assert.Equal(text, heading.content);
        }

        [Fact]
        public void Heading_Constructor_AssignLevel()
        {
            int level = HeadingLevel();

            var heading = new MHeading(level: level, content: HeadingText());

            Assert.Equal(level, heading.Level);
        }
    }
}
