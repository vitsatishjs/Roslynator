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
                    mw.WriteListItem();
                    mw.Write(analyzer.Id);
                    mw.Write(" - ");
                    mw.WriteLink(analyzer.Title.TrimEnd('.'), $"docs/analyzers/{analyzer.Id}.md");
                    mw.WriteLine();
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
                    mw.WriteListItem();
                    mw.WriteBold("Syntax");
                    mw.Write(": ");

                    bool isFirst = true;

                    foreach (SyntaxDescriptor syntax in refactoring.Syntaxes)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                        }
                        else
                        {
                            mw.Write(", ");
                        }

                        mw.WriteMarkdown(syntax.Name);
                    }

                    mw.WriteLine();

                    if (!string.IsNullOrEmpty(refactoring.Span))
                    {
                        mw.WriteListItem();
                        mw.WriteBold("Span");
                        mw.Write(": ");
                        mw.WriteMarkdown(refactoring.Span);
                        mw.WriteLine();
                    }

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

                mw.WriteHeader(beforeHeader);
                mw.WriteCSharpCodeBlock(sample.Before);

                if (!string.IsNullOrEmpty(sample.After))
                {
                    mw.WriteHeader(afterHeader);
                    mw.WriteCSharpCodeBlock(sample.After);
                }
            }
        }

        public static string CreateRefactoringMarkdown(RefactoringDescriptor refactoring)
        {
            using (var mw = new MarkdownWriter(new MarkdownSettings(tableFormatting: TableFormatting.All)))
            {
                mw.WriteHeader2(refactoring.Title);

                var table = new Table();

                table.AddHeaders("Property", "Value");
                table.AddRow("Id", refactoring.Id);
                table.AddRow("Title", refactoring.Title);
                table.AddRow("Syntax", string.Join(", ", refactoring.Syntaxes.Select(f => f.Name)));

                if (!string.IsNullOrEmpty(refactoring.Span))
                    table.AddRow("Span", refactoring.Span);

                table.AddRow("Enabled by Default", RawText(GetBooleanAsText(refactoring.IsEnabledByDefault)));

                mw.WriteTable(table);

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
                string title = analyzer.Title.TrimEnd('.');
                mw.WriteHeader1($"{((analyzer.IsObsolete) ? "[deprecated] " : "")}{analyzer.Id}: {title}");

                var table = new Table();

                table.AddHeaders("Property", "Value");
                table.AddRow("Id", analyzer.Id);
                table.AddRow("Category", analyzer.Category);
                table.AddRow("Default Severity", analyzer.DefaultSeverity);
                table.AddRow("Enabled by Default", RawText(GetBooleanAsText(analyzer.IsEnabledByDefault)));
                table.AddRow("Supports Fade-Out", RawText(GetBooleanAsText(analyzer.SupportsFadeOut)));
                table.AddRow("Supports Fade-Out Analyzer", RawText(GetBooleanAsText(analyzer.SupportsFadeOutAnalyzer)));
                mw.WriteTable(table);

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

                mw.WriteCSharpCodeBlock($@"#pragma warning disable {analyzer.Id} // {analyzer.Title}
#pragma warning restore {analyzer.Id} // {analyzer.Title}");

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

                var table = new Table();

                table.AddHeaders("Id", "Title", "Category", TableHeader("Enabled by Default", Alignment.Center));

                foreach (AnalyzerDescriptor analyzer in analyzers.OrderBy(f => f.Id, comparer))
                {
                    table.AddRow(
                        analyzer.Id,
                        Link(analyzer.Title.TrimEnd('.'), $"../../docs/analyzers/{analyzer.Id}.md"),
                        analyzer.Category,
                        (analyzer.IsEnabledByDefault) ? RawText("&#x2713;") : null);
                }

                mw.WriteTable(table);

                return mw.ToString();
            }
        }

        public static string CreateRefactoringsReadMe(IEnumerable<RefactoringDescriptor> refactorings, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Refactorings");

                var table = new Table();

                table.AddHeaders("Id", "Title", TableHeader("Enabled by Default", Alignment.Center));

                foreach (RefactoringDescriptor refactoring in refactorings.OrderBy(f => f.Title, comparer))
                {
                    table.AddRow(
                        refactoring.Id,
                        Link(refactoring.Title.TrimEnd('.'), $"../../docs/refactorings/{refactoring.Id}.md"),
                        (refactoring.IsEnabledByDefault) ? RawText("&#x2713;") : null);
                }

                mw.WriteTable(table);

                return mw.ToString();
            }
        }

        public static string CreateCodeFixesReadMe(IEnumerable<CodeFixDescriptor> codeFixes, IEnumerable<CompilerDiagnosticDescriptor> diagnostics, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Code Fixes");

                var table = new Table();

                table.AddHeaders("Id", "Title", "Fixable Diagnostics", TableHeader("Enabled by Default", Alignment.Center));

                foreach (CodeFixDescriptor codeFix in codeFixes.OrderBy(f => f.Title, comparer))
                {
                    IEnumerable<object> links = codeFix
                        .FixableDiagnosticIds
                        .Join(diagnostics, f => f, f => f.Id, (f, g) => (object)Link(f, g.HelpUrl));

                    table.AddRow(
                        codeFix.Id,
                        codeFix.Title.TrimEnd('.'),
                        Join(", ", links),
                        (codeFix.IsEnabledByDefault) ? RawText("&#x2713;") : null);
                }

                mw.WriteTable(table);

                return mw.ToString();
            }
        }

        public static string CreateCodeFixesByDiagnosticId(IEnumerable<CodeFixDescriptor> codeFixes, IEnumerable<CompilerDiagnosticDescriptor> diagnostics)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Code Fixes by Diagnostic Id");

                var table = new Table();

                table.AddHeaders("Diagnostic", "Code Fixes");

                foreach (var grouping in codeFixes
                    .SelectMany(codeFix => codeFix.FixableDiagnosticIds.Select(diagnosticId => new { DiagnosticId = diagnosticId, CodeFixDescriptor = codeFix }))
                    .OrderBy(f => f.DiagnosticId)
                    .ThenBy(f => f.CodeFixDescriptor.Id)
                    .GroupBy(f => f.DiagnosticId))
                {
                    CompilerDiagnosticDescriptor diagnostic = diagnostics.FirstOrDefault(f => f.Id == grouping.Key);

                    var row = new TableRow();

                    if (!string.IsNullOrEmpty(diagnostic?.HelpUrl))
                    {
                        row.Add(Link(diagnostic.Id, diagnostic.HelpUrl));
                    }
                    else
                    {
                        row.Add(grouping.Key);
                    }

                    row.Add(Join(", ", grouping.Select(f => f.CodeFixDescriptor.Id)));
                    table.Rows.Add(row);
                }

                mw.WriteTable(table);

                return mw.ToString();
            }
        }

        public static string CreateAnalyzersByCategoryMarkdown(IEnumerable<AnalyzerDescriptor> analyzers, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Analyzers by Category");

                var table = new Table();

                table.AddHeaders("Category", "Title", "Id", TableHeader("Enabled by Default", Alignment.Center));

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
                            (analyzer.IsEnabledByDefault) ? RawText("&#x2713;") : null);
                    }
                }

                mw.WriteTable(table);

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
