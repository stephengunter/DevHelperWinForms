using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DevHelperWinForms;

public static class GeneralHelpers
{
   public static string? GetClassLine(this ICollection<string> lines)
   {
      var regex = new Regex(@"^\s*public\s+class", RegexOptions.IgnoreCase);
      return lines.FirstOrDefault(line => regex.IsMatch(line));
   }
   public static string? GetBaseClass(this string classLine)
   {
      // Check if the line contains inheritance (a colon ':')
      int colonIndex = classLine.IndexOf(':');
      if (colonIndex == -1) return null; // No inheritance found

      // Extract the part after the colon
      string inheritancePart = classLine.Substring(colonIndex + 1).Trim();

      // Split the inheritance list by commas
      string[] inheritanceList = inheritancePart.Split(',');

      // Return the first item as the base class
      return inheritanceList.Length > 0 ? inheritanceList[0].Trim() : null;
   }
   public static List<string> GetImplementedInterfaces(this string classLine)
   {
      var interfaces = new List<string>();

      // Check if the line contains inheritance (a colon ':')
      int colonIndex = classLine.IndexOf(':');
      if (colonIndex == -1) return interfaces; // No inheritance found

      // Extract the part after the colon
      string inheritancePart = classLine.Substring(colonIndex + 1).Trim();

      // Split the inheritance list by commas
      string[] inheritanceList = inheritancePart.Split(',');

      // Loop through each item, skipping the first (base class)
      for (int i = 1; i < inheritanceList.Length; i++)
      {
         interfaces.Add(inheritanceList[i].Trim());
      }

      return interfaces;
   }
   public static List<string> GetPropLines(this IEnumerable<string> lines)
   {
      var result = new List<string>();
      bool insideBraces = false;
      int braceCount = 0;

      foreach (var line in lines)
      {
         if (line.Contains("{"))
         {
            if (!insideBraces)
            {
               insideBraces = true;
            }
            braceCount++;
         }

         if (insideBraces)
         {
            result.Add(line);
         }

         if (line.Contains("}"))
         {
            braceCount--;
            if (braceCount == 0)
            {
               insideBraces = false;
               break;
            }
         }
      }

      // Remove the first line (opening brace) and the last line (closing brace)
      if (result.Count > 1)
      {
         result.RemoveAt(0); // Remove the opening brace
         result.RemoveAt(result.Count - 1); // Remove the closing brace
      }

      return result;
   }
   public static void AddUsing(this ICollection<string> lines, string ns, ICollection<string> names)
   {
      for (int i = 0; i < names.Count - 1; i++)
      {
         ns += $".{names.ToList()[i]}";
      }

      lines.Add($"using {ns};");
   }
   public static void AddNamespace(this ICollection<string> lines, string ns, ICollection<string> names)
   {
      for (int i = 0; i < names.Count - 1; i++)
      {
         ns += $".{names.ToList()[i]}";
      }

      lines.Add($"namespace {ns};");
   }
   public static string StartWithTab(this string input, int tabs)
   {
      string tab = new string(' ', 3); // Define a "tab" as 3 spaces
      string tabsPrefix = new string(' ', tabs * 3); // Generate the prefix
      return tabsPrefix + input; // Prepend the tabs to the input
   }
   public static List<RecognizedProperty> RecognizePropertiesWithAttributes(this List<string> lines)
   {
      var result = new List<RecognizedProperty>();
      string? currentAttribute = null;

      foreach (var line in lines)
      {
         if (line.TrimStart().StartsWith("["))
         {
            // Capture the attribute
            currentAttribute = line.Trim();
         }
         else if (line.TrimStart().StartsWith("public"))
         {
            // Capture the property associated with the last attribute
            result.Add(new RecognizedProperty
            {
               Attribute = currentAttribute,
               Property = line.Trim()
            });
            currentAttribute = null; // Reset for next property
         }
      }

      return result;
   }
   public static ClassContent ParseLinesForPropsAndMethods(this List<string> lines)
   {
      var properties = new List<ClassProperty>();
      var methods = new List<string>();
      var attributesBuffer = new List<string>();

      foreach (var line in lines)
      {
         var trimmedLine = line.Trim();

         // Collect attributes (lines starting with [)
         if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
         {
            attributesBuffer.Add(trimmedLine);
            continue;
         }

         // Check for properties: must start with "public" and contain { get; set; } or =>
         if (IsPropertyLine(trimmedLine))
         {
            // Extract the name and content from the property line
            var propertyName = ExtractPropertyName(trimmedLine);
            var propertyContent = trimmedLine; // Full line as content

            properties.Add(new ClassProperty(
                propertyName,
                propertyContent,
                attributesBuffer.Any() ? new List<string>(attributesBuffer) : null
            ));
            attributesBuffer.Clear();
         }
         // Check for methods: must start with "public" and contain "(" and ")"
         else if (IsMethodLine(trimmedLine))
         {
            methods.Add(trimmedLine);
            attributesBuffer.Clear();
         }
      }

      return new ClassContent(properties, methods);
   }

   // Helper to determine if a line represents a property
   private static bool IsPropertyLine(string line)
   {
      return line.StartsWith("public") && (line.Contains("{ get;") || line.Contains("=>"));
   }

   // Helper to determine if a line represents a method
   private static bool IsMethodLine(string line)
   {
      return line.StartsWith("public") && line.Contains("(") && line.Contains(")") && !line.Contains("{ get; }");
   }
   private static string ExtractPropertyName(string line)
   {
      // Split the line by whitespace to get tokens
      var tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

      // The property name is usually the third token in a "public Type Name" pattern
      // Ensure that tokens[2] exists and is not a keyword
      if (tokens.Length > 2 && !tokens[2].Contains("{") && !tokens[2].Contains("=>"))
      {
         return tokens[2];
      }

      // Fallback: Attempt to locate the property name in a more generic way
      var match = Regex.Match(line, @"\bpublic\s+\w+\s+(\w+)\s*[{=>]");
      return match.Success ? match.Groups[1].Value : "Unknown";
   }
   public static string? FindEditorAttribute(this ClassProperty prop)
   {
      if (prop.Attributes == null) return null;
      if (prop.Attributes.Count == 0) return null;
      return prop.Attributes.FirstOrDefault(attribute => attribute.TrimStart().StartsWith("[Editor"));
   }

   //public static void AddClass(this ICollection<string> lines, string name, ICollection<string> names)
   //{
   //   for (int i = 0; i < names.Count - 1; i++)
   //   {
   //      ns += $".{names.ToList()[i]}";
   //   }

   //   lines.Add($"public class {ns};");
   //}
}

public class ClassContent
{
   public ClassContent(List<ClassProperty> properties, List<string> methods)
   {
      Properties = properties;
      Methods = methods;
   }
   public List<ClassProperty> Properties { get; set; }
   public List<string> Methods { get; set; }
}
public class ClassProperty
{
   public ClassProperty(string name, string content, List<string>? attributes = null)
   {
      Name = name;
      Content = content;
      Attributes = attributes;
   }
   public List<string>? Attributes { get; set; }
   public string Name { get; set; }
   public string Content { get; set; }
}
public class EditableProperty
{
   public EditableProperty(string name, string content, string label)
   {
      Name = name;
      Content = content;
      Label = label;
   }
   public string Name { get; set; }
   public string Content { get; set; }
   public string Label { get; set; }
}
public class RecognizedProperty
{
   public string? Attribute { get; set; }
   public string Property { get; set; } = string.Empty;
}
