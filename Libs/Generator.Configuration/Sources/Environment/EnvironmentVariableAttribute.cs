using System;

namespace D9bolic.Generator.Configuration.Sources.Environment
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EnvironmentVariableAttribute : Attribute
    {
        public EnvironmentVariableAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }
}