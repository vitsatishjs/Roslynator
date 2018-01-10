// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown
{
    internal static class MarkdownStringBuilderCache
    {
        [ThreadStatic]
        private static MarkdownStringWriter _cachedInstance;

        public static MarkdownStringWriter GetInstance(MarkdownWriterSettings settings = null)
        {
            MarkdownStringWriter writer = _cachedInstance;

            if (writer != null)
            {
                _cachedInstance = null;
                writer.Clear();

                if (settings != null
                    && settings != writer.Settings)
                {
                    writer.Settings = settings;
                }

                return writer;
            }

            return new MarkdownStringWriter();
        }

        public static void Free(MarkdownStringWriter writer)
        {
            _cachedInstance = writer;
        }

        public static string GetResultAndFree(MarkdownStringWriter writer)
        {
            string value = writer.ToString();

            Free(writer);

            return value;
        }
    }
}