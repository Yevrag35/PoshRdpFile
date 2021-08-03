using System;
using System.Collections.Generic;
using System.IO;

namespace MG.Pwsh.Rdp.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static IEnumerable<string> TestPaths(this IEnumerable<string> paths)
        {
            foreach (string path in paths)
            {
                if (!Path.GetExtension(path).Equals(".rdp", StringComparison.CurrentCultureIgnoreCase))
                    throw new ArgumentException("The specified path must end in the extension '.rdp'");
                else
                    yield return path;
            }
        }
    }
}
