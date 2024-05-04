//-----------------------------------------------------------------------
// <copyright file="SwaggerToCSharpCommand.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using NJsonSchema.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp;

namespace D9bolic.Generator.API.CRUD.Utils.Settings
{
    /// <inheritdoc/>
    public abstract class OpenApiToCSharpCommandBase<TSettings> : CodeGeneratorCommandBase<TSettings>
        where TSettings : CSharpGeneratorBaseSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiToCSharpCommandBase{TSettings}"/> class.
        /// </summary>
        /// <param name="settings">Outer settings.</param>
        protected OpenApiToCSharpCommandBase(TSettings settings)
            : base(settings)
        {
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ClassName
        {
            get { return Settings.ClassName; }
            set { Settings.ClassName = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public OperationGenerationMode OperationGenerationMode
        {
            get { return OperationGenerationModeConverter.GetOperationGenerationMode(Settings.OperationNameGenerator); }
            set { Settings.OperationNameGenerator = OperationGenerationModeConverter.GetOperationNameGenerator(value); }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string[] AdditionalNamespaceUsages
        {
            get { return Settings.AdditionalNamespaceUsages; }
            set { Settings.AdditionalNamespaceUsages = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string[] AdditionalContractNamespaceUsages
        {
            get { return Settings.AdditionalContractNamespaceUsages; }
            set { Settings.AdditionalContractNamespaceUsages = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateOptionalParameters
        {
            get { return Settings.GenerateOptionalParameters; }
            set { Settings.GenerateOptionalParameters = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateJsonMethods
        {
            get { return Settings.CSharpGeneratorSettings.GenerateJsonMethods; }
            set { Settings.CSharpGeneratorSettings.GenerateJsonMethods = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool EnforceFlagEnums
        {
            get { return Settings.CSharpGeneratorSettings.EnforceFlagEnums; }
            set { Settings.CSharpGeneratorSettings.EnforceFlagEnums = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ParameterArrayType
        {
            get { return Settings.ParameterArrayType; }
            set { Settings.ParameterArrayType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ParameterDictionaryType
        {
            get { return Settings.ParameterDictionaryType; }
            set { Settings.ParameterDictionaryType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ResponseArrayType
        {
            get { return Settings.ResponseArrayType; }
            set { Settings.ResponseArrayType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ResponseDictionaryType
        {
            get { return Settings.ResponseDictionaryType; }
            set { Settings.ResponseDictionaryType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool WrapResponses
        {
            get { return Settings.WrapResponses; }
            set { Settings.WrapResponses = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string[] WrapResponseMethods
        {
            get { return Settings.WrapResponseMethods; }
            set { Settings.WrapResponseMethods = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateResponseClasses
        {
            get { return Settings.GenerateResponseClasses; }
            set { Settings.GenerateResponseClasses = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ResponseClass
        {
            get { return Settings.ResponseClass; }
            set { Settings.ResponseClass = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string Namespace
        {
            get { return Settings.CSharpGeneratorSettings.Namespace; }
            set { Settings.CSharpGeneratorSettings.Namespace = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool RequiredPropertiesMustBeDefined
        {
            get { return Settings.CSharpGeneratorSettings.RequiredPropertiesMustBeDefined; }
            set { Settings.CSharpGeneratorSettings.RequiredPropertiesMustBeDefined = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string DateType
        {
            get { return Settings.CSharpGeneratorSettings.DateType; }
            set { Settings.CSharpGeneratorSettings.DateType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string[] JsonConverters
        {
            get { return Settings.CSharpGeneratorSettings.JsonConverters; }
            set { Settings.CSharpGeneratorSettings.JsonConverters = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string AnyType
        {
            get { return Settings.CSharpGeneratorSettings.AnyType; }
            set { Settings.CSharpGeneratorSettings.AnyType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string DateTimeType
        {
            get { return Settings.CSharpGeneratorSettings.DateTimeType; }
            set { Settings.CSharpGeneratorSettings.DateTimeType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string TimeType
        {
            get { return Settings.CSharpGeneratorSettings.TimeType; }
            set { Settings.CSharpGeneratorSettings.TimeType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string TimeSpanType
        {
            get { return Settings.CSharpGeneratorSettings.TimeSpanType; }
            set { Settings.CSharpGeneratorSettings.TimeSpanType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ArrayType
        {
            get { return Settings.CSharpGeneratorSettings.ArrayType; }
            set { Settings.CSharpGeneratorSettings.ArrayType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ArrayInstanceType
        {
            get { return Settings.CSharpGeneratorSettings.ArrayInstanceType; }
            set { Settings.CSharpGeneratorSettings.ArrayInstanceType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string DictionaryType
        {
            get { return Settings.CSharpGeneratorSettings.DictionaryType; }
            set { Settings.CSharpGeneratorSettings.DictionaryType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string DictionaryInstanceType
        {
            get { return Settings.CSharpGeneratorSettings.DictionaryInstanceType; }
            set { Settings.CSharpGeneratorSettings.DictionaryInstanceType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string ArrayBaseType
        {
            get { return Settings.CSharpGeneratorSettings.ArrayBaseType; }
            set { Settings.CSharpGeneratorSettings.ArrayBaseType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string DictionaryBaseType
        {
            get { return Settings.CSharpGeneratorSettings.DictionaryBaseType; }
            set { Settings.CSharpGeneratorSettings.DictionaryBaseType = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public CSharpClassStyle ClassStyle
        {
            get { return Settings.CSharpGeneratorSettings.ClassStyle; }
            set { Settings.CSharpGeneratorSettings.ClassStyle = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public CSharpJsonLibrary JsonLibrary
        {
            get { return Settings.CSharpGeneratorSettings.JsonLibrary; }
            set { Settings.CSharpGeneratorSettings.JsonLibrary = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateDefaultValues
        {
            get { return Settings.CSharpGeneratorSettings.GenerateDefaultValues; }
            set { Settings.CSharpGeneratorSettings.GenerateDefaultValues = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateDataAnnotations
        {
            get { return Settings.CSharpGeneratorSettings.GenerateDataAnnotations; }
            set { Settings.CSharpGeneratorSettings.GenerateDataAnnotations = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string[] ExcludedTypeNames
        {
            get { return Settings.CSharpGeneratorSettings.ExcludedTypeNames; }
            set { Settings.CSharpGeneratorSettings.ExcludedTypeNames = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string[] ExcludedParameterNames
        {
            get { return Settings.ExcludedParameterNames; }
            set { Settings.ExcludedParameterNames = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool HandleReferences
        {
            get { return Settings.CSharpGeneratorSettings.HandleReferences; }
            set { Settings.CSharpGeneratorSettings.HandleReferences = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateImmutableArrayProperties
        {
            get { return Settings.CSharpGeneratorSettings.GenerateImmutableArrayProperties; }
            set { Settings.CSharpGeneratorSettings.GenerateImmutableArrayProperties = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateImmutableDictionaryProperties
        {
            get { return Settings.CSharpGeneratorSettings.GenerateImmutableDictionaryProperties; }
            set { Settings.CSharpGeneratorSettings.GenerateImmutableDictionaryProperties = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public string JsonSerializerSettingsTransformationMethod
        {
            get { return Settings.CSharpGeneratorSettings.JsonSerializerSettingsTransformationMethod; }
            set { Settings.CSharpGeneratorSettings.JsonSerializerSettingsTransformationMethod = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool InlineNamedArrays
        {
            get { return Settings.CSharpGeneratorSettings.InlineNamedArrays; }
            set { Settings.CSharpGeneratorSettings.InlineNamedArrays = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool InlineNamedDictionaries
        {
            get { return Settings.CSharpGeneratorSettings.InlineNamedDictionaries; }
            set { Settings.CSharpGeneratorSettings.InlineNamedDictionaries = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool InlineNamedTuples
        {
            get { return Settings.CSharpGeneratorSettings.InlineNamedTuples; }
            set { Settings.CSharpGeneratorSettings.InlineNamedTuples = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool InlineNamedAny
        {
            get { return Settings.CSharpGeneratorSettings.InlineNamedAny; }
            set { Settings.CSharpGeneratorSettings.InlineNamedAny = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateDtoTypes
        {
            get { return Settings.GenerateDtoTypes; }
            set { Settings.GenerateDtoTypes = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateOptionalPropertiesAsNullable
        {
            get { return Settings.CSharpGeneratorSettings.GenerateOptionalPropertiesAsNullable; }
            set { Settings.CSharpGeneratorSettings.GenerateOptionalPropertiesAsNullable = value; }
        }

        /// <summary>
        /// Represent selfnamed settings prop.
        /// </summary>
        public bool GenerateNullableReferenceTypes
        {
            get { return Settings.CSharpGeneratorSettings.GenerateNullableReferenceTypes; }
            set { Settings.CSharpGeneratorSettings.GenerateNullableReferenceTypes = value; }
        }
    }
}