using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
using SaberTailor.HarmonyPatches;
using SaberTailor.Settings.Classes;
using SaberTailor.Settings.Utilities;
using System.Text.RegularExpressions;
using TMPro;
using SaberTailor.Settings;
using SaberTailor.Utilities;
using System.Diagnostics;
using HMUI;

namespace SaberTailor.UI
{
    [ViewDefinition("SaberTailor.UI.Views.MiddleMainSettings.bsml")]
    public class MiddleSettingsPaneController : BSMLAutomaticViewController
    {
        public string profileListSelected = "None selected";

#pragma warning disable CS0649
        [UIParams]
        public BSMLParserParams parserParams;

        [UIComponent("ddl_profiles")]
        DropDownListSetting ddlsProfiles;
#pragma warning restore CS0649

        #region Precision
        [UIValue("saber-pos-unit-options")]
        public List<object> SaberPosUnitValues = Enum.GetNames(typeof(PositionUnit)).ToList<object>();

        [UIValue("saber-pos-display-unit-options")]
        public List<object> SaberPosDisplayUnitValues = Enum.GetNames(typeof(PositionDisplayUnit)).ToList<object>();

        [UIValue("saber-pos-unit-value")]
        public string SaberPosIncUnit
        {
            get => Configuration.Menu.SaberPosIncUnit.ToString();
            set
            {
                Configuration.Menu.SaberPosIncUnit = Enum.TryParse(value, out PositionUnit positionUnit) ? positionUnit : PositionUnit.cm;
                UpdateSaberPosIncrement(Configuration.Menu.SaberPosIncUnit);
                SaberUtils.RefreshPositionSettings();
            }
        }

        [UIValue("saber-pos-increment-value")]
        public int SaberPosIncValue
        {
            get => Configuration.Menu.SaberPosIncValue;
            set
            {
                Configuration.Menu.SaberPosIncValue = value;
                UpdateSaberPosIncrement(Configuration.Menu.SaberPosIncUnit);
                SaberUtils.RefreshPositionSettings();
            }
        }

        [UIValue("saber-rot-increment-value")]
        public int SaberRotIncrement
        {
            get => Configuration.Menu.SaberRotIncrement;
            set => Configuration.Menu.SaberRotIncrement = value;
        }

        [UIValue("saber-pos-display-unit-value")]
        public string SaberPosDisplayUnit
        {
            get => Configuration.Menu.SaberPosDisplayUnit.ToString();
            set
            {
                Configuration.Menu.SaberPosDisplayUnit = Enum.TryParse(value, out PositionDisplayUnit unit) ? unit : PositionDisplayUnit.cm;
                SaberUtils.RefreshPositionSettings();
            }
        }
        #endregion

        #region Saber Grip
        [UIValue("saber-grip-tweak-enabled")]
        public bool GripTweakEnabled
        {
            get => Configuration.Grip.IsGripModEnabled;
            set
            {
                Configuration.Grip.IsGripModEnabled = value;
                SaberTailorPatches.CheckHarmonyPatchStatus();
            }
        }
        #endregion

        #region Saber Grip MenuHilt
        [UIValue("menuhiltadjust-enabled")]
        public bool GripModifyMenuHiltGrip
        {
            get => Configuration.Grip.ModifyMenuHiltGrip;
            set => Configuration.Grip.ModifyMenuHiltGrip = value;
        }
        #endregion

        #region Saber Adjustment Mode
        [UIValue("basegameadjustmentmode-enabled")]
        public bool UseBaseGameAdjustmentMode
        {
            get => Configuration.Grip.UseBaseGameAdjustmentMode;
            set => Configuration.Grip.UseBaseGameAdjustmentMode = value;
        }
        #endregion

        #region Saber Scale
        [UIValue("saber-scale-tweak-enabled")]
        public bool ScaleTweakEnabled
        {
            get => Configuration.Scale.TweakEnabled;
            set => Configuration.Scale.TweakEnabled = value;
        }

        [UIValue("saber-scale-hitbox-enabled")]
        public bool ScaleHitboxEnabled
        {
            get => Configuration.Scale.ScaleHitBox;
            set => Configuration.Scale.ScaleHitBox = value;
        }

        [UIValue("saber-scale-length")]
        public int ScaleLength
        {
            get => Configuration.ScaleCfg.Length;
            set => Configuration.ScaleCfg.Length = value;
        }

        [UIValue("saber-scale-girth")]
        public int ScaleGirth
        {
            get => Configuration.ScaleCfg.Girth;
            set => Configuration.ScaleCfg.Girth = value;
        }
        #endregion

        #region Saber Trail
        [UIValue("saber-trail-tweak-enabled")]
        public bool TrailTweakEnabled
        {
            get => Configuration.Trail.TweakEnabled;
            set
            {
                Configuration.Trail.TweakEnabled = value;
                SaberTailorPatches.CheckHarmonyPatchStatus();
            }
        }

        [UIValue("saber-trail-enabled")]
        public bool TrailEnabled
        {
            get => Configuration.Trail.TrailEnabled;
            set
            {
                Configuration.Trail.TrailEnabled = value;
                SaberTailorPatches.CheckHarmonyPatchStatus();
            }
        }

        [UIValue("saber-trail-duration")]
        public int TrailDuration
        {
            get => Configuration.Trail.Duration;
            set => Configuration.Trail.Duration = value;
        }

        [UIValue("saber-trail-granularity")]
        public int TrailLength
        {
            get => Configuration.Trail.Granularity;
            set => Configuration.Trail.Granularity = value;
        }

        [UIValue("saber-trail-whiteduration")]
        public int TrailWhiteSectionDuration
        {
            get => Configuration.Trail.WhiteSectionDuration;
            set => Configuration.Trail.WhiteSectionDuration = value;
        }
        #endregion

        #region Transfer Grip
#pragma warning disable CS0649
        [UIComponent("transfer-txt")]
        private TextMeshProUGUI TransferText;
#pragma warning restore CS0649
        #endregion

        [UIComponent("randomizebool")]
        private NoTransitionsButton randomizebool;

        #region Profile Manager
        [UIValue("profile-list-options")]
        public List<object> ProfileListValues = ProfileManager.profileNames;

        [UIValue("profile-list-value")]
        public string _profileListSelected
        {
            get => profileListSelected;
            set => profileListSelected = value;
        }

        [UIValue("profile-save-name")]
        public string ProfileSaveName = "Default";

#pragma warning disable CS0649
        [UIComponent("profile-txt")]
        private TextMeshProUGUI ProfileStatusText;
#pragma warning restore CS0649
        #endregion

        [UIValue("electrostats")]
        public bool Electrostats
        {
            get => Configuration.Base.Electrostats;
            set
            {
                Configuration.Base.Electrostats = value;
                SaberUtils.Electro();
            }
        }

        [UIValue("electroball")]
        public bool Electroball
        {
            get => Configuration.Base.electroball;
            set
            {
                SaberUtils.Electro();
                Configuration.Base.electroball = value;
                PluginConfig.Instance.electroball = value;
            }
        }

        [UIValue("randomize")]
        public bool Randomize
        {
            get => Configuration.Base.Randomize;
            set
            {
                randomizebool.interactable = value;
                Configuration.Base.Randomize = value;
                SaberUtils.Electro();
            }
        }

        [UIAction("#randomizer")]
        public void RandomizeSettings()
        {
            if (Configuration.Base.Randomize)
            {
                Configuration.RandomizeSabers();
                SaberUtils.RefreshModSettingsUI();
                SaberUtils.Electro();
            }
        }


        #region Formatters
        [UIAction("position-inc-formatter")]
        public string PositionIncString(int value)
        {
            switch (Configuration.Menu.SaberPosIncUnit)
            {
                case PositionUnit.cm:
                    return $"{value} cm";
                //case PositionUnit.inches:
                //    return string.Format("{0}/8 inches", value);
                default:
                    return $"{value} mm";
            }
        }

        [UIAction("rotation-formatter")]
        public string RotationString(int value) => SaberUtils.RotationString(value);

        [UIAction("multiplier-formatter")]
        public string MultiplierString(int value) => SaberUtils.MultiplierString(value);

        [UIAction("trail-time-formatter")]
        public string TrailTimeString(int value)
        {
            return string.Format("{0:0.0} s", value / 1000f);
        }

        [UIAction("trail-white-time-formatter")]
        public string TrailWhiteTimeString(int value)
        {
            return string.Format("{0:0.00} s", value / 1000f);
        }
        #endregion

        [UIAction("#saber-grip-export")]
        public void OnGripExport() => ExportGripToGameSettings();

        [UIAction("#saber-grip-import")]
        public void OnGripImport() => ImportGripFromGameSettings();

        [UIAction("#profile-delete")]
        public void OnProfileDelete() => DeleteProfile();

        [UIAction("#profile-load")]
        public void OnProfileLoad() => LoadProfile();

        [UIAction("#profile-save")]
        public void OnProfileSave() => SaveProfile();

        [UIAction("#profile-list-reload")]
        public void OnProfileListReload() => RefreshProfileList();

        [UIAction("#reset-saber-config-scale")]
        public void OnResetSaberConfigScale() => SaberUtils.ReloadConfiguration(ConfigSection.Scale);

        [UIAction("#reset-saber-config-trail")]
        public void OnResetSaberConfigTrail() => SaberUtils.ReloadConfiguration(ConfigSection.Trail);

        [UIAction("#SteffanGithub")]
        public void SteffanGithub() => OpenGithub("https://github.com/SteffanDonal");

        [UIAction("#ShadnixGithub")]
        public void ShadnixGithub() => OpenGithub("https://github.com/Shadnix-was-taken");

        [UIAction("#rondGithub")]
        public void rondGithub() => OpenGithub("https://github.com/rondDev");

        private void OpenGithub(string url)
        {
            Process.Start(url);
        }

        public void Awake()
        {
            if (ProfileManager.profilesPresent)
            {
                profileListSelected = ProfileManager.profileNames[0].ToString();
            }
        }

        private void ImportGripFromGameSettings()
        {
            try
            {
                bool importSuccessful = GameSettingsTransfer.ImportFromGame();
                if (importSuccessful)
                {
                    TransferText.text = "Import successful. Please remember to enable saber grip adjustments in SaberTailor for enabling SaberTailor.";
                    Configuration.UpdateSaberPosition();
                    Configuration.UpdateSaberRotation();
                    SaberUtils.RefreshModSettingsUI();
                }
                else
                {
                    TransferText.text = "<color=#fb484e>Unable to import from base game: Unknown error.</color>";
                }
            }
            catch (Exception e)
            {
                Logger.log.Error(e);
            }
        }

        private void ExportGripToGameSettings()
        {
            GameSettingsTransfer.ExportToGame(out string statusMsg);
            TransferText.text = statusMsg;
        }

        private void DeleteProfile()
        {
            // Maybe add an additional confirmation dialog
            if (!ProfileManager.profilesPresent)
            {
                ProfileStatusText.text = "<color=#fb484e>Unable to delete profile: None found.</color>";
                return;
            }
            string profileName = profileListSelected;

            bool deleteSuccessful = ProfileManager.DeleteProfile(profileName, out string statusMsg);

            // Refresh UI
            RefreshProfileList();

            if (!deleteSuccessful)
            {
                statusMsg = "<color=#fb484e>" + statusMsg + "</color>";
            }
            ProfileStatusText.text = statusMsg;
        }

        private void LoadProfile()
        {
            if (!ProfileManager.profilesPresent)
            {
                ProfileStatusText.text = "<color=#fb484e>Unable to load profile: None found.</color>";
                return;
            }
            string profileName = profileListSelected;

            bool loadSuccessful = ProfileManager.LoadProfile(profileName, out string statusMsg);
            SaberUtils.RefreshModSettingsUI();

            if (!loadSuccessful)
            {
                statusMsg = "<color=#fb484e>" + statusMsg + "</color>";
            }
            ProfileStatusText.text = statusMsg;
        }

        private void SaveProfile()
        {
            string profileName = ProfileSaveName;

            Regex r = new Regex(@"^([a-zA-Z0-9_-]+)$");
            Match m = r.Match(profileName);
            if (!m.Success)
            {
                ProfileStatusText.text = "<color=#fb484e>Invalid profile name. Profile names may only contain letters A-Z, numbers, dashes and underscores.</color>";
                return;
            }

            bool saveSuccessful = ProfileManager.SaveProfile(profileName, out string statusMsg);

            // Refresh UI
            ProfileManager.LoadProfiles();
            ddlsProfiles.values = ProfileManager.profileNames;
            ddlsProfiles.UpdateChoices();
            parserParams.EmitEvent("refresh-profile-list");

            if (!saveSuccessful)
            {
                statusMsg = "<color=#fb484e>" + statusMsg + "</color>";
            }

            ProfileStatusText.text = statusMsg;
        }

        private void RefreshProfileList()
        {
            ProfileManager.LoadProfiles();
            ddlsProfiles.values = ProfileManager.profileNames;
            profileListSelected = ddlsProfiles.values[0].ToString();
            ddlsProfiles.UpdateChoices();
            parserParams.EmitEvent("refresh-profile-list");
        }

        private void UpdateSaberPosIncrement(PositionUnit unit)
        {
            switch (unit)
            {
                case PositionUnit.cm:
                    Configuration.Menu.SaberPosIncrement = Configuration.Menu.SaberPosIncValue * 10;
                    break;
                //case PositionUnit.inches:
                //    Configuration.Menu.SaberPosIncrement = Configuration.Menu.SaberPosIncValue / 25.4f;
                //    break;
                default:
                    Configuration.Menu.SaberPosIncrement = Configuration.Menu.SaberPosIncValue;
                    break;
            }
        }

        /// <summary>
        /// Returns a value incremented by the magic number
        /// </summary>
        /// <param name="currentValue">Current value</param>
        /// <param name="incrementBy">Magic increment number</param>
        /// <param name="value">Real increment number</param>
        /// <returns></returns>
        private int Increment(int currentValue, int incrementBy, int value)
        {
            int result = currentValue;

            if (currentValue < value)
            {
                result += incrementBy;
            }
            else if (currentValue > value)
            {
                result -= incrementBy;
            }

            return result;
        }
    }
}
