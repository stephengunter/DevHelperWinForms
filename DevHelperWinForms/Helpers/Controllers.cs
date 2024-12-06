using Humanizer;

namespace DevHelperWinForms;
public static class ControllerHelpers
{
   public static string AddControllerClassLine(this ICollection<string> lines, string name)
   {
      string controllerName = $"{name.Pluralize()}Controller";
      lines.Add($"public class {controllerName} : BaseAdminController");
      return controllerName;
   }
   public static void AddControllerContent(this ICollection<string> lines, string name)
   {
      string service = $"_{name.ToLower()}Service";
      string init_service = $"{name.ToLower()}Service";
      lines.Add($"private readonly I{name}Service {service};".StartWithTab(1));
      lines.Add("private readonly IMapper _mapper;".StartWithTab(1));

      lines.Add($"public {name.Pluralize()}Controller(I{name}Service {init_service}, IMapper mapper)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add($"{service} = {init_service};".StartWithTab(2));
      lines.Add("_mapper = mapper;".StartWithTab(2));
      lines.Add("}".StartWithTab(1));

      lines.Add($"[HttpGet(\"init\")]".StartWithTab(1));
      lines.Add($"public async Task<IEnumerable<{name}>> FetchAsync()".StartWithTab(1));
      lines.Add($"=> await {service}.ListAsync(new {name}Specification());".StartWithTab(2));
      lines.Add("");

      //lines.Add($"public async Task<{name}?> GetByIdAsync(int id)".StartWithTab(1));
      //lines.Add($"=> await {service}.FirstOrDefaultAsync(new {name}Specification(id));".StartWithTab(2));
      //lines.Add("");

      //lines.Add($"public async Task<Host> CreateAsync({name} entity)".StartWithTab(1));
      //lines.Add($"=> await {service}.AddAsync(entity);".StartWithTab(2));
      //lines.Add("");

      //lines.Add($"public async Task UpdateAsync({name} entity)".StartWithTab(1));
      //lines.Add($"=>  await {service}.UpdateAsync(entity);".StartWithTab(2));
      //lines.Add("");
   }
}
