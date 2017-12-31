// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
namespace Pihrtsoft.Markdown.Tests
{
    internal static class Extensions
    {
        public static string Modify(this string value)
        {
            return value + "x";
        }

        public static int Modify(this int value)
        {
            return value + 1;
        }

        public static bool Modify(this bool value)
        {
            return !value;
        }

        public static Alignment Modify(this Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Left:
                    return Alignment.Center;
                case Alignment.Center:
                    return Alignment.Right;
                case Alignment.Right:
                    return Alignment.Left;
                default:
                    throw new ArgumentException("", nameof(alignment));
            }
        }

        public static HorizontalRuleStyle Modify(this HorizontalRuleStyle style)
        {
            switch (style)
            {
                case HorizontalRuleStyle.Hyphen:
                    return HorizontalRuleStyle.Asterisk;
                case HorizontalRuleStyle.Asterisk:
                    return HorizontalRuleStyle.Underscore;
                case HorizontalRuleStyle.Underscore:
                    return HorizontalRuleStyle.Hyphen;
                default:
                    throw new ArgumentException("", nameof(style));
            }
        }
    }
}
