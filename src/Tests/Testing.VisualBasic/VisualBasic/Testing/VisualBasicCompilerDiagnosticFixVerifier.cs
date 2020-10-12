﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using Roslynator.Testing;

namespace Roslynator.VisualBasic.Testing
{
    public abstract class VisualBasicCompilerDiagnosticFixVerifier : CompilerDiagnosticFixVerifier
    {
        private VisualBasicCodeVerificationOptions _options;

        protected VisualBasicCompilerDiagnosticFixVerifier() : base(VisualBasicWorkspaceFactory.Instance)
        {
        }

        new public VisualBasicCodeVerificationOptions Options
        {
            get
            {
                if (_options == null)
                    Interlocked.CompareExchange(ref _options, CreateAndUpdateOptions(), null);

                return _options;
            }
        }

        protected override CodeVerificationOptions CommonOptions => Options;

        private VisualBasicCodeVerificationOptions CreateAndUpdateOptions()
        {
            VisualBasicCodeVerificationOptions options = CreateOptions();

            return UpdateOptions(options);
        }

        protected virtual VisualBasicCodeVerificationOptions CreateOptions()
        {
            return VisualBasicCodeVerificationOptions.Default;
        }

        protected virtual VisualBasicCodeVerificationOptions UpdateOptions(VisualBasicCodeVerificationOptions options)
        {
            return options;
        }
    }
}
