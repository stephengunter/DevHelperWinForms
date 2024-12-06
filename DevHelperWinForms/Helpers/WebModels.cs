using Humanizer;
using System.Xml.Linq;

namespace DevHelperWinForms;
public static class WebModelHelpers
{
   static string AddWebModelLabels(this ICollection<string> lines, string name)
   {
      string className = $"{name}Labels";
      lines.Add($"public class {className}");
      lines.Add("{");
      lines.Add($"public string Title => \"名稱\";".StartWithTab(1));
      lines.Add("}");
      return className;
   }
   static string AddWebModelFetchRequest(this ICollection<string> lines, string name)
   {
      string className = $"{name.Pluralize()}FetchRequest";
      lines.Add($"public class {className}");
      lines.Add("{");
      
      lines.Add($"public {className}(bool active)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add("Active = active;".StartWithTab(2));
      lines.Add("}".StartWithTab(1));

      lines.Add("public bool Active { get; set; }".StartWithTab(1));

      lines.Add("}");
      return className;
   }
   static void AddWebModelIndex(this ICollection<string> lines, string name, string labelsName, string fetchRequestName)
   {
      string className = $"{name.Pluralize()}IndexModel";
      lines.Add($"public class {className}");
      lines.Add("{");
      lines.Add($"public {className}({fetchRequestName} request)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add("Request = request;".StartWithTab(2));
      lines.Add("}".StartWithTab(1));

      string input = $"public {fetchRequestName} Request " + "{ get; set; }";
      lines.Add(input.StartWithTab(1));

      lines.Add("}");
   }
   public static void AddWebModelContent(this ICollection<string> lines, string name)
   {
      string labelsName = AddWebModelLabels(lines, name); 
      string fetchRequestName = AddWebModelFetchRequest(lines, name);
      AddWebModelIndex(lines, name, labelsName, fetchRequestName);
      AddWebModelForms(lines, name);
   }

   static void AddWebModelForms(ICollection<string> lines, string name)
   {
      string baseFormName = $"Base{name}Form";
      lines.Add($"public abstract class {baseFormName}");
      lines.Add("{");
      lines.Add("public string Title { get; set; } = String.Empty;".StartWithTab(1));
      lines.Add("}");

      string addFormName = $"{name}AddForm";
      lines.Add("");
      lines.Add($"public class {addFormName} : {baseFormName}");
      lines.Add("{");
      lines.Add("");
      lines.Add("}");

      string editFormName = $"{name}EditForm";
      lines.Add("");
      lines.Add($"public class {editFormName} : {baseFormName}");
      lines.Add("{");
      lines.Add("");
      lines.Add("}");

      lines.Add("");
      lines.Add($"public class {name}AddRequest");
      lines.Add("{");
      lines.Add($"public {name}AddRequest({addFormName} form)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add("Form = form;".StartWithTab(2));
      lines.Add("}".StartWithTab(1));

      string input = $"public {addFormName} Form " + "{ get; set; }";
      lines.Add(input.StartWithTab(1));

      lines.Add("}");

      lines.Add("");
      lines.Add($"public class {name}EditRequest");
      lines.Add("{");
      lines.Add($"public {name}EditRequest({editFormName} form)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add("Form = form;".StartWithTab(2));
      lines.Add("}".StartWithTab(1));

      input = $"public {editFormName} Form " + "{ get; set; }";
      lines.Add(input.StartWithTab(1));

      lines.Add("}");
   }
}
