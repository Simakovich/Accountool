//-----------------------------------------------------------------------
// <copyright file="SwaggerToCSharpControllerCommand.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace D9bolic.Generator.API.CRUD.Utils.Settings
{
    /// <inheritdoc/>
    public class SwaggerToCSharpControllerCommand : OpenApiToCSharpCommandBase<CSharpControllerGeneratorSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerToCSharpControllerCommand"/> class.
        /// </summary>
        public SwaggerToCSharpControllerCommand()
            : base(new CSharpControllerGeneratorSettings())
        {
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ControllerBaseClass
        {
            get { return Settings.ControllerBaseClass; }
            set { Settings.ControllerBaseClass = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public CSharpControllerStyle ControllerStyle
        {
            get { return Settings.ControllerStyle; }
            set { Settings.ControllerStyle = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public CSharpControllerTarget ControllerTarget
        {
            get { return Settings.ControllerTarget; }
            set { Settings.ControllerTarget = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool UseCancellationToken
        {
            get { return Settings.UseCancellationToken; }
            set { Settings.UseCancellationToken = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool UseActionResultType
        {
            get { return Settings.UseActionResultType; }
            set { Settings.UseActionResultType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateModelValidationAttributes
        {
            get { return Settings.GenerateModelValidationAttributes; }
            set { Settings.GenerateModelValidationAttributes = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public CSharpControllerRouteNamingStrategy RouteNamingStrategy
        {
            get { return Settings.RouteNamingStrategy; }
            set { Settings.RouteNamingStrategy = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string BasePath
        {
            get { return Settings.BasePath; }
            set { Settings.BasePath = value; }
        }
    }
}