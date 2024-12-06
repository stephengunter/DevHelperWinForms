using Humanizer;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Ude;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DevHelperWinForms;
public partial class Form1 : Form
{

   private readonly AppSettings _appSettings;
   public Form1(IOptions<AppSettings> appSettings)
   {
      _appSettings = appSettings.Value;

      InitializeComponent();
      txt_template.Enabled = false;

      txt_root.Text = _appSettings.RootPath;
      txt_core.Text = _appSettings.CorePath;
      txt_web.Text = _appSettings.WebPath;

      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
   }

   string RootPath => txt_root.Text;
   string CorePath => Path.Combine(RootPath, txt_core.Text);
   string WebPath => Path.Combine(RootPath, txt_web.Text);

   ICollection<string> ModelNames => txt_name.Text.Split(".");
   string GetModelName() => ModelNames.Last().Titleize();

   string TemplatePath => txt_template.Text;
   void SetTemplatePath(string folder) 
   {
      string path = Path.Combine(CorePath, folder);
      foreach (var part in ModelNames)
      {
         path = Path.Combine(path, part);
      }
      txt_template.Text = $"{path}.cs"; ;
   }
   List<string> ReadSourcceLines() => File.ReadAllLines(TemplatePath, Encoding.GetEncoding(1252)).ToList();

   List<EditableProperty> AddViewModel()
   {
      string name = GetModelName();
      var sourceLines = ReadSourcceLines();
      var classLine = sourceLines.GetClassLine();

      string? baseClass = classLine.GetBaseClass();
      var interfaces = classLine.GetImplementedInterfaces();

      var lines = new List<string>();
      lines.Add("using Infrastructure.Views;");
      lines.Add("");
      lines.AddNamespace("ApplicationCore.Views", ModelNames);
      lines.AddViewClassLine(name, baseClass, interfaces);

      lines.Add("{");
      //props
      var props = sourceLines.GetPropLines();
      var ClassContent = props.ParseLinesForPropsAndMethods();
      var editableProps = new List<EditableProperty>();
      foreach (ClassProperty prop in ClassContent.Properties)
      {
         
         if (prop.Attributes == null)
         {
            lines.Add(prop.Content.StartWithTab(1));
         }
         else 
         {
            lines.Add(prop.Content.StartWithTab(1));
            foreach (var attribute in prop.Attributes)
            {
               if (attribute.TrimStart().StartsWith("[Editor"))
               {
                  // Extract the attribute details
                  var labelMatch = System.Text.RegularExpressions.Regex.Match(attribute, @"\(\""(.*?)\""");
                  var enableMatch = System.Text.RegularExpressions.Regex.Match(attribute, @"Enable\s*=\s*(\w+)");

                  var label = labelMatch.Success ? labelMatch.Groups[1].Value : "Unknown";
                  var enable = enableMatch.Success ? enableMatch.Groups[1].Value : "False";
                  editableProps.Add(new EditableProperty(prop.Content, label));
                  break;
               }
            }
         }
      }

      lines.AddViewImplements(interfaces);
      lines.Add("}");

      string path = Path.Combine(CorePath, "Views");
      foreach (var part in ModelNames)
      {
         path = Path.Combine(path, part);
      }
      string fileName = $"{path}.cs";

      fileName = SaveLinesToFile(fileName, lines);
      AddMessage($"{fileName} Created.");
      return editableProps;
   }
   void AddWebModel(List<EditableProperty> editableProperties)
   {
      string name = GetModelName();

      var lines = new List<string>();
      lines.Add("");
      lines.AddNamespace("Web.Models", ModelNames);
      lines.AddWebModelContent(name);

      string path = Path.Combine(WebPath, "Models");
      foreach (var part in ModelNames)
      {
         path = Path.Combine(path, part);
      }
      string fileName = $"{path}.cs";

      fileName = SaveLinesToFile(fileName, lines);
      AddMessage($"{fileName} Created.");
   }
   void AddDtoMapper()
   {
      string name = GetModelName();

      var lines = new List<string>();
      lines.AddUsing("ApplicationCore.Models", ModelNames);
      lines.AddUsing("ApplicationCore.Views", ModelNames);
      lines.Add("using AutoMapper;");
      lines.Add("");
      lines.AddNamespace("ApplicationCore.DtoMapper", ModelNames);
      lines.AddDtoMapperClassLine(name);

      lines.Add("{");
      lines.AddDtoMapperContent(name);
      lines.Add("}");

      string path = Path.Combine(CorePath, "DtoMapper");
      foreach (var part in ModelNames)
      {
         path = Path.Combine(path, part);
      }
      string fileName = $"{path}.cs";

      fileName = SaveLinesToFile(fileName, lines);
      AddMessage($"{fileName} Created.");
   }


   void AddSpecification()
   {
      string name = GetModelName();

      var lines = new List<string>();
      lines.AddUsing("ApplicationCore.Models", ModelNames);
      lines.AddUsing("Ardalis.Specification", new List<string>());
      lines.Add("");
      lines.AddNamespace("ApplicationCore.Specifications", ModelNames);
      lines.AddSpecificationClassLine(name);

      lines.Add("{");
      lines.AddSpecificationContent(name);
      lines.Add("}");

      string path = Path.Combine(CorePath, "Specifications");
      foreach (var part in ModelNames)
      {
         path = Path.Combine(path, part);
      }
      string fileName = $"{path}.cs";

      fileName = SaveLinesToFile(fileName, lines);
      AddMessage($"{fileName} Created.");
   }
   void AddService()
   {
      string name = GetModelName();

      var lines = new List<string>();
      lines.AddUsing("ApplicationCore.Models", ModelNames);
      lines.AddUsing("ApplicationCore.Specifications", ModelNames);
      lines.AddUsing("ApplicationCore.DataAccess", new List<string>());

      lines.Add("");
      lines.AddNamespace("ApplicationCore.Services", ModelNames);

      string interFaceName = lines.AddServiceInterface(name);

      lines.AddServiceClassLine(name, interFaceName);

      lines.Add("{");
      lines.AddServiceContent(name);
      lines.Add("}");

      string path = Path.Combine(CorePath, "Services");
      foreach (var part in ModelNames)
      {
         path = Path.Combine(path, part);
      }
      string fileName = $"{path}.cs";

      fileName = SaveLinesToFile(fileName, lines);
      AddMessage($"{fileName} Created.");
   }
   void AddHelper()
   {
      string name = GetModelName();

      var lines = new List<string>();
      lines.AddUsing("ApplicationCore.Models", ModelNames);
      lines.AddUsing("ApplicationCore.Views", ModelNames);
      lines.AddUsing("AutoMapper", new List<string>());
      lines.AddUsing("Infrastructure.Paging", new List<string>());
      lines.AddUsing("Infrastructure.Helpers", new List<string>());
      lines.Add("");

      lines.AddNamespace("ApplicationCore.Helpers", ModelNames);
      lines.AddHelperClassLine(name);

      lines.Add("{");
      lines.AddHelperContent(name);
      lines.Add("}");

      string path = Path.Combine(CorePath, "Helpers");
      foreach (var part in ModelNames)
      {
         path = Path.Combine(path, part);
      }
      string fileName = $"{path}.cs";

      fileName = SaveLinesToFile(fileName, lines);
      AddMessage($"{fileName} Created.");
   }
   void AddController()
   {
      string name = GetModelName();

      var lines = new List<string>();
      lines.AddUsing("ApplicationCore.Services", ModelNames);
      lines.AddUsing("ApplicationCore.Views", ModelNames);
      lines.AddUsing("ApplicationCore.Helpers", ModelNames);
      lines.AddUsing("AutoMapper", new List<string>());
      lines.Add("");

      lines.AddNamespace("Web.Controllers.Admin", ModelNames);
      string controllerName = lines.AddControllerClassLine(name);

      lines.Add("{");
      lines.AddControllerContent(name);
      lines.Add("}");

      string path = Path.Combine(WebPath, "Controllers", "Admin");
      for (int i = 0; i < ModelNames.Count - 1; i++)
      {
         path = Path.Combine(path, ModelNames.ToList()[i]);
      }
      string fileName = Path.Combine(path, $"{controllerName}.cs");

      fileName = SaveLinesToFile(fileName, lines);
      AddMessage($"{fileName} Created.");
   }
   private void Form1_Load(object sender, EventArgs e)
   {
      this.ActiveControl = txt_name;
   }

   string ErrMsg()
   {
      if (string.IsNullOrEmpty(txt_name.Text)) return "¥²¶·¶ñ¼gName";

      SetTemplatePath("Models");

      if (!File.Exists(TemplatePath)) return "Template File Not Found.";

      return "";
   }

   private void btn_submit_Click(object sender, EventArgs e)
   {
      ClearMessage();
      string message = ErrMsg();
      if (!string.IsNullOrEmpty(message))
      {
         AddMessage(message, true);
         return;
      }



      var editableProperties = AddViewModel();
      //AddDtoMapper();
      AddWebModel(editableProperties);
      //AddSpecification();
      //AddService();
      //AddHelper();
      //AddController();

   }
   
   private void ClearMessage()
   {
      chatPanel.Controls.Clear();
   }
   private void AddMessage(string message, bool error = false)
   {
      if (error) AddMessage(message, Color.Red, Color.White);
      else AddMessage(message, Color.LightBlue, Color.Black);
   }
   private void AddMessage(string message, Color backColor, Color textColor)
   {
      // Create a new label for the message
      Label messageLabel = new Label
      {
         AutoSize = true,
         MaximumSize = new Size(350, 0), // Limit width, auto-wrap text
         Text = message,
         Font = new Font("Arial", 10),
         BackColor = backColor,
         ForeColor = textColor,
         Padding = new Padding(5),
         Margin = new Padding(5)
      };

      // Add the label to the FlowLayoutPanel
      chatPanel.Controls.Add(messageLabel);
      chatPanel.ScrollControlIntoView(messageLabel); // Scroll to the latest message
   }

   string ReplaceTemplate(string content, string templateName, string name)
   {
      return content.Replace(templateName, name) //article, name
                        .Replace(templateName.Titleize(), name.Titleize())  //Article, Name
                        .Replace(templateName.Pluralize(), name.Pluralize())  //articles, names
                        .Replace(templateName.Pluralize().Titleize(), name.Pluralize().Titleize())  //Articles, Names
                        .Replace(templateName.ToUpper(), name.ToUpper()) //ARTICLE, NAME         
                        .Replace(templateName.Pluralize().ToUpper(), name.Pluralize().ToUpper()); //ARTICLES, NAMES
   }
   private string SaveLinesToFile(string filePath, ICollection<string> lines)
   {
      string directory = Path.GetDirectoryName(filePath) ?? Directory.GetCurrentDirectory();
      string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
      string extension = Path.GetExtension(filePath);

      string newFilePath;
      int counter = 1;

      // Ensure the directory exists
      Directory.CreateDirectory(directory);

      do
      {
         // Create a new file name with a counter if the file exists
         newFilePath = Path.Combine(directory, $"{fileNameWithoutExtension}_{counter}{extension}");
         counter++;
      } while (File.Exists(newFilePath));

      // Write to the new file
      using (StreamWriter writer = new StreamWriter(newFilePath, false, Encoding.GetEncoding(1252)))
      {
         foreach (var line in lines)
         {
            if (string.IsNullOrEmpty(line)) writer.WriteLine();
            else writer.WriteLine(line);
         } 
      }
      return newFilePath;
   }


}