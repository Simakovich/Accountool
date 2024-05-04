using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace D9bolic.Generator.API.CRUD.Utils;

/// <summary>
/// Different strings extensions.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Format code with default ruleset.
    /// </summary>
    /// <param name="sourceCode">Source code.</param>
    /// <returns>Formated source code.</returns>
    public static string FormatCode(this string sourceCode)
    {
        var tree = CSharpSyntaxTree.ParseText(sourceCode);
        var root = (CSharpSyntaxNode) tree.GetRoot();
        sourceCode = root.NormalizeWhitespace().ToFullString();

        return sourceCode;
    }

    /// <summary>
    /// Log generation phase exceptions during the generation.
    /// </summary>
    /// <param name="context">Generation context.</param>
    /// <param name="ex">Logged exception.</param>
    public static void LogExceptionWithStack(this GeneratorExecutionContext context, Exception ex)
    {
        LogException(context, ex);
        context.ReportDiagnostic(Diagnostic.Create(
            new DiagnosticDescriptor(
                "SI0000",
                ex.StackTrace,
                ex.StackTrace,
                "API.Client.Generator",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true),
            Location.None));
    }

    private static void LogException(GeneratorExecutionContext context, Exception ex)
    {
        while (true)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("SI0000", ex.Message, ex.Message, "API.Client.Generator",
                    DiagnosticSeverity.Error, isEnabledByDefault: true), Location.None));

            if (ex.InnerException != null)
            {
                ex = ex.InnerException;
                continue;
            }

            break;
        }
    }

    public static string TrimEnd(this string input, string suffixToRemove,
        StringComparison comparisonType = StringComparison.InvariantCulture)
    {
        if (suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
        {
            return input.Substring(0, input.Length - suffixToRemove.Length);
        }

        return input;
    }

    public static string EscapeFileName(this string fileName) => new[] {'<', '>', ','}
        .Aggregate(new StringBuilder(fileName), (s, c) => s.Replace(c, '_')).ToString();
}