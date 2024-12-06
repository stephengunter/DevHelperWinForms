namespace DevHelperWinForms;
public static class SpecificationHelpers
{
   public static void AddSpecificationClassLine(this ICollection<string> lines, string name)
   {
      string line = $"public class {name}Specification : Specification<{name}>";
      lines.Add(line);
   }
   public static void AddSpecificationContent(this ICollection<string> lines, string name)
   {
      lines.Add($"public {name}Specification()".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add("Query.Where(item => !item.Removed);".StartWithTab(2));
      lines.Add("}".StartWithTab(1));
   }
}
