// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("Value = {Value,nq} Count = {Count} Separator = {Space}")]
    public struct HorizontalRuleFormat : IEquatable<HorizontalRuleFormat>
    {
        public HorizontalRuleFormat(char value, int count, string separator)
        {
            Value = value;
            Count = count;
            Separator = separator;
        }

        public static HorizontalRuleFormat Default { get; } = new HorizontalRuleFormat('-', 3, " ");

        public char Value { get; }

        public int Count { get; }

        public string Separator { get; }

        public override bool Equals(object obj)
        {
            return (obj is HorizontalRuleFormat other)
                && Equals(other);
        }

        public bool Equals(HorizontalRuleFormat other)
        {
            return Value == other.Value
                   && Count == other.Count
                   && Separator == other.Separator;
        }

        public override int GetHashCode()
        {
            return Hash.Combine((int)Value, Hash.Combine(Count, Hash.Create(Separator)));
        }

        public static bool operator ==(HorizontalRuleFormat format1, HorizontalRuleFormat format2)
        {
            return format1.Equals(format2);
        }

        public static bool operator !=(HorizontalRuleFormat format1, HorizontalRuleFormat format2)
        {
            return !(format1 == format2);
        }
    }
}
