using Site13Kernel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UI.Customizations
{
    public class WeaponCustomizationMenu : MonoBehaviour
    {
        public UIButton BackButton;
        public GameObject ButtonPrefab;
        public Transform WeaponHolder;
        public Transform CoatingsHolder;
        public List<WeaponCoatingCollection> Weapons;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
    [Serializable]
    public class WeaponCoatingCollection
    {
        public PrefabReference Weapon;
        public List<WeaponCoating> Coatings= new List<WeaponCoating>();
    }

}
