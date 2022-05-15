using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Site13Kernel.Data.Serializables;
using System.Reflection;

namespace CampaignScriptEditor.Editors.Fields
{
    public partial class QuaternionField : UserControl, IFieldEditor
    {
        public QuaternionField()
        {
            InitializeComponent();
        }

        static SerializableQuaternion ToQuaternion(TextBox X, TextBox Y, TextBox Z, TextBox W)
        {
            return new SerializableQuaternion() { X = float.Parse(X.Text), Y = float.Parse(Y.Text), Z = float.Parse(Z.Text), W = float.Parse(W.Text) };
        }
        static void FromQuaternion(SerializableQuaternion quaternion, TextBox X, TextBox Y, TextBox Z, TextBox W)
        {
            X.Text = quaternion.X.ToString();
            Y.Text = quaternion.Y.ToString();
            Z.Text = quaternion.Z.ToString();
            W.Text = quaternion.W.ToString();
        }

        public void SetField(FieldInfo fi, object? initialValue = null)
        {
            Header.Text = fi.Name;
            if (initialValue is SerializableQuaternion v)
            {
                FromQuaternion(v, Q_0_X, Q_0_Y, Q_0_Z, Q_0_W);
            }else
            {
                FromQuaternion(new SerializableQuaternion(), Q_0_X, Q_0_Y, Q_0_Z, Q_0_W);
            }
        }

        public object GetObject()
        {
            return ToQuaternion(Q_0_X, Q_0_Y, Q_0_Z, Q_0_W);
        }
    }
}
