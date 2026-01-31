using System.Reflection;

namespace MkDocsSharp.MDGen.Test
{
    internal static class TestUtil
    {
        internal static string FetchResourceAsString(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using Stream? stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new InvalidOperationException($"Resource '{resourceName}' not found.");
            }
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
