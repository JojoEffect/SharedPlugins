namespace Contracts
{
    using System;
    using System.Reflection;
    using System.Runtime.Loader;
    using System.Text;

    public static class ConsoleLog
    {
        public static void WriteAssemblyInformation(string comment = "", params Type[] types)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string separator = new string('-', 50);

            stringBuilder.AppendLine();
            stringBuilder.Append(separator);
            stringBuilder.AppendLine(separator);
            stringBuilder.Append(separator);
            stringBuilder.AppendLine(comment);
            stringBuilder.Append(separator);
            stringBuilder.AppendLine(separator);
            
            foreach (Type t in types)
            {
                Assembly assembly = t.Assembly;
                stringBuilder.AppendLine($"Type '{t.Name}' assembly check:");
                stringBuilder.AppendLine($"\tHash:\t{assembly.GetHashCode()}");
                stringBuilder.AppendLine($"\tPath:\t{assembly.Location}");
                stringBuilder.AppendLine($"\tName:\t{assembly.FullName}");
                stringBuilder.AppendLine($"\tALC:\t{AssemblyLoadContext.GetLoadContext(assembly).Name}");
                stringBuilder.Append(separator);
                stringBuilder.AppendLine(separator);
            }

            stringBuilder.Append(separator);
            stringBuilder.AppendLine(separator);

            Console.WriteLine(stringBuilder.ToString());
        }
    }
}