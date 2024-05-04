//-----------------------------------------------------------------------
// <copyright file="NSwagSettings.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace D9bolic.Generator.API.CRUD.Utils.Settings
{
    /// <summary>The NSwagDocument base class.</summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class NSwagDocument
    {
        /// <summary>Gets the code generators.</summary>
        [JsonProperty("CodeGenerators")]
        public CodeGeneratorCollection CodeGenerators { get; } = new CodeGeneratorCollection();
    }
}