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
                mw.WriteLine();

                foreach (AnalyzerDescriptor analyzer in analyzers.OrderBy(f => f.Id, comparer))
                {
                    mw.WriteListItem();
                    mw.Write(analyzer.Id);
                    mw.Write(" - ");
                    mw.WriteLink(analyzer.Title.TrimEnd('.'), $"docs/analyzers/{analyzer.Id}.md");
                    mw.WriteLine();
                }

                mw.WriteLine();
                mw.WriteHeader3("List of Refactorings");
                mw.WriteLine();

                foreach (RefactoringDescriptor refactoring in refactorings.OrderBy(f => f.Title, comparer))
                {
                    mw.WriteListItem();
                    mw.WriteLink(refactoring.Title.TrimEnd('.'), $"docs/refactorings/{refactoring.Id}.md");
                }

                return mw.ToString();
            }
        }

        public static string CreateRefactoringsMarkdown(IEnumerable<RefactoringDescriptor> refactorings, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.Write(Header2("Roslynator Refactorings"));

                foreach (RefactoringDescriptor refactoring in refactorings
                    .OrderBy(f => f.Title, comparer))
                {
                    mw.WriteLine();
                    mw.WriteHeader4($"{refactoring.Title} ({refactoring.Id})");
                    mw.WriteLine();
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

                        mw.WriteMarkdown(syntax.Name, escape: true);
                    }

                    mw.WriteLine();

                    if (!string.IsNullOrEmpty(refactoring.Span))
                    {
                        mw.WriteListItem();
                        mw.WriteBold("Span");
                        mw.Write(": ");
                        mw.WriteMarkdown(refactoring.Span, escape: true);
                        mw.WriteLine();
                    }

                    mw.WriteLine();

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
            }
            else
            {
                WriteRefactoringImage(mw, refactoring, refactoring.Identifier);
            }
        }

        private static void WriteSamples(MarkdownWriter mw, IEnumerable<SampleDescriptor> samples, MarkdownHeader beforeHeader, MarkdownHeader afterHeader)
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
                mw.WriteLine();
                mw.WriteCSharpCodeBlock(sample.Before);

                if (!string.IsNullOrEmpty(sample.After))
                {
                    mw.WriteLine();
                    mw.WriteHeader(afterHeader);
                    mw.WriteLine();
                    mw.WriteCSharpCodeBlock(sample.After);
                }
            }
        }

        public static string CreateRefactoringMarkdown(RefactoringDescriptor refactoring)
        {
            using (var mw = new MarkdownWriter(new MarkdownSettings(tableFormatting: MarkdownTableFormatting.All)))
            {
                mw.WriteHeader2(refactoring.Title);
                mw.WriteLine();

                var tb = new MarkdownTableBuilder();

                tb.AddHeaders("Property", "Value");
                tb.AddRow("Id", refactoring.Id);
                tb.AddRow("Title", refactoring.Title);
                tb.AddRow("Syntax", string.Join(", ", refactoring.Syntaxes.Select(f => f.Name)));

                if (!string.IsNullOrEmpty(refactoring.Span))
                    tb.AddRow("Span", refactoring.Span);

                tb.AddRow("Enabled by Default", RawText(GetBooleanAsText(refactoring.IsEnabledByDefault)));

                tb.WriteTo(mw);

                mw.WriteLine();
                mw.WriteHeader3("Usage");
                mw.WriteLine();

                WriteRefactoringSamples(mw, refactoring);

                mw.WriteLine();

                mw.WriteLink("full list of refactorings", "Refactorings.md");

                return mw.ToString();
            }
        }

        public static string CreateAnalyzerMarkdown(AnalyzerDescriptor analyzer)
        {
            using (var mw = new MarkdownWriter(new MarkdownSettings(tableFormatting: MarkdownTableFormatting.All)))
            {
                string title = analyzer.Title.TrimEnd('.');
                mw.WriteHeader1($"{((analyzer.IsObsolete) ? "[deprecated] " : "")}{analyzer.Id}: {title}");
                mw.WriteLine();

                var tb = new MarkdownTableBuilder();

                tb.AddHeaders("Property", "Value");
                tb.AddRow("Id", analyzer.Id);
                tb.AddRow("Category", analyzer.Category);
                tb.AddRow("Default Severity", analyzer.DefaultSeverity);
                tb.AddRow("Enabled by Default", RawText(GetBooleanAsText(analyzer.IsEnabledByDefault)));
                tb.AddRow("Supports Fade-Out", RawText(GetBooleanAsText(analyzer.SupportsFadeOut)));
                tb.AddRow("Supports Fade-Out Analyzer", RawText(GetBooleanAsText(analyzer.SupportsFadeOutAnalyzer)));
                tb.WriteTo(mw);

                ReadOnlyCollection<SampleDescriptor> samples = analyzer.Samples;

                if (samples.Count > 0)
                {
                    mw.WriteLine();
                    mw.WriteHeader2((samples.Count == 1) ? "Example" : "Examples");
                    mw.WriteLine();

                    WriteSamples(mw, samples, Header3("Code with Diagnostic"), Header3("Code with Fix"));
                }

                mw.WriteLine();
                mw.WriteHeader2("How to Suppress");
                mw.WriteLine();

                mw.WriteHeader3("SuppressMessageAttribute");
                mw.WriteLine();

                mw.WriteCSharpCodeBlock($"[assembly: SuppressMessage(\"{analyzer.Category}\", \"{analyzer.Id}:{analyzer.Title}\", Justification = \"<Pending>\")]");
                mw.WriteLine();

                mw.WriteHeader3("#pragma");
                mw.WriteLine();

                mw.WriteCSharpCodeBlock($@"#pragma warning disable {analyzer.Id} // {analyzer.Title}
#pragma warning restore {analyzer.Id} // {analyzer.Title}");

                mw.WriteLine();

                mw.WriteHeader3("Ruleset");
                mw.WriteLine();

                mw.WriteListItem();
                mw.WriteLink("How to configure rule set", "../HowToConfigureAnalyzers.md");
                mw.WriteLine();

                return mw.ToString();
            }
        }

        public static string CreateAnalyzersReadMe(IEnumerable<AnalyzerDescriptor> analyzers, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Analyzers");
                mw.WriteLine();

                mw.WriteTableHeader("Id", "Title", "Category", TableHeader("Enabled by Default", Alignment.Center));

                foreach (AnalyzerDescriptor analyzer in analyzers.OrderBy(f => f.Id, comparer))
                {
                    mw.WriteTableRow(
                        analyzer.Id,
                        Link(analyzer.Title.TrimEnd('.'), $"../../docs/analyzers/{analyzer.Id}.md"),
                        analyzer.Category,
                        (analyzer.IsEnabledByDefault) ? RawText("&#x2713;") : EmptyText);
                }

                return mw.ToString();
            }
        }

        public static string CreateRefactoringsReadMe(IEnumerable<RefactoringDescriptor> refactorings, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Refactorings");
                mw.WriteLine();

                mw.WriteTableHeader("Id", "Title", TableHeader("Enabled by Default", Alignment.Center));

                foreach (RefactoringDescriptor refactoring in refactorings.OrderBy(f => f.Title, comparer))
                {
                    mw.WriteTableRow(
                        refactoring.Id,
                        Link(refactoring.Title.TrimEnd('.'), $"../../docs/refactorings/{refactoring.Id}.md"),
                        (refactoring.IsEnabledByDefault) ? RawText("&#x2713;") : EmptyText);
                }

                return mw.ToString();
            }
        }

        public static string CreateCodeFixesReadMe(IEnumerable<CodeFixDescriptor> codeFixes, IEnumerable<CompilerDiagnosticDescriptor> diagnostics, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Code Fixes");
                mw.WriteLine();

                mw.WriteTableHeader("Id", "Title", "Fixable Diagnostics", TableHeader("Enabled by Default", Alignment.Center));

                foreach (CodeFixDescriptor codeFix in codeFixes.OrderBy(f => f.Title, comparer))
                {
                    IEnumerable<MarkdownLink> links = codeFix
                        .FixableDiagnosticIds
                        .Join(diagnostics, f => f, f => f.Id, (f, g) => Link(f, g.HelpUrl));

                    mw.WriteTableRowStart();
                    mw.WriteMarkdown(codeFix.Id);
                    mw.WriteTableRowSeparator();
                    mw.WriteMarkdown(codeFix.Title.TrimEnd('.'));
                    mw.WriteTableRowSeparator();
                    mw.WriteJoin(", ", links);
                    mw.WriteTableRowSeparator();
                    mw.WriteMarkdown((codeFix.IsEnabledByDefault) ? RawText("&#x2713;") : EmptyText);
                    mw.WriteTableRowEnd();
                }

                return mw.ToString();
            }
        }

        public static string CreateCodeFixesByDiagnosticId(IEnumerable<CodeFixDescriptor> codeFixes, IEnumerable<CompilerDiagnosticDescriptor> diagnostics)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Code Fixes by Diagnostic Id");
                mw.WriteLine();

                mw.WriteTableHeader("Diagnostic", "Code Fixes");

                foreach (var grouping in codeFixes
                    .SelectMany(codeFix => codeFix.FixableDiagnosticIds.Select(diagnosticId => new { DiagnosticId = diagnosticId, CodeFixDescriptor = codeFix }))
                    .OrderBy(f => f.DiagnosticId)
                    .ThenBy(f => f.CodeFixDescriptor.Id)
                    .GroupBy(f => f.DiagnosticId))
                {
                    CompilerDiagnosticDescriptor diagnostic = diagnostics.FirstOrDefault(f => f.Id == grouping.Key);

                    mw.WriteTableRowStart();

                    if (!string.IsNullOrEmpty(diagnostic?.HelpUrl))
                    {
                        mw.WriteLink(diagnostic.Id, diagnostic.HelpUrl);
                    }
                    else
                    {
                        mw.Write(grouping.Key);
                    }

                    mw.WriteTableRowSeparator();
                    mw.WriteJoin(", ", grouping.Select(f => MarkdownText(f.CodeFixDescriptor.Id)));
                    mw.WriteTableRowEnd();
                }

                return mw.ToString();
            }
        }

        public static string CreateAnalyzersByCategoryMarkdown(IEnumerable<AnalyzerDescriptor> analyzers, IComparer<string> comparer)
        {
            using (var mw = new MarkdownWriter())
            {
                mw.WriteHeader2("Roslynator Analyzers by Category");
                mw.WriteLine();

                mw.WriteTableHeader("Category", "Title", "Id", TableHeader("Enabled by Default", Alignment.Center));

                foreach (IGrouping<string, AnalyzerDescriptor> grouping in analyzers
                    .GroupBy(f => MarkdownEscaper.Default.Escape(f.Category))
                    .OrderBy(f => f.Key, comparer))
                {
                    foreach (AnalyzerDescriptor analyzer in grouping.OrderBy(f => f.Title, comparer))
                    {
                        mw.WriteTableRow(
                            grouping.Key,
                            Link(analyzer.Title.TrimEnd('.'), $"../../docs/analyzers/{analyzer.Id}.md"),
                            analyzer.Id,
                            (analyzer.IsEnabledByDefault) ? RawText("&#x2713;") : EmptyText);
                    }
                }

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
