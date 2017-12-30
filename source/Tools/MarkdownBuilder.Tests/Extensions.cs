// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
    }
}
