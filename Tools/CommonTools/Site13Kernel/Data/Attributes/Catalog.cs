using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class CatalogAttribute : Attribute
    {
        readonly string _Catalog;
        public CatalogAttribute(string CatalogString)
        {
            this._Catalog = CatalogString;

        }

        public string CatalogString
        {
            get { return _Catalog; }
        }
    }
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class DescriptionAttribute : Attribute
    {
        readonly string _DescriptionString;

        public DescriptionAttribute(string DescriptionString)
        {
            this._DescriptionString = DescriptionString;

        }

        public string DescriptionString
        {
            get { return _DescriptionString; }
        }

    }
}
