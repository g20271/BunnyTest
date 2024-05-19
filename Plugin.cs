using BepInEx;
using BepInEx.Unity.Mono;
using BepInEx.Logging;
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
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using BunnyTestVR.Logging;
using BunnyTestVR.VRUtils;
using BunnyTestVR.VRGet;
using System.Linq;


namespace BunnyTestVR
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]    //MODの情報(属性)を与える
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.aka.VRMods.BunnyVR";
        public const string PLUGIN_NAME = "BunnyVR";
        public const string PLUGIN_VERSION = "0.0.2";

        public static string gameExePath = Process.GetCurrentProcess().MainModule.FileName;
        public static string gamePath = Path.GetDirectoryName(gameExePath);
        public static string HMDModel = "";

        public static Camera RenderMainCamera = null;
        public static GameObject getActiveCamera = null, vrcontroller = null;

        private void Awake()
        {

            Logger.LogInfo("Hello, world! VR");
            Harmony Harmony = new Harmony(PLUGIN_GUID);

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            PluginLog.Setup(Logger);

            // Initialize VR if the SteamVR process is running.
            if (IsSteamVRRunning)
            {
                VR.Initialize(() =>
                {
                    SceneManager.sceneLoaded += OnSceneLoaded;
                });
            }

        }


        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {

            Logger.LogInfo("OnSceneLoaded: " + scene.name + " " + mode);
            // Detects a single mode scene and starts VR control of the scene.
            if (mode == LoadSceneMode.Single || scene.name == "TitleScene")
            {
                if (vrcontroller == null)
                {
                    vrcontroller = new GameObject($"{nameof(SimpleVRController)}{scene.name}");
                    vrcontroller.AddComponent<SimpleVRController>();
                    DontDestroyOnLoad(vrcontroller);
                }


                if (getActiveCamera == null)
                {
                    getActiveCamera = new GameObject($"{nameof(GetActiveCamera)}{scene.name}");
                    getActiveCamera.AddComponent<GetActiveCamera>();
                    DontDestroyOnLoad(getActiveCamera);
                    PluginLog.Info("GetActiveCamera created");
                }

                var cube = new GameObject("Cube");
                cube.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
                cube.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                cube.transform.position = new Vector3(0, 0, 2);
                DontDestroyOnLoad(cube);
            }
        }

        /// <summary>
        /// Checks if the SteamVR compositor process is currently running. Used to determine if VR initialization is necessary.
        /// </summary>
        private bool IsSteamVRRunning => Process.GetProcesses().Any(i => i.ProcessName == "vrcompositor");

        public ManualLogSource Log { get; private set; }
    }
}
