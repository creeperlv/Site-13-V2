using Site13Kernel.SceneBuild;
using UnityEngine.Rendering;

namespace Site13Kernel.UI.Forge
{
    public class EditableReflectionProbeEditor : ForgeComponentEditorBase
    {
        public ComboBox RefreshMode;
        public TextBox Size_X;
        public TextBox Size_Y;
        public TextBox Size_Z;
        public EditableReflectionProbe ControlledComponent;
        bool LoadingValues;
        public override void Init()
        {
            base.Init();
            {
                Size_X.SetCallback(ApplySize);
                Size_Y.SetCallback(ApplySize);
                Size_Z.SetCallback(ApplySize);
            }
            RefreshMode.SetCallback((o) =>
            {
                ControlledComponent.RefreshMode = ToRefreshMode(RefreshMode.value);
                ControlledComponent.UpdateScene();
            });
        }
        void ApplySize(object o)
        {
            if (LoadingValues) return;
            if (float.TryParse(Size_X.text, out var x))
            {
                if (float.TryParse(Size_Y.text, out var y))
                {
                    if (float.TryParse(Size_Z.text, out var z))
                    {
                        ControlledComponent.Size = new SerializableVector3(x, y, z);
                        ControlledComponent.UpdateScene();
                    }
                }
            }
        }
        public override void SetComponent(EditableComponent component)
        {
            ControlledComponent = component as EditableReflectionProbe;
            ControlledComponent.UpdateValue();
            LoadingValues = true;
            Size_X.text = ControlledComponent.Size.x.ToString();
            Size_Y.text = ControlledComponent.Size.y.ToString();
            Size_Z.text = ControlledComponent.Size.z.ToString();
            RefreshMode.value = ToInt(ControlledComponent.RefreshMode);
            LoadingValues = false;
        }
        int ToInt(ReflectionProbeRefreshMode s) => s switch
        {
            ReflectionProbeRefreshMode.OnAwake => 0,
            ReflectionProbeRefreshMode.EveryFrame => 1,
            _ => -1
        };
        ReflectionProbeRefreshMode ToRefreshMode(int s) => s switch
        {
            0 => ReflectionProbeRefreshMode.OnAwake,
            1 => ReflectionProbeRefreshMode.EveryFrame,
            _ => ReflectionProbeRefreshMode.OnAwake
        };
    }
}