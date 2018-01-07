// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace Pihrtsoft.Markdown
{
    internal static class TextUtility
    {
        public static bool IsCarriageReturnOrLinefeed(char ch)
        {
            return ch == '\n' || ch == '\r';
        }

        public static bool IsAlphanumeric(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            for (int i = 0; i < s.Length; i++)
            {
                if (!(i >= 48 && i <= 57)
                    && !(i >= 65 && i <= 90)
                    && !(i >= 97 && i <= 122))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsWhiteSpace(StringBuilder sb)
        {
            return IsWhiteSpace(sb, 0, sb.Length);
        }

        public static bool IsWhiteSpace(StringBuilder sb, int index, int length)
        {
            int max = index + length;

            for (int i = index; i < max; i++)
            {
                if (!char.IsWhiteSpace(sb[i]))
                    return false;
            }

            return true;
        }
    }
}
