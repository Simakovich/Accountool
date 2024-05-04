using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace D9bolic.Generator.API.CRUD.Utils;

/// <summary>
/// Debug generator locker.
/// </summary>
public static class Logger
{
    private static List<string> Logs { get; set; } = new();

    /// <summary>
    /// Add new log entry.
    /// </summary>
    /// <param name="msg">Log message.</param>
    public static void WriteInfo(string msg) => Logs.Add($"/*--info--\t{msg}*/");

    /// <summary>
    /// Write log to the generation context.
    /// </summary>
    /// <param name="context">Generation context.</param>
    /// <param name="fileName">Logs file name.</param>
    public static SourceText PrintLogs() => SourceText.From(string.Join("\n", Logs), Encoding.UTF8);

    /// <summary>
    /// Reset logs.
    /// </summary>
    public static void Reset()
    {
        Logs = new();
    }
}