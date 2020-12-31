using System;
using System.IO;
using System.Reflection;

namespace Luizanac.Infra.Http
{
    public static class HttpUtils
    {
        public static string ExtractControllerName(this string path)
        {
            _ = path ?? throw new ArgumentNullException(nameof(path));

            var controllerNamePosition = 0;
            var defaultControllerName = "Home";
            var splitedPath = path.Split("/", StringSplitOptions.RemoveEmptyEntries);

            if (controllerNamePosition + 1 > splitedPath.Length || string.IsNullOrEmpty(splitedPath[controllerNamePosition]))
                return defaultControllerName;

            return splitedPath[controllerNamePosition];
        }

        public static string ExtractActionName(this string path)
        {
            _ = path ?? throw new ArgumentNullException(nameof(path));

            var actionNamePosition = 1;
            var defaultActionName = "Index";
            var splitedPath = path.Split("/", StringSplitOptions.RemoveEmptyEntries);

            if (actionNamePosition + 1 > splitedPath.Length || string.IsNullOrEmpty(splitedPath[actionNamePosition]))
                return defaultActionName;

            return splitedPath[actionNamePosition];
        }

        public static bool IsStaticResource(this string path) =>
            !string.IsNullOrEmpty(Path.GetExtension(path));


        public static string PathToAssemblyNamespace(this string path)
        {
            _ = path ?? throw new ArgumentNullException(nameof(path));

            var assemblyPrefix = Assembly.GetCallingAssembly().GetName().Name;
            return $"{assemblyPrefix}{path.Replace('/', '.')}";
        }

        public static string ResolveMimeType(this string path)
        {
            _ = path ?? throw new ArgumentNullException(nameof(path));
            var extension = Path.GetExtension(path).Replace(".", "");
            return extension switch
            {
                SupportedMimeTypes.Css => "text/css",
                SupportedMimeTypes.Javascript => "application/js",
                SupportedMimeTypes.Html => "text/html",
                _ => "text/plain"
            };
        }

        public static string ResolveContentType(this string path) => $"{path.ResolveMimeType()}; charset=utf-8";
    }

    public static class SupportedMimeTypes
    {
        public const string Css = "css";
        public const string Javascript = "js";
        public const string Html = "html";
    }
}