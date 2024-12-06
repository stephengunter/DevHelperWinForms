namespace DevHelperWinForms;
public static class ServiceHelpers
{
   public static string AddServiceInterface(this ICollection<string> lines, string name)
   {
      string interfaceName = $"I{name}Service";
      lines.Add($"public interface {interfaceName}");
      lines.Add("{");
      lines.Add($"Task<IEnumerable<{name}>> FetchAsync();".StartWithTab(1));
      lines.Add($"Task<{name}?> GetByIdAsync(int id);".StartWithTab(1));
      lines.Add($"Task<{name}> CreateAsync({name} entity);".StartWithTab(1));
      lines.Add($"Task UpdateAsync({name} entity);".StartWithTab(1));
      lines.Add("}");
      return interfaceName;
   }


   public static void AddServiceClassLine(this ICollection<string> lines, string name, string interfaceName)
   {
      string line = $"public class {name}Service : {interfaceName}";
      lines.Add(line);
   }
   public static void AddServiceContent(this ICollection<string> lines, string name)
   {
      string repository = $"_{name.ToLower()}Repository";
      string init_repository = $"{name.ToLower()}Repository";
      lines.Add($"private readonly IDefaultRepository<{name}> {repository};".StartWithTab(1));
      lines.Add($"public {name}Service(IDefaultRepository<{name}> {init_repository})".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add($"{repository} = {init_repository};".StartWithTab(2));
      lines.Add("}".StartWithTab(1));


      lines.Add($"public async Task<IEnumerable<{name}>> FetchAsync()".StartWithTab(1));
      lines.Add($"=> await {repository}.ListAsync(new {name}Specification());".StartWithTab(2));
      lines.Add("");

      lines.Add($"public async Task<{name}?> GetByIdAsync(int id)".StartWithTab(1));
      lines.Add($"=> await {repository}.FirstOrDefaultAsync(new {name}Specification(id));".StartWithTab(2));
      lines.Add("");

      lines.Add($"public async Task<Host> CreateAsync({name} entity)".StartWithTab(1));
      lines.Add($"=> await {repository}.AddAsync(entity);".StartWithTab(2));
      lines.Add("");

      lines.Add($"public async Task UpdateAsync({name} entity)".StartWithTab(1));
      lines.Add($"=>  await {repository}.UpdateAsync(entity);".StartWithTab(2));
      lines.Add("");
   }
}
