namespace DevHelperWinForms;
public static class DtoMapperHelpers
{
   public static void AddDtoMapperClassLine(this ICollection<string> lines, string name)
   {
      string line = $"public class {name}MappingProfile : Profile";
      lines.Add(line);
   }
   public static void AddDtoMapperContent(this ICollection<string> lines, string name)
   {

      lines.Add($"public {name}Specification()".StartWithTab(1));


      lines.Add($"public {name}MappingProfile()".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add($"CreateMap<{name}, {name}ViewModel>();".StartWithTab(2));
      lines.Add($"CreateMap<{name}ViewModel, {name}>();".StartWithTab(2));
      lines.Add("}".StartWithTab(1));
   }
}
