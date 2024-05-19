//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BunnyTestVR;
//using HarmonyLib;
//using UnityEngine;
//using UnityEngine.Profiling;
//using Valve.VR;
//using GB;


//namespace BunnyModTest4.VRCamera.Patches
//{
//    [HarmonyPatch]
//    internal class CameraPatches
//    {
//        [HarmonyPostfix]
//        [HarmonyPatch(typeof(GB.Scene.BarScene), nameof(GB.Scene.BarScene.PlayBGM))]
//        private static void OnCameraRigEnabled()
//        {
//            Logs.WriteInfo("CameraRig OnEnable started");

//            //CameraManager.ReduceNearClipping();
//            GameObject mainCamObj;
//            //mainCamObj = Camera.current.gameObject;
//            //mainCamObj = null;
//            //if (mainCamObj == null)
//            //{
//            //    Logs.WriteInfo("Main Camera not found");
//            //    if (GameObject.Find("MainCamera") != null)
//            //    {
//            //        Logs.WriteInfo("MainCamera found");
//            //        mainCamObj = GameObject.Find("MainCamera");
//            //    }
//            //    else
//            //    {
//            //        Logs.WriteInfo("MainCamera not found");
//            //        if (GameObject.Find("GameCamera") != null)
//            //        {
//            //            Logs.WriteInfo("GameCamera found");
//            //            mainCamObj = GameObject.Find("GameCamera");
//            //        }
//            //        else
//            //        {
//            //            Logs.WriteInfo("GameCamera not found");
//            //            return;
//            //        }
//            //    }
//            //
//            mainCamObj = Plugin.MainCamera.gameObject;

//            //Without this there is no headtracking
//            mainCamObj.AddComponent<SteamVR_TrackedObject>();
//            Logs.WriteInfo("SteamVR_TrackedObject ok started");

//            Plugin.SecondEye = new GameObject("SecondEye");
//            Plugin.SecondCam = Plugin.SecondEye.AddComponent<Camera>();
//            Plugin.SecondCam.gameObject.AddComponent<SteamVR_TrackedObject>();
//            Plugin.SecondCam.CopyFrom(mainCamObj.GetComponent<Camera>());

//            // Without this the right eye gets stuck at a very far point in the map
//            Plugin.SecondCam.transform.parent = mainCamObj.transform.parent;

//            // Pimax 5K plus causes the fog of war to behave very bad, this is supposed to fix it but doesn't work yet.
//            if (Plugin.HMDModel == "Vive MV")
//            {
//                Logs.WriteInfo("HMD recognised as VIVE MV, disabling FogOfWar");
//                //Owlcat.Runtime.Visual.RenderPipeline.RendererFeatures.FogOfWar.FogOfWarFeature.Instance.DisableFeature();
//            }

//        }
//    }
//}
