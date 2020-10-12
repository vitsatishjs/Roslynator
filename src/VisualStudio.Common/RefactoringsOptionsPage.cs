﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Roslynator.VisualStudio
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("1D9ECCF3-5D2F-4112-9B25-264596873DC9")]
    public partial class RefactoringsOptionsPage : BaseOptionsPage
    {
        private const string RefactoringCategory = "Refactoring";

        [Category(RefactoringCategory)]
        [Browsable(false)]
        public string DisabledRefactorings
        {
            get { return string.Join(",", DisabledItems); }
            set
            {
                DisabledItems.Clear();

                if (!string.IsNullOrEmpty(value))
                {
                    foreach (string id in value.Split(','))
                        DisabledItems.Add(id);
                }
            }
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);

            if (e.ApplyBehavior == ApplyKind.Apply)
            {
                ApplyTo(Settings.Instance);
                Settings.Instance.ApplyTo(RefactoringSettings.Current);
            }
        }

        internal void ApplyTo(Settings settings)
        {
            IEnumerable<KeyValuePair<string, bool>> refactorings = GetDisabledItems()
                .Select(f => new KeyValuePair<string, bool>(f, false));

            settings.VisualStudio = settings.VisualStudio.WithRefactorings(refactorings);
        }
    }
}
