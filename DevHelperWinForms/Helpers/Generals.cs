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
         if (trimmedLine.StartsWith("["))
         {
            attributesBuffer.Add(trimmedLine);
            continue;
         }

         // Check for properties: must start with "public" and contain { get; set; } or =>
         if (trimmedLine.StartsWith("public") && (trimmedLine.Contains("{ get;") || trimmedLine.Contains("=>")))
         {
            properties.Add(new ClassProperty(trimmedLine, attributesBuffer.Any() ? new List<string>(attributesBuffer) : null));
            attributesBuffer.Clear();
         }
         // Check for methods: must start with "public" and contain "(" and ")"
         else if (trimmedLine.StartsWith("public") && trimmedLine.Contains("(") && trimmedLine.EndsWith(")"))
         {
            methods.Add(trimmedLine);
         }
      }

      return new ClassContent(properties, methods);
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
   public EditableProperty(string content, string label)
   {
      Content = content;
      Label = label;
   }
   public string Content { get; set; }
   public string Label { get; set; }
}
public class RecognizedProperty
{
   public string? Attribute { get; set; }
   public string Property { get; set; } = string.Empty;
}
