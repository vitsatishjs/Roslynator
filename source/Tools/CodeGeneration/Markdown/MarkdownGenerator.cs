// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Roslynator.Metadata;
using Roslynator.Utilities.Markdown;
using static Roslynator.Utilities.Markdown.MarkdownFactory;

namespace Roslynator.CodeGeneration.Markdown
{
    public static class MarkdownGenerator
    {
        public static string CreateReadMe(IEnumerable<AnalyzerDescriptor> analyzers, IEnumerable<RefactoringDescriptor> refactorings, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteLine(File.ReadAllText(@"..\text\ReadMe.txt", Encoding.UTF8));

                mw.WriteHeader3("List of Analyzers");

                foreach (AnalyzerDescriptor analyzer in analyzers.OrderBy(f => f.Id, comparer))
                {
                    mw.WriteListItem(analyzer.Id, " - ", Link(analyzer.Title.TrimEnd('.'), $"docs/analyzers/{analyzer.Id}.md"));
                }

                mw.WriteHeader3("List of Refactorings");

                foreach (RefactoringDescriptor refactoring in refactorings.OrderBy(f => f.Title, comparer))
                {
                    mw.WriteListItem(Link(refactoring.Title.TrimEnd('.'), $"docs/refactorings/{refactoring.Id}.md"));
                }

                return mw.ToString();
            }
        }

