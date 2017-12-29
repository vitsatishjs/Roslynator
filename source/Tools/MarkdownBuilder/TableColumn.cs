// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Name,nq} Alignment = {Alignment}")]
    public struct TableColumn : IEquatable<TableColumn>
    {
        internal TableColumn(string name, Alignment alignment = Alignment.Left)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Alignment = alignment;
        }

        public string Name { get; }

        public Alignment Alignment { get; }

        public override bool Equals(object obj)
        {
            return (obj is TableColumn other)
                && Equals(other);
        }

        public bool Equals(TableColumn other)
        {
            return Alignment == other.Alignment
                   && string.Equals(Name, other.Name, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Name, Hash.Combine((int)Alignment, Hash.OffsetBasis));
        }

        public static bool operator ==(TableColumn column1, TableColumn column2)
        {
            return column1.Equals(column2);
        }

        public static bool operator !=(TableColumn column1, TableColumn column2)
        {
            return !(column1 == column2);
        }

        public static implicit operator TableColumn(string value)
        {
            return new TableColumn(value);
        }
    }
}
