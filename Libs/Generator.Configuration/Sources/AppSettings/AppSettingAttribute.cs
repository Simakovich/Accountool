using System;

namespace D9bolic.Generator.Configuration.Sources.AppSettings
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AppSettingAttribute : Attribute
    {
        public AppSettingAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }
}