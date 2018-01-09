// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown
{
    internal static class Error
    {
        public static void ThrowOnInvalidHorizontalRuleCount(int count)
        {
            if (count < 3)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Number of characters in horizontal rule cannot be less than 3.");
        }

        public static void ThrowOnInvalidHeadingLevel(int level)
        {
            if (level < 1
                || level > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(level), level, "Heading level must be in range from 1 to 6.");
            }
        }

        public static void ThrowOnInvalidEntityName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length == 0)
                throw new ArgumentException("Entity name cannot be empty.", nameof(name));

            if (!TextUtility.IsAlphanumeric(name))
                throw new ArgumentException("Entity name can contains only alphanumeric characters.", nameof(name));
        }

        public static void ThrowOnInvalidOrderedListNumber(int number)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number), number, "Ordered list item number cannot be negative.");
        }

        public static void ThrowIfContainsWhitespace(string url)
        {
            if (url == null)
                return;

            for (int i = 0; i < url.Length; i++)
            {
                if (char.IsWhiteSpace(url[i]))
                    throw new ArgumentException("Link or image url cannot contain whitespace character(s).", nameof(url));
            }
        }
    }
}
