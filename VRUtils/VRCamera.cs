﻿
using UnityEngine;
using Valve.VR;

using BunnyTestVR.Logging;
using UnityEngine.Rendering.Universal;

namespace BunnyTestVR.VRUtils
{
    /// <summary>
    /// A VR camera capable of projecting images onto the HMD and supporting HMD tracking.
    /// 
    /// The internal structure of the game objects is as follows:
    /// - parentGameObject
    ///   - Origin: VR Camera's origin
    ///     - Camera: Normal and VR Camera
    /// </summary>
    public class VRCamera : MonoBehaviour
    {

        public static VRCamera Create(GameObject parentGameObject, string name, int depth)
        {
            PluginLog.Debug($"Create VRCamera: {name}");
            var gameObject = new GameObject($"{parentGameObject.name}{name}Origin");
            // Ensure the lifecycle of the GameObject is synchronized with its parent.
            gameObject.transform.parent = parentGameObject.transform;
            gameObject.SetActive(false);
            var result = gameObject.AddComponent<VRCamera>();
            result.Depth = depth;
            gameObject.SetActive(true);
            return result;
        }

        public static bool IsBaseHeadSet { get; private set; } = false;
        public static Vector3 BaseHeadPosition { get; private set; } = Vector3.zero;
        public static Quaternion BaseHeadRotation { get; private set; } = Quaternion.identity;

        /// <summary>
        /// Sets the current head position and rotation as the center point for future viewpoints.
        /// </summary>
        public static void UpdateViewport(VRCamera vrCamera)
        {
            IsBaseHeadSet = true;
            BaseHeadPosition = vrCamera.VR.head.localPosition;
            var orientationEulerAngles = vrCamera.VR.head.localRotation.eulerAngles;
            //BaseHeadRotation = Quaternion.Euler(
            //    PluginConfig.ReflectHMDRotationXOnViewport.Value ? orientationEulerAngles.x : 0,
            //    PluginConfig.ReflectHMDRotationYOnViewport.Value ? orientationEulerAngles.y : 0,
            //    PluginConfig.ReflectHMDRotationZOnViewport.Value ? orientationEulerAngles.z : 0);
        }

        private int Depth { get; set; }
        private GameObject CameraObject { get; set; }
        public Camera Normal { get; private set; }
        public UniversalAdditionalCameraData NormalAddCameraData { get; private set; }
        public SteamVR_Camera VR { get; private set; }

        void Start()
        {
            PluginLog.Info($"jsafklsdafjklsdajfklasdjflksadjflk;jasdlkfjasdlkfjadsl;kj;kljAwake: {name}");
            Setup();
        }

        void OnDestroy()
        {
            PluginLog.Debug($"OnDestroy: {name}");
        }

        private void Setup()
        {
            if (!CameraObject)
            {
                CameraObject = new GameObject($"{name}Camera");
                // Ensure the lifecycle of the GameObject is synchronized with its parent.
                CameraObject.transform.parent = gameObject.transform;
            }

            // Prepare a VR camera separate from the game camera to minimize the impact on the game.
            if (!CameraObject.GetComponent<Camera>())
            {
                Normal = CameraObject.AddComponent<Camera>();
                Normal.depth = Depth;
            }
            if (!CameraObject.GetComponent<UniversalAdditionalCameraData>())
            {
                NormalAddCameraData = CameraObject.AddComponent<UniversalAdditionalCameraData>();
                NormalAddCameraData.renderPostProcessing = true;
                NormalAddCameraData.renderShadows = true;
            }

            // By combining Camera and SteamVR_Camera, the player can see the camera's view from the HMD.
            if (!CameraObject.GetComponent<SteamVR_Camera>()) VR = CameraObject.AddComponent<SteamVR_Camera>();
            // When SteamVR_TrackedObject is also combined, the camera moves with the movement of the HMD.
            if (!CameraObject.GetComponent<SteamVR_TrackedObject>()) CameraObject.AddComponent<SteamVR_TrackedObject>();

            // After that, just move the camera as you like.
            // This project camera usage is just one example.

        }

        /// <summary>
        /// Hijacks the viewpoint of a camera and displays it through the VR camera.
        /// </summary>
        /// <param name="targetCamera">The target camera.</param>
        /// <param name="useCopyFrom">If true, copies the camera settings using Camera.CopyFrom. Specify false to adjust the camera settings independently.</param>
        /// <param name="synchronization">If true, synchronizes some of the camera settings in real-time. Refer to CameraHijacker.Synchronize for detailed synchronization content.</param>
        public void Hijack(Camera targetCamera, bool useCopyFrom = true, bool synchronization = true)
        {
            Setup();

            if (targetCamera != null)
            {
                CameraHijacker.Hijack(targetCamera, Normal, useCopyFrom, synchronization);

                // Set origin to the inverse position of the base head from the target camera.
                // The origin of the VR camera is the center of the play area (Usually at the player's feet).
                //VR.origin.rotation = targetCamera.transform.rotation * Quaternion.Inverse(BaseHeadRotation);
                VR.origin.rotation = Quaternion.Euler(0, targetCamera.transform.rotation.eulerAngles.y, 0) * Quaternion.Inverse(BaseHeadRotation);

                VR.origin.position = targetCamera.transform.position - VR.origin.rotation * BaseHeadPosition;
                //VR.origin.position = new Vector3(targetCamera.transform.position.x, BaseHeadPosition.y, targetCamera.transform.position.z) - VR.origin.rotation * BaseHeadPosition;

                VR.origin.SetParent(targetCamera.transform);
            }

            Normal.depth = Depth;
        }
    }
}
