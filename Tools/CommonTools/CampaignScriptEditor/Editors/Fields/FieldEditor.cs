using System.Reflection;

namespace CampaignScriptEditor.Editors.Fields
{
    public interface IFieldEditor
    {
        void SetField(FieldInfo fi, object? initialValue = null);
        object GetObject();
    }
}
