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
   public static void AddControllerContent(this ICollection<string> lines, string name, WebModelResult result)
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

      // init
      lines.Add($"[HttpGet(\"init\")]".StartWithTab(1));
      lines.Add($"public async Task<ActionResult<{result.IndexModelName}>> Init()".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add("bool active = true;".StartWithTab(2));
      lines.Add($"var request = new {result.FetchRequestName}(active);".StartWithTab(2));
      lines.Add($"return new {result.IndexModelName}(request);".StartWithTab(2));

      lines.Add("}".StartWithTab(1));
      lines.Add("");

      // index
      lines.Add($"[HttpGet]".StartWithTab(1));
      lines.Add($"public async Task<ActionResult<ICollection<{name}ViewModel>>> Index(bool active)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add($"var list = await {service}.FetchAsync();".StartWithTab(2));
      lines.Add($"return list.MapViewModelList(_mapper);".StartWithTab(2));

      lines.Add("}".StartWithTab(1));
      lines.Add("");

      // create
      lines.Add($"[HttpGet(\"create\")]".StartWithTab(1));
      lines.Add($"public async Task<ActionResult<{result.AddRequestName}>> Index()".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add($"var form = new SystemAppAddForm();".StartWithTab(2));
      lines.Add($"return new {result.AddRequestName}(form);".StartWithTab(2));

      lines.Add("}".StartWithTab(1));
      lines.Add("");

      // store
      lines.Add($"[HttpPost]".StartWithTab(1));
      lines.Add($"public async Task<ActionResult<{name}ViewModel>> Store([FromBody] {result.AddFormName} form)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add($"await ValidateRequestAsync(form, 0);".StartWithTab(2));
      lines.Add($"if (!ModelState.IsValid) return BadRequest(ModelState);".StartWithTab(2));
      lines.Add("");
      lines.Add($"var entity = new {name}();".StartWithTab(2));
      lines.Add($"form.SetValuesTo(entity);".StartWithTab(2));
      lines.Add($"entity.SetCreated(User.Id());".StartWithTab(2));
      lines.Add("");
      lines.Add($"entity = await {service}.CreateAsync(entity, User.Id());".StartWithTab(2));
      lines.Add($"return entity.MapViewModel(_mapper);".StartWithTab(2));

      lines.Add("}".StartWithTab(1));
      lines.Add("");

      // edit
      lines.Add("[HttpGet(\"edit/{id}\")]".StartWithTab(1));
      lines.Add($"public async Task<ActionResult<{result.EditRequestName}>> Edit(int id)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add($"var entity = await {service}.GetByIdAsync(id);".StartWithTab(2));
      lines.Add($"if (entity == null) return NotFound();".StartWithTab(2));
      lines.Add("");
      lines.Add($"var form = new {result.EditFormName}();".StartWithTab(2));
      lines.Add($"entity.SetValuesTo(form);".StartWithTab(2));
      lines.Add($"return new {result.EditRequestName}(form);".StartWithTab(2));

      lines.Add("}".StartWithTab(1));
      lines.Add("");

      // update
      lines.Add("[HttpPut(\"{id}\")]".StartWithTab(1));
      lines.Add($"public async Task<ActionResult> Update(int id, [FromBody] {result.EditFormName} form)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add($"var entity = await {service}.GetByIdAsync(id);".StartWithTab(2));
      lines.Add($"if (entity == null) return NotFound();".StartWithTab(2));
      lines.Add("");
      lines.Add($"await ValidateRequestAsync(form, id);".StartWithTab(2));
      lines.Add($"if (!ModelState.IsValid) return BadRequest(ModelState);".StartWithTab(2)); 
      lines.Add("");
      lines.Add($"form.SetValuesTo(entity);".StartWithTab(2));
      lines.Add($"entity.SetUpdated(User.Id());".StartWithTab(2));
      lines.Add($"await {service}.UpdateAsync(entity);".StartWithTab(2));
      lines.Add($"return NoContent();".StartWithTab(2));

      lines.Add("}".StartWithTab(1));
      lines.Add("");

      // remove
      lines.Add("[HttpDelete(\"{id}\")]".StartWithTab(1));
      lines.Add($"public async Task<ActionResult> Remove(int id)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add($"var entity = await {service}.GetByIdAsync(id);".StartWithTab(2));
      lines.Add($"if (entity == null) return NotFound();".StartWithTab(2));
      lines.Add("");
      lines.Add($"await {service}.RemoveAsync(entity, User.Id());".StartWithTab(2));
      lines.Add($"return NoContent();".StartWithTab(2));
      lines.Add("}".StartWithTab(1));
      lines.Add("");

      // validate
      lines.Add($"async Task ValidateRequestAsync({result.BaseFormName} form, int id)".StartWithTab(1));
      lines.Add("{".StartWithTab(1));
      lines.Add($"var labels = new {name}Labels();".StartWithTab(2));
      lines.Add($"if (String.IsNullOrEmpty(form.Title)) ModelState.AddModelError(nameof(form.Title), ValidationMessages.Required(labels.Title));".StartWithTab(2));
      lines.Add("");
      lines.Add($"if (!ModelState.IsValid) return;".StartWithTab(2));

      lines.Add("}".StartWithTab(1));
      lines.Add("");
   }
}
