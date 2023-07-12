using VoTanPhatVCBRDemo.Debugging;

namespace VoTanPhatVCBRDemo
{
    public class VoTanPhatVCBRDemoConsts
    {
        public const string LocalizationSourceName = "VoTanPhatVCBRDemo";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "b2bd8d4f5e0f487eb8cec6f7194e0e15";
    }
}
