using CLUNL.Localization;
using Newtonsoft.Json;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.Data;
using Site13Kernel.GameLogic;
using Site13Kernel.GameLogic.Forge;
using Site13Kernel.GameLogic.Level;
using Site13Kernel.IO;
using Site13Kernel.UI;
using Site13Kernel.UI.Forge;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.SceneBuild
{
    public class ForgeSystem : ControlledBehavior
    {
        public ForgeData DataSet;
        public Camera ForgeCam;
        public Transform MenuTemplate;
        public Transform MenuHolder;
        public GameObject ObjectMenu;
        public Text Title;
        public GameObject ButtonTemplate;
        public List<ForgeEditorDefinition> Editors;
        public List<KVPair<LocalizedString, Transform>> Menus;
        public TextBox PlacementRange;
        public TextBox AngleSnap;
        public bool PreBuild;
        [Header("Pause Menu")]
        public GameObject PauseMenu;
        public UIButton Save;
        public UIButton Reset;
        public UIButton Continue;
        public UIButton Exit;
        FPSController FPSC;
        public ForgeController ForgeController;
        [Header("Entrance")]
        public GameObject LoadingScreen;
        [Multiline]
        public string PreBuildContent;
        public override void Init()
        {
            var MN = new LocalizedString("Forge.Menu", "FORGE MAIN MENU");
            var BK = new LocalizedString("Forge.Menu.Back", "BACK");
            StartCoroutine(SpawnPlayer());
            Parent.RegisterRefresh(this);
            InitPauseMenu();
            InitMenus(MN, BK);
            InitEditors(MN);
            ConstructWorld();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void InitPauseMenu()
        {
            Exit.OnClick = () =>
            {
                SceneLoader.Instance.EndLevel();
            };
            Reset.OnClick = () =>
            {
                ConstructWorld(true);
            };
            Continue.OnClick = () => { TooglePauseMenu(); };
            if (ForgeLocals.Instance != null)
            {
                if (ForgeLocals.Instance.isReadOnly)
                {
                    Save.gameObject.SetActive(false);
                }
            }
            Save.OnClick = () =>
            {
                var content = SceneBuilder.CurrentBuilder.Serialize();
                FileIO.WriteAllText(ForgeLocals.Instance.SceneDescriptionFile, content);
                //DialogManager.Show("Success!", "Map is now saved", "OK", () => { });
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ConstructWorld(bool isReset = false)
        {
            StartCoroutine(__construct(isReset));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator __construct(bool isReset = false)
        {
            yield return null;//Wait 1 frame to wait scene builder to initialize.
            if (ForgeLocals.Instance != null)
            {
                if (ForgeLocals.Instance.SceneDescriptionFile != null)
                {
                    if (isReset == false)
                    {
                        //Load Base Map.
                        if (ForgeLocals.Instance.BaseMap != null)
                        {
                            {
                                if (int.TryParse(ForgeLocals.Instance.BaseMap, out var id))
                                {
                                    SceneLoader.Instance.LoadScene(id, true, true, false);
                                }
                                else
                                {
                                    SceneLoader.Instance.LoadScene(ForgeLocals.Instance.BaseMap, true, true, false);
                                }
                            }
                            while (SceneLoader.Instance.LoadingOperationCount > 0)
                            {
                                yield return null;
                            }
                            {
                                if (int.TryParse(ForgeLocals.Instance.BaseMap, out var id))
                                {
                                    SceneLoader.Instance.SetActive(id);
                                }
                                else
                                {
                                    SceneLoader.Instance.SetActive(ForgeLocals.Instance.BaseMap);
                                }
                            }
                        }
                    }
                    if (ForgeLocals.Instance.SceneDescriptionFile.Exists)
                    {
                        //Construce scene.
                        SceneDescription sceneDescription = null;
                        try
                        {
                            sceneDescription = JsonConvert.DeserializeObject<SceneDescription>(File.ReadAllText(ForgeLocals.Instance.SceneDescriptionFile.FullName), SceneBuilder.settings);

                        }
                        catch (System.Exception e)
                        {
                            DialogManager.Show("Unable to conststruct the world.", "Something went wrong:" + e, "OK", () => { });
                        }
                        yield return null;//Wait 1 frame to wait scene builder to initialize.
                        try
                        {
                            SceneBuilder.CurrentBuilder.Deserialize(sceneDescription);
                            var FirstP = SceneBuilder.CurrentBuilder.ObjectsContainer.GetComponentInChildren<SpawnPoint>();
                            if (FirstP != null)
                            {

                                ForgeController.transform.position = FirstP.transform.position;
                                FPSC.transform.position = FirstP.transform.position;
                            }
                        }
                        catch (System.Exception e)
                        {
                            DialogManager.Show("Unable to conststruct the world.", "Something went wrong:" + e, "OK", () => { });
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    //Must be in Unity editor mode.
                    DialogManager.Show("Editor mode", "You are in Unity Editor or ForgeLocal failed to setup, hence, you cannot save this world", "OK", () => { });
                }
            }
            else
            {
                //Must be in Unity editor mode.
                DialogManager.Show("Editor mode", "You are in Unity Editor or ForgeLocal failed to setup, hence, you cannot save this world", "OK", () => { });
            }
            LoadingScreen.SetActive(false);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitEditors(LocalizedString MN)
        {
            foreach (var item in Editors)
            {
                item.TargetEditor.CurrentSystem = this;
                item.TargetEditor.Init();
            }
            ShowMenu(MN);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitMenus(LocalizedString MN, LocalizedString BK)
        {
            foreach (var item in DataSet.ForgeCategories)
            {
                InitCategory(MN, BK, item);
            }
            {
                var T = Instantiate(MenuTemplate, MenuHolder);
                foreach (var item in Menus)
                {
                    var B = Instantiate(ButtonTemplate, T);
                    var _btn = B.GetComponent<UI.UIButton>();
                    _btn.Content = item.Key;
                    _btn.OnClick = () =>
                    {
                        ShowMenu(item.Key);
                    };
                }
                Menus.Add(new() { Key = MN, Value = T.transform });
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitCategory(LocalizedString MN, LocalizedString BK, ForgeCategory item)
        {
            var T = Instantiate(MenuTemplate, MenuHolder);
            Menus.Add(new() { Key = item.Name, Value = T.transform });
            foreach (var obj in item.objects)
            {
                var B = Instantiate(ButtonTemplate, T);
                var _btn = B.GetComponent<UI.UIButton>();
                _btn.Content = obj.Name;
                _btn.OnClick = () =>
                {
                    MenuItemClick(obj);
                };
            }
            {
                var B = Instantiate(ButtonTemplate, T);
                var _btn = B.GetComponent<UI.UIButton>();
                _btn.Content = BK;
                _btn.OnClick = () =>
                {
                    ShowMenu(MN);
                };
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MenuItemClick(ForgeObject obj)
        {
            var r = ForgeCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            GameObject _obj;
            if (obj.Prefab.useString)
                _obj = ResourceBuilder.ObtainGameObject(obj.Prefab.Key);
            else
                _obj = ResourceBuilder.ObtainGameObject(obj.Prefab.ID);
            float Reach = 5;
            float.TryParse(PlacementRange.text, out Reach);
            if (Physics.Raycast(r, out var hit, Reach))
            {
                //Camera.main.transform.position+r.direction
                var L = (ForgeCam.transform.position - hit.collider.transform.position).magnitude;
                var p = _obj.GetComponent<EditablePhysics>();
                if (p != null)
                {
                    var S = p.ObtainSize();
                    var __P = ForgeCam.transform.position + r.direction * (L - S.magnitude);
                    SceneBuilder.NewObject(obj.Prefab, __P, Quaternion.identity);
                    Debug.Log(obj.Name + ":" + S);
                }
                else
                {
                    var __P = ForgeCam.transform.position + r.direction * (L);
                    SceneBuilder.NewObject(obj.Prefab, __P, Quaternion.identity);
                }
            }
            else
            {
                var __P = ForgeCam.transform.position + r.direction * Reach;
                SceneBuilder.NewObject(obj.Prefab, __P, Quaternion.identity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void HideAllMenus()
        {
            foreach (var item in Menus)
            {
                item.Value.gameObject.SetActive(false);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EditObject(EditableObject editable)
        {
            foreach (var editor in Editors)
            {
                editor.TargetEditor.gameObject.SetActive(false);
            }
            if (editable == null) return;
            foreach (var item in editable.EditableData.components)
            {
                Debug.Log(item.GetType().Name);
                foreach (var editor in Editors)
                {
                    if (item.GetType().Name == editor.TypeName)
                    {
                        editor.TargetEditor.gameObject.SetActive(true);
                        editor.TargetEditor.SetComponent(item);
                        break;
                    }
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator SpawnPlayer()
        {
            yield return null;
            {
                var PLAYER = GlobalBioController.CurrentGlobalBioController.Spawn("PLAYER", Vector3.zero, Vector3.zero);
                FPSC = PLAYER.GetComponentInChildren<FPSController>();
                Parent.RegisterRefresh(FPSC);
                FPSC.Parent = Parent;
                FPSC.Init();
                FPSC.gameObject.SetActive(false);
                FPSC.Disable();
            }
            yield return null;
            if (ForgeLocals.Instance != null)
            {
                if (ForgeLocals.Instance.isReadOnly)
                {
                    ToggleMonitor();
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ForceObjectMenuUpdate()
        {
            StartCoroutine(UIUpdate(ObjectMenu));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ForgeObject FindObject(PrefabReference prefab)
        {
            foreach (var cate in DataSet.ForgeCategories)
            {
                foreach (var obj in cate.objects)
                {
                    if (prefab.useString)
                    {
                        if (obj.Prefab.Key == prefab.Key)
                        {
                            return obj;
                        }
                    }
                    else
                    {
                        if (obj.Prefab.ID == prefab.ID)
                        {
                            return obj;
                        }
                    }
                }
            }
            return null;
        }
        bool __mouse_visible;
        CursorLockMode __lock_mode;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void TooglePauseMenu()
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);
            if (PauseMenu.activeSelf)
            {
                __mouse_visible = Cursor.visible;
                __lock_mode = Cursor.lockState;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.visible = __mouse_visible;
                Cursor.lockState = __lock_mode;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleMonitor()
        {
            if (ForgeController.transform.parent.gameObject.activeSelf)
            {
                FPSC.transform.position = ForgeController.transform.position;
                FPSC.transform.rotation = Quaternion.Euler(ForgeController.transform.rotation.eulerAngles);
                __mouse_visible = Cursor.visible;
                __lock_mode = Cursor.lockState;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                FPSC.gameObject.SetActive(true);
                FPSC.Enable();
                ForgeController.Disable();
                ForgeController.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Cursor.visible = __mouse_visible;
                Cursor.lockState = __lock_mode;
                ForgeController.transform.position = FPSC.transform.position;
                ForgeController.transform.rotation = Quaternion.Euler(FPSC.transform.rotation.eulerAngles);
                FPSC.gameObject.SetActive(false);
                FPSC.Disable();
                ForgeController.Enable();
                ForgeController.transform.parent.gameObject.SetActive(true);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (InputProcessor.GetInputDown("Esc"))
            {
                TooglePauseMenu();
            }
            if (ForgeLocals.Instance != null) if (ForgeLocals.Instance.isReadOnly) return;
            if (InputProcessor.GetInputDown("Detail"))
            {
                ToggleMonitor();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator UIUpdate(GameObject gameObject)
        {
            yield return null;
            var vlg = gameObject.GetComponent<VerticalLayoutGroup>();
            if (vlg != null) vlg.enabled = false;
            //gameObject.SetActive(false);
            yield return null;
            //gameObject.SetActive(true);
            if (vlg != null) vlg.enabled = true;
            LayoutRebuilder.MarkLayoutForRebuild(gameObject.transform as RectTransform);
            //if (vlg != null) vlg.CalculateLayoutInputVertical();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ShowMenu(LocalizedString target)
        {
            HideAllMenus();
            foreach (var item in Menus)
            {
                if (item.Key == target)
                {
                    Title.text = target;
                    item.Value.gameObject.SetActive(true);
                    var s = item.Value.GetChild(0).GetComponent<Selectable>();
                    if (s != null) s.Select();
                    return;
                }
            }
        }
    }
}