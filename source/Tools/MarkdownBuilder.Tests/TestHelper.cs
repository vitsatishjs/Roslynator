// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;

namespace Pihrtsoft.Markdown.Tests
{
    internal static class TestHelper
    {
        public const string StringValue = "x";
        public const string StringValue2 = "x2";

        public const int IntValue = 1;
        public const int IntValue2 = 2;

        public const bool BoolValue = true;
        public const bool BoolValue2 = false;

        public const HorizontalRuleStyle HorizontalRuleStyleValue = HorizontalRuleStyle.Hyphen;
        public const HorizontalRuleStyle HorizontalRuleStyleValue2 = HorizontalRuleStyle.Asterisk;

        public const string SpecialChars = @"\ ` * _ { } [ ] ( ) # + - . ! <";

        public const string SpecialCharsEscaped = @"\\ \` \* \_ \{ \} \[ \] \( \) \# \+ \- \. \! \<";

        public const string SpecialCharsBacktickDoubled = @"\ `` * _ { } [ ] ( ) # + - . ! <";

        public static string NewLine { get; } = Environment.NewLine;

        public static string Backtick { get; } = "`";

        public static MarkdownBuilder CreateBuilder(MarkdownSettings settings = null)
        {
            return CreateBuilder(new StringBuilder(), settings);
        }

        public static MarkdownBuilder CreateBuilder(StringBuilder sb, MarkdownSettings settings = null)
        {
            return new MarkdownBuilder(sb, settings);
        }

        public static CodeMarkdownBuilder CreateCodeBuilder(MarkdownSettings settings = null)
        {
            return CreateCodeBuilder(new StringBuilder(), settings);
        }

        public static CodeMarkdownBuilder CreateCodeBuilder(StringBuilder sb, MarkdownSettings settings = null)
        {
            return new CodeMarkdownBuilder(sb, settings);
        }
    }
}
