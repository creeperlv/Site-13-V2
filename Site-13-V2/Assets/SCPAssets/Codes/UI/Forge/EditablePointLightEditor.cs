using Site13Kernel.SceneBuild;
using System;
using UnityEngine;

namespace Site13Kernel.UI.Forge
{
    public class EditablePointLightEditor : ForgeComponentEditorBase
    {
        public ComboBox Shadow;
        public TextBox Color_R;
        public TextBox Color_G;
        public TextBox Color_B;
        public TextBox Color_A;
        public TextBox Intensity;
        public TextBox Range;
        public EditablePointLight ControlledComponent;
        bool LoadingValues;
        public override void Init()
        {
            base.Init();
            {
                Color_R.SetCallback(ApplyColor);
                Color_G.SetCallback(ApplyColor);
                Color_B.SetCallback(ApplyColor);
                Color_A.SetCallback(ApplyColor);
            }
            Shadow.SetCallback((o) =>
            {
                ControlledComponent.Shadows = ToLightShadows(Shadow.value);
                ControlledComponent.UpdateScene();
            });
            {
                Intensity.SetCallback((o) =>
                {
                    if (LoadingValues) return;
                    var t = o.ToString();
                    if (float.TryParse(t, out var v))
                    {
                        ControlledComponent.LightIntensity = v;
                    }
                    ControlledComponent.UpdateScene();
                });
                Range.SetCallback((o) =>
                {
                    if (LoadingValues) return;
                    var t = o.ToString();
                    if (float.TryParse(t, out var v))
                    {
                        ControlledComponent.LightRange = v;
                    }
                    ControlledComponent.UpdateScene();
                });
            }
        }
        void ApplyColor(object o)
        {
            if (LoadingValues) return;
            if (float.TryParse(Color_R.text, out var r))
            {
                if (float.TryParse(Color_G.text, out var g))
                {
                    if (float.TryParse(Color_B.text, out var b))
                    {
                        if (float.TryParse(Color_A.text, out var a))
                        {
                            ControlledComponent.Color = new SerializableColor(r, g, b, a);
                            ControlledComponent.UpdateScene();
                        }
                    }
                }
            }
        }
        public override void SetComponent(EditableComponent component)
        {
            ControlledComponent = component as EditablePointLight;
            ControlledComponent.UpdateValue();
            LoadingValues = true;
            Intensity.text = ControlledComponent.LightIntensity.ToString();
            Range.text = ControlledComponent.LightRange.ToString();
            Color_R.text = ControlledComponent.Color.R.ToString();
            Color_G.text = ControlledComponent.Color.G.ToString();
            Color_B.text = ControlledComponent.Color.B.ToString();
            Color_A.text = ControlledComponent.Color.A.ToString();
            Shadow.value = ToInt(ControlledComponent.Shadows);
            LoadingValues = false;
        }
        int ToInt(LightShadows s) => s switch
        {
            LightShadows.None => 0,
            LightShadows.Hard => 1,
            LightShadows.Soft => 2,
            _ => -1
        };
        LightShadows ToLightShadows(int s) => s switch
        {
            0 => LightShadows.None,
            1 => LightShadows.Hard,
            2 => LightShadows.Soft,
            _ => LightShadows.None
        };
    }
}