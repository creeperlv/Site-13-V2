using Site13Kernel.Animations;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.Controls;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.GameLogic.Level;
using Site13Kernel.GameLogic.Physic;
using Site13Kernel.GameLogic.RuntimeScenes;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;

namespace Site13Kernel.GameLogic.Character
{
	public class BipedController : BasicController
	{
		public string BipedID;
		public ActiveInteractor Interactor;
		public BipedEntity Entity;
		public bool UseControlledBehaviorWorkflow;
		public CharacterController CC;
		public float NormalSkinWidth = 0.08f;
		public float CrouchSkinWidth = 0.008f;
		public WrappedAnimator ControlledAnimator;
		public float WalkFootStepMultiplier = 0.5f;
		public float SprintFootStepMultiplier = 0.75f;
		public float CrouchFootStepMultiplier = 0.15f;
		public float LandFootStepMultiplier = 0.85f;
		public float HeavyLandFootStepMultiplier = 0.9f;
		public float CurrentFootStepMultiplier = 0.5f;
		public AudioSource FootStepSoundSource;
		public ActionLock MovementLock = new ActionLock();
		public ActionLock CombatLock = new ActionLock();
		public float NormalCharacterHeight = 1.5f;
		public float NormalCharacterOffset_Y = 0.75f;
		public float CrouchCharacterHeight = 0.75f;
		public float CrouchCharacterOffset_Y = 0.325f;
		public float CharacterHeightChangeSpeed = 2f;
		public MoveState MoveState = MoveState.Walk;
		public int FireID = 100;
		public int MeleeID = 101;
		public float RunningJumpHeight = 1f;
		public float MouseVerticalIntensity = 1f;
		public float MouseHoriztonalIntensity = 1f;
		public float MaxV = 50;
		public float MinV = -50;
		public float WalkSpeed = 5f;
		public float MoveFriction = 9.8f;
		public float SprintMultiplyer = 1.5f;
		public float CrouchMultiplyer = 1.5f;
		public float SpeedMultiplyer = 1;
		public float Gravity = 9.8f;
		public float JumpForce = 10f;
		public float RunningJumpForce = 15f;
		public float DropThreshold = 0.5f;
		public CamPosTarget CamPosTarget;
		public bool SmoothCamFollow = false;
		public bool isPlayer;
		public bool IsFPSBiped;
		public GameObject FlashLightObject;
		public KVList<int , GameObject> Grenades = new KVList<int , GameObject>();
		public Dictionary<int , GameObject> _Grenades;
		float MH;
		float MV;
		float VR;
		float HR;
		/// <summary>
		/// Sprint Lock
		/// </summary>
		public bool ALLOW_FIRE_FLAG_0 = true;   //SPRINT RUN LOCK
		/// <summary>
		/// Pick-up and Take-out
		/// </summary>
		public bool ALLOW_FIRE_FLAG_1 = true;  // PICK UP ACTION LOCK
		/// <summary>
		/// Melee Action
		/// </summary>
		public bool ALLOW_FIRE_FLAG_2 = true;  // MELEE ACTION LOCK
		/// <summary>
		/// Reload Lock
		/// </summary>
		public bool ALLOW_FIRE_FLAG_3 = true;  // RELOAD LOCK
		/// <summary>
		/// Grenade Lock
		/// </summary>
		public bool ALLOW_FIRE_FLAG_4 = true;  // GRENADE LOCK
		/// <summary>
		/// Holding Object.
		/// </summary>
		public bool ALLOW_FIRE_FLAG_5 = true;
		/// <summary>
		/// Using Equipment.
		/// </summary>
		public bool ALLOW_FIRE_FLAG_6 = true;

		public bool IsInOverheat = false;
		public bool FLAG_IS_THROWING = false;
		Vector3 _MOVE;
		public AnimationCollection CurrentCollection;
		public AuxiliaryBipedControls ABC;
		public SimulatedRigidBodyOverCharacterController SRBoCC;
		#region Registry

		[Header("BipedShield")]
		public string ShieldKey = "MobileSRA";
		public string MaxShieldQuery = "MaxShield";
		public bool RegisteredShield;
		public float ShieldRecoverDelay = 5;
		public float ShieldRecoverSpeed = 15f;
		public float MaxShield = 100f;
		bool _ShieldEnabled;
		#endregion

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegistryRelated(float DeltaTime)
		{
			if (RegisteredShield)
			{
				var ShieldEnabled = LevelRuntimeRegistry.QueryBool(ShieldKey);

				if (ShieldEnabled != _ShieldEnabled)
				{
					if (ShieldEnabled)
					{
						MaxShield = LevelRuntimeRegistry.QueryFloat(MaxShieldQuery , MaxShield);
						Entity.ShieldRecoverDelay = ShieldRecoverDelay;
						Entity.ShieldRecoverSpeed = ShieldRecoverSpeed;
						Entity.MaxShield = MaxShield;
					}
					else
					{
						Entity.ShieldRecoverSpeed = 0;
						Entity.CurrentShield = 0;
					}
					_ShieldEnabled = ShieldEnabled;
				}
			}
		}
		public void Start()
		{
			if (UseControlledBehaviorWorkflow) return;
			__init();
		}
		public override void Init()
		{
			if (!UseControlledBehaviorWorkflow) return;
			__init();
		}
		void __init()
		{
			_Grenades = Grenades.ObtainMap();
			ControlledAnimator.ControlledAnimator.keepAnimatorStateOnDisable = true;
			Entity.EntityBag.OnObtainWeapon.Add((w) =>
			{
				w.OnSingleFire.Add(() =>
				{
					ControlledAnimator.SetTrigger("Fire");
					ControlledAnimator.LastTrigger = "";
				});
				w.OnOverheat.Add(OnOverheat);
				w.OnCooled.Add(OnCooled);
				w.isHoldByPlayer = isPlayer;
				ApplyWeapon();
				StartCoroutine(PlayPickup());
			});
			Entity.EntityBag.OnObtainHoldable.Add((h) =>
			{
				//TODO
				ALLOW_FIRE_FLAG_5 = false;
				ApplyHoldable();
				StartCoroutine(PlayPickup());
			});
			Entity.EntityBag.OnDropHoldable.Add((h) =>
			{
				if (FLAG_IS_THROWING) return;
				ALLOW_FIRE_FLAG_5 = true;
				ApplyWeapon();
				StartCoroutine(__TakeoutAnimation());
			});
			Entity.OnSwapWeapon.Add(() =>
			{
				ApplyWeapon();
			});
		}
		void ApplyHoldable()
		{
			StartCoroutine(DAH());
		}

		void OnOverheat()
		{
			ControlledAnimator.SetTrigger("Overheat");
		}
		void OnCooled()
		{
			ControlledAnimator.SetTrigger("Idle");
		}
		IEnumerator DAH()
		{
			yield return null;
			var CurrentMain = Entity.EntityBag.HoldableObject;
			ControlledAnimator.
				UseAnimationCollection(RuntimeAnimationResource.CachedResources [ BipedID ].Animations [ CurrentMain.TargetAnimationSetID ] , false);
			CurrentMain.transform.SetParent(Entity.WeaponHand);
			CurrentMain.transform.localPosition = Vector3.zero;
			CurrentMain.transform.localRotation = Quaternion.identity;
			CurrentMain.transform.localScale = Vector3.one;
		}
		IEnumerator PlayPickup()
		{
			ALLOW_FIRE_FLAG_1 = false;
			yield return null;
			yield return null;
			var col = ControlledAnimator.SetTrigger("Pickup");
			if (col != null)
			{
				if (col.Length >= 0)
					yield return new WaitForSeconds(col.Length);
				else yield return null;
			}
			else yield return null;
			ControlledAnimator.LastTrigger = "";
			ALLOW_FIRE_FLAG_1 = true;
		}
		void ApplyWeapon()
		{
			StartCoroutine(DelayedApplyWeapon());
		}
		IEnumerator DelayedApplyWeapon()
		{
			yield return null;

			GenericWeapon CurrentMain = Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ];
			GenericWeapon Side = null;
			if (Entity.EntityBag.Weapons.Count > 1)
			{
				Side = Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon == 1 ? 0 : 1 ];
			}
#if DEBUG
			Debug.Log("Biped Apply Weapon.");
#endif
			ControlledAnimator.UseAnimationCollection(RuntimeAnimationResource.CachedResources [ BipedID ].Animations [ CurrentMain.AnimationCollectionName ] , false);
			CurrentMain.transform.SetParent(Entity.WeaponHand);
			CurrentMain.transform.localPosition = Vector3.zero;
			CurrentMain.transform.localRotation = Quaternion.identity;
			CurrentMain.transform.localScale = Vector3.one;
			if (Side != null)
			{
				Side.transform.SetParent(Entity.Weapon1);
				Side.transform.localPosition = Vector3.zero;
				Side.transform.localRotation = Quaternion.identity;
				Side.transform.localScale = Vector3.one;
			}
		}
		public void PlayWeaponReloadSound()
		{
			if (Entity.EntityBag.Weapons.Count > Entity.EntityBag.CurrentWeapon)
				Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].PlayReloadSound();
		}
		public void PlayWeaponReloadSoundWithEmpty()
		{
			if (Entity.EntityBag.Weapons.Count > Entity.EntityBag.CurrentWeapon)
				Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].PlayReloadSoundWithEmpty();
		}
		public void PlayWeaponChamberingAnimation()
		{
			if (Entity.EntityBag.Weapons.Count > Entity.EntityBag.CurrentWeapon)
				Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].PlayWeaponChamberingAnimation();
		}
		public void PlayWeaponChamberingFromEmptyAnimation()
		{
			if (Entity.EntityBag.Weapons.Count > Entity.EntityBag.CurrentWeapon)
				Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].PlayWeaponChamberingFromEmptyAnimation();
		}
		public override void Move(Vector2 Movement , float DeltaTime)
		{
			MV = Movement.x;
			MH = Movement.y;
		}
		public override void HorizontalRotation(float Angle)
		{
			HR = Angle;
		}
		public override void Reload()
		{
			CancelRun();
			CancelZoom();
			if (!ALLOW_FIRE_FLAG_5) return;
			if (!ALLOW_FIRE_FLAG_0) return;
			if (!ALLOW_FIRE_FLAG_1) return;
			if (!ALLOW_FIRE_FLAG_2) return;
			if (!ALLOW_FIRE_FLAG_3) return;
			if (IsInOverheat) return;
			StartCoroutine(ReloadProcess());
		}
		IEnumerator ReloadProcess()
		{
			if (Entity.EntityBag.Weapons.Count == 0)
			{
				yield break;
			}
			if (Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].WeaponData.CurrentBackup <= 0) yield break;
			if (Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].WeaponData.CurrentMagazine >=
				Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].WeaponData.MagazineCapacity) yield break;
			ALLOW_FIRE_FLAG_3 = false;
			Site13AnimationClip clip;
			if (Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].WeaponData.CurrentMagazine == 0)
				clip = ControlledAnimator.SetTrigger("Reload-Empty");
			else
				clip = ControlledAnimator.SetTrigger("Reload");
			yield return new WaitForSeconds(clip.Length);
			ControlledAnimator.LastTrigger = "";
			Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].Reload();
			ALLOW_FIRE_FLAG_3 = true;
		}
		public override void VerticalRotation(float Angle)
		{
			VR = Angle;
		}
		public int CurrentStandingMaterial = 0;
		float VolumeMultiplier = 1;
		public void PlayFootstep()
		{
			if (Physics.Raycast(CC.transform.position + new Vector3(0 , (SpeedMultiplyer == CrouchMultiplyer ? CrouchCharacterOffset_Y : NormalCharacterOffset_Y) , 0) , Vector3.down , out RaycastHit hit , 5 , GameRuntime.CurrentGlobals.LayerExcludePlayerAndAirBlock , QueryTriggerInteraction.Collide))
			{
				var SM = hit.collider.GetComponent<PhysicalMaterial>();
				if (SM != null)
				{
					CurrentStandingMaterial = SM.MaterialID;
					VolumeMultiplier = SoundResources.FindMultiplier(CurrentStandingMaterial);
				}
			}
			if (FootStepSoundSource != null)
			{
				FootStepSoundSource.Stop();
				FootStepSoundSource.volume = VolumeMultiplier * CurrentFootStepMultiplier;
				FootStepSoundSource.clip = SoundResources.ObtainOneClip(CurrentStandingMaterial);
				FootStepSoundSource.Play();
			}
			if (Entity.isTookControl)
			{
				if (SpeedMultiplyer == SprintMultiplyer)
					ABC.ShakeCamRunStep();
				if (SpeedMultiplyer == 1)
					ABC.ShakeCamWalkStep();
			}
		}
		public override void Melee()
		{
			if (SpeedMultiplyer == SprintMultiplyer)
				CancelRun();
			CancelZoom();
			if (!ALLOW_FIRE_FLAG_1) return;
			if (!ALLOW_FIRE_FLAG_2) return;
			var Clip = this.ControlledAnimator.SetTrigger("Melee");
			StartCoroutine(MeleeProcess(Clip));
		}
		public IEnumerator MeleeProcess(Site13AnimationClip clip)
		{
			ALLOW_FIRE_FLAG_2 = false;
			yield return new WaitForSeconds(clip.Length);
			ControlledAnimator.LastTrigger = "";
			ALLOW_FIRE_FLAG_2 = true;
		}
		bool __try_jump = false;
		public override void Jump()
		{
			__try_jump = true;
		}
		public override void UseEquipment()
		{

		}
		public override void Inspect()
		{
			CancelRun();
			CancelAim();
			CancelFire();
			CancelZoom();
			var interative = LevelRuntimeRegistry.QueryBool("IsInspIUI" , false);
			if (interative)
			{
				GenericInputControl.Instance.isUIControl = true;
			}
		}
		public override void ThrowGrenade()
		{
			CancelRun();
			CancelZoom();
			if ((ALLOW_FIRE_FLAG_1 && ALLOW_FIRE_FLAG_2 && ALLOW_FIRE_FLAG_3 && ALLOW_FIRE_FLAG_4) == false)
			{
				return;
			}
			if (Entity.EntityBag.Grenades [ Entity.EntityBag.CurrentGrenade ].RemainingCount > 0)
				StartCoroutine(TG_Animation());
		}
		IEnumerator TG_Animation()
		{
			foreach (var item in _Grenades)
			{
				if (item.Key == Entity.EntityBag.CurrentGrenade)
				{
					item.Value.SetActive(true);
				}
				else
				{
					item.Value.SetActive(false);
				}
			}
			ALLOW_FIRE_FLAG_4 = false;
			var c = ControlledAnimator.SetTrigger("Throw-Grenade");
			yield return new WaitForSeconds(c.Length);
			ControlledAnimator.LastTrigger = "";
			ALLOW_FIRE_FLAG_4 = true;

		}
		public override bool isRunning => SpeedMultiplyer == SprintMultiplyer;
		public override void Zoom()
		{
			if (Zoomed) return;
			Zoomed = true;
			if (SpeedMultiplyer == SprintMultiplyer)
				CancelRun();
			if (!ALLOW_FIRE_FLAG_5) return;
			if (!ALLOW_FIRE_FLAG_1) return;
			if (!ALLOW_FIRE_FLAG_2) return;
			if (!ALLOW_FIRE_FLAG_0) return;
			if (!ALLOW_FIRE_FLAG_3) return;
			if (Entity.EntityBag.Weapons.Count > 0)
			{
				Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].AimingMode = 1;
				MainCamera.Instance.TargetScale = Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].ZoomScale;
				WeaponLayerCamera.Instance.TargetScale = 2;
				if (IsFPSBiped)
				{
					if (!Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].ShowWeaponCamInZoom)

						if (WeaponLayerCamera.Instance.RealCamera.enabled == true)
							WeaponLayerCamera.Instance.RealCamera.enabled = false;
					return;
				}
			}
			ControlledAnimator.SetTrigger("Zoom");
		}
		bool Zoomed = false;
		public override bool isAiming => Zoomed;
		public override void CancelZoom()
		{
			if (Zoomed == false) return;
			Zoomed = false;
			if (Entity.EntityBag.Weapons.Count > 0)
			{
				Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].AimingMode = 0;
				MainCamera.Instance.TargetScale = 1;
				WeaponLayerCamera.Instance.TargetScale = 1;
			}
			if (IsFPSBiped)
			{
				if (WeaponLayerCamera.Instance.RealCamera.enabled == false)
					WeaponLayerCamera.Instance.RealCamera.enabled = true;
				return;
			}
			ControlledAnimator.SetTrigger("CancelZoom");
		}
		public override void SwitchWeapon()
		{
			if (!ALLOW_FIRE_FLAG_5)
			{
				Entity.EntityBag.DropHoldable(Entity.EntityBag.HoldableObject , true);
				return;
			}
			CancelRun();
			CancelZoom();
			if (Entity.EntityBag.IsHoldingObject)
			{
				Entity.OnDropHoldable.Invoke();
				return;
			}
			if (Entity.EntityBag.Weapons.Count == 1)
				return;
			if (Entity.EntityBag.Weapons.Count > 1)
				Entity.EntityBag.CurrentWeapon = (Entity.EntityBag.CurrentWeapon == 1 ? 0 : 1);
			Entity.OnSwapWeapon.Invoke();
			ControlledAnimator.LastTrigger = "";
			ControlledAnimator.CurrentClip.WaitUntilDone = false;
			StartCoroutine(__TakeoutAnimation());
		}
		IEnumerator __TakeoutAnimation()
		{
			ALLOW_FIRE_FLAG_1 = false;
			yield return null;
			var clip = ControlledAnimator.SetTrigger("Takeout");
			if (clip.Length > 0)
				clip.WaitUntilDone = true;
			yield return new WaitForSeconds(clip.Length);
			ControlledAnimator.LastTrigger = "";
			ALLOW_FIRE_FLAG_1 = true;

		}
		public override void FlashLight()
		{
			FlashLightObject.SetActive(!FlashLightObject.activeSelf);
		}
		public override void Run()
		{
			CancelZoom();
			if (!ALLOW_FIRE_FLAG_5) { if (!Entity.EntityBag.HoldableObject.AllowRun) return; }
			if (ALLOW_FIRE_FLAG_1 == false) return;
			if (ALLOW_FIRE_FLAG_2 == false) return;
			if (ALLOW_FIRE_FLAG_3 == false) return;
			if (ALLOW_FIRE_FLAG_4 == false) return;
			//ControlledAnimator.SetTrigger("FSMR-SM2");
			//ControlledAnimator.SetTrigger("Run");
			SpeedMultiplyer = SprintMultiplyer;
			ALLOW_FIRE_FLAG_0 = false;
		}
		public override void CancelRun()
		{
			//ControlledAnimator.SetTrigger("Idle");
			ALLOW_FIRE_FLAG_0 = true;
			if (SpeedMultiplyer == SprintMultiplyer)
				SpeedMultiplyer = 1;
		}
		public override bool isCrouching =>
			SpeedMultiplyer == CrouchMultiplyer;
		public override void Crouch()
		{
			//CancelRun();
			SpeedMultiplyer = CrouchMultiplyer;
		}
		public override void CancelCrouch()
		{
			SpeedMultiplyer = 1;
		}
		IEnumerator __ThrowOutAnimation()
		{
			FLAG_IS_THROWING = true;
			//Debug.Log("Throwing out...");
			yield return null;
			var c = ControlledAnimator.SetTrigger("Throw-Out");
			yield return new WaitForSeconds(c.Length);
			ALLOW_FIRE_FLAG_5 = true;
			yield return null;
			ApplyWeapon();
			StartCoroutine(__TakeoutAnimation());
			FLAG_IS_THROWING = false;
		}
		public bool Fire = false;
		public override void StartFire()
		{
			CancelRun();
			if (!ALLOW_FIRE_FLAG_5)
			{
				if (FLAG_IS_THROWING) return;
				if (Entity.EntityBag.HoldableObject.CanThrowOut)
				{
					StartCoroutine(__ThrowOutAnimation());
				}
				return;
			}
			if (!ALLOW_FIRE_FLAG_0) return;
			if (!ALLOW_FIRE_FLAG_1) return;
			if (!ALLOW_FIRE_FLAG_2) return;
			if (!ALLOW_FIRE_FLAG_3) return;
			if (!ALLOW_FIRE_FLAG_6) return;
			if (!Fire)
			{
				if (Entity.EntityBag.Weapons.Count >= Entity.EntityBag.CurrentWeapon + 1)
					if (Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].WeaponData.CurrentMagazine > 0)
						Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].Fire();
				Fire = true;
			}
		}
		public override void CancelFire()
		{
			if (Fire)
			{
				if (Entity.EntityBag.Weapons.Count >= Entity.EntityBag.CurrentWeapon + 1)
					Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ].Unfire();
				Fire = false;
			}
		}
		public override void Interact()
		{
			if (ALLOW_FIRE_FLAG_1)
				Interactor.Interact();
		}
		public override void CancelInteract()
		{
			Interactor.UnInteract();
		}
		public void Update()
		{
			if (UseControlledBehaviorWorkflow)
			{
				return;
			}
			{
				float DT = Time.deltaTime;
				float UDT = Time.unscaledDeltaTime;
				OnFrame(DT , UDT);
			}
		}
		public override void SwitchGrenade()
		{
			if (ALLOW_FIRE_FLAG_4 == false) return;

			var K = Entity.EntityBag.Grenades.Keys.ToList();
			var i = K.IndexOf(Entity.EntityBag.CurrentGrenade);
			if (i + 1 < K.Count)
			{
				Entity.EntityBag.CurrentGrenade = K [ i + 1 ];
			}
			else
			{
				Entity.EntityBag.CurrentGrenade = K [ 0 ];
			}
		}

		public override void Refresh(float DeltaTime , float UnscaledDeltaTime)
		{
			if (UseControlledBehaviorWorkflow)
			{
				OnFrame(DeltaTime , UnscaledDeltaTime);
			}
		}
		public override void SwitchEquipment()
		{
			var K = EquipmentManifest.Instance.EqupimentMap.Keys.ToList();
			var i = K.IndexOf(Entity.EntityBag.CurrentEquipment);
			if (i + 1 < K.Count)
			{
				Entity.EntityBag.CurrentEquipment = K [ i + 1 ];
			}
			else
			{
				Entity.EntityBag.CurrentEquipment = K [ 0 ];
			}
		}
		internal bool isInRange;
		void Rotation(float DT)
		{
			float RotationIntensity = 1;
			if (this.Entity.isTookControl)
			{
				isInRange = false;
				var trans = this.Entity.FirePoint.transform;
				if (Entity.EntityBag.Weapons.Count > 0)
				{
					var WEAPON = Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ];
					var D = WEAPON.AimAssistDistance;
					var P = trans.position;
					var A = (WEAPON.AimingMode == 0 ? WEAPON.MaxScatterAngle : WEAPON.MaxScatterAngleAimMode);
					var R = WEAPON.AimAssistRotationResistance;
					Vector3 V = trans.forward;
					for (int i = 0 ; i < 20 ; i++)
					{
						Vector3 RecoilAngle2 =
							MathUtilities.RandomDirectionAngleOnXYAndZ1(A);

						var _V = V + trans.TransformDirection(RecoilAngle2);
						var p = Physics.Raycast(P , _V , out var info , D , GameRuntime.CurrentGlobals.LayerExcludePlayerAndAirBlockAndEventTrigger , QueryTriggerInteraction.Ignore);
						if (p)
						{
							var ent = info.collider.GetComponentInChildren<BioEntity>();
							var weak = info.collider.GetComponentInChildren<WeakPoint>();
							var reference = info.collider.GetComponentInChildren<DamagableEntityReference>();
							if (reference != null)
							{
								if (reference.Reference is BioEntity e)
								{
									ent = e;
								}
							}
							if (weak != null)
							{
								ent = weak.AttachedBioEntity;
							}
							if (ent != null)
							{
								{
									isInRange = true;
									RotationIntensity = R;
									break;
								}
							}
						}
					}
				}
				else
				{
					var P = trans.position;
					Vector3 V = trans.forward;
					var p = Physics.Raycast(P , V , out var info , 50 , GameRuntime.CurrentGlobals.LayerExcludePlayerAndAirBlockAndEventTrigger , QueryTriggerInteraction.Ignore);
					if (p)
					{
						var ent = info.collider.GetComponentInChildren<BioEntity>();
						var weak = info.collider.GetComponentInChildren<WeakPoint>();
						var reference = info.collider.GetComponentInChildren<DamagableEntityReference>();
						if (reference != null)
						{
							if (reference.Reference is BioEntity e)
							{
								ent = e;
							}
						}
						if (weak != null)
						{
							ent = weak.AttachedBioEntity;
						}
						if (ent != null)
						{
							{
								isInRange = true;
								RotationIntensity = 0.9f;
							}
						}
					}
				}
			}
			HorizontalTransform.Rotate(new Vector3(0 , HR * MouseHoriztonalIntensity * RotationIntensity * DT * Data.Settings.CurrentSettings.MouseSensibly , 0));
			var Head_V = VR * MouseHoriztonalIntensity * RotationIntensity * DT * Data.Settings.CurrentSettings.MouseSensibly;
			var ea = VerticalTransform.localEulerAngles;
			ea.x += Head_V;
			if (ea.x < 180)
			{
				ea.x = Mathf.Clamp(ea.x , MinV , MaxV);
			}
			else
			{
				ea.x = Mathf.Clamp(ea.x , 360 + MinV , 360);

			}
			VerticalTransform.localEulerAngles = ea;
		}
		public float DropTime = 0;
		public float ActiveMovementTime;
		public float WalkInteval;
		public float RunInteval;
		public float CrouchInteval;
		void Move(float DT)
		{
			if (CC.isGrounded)
			{
				if (MV == 0 && MH == 0)
				{
					_MOVE -= _MOVE * MoveFriction * DT;
					if (_MOVE.magnitude <= 0.03f)
					{
						_MOVE = Vector3.zero;

						_MOVE.y = -Gravity;
					}
				}
				else
				{
					var m = (CC.transform.right * (MH * math.sqrt(1 - (MV * MV) * .5f)) + CC.transform.forward * (MV * math.sqrt(1 - (MH * MH) * .5f)));
					m *= WalkSpeed;
					m *= SpeedMultiplyer;
					//_MOVE.y = -Gravity;
					m.y = _MOVE.y;
					_MOVE = m;
				}
				if (__try_jump)
				{
					_MOVE.y = (SpeedMultiplyer == SprintMultiplyer ? RunningJumpForce : JumpForce);
					__try_jump = false;
				}
				else
				{

				}
			}
			else
			{

				_MOVE.y -= Gravity * DT;
				__try_jump = false;
			}

			if (CC.isGrounded)
			{
				if (DropTime >= DropThreshold)
				{
					{
						CurrentFootStepMultiplier = HeavyLandFootStepMultiplier;
						PlayFootstep();
						ControlledAnimator.SetTrigger("HeavyLand");
					}
				}
				else if (DropTime >= 0.1f)
				{
					CurrentFootStepMultiplier = LandFootStepMultiplier;
					PlayFootstep();
				}
				DropTime = 0;
			}
			else
			{
				if (DropTime >= 0.1f)
				{
					if (IsInOverheat)
					{

						ControlledAnimator.SetTrigger("Overheat");
					}
					else
						ControlledAnimator.SetTrigger("Idle");
				}
				DropTime += DT;
			}
			CC.Move(_MOVE * DT);

			if ((new Vector2(CC.velocity.x , CC.velocity.z)).sqrMagnitude > 0.1f)
			{
				if (CC.isGrounded)
					if (MH != 0 || MV != 0)
					{
						ActiveMovementTime += DT;

						if (SpeedMultiplyer == SprintMultiplyer)
						{
							if (ActiveMovementTime >= RunInteval)
							{
								CurrentFootStepMultiplier = SprintFootStepMultiplier;
								ActiveMovementTime = 0;
								if (Entity.RoatationSync != null)
									Entity.RoatationSync.Sync();
								PlayFootstep();
							}
						}
						else if (SpeedMultiplyer == 1)
						{
							if (ActiveMovementTime >= WalkInteval)
							{
								CurrentFootStepMultiplier = WalkFootStepMultiplier;
								ActiveMovementTime = 0;
								if (Entity.RoatationSync != null)
									Entity.RoatationSync.Sync();
								PlayFootstep();
							}
						}
						else
						{
							if (ActiveMovementTime >= CrouchInteval)
							{
								CurrentFootStepMultiplier = CrouchFootStepMultiplier;
								ActiveMovementTime = 0;
								if (Entity.RoatationSync != null)
									Entity.RoatationSync.Sync();
								PlayFootstep();
							}
						}
						switch (ControlledAnimator.LastTrigger)
						{
							case "Run":
							case "Walk":
							case "Idle":
							case "Crouch":
							case "HeavyLand":
							case "Pickup":
							case "Overheat":
							case "":
								{
									if (SpeedMultiplyer == SprintMultiplyer)
									{
										//Running.
										ControlledAnimator.SetTrigger("Run");
									}
									else
									if (SpeedMultiplyer == 1)
									{
										//Running.
										if (IsInOverheat)
										{
											ControlledAnimator.SetTrigger("Overheat");

										}
										else
											ControlledAnimator.SetTrigger("Walk");
									}
									else
									{
										ControlledAnimator.SetTrigger("Crouch");
									}
								}
								break;
							default:
								break;
						}
					}
			}
			else
			{
				switch (ControlledAnimator.LastTrigger)
				{
					case "Run":
					case "Crouch":
					case "Walk":
						ControlledAnimator.SetTrigger("Idle");
						break;
					default:
						break;
				}
			}
		}
		void _Crouch(float DT)
		{
			if (SpeedMultiplyer == CrouchMultiplyer)
			{
				if (CC.height > CrouchCharacterHeight)
				{
					if (CC.skinWidth != CrouchSkinWidth)
						CC.skinWidth = CrouchSkinWidth;

					float delta = (CrouchCharacterHeight - CC.height);
					if (Mathf.Abs(delta) <= 0.02f)
					{
						CC.height = CrouchCharacterHeight;
					}
					else
						CC.height += delta * CharacterHeightChangeSpeed * DT;
				}
				else
				{
					if (CC.skinWidth != NormalSkinWidth)
						CC.skinWidth = NormalSkinWidth;

				}
				if (CC.center.y != CrouchCharacterOffset_Y)
				{
					var c = CC.center;
					c.y += (CrouchCharacterOffset_Y - c.y) * CharacterHeightChangeSpeed * DT;
					CC.center = c;
				}
			}
			else
			{
				if (CC.height < NormalCharacterHeight)
				{
					if (CC.skinWidth != CrouchSkinWidth)
						CC.skinWidth = CrouchSkinWidth;
					float delta = (NormalCharacterHeight - CC.height);
					if (Mathf.Abs(delta) <= 0.02f)
					{
						CC.height = NormalCharacterHeight;
					}
					else
						CC.height += delta * CharacterHeightChangeSpeed * DT;
				}
				else
				{
					if (CC.skinWidth != NormalSkinWidth)
						CC.skinWidth = NormalSkinWidth;

				}
				if (CC.center.y != NormalCharacterOffset_Y)
				{
					var c = CC.center;
					c.y += (NormalCharacterOffset_Y - c.y) * CharacterHeightChangeSpeed * DT;
					CC.center = c;
				}
			}
		}
		public void OnFrame(float DT , float UDT)
		{
			{
				if (Entity.EntityBag.Weapons.Count > 0)
					if (Entity.EntityBag.Weapons.Count >= Entity.EntityBag.CurrentWeapon)
					{
						var w = Entity.EntityBag.Weapons [ Entity.EntityBag.CurrentWeapon ];
						IsInOverheat = w.WeaponMode == WeaponConstants.WEAPON_MODE_OVERHEAT;
					}
				_Crouch(DT);
				Rotation(DT);
				Move(DT);
				SRBoCC.Refresh(DT , UDT);
			}
			{
				ControlledAnimator.AccumulativeTrigger(DT);
			}
		}
	}
}
