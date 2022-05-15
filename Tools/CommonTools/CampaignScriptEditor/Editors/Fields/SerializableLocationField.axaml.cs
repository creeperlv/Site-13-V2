using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Site13Kernel.Data.Serializables;
using System.Reflection;

namespace CampaignScriptEditor.Editors.Fields
{
    public partial class SerializableLocationField : UserControl, IFieldEditor
    {
        public SerializableLocationField()
        {
            InitializeComponent();
            PositionField = new Vector3Field();
            ScaleField = new Vector3Field();
            RotationField = new QuaternionField();
            SubFields.Children.Add(PositionField);
            SubFields.Children.Add(RotationField);
            SubFields.Children.Add(ScaleField);
        }
        QuaternionField RotationField;
        Vector3Field PositionField;
        Vector3Field ScaleField;
        public object GetObject()
        {
            SerializableLocation serializableLocation = new SerializableLocation(); ;
            if (UseSceneLookUp.IsChecked.HasValue)
                serializableLocation.UseSceneLookUp = UseSceneLookUp.IsChecked.Value;
            serializableLocation.LookUpName = LookUpName.Text;
            serializableLocation.Position = PositionField.GetObject() as SerializableVector3;
            serializableLocation.Rotation = RotationField.GetObject() as SerializableQuaternion;
            serializableLocation.Scale = ScaleField.GetObject() as SerializableVector3;
            return serializableLocation;
        }
        public void SetField(FieldInfo fi, object? initialValue = null)
        {
            Header.Text = fi.Name;
            if(initialValue is SerializableLocation sl)
            {
                var p=typeof(SerializableLocation).GetField("Position");
                var r=typeof(SerializableLocation).GetField("Rotation");
                var s=typeof(SerializableLocation).GetField("Scale");
                UseSceneLookUp.IsChecked = sl.UseSceneLookUp;
                LookUpName.Text = sl.LookUpName;
                
                PositionField.SetField(p!, sl.Position);
                RotationField.SetField(r!, sl.Rotation);
                ScaleField.SetField(s!, sl.Scale);
            }
            else
            {

                var p = typeof(SerializableLocation).GetField("Position");
                var r = typeof(SerializableLocation).GetField("Rotation");
                var s = typeof(SerializableLocation).GetField("Scale");
                PositionField.SetField(p!,null);
                RotationField.SetField(r!, null);
                ScaleField.SetField(s!, null);
            }
        }

    }
}
