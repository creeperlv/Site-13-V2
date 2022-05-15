using CampaignScriptEditor.Editors.Fields;
using Site13Kernel.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CampaignScriptEditor
{
    public class FieldEditorPool
    {
        public static Dictionary<Type, Type> FieldEditors = new Dictionary<Type, Type>();
        static Type GenericList = typeof(List<int>).GetGenericTypeDefinition();
        static Type ObjectArray = typeof(object[]);
        static Type KVPairT = typeof(KVPair<int,int>).GetGenericTypeDefinition();
        public static IFieldEditor? CreateField(Type t,object? Value=null)
        {
            if (t == ObjectArray) return null;
            if (t.ToString() == ObjectArray.ToString()) return null;
            if (FieldEditors.ContainsKey(t))
            {
                IFieldEditor? editor = Activator.CreateInstance(FieldEditors[t]) as IFieldEditor;
                if(editor is IGenericField gf)
                {
                    gf.SetType(t, Value);
                }
                return editor;
            }
            if (t.IsGenericType)
            {
                if (t.GetGenericTypeDefinition().ToString() == GenericList.ToString())
                {
                    ListField LF = new ListField();
                    LF.SetType(t, Value);
                    return LF;
                }else if(t.GetGenericTypeDefinition().ToString()== KVPairT.ToString())
                {
                    KVPairField KVPF = new KVPairField();
                    KVPF.SetType(t, Value);
                    return KVPF;
                }
            }
            if (t.IsEnum)
            {
                EnumField ef = new EnumField();
                ef.SetType(t, Value);
                return ef;
            }
            {
                GenericField g = new();
                g.SetType(t,Value);
                return g;

            }
        }
        public static IFieldEditor? CreateField(FieldInfo fi, Object? obj = null)
        {
            if (fi.FieldType == ObjectArray) return null;
            if (fi.FieldType.ToString() == ObjectArray.ToString()) return null;
            if (FieldEditors.ContainsKey(fi.FieldType))
            {
                IFieldEditor? editor = Activator.CreateInstance(FieldEditors[fi.FieldType]) as IFieldEditor;
                if (editor != null)
                {
                    if (obj is not null)
                        editor.SetField(fi, fi.GetValue(obj));
                    else 
                        editor.SetField(fi, null);
                    return editor;
                }
            }
            if (fi.FieldType.IsGenericType)
            {
                Trace.WriteLine("A Generic Type");
                if (fi.FieldType.GetGenericTypeDefinition().ToString() == GenericList.ToString())
                {
                    Trace.WriteLine("A List");
                    ListField LF = new ListField();
                    if (obj is not null)
                        LF.SetField(fi, fi.GetValue(obj));
                    else
                        LF.SetField(fi, null);
                    return LF;
                }
            }
            if (fi.FieldType.IsEnum)
            {
                EnumField ef = new EnumField();
                if (obj is not null)
                    ef.SetField(fi, fi.GetValue(obj));
                else
                    ef.SetField(fi, null);
                return ef;
            }
            {
                GenericField g = new();
                if (obj is not null)
                    g.SetField(fi, fi.GetValue(obj));
                else
                    g.SetField(fi, null);
                return g;

            }
        }
    }
}
