using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
using SaberTailor.HarmonyPatches;
using SaberTailor.Settings.Classes;
using SaberTailor.Settings.Utilities;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using SaberTailor.Settings;
using SaberTailor.Utilities;

namespace SaberTailor.UI
{
    [ViewDefinition("SaberTailor.UI.Views.RightControllerSettings.bsml")]
    public class RightController : BSMLAutomaticViewController
    {
#pragma warning disable CS0649
        // Made this public to use in SaberUtils
        [UIParams]
        public BSMLParserParams parserParams;

#pragma warning restore CS0649

        #region Limits
        [UIValue("saber-pos-inc-max")]
        public int SaberPosIncMax => 200;

        [UIValue("saber-pos-inc-min")]
        public int SaberPosIncMin => 1;

        [UIValue("saber-pos-max")]
        public int SaberPosMax => 500;

        [UIValue("saber-pos-min")]
        public int SaberPosMin => -500;

        [UIValue("saber-rot-max")]
        public int SaberRotMax => 360;

        [UIValue("saber-rot-min")]
        public int SaberRotMin => -360;
        #endregion

        #region Increments
        [UIValue("saber-pos-unit-value")]
        public string SaberPosIncUnit => Configuration.Menu.SaberPosIncUnit.ToString();

        [UIValue("saber-pos-increment-value")]
        public int SaberPosIncValue => Configuration.Menu.SaberPosIncValue;

        [UIValue("saber-rot-increment-value")]
        public int SaberRotIncrement => Configuration.Menu.SaberRotIncrement;
        #endregion

        #region Saber Grip Right
        [UIValue("saber-right-position-x")]
            public int GripRightPositionX
        {
            get => Configuration.GripCfg.PosRight.x;
            set
            {
                int newVal = Increment(Configuration.GripCfg.PosRight.x, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.PosRight.x = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                SaberUtils.RefreshPositionSettings();
            }
        }

        [UIValue("saber-right-position-y")]
        public int GripRightPositionY
        {
            get => Configuration.GripCfg.PosRight.y;
            set
            {
                int newVal = Increment(Configuration.GripCfg.PosRight.y, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.PosRight.y = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                SaberUtils.RefreshPositionSettings();
            }
        }

        [UIValue("saber-right-position-z")]
        public int GripRightPositionZ
        {
            get => Configuration.GripCfg.PosRight.z;
            set
            {
                int newVal = Increment(Configuration.GripCfg.PosRight.z, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.PosRight.z = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                SaberUtils.RefreshPositionSettings();
            }
        }

        [UIValue("saber-right-rotation-x")]
        public int GripRightRotationX
        {
            get => Configuration.GripCfg.RotRight.x;
            set
            {
                int newVal = Increment(Configuration.GripCfg.RotRight.x, SaberRotIncrement, value);
                Configuration.GripCfg.RotRight.x = Mathf.Clamp(newVal, SaberRotMin, SaberRotMax);
                SaberUtils.RefreshRotationSettings();
            }
        }

        [UIValue("saber-right-rotation-y")]
        public int GripRightRotationY
        {
            get => Configuration.GripCfg.RotRight.y;
            set
            {
                int newVal = Increment(Configuration.GripCfg.RotRight.y, SaberRotIncrement, value);
                Configuration.GripCfg.RotRight.y = Mathf.Clamp(newVal, SaberRotMin, SaberRotMax);
                SaberUtils.RefreshRotationSettings();
            }
        }

        [UIValue("saber-right-rotation-z")]
        public int GripRightRotationZ
        {
            get => Configuration.GripCfg.RotRight.z;
            set
            {
                int newVal = Increment(Configuration.GripCfg.RotRight.z, SaberRotIncrement, value);
                Configuration.GripCfg.RotRight.z = Mathf.Clamp(newVal, SaberRotMin, SaberRotMax);
                SaberUtils.RefreshRotationSettings();
            }
        }

        [UIValue("saber-right-offset-x")]
        public int GripRightOffsetX
        {
            get => Configuration.GripCfg.OffsetRight.x;
            set
            {
                int newVal = Increment(Configuration.GripCfg.OffsetRight.x, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.OffsetRight.x = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                SaberUtils.RefreshOffsetSettings();
            }
        }

        [UIValue("saber-right-offset-y")]
        public int GripRightOffsetY
        {
            get => Configuration.GripCfg.OffsetRight.y;
            set
            {
                int newVal = Increment(Configuration.GripCfg.OffsetRight.y, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.OffsetRight.y = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                SaberUtils.RefreshOffsetSettings();
            }
        }

        [UIValue("saber-right-offset-z")]
        public int GripRightOffsetZ
        {
            get => Configuration.GripCfg.OffsetRight.z;
            set
            {
                int newVal = Increment(Configuration.GripCfg.OffsetRight.z, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.OffsetRight.z = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                SaberUtils.RefreshOffsetSettings();
            }
        }
        #endregion


        [UIAction("position-formatter")]
        public string PositionString(int value) => SaberUtils.PositionString(value);

        [UIAction("rotation-formatter")]
        public string RotationString(int value) => SaberUtils.RotationString(value);

        [UIAction("update-saber-rotation")]
        public async void OnUpdateSaberRotation(float _)
        {
            // Delay this UI event to allow UI value to be set first
            await Task.Delay(20);
            Configuration.UpdateSaberRotation();
        }

        [UIAction("update-saber-position")]
        public async void OnUpdateSaberPosition(float _)
        {
            // Delay this UI event to allow UI value to be set first
            await Task.Delay(20);
            Configuration.UpdateSaberPosition();
        }

        [UIAction("update-saber-offset")]
        public async void OnUpdateSaberOffset(float _)
        {
            // Delay this UI event to allow UI value to be set first
            await Task.Delay(20);
            Configuration.UpdateSaberOffset();
        }

        [UIAction("#mirror-grip-right-to-left")]
        public void OnMirrorGripRightToLeft() => SaberUtils.MirrorGripToSide(GripConfigSide.Left);

        [UIAction("#reset-saber-config")]
        public void OnResetSaberConfig() => SaberUtils.ReloadConfiguration();

        [UIAction("#reset-saber-config-grip-right")]
        public void OnResetSaberConfigGripRight() => SaberUtils.ReloadConfiguration(ConfigSection.GripRight);

        public void Awake()
        {
            
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
