using IPA.Utilities;
using SaberTailor.Settings.Classes;
using SaberTailor.Settings.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SaberTailor.Settings.Utilities
{
    class ProfileManager
    {
        internal static bool profilesLoaded = false;
        internal static bool profilesPresent = false;
        internal static List<object> profileNames;
        internal static string filesPath = $"{UnityGame.UserDataPath}/SaberTailor";

        internal static void LoadProfiles()
        {
            profileNames = new List<object>();

            string[] fileNames = Directory.GetFiles(filesPath, @"SaberTailor.*.json");
            foreach (string fileName in fileNames)
            {
                string profileName = Path.GetFileNameWithoutExtension(fileName);
                profileName = profileName.Substring(profileName.IndexOf('.') + 1);
                profileNames.Add(profileName);
            }

            if (profileNames.Count > 0)
            {
                profilesPresent = true;
            }
            else
            {
                profilesPresent = false;
                profileNames.Add("NONE AVAILABLE");
            }

            profilesLoaded = true;
        }

        internal static void MigrateProfiles()
        {
            if (File.Exists($"{UnityGame.UserDataPath}/SaberTailor.json"))
            {

            }
            string[] fileNames = Directory.GetFiles(UnityGame.UserDataPath, @"SaberTailor.*.json");
            foreach (string fileName in fileNames)
            {
                string profileName = Path.GetFileName(fileName);
                if (!File.Exists($"{UnityGame.UserDataPath}/SaberTailor/{profileName}"))
                {
                    
                    File.Copy(fileName, $"{UnityGame.UserDataPath}/SaberTailor/{profileName}");
                }
            }
            if (File.Exists($"{UnityGame.UserDataPath}/SaberTailor.json") && !File.Exists($"{UnityGame.UserDataPath}/SaberTailor/SaberTailor.json")) 
                File.Copy($"{UnityGame.UserDataPath}/SaberTailor.json", $"{UnityGame.UserDataPath}/SaberTailor/SaberTailor.json");
        }

        internal static bool DeleteProfile(string profileName, out string statusMsg)
        {
            string fileName = @"SaberTailor." + profileName + @".json";
            string filePath = Path.Combine(filesPath, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    statusMsg = "Profile '" + profileName + "' deleted successfully.";
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.log.Warn(ex);
                    Logger.log.Warn("Unable to delete profile '" + profileName + "'.");
                    statusMsg = "Unable to delete '" + profileName + "'. Please check the logs.";
                    return false;
                }
            }
            else
            {
                Logger.log.Debug("File not found. Unable to delete profile '" + profileName + "'.");
                statusMsg = "Profile '" + profileName + "' not found. Delete failed.";
                return false;
            }
        }

        internal static bool LoadProfile(string profileName, out string statusMsg)
        {
            string fileName = @"SaberTailor." + profileName + @".json";
            bool loadSuccessful = FileHandler.LoadConfig(out PluginConfig config, fileName);
            if (loadSuccessful)
            {
                PluginConfig.Instance = config;
                Configuration.Load();
                statusMsg = "Profile '" + profileName + "' loaded successfully.";
                return true;
            }
            else
            {
                statusMsg = "Profile loading failed. Please check logs files.";
                return false;
            }
        }

        internal static bool LoadDefaultProfile()
        {
            string fileName = @"SaberTailor.json";
            bool loadSuccessful = FileHandler.LoadConfig(out PluginConfig config, fileName);
            if (loadSuccessful)
            {
                PluginConfig.Instance = config;
                Configuration.Load();
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static bool SaveProfile(string profileName, out string statusMsg)
        {
            string fileName = @"SaberTailor." + profileName + @".json";

            //Save the active configuration to a new object
            PluginConfig config = new PluginConfig();
            Configuration.SaveConfig(ref config);

            bool saveSuccessful = FileHandler.SaveConfig(config, fileName);

            if (saveSuccessful)
            {
                statusMsg = "Profile '" + profileName + "' saved successfully.";
                return true;
            }
            else
            {
                statusMsg = "Error saving profile. Please check the log files.";
                return false;
            }
        }

        internal static void PrintProfileNames()
        {
            Logger.log.Debug("Printing list of Profile names:");
            if (!profilesPresent)
            {
                Logger.log.Debug("No profiles present.");
                return;
            }
            Logger.log.Debug(String.Join("; ", profileNames));
        }

        internal static List<object> ReloadProfileList()
        {
            Logger.log.Debug("Reloading profiles");
            LoadProfiles();
            return profileNames;
        }
    }
}
