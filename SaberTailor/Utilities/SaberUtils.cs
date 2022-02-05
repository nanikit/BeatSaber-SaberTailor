using SaberTailor.Settings;
using SaberTailor.Settings.Classes;
using SaberTailor.Settings.Utilities;
using SaberTailor.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaberTailor.Utilities
{
    public static class SaberUtils
    {
        /// <summary>
        /// Save and update configuration
        /// </summary>
        public static void StoreConfiguration()
        {
            Configuration.Save();
            Configuration.UpdateModVariables();
        }

        
        /// <summary>
        /// Mirror a grip from one side to another
        /// </summary>
        /// <param name="targetSide"></param>
        public static void MirrorGripToSide(GripConfigSide targetSide)
        {
            Configuration.MirrorGripToSide(targetSide);
            RefreshModSettingsUI();
            Configuration.UpdateSaberPosition();
            Configuration.UpdateSaberRotation();
            Configuration.UpdateSaberOffset();
        }

        private static void EmitAll(string eventString)
        {
            LeftController leftController = UIFlowCoordinator._leftControllerController;
            MiddleSettingsPaneController middleController = UIFlowCoordinator._middleSettingsPaneController;
            RightController rightController = UIFlowCoordinator._rightControllerController;
            leftController.parserParams.EmitEvent(eventString);
            middleController.parserParams.EmitEvent(eventString);
            rightController.parserParams.EmitEvent(eventString);
        }

        /// <summary>
        /// Reload configuration and refresh UI
        /// </summary>
        public static void ReloadConfiguration(ConfigSection cfgSection = ConfigSection.All)
        {
            Configuration.Reload(cfgSection);
            SaberUtils.RefreshModSettingsUI();
        }

        /// <summary>
        /// Refresh the entire UI
        /// </summary>
        public static void RefreshModSettingsUI()
        {
            RefreshRotationSettings();
            RefreshPositionSettings();
            RefreshOffsetSettings();
            EmitAll("refresh-sabertailor-values");
        }

        /// <summary>
        /// Refresh position settings UI
        /// </summary>
        public static void RefreshPositionSettings()
        {
            EmitAll("refresh-sabertailor-position-values");
        }

        /// <summary>
        /// Refresh rotation settings UI
        /// </summary>
        public static void RefreshRotationSettings()
        {
            EmitAll("refresh-sabertailor-rotation-values");
        }

        /// <summary>
        /// Refresh offset settings UI
        /// </summary>
        public static void RefreshOffsetSettings()
        {
            EmitAll("refresh-sabertailor-offset-values");
        }

        public static void Electro()
        {
            Configuration.UpdateModVariables();
            RefreshModSettingsUI();
            //Configuration.UpdateSaberPosition();
            //Configuration.UpdateSaberRotation();
            //Configuration.UpdateSaberOffset();
        }

        public static void RandomizerThingy()
        {
            MiddleSettingsPaneController middleController = UIFlowCoordinator._middleSettingsPaneController;
            middleController.parserParams.EmitEvent("randomizerthingy");
        }

        public static string PositionString(int value)
        {
            switch (Configuration.Menu.SaberPosDisplayUnit)
            {
                case PositionDisplayUnit.inches:
                    return string.Format("{0:0.000} inches", value / 25.4f);
                case PositionDisplayUnit.nauticalmiles:
                    return string.Format("{0:0.000000} nmi", value / 1852000f);
                case PositionDisplayUnit.miles:
                    return string.Format("{0:0.000000} miles", value / 1609344f);
                default:
                    return string.Format("{0:0.0} cm", value / 10f);
            }
        }

        public static string RotationString(int value)
        {
            return $"{value} deg";
        }

        public static string MultiplierString(int value)
        {
            return $"{value}%";
        }
    }
}
