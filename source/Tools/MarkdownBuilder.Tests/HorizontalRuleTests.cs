// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class HorizontalRuleTests
    {
        [Fact]
        public void HorizontalRule_Equals()
        {
            HorizontalRule horizontalRule = CreateHorizontalRule();

            Assert.True(horizontalRule.Equals((object)horizontalRule));
        }

        [Fact]
        public void HorizontalRule_NotEquals()
        {
            HorizontalRule horizontalRule = CreateHorizontalRule();
            HorizontalRule horizontalRule2 = horizontalRule.Modify();

            Assert.False(horizontalRule.Equals((object)horizontalRule2));
        }

        [Fact]
        public void HorizontalRule_IEquatableEquals()
        {
            HorizontalRule horizontalRule = CreateHorizontalRule();
            HorizontalRule horizontalRule2 = horizontalRule;
            IEquatable<HorizontalRule> equatable = horizontalRule;

            Assert.True(equatable.Equals(horizontalRule2));
        }

        [Fact]
        public void HorizontalRule_IEquatableNotEquals()
        {
            HorizontalRule horizontalRule = CreateHorizontalRule();
            HorizontalRule horizontalRule2 = CreateHorizontalRule().Modify();
            IEquatable<HorizontalRule> equatable = horizontalRule;

            Assert.False(equatable.Equals(horizontalRule2));
        }

        [Fact]
        public void HorizontalRule_GetHashCode_Equal()
        {
            HorizontalRule horizontalRule = CreateHorizontalRule();

            Assert.Equal(horizontalRule.GetHashCode(), horizontalRule.GetHashCode());
        }

        [Fact]
        public void HorizontalRule_GetHashCode_NotEqual()
        {
            HorizontalRule horizontalRule = CreateHorizontalRule();
            HorizontalRule horizontalRule2 = horizontalRule.Modify();

            Assert.NotEqual(horizontalRule.GetHashCode(), horizontalRule2.GetHashCode());
        }

        [Fact]
        public void HorizontalRule_OperatorEquals()
        {
            HorizontalRule horizontalRule = CreateHorizontalRule();
            HorizontalRule horizontalRule2 = horizontalRule;

            Assert.True(horizontalRule == horizontalRule2);
        }

        [Fact]
        public void HorizontalRule_OperatorNotEquals()
        {
            HorizontalRule horizontalRule = CreateHorizontalRule();
            HorizontalRule horizontalRule2 = horizontalRule.Modify();

            Assert.True(horizontalRule != horizontalRule2);
        }

        [Fact]
        public void HorizontalRule_Constructor_AssignStyle()
        {
            HorizontalRuleStyle style = HorizontalRuleStyle();

            var horizontalRule = new HorizontalRule(style, HorizontalRuleCount(), HorizontalRuleSpace());

            Assert.Equal(style, horizontalRule.Style);
        }

        [Fact]
        public void HorizontalRule_WithStyle()
        {
            HorizontalRule horizontalRule = CreateHorizontalRule();

            HorizontalRuleStyle style = horizontalRule.Style.Modify();

            Assert.Equal(style, horizontalRule.WithStyle(style).Style);
        }

        [Fact]
        public void HorizontalRule_Constructor_AssignCount()
        {
            int count = HorizontalRuleCount();

            var horizontalRule = new HorizontalRule(HorizontalRuleStyle(), count, HorizontalRuleSpace());

            Assert.Equal(count, horizontalRule.Count);
        }

        [Fact]
        public void HorizontalRule_WithCount()
        {
            HorizontalRule horizontalRule = CreateHorizontalRule();

            int count = horizontalRule.Count.Modify();

            Assert.Equal(count, horizontalRule.WithCount(count).Count);
        }

        [Fact]
        public void HorizontalRule_Constructor_AssignSpace()
        {
            string space = HorizontalRuleSpace();

            var horizontalRule = new HorizontalRule(HorizontalRuleStyle(), HorizontalRuleCount(), space);

            Assert.Equal(space, horizontalRule.Space);
        }

        [Fact]
        public void HorizontalRule_WithSpace()
        {
            HorizontalRule horizontalRule = CreateHorizontalRule();

            string space = horizontalRule.Space.Modify();

            Assert.Equal(space, horizontalRule.WithSpace(space).Space);
        }
    }
}
