﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Roslynator
{
    internal abstract class FlagsUtility<T> where T : struct
    {
        public static FlagsUtility<T> Instance { get; } = (FlagsUtility<T>)GetInstance();

        private static object GetInstance()
        {
            if (typeof(T) == typeof(sbyte))
                return new SByteFlagsUtility();

            if (typeof(T) == typeof(byte))
                return new ByteFlagsUtility();

            if (typeof(T) == typeof(short))
                return new ShortFlagsUtility();

            if (typeof(T) == typeof(ushort))
                return new UShortFlagsUtility();

            if (typeof(T) == typeof(int))
                return new IntFlagsUtility();

            if (typeof(T) == typeof(uint))
                return new UIntFlagsUtility();

            if (typeof(T) == typeof(long))
                return new LongFlagsUtility();

            if (typeof(T) == typeof(ulong))
                return new ULongFlagsUtility();

            throw new InvalidOperationException();
        }

        public abstract int ByteCount { get; }

        public abstract T MaxValue { get; }

        public abstract Optional<T> GetUniquePowerOfTwo(IEnumerable<T> reservedValues, bool startFromHighestExistingValue = false);

        public abstract bool IsZeroOrPowerOfTwo(T value);

        public abstract bool IsPowerOfTwo(T value);

        public abstract bool IsComposite(T value);

        public abstract IEnumerable<T> Decompose(T value);

        public abstract Optional<T> TryCompose(ImmutableArray<T> values);

        private class SByteFlagsUtility : FlagsUtility<sbyte>
        {
            public override int ByteCount => (sizeof(sbyte) * 8) - 1;

            public override sbyte MaxValue => sbyte.MaxValue;

            public override Optional<sbyte> GetUniquePowerOfTwo(IEnumerable<sbyte> reservedValues, bool startFromHighestExistingValue = false)
            {
                if (reservedValues == null)
                    throw new ArgumentNullException(nameof(reservedValues));

                sbyte[] values = reservedValues.Where(f => f >= 0 && IsZeroOrPowerOfTwo(f)).ToArray();

                if (values.Length == 0)
                    return 0;

                if (values.Length == 1 && values[0] == 0)
                    return 1;

                if (startFromHighestExistingValue)
                {
                    sbyte i = values.Max();

                    i *= 2;

                    if (i > 0)
                        return i;
                }
                else
                {
                    sbyte i = 1;

                    while (i > 0)
                    {
                        if (Array.IndexOf(values, i) == -1)
                            return i;

                        i *= 2;
                    }
                }

                return default;
            }

            public override bool IsZeroOrPowerOfTwo(sbyte value)
            {
                return (value & (value - 1)) == 0;
            }

            public override bool IsPowerOfTwo(sbyte value)
            {
                return value > 0 && (value & (value - 1)) == 0;
            }

            public override bool IsComposite(sbyte value)
            {
                return value > 0
                    && (value & (value - 1)) != 0;
            }

            public override IEnumerable<sbyte> Decompose(sbyte value)
            {
                for (int i = 0; i < ByteCount; i++)
                {
                    var x = (sbyte)(1 << i);

                    if (x > value)
                        yield break;

                    if ((value & x) != 0)
                        yield return x;
                }
            }

            public override Optional<sbyte> TryCompose(ImmutableArray<sbyte> values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        if (j != i
                            && (values[i] & values[j]) != 0)
                        {
                            return default;
                        }
                    }
                }

                return values.Aggregate((f, g) => (sbyte)(f + g));
            }
        }

        private class ByteFlagsUtility : FlagsUtility<byte>
        {
            public override int ByteCount => (sizeof(byte) * 8) - 1;

            public override byte MaxValue => byte.MaxValue;

            public override Optional<byte> GetUniquePowerOfTwo(IEnumerable<byte> reservedValues, bool startFromHighestExistingValue = false)
            {
                if (reservedValues == null)
                    throw new ArgumentNullException(nameof(reservedValues));

                byte[] values = reservedValues.Where(f => IsZeroOrPowerOfTwo(f)).ToArray();

                if (values.Length == 0)
                    return 0;

                if (values.Length == 1 && values[0] == 0)
                    return 1;

                if (startFromHighestExistingValue)
                {
                    byte i = values.Max();

                    i *= 2;

                    if (i > 0)
                        return i;
                }
                else
                {
                    byte i = 1;

                    while (i > 0)
                    {
                        if (Array.IndexOf(values, i) == -1)
                            return i;

                        i *= 2;
                    }
                }

                return default;
            }

            public override bool IsZeroOrPowerOfTwo(byte value)
            {
                return (value & (value - 1)) == 0;
            }

            public override bool IsPowerOfTwo(byte value)
            {
                return value > 0 && (value & (value - 1)) == 0;
            }

            public override bool IsComposite(byte value)
            {
                return value > 0
                    && (value & (value - 1)) != 0;
            }

            public override IEnumerable<byte> Decompose(byte value)
            {
                for (int i = 0; i < ByteCount; i++)
                {
                    var x = (byte)(1 << i);

                    if (x > value)
                        yield break;

                    if ((value & x) != 0)
                        yield return x;
                }
            }

            public override Optional<byte> TryCompose(ImmutableArray<byte> values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        if (j != i
                            && (values[i] & values[j]) != 0)
                        {
                            return default;
                        }
                    }
                }

                return values.Aggregate((f, g) => (byte)(f + g));
            }
        }

        private class ShortFlagsUtility : FlagsUtility<short>
        {
            public override int ByteCount => (sizeof(short) * 8) - 1;

            public override short MaxValue => short.MaxValue;

            public override Optional<short> GetUniquePowerOfTwo(IEnumerable<short> reservedValues, bool startFromHighestExistingValue = false)
            {
                if (reservedValues == null)
                    throw new ArgumentNullException(nameof(reservedValues));

                short[] values = reservedValues.Where(f => f >= 0 && IsZeroOrPowerOfTwo(f)).ToArray();

                if (values.Length == 0)
                    return 0;

                if (values.Length == 1 && values[0] == 0)
                    return 1;

                if (startFromHighestExistingValue)
                {
                    short i = values.Max();

                    i *= 2;

                    if (i > 0)
                        return i;
                }
                else
                {
                    short i = 1;

                    while (i > 0)
                    {
                        if (Array.IndexOf(values, i) == -1)
                            return i;

                        i *= 2;
                    }
                }

                return default;
            }

            public override bool IsZeroOrPowerOfTwo(short value)
            {
                return (value & (value - 1)) == 0;
            }

            public override bool IsPowerOfTwo(short value)
            {
                return value > 0 && (value & (value - 1)) == 0;
            }

            public override bool IsComposite(short value)
            {
                return value > 0
                    && (value & (value - 1)) != 0;
            }

            public override IEnumerable<short> Decompose(short value)
            {
                for (int i = 0; i < ByteCount; i++)
                {
                    var x = (short)(1 << i);

                    if (x > value)
                        yield break;

                    if ((value & x) != 0)
                        yield return x;
                }
            }

            public override Optional<short> TryCompose(ImmutableArray<short> values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        if (j != i
                            && (values[i] & values[j]) != 0)
                        {
                            return default;
                        }
                    }
                }

                return values.Aggregate((f, g) => (short)(f + g));
            }
        }

        private class UShortFlagsUtility : FlagsUtility<ushort>
        {
            public override int ByteCount => (sizeof(ushort) * 8) - 1;

            public override ushort MaxValue => ushort.MaxValue;

            public override Optional<ushort> GetUniquePowerOfTwo(IEnumerable<ushort> reservedValues, bool startFromHighestExistingValue = false)
            {
                if (reservedValues == null)
                    throw new ArgumentNullException(nameof(reservedValues));

                ushort[] values = reservedValues.Where(f => IsZeroOrPowerOfTwo(f)).ToArray();

                if (values.Length == 0)
                    return 0;

                if (values.Length == 1 && values[0] == 0)
                    return 1;

                if (startFromHighestExistingValue)
                {
                    ushort i = values.Max();

                    i *= 2;

                    if (i > 0)
                        return i;
                }
                else
                {
                    ushort i = 1;

                    while (i > 0)
                    {
                        if (Array.IndexOf(values, i) == -1)
                            return i;

                        i *= 2;
                    }
                }

                return default;
            }

            public override bool IsZeroOrPowerOfTwo(ushort value)
            {
                return (value & (value - 1)) == 0;
            }

            public override bool IsPowerOfTwo(ushort value)
            {
                return value > 0 && (value & (value - 1)) == 0;
            }

            public override bool IsComposite(ushort value)
            {
                return value > 0
                    && (value & (value - 1)) != 0;
            }

            public override IEnumerable<ushort> Decompose(ushort value)
            {
                for (int i = 0; i < ByteCount; i++)
                {
                    var x = (ushort)(1 << i);

                    if (x > value)
                        yield break;

                    if ((value & x) != 0)
                        yield return x;
                }
            }

            public override Optional<ushort> TryCompose(ImmutableArray<ushort> values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        if (j != i
                            && (values[i] & values[j]) != 0)
                        {
                            return default;
                        }
                    }
                }

                return values.Aggregate((f, g) => (ushort)(f + g));
            }
        }

        private class IntFlagsUtility : FlagsUtility<int>
        {
            public override int ByteCount => (sizeof(int) * 8) - 1;

            public override int MaxValue => int.MaxValue;

            public override Optional<int> GetUniquePowerOfTwo(IEnumerable<int> reservedValues, bool startFromHighestExistingValue = false)
            {
                if (reservedValues == null)
                    throw new ArgumentNullException(nameof(reservedValues));

                int[] values = reservedValues.Where(f => f >= 0 && IsZeroOrPowerOfTwo(f)).ToArray();

                if (values.Length == 0)
                    return 0;

                if (values.Length == 1 && values[0] == 0)
                    return 1;

                if (startFromHighestExistingValue)
                {
                    int i = values.Max();

                    i *= 2;

                    if (i > 0)
                        return i;
                }
                else
                {
                    int i = 1;

                    while (i > 0)
                    {
                        if (Array.IndexOf(values, i) == -1)
                            return i;

                        i *= 2;
                    }
                }

                return default;
            }

            public override bool IsZeroOrPowerOfTwo(int value)
            {
                return (value & (value - 1)) == 0;
            }

            public override bool IsPowerOfTwo(int value)
            {
                return value > 0 && (value & (value - 1)) == 0;
            }

            public override bool IsComposite(int value)
            {
                return value > 0
                    && (value & (value - 1)) != 0;
            }

            public override IEnumerable<int> Decompose(int value)
            {
                for (int i = 0; i < ByteCount; i++)
                {
                    int x = 1 << i;

                    if (x > value)
                        yield break;

                    if ((value & x) != 0)
                        yield return x;
                }
            }

            public override Optional<int> TryCompose(ImmutableArray<int> values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        if (j != i
                            && (values[i] & values[j]) != 0)
                        {
                            return default;
                        }
                    }
                }

                return values.Aggregate((f, g) => f + g);
            }
        }

        private class UIntFlagsUtility : FlagsUtility<uint>
        {
            public override int ByteCount => (sizeof(uint) * 8) - 1;

            public override uint MaxValue => uint.MaxValue;

            public override Optional<uint> GetUniquePowerOfTwo(IEnumerable<uint> reservedValues, bool startFromHighestExistingValue = false)
            {
                if (reservedValues == null)
                    throw new ArgumentNullException(nameof(reservedValues));

                uint[] values = reservedValues.Where(f => IsZeroOrPowerOfTwo(f)).ToArray();

                if (values.Length == 0)
                    return 0;

                if (values.Length == 1 && values[0] == 0)
                    return 1;

                if (startFromHighestExistingValue)
                {
                    uint i = values.Max();

                    i *= 2;

                    if (i > 0)
                        return i;
                }
                else
                {
                    uint i = 1;

                    while (i > 0)
                    {
                        if (Array.IndexOf(values, i) == -1)
                            return i;

                        i *= 2;
                    }
                }

                return default;
            }

            public override bool IsZeroOrPowerOfTwo(uint value)
            {
                return (value & (value - 1)) == 0;
            }

            public override bool IsPowerOfTwo(uint value)
            {
                return value > 0 && (value & (value - 1)) == 0;
            }

            public override bool IsComposite(uint value)
            {
                return value > 0
                    && (value & (value - 1)) != 0;
            }

            public override IEnumerable<uint> Decompose(uint value)
            {
                for (int i = 0; i < ByteCount; i++)
                {
                    var x = (uint)(1 << i);

                    if (x > value)
                        yield break;

                    if ((value & x) != 0)
                        yield return x;
                }
            }

            public override Optional<uint> TryCompose(ImmutableArray<uint> values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        if (j != i
                            && (values[i] & values[j]) != 0)
                        {
                            return default;
                        }
                    }
                }

                return values.Aggregate((f, g) => f + g);
            }
        }

        private class LongFlagsUtility : FlagsUtility<long>
        {
            public override int ByteCount => (sizeof(long) * 8) - 1;

            public override long MaxValue => long.MaxValue;

            public override Optional<long> GetUniquePowerOfTwo(IEnumerable<long> reservedValues, bool startFromHighestExistingValue = false)
            {
                if (reservedValues == null)
                    throw new ArgumentNullException(nameof(reservedValues));

                long[] values = reservedValues.Where(f => f >= 0 && IsZeroOrPowerOfTwo(f)).ToArray();

                if (values.Length == 0)
                    return 0;

                if (values.Length == 1 && values[0] == 0)
                    return 1;

                if (startFromHighestExistingValue)
                {
                    long i = values.Max();

                    i *= 2;

                    if (i > 0)
                        return i;
                }
                else
                {
                    long i = 1;

                    while (i > 0)
                    {
                        if (Array.IndexOf(values, i) == -1)
                            return i;

                        i *= 2;
                    }
                }

                return default;
            }

            public override bool IsZeroOrPowerOfTwo(long value)
            {
                return (value & (value - 1)) == 0;
            }

            public override bool IsPowerOfTwo(long value)
            {
                return value > 0 && (value & (value - 1)) == 0;
            }

            public override bool IsComposite(long value)
            {
                return value > 0
                    && (value & (value - 1)) != 0;
            }

            public override IEnumerable<long> Decompose(long value)
            {
                for (int i = 0; i < ByteCount; i++)
                {
                    long x = 1L << i;

                    if (x > value)
                        yield break;

                    if ((value & x) != 0)
                        yield return x;
                }
            }

            public override Optional<long> TryCompose(ImmutableArray<long> values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        if (j != i
                            && (values[i] & values[j]) != 0)
                        {
                            return default;
                        }
                    }
                }

                return values.Aggregate((f, g) => f + g);
            }
        }

        private class ULongFlagsUtility : FlagsUtility<ulong>
        {
            public override int ByteCount => (sizeof(ulong) * 8) - 1;

            public override ulong MaxValue => ulong.MaxValue;

            public override Optional<ulong> GetUniquePowerOfTwo(IEnumerable<ulong> reservedValues, bool startFromHighestExistingValue = false)
            {
                if (reservedValues == null)
                    throw new ArgumentNullException(nameof(reservedValues));

                ulong[] values = reservedValues.Where(f => IsZeroOrPowerOfTwo(f)).ToArray();

                if (values.Length == 0)
                    return 0;

                if (values.Length == 1 && values[0] == 0)
                    return 1;

                if (startFromHighestExistingValue)
                {
                    ulong i = values.Max();

                    i *= 2;

                    if (i > 0)
                        return i;
                }
                else
                {
                    ulong i = 1;

                    while (i > 0)
                    {
                        if (Array.IndexOf(values, i) == -1)
                            return i;

                        i *= 2;
                    }
                }

                return default;
            }

            public override bool IsZeroOrPowerOfTwo(ulong value)
            {
                return (value & (value - 1)) == 0;
            }

            public override bool IsPowerOfTwo(ulong value)
            {
                return value > 0 && (value & (value - 1)) == 0;
            }

            public override bool IsComposite(ulong value)
            {
                return value > 0
                    && (value & (value - 1)) != 0;
            }

            public override IEnumerable<ulong> Decompose(ulong value)
            {
                for (int i = 0; i < ByteCount; i++)
                {
                    ulong x = 1UL << i;

                    if (x > value)
                        yield break;

                    if ((value & x) != 0)
                        yield return x;
                }
            }

            public override Optional<ulong> TryCompose(ImmutableArray<ulong> values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        if (j != i
                            && (values[i] & values[j]) != 0)
                        {
                            return default;
                        }
                    }
                }

                return values.Aggregate((f, g) => f + g);
            }
        }
    }
}