
namespace TVB.Core.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentAttribute : Attribute
    {
        public bool Verbose;

        public GetComponentAttribute(bool verbose = false)
        {
            Verbose = verbose;
        }
    }
}