using System.Collections.Generic;
using System.Linq;
using D9bolic.Generator.API.CRUD.Utils;
using Humanizer;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.API.CRUD.Providers;

public static class ControllerGenerator
{
    public static void Generate(GeneratorExecutionContext context, ITypeSymbol candidate)
    {
        var assemblyName = context.Compilation.AssemblyName;

        var typeName =
            candidate!.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).EscapeFileName();

        var dependencies =
            DependenciesProvider.GetDependencies(context, $"{typeName}Controller", $"{assemblyName}.Controllers");
        var code = @$"using System.Data;
                  using Microsoft.AspNetCore.Mvc;
                  using {assemblyName}.Services;
                  using {candidate.ContainingNamespace};
  
                  namespace {assemblyName}.Controllers;    

                  {GenerateClass(candidate, typeName, dependencies)}";

        var fileName = $" {assemblyName}.Controllers.{typeName}Controller.g.cs"!;
        context.AddSource(fileName, code.FormatCode());
    }

    private static string GenerateClass(ITypeSymbol candidate, string typeName,
        IEnumerable<DependenciesProvider.Dependency> dependencies)
    {
        var pluralName = typeName.Pluralize();
        return @$"
               [ApiController]
               public partial class {typeName}Controller : Controller
               {{
                    private readonly I{typeName}Service _service;
                
                    public {typeName}Controller(I{typeName}Service service{dependencies.GetConstructorArguments()}) 
                    {{
                        _service = service;
                        {dependencies.GetConstructorAssigment()}
                    }}
                
                    [HttpGet(""{pluralName}"", Name = ""Get{pluralName}"")]
                    [ProducesResponseType(typeof(IEnumerable<{typeName}>), 200)]
                    public async Task<IActionResult> GetAll()
                    {{
                        return Ok(await _service.GetAll());
                    }}
                
                    [HttpGet(""{pluralName}/{{id:int}}"", Name = ""Get{typeName}ById"")]
                    [ProducesResponseType(typeof({typeName}), 200)]
                    public async Task<IActionResult> GetById(int id)
                    {{
                        return Ok(await _service.GetById(id));
                    }}
                
                    [HttpPost(""{pluralName}"", Name = ""Create{typeName}"")]
                    [ProducesResponseType(typeof(int), 200)]
                    public async Task<IActionResult> Post([FromBody] {typeName} entity)
                    {{
                        return Ok(await _service.Create(entity));
                    }}
                
                    [HttpPut(""{pluralName}"", Name = ""Update{typeName}"")]
                    [ProducesResponseType(typeof(void), 200)]
                    public async Task<IActionResult> Put([FromBody] {typeName} entity)
                    {{
                        await _service.Update(entity);
                        return Ok();
                    }}
                
                    [HttpDelete(""{pluralName}/{{id:int}}"", Name = ""Delete{typeName}"")]
                    [ProducesResponseType(typeof(void), 200)]
                    public async Task<IActionResult> Delete(int id)
                    {{
                        await _service.Delete(id);
                        return Ok();
                    }}
        
                    {GenerateClassForeignMethods(candidate, typeName)}
               }}";
    }

    private static string GenerateClassForeignMethods(ITypeSymbol candidate, string typeName)
    {
        return string.Join(" ", candidate
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(x => x.GetAttributes().Any(attr =>
                attr.AttributeClass.IsBaseClass("ForeignKeyAttribute", "System.ComponentModel.DataAnnotations.Schema")))
            .Select(prop =>
            {
                var foreignName = prop.Name.TrimEnd("Id");
                var foreignPluralName = foreignName.Pluralize();
                var pluralName = typeName.Pluralize();

                return
                    $@"[HttpGet(""{foreignPluralName}/{{{prop.Name}}}/{pluralName}"", Name = ""Get{pluralName}For{foreignName}"")]
                          [ProducesResponseType(typeof(IEnumerable<{typeName}>), 200)]
                          public async Task<IActionResult> Get{pluralName}For{foreignName}(int {prop.Name})
                          {{
                              return Ok(await _service.GetFor{foreignName}({prop.Name}));
                          }}";
            }));
    }
}