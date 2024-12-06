using System.Xml.Linq;

namespace DevHelperWinForms;
public static class ViewHelpers
{
   public static void AddViewClassLine(this ICollection<string> lines, string name, string baseClass, ICollection<string> interfaces)
   {
      string line = $"public class {name}ViewModel";
      if (!string.IsNullOrEmpty(baseClass))
      {
         if (baseClass == "EntityBase") line += " : EntityBaseView";
      }
      if (interfaces.Count > 0)
      {
         foreach (string item in interfaces) 
         {
            if (item == "IBaseRecord") line += ", IBaseRecordView";
         }
      }
      lines.Add(line);
   }
   public static void AddViewImplements(this ICollection<string> lines, ICollection<string> interfaces)
   {
      foreach (string item in interfaces)
      {
         if (item == "IBaseRecord")
         {
            lines.Add("public string CreatedAtText => CreatedAt.ToDateTimeString();".StartWithTab(1));
            lines.Add("public string LastUpdatedText => LastUpdated.ToDateTimeString();".StartWithTab(1));
         }
      }
   }
}
