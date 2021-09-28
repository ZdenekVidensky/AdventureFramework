
namespace TVB.Core.Attributes
{
    using System;
    
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentInChildrenAttribute : Attribute
    {
        public bool Verbose;
        public string Path;

        public GetComponentInChildrenAttribute(string path = "", bool verbose = false)
        {
            Path = path;
            Verbose = verbose;
        }

        public GetComponentInChildrenAttribute(bool verbose)
        {
            Path = "";
            Verbose = verbose;
        }
    }
}