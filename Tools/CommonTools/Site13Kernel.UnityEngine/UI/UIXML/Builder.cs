using System.Text;
using System.Xml;
using UnityEngine;

namespace Site13Kernel.UI.UIXML
{
    internal class Builder
    {
        UIStyleDefinition PreferredDefinition;
        UIStyleDefinition FallbackDefinition;
        internal Builder(UIStyleDefinition definition,UIStyleDefinition fallbackDefinition=null)
        {
            PreferredDefinition = definition;
            FallbackDefinition = fallbackDefinition;
        }
        internal void BuildUI(Transform Base, string UIContent)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(UIContent);

        }
    }
}
