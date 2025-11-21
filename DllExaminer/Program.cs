using System;
using System.Reflection;
using System.Linq;

var asm = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), "4DO.exe"));
Console.WriteLine($"Assembly: {asm.FullName}\n");

Console.WriteLine("=== Looking for FreeDO-related P/Invoke or types ===\n");

foreach (var type in asm.GetTypes().Where(t => t.Name.Contains("FreeDO") || t.Name.Contains("Core") || t.Name.Contains("Emulator")))
{
    Console.WriteLine($"\n** {type.FullName} **");

    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .Take(20);

    foreach (var method in methods)
    {
        if (method.GetCustomAttribute(typeof(System.Runtime.InteropServices.DllImportAttribute)) != null)
        {
            var dllImport = (System.Runtime.InteropServices.DllImportAttribute)method.GetCustomAttribute(typeof(System.Runtime.InteropServices.DllImportAttribute));
            Console.WriteLine($"  [DllImport(\"{dllImport.Value}\")] {method.ReturnType.Name} {method.Name}({string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"))})");
        }
        else
        {
            var parameters = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
            Console.WriteLine($"  {method.ReturnType.Name} {method.Name}({parameters})");
        }
    }
}
