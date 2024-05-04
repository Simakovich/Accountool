using System;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.API.CRUD.Utils
{
    /// <summary>
    /// Attribute data processing methods.
    /// </summary>
    public static class AttributeDataExtensions
    {
        /// <summary>
        /// Check that attribute is child of a presented type.
        /// </summary>
        /// <param name="attributeData">Checked attribute.</param>
        /// <param name="type">Checked type.</param>
        /// <returns>True if attribute is type inheritor, and false if not.</returns>
        public static bool IsChildOf(this AttributeData attributeData, Type type)
            => attributeData.AttributeClass.IsBaseClass(type);

        /// <summary>
        /// Check that attribute is child of a presented type.
        /// </summary>
        /// <param name="attributeData">Checked attribute.</param>
        /// <param name="type">Checked type.</param>
        /// <returns>True if attribute is type inheritor, and false if not.</returns>
        public static bool IsChildOf<TType>(this AttributeData attributeData)
            => attributeData.AttributeClass.IsBaseClass(typeof(TType));
    }
}