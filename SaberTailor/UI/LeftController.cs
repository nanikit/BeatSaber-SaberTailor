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
    [ViewDefinition("SaberTailor.UI.Views.LeftControllerSettings.bsml")]
    public class LeftController : BSMLAutomaticViewController
    {
#pragma warning disable CS0649
        // Made this public to use in SaberUtils
        [UIParams]
        public BSMLParserParams parserParams;
#pragma warning restore CS0649

        #region Precision

        [UIValue("saber-pos-increment-value")]
        public int SaberPosIncValue => Configuration.Menu.SaberPosIncValue;

        [UIValue("saber-rot-increment-value")]
        public int SaberRotIncrement => Configuration.Menu.SaberRotIncrement;

        [UIValue("saber-pos-display-unit-value")]
        public string SaberPosDisplayUnit = Configuration.Menu.SaberPosDisplayUnit.ToString();
        #endregion

        #region Saber Grip Left
        [UIValue("saber-left-position-x")]
        public int GripLeftPositionX
        {
            get => Configuration.GripCfg.PosLeft.x;
            set
            {
                int newVal = Increment(Configuration.GripCfg.PosLeft.x, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.PosLeft.x = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                SaberUtils.RefreshPositionSettings();
            }
        }

        [UIValue("saber-left-position-y")]
        public int GripLeftPositionY
        {
            get => Configuration.GripCfg.PosLeft.y;
            set
            {
                int newVal = Increment(Configuration.GripCfg.PosLeft.y, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.PosLeft.y = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                SaberUtils.RefreshPositionSettings();
            }
        }

        [UIValue("saber-left-position-z")]
        public int GripLeftPositionZ
        {
            get => Configuration.GripCfg.PosLeft.z;
            set
            {
                int newVal = Increment(Configuration.GripCfg.PosLeft.z, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.PosLeft.z = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                SaberUtils.RefreshPositionSettings();
            }
        }

        [UIValue("saber-left-rotation-x")]
        public int GripLeftRotationX
        {
            get => Configuration.GripCfg.RotLeft.x;
            set
            {
                int newVal = Increment(Configuration.GripCfg.RotLeft.x, SaberRotIncrement, value);
                Configuration.GripCfg.RotLeft.x = Mathf.Clamp(newVal, SaberRotMin, SaberRotMax);
                SaberUtils.RefreshRotationSettings();
            }
        }

        [UIValue("saber-left-rotation-y")]
        public int GripLeftRotationY
        {
            get => Configuration.GripCfg.RotLeft.y;
            set
            {
                int newVal = Increment(Configuration.GripCfg.RotLeft.y, SaberRotIncrement, value);
                Configuration.GripCfg.RotLeft.y = Mathf.Clamp(newVal, SaberRotMin, SaberRotMax);
                SaberUtils.RefreshRotationSettings();
            }
        }

        [UIValue("saber-left-rotation-z")]
        public int GripLeftRotationZ
        {
            get => Configuration.GripCfg.RotLeft.z;
            set
            {
                int newVal = Increment(Configuration.GripCfg.RotLeft.z, SaberRotIncrement, value);
                Configuration.GripCfg.RotLeft.z = Mathf.Clamp(newVal, SaberRotMin, SaberRotMax);
                SaberUtils.RefreshRotationSettings();
            }
        }

        [UIValue("saber-left-offset-x")]
        public int GripLeftOffsetX
        {
            get => Configuration.GripCfg.OffsetLeft.x;
            set
            {
                int newVal = Increment(Configuration.GripCfg.OffsetLeft.x, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.OffsetLeft.x = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                SaberUtils.RefreshOffsetSettings();
            }
        }

        [UIValue("saber-left-offset-y")]
        public int GripLeftOffsetY
        {
            get => Configuration.GripCfg.OffsetLeft.y;
            set
            {
                int newVal = Increment(Configuration.GripCfg.OffsetLeft.y, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.OffsetLeft.y = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                SaberUtils.RefreshOffsetSettings();
            }
        }

        [UIValue("saber-left-offset-z")]
        public int GripLeftOffsetZ
        {
            get => Configuration.GripCfg.OffsetLeft.z;
            set
            {
                int newVal = Increment(Configuration.GripCfg.OffsetLeft.z, Configuration.Menu.SaberPosIncrement, value);
                Configuration.GripCfg.OffsetLeft.z = Mathf.Clamp(newVal, SaberPosMin, SaberPosMax);
                SaberUtils.RefreshOffsetSettings();
            }
        }
        #endregion

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

        #region Formatters

        [UIAction("position-formatter")]
        public string PositionString(int value) => SaberUtils.PositionString(value);

        [UIAction("rotation-formatter")]
        public string RotationString(int value) => SaberUtils.RotationString(value);

        #endregion

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

        [UIAction("#mirror-grip-left-to-right")]
        public void OnMirrorGripLeftToRight() => SaberUtils.MirrorGripToSide(GripConfigSide.Right);

        [UIAction("#reset-saber-config")]
        public void OnResetSaberConfig() => SaberUtils.ReloadConfiguration();

        [UIAction("#reset-saber-config-grip-left")]
        public void OnResetSaberConfigGripLeft() => SaberUtils.ReloadConfiguration(ConfigSection.GripLeft);

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
