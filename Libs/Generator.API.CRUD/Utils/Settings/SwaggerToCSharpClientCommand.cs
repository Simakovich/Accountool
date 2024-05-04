//-----------------------------------------------------------------------
// <copyright file="SwaggerToCSharpClientCommand.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using NSwag.CodeGeneration.CSharp;

namespace D9bolic.Generator.API.CRUD.Utils.Settings
{
    /// <inheritdoc/>
    public class SwaggerToCSharpClientCommand : OpenApiToCSharpCommandBase<CSharpClientGeneratorSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerToCSharpClientCommand"/> class.
        /// </summary>
        public SwaggerToCSharpClientCommand()
            : base(new CSharpClientGeneratorSettings())
        {
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ClientBaseClass
        {
            get { return Settings.ClientBaseClass; }
            set { Settings.ClientBaseClass = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ConfigurationClass
        {
            get { return Settings.ConfigurationClass; }
            set { Settings.ConfigurationClass = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateClientClasses
        {
            get { return Settings.GenerateClientClasses; }
            set { Settings.GenerateClientClasses = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateClientInterfaces
        {
            get { return Settings.GenerateClientInterfaces; }
            set { Settings.GenerateClientInterfaces = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ClientBaseInterface
        {
            get { return Settings.ClientBaseInterface; }
            set { Settings.ClientBaseInterface = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool InjectHttpClient
        {
            get { return Settings.InjectHttpClient; }
            set { Settings.InjectHttpClient = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool DisposeHttpClient
        {
            get { return Settings.DisposeHttpClient; }
            set { Settings.DisposeHttpClient = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string[] ProtectedMethods
        {
            get { return Settings.ProtectedMethods; }
            set { Settings.ProtectedMethods = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateExceptionClasses
        {
            get { return Settings.GenerateExceptionClasses; }
            set { Settings.GenerateExceptionClasses = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ExceptionClass
        {
            get { return Settings.ExceptionClass; }
            set { Settings.ExceptionClass = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool WrapDtoExceptions
        {
            get { return Settings.WrapDtoExceptions; }
            set { Settings.WrapDtoExceptions = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool UseHttpClientCreationMethod
        {
            get { return Settings.UseHttpClientCreationMethod; }
            set { Settings.UseHttpClientCreationMethod = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string HttpClientType
        {
            get { return Settings.HttpClientType; }
            set { Settings.HttpClientType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool UseHttpRequestMessageCreationMethod
        {
            get { return Settings.UseHttpRequestMessageCreationMethod; }
            set { Settings.UseHttpRequestMessageCreationMethod = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool UseBaseUrl
        {
            get { return Settings.UseBaseUrl; }
            set { Settings.UseBaseUrl = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateBaseUrlProperty
        {
            get { return Settings.GenerateBaseUrlProperty; }
            set { Settings.GenerateBaseUrlProperty = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateSyncMethods
        {
            get { return Settings.GenerateSyncMethods; }
            set { Settings.GenerateSyncMethods = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GeneratePrepareRequestAndProcessResponseAsAsyncMethods
        {
            get { return Settings.GeneratePrepareRequestAndProcessResponseAsAsyncMethods; }
            set { Settings.GeneratePrepareRequestAndProcessResponseAsAsyncMethods = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool ExposeJsonSerializerSettings
        {
            get { return Settings.ExposeJsonSerializerSettings; }
            set { Settings.ExposeJsonSerializerSettings = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ClientClassAccessModifier
        {
            get { return Settings.ClientClassAccessModifier; }
            set { Settings.ClientClassAccessModifier = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string TypeAccessModifier
        {
            get { return Settings.CSharpGeneratorSettings.TypeAccessModifier; }
            set { Settings.CSharpGeneratorSettings.TypeAccessModifier = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateContractsOutput { get; set; }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ContractsNamespace { get; set; }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ContractsOutputFilePath { get; set; }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ParameterDateTimeFormat
        {
            get { return Settings.ParameterDateTimeFormat; }
            set { Settings.ParameterDateTimeFormat = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ParameterDateFormat
        {
            get { return Settings.ParameterDateFormat; }
            set { Settings.ParameterDateFormat = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateUpdateJsonSerializerSettingsMethod
        {
            get { return Settings.GenerateUpdateJsonSerializerSettingsMethod; }
            set { Settings.GenerateUpdateJsonSerializerSettingsMethod = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool UseRequestAndResponseSerializationSettings
        {
            get { return Settings.UseRequestAndResponseSerializationSettings; }
            set { Settings.UseRequestAndResponseSerializationSettings = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool SerializeTypeInformation
        {
            get { return Settings.SerializeTypeInformation; }
            set { Settings.SerializeTypeInformation = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string QueryNullValue
        {
            get { return Settings.QueryNullValue; }
            set { Settings.QueryNullValue = value; }
        }
    }
}