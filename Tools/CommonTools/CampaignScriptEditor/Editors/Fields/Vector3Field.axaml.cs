using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Site13Kernel.Data.Serializables;
using System.Reflection;

namespace CampaignScriptEditor.Editors.Fields
{
    public partial class Vector3Field : UserControl,IFieldEditor
    {
        public Vector3Field()
        {
            InitializeComponent();
        }

        static void FromVector3(SerializableVector3 vector3, TextBox X, TextBox Y, TextBox Z)
        {
            X.Text = vector3.X.ToString();
            Y.Text = vector3.Y.ToString();
            Z.Text = vector3.Z.ToString();
        }
        static SerializableVector3 ToVector3(TextBox X, TextBox Y, TextBox Z)
        {
            return new SerializableVector3() { X = float.Parse(X.Text), Y = float.Parse(Y.Text), Z = float.Parse(Z.Text) };
        }
        public object GetObject()
        {
            return ToVector3(V3_0_X, V3_0_Y, V3_0_Z);
        }

        public void SetField(FieldInfo fi, object? initialValue = null)
        {
            Header.Text = fi.Name;
            if(initialValue is SerializableVector3 v)
            {
                FromVector3(v, V3_0_X, V3_0_Y, V3_0_Z);
            }
            else
            {
                FromVector3(new SerializableVector3(), V3_0_X, V3_0_Y, V3_0_Z);
            }
        }
    }
}
