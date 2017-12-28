// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public struct TableColumn
    {
        internal TableColumn(string name, Alignment alignment = Alignment.Left)
        {
            Name = name;
            Alignment = alignment;
        }

        public string Name { get; }

        public Alignment Alignment { get; }

        public static implicit operator TableColumn(string value)
        {
            return new TableColumn(value);
        }
    }
}
