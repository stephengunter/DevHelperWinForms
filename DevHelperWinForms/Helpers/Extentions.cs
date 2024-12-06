namespace DevHelperWinForms;
public static class ExtensionHelpers
{
   public static void AddHelperClassLine(this ICollection<string> lines, string name)
   {
      lines.Add($"public static class {name}Helpers");
   }
   public static void AddHelperContent(this ICollection<string> lines, string name)
   {
      string viewModelName = $"{name}ViewModel"; 

      lines.Add($"public static {viewModelName} MapViewModel(this {name} entity, IMapper mapper)".StartWithTab(1));
      lines.Add($"=> mapper.Map<{viewModelName}>(entity);".StartWithTab(2));
      lines.Add("");

      lines.Add($"public static List<{viewModelName}> MapViewModelList(this IEnumerable<{name}> entities, IMapper mapper)".StartWithTab(1));
      lines.Add($"=> entities.Select(item => MapViewModel(item, mapper)).ToList();".StartWithTab(2));
      lines.Add("");

      lines.Add($"public static PagedList<{name}, {viewModelName}> GetPagedList(this IEnumerable<{name}> entities, IMapper mapper, int page = 1, int pageSize = 999)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add($"var pageList = new PagedList<{name}, {viewModelName}>(entities, page, pageSize);".StartWithTab(2));
      lines.Add($"pageList.SetViewList(pageList.List.MapViewModelList(mapper));".StartWithTab(2));
      lines.Add($"return pageList;".StartWithTab(2));

      lines.Add("}".StartWithTab(1));
      lines.Add("");
   }
}
