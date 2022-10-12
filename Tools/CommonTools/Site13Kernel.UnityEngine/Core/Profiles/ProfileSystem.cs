using Site13Kernel.Data;
using Site13Kernel.IO;
using Site13Kernel.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Core.Profiles
{
    public class ProfileSystem : ControlledBehavior
    {
        public enum SetActiveProfileResult
        {
            SUCCESS_DIRECT_SET, FAILE_PROFILE_NOT_FOUND, SUCCESS_OVERWRITE
        }
        internal readonly static Guid TrustedInstallerID = new Guid('S', (short)'P', (short)'T', (byte)'N',
            (byte)'J', (byte)'O', (byte)'H', (byte)'N', 1, 1, 7);
        public readonly static Profile TrustedInstaller = new Profile
        {
            PlayerID = TrustedInstallerID,
            PlayerName = "TrustedInstaller"
        };
        public List<Profile> ExistingProfiles = new List<Profile>();
        public bool EnableTrustedInstaller = true;
        public static ProfileSystem Instance;
        public static Profile ActiveID = new Profile() { PlayerID = Guid.Empty };
        public bool IsProfileListEmpty = false;
        public override void Init()
        {
            Instance = this;
            ProfileRoot = ApplicationData.LocalFolder.CreateOrOpenFolder("Profiles");
            LoadProfileList();
            if (EnableTrustedInstaller)
                ExistingProfiles.Add(TrustedInstaller);
        }
        public static StorageFolder GetActiveProfileRoot()
        {
            return GetProfileRoot(ActiveID);
        }
        public static StorageFolder GetProfileRoot(Profile profile)
        {
            return Instance.ProfileRoot.CreateOrOpenFolder(profile.PlayerID.ToString());
        }
        public static void AddProfile(Profile profile) => Instance._AddProfile(profile);
        public void _AddProfile(Profile profile)
        {
            foreach (var item in ExistingProfiles)
            {
                if (item == profile) return;
            }
            ExistingProfiles.Add(profile);
        }
        public StorageFolder ProfileRoot;
        public static void LoadProfileList() => Instance._LoadProfileList();
        public void _LoadProfileList()
        {

            if (ProfileRoot.TryOpenFile("Profiles.json", out var sf))
            {
                ExistingProfiles = FileIO.DeserializeFromFile<List<Profile>>(sf);
            }
            else
            {
                IsProfileListEmpty = true;
            }
        }
        public static void SaveProfileList() => Instance._SaveProfileList();
        public void _SaveProfileList()
        {
            StorageFile sf = ProfileRoot.CreateOrOpenFile("Profiles.json");
            sf.Delete();
            sf = ProfileRoot.CreateOrOpenFile("Profiles.json");
            FileIO.SerializeToFile(ExistingProfiles, sf);
        }
        public static bool isActiveProfile(Profile P)
        {
            return ActiveID == P;
        }
        public static bool isActiveProfile(Guid P)
        {
            return ActiveID.PlayerID == P;
        }

        public static SetActiveProfileResult SetActiveProfile(Profile p) => Instance._SetActiveProfile(p);
        public SetActiveProfileResult _SetActiveProfile(Profile profile)
        {
            SetActiveProfileResult result = SetActiveProfileResult.FAILE_PROFILE_NOT_FOUND;
            foreach (var item in ExistingProfiles)
            {
                if (item == profile)
                {
                    if (ActiveID.PlayerID == Guid.Empty)
                        result = SetActiveProfileResult.SUCCESS_DIRECT_SET;
                    else
                        result = SetActiveProfileResult.SUCCESS_OVERWRITE;
                    ActiveID = item;
                    return result;
                }
            }
            return result;
        }
    }
    [Serializable]
    public class Profile : IDuplicatable<Profile>
    {
        public Guid PlayerID;
        public string PlayerName;

        public Profile Duplicate()
        {
            return new Profile
            {
                PlayerID = PlayerID,
                PlayerName = PlayerName.ToString()
            };
        }

        public override int GetHashCode()
        {
            return PlayerID.GetHashCode();
        }
    }
}
