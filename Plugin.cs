using BepInEx;
using HarmonyLib;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using GB.Game;

using Valve.VR;
using UnityEngine.XR.Management;
using UnityEngine.XR.OpenXR;
using Unity.XR.OpenVR;
using UnityEngine.XR;
using BunnyModTest4.VRCamera;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace BunnyModTest2
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]    //MODの情報(属性)を与える
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.aka.VRMods.BunnyVR";
        public const string PLUGIN_NAME = "BunnyVR";
        public const string PLUGIN_VERSION = "0.0.1";

        public static string gameExePath = Process.GetCurrentProcess().MainModule.FileName;
        public static string gamePath = Path.GetDirectoryName(gameExePath);
        public static string HMDModel = "";

        //public static MBHelper MyHelper;

        public static UnityEngine.XR.Management.XRManagerSettings managerSettings = null;

        public static List<UnityEngine.XR.XRDisplaySubsystemDescriptor> displaysDescs = new List<UnityEngine.XR.XRDisplaySubsystemDescriptor>();
        public static List<UnityEngine.XR.XRDisplaySubsystem> displays = new List<UnityEngine.XR.XRDisplaySubsystem>();
        public static UnityEngine.XR.XRDisplaySubsystem MyDisplay = null;


        public static Camera MainCamera = null;
        public static GameObject SecondEye = null;
        public static Camera SecondCam = null;

        //Create a class that actually inherits from MonoBehaviour
        public class MyStaticMB : MonoBehaviour
        {
        }

        //Variable reference for the class
        public static MyStaticMB myStaticMB;

        private void Awake()
        {

            //new AssetLoader();
            Logger.LogInfo("Hello, world! VR");
            Harmony Harmony = new Harmony(PLUGIN_GUID);

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            //If the instance not exit the first time we call the static class
            if (myStaticMB == null)
            {
                //Create an empty object called MyStatic
                GameObject gameObject = new GameObject("MyStatic");


                //Add this script to the object
                myStaticMB = gameObject.AddComponent<MyStaticMB>();
            }

            myStaticMB.StartCoroutine(InitVRLoader());

            //Game.s_Instance.ControllerMode = Game.ControllerModeType.Gamepad;
            //Game.Instance.ControllerMode = Game.ControllerModeType.Gamepad;

            //Logs.WriteInfo("ControllerMode is: " + Game.Instance.ControllerMode);
            SceneManager.sceneLoaded += (UnityAction<Scene, LoadSceneMode>)OnSceneLoaded;

            Logs.WriteInfo("Reached end of Plugin.Awake()");
        }
        public static System.Collections.IEnumerator InitVRLoader()
        {

            SteamVR_Actions.PreInitialize();

            var generalSettings = ScriptableObject.CreateInstance<XRGeneralSettings>();
            managerSettings = ScriptableObject.CreateInstance<XRManagerSettings>();
            var xrLoader = ScriptableObject.CreateInstance<OpenVRLoader>();


            var settings = OpenVRSettings.GetSettings();
            settings.StereoRenderingMode = OpenVRSettings.StereoRenderingModes.MultiPass;


            generalSettings.Manager = managerSettings;

            managerSettings.loaders.Clear();
            managerSettings.loaders.Add(xrLoader);

            managerSettings.InitializeLoaderSync(); ;


            XRGeneralSettings.AttemptInitializeXRSDKOnLoad();
            XRGeneralSettings.AttemptStartXRSDKOnBeforeSplashScreen();

            SteamVR.Initialize(true);


            SubsystemManager.GetInstances(displays);
            MyDisplay = displays[0];
            MyDisplay.Start();

            Logs.WriteInfo("SteamVR hmd modelnumber: " + SteamVR.instance.hmd_ModelNumber);
            HMDModel = SteamVR.instance.hmd_ModelNumber;

            //new VRInputManager();

            Logs.WriteInfo("Reached end of InitVRLoader");

            yield return null;

        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameObject gameObject;
            Logger.LogInfo("OnSceneLoaded: " + scene.name + " " + mode);
            // Detects a single mode scene and starts VR control of the scene.
            if (mode == LoadSceneMode.Single || scene.name == "TitleScene")
            {
                gameObject = new GameObject($"{nameof(GetActiveCamera)}{scene.name}");
                gameObject.AddComponent<GetActiveCamera>();
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
