using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using D9bolic.Generator.API.CRUD.Utils.Settings;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace D9bolic.Generator.API.CRUD.Utils
{
    /// <summary>
    /// Extensions over the analized assembly.
    /// </summary>
    public static class ContextExtensions
    {
        private static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include,
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter(),
                },
            };
        }

        /// <summary>
        /// Provide all aditional files based on the extension.
        /// </summary>
        /// <param name="context">Current generation context.</param>
        /// <param name="extension">File extension.</param>
        /// <returns>List of files.</returns>
        public static IEnumerable<AdditionalText> GetFiles(this GeneratorExecutionContext context, string extension)
        {
            return context.AdditionalFiles
                .Where(f => f.Path.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Provide all client setup files. Expected content - OpenAPI 3 scheme.
        /// </summary>
        /// <param name="context">Current generation context.</param>
        /// <returns>List of files.</returns>
        public static IEnumerable<AdditionalText> GetClients(this GeneratorExecutionContext context)
        {
            return context.GetFiles(".client");
        }

        /// <summary>
        /// Provide all nswag custom settings.
        /// </summary>
        /// <param name="context">Current generation context.</param>
        /// <returns>List of files.</returns>
        public static IEnumerable<AdditionalText> GetNswagSettings(this GeneratorExecutionContext context)
        {
            return context.GetFiles(".client.nswag");
        }

        /// <summary>
        /// Provide all nswag custom generation configs.
        /// </summary>
        /// <param name="context">Current generation context.</param>
        /// <returns>List of files.</returns>
        public static IEnumerable<AdditionalText> GetClientSettings(this GeneratorExecutionContext context)
        {
            return context.GetFiles(".client.config");
        }

        /// <summary>
        /// Provide custom setting, relevant for the provided client setup.
        /// </summary>
        /// <typeparam name="TType">Expected settings type.</typeparam>
        /// <param name="file">Client setup file.</param>
        /// <param name="docs">Filtered files.</param>
        /// <param name="cancellationToken">Canceletion token.</param>
        /// <returns>Found settings.</returns>
        public static TType GetRelevantSettings<TType>(this AdditionalText file, IEnumerable<AdditionalText> docs,
            CancellationToken cancellationToken)
            where TType : class
        {
            var fileName = Path.GetFileName(file.Path);
            var configFile = docs.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x.Path) == fileName);
            if (configFile is null)
            {
                return null;
            }

            var configFileText = configFile.GetText(cancellationToken);
            var settings = JsonConvert.DeserializeObject<TType>(configFileText.ToString(), GetSerializerSettings());
            return settings;
        }

        /// <summary>
        /// Generation setup DTO.
        /// </summary>
        /// <typeparam name="TGenerationSettings"></typeparam>
        public class GenerationSetup<TGenerationSettings>
        {
            /// <summary>
            /// Client name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Client content. Expected OpenAPI scheme.
            /// </summary>
            public string Content { get; set; }

            /// <summary>
            /// Custom Nswag settings.
            /// </summary>
            public NSwagDocument Nswag { get; set; }

            /// <summary>
            /// Generation custom expected configs.
            /// </summary>
            public TGenerationSettings Config { get; set; }
        }

        /// <summary>
        /// Provide setup of generations based on the current context.
        /// </summary>
        /// <typeparam name="TGenerationSettings">Custom configs type.</typeparam>
        /// <param name="context">Generation context.</param>
        /// <param name="cancellationToken">Canceletion token.</param>
        /// <returns>List of setups.</returns>
        public static IEnumerable<GenerationSetup<TGenerationSettings>> GetSetups<TGenerationSettings>(
            this GeneratorExecutionContext context, CancellationToken cancellationToken)
            where TGenerationSettings : class
        {
            var clients = context.GetClients();
            var nswags = context.GetNswagSettings();
            var configs = context.GetClientSettings();

            foreach (var client in clients)
            {
                var content = client.GetText(cancellationToken).ToString();
                if (string.IsNullOrWhiteSpace(content))
                {
                    continue;
                }

                yield return new GenerationSetup<TGenerationSettings>
                {
                    Content = content,
                    Name = Path.GetFileNameWithoutExtension(client.Path),
                    Nswag = client.GetRelevantSettings<NSwagDocument>(nswags, cancellationToken),
                    Config = client.GetRelevantSettings<TGenerationSettings>(configs, cancellationToken),
                };
            }
        }
    }
}