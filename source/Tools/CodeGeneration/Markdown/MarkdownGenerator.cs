// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Pihrtsoft.Markdown;
using Roslynator.Metadata;
using static Pihrtsoft.Markdown.MarkdownFactory;

namespace Roslynator.CodeGeneration.Markdown
{
    public static class MarkdownGenerator
    {
        public static string CreateReadMe(IEnumerable<AnalyzerDescriptor> analyzers, IEnumerable<RefactoringDescriptor> refactorings, IComparer<string> comparer)
        {
            var document = new MDocument(
                Heading3("List of Analyzers"),
                analyzers.OrderBy(f => f.Id, comparer).Select(analyzer => ListItem(analyzer.Id, " - ", Link(analyzer.Title.TrimEnd('.'), $"docs/analyzers/{analyzer.Id}.md"))),
                Heading3("List of Refactorings"),
                refactorings.OrderBy(f => f.Title, comparer).Select(refactoring => ListItem(Link(refactoring.Title.TrimEnd('.'), $"docs/refactorings/{refactoring.Id}.md"))));

            return File.ReadAllText(@"..\text\ReadMe.txt", Encoding.UTF8)
                + Environment.NewLine
                + document;
        }

        public static string CreateRefactoringsMarkdown(IEnumerable<RefactoringDescriptor> refactorings, IComparer<string> comparer)
        {
            var document = new MDocument(Heading2("Roslynator Refactorings"));

            foreach (RefactoringDescriptor refactoring in refactorings.OrderBy(f => f.Title, comparer))
            {
                document.Add(
                    Heading4($"{refactoring.Title} ({refactoring.Id})"),
                    ListItem(Bold("Syntax"), ": ", Join(", ", refactoring.Syntaxes.Select(f => f.Name))));

                if (!string.IsNullOrEmpty(refactoring.Span))
                    document.Add(ListItem(Bold("Span"), ": ", refactoring.Span));

                document.Add(GetRefactoringSamples(refactoring));
            }

            return document.ToString();
        }

        private static IEnumerable<MElement> GetRefactoringSamples(RefactoringDescriptor refactoring)
        {
            if (refactoring.Samples.Count > 0)
            {
                foreach (MElement element in GetSamples(refactoring.Samples, Heading4("Before"), Heading4("After")))
                    yield return element;
            }
            else if (refactoring.Images.Count > 0)
            {
                bool isFirst = true;

                foreach (ImageDescriptor image in refactoring.Images)
                {
                    if (!isFirst)
                        yield return NewLine;

                    yield return RefactoringImage(refactoring, image.Name);
                    yield return NewLine;

                    isFirst = false;
                }

                yield return NewLine;
            }
            else
            {
                yield return RefactoringImage(refactoring, refactoring.Identifier);
                yield return NewLine;
                yield return NewLine;
            }
        }

        private static IEnumerable<MElement> GetSamples(IEnumerable<SampleDescriptor> samples, Heading beforeHeader, Heading afterHeader)
        {
            bool isFirst = true;

            foreach (SampleDescriptor sample in samples)
            {
                if (!isFirst)
                {
                    yield return HorizontalRule();
                }
                else
                {
                    isFirst = false;
                }

                yield return beforeHeader;
                yield return FencedCodeBlock(sample.Before, LanguageIdentifiers.CSharp);

                if (!string.IsNullOrEmpty(sample.After))
                {
                    yield return afterHeader;
                    yield return FencedCodeBlock(sample.After, LanguageIdentifiers.CSharp);
                }
            }
        }

        public static string CreateRefactoringMarkdown(RefactoringDescriptor refactoring)
        {
            var format = new MarkdownFormat(tableOptions: MarkdownFormat.Default.TableOptions | TableOptions.FormatContent);

            var doc = new MDocument(
                Heading2(refactoring.Title),
                SimpleTable(TableHeader("Property", "Value"),
                    TableRow("Id", refactoring.Id),
                    TableRow("Title", refactoring.Title),
                    TableRow("Syntax", string.Join(", ", refactoring.Syntaxes.Select(f => f.Name))),
                    (!string.IsNullOrEmpty(refactoring.Span)) ? TableRow("Span", refactoring.Span) : null,
                    TableRow("Enabled by Default", CheckboxOrHyphen(refactoring.IsEnabledByDefault))),
                Heading3("Usage"),
                GetRefactoringSamples(refactoring),
                Link("full list of refactorings", "Refactorings.md"));

            return doc.ToString(format);
        }

        public static string CreateAnalyzerMarkdown(AnalyzerDescriptor analyzer)
        {
            var format = new MarkdownFormat(tableOptions: MarkdownFormat.Default.TableOptions | TableOptions.FormatContent);

            var document = new MDocument(
                Heading1($"{((analyzer.IsObsolete) ? "[deprecated] " : "")}{analyzer.Id}: {analyzer.Title.TrimEnd('.')}"),
                SimpleTable(
                    TableHeader("Property", "Value"),
                    TableRow("Id", analyzer.Id),
                    TableRow("Category", analyzer.Category),
                    TableRow("Default Severity", analyzer.DefaultSeverity),
                    TableRow("Enabled by Default", CheckboxOrHyphen(analyzer.IsEnabledByDefault)),
                    TableRow("Supports Fade-Out", CheckboxOrHyphen(analyzer.SupportsFadeOut)),
                    TableRow("Supports Fade-Out Analyzer", CheckboxOrHyphen(analyzer.SupportsFadeOutAnalyzer))));

            ReadOnlyCollection<SampleDescriptor> samples = analyzer.Samples;

            if (samples.Count > 0)
            {
                document.Add(
                    Heading2((samples.Count == 1) ? "Example" : "Examples"),
                    GetSamples(samples, Heading3("Code with Diagnostic"), Heading3("Code with Fix")));
            }

            document.Add(
                Heading2("How to Suppress"),
                Heading3("SuppressMessageAttribute"),
                FencedCodeBlock($"[assembly: SuppressMessage(\"{analyzer.Category}\", \"{analyzer.Id}:{analyzer.Title}\", Justification = \"<Pending>\")]", LanguageIdentifiers.CSharp),
                Heading3("#pragma"),
                FencedCodeBlock($"#pragma warning disable {analyzer.Id} // {analyzer.Title}\r\n#pragma warning restore {analyzer.Id} // {analyzer.Title}", LanguageIdentifiers.CSharp),
                Heading3("Ruleset"),
                ListItem(Link("How to configure rule set", "../HowToConfigureAnalyzers.md")));

            return document.ToString(format);
        }

        public static string CreateAnalyzersReadMe(IEnumerable<AnalyzerDescriptor> analyzers, IComparer<string> comparer)
        {
            var document = new MDocument(
                Heading2("Roslynator Analyzers"),
                Table(
                    TableHeader("Id", "Title", "Category", TableColumn("Enabled by Default", Alignment.Center)),
                    new Func<AnalyzerDescriptor, object>[]
                    {
                        f => f.Id,
                        f => Link(f.Title.TrimEnd('.'), $"../../docs/analyzers/{f.Id}.md"),
                        f => f.Category,
                        f => CheckboxOrEmpty(f.IsEnabledByDefault)
                    },
                    analyzers.OrderBy(f => f.Id, comparer)));

            return document.ToString();
        }

        public static string CreateRefactoringsReadMe(IEnumerable<RefactoringDescriptor> refactorings, IComparer<string> comparer)
        {
            var document = new MDocument(
                Heading2("Roslynator Refactorings"),
                Table(
                    TableHeader("Id", "Title", TableColumn("Enabled by Default", Alignment.Center)),
                    new Func<RefactoringDescriptor, object>[]
                    {
                        f => f.Id,
                        f => Link(f.Title.TrimEnd('.'), $"../../docs/refactorings/{f.Id}.md"),
                        f => CheckboxOrEmpty(f.IsEnabledByDefault)
                    },
                    refactorings.OrderBy(f => f.Title, comparer)));

            return document.ToString();
        }

        public static string CreateCodeFixesReadMe(IEnumerable<CodeFixDescriptor> codeFixes, IEnumerable<CompilerDiagnosticDescriptor> diagnostics, IComparer<string> comparer)
        {
            var document = new MDocument(
                Heading2("Roslynator Code Fixes"),
                Table(
                    TableHeader("Id", "Title", "Fixable Diagnostics", TableColumn("Enabled by Default", Alignment.Center)),
                    new Func<CodeFixDescriptor, object>[]
                    {
                        f => f.Id,
                        f => f.Title.TrimEnd('.'),
                        f => Join(", ", f.FixableDiagnosticIds.Join(diagnostics, x => x, y => y.Id, (x, y) => (object)LinkOrText(x, y.HelpUrl))),
                        f => CheckboxOrEmpty(f.IsEnabledByDefault)
                    },
                    codeFixes.OrderBy(f => f.Title, comparer)));

            return document.ToString();
        }

        public static string CreateCodeFixesByDiagnosticId(IEnumerable<CodeFixDescriptor> codeFixes, IEnumerable<CompilerDiagnosticDescriptor> diagnostics)
        {
            var document = new MDocument(
                Heading2("Roslynator Code Fixes by Diagnostic Id"),
                SimpleTable(
                    TableHeader("Diagnostic", "Code Fixes"),
                    GetRows()));

            return document.ToString();

            IEnumerable<TableRow> GetRows()
            {
                foreach (var grouping in codeFixes
                    .SelectMany(codeFix => codeFix.FixableDiagnosticIds.Select(diagnosticId => new { DiagnosticId = diagnosticId, CodeFixDescriptor = codeFix }))
                    .OrderBy(f => f.DiagnosticId)
                    .ThenBy(f => f.CodeFixDescriptor.Id)
                    .GroupBy(f => f.DiagnosticId))
                {
                    CompilerDiagnosticDescriptor diagnostic = diagnostics.FirstOrDefault(f => f.Id == grouping.Key);

                    yield return TableRow(
                        LinkOrText(diagnostic?.Id ?? grouping.Key, diagnostic?.HelpUrl),
                        Join(", ", grouping.Select(f => f.CodeFixDescriptor.Id)));
                }
            }
        }

        public static string CreateAnalyzersByCategoryMarkdown(IEnumerable<AnalyzerDescriptor> analyzers, IComparer<string> comparer)
        {
            var document = new MDocument(
                Heading2("Roslynator Analyzers by Category"),
                SimpleTable(
                    TableHeader("Category", "Title", "Id", TableColumn("Enabled by Default", Alignment.Center)),
                    GetRows()));

            return document.ToString();

            IEnumerable<TableRow> GetRows()
            {
                foreach (IGrouping<string, AnalyzerDescriptor> grouping in analyzers
                    .GroupBy(f => MarkdownEscaper.Escape(f.Category))
                    .OrderBy(f => f.Key, comparer))
                {
                    foreach (AnalyzerDescriptor analyzer in grouping.OrderBy(f => f.Title, comparer))
                    {
                        yield return TableRow(
                            grouping.Key,
                            Link(analyzer.Title.TrimEnd('.'), $"../../docs/analyzers/{analyzer.Id}.md"),
                            analyzer.Id,
                            CheckboxOrEmpty(analyzer.IsEnabledByDefault));
                    }
                }
            }
        }

        private static Image RefactoringImage(RefactoringDescriptor refactoring, string fileName)
        {
            return Image(refactoring.Title, $"../../images/refactorings/{fileName}.png");
        }

        private static IMarkdown CheckboxOrEmpty(bool value)
        {
            if (value)
            {
                return HtmlEntity(0x2713);
            }
            else
            {
                return RawText("");
            }
        }

        private static IMarkdown CheckboxOrHyphen(bool value)
        {
            if (value)
            {
                return HtmlEntity(0x2713);
            }
            else
            {
                return RawText("-");
            }
        }
    }
}
