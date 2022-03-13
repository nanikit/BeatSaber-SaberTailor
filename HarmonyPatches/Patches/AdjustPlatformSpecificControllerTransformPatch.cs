using BS_Utils.Utilities;
using HarmonyLib;
using SaberTailor.Settings;
using UnityEngine;
using UnityEngine.XR;

namespace SaberTailor.HarmonyPatches
{
    [HarmonyPatch(typeof(DevicelessVRHelper))]
    [HarmonyPatch("AdjustControllerTransform")]
    internal class DevicelessVRHelperAdjustControllerTransform
    {
        private static void Prefix(XRNode node, Transform transform, ref Vector3 position, ref Vector3 rotation)
        {
            if (!Configuration.Grip.IsGripModEnabled)
            {
                return;
            }
            Utilities.AdjustControllerTransform(node, transform, ref position, ref rotation);
        }
    }

    [HarmonyPatch(typeof(OculusVRHelper))]
    [HarmonyPatch("AdjustControllerTransform")]
    internal class OculusVRHelperAdjustControllerTransform
    {
        private static void Prefix(XRNode node, Transform transform, ref Vector3 position, ref Vector3 rotation)
        {
            if (!Configuration.Grip.IsGripModEnabled)
            {
                return;
            }
            Utilities.AdjustControllerTransform(node, transform, ref position, ref rotation);
        }
    }

    [HarmonyPatch(typeof(OpenVRHelper))]
    [HarmonyPatch("AdjustControllerTransform")]
    internal class OpenVRHelperAdjustControllerTransform
    {
        private static void Prefix(XRNode node, Transform transform, ref Vector3 position, ref Vector3 rotation)
        {
            if (!Configuration.Grip.IsGripModEnabled)
            {
                return;
            }
            Utilities.AdjustControllerTransform(node, transform, ref position, ref rotation);
        }
    }

    internal class Utilities
    {
        internal static void AdjustControllerTransform(XRNode node, Transform transform, ref Vector3 position, ref Vector3 rotation)
        {
            position = Vector3.zero;
            rotation = Vector3.zero;

            // Always check for sabers first and modify and exit out immediately if found
            if (transform.gameObject.name == "LeftHand" || transform.gameObject.name.Contains("Saber A"))
            {
                // https://github.com/Shadnix-was-taken/BeatSaber-SaberTailor/issues/29 - Code taken from commit here, don't know how accurate or correct it is
                if (Configuration.Base.Electrostats)
                {
                    transform.Rotate(0, 0, Configuration.Grip.RotLeft.z);
                    transform.Translate(Configuration.Grip.PosLeft);
                    transform.Rotate(Configuration.Grip.RotLeft.x, Configuration.Grip.RotLeft.y, 0);
                    transform.Translate(Configuration.Grip.OffsetLeft, Space.World);
                }
                if (Configuration.Grip.UseBaseGameAdjustmentMode)
                {
                    position = Configuration.Grip.PosLeft;
                    rotation = Configuration.Grip.RotLeft;
                }
                else
                {
                    if (Configuration.Base.Electrostats) return;
                    transform.Translate(Configuration.Grip.PosLeft);
                    transform.Rotate(Configuration.Grip.RotLeft);
                    transform.Translate(Configuration.Grip.OffsetLeft, Space.World);
                }
                
                return;
            }
            else if (transform.gameObject.name == "RightHand" || transform.gameObject.name.Contains("Saber B"))
            {
                // https://github.com/Shadnix-was-taken/BeatSaber-SaberTailor/issues/29 - Code taken from commit here, don't know how accurate or correct it is
                if (Configuration.Base.Electrostats)
                {
                    transform.Rotate(0, 0, Configuration.Grip.RotRight.z);
                    transform.Translate(Configuration.Grip.PosRight);
                    transform.Rotate(Configuration.Grip.RotRight.x, Configuration.Grip.RotRight.y, 0);
                    transform.Translate(Configuration.Grip.OffsetRight, Space.World);
                }
                if (Configuration.Grip.UseBaseGameAdjustmentMode)
                {
                    position = Configuration.Grip.PosRight;
                    rotation = Configuration.Grip.RotRight;
                }
                else
                {
                    if (Configuration.Base.Electrostats) return;
                    transform.Translate(Configuration.Grip.PosRight);
                    transform.Rotate(Configuration.Grip.RotRight);
                    transform.Translate(Configuration.Grip.OffsetRight, Space.World);
                }
                
                
                return;
            }

            // Check settings if modifications should also apply to menu hilts
            if (Configuration.Grip.ModifyMenuHiltGrip != false)
            {
                if (transform.gameObject.name == "ControllerLeft")
                {
                    // https://github.com/Shadnix-was-taken/BeatSaber-SaberTailor/issues/29 - Code taken from commit here, don't know how accurate or correct it is
                    if (Configuration.Base.Electrostats)
                    {
                        transform.Rotate(0, 0, Configuration.Grip.RotLeft.z);
                        transform.Translate(Configuration.Grip.PosLeft);
                        transform.Rotate(Configuration.Grip.RotLeft.x, Configuration.Grip.RotLeft.y, 0);
                        transform.Translate(Configuration.Grip.OffsetLeft, Space.World);
                    }
                    if (Configuration.Grip.UseBaseGameAdjustmentMode)
                    {
                        position = Configuration.Grip.PosLeft;
                        rotation = Configuration.Grip.RotLeft;
                    }
                    else
                    {
                        if (Configuration.Base.Electrostats) return;
                        transform.Translate(Configuration.Grip.PosLeft);
                        transform.Rotate(Configuration.Grip.RotLeft);
                        transform.Translate(Configuration.Grip.OffsetLeft, Space.World);
                    }
                    
                    
                    return;
                }
                else if (transform.gameObject.name == "ControllerRight")
                {
                    // https://github.com/Shadnix-was-taken/BeatSaber-SaberTailor/issues/29 - Code taken from commit here, don't know how accurate or correct it is
                    if (Configuration.Base.Electrostats)
                    {
                        transform.Rotate(0, 0, Configuration.Grip.RotRight.z);
                        transform.Translate(Configuration.Grip.PosRight);
                        transform.Rotate(Configuration.Grip.RotRight.x, Configuration.Grip.RotRight.y, 0);
                        transform.Translate(Configuration.Grip.OffsetRight, Space.World);
                    }
                    if (Configuration.Grip.UseBaseGameAdjustmentMode)
                    {
                        position = Configuration.Grip.PosRight;
                        rotation = Configuration.Grip.RotRight;
                    }
                    else
                    {
                        if (Configuration.Base.Electrostats) return;
                        transform.Translate(Configuration.Grip.PosRight);
                        transform.Rotate(Configuration.Grip.RotRight);
                        transform.Translate(Configuration.Grip.OffsetRight, Space.World);
                    }
                   
                    
                    return;
                }
            }
        }
    }
}