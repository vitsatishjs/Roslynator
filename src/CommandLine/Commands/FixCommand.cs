﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Roslynator.CodeFixes;
using static Roslynator.Logger;

namespace Roslynator.CommandLine
{
    internal class FixCommand : MSBuildWorkspaceCommand
    {
        public FixCommand(
            FixCommandLineOptions options,
            DiagnosticSeverity severityLevel,
            IEnumerable<KeyValuePair<string, string>> diagnosticFixMap,
            IEnumerable<KeyValuePair<string, string>> diagnosticFixerMap,
            in ProjectFilter projectFilter) : base(projectFilter)
        {
            Options = options;
            SeverityLevel = severityLevel;
            DiagnosticFixMap = diagnosticFixMap;
            DiagnosticFixerMap = diagnosticFixerMap;
        }

        public FixCommandLineOptions Options { get; }

        public DiagnosticSeverity SeverityLevel { get; }

        public IEnumerable<KeyValuePair<string, string>> DiagnosticFixMap { get; }

        public IEnumerable<KeyValuePair<string, string>> DiagnosticFixerMap { get; }

        public override async Task<CommandResult> ExecuteAsync(ProjectOrSolution projectOrSolution, CancellationToken cancellationToken = default)
        {
            AssemblyResolver.Register();

            var codeFixerOptions = new CodeFixerOptions(
                severityLevel: SeverityLevel,
                ignoreCompilerErrors: Options.IgnoreCompilerErrors,
                ignoreAnalyzerReferences: Options.IgnoreAnalyzerReferences,
                supportedDiagnosticIds: Options.SupportedDiagnostics,
                ignoredDiagnosticIds: Options.IgnoredDiagnostics,
                ignoredCompilerDiagnosticIds: Options.IgnoredCompilerDiagnostics,
                diagnosticIdsFixableOneByOne: Options.DiagnosticsFixableOneByOne,
                diagnosticFixMap: DiagnosticFixMap,
                diagnosticFixerMap: DiagnosticFixerMap,
                fileBanner: Options.FileBanner,
                maxIterations: Options.MaxIterations,
                batchSize: Options.BatchSize,
                format: Options.Format);

            IEnumerable<AnalyzerAssembly> analyzerAssemblies = Options.AnalyzerAssemblies
                .SelectMany(path => AnalyzerAssemblyLoader.LoadFrom(path).Select(info => info.AnalyzerAssembly));

            CultureInfo culture = (Options.Culture != null) ? CultureInfo.GetCultureInfo(Options.Culture) : null;

            var projectFilter = new ProjectFilter(Options.Projects, Options.IgnoredProjects, Language);

            return await FixAsync(projectOrSolution, analyzerAssemblies, codeFixerOptions, projectFilter, culture, cancellationToken);
        }

        internal static async Task<CommandResult> FixAsync(
            ProjectOrSolution projectOrSolution,
            IEnumerable<AnalyzerAssembly> analyzerAssemblies,
            CodeFixerOptions codeFixerOptions,
            ProjectFilter projectFilter,
            IFormatProvider formatProvider = null,
            CancellationToken cancellationToken = default)
        {
            if (projectOrSolution.IsProject)
            {
                Project project = projectOrSolution.AsProject();

                Solution solution = project.Solution;

                CodeFixer codeFixer = GetCodeFixer(solution);

                WriteLine($"Fix '{project.Name}'", ConsoleColor.Cyan, Verbosity.Minimal);

                Stopwatch stopwatch = Stopwatch.StartNew();

                ProjectFixResult result = await codeFixer.FixProjectAsync(project, cancellationToken);

                stopwatch.Stop();

                WriteLine($"Done fixing project '{project.FilePath}' in {stopwatch.Elapsed:mm\\:ss\\.ff}", Verbosity.Minimal);

                LogHelpers.WriteProjectFixResults(new ProjectFixResult[] { result }, codeFixerOptions, formatProvider);
            }
            else
            {
                Solution solution = projectOrSolution.AsSolution();

                CodeFixer codeFixer = GetCodeFixer(solution);

                await codeFixer.FixSolutionAsync(f => projectFilter.IsMatch(f), cancellationToken);
            }

            return CommandResult.Success;

            CodeFixer GetCodeFixer(Solution solution)
            {
                return new CodeFixer(
                    solution,
                    analyzerAssemblies: analyzerAssemblies,
                    formatProvider: formatProvider,
                    options: codeFixerOptions);
            }
        }

        protected override void OperationCanceled(OperationCanceledException ex)
        {
            WriteLine("Fixing was canceled.", Verbosity.Quiet);
        }
    }
}
