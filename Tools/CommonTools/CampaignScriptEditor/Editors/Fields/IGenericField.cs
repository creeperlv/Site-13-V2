using System;

namespace CampaignScriptEditor.Editors.Fields
{
    public interface IGenericField
    {
        void SetType(Type t,object? value=null);
    }
}
