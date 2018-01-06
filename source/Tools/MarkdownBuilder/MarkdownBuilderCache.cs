// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown
{
    internal static class MarkdownBuilderCache
    {
        [ThreadStatic]
        private static MarkdownBuilder _cachedInstance;

        public static MarkdownBuilder GetInstance(MarkdownFormat format = null)
        {
            MarkdownBuilder builder = _cachedInstance;

            if (builder != null)
            {
                _cachedInstance = null;
                builder.Clear();

                if (format != null)
                    builder.Format = format;

                return builder;
            }

            return new MarkdownBuilder();
        }

        public static void Free(MarkdownBuilder builder)
        {
            _cachedInstance = builder;
        }

        public static string GetResultAndFree(MarkdownBuilder builder)
        {
            string value = builder.ToString();

            Free(builder);

            return value;
        }
    }
}