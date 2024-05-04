﻿//-----------------------------------------------------------------------
// <copyright file="OperationGenerationModeConverterv.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using NSwag.CodeGeneration.OperationNameGenerators;

namespace D9bolic.Generator.API.CRUD.Utils.Settings
{
    /// <summary>
    /// Mapper of nswag settings to the name generators.
    /// </summary>
    internal static class OperationGenerationModeConverter
    {
        /// <summary>
        /// Map generator to the Nswag settings.
        /// </summary>
        /// <param name="operationNameGenerator">Nswag name generator.</param>
        /// <returns>Nswag settings value.</returns>
        internal static OperationGenerationMode GetOperationGenerationMode(
            IOperationNameGenerator operationNameGenerator)
        {
            if (operationNameGenerator is MultipleClientsFromOperationIdOperationNameGenerator)
            {
                return OperationGenerationMode.MultipleClientsFromOperationId;
            }

            if (operationNameGenerator is MultipleClientsFromPathSegmentsOperationNameGenerator)
            {
                return OperationGenerationMode.MultipleClientsFromPathSegments;
            }

            if (operationNameGenerator is MultipleClientsFromFirstTagAndPathSegmentsOperationNameGenerator)
            {
                return OperationGenerationMode.MultipleClientsFromFirstTagAndPathSegments;
            }

            if (operationNameGenerator is MultipleClientsFromFirstTagAndOperationIdGenerator)
            {
                return OperationGenerationMode.MultipleClientsFromFirstTagAndOperationId;
            }

            if (operationNameGenerator is MultipleClientsFromFirstTagAndOperationNameGenerator)
            {
                return OperationGenerationMode.MultipleClientsFromFirstTagAndOperationName;
            }

            if (operationNameGenerator is SingleClientFromOperationIdOperationNameGenerator)
            {
                return OperationGenerationMode.SingleClientFromOperationId;
            }

            if (operationNameGenerator is SingleClientFromPathSegmentsOperationNameGenerator)
            {
                return OperationGenerationMode.SingleClientFromPathSegments;
            }

            return OperationGenerationMode.MultipleClientsFromOperationId;
        }

        /// <summary>
        /// Provide generator based on Nswag settings.
        /// </summary>
        /// <param name="operationGenerationMode">Nswag settings.</param>
        /// <returns>Generator.</returns>
        internal static IOperationNameGenerator GetOperationNameGenerator(
            OperationGenerationMode operationGenerationMode)
        {
            if (operationGenerationMode == OperationGenerationMode.MultipleClientsFromOperationId)
            {
                return new MultipleClientsFromOperationIdOperationNameGenerator();
            }
            else if (operationGenerationMode == OperationGenerationMode.MultipleClientsFromPathSegments)
            {
                return new MultipleClientsFromPathSegmentsOperationNameGenerator();
            }
            else if (operationGenerationMode == OperationGenerationMode.MultipleClientsFromFirstTagAndPathSegments)
            {
                return new MultipleClientsFromFirstTagAndPathSegmentsOperationNameGenerator();
            }
            else if (operationGenerationMode == OperationGenerationMode.MultipleClientsFromFirstTagAndOperationId)
            {
                return new MultipleClientsFromFirstTagAndOperationIdGenerator();
            }
            else if (operationGenerationMode == OperationGenerationMode.MultipleClientsFromFirstTagAndOperationName)
            {
                return new MultipleClientsFromFirstTagAndOperationNameGenerator();
            }
            else if (operationGenerationMode == OperationGenerationMode.SingleClientFromOperationId)
            {
                return new SingleClientFromOperationIdOperationNameGenerator();
            }
            else if (operationGenerationMode == OperationGenerationMode.SingleClientFromPathSegments)
            {
                return new SingleClientFromPathSegmentsOperationNameGenerator();
            }
            else
            {
                return new MultipleClientsFromOperationIdOperationNameGenerator();
            }
        }
    }
}