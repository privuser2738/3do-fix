using System;
using System.Reflection;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            var dll = Assembly.LoadFile(@"C:\Users\rob\Games\3DO\4DO\FreeDOCore.dll");
            Console.WriteLine($"Assembly: {dll.FullName}");
            Console.WriteLine("\n=== Public Types ===");

            foreach (var type in dll.GetExportedTypes().Take(20))
            {
                Console.WriteLine($"\nType: {type.FullName}");

                // Show public methods
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                    .Where(m => m.DeclaringType == type)
                    .Take(10);

                foreach (var method in methods)
                {
                    var parameters = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                    Console.WriteLine($"  {method.ReturnType.Name} {method.Name}({parameters})");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}
