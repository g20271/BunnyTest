using System.Collections;
using System.Text.RegularExpressions;

using BepInEx;
using UnityEngine;

using BunnyTestVR.Logging;
using BunnyTestVR.VRUtils;

namespace BunnyTestVR
{
    public class SimpleVRController : MonoBehaviour
    {

        // Layer: These layers should be exclusive to the VR UI to prevent interference with game's main rendering layers.
        // for UGUI capture work
        public const int UGUI_CAPTURE_LAYER = 15;
        // for UI screen camera
        public const int UI_SCREEN_LAYER = 30;

        // Camera depth: Set the output after the game camera.
        public const int MAIN_VR_CAMERA_DEPTH = 1000;
        public const int UI_SCREEN_CAMERA_DEPTH = MAIN_VR_CAMERA_DEPTH + 10;

        // Distance from player to UI screen, in meters.
        public const float UI_SCREEN_DISTANCE = 1.0f;

        public VRCamera MainVRCamera { get; private set; }
        public VRController MainVRController { get; private set; }
        //public UIScreen UIScreen { get; private set; }

        Camera _beforeCamera = null;

        void Start()
        {
            PluginLog.Debug($"Awake: {name}");

            // MainVRCamera is a camera that displays 3D space.
            MainVRCamera = VRCamera.Create(gameObject, nameof(MainVRCamera), MAIN_VR_CAMERA_DEPTH);
            MainVRController = VRController.Create(gameObject, nameof(MainVRController), MAIN_VR_CAMERA_DEPTH);

            // UIScreen displays the texture as a screen using the built-in VRCamera.
            // Also projects the mouse cursor onto the 3D screen.
            //UIScreen = UIScreen.Create(gameObject, nameof(UIScreen), UI_SCREEN_CAMERA_DEPTH, UI_SCREEN_LAYER,
            //    new UIScreenPanel[] {
            //        new(UGUICapture.Create(gameObject, nameof(UGUICapture), UGUI_CAPTURE_LAYER).Texture),
            //        //new(IMGUICapture.Create(gameObject, nameof(IMGUICapture)).Texture, -0.001f * Vector3.forward, Vector3.one),
            //    });

            this.StartCoroutine(Setup());
        }

        void OnDestroy()
        {
            PluginLog.Debug($"OnDestroy: {name}");
        }

        IEnumerator Setup()
        {
            // Delays setup to ensure accurate HMD tracking initialization.
            yield return new WaitForSeconds(0.1f);

            UpdateCamera(false);
        }

        void UpdateCamera(bool forceUpdateOrientationPose)
        {
            if (!VRCamera.IsBaseHeadSet || forceUpdateOrientationPose) VRCamera.UpdateViewport(MainVRCamera);

            // Redirects the main game camera's view to the MainVRCamera
            // and positions the UIScreen at a specified distance in front of it,
            // ensuring it remains static relative to the player's head movement.
            //MainVRCamera.Hijack(Camera.main);
            //if (GameObject.Find("GameCamera") != null)
            //{
            //    Logs.WriteInfo("GameCamera found");
            //    GameObject mainCamObj = GameObject.Find("GameCamera");
            //    MainVRCamera.Hijack(mainCamObj.GetComponent<Camera>());
            //    //UIScreen.LinkToFront(MainVRCamera, UI_SCREEN_DISTANCE);
            //}
            if (Plugin.RenderMainCamera == null)
            {
                PluginLog.Error($"Missing Camera");
                MainVRCamera.Hijack(Camera.main);


            }
            else
            {
                if (_beforeCamera != Plugin.RenderMainCamera)
                {
                    VRCamera.UpdateViewport(MainVRCamera);
                    _beforeCamera = Plugin.RenderMainCamera;
                    //UIScreen.LinkToFront(MainVRCamera, UI_SCREEN_DISTANCE);
                }
                MainVRCamera.Hijack(Plugin.RenderMainCamera);

            }

        }

        private float LastClickTime { get; set; } = -1f;

        void Update()
        {
            UpdateViewport();
            OptimizeVRExperience();
        }

        void UpdateViewport()
        {
            UpdateCamera(false);
            // Update the viewport with a double-click of the right mouse button.
            //if (0f < PluginConfig.DoubleClickIntervalToUpdateViewport.Value)
            ////if (0f < PluginConfig.DoubleClickIntervalToUpdateViewport.Value && Input.GetMouseButtonDown(1))
            //{
            //    var currentTime = Time.time;
            //    if (currentTime - LastClickTime <= PluginConfig.DoubleClickIntervalToUpdateViewport.Value)
            //    {
            //        UpdateCamera(true);
            //        LastClickTime = 0f;
            //    }
            //    else
            //    {
            //        LastClickTime = currentTime;
            //    }
            //}
        }

        void OptimizeVRExperience()
        {
            //// #2: Improved the discomfort between the left and right eyes: Shadows and lights.
            //if (PluginConfig.DisableLights.Value)
            //{
            //    foreach (var i in FindObjectsOfType<Light>())
            //    {
            //        if (i.shadows != LightShadows.None)
            //        {
            //            PluginLog.Info($"Disable Light shadows: {i.name}");
            //            i.shadows = LightShadows.None;
            //        }
            //        if (i.enabled && (i.type == LightType.Spot || i.type == LightType.Point))
            //        {
            //            PluginLog.Info($"Disable Light: {i.name}");
            //            i.enabled = false;
            //        }
            //    }
            //}

            //// #3: Improved the discomfort between the left and right eyes: Plants.
            //if (PluginConfig.DisableLODGroups.Value)
            //{
            //    foreach (var i in FindObjectsOfType<LODGroup>())
            //    {
            //        if (1 < i.lodCount)
            //        {
            //            PluginLog.Info($"Disable LODGroup: {i.name}");
            //            i.SetLODs(new LOD[] { i.GetLODs()[0] });
            //            i.RecalculateBounds();
            //        }
            //    }
            //}

            //// #10: Improved the discomfort between the left and right eyes: Particles.
            //if (PluginConfig.DisableParticleSystems.Value)
            //{
            //    foreach (var i in FindObjectsOfType<ParticleSystem>())
            //    {
            //        if (Regex.IsMatch(i.name, PluginConfig.ParticleNameDisableRegex.Value))
            //        {
            //            PluginLog.Info($"Disable ParticleSystem: {i.name}");
            //            i.gameObject.SetActive(false);
            //        }
            //    }
            //}
        }
    }
}
