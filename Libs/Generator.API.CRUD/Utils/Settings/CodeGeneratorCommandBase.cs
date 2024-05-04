//-----------------------------------------------------------------------
// <copyright file="CodeGeneratorCommandBase.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using NSwag.CodeGeneration;

namespace D9bolic.Generator.API.CRUD.Utils.Settings
{
    /// <summary>
    /// Base outer Nswag settings DTO.
    /// </summary>
    /// <typeparam name="TSettings">Inner settings to be parsed from the outer.</typeparam>
    public abstract class CodeGeneratorCommandBase<TSettings>
        where TSettings : ClientGeneratorBaseSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGeneratorCommandBase{TSettings}"/> class.
        /// </summary>
        /// <param name="settings">Inner settings.</param>
        protected CodeGeneratorCommandBase(TSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Settings instance that can be retreived by consumer.
        /// </summary>
        [JsonIgnore]
        public TSettings Settings { get; }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string TemplateDirectory
        {
            get { return Settings.CodeGeneratorSettings.TemplateDirectory; }
            set { Settings.CodeGeneratorSettings.TemplateDirectory = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string TypeNameGeneratorType { get; set; }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string PropertyNameGeneratorType { get; set; }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string EnumNameGeneratorType { get; set; }
    }
}