using System.IO;
using System.Reflection;

namespace ToolQit.Tools
{
    /// <summary>
    /// Simple loader implementations.
    /// </summary>
    public static class Loader
    {
        public static Stream LoadEmbedManifestStream(string nsPath) => Assembly.GetExecutingAssembly().GetManifestResourceStream(nsPath) ?? Stream.Null;
    }
}