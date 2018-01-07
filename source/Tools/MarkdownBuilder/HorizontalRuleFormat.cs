// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("Style = {Style} Count = {Count} Space = {Space}")]
    public struct HorizontalRuleFormat : IEquatable<HorizontalRuleFormat>
    {
        public HorizontalRuleFormat(HorizontalRuleStyle style, int count, string space)
        {
            Style = style;
            Count = count;
            Space = space;
        }

        public static HorizontalRuleFormat Default { get; } = new HorizontalRuleFormat(HorizontalRuleStyle.Hyphen, 3, " ");

        public HorizontalRuleStyle Style { get; }

        public int Count { get; }

        public string Space { get; }

        public override bool Equals(object obj)
        {
            return (obj is HorizontalRuleFormat other)
                && Equals(other);
        }

        public bool Equals(HorizontalRuleFormat other)
        {
            return Style == other.Style
                   && Count == other.Count
                   && Space == other.Space;
        }

        public override int GetHashCode()
        {
            return Hash.Combine((int)Style, Hash.Combine(Count, Hash.Create(Space)));
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
