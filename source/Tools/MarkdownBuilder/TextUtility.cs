// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
    }
}
