using System;
using System.Collections;

using BepInEx;
using Unity.XR.OpenVR;
using UnityEngine;
using Valve.VR;
using UnityEngine.XR.Management;

using BunnyTestVR.Logging;
using UnityEngine.XR.OpenXR;

namespace BunnyTestVR.VRUtils
{
    public class VR : MonoBehaviour
    {
        public static UnityEngine.XR.Management.XRManagerSettings managerSettings = null;
        public static bool Initialized { get; private set; } = false;

        public static void Initialize(Action actionAfterInitialization, bool force = false)
        {
            if (force || !Initialized)
            {
                Initialized = false;
                ActionAfterInitialization = actionAfterInitialization;
                new GameObject(nameof(VR)) { hideFlags = HideFlags.HideAndDontSave }.AddComponent<VR>();
            }
        }

        private static Action ActionAfterInitialization { get; set; }

        void Start()
        {
            this.StartCoroutine(Setup());
        }

        private IEnumerator Setup()
        {
            PluginLog.Info("Start Setup");

            try
            {
                try
                {
                    // Initialize the OpenVR Display and OpenVR Input submodules.
                    var vrLoader = ScriptableObject.CreateInstance<OpenVRLoader>();
                    if (vrLoader.Initialize())
                    {
                        PluginLog.Info("OpenVRLoader.Initialize succeeded.");
                    }
                    else
                    {
                        PluginLog.Error("OpenVRLoader.Initialize failed.");
                        yield break;
                    }

                    // Start the OpenVR Display and OpenVR Input submodules.
                    if (vrLoader.Start())
                    {
                        PluginLog.Info("OpenVRLoader.Start succeeded.");
                    }
                    else
                    {
                        PluginLog.Error("OpenVRLoader.Start failed.");
                        yield break;
                    }
                    //XRGeneralSettings.AttemptInitializeXRSDKOnLoad();
                    //XRGeneralSettings.AttemptStartXRSDKOnBeforeSplashScreen();
                    // Initialize SteamVR.
                    //SteamVR_Behaviour.Initialize(false);
                    SteamVR_Actions.PreInitialize();


                    //var generalSettings = ScriptableObject.CreateInstance<XRGeneralSettings>();
                    //managerSettings = ScriptableObject.CreateInstance<XRManagerSettings>();
                    //var xrLoader = ScriptableObject.CreateInstance<OpenVRLoader>();


                    //var settings = OpenVRSettings.GetSettings();
                    //settings.StereoRenderingMode = OpenVRSettings.StereoRenderingModes.MultiPass;


                    //generalSettings.Manager = managerSettings;

                    //managerSettings.loaders.Clear();
                    //managerSettings.loaders.Add(xrLoader);

                    //managerSettings.InitializeLoaderSync();


                    //XRGeneralSettings.AttemptInitializeXRSDKOnLoad();
                    //XRGeneralSettings.AttemptStartXRSDKOnBeforeSplashScreen();



                    SteamVR_Behaviour.Initialize();
                    //SteamVR.Initialize();
                }
                catch (Exception e)
                {
                    throw new Exception("SteamVR initialization error.", e);
                }

                // Wait for initialization.
                while (true)
                {
                    switch (SteamVR.initializedState)
                    {
                        case SteamVR.InitializedStates.InitializeSuccess:
                            PluginLog.Info("SteamVR initialization succeeded.");
                            break;
                        case SteamVR.InitializedStates.InitializeFailure:
                            PluginLog.Error("SteamVR initialization failed.");
                            yield break;
                        default:
                            yield return new WaitForSeconds(0.1f);
                            continue;
                    }

                    break;
                }

                Initialized = true;
                ActionAfterInitialization?.Invoke();
            }
            finally
            {
                PluginLog.Info("Finish Setup");
                Destroy(gameObject);
            }
        }
    }
}
