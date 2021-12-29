using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.UI.UIXML
{
    public class Constructor : MonoBehaviour
    {
        public List<UIStyleDefinition> Definitions;
        static List<UIStyleDefinition> _Definitions;
        public int DefaultStyle = 0;
        static Builder builder;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Start()
        {
            _Definitions = Definitions;
            UseStyle(DefaultStyle);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UseStyle(string Name)
        {
            for (int i = 0; i < _Definitions.Count; i++)
            {
                var item = _Definitions[i];

                if (item.StyleName.ToUpper() == Name.ToUpper())
                {
                    UseStyle(i);
                    return;
                }
            }
            Diagnostics.Debug.LogError($"[UIXML]Target Style name ({Name}) does not exist.");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UseStyle(int Index)
        {
            if (Index < 0 || Index >= _Definitions.Count)
            {
                Diagnostics.Debug.LogError($"[UIXML]Target Style index ({Index}) does not exist.");
                return;
            }
            builder = new Builder(_Definitions[Index]);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ConstructUI(Transform Base, string UIContent)
        {
            if (builder != null)
            {
                builder.BuildUI(Base, UIContent);
            }
            else
            {
                Diagnostics.Debug.LogError("[UIXML]Builder is not initialized.");
            }
        }
    }
}
