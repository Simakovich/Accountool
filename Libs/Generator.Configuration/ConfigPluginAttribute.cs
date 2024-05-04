using System;

namespace D9bolic.Generator.Configuration
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public class ConfigPluginAttribute : Attribute
    {
    }
}