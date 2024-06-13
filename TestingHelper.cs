using OpenLobby.Utility.Utils;
using System.Text;

namespace OpenLobby.Utility.Tests;

[TestFixture]
public class TestingHelper
{
    const bool CREATE = false;
    const string PATH = "C:\\Users\\Shiv\\source\\repos\\OpenLobby.Utility.Tests\\";

    [Test]
    [Ignore("Don't Create")]
    [TestCase(typeof(Helper))]
    public void GenerateTemplate(Type type)
    {
        if (!CREATE) return;

#pragma warning disable CS0162 // Unreachable code detected
        string className = type.Name + "Tests";
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("namespace OpenLobby.Utility.Tests;");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("[TestFixture]");
        stringBuilder.AppendLine($"internal class {className}");
        stringBuilder.AppendLine("{");
        foreach (var method in type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
        {
            stringBuilder.AppendLine($"    [Test]");
            stringBuilder.AppendLine($"    [Ignore(\"Not Implemented\")]");
            stringBuilder.AppendLine($"    public void {method.Name}_Correct()");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("        Assert.Fail(\"Not Implemented\");");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine();
        }
        stringBuilder.AppendLine("}");

        // Write the generated test methods to the file
        string file = PATH + className + ".cs";
        string code = stringBuilder.ToString();
        File.WriteAllText(file, code);
        Console.WriteLine($"{className} template has been written");
#pragma warning restore CS0162 // Unreachable code detected
    }
}