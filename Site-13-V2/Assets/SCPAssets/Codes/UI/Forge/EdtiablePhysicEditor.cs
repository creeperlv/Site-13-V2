using Site13Kernel.SceneBuild;
using UnityEngine.Rendering;

namespace Site13Kernel.UI.Forge
{
    public class EdtiablePhysicEditor : ForgeComponentEditorBase
    {
        public Toggle UseCollider;
        public Toggle UseRigidbody;
        public Toggle UseGravity;
        public TextBox Mass;
        public TextBox Drag;
        public TextBox AngularDrag;
        public EditablePhysics ControlledComponent;
        bool LoadingValues;
        public override void Init()
        {
            base.Init();
            {
                Mass.onEndEdit.AddListener((v) =>
                {
                    if (LoadingValues) return;
                    if (float.TryParse(v, out var _v))
                    {
                        ControlledComponent.Mass = _v;
                        ControlledComponent.UpdateScene();
                    }
                });
                Drag.onEndEdit.AddListener((v) =>
                {
                    if (LoadingValues) return;
                    if (float.TryParse(v, out var _v))
                    {
                        ControlledComponent.Drag = _v;
                        ControlledComponent.UpdateScene();
                    }
                });
                AngularDrag.onEndEdit.AddListener((v) =>
                {
                    if (LoadingValues) return;
                    if (float.TryParse(v, out var _v))
                    {
                        ControlledComponent.AngularDrag = _v;
                        ControlledComponent.UpdateScene();
                    }
                });
                {
                    UseCollider.onValueChanged.AddListener((b) =>
                    {
                        if (LoadingValues) return;
                        ControlledComponent.useCollider = b;
                        ControlledComponent.UpdateScene();
                    });
                }
                {
                    UseRigidbody.onValueChanged.AddListener((b) =>
                    {
                        if (LoadingValues) return;
                        ControlledComponent.useRigidbody = b;
                        ControlledComponent.UpdateScene();
                    });
                }
                {
                    UseGravity.onValueChanged.AddListener((b) =>
                    {
                        if (LoadingValues) return;
                        ControlledComponent.useGravity = b;
                        ControlledComponent.UpdateScene();
                    });
                }
            }
        }
        public override void SetComponent(EditableComponent component)
        {
            ControlledComponent = component as EditablePhysics;
            LoadingValues = true;
            Mass.text = ControlledComponent.Mass.ToString();
            Drag.text = ControlledComponent.Mass.ToString();
            AngularDrag.text = ControlledComponent.Mass.ToString();
            UseCollider.isOn = ControlledComponent.useCollider;
            UseRigidbody.isOn = ControlledComponent.useRigidbody;
            UseGravity.isOn = ControlledComponent.useGravity;
            LoadingValues = false;
        }
    }
}