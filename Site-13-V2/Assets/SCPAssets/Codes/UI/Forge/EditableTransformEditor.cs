using Site13Kernel.SceneBuild;
using UnityEngine;

namespace Site13Kernel.UI.Forge
{
    public class EditableTransformEditor : ForgeComponentEditorBase
    {
        public TextBox T_X;
        public TextBox T_Y;
        public TextBox T_Z;
        public TextBox R_X;
        public TextBox R_Y;
        public TextBox R_Z;
        public TextBox S_X;
        public TextBox S_Y;
        public TextBox S_Z;
        EditableTransform ControlledComponent;
        bool LoadingValues = false;
        public override void Init()
        {
            base.Init();
            {
                //Position
                T_X.onEndEdit.AddListener(ApplyPosition);
                T_Y.onEndEdit.AddListener(ApplyPosition);
                T_Z.onEndEdit.AddListener(ApplyPosition);
            }
            {
                //Rotation
                R_X.onEndEdit.AddListener(ApplyRotation);
                R_Y.onEndEdit.AddListener(ApplyRotation);
                R_Z.onEndEdit.AddListener(ApplyRotation);
            }
            {
                //Scale
                S_X.onEndEdit.AddListener(ApplyScale);
                S_Y.onEndEdit.AddListener(ApplyScale);
                S_Z.onEndEdit.AddListener(ApplyScale);
            }
        }
        void ApplyPosition(string t)
        {
            {
                if (float.TryParse(T_X.text, out var v))
                {
                    ControlledComponent.Position.x = v;
                }
            }
            {
                if (float.TryParse(T_Y.text, out var v))
                {
                    ControlledComponent.Position.y = v;
                }
            }
            {
                if (float.TryParse(T_Z.text, out var v))
                {
                    ControlledComponent.Position.z = v;
                }
            }
            ControlledComponent.UpdateScene();
        }
        void ApplyScale(string t)
        {
            {
                if (float.TryParse(S_X.text, out var v))
                {
                    ControlledComponent.Scale.x = v;
                }
            }
            {
                if (float.TryParse(S_Y.text, out var v))
                {
                    ControlledComponent.Scale.y = v;
                }
            }
            {
                if (float.TryParse(S_Z.text, out var v))
                {
                    ControlledComponent.Scale.z = v;
                }
            }
            ControlledComponent.UpdateScene();
        }
        void ApplyRotation(string o)
        {
            if (LoadingValues) return;
            if (float.TryParse(R_X.text, out var x))
            {
                if (float.TryParse(R_Y.text, out var y))
                {
                    if (float.TryParse(R_Z.text, out var z))
                    {
                        ControlledComponent.Rotation = Quaternion.Euler(new Vector3(x, y, z));
                    }
                }
            }
            ControlledComponent.UpdateScene();

        }
        public override void SetComponent(EditableComponent component)
        {
            ControlledComponent = component as EditableTransform;
            ControlledComponent.UpdateValue();
            LoadingValues = true;
            T_X.text = ControlledComponent.Position.x.ToString();
            T_Y.text = ControlledComponent.Position.y.ToString();
            T_Z.text = ControlledComponent.Position.z.ToString();
            var r_v3 = ((Quaternion)ControlledComponent.Rotation).eulerAngles;
            R_X.text = r_v3.x.ToString();
            R_Y.text = r_v3.y.ToString();
            R_Z.text = r_v3.z.ToString();
            S_X.text = ControlledComponent.Scale.x.ToString();
            S_Y.text = ControlledComponent.Scale.y.ToString();
            S_Z.text = ControlledComponent.Scale.z.ToString();
            LoadingValues = false;
        }
    }
}