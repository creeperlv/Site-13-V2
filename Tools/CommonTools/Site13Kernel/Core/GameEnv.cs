using Site13Kernel.GameLogic.CampaignActions;
using System.IO;

namespace Site13Kernel.Core
{
    public static class GameEnv
    {
        public static float CollisionDamageSpeedThreshold = 2;
        public static float CollisionDamageIntensity = 1;
        public static string DataPath;
        public const string EmptyString = "";
        public static char PathSep = Path.DirectorySeparatorChar;
        public static float ExplosionIntensityOnSimulatedRigidBody=.5f;
    }
}
