// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown
{
    internal static class CodeMarkdownBuilderCache
    {
        [ThreadStatic]
        private static CodeMarkdownBuilder _cachedInstance;

        public static CodeMarkdownBuilder GetInstance()
        {
            CodeMarkdownBuilder builder = _cachedInstance;

            if (builder != null)
            {
                _cachedInstance = null;
                builder.Clear();
                return builder;
            }

            return new CodeMarkdownBuilder();
        }

        public static void Free(CodeMarkdownBuilder builder)
        {
            _cachedInstance = builder;
        }

        public static string GetResultAndFree(CodeMarkdownBuilder builder)
        {
            string value = builder.ToString();

            Free(builder);

            return value;
        }
    }
}