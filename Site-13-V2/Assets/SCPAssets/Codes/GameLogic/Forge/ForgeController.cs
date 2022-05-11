using Site13Kernel.Core;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.SceneBuild;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.GameLogic.Forge
{
    public class ForgeController : ControlledBehavior
    {
        public float MoveSpeed;
        public float RunMultiplier;
        public float MoveFriction;
        public CharacterController cc;
        public Transform Head;
        public Camera MainCam;
        public float MouseHoriztonalIntensity = 1f;
        Vector3 _MOVE = Vector3.zero;
        public float MaxV;
        public float MinV;
        public MonitorWorkingMode WorkingMode = MonitorWorkingMode.FreeMove;
        public ForgeSystem CurrentSystem;
        [Header("UI")]
        public Image HandIcon;
        public Material NormalHand;
        public Material GreenHand;
        bool HandStatus = false;
        public GameObject MovingObejct;
        public List<Text> SelectionNames;
        [Header("Menus")]
        public GameObject BuildMenu;
        public GameObject ObjectMenu;
        public float MovingSoundMaxVolume = 0.5f;
        public AudioSource MovingSound;
        public float MovingSoundMaxVolumeSpeed = 3f;
        bool Interrupt00 = false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Init()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Parent.RegisterRefresh(this);
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (Interrupt00) return;
            ObjectDetection();
            if (GameRuntime.CurrentGlobals.isPaused) return;
            Movement(DeltaTime);
            MoveSFX();
            Menu();
            Drag();

            Deletion();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Deletion()
        {
            if (WorkingMode == MonitorWorkingMode.FreeMove)
            {
                if (SelectedObject != null)
                {
                    if (InputProcessor.GetInputDown("Combat"))
                    {
                        Destroy(SelectedObject.gameObject);
                    }
                }
            }
        }

        Vector3 Delta = Vector3.zero;
        bool F_0 = false;
        bool r_0 = true;
        bool r_1 = true;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Drag()
        {
            if (InputProcessor.GetAxis("Fire") > 0.5f)
            {
                if (WorkingMode == MonitorWorkingMode.FreeMove)
                {
                    //if (!F_0)
                    {
                        F_0 = true;
                        if (SelectedObject != null)
                        {
                            float.TryParse(CurrentSystem.AngleSnap.text, out AngleSnap);
                            WorkingMode = MonitorWorkingMode.Drag;
                            var r = SelectedObject.GetComponent<Rigidbody>();
                            if (r != null)
                            {
                                r.Sleep();
                            }
                            Delta = SelectedObject.transform.position - Camera.main.transform.position;
                        }
                    }
                }
            }
            else
            {
                //if (F_0)
                {
                    if (WorkingMode == MonitorWorkingMode.Drag)
                    {
                        F_0 = false;
                        if (SelectedObject != null)
                        {
                            var r = SelectedObject.GetComponent<Rigidbody>();
                            if (r != null)
                            {
                                r.Sleep();
                            }
                        }
                        WorkingMode = MonitorWorkingMode.FreeMove;
                    }
                }
            }
            if (WorkingMode == MonitorWorkingMode.Drag)
            {
                SelectedObject.transform.position = Camera.main.transform.position + Delta;

                {
                    var H = InputProcessor.GetAxis("MouseH");
                    var V = InputProcessor.GetAxis("MouseV");
                    if (math.abs(H) > math.abs(V))
                    {
                        if (math.abs(H) > 1)
                        {
                            if (r_0)
                            {
                                r_0 = false;
                                SelectedObject.transform.Rotate(new Vector3(0, AngleSnap * (H > 0 ? 1 : -1), 0));
                            }
                        }
                        else r_0 = true; r_1 = true;
                    }
                    else
                    {
                        if (math.abs(V) > 1)
                        {
                            if (r_1)
                            {
                                r_1 = false;
                                SelectedObject.transform.Rotate(new Vector3(AngleSnap * (V > 0 ? 1 : -1), 0, 0));
                            }
                        }
                        else r_1 = true; r_0 = true;
                    }
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MoveSFX()
        {
            if (_MOVE != Vector3.zero)
            {
                if (MovingObejct.activeSelf == false)
                    MovingObejct.SetActive(true);
            }
            else
            {
                if (MovingObejct.activeSelf == true)
                    MovingObejct.SetActive(false);
            }
        }
        EditableObject SelectedObject = null;
        float AngleSnap = 1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ObjectDetection()
        {
            var r = MainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            bool _hit = false;

            if (WorkingMode != MonitorWorkingMode.FreeMove) return;
            if (Physics.Raycast(r, out var hit, 25))
            {
                if (hit.collider.gameObject != null)
                {
                    var EO = hit.collider.gameObject.GetComponent<EditableObject>();
                    if (EO != null)
                    {
                        _hit = true;
                        SelectedObject = EO;
                    }
                    else
                    {
                        var REO = hit.collider.gameObject.GetComponent<ReferenceEditableObject>();
                        if (REO != null)
                        {
                            _hit = true;
                            SelectedObject = REO.ReferedObject;
                        }
                    }
                }
            }
            if (_hit)
            {
                {
                    var fo = CurrentSystem.FindObject(SelectedObject.EditableData.Reference);
                    if (fo != null)
                        foreach (var item in SelectionNames)
                        {
                            item.text = fo.Name;
                        }
                }
                if (!HandStatus)
                {
                    HandIcon.material = GreenHand;
                    HandStatus = true;
                }
            }
            else
            {
                {
                    foreach (var item in SelectionNames)
                    {
                        item.text = "";
                    }
                }
                if (HandStatus)
                {

                    HandIcon.material = NormalHand;
                    HandStatus = false;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Menu()
        {
            if (InputProcessor.GetInputDown("Zoom"))
            {
                if (WorkingMode != MonitorWorkingMode.NewObject)
                {
                    BuildMenu.SetActive(true);
                    WorkingMode = MonitorWorkingMode.NewObject;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    foreach (var item in CurrentSystem.Menus)
                    {
                        if (item.Value.gameObject.activeSelf)
                        {
                            var S = item.Value.GetChild(0).GetComponent<Selectable>();
                            if (S != null) S.Select();
                            break;
                        }
                    }
                }
                else
                {
                    WorkingMode = MonitorWorkingMode.FreeMove;
                    BuildMenu.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            if (InputProcessor.GetInputDown("Interact"))
            {
                if (SelectedObject != null)
                {
                    if (WorkingMode != MonitorWorkingMode.ObjectOriented)
                    {
                        ObjectMenu.SetActive(true);
                        if (SelectedObject != null)
                            CurrentSystem.EditObject(SelectedObject);
                        WorkingMode = MonitorWorkingMode.ObjectOriented;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                    else
                    {
                        WorkingMode = MonitorWorkingMode.FreeMove;
                        ObjectMenu.SetActive(false);
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                }
            }
            switch (WorkingMode)
            {
                case MonitorWorkingMode.FreeMove:
                    {
                        if (BuildMenu.activeSelf)
                            BuildMenu.SetActive(false);
                        if (ObjectMenu.activeSelf)
                            ObjectMenu.SetActive(false);
                    }
                    break;
                case MonitorWorkingMode.ObjectOriented:
                    {
                        if (BuildMenu.activeSelf)
                            BuildMenu.SetActive(false);
                        if (!ObjectMenu.activeSelf)
                            ObjectMenu.SetActive(true);
                    }
                    break;
                case MonitorWorkingMode.NewObject:
                    {
                        if (ObjectMenu.activeSelf)
                            ObjectMenu.SetActive(false);
                        if (!BuildMenu.activeSelf)
                            BuildMenu.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Movement(float DeltaTime)
        {
            {
                var MV = 0f;
                var MH = 0f;
                bool Up = false;
                bool Down = false;
                bool Run = false;
                if (WorkingMode == MonitorWorkingMode.FreeMove || WorkingMode == MonitorWorkingMode.Drag)
                {
                    Run = InputProcessor.GetInput("Run");
                    Up = InputProcessor.GetInput("Jump");
                    Down = InputProcessor.GetInput("Crouch");
                    MV = InputProcessor.GetAxis("MoveVertical");
                    MH = InputProcessor.GetAxis("MoveHorizontal");
                }
                var V = new Vector3(MH, 0, MV);
                if (MV == 0 && MH == 0)
                {
                    _MOVE -= _MOVE * MoveFriction * DeltaTime;
                    if (_MOVE.magnitude <= 0.15f)
                    {
                        _MOVE = Vector3.zero;
                    }
                }
                else
                {
                    _MOVE = cc.transform.right * (MH * math.sqrt(1 - (MV * MV) * .5f)) + cc.transform.forward * (MV * math.sqrt(1 - (MH * MH) * .5f));
                }
                if (Up)
                {
                    _MOVE.y = 1;
                }
                if (Down)
                {
                    _MOVE.y = -1;
                }
                var D = _MOVE * (Run ? RunMultiplier : 1);
                MovingSound.volume = MovingSoundMaxVolume * Mathf.InverseLerp(0, MovingSoundMaxVolumeSpeed, D.magnitude);
                cc.Move(_MOVE * (Run ? RunMultiplier : 1) * DeltaTime);
            }
            if (WorkingMode == MonitorWorkingMode.FreeMove)
                CamRotation(DeltaTime);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable()
        {
            Interrupt00 = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable()
        {
            Interrupt00 = false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CamRotation(float DeltaTime)
        {
            {
                //View rotation
                cc.transform.Rotate(0, InputProcessor.GetAxis("MouseH") * MouseHoriztonalIntensity * DeltaTime, 0);
                var Head_V = InputProcessor.GetAxis("MouseV") * MouseHoriztonalIntensity * DeltaTime;
                var ea = Head.localEulerAngles;
                ea.x += Head_V;
                if (ea.x < 180)
                {
                    ea.x = Mathf.Clamp(ea.x, MinV, MaxV);
                }
                else
                {
                    ea.x = Mathf.Clamp(ea.x, 360 + MinV, 360);

                }
                Head.localEulerAngles = ea;
            }

        }
    }
}