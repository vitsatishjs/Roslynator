﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1241ImplementNonGenericCounterpartTests : AbstractCSharpFixVerifier
    {
        private static readonly ImplementNonGenericCounterpartCodeFixProvider _fixProvider = new ImplementNonGenericCounterpartCodeFixProvider();

        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.ImplementNonGenericCounterpart;

        public override DiagnosticAnalyzer Analyzer { get; } = new NamedTypeSymbolAnalyzer();

        public override CodeFixProvider FixProvider { get; } = _fixProvider;

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ImplementNonGenericCounterpart)]
        public async Task Test_IComparable()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
using System.Collections.Generic;

public class C
{
}

public abstract class [|Comparable|] : IComparable<C>
{
    public abstract int CompareTo(C other);
}
", @"
using System;
using System.Collections.Generic;

public class C
{
}

public abstract class Comparable : IComparable<C>, IComparable
{
    public abstract int CompareTo(C other);

    public int CompareTo(object obj)
    {
        if (obj == null)
        {
            return 1;
        }

        if (obj is C x)
        {
            return CompareTo(x);
        }

        throw new ArgumentException("""", nameof(obj));
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ImplementNonGenericCounterpart)]
        public async Task Test_IComparable_Explicit()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
using System.Collections.Generic;

public class C
{
}

public abstract class [|Comparable|] : IComparable<C>
{
    public abstract int CompareTo(C other);
}
", @"
using System;
using System.Collections.Generic;

public class C
{
}

public abstract class Comparable : IComparable<C>, IComparable
{
    public abstract int CompareTo(C other);

    int IComparable.CompareTo(object obj)
    {
        if (obj == null)
        {
            return 1;
        }

        if (obj is C x)
        {
            return CompareTo(x);
        }

        throw new ArgumentException("""", nameof(obj));
    }
}
", equivalenceKey: _fixProvider.ExplicitEquivalenceKey);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ImplementNonGenericCounterpart)]
        public async Task Test_IComparer()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
using System.Collections;
using System.Collections.Generic;

public class C
{
}

public abstract class [|Comparer|] : IComparer<C>
{
    public abstract int Compare(C x, C y);
}
", @"
using System;
using System.Collections;
using System.Collections.Generic;

public class C
{
}

public abstract class Comparer : IComparer<C>, IComparer
{
    public abstract int Compare(C x, C y);

    public int Compare(object x, object y)
    {
        if (x == y)
        {
            return 0;
        }

        if (x == null)
        {
            return -1;
        }

        if (y == null)
        {
            return 1;
        }

        if (x is C a
            && y is C b)
        {
            return Compare(a, b);
        }

        throw new ArgumentException("""", nameof(x));
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ImplementNonGenericCounterpart)]
        public async Task Test_IComparer_Explicit()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
using System.Collections;
using System.Collections.Generic;

public class C
{
}

public abstract class [|Comparer|] : IComparer<C>
{
    public abstract int Compare(C x, C y);
}
", @"
using System;
using System.Collections;
using System.Collections.Generic;

public class C
{
}

public abstract class Comparer : IComparer<C>, IComparer
{
    public abstract int Compare(C x, C y);

    int IComparer.Compare(object x, object y)
    {
        if (x == y)
        {
            return 0;
        }

        if (x == null)
        {
            return -1;
        }

        if (y == null)
        {
            return 1;
        }

        if (x is C a
            && y is C b)
        {
            return Compare(a, b);
        }

        throw new ArgumentException("""", nameof(x));
    }
}
", equivalenceKey: _fixProvider.ExplicitEquivalenceKey);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ImplementNonGenericCounterpart)]
        public async Task Test_IEqualityComparer()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
using System.Collections;
using System.Collections.Generic;

public class C
{
}

public abstract class [|EqualityComparer|] : IEqualityComparer<C>
{
    public abstract bool Equals(C x, C y);

    public abstract int GetHashCode(C obj);
}
", @"
using System;
using System.Collections;
using System.Collections.Generic;

public class C
{
}

public abstract class EqualityComparer : IEqualityComparer<C>, IEqualityComparer
{
    public abstract bool Equals(C x, C y);

    public abstract int GetHashCode(C obj);

    new public bool Equals(object x, object y)
    {
        if (x == y)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        if (x is C a
            && y is C b)
        {
            return Equals(a, b);
        }

        throw new ArgumentException("""", nameof(x));
    }

    public int GetHashCode(object obj)
    {
        if (obj == null)
        {
            return 0;
        }

        if (obj is C x)
        {
            return GetHashCode(x);
        }

        throw new ArgumentException("""", nameof(obj));
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ImplementNonGenericCounterpart)]
        public async Task Test_IEqualityComparer_Explicit()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
using System.Collections;
using System.Collections.Generic;

public class C
{
}

public abstract class [|EqualityComparer|] : IEqualityComparer<C>
{
    public abstract bool Equals(C x, C y);

    public abstract int GetHashCode(C obj);
}
", @"
using System;
using System.Collections;
using System.Collections.Generic;

public class C
{
}

public abstract class EqualityComparer : IEqualityComparer<C>, IEqualityComparer
{
    public abstract bool Equals(C x, C y);

    public abstract int GetHashCode(C obj);

    bool IEqualityComparer.Equals(object x, object y)
    {
        if (x == y)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        if (x is C a
            && y is C b)
        {
            return Equals(a, b);
        }

        throw new ArgumentException("""", nameof(x));
    }

    int IEqualityComparer.GetHashCode(object obj)
    {
        if (obj == null)
        {
            return 0;
        }

        if (obj is C x)
        {
            return GetHashCode(x);
        }

        throw new ArgumentException("""", nameof(obj));
    }
}
", equivalenceKey: _fixProvider.ExplicitEquivalenceKey);
        }
    }
}
