using <%= classify(name) %>.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace <%= classify(name) %>.Core.Logging;
public class LogProvider
{
    public string logDirectory;
    public string LogDirectory
    {
        get => logDirectory;
        set
        {
            logDirectory = value;
            LogDirectory.EnsureDirectoryExists();
        }
    }

    public static string GetLogName() => $"log-{DateTime.Now:yyyy-MM-dd-hh-mm-ss}.txt";

    public async Task CreateLog(HttpContext context, Exception exception, string subPath = "")
    {
        var builder = new StringBuilder();
        builder.AppendLine("ContextDetails");
        builder.AppendLine();
        builder.AppendLine(await context.GetContextDetails());
        builder.AppendLine("Exception Details");
        builder.AppendLine(exception.GetExceptionChain());

        var path = string.IsNullOrEmpty(subPath)
            ? LogDirectory
            : $@"{LogDirectory}\{subPath}";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        await builder.WriteLog($@"{path}\{GetLogName()}");
    }
}