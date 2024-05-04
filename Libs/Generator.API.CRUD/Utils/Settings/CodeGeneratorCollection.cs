using Newtonsoft.Json;

namespace D9bolic.Generator.API.CRUD.Utils.Settings
{
    /// <summary>The command collection.</summary>
    public class CodeGeneratorCollection
    {
        /// <summary>Gets or sets the SwaggerToCSharpClientCommand.</summary>
        [JsonProperty("OpenApiToCSharpClient", NullValueHandling = NullValueHandling.Ignore)]
        public OpenApiToCSharpClientCommand OpenApiToCSharpClientCommand { get; set; }
    }
}