using Humanizer;

namespace DevHelperWinForms;
public static class WebModelHelpers
{
   public static WebModelResult AddWebModelContent(this ICollection<string> lines, string name, List<EditableProperty> editableProperties)
   {
      string labelsName = AddWebModelLabels(lines, name, editableProperties); 
      string fetchRequestName = AddWebModelFetchRequest(lines, name);
      string modelIndexName = AddWebModelIndex(lines, name, labelsName, fetchRequestName);

      var result = AddWebModelForms(lines, name, editableProperties);
     
      result.LabelsName = labelsName;
      result.FetchRequestName = fetchRequestName;
      result.IndexModelName = modelIndexName;
      return result;
   }
   static string AddWebModelLabels(this ICollection<string> lines, string name, List<EditableProperty> editableProperties)
   {
      string className = $"{name}Labels";
      lines.Add($"public class {className}");
      lines.Add("{");
      foreach (var editableProperty in editableProperties)
      {
         lines.Add($"public string {editableProperty.Name} => \"{editableProperty.Label}\";".StartWithTab(1));
      }
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
   static string AddWebModelIndex(this ICollection<string> lines, string name, string labelsName, string fetchRequestName)
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
      return className;
   }
   static WebModelResult AddWebModelForms(ICollection<string> lines, string name, List<EditableProperty> editableProperties)
   {
      string baseFormName = $"Base{name}Form";
      lines.Add($"public abstract class {baseFormName}");
      lines.Add("{");
      foreach (var editableProperty in editableProperties)
      {
         lines.Add(editableProperty.Content.StartWithTab(1));
      }
      
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

      string addRequestName = $"{name}AddRequest";
      lines.Add("");
      lines.Add($"public class {addRequestName}");
      lines.Add("{");
      lines.Add($"public {addRequestName}({addFormName} form)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add("Form = form;".StartWithTab(2));
      lines.Add("}".StartWithTab(1));
      lines.Add($"public {addFormName} Form " + "{ get; set; }".StartWithTab(1));
      lines.Add("}");

      string editRequestName = $"{name}EditRequest";
      lines.Add("");
      lines.Add($"public class {editRequestName}");
      lines.Add("{");
      lines.Add($"public {editRequestName}({editFormName} form)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add("Form = form;".StartWithTab(2));
      lines.Add("}".StartWithTab(1));
      lines.Add($"public {editFormName} Form " + "{ get; set; }".StartWithTab(1));

      lines.Add("}");
      return new WebModelResult()
      {
         BaseFormName = baseFormName,
         AddFormName = addFormName,
         EditFormName = editFormName,
         AddRequestName = addRequestName,
         EditRequestName = editRequestName
      };
   }
   //static void AddEditForm(ICollection<string> lines, List<EditableProperty> editableProperties, string editFormName)
   //{
   //   lines.Add($"public class {requestName}");
   //   lines.Add("{");
   //   lines.Add($"public {requestName}({editFormName} form)".StartWithTab(1));
   //   lines.Add("{".StartWithTab(1));
   //   lines.Add("Form = form;".StartWithTab(2));
   //   lines.Add("}".StartWithTab(1));

   //   lines.Add($"public {editFormName} Form " + "{ get; set; }".StartWithTab(1));

   //   lines.Add("}");
   //}
   //static void AddEditRequest(ICollection<string> lines, List<EditableProperty> editableProperties, string requestName, string editFormName)
   //{
   //   lines.Add($"public class {requestName}");
   //   lines.Add("{");
   //   lines.Add($"public {requestName}({editFormName} form)".StartWithTab(1));
   //   lines.Add("{".StartWithTab(1));
   //   lines.Add("Form = form;".StartWithTab(2));
   //   lines.Add("}".StartWithTab(1));

   //   lines.Add($"public {editFormName} Form " + "{ get; set; }".StartWithTab(1));

   //   lines.Add("}");
   //}
}

public class WebModelResult
{
   public string LabelsName { get; set; }
   public string FetchRequestName { get; set; }
   public string IndexModelName { get; set; }
   public string BaseFormName { get; set; }
   public string AddFormName { get; set; }
   public string EditFormName { get; set; }
   public string AddRequestName { get; set; }
   public string EditRequestName { get; set; }
}
