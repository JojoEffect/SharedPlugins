namespace Contracts
{
    using System;
    using System.Reflection;
    using System.Runtime.Loader;
    using System.Text;

    public static class ConsoleLog
    {
        static readonly string Separator = new string('-', 50);

        public static void WriteAssemblyInformation(string comment = "", params Type[] types)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine();
            stringBuilder.Append(Separator);
            stringBuilder.AppendLine(Separator);
            stringBuilder.Append(Separator);
            stringBuilder.AppendLine(comment);
            stringBuilder.Append(Separator);
            stringBuilder.AppendLine(Separator);

            foreach (Type t in types)
            {
                Assembly assembly = t.Assembly;
                stringBuilder.AppendLine($"Type '{t.Name}' assembly check:");
                stringBuilder.AppendLine($"\tHash:\t{assembly.GetHashCode()}");
                stringBuilder.AppendLine($"\tPath:\t{assembly.Location}");
                stringBuilder.AppendLine($"\tName:\t{assembly.FullName}");
                stringBuilder.AppendLine($"\tALC:\t{AssemblyLoadContext.GetLoadContext(assembly).Name}");
                stringBuilder.Append(Separator);
                stringBuilder.AppendLine(Separator);
            }

            stringBuilder.Append(Separator);
            stringBuilder.AppendLine(Separator);

            Console.WriteLine(stringBuilder.ToString());
        }

        public static void WriteResolvingEvent(AssemblyLoadContext assemblyLoadContext, AssemblyName assemblyName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine();
            stringBuilder.Append(Separator);
            stringBuilder.AppendLine(Separator);
            stringBuilder.Append(Separator);
            stringBuilder.AppendLine("Resolving event");
            stringBuilder.Append(Separator);
            stringBuilder.AppendLine(Separator);

            stringBuilder.AppendLine($"\tName:\t{assemblyName.FullName}");
            stringBuilder.AppendLine($"\tALC:\t{assemblyLoadContext.Name}");

            stringBuilder.Append(Separator);
            stringBuilder.AppendLine(Separator);
            stringBuilder.Append(Separator);
            stringBuilder.AppendLine(Separator);

            Console.WriteLine(stringBuilder.ToString());
        }

        public static void WriteLoadContextLoadCall(AssemblyLoadContext loadContext, AssemblyName assemblyName)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine();
            stringBuilder.Append(Separator);
            stringBuilder.AppendLine(Separator);
            stringBuilder.Append(Separator);
            stringBuilder.AppendLine($"{loadContext.Name} Load({assemblyName.Name})");
            stringBuilder.Append(Separator);
            stringBuilder.AppendLine(Separator);

            Console.WriteLine(stringBuilder.ToString());
        }
    }
}