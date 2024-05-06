using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Rendering;
using UnityEngine;
using BepInEx;
using HarmonyLib;
using BunnyModTest2;
using Valve.VR;

namespace BunnyModTest4.VRCamera
{
    public class GetActiveCamera : MonoBehaviour
    {
        // 直近で描画に利用したカメラ
        public Camera _currentCamera;
        public Camera beforeCamera;
        private void Start()
        {
            Debug.Log("GetActiveCamera Startaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            // カメラ描画イベントを購読する
            RenderPipelineManager.beginCameraRendering += WriteLogMessage;
        }
        private void OnDisable()
        {
            // カメラ描画イベントを解除する
            RenderPipelineManager.beginCameraRendering -= WriteLogMessage;
        }

        void WriteLogMessage(ScriptableRenderContext context, Camera camera)
        {
            if (camera.gameObject.name != "SecondEye")
            {
                //Debug.Log($"Beginning rendering the camera: {camera.name}");

                // 最新の描画用カメラとして登録する
                _currentCamera = camera;
            }

        }

        private void Update()
        {
            // 最後に描画したカメラを表示する
            //Debug.Log("_currentCamera : " + _currentCamera);

            if (_currentCamera != null && beforeCamera == null)
            {
                //Without this there is no headtracking
                _currentCamera.gameObject.AddComponent<SteamVR_TrackedObject>();
                Logs.WriteInfo("SteamVR_TrackedObject ok started");

                Plugin.SecondEye = new GameObject("SecondEye");
                Plugin.SecondCam = Plugin.SecondEye.AddComponent<Camera>();
                Plugin.SecondCam.gameObject.AddComponent<SteamVR_TrackedObject>();
                Plugin.SecondCam.CopyFrom(_currentCamera);

                // Without this the right eye gets stuck at a very far point in the map
                Plugin.SecondCam.transform.parent = _currentCamera.transform.parent;


                beforeCamera = _currentCamera;
            }
            else if (_currentCamera != beforeCamera)
            {
                Destroy(beforeCamera.gameObject.GetComponent<SteamVR_TrackedObject>());
                _currentCamera.gameObject.AddComponent<SteamVR_TrackedObject>();
                Plugin.SecondCam.transform.parent = _currentCamera.transform.parent;
                Debug.Log("New Changed beforeCamera : " + beforeCamera);

                beforeCamera = _currentCamera;
            }

        }
        private void OnDestroy()
        {
            Debug.Log("GetActiveCamera OnDestroyjdsfjkasfkladshfjkladshfjkadshfjkadshfjkasdhfkjladshfkljsadklfjsadkl;fjasdkl;jfkl;dsjl");
        }
    }


}