        public static string CreateRefactoringsMarkdown(IEnumerable<RefactoringDescriptor> refactorings, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.Write(Header2("Roslynator Refactorings"));

                foreach (RefactoringDescriptor refactoring in refactorings.OrderBy(f => f.Title, comparer))
                {
                    mw.WriteHeader4($"{refactoring.Title} ({refactoring.Id})");
                    mw.WriteListItem(Bold("Syntax"), ": ", Join(", ", refactoring.Syntaxes.Select(f => f.Name)));

                    if (!string.IsNullOrEmpty(refactoring.Span))
                        mw.WriteListItem(Bold("Span"), ": ", refactoring.Span);

                    WriteRefactoringSamples(mw, refactoring);
                }

                return mw.ToString();
            }
        }

        private static void WriteRefactoringSamples(MarkdownWriter mw, RefactoringDescriptor refactoring)
        {
            if (refactoring.Samples.Count > 0)
            {
                WriteSamples(mw, refactoring.Samples, Header4("Before"), Header4("After"));
            }
            else if (refactoring.Images.Count > 0)
            {
                bool isFirst = true;

                foreach (ImageDescriptor image in refactoring.Images)
                {
                    if (!isFirst)
                        mw.WriteLine();

                    WriteRefactoringImage(mw, refactoring, image.Name);
                    isFirst = false;
                }

                mw.WriteLine();
            }
            else
            {
                WriteRefactoringImage(mw, refactoring, refactoring.Identifier);
                mw.WriteLine();
            }
        }

        private static void WriteSamples(MarkdownWriter mw, IEnumerable<SampleDescriptor> samples, Header beforeHeader, Header afterHeader)
        {
            bool isFirst = true;

            foreach (SampleDescriptor sample in samples)
            {
                if (!isFirst)
                {
                    mw.WriteHorizonalRule();
                }
                else
                {
                    isFirst = false;
                }

                mw.Write(beforeHeader);
                mw.WriteCSharpCodeBlock(sample.Before);

                if (!string.IsNullOrEmpty(sample.After))
                {
                    mw.Write(afterHeader);
                    mw.WriteCSharpCodeBlock(sample.After);
                }
            }
        }

        public static string CreateRefactoringMarkdown(RefactoringDescriptor refactoring)
        {
            using (var mw = new MarkdownWriter(new MarkdownSettings(tableFormatting: TableFormatting.All)))
            {
                mw.WriteHeader2(refactoring.Title);

                Table("Property", "Value")
                    .AddRow("Id", refactoring.Id)
                    .AddRow("Title", refactoring.Title)
                    .AddRow("Syntax", string.Join(", ", refactoring.Syntaxes.Select(f => f.Name)))
                    .AddRowIf(!string.IsNullOrEmpty(refactoring.Span), "Span", refactoring.Span)
                    .AddRow("Enabled by Default", RawText(GetBooleanAsText(refactoring.IsEnabledByDefault)))
                    .WriteTo(mw);

                mw.WriteHeader3("Usage");

                WriteRefactoringSamples(mw, refactoring);

                mw.WriteLink("full list of refactorings", "Refactorings.md");

                return mw.ToString();
            }
        }

        public static string CreateAnalyzerMarkdown(AnalyzerDescriptor analyzer)
        {
            using (var mw = new MarkdownWriter(new MarkdownSettings(tableFormatting: TableFormatting.All)))
            {
                mw.WriteHeader1($"{((analyzer.IsObsolete) ? "[deprecated] " : "")}{analyzer.Id}: {analyzer.Title.TrimEnd('.')}");

                Table table = Table("Property", "Value")
                    .AddRow("Id", analyzer.Id)
                    .AddRow("Category", analyzer.Category)
                    .AddRow("Default Severity", analyzer.DefaultSeverity)
                    .AddRow("Enabled by Default", RawText(GetBooleanAsText(analyzer.IsEnabledByDefault)))
                    .AddRow("Supports Fade-Out", RawText(GetBooleanAsText(analyzer.SupportsFadeOut)))
                    .AddRow("Supports Fade-Out Analyzer", RawText(GetBooleanAsText(analyzer.SupportsFadeOutAnalyzer)));

                mw.Write(table);

                ReadOnlyCollection<SampleDescriptor> samples = analyzer.Samples;

                if (samples.Count > 0)
                {
                    mw.WriteHeader2((samples.Count == 1) ? "Example" : "Examples");

                    WriteSamples(mw, samples, Header3("Code with Diagnostic"), Header3("Code with Fix"));
                }

                mw.WriteHeader2("How to Suppress");

                mw.WriteHeader3("SuppressMessageAttribute");

                mw.WriteCSharpCodeBlock($"[assembly: SuppressMessage(\"{analyzer.Category}\", \"{analyzer.Id}:{analyzer.Title}\", Justification = \"<Pending>\")]");

                mw.WriteHeader3("#pragma");

                mw.WriteCSharpCodeBlock($"#pragma warning disable {analyzer.Id} // {analyzer.Title}\r\n#pragma warning restore {analyzer.Id} // {analyzer.Title}");

                mw.WriteHeader3("Ruleset");

                mw.WriteListItem(Link("How to configure rule set", "../HowToConfigureAnalyzers.md"));

                return mw.ToString();
            }
        }

        public static string CreateAnalyzersReadMe(IEnumerable<AnalyzerDescriptor> analyzers, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Analyzers");

                Table table = Table("Id", "Title", "Category", TableHeader("Enabled by Default", Alignment.Center));

                foreach (AnalyzerDescriptor analyzer in analyzers.OrderBy(f => f.Id, comparer))
                {
                    table.AddRow(
                        analyzer.Id,
                        Link(analyzer.Title.TrimEnd('.'), $"../../docs/analyzers/{analyzer.Id}.md"),
                        analyzer.Category,
                        RawText((analyzer.IsEnabledByDefault) ? "&#x2713;" : ""));
                }

                mw.Write(table);

                return mw.ToString();
            }
        }

        public static string CreateRefactoringsReadMe(IEnumerable<RefactoringDescriptor> refactorings, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Refactorings");

                Table table = Table("Id", "Title", TableHeader("Enabled by Default", Alignment.Center));

                foreach (RefactoringDescriptor refactoring in refactorings.OrderBy(f => f.Title, comparer))
                {
                    table.AddRow(
                        refactoring.Id,
                        Link(refactoring.Title.TrimEnd('.'), $"../../docs/refactorings/{refactoring.Id}.md"),
                        RawText((refactoring.IsEnabledByDefault) ? "&#x2713;" : ""));
                }

                mw.Write(table);

                return mw.ToString();
            }
        }

        public static string CreateCodeFixesReadMe(IEnumerable<CodeFixDescriptor> codeFixes, IEnumerable<CompilerDiagnosticDescriptor> diagnostics, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Code Fixes");

                Table table = Table("Id", "Title", "Fixable Diagnostics", TableHeader("Enabled by Default", Alignment.Center));

                foreach (CodeFixDescriptor codeFix in codeFixes.OrderBy(f => f.Title, comparer))
                {
                    table.AddRow(
                        codeFix.Id,
                        codeFix.Title.TrimEnd('.'),
                        Join(", ", codeFix.FixableDiagnosticIds.Join(diagnostics, f => f, f => f.Id, (f, g) => (object)Link(f, g.HelpUrl))),
                        RawText((codeFix.IsEnabledByDefault) ? "&#x2713;" : ""));
                }

                mw.Write(table);

                return mw.ToString();
            }
        }

        public static string CreateCodeFixesByDiagnosticId(IEnumerable<CodeFixDescriptor> codeFixes, IEnumerable<CompilerDiagnosticDescriptor> diagnostics)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Code Fixes by Diagnostic Id");

                Table table = Table("Diagnostic", "Code Fixes");

                foreach (var grouping in codeFixes
                    .SelectMany(codeFix => codeFix.FixableDiagnosticIds.Select(diagnosticId => new { DiagnosticId = diagnosticId, CodeFixDescriptor = codeFix }))
                    .OrderBy(f => f.DiagnosticId)
                    .ThenBy(f => f.CodeFixDescriptor.Id)
                    .GroupBy(f => f.DiagnosticId))
                {
                    CompilerDiagnosticDescriptor diagnostic = diagnostics.FirstOrDefault(f => f.Id == grouping.Key);

                    table.AddRow(
                        Link(diagnostic?.Id ?? grouping.Key, diagnostic?.HelpUrl),
                        Join(", ", grouping.Select(f => f.CodeFixDescriptor.Id)));
                }

                mw.Write(table);

                return mw.ToString();
            }
        }

        public static string CreateAnalyzersByCategoryMarkdown(IEnumerable<AnalyzerDescriptor> analyzers, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Analyzers by Category");

                Table table = Table("Category", "Title", "Id", TableHeader("Enabled by Default", Alignment.Center));

                foreach (IGrouping<string, AnalyzerDescriptor> grouping in analyzers
                    .GroupBy(f => mw.Settings.Escape(f.Category))
                    .OrderBy(f => f.Key, comparer))
                {
                    foreach (AnalyzerDescriptor analyzer in grouping.OrderBy(f => f.Title, comparer))
                    {
                        table.AddRow(
                            grouping.Key,
                            Link(analyzer.Title.TrimEnd('.'), $"../../docs/analyzers/{analyzer.Id}.md"),
                            analyzer.Id,
                            RawText((analyzer.IsEnabledByDefault) ? "&#x2713;" : ""));
                    }
                }

                mw.Write(table);

                return mw.ToString();
            }
        }

        private static void WriteRefactoringImage(MarkdownWriter mw, RefactoringDescriptor refactoring, string fileName)
        {
            mw.WriteImage(refactoring.Title, $"../../images/refactorings/{fileName}.png");
            mw.WriteLine();
        }

        private static string GetBooleanAsText(bool value)
        {
            return (value) ? "&#x2713;" : "-";
        }
    }
}
