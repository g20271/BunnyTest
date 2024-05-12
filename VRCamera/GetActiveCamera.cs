using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Rendering;
using UnityEngine;
using BepInEx;
using HarmonyLib;
using BunnyModTest2;
using BunnyModTest2.Logging;
using Valve.VR;
using UnityEngine.Rendering;

namespace BunnyModTest2.VRGet
{
    public class GetActiveCamera : MonoBehaviour
    {
        // 直近で描画に利用したカメラ
        public Camera _currentCamera;
        public Camera beforeCamera;
        private void Start()
        {
            PluginLog.Info("GetActiveCamera Start");
            // カメラ描画イベントを購読する
            RenderPipelineManager.endCameraRendering += WriteLogMessage;
        }
        private void OnDisable()
        {
            // カメラ描画イベントを解除する
            RenderPipelineManager.endCameraRendering -= WriteLogMessage;
        }

        void WriteLogMessage(ScriptableRenderContext context, Camera camera)
        {
            if (camera.gameObject.name != "SimpleVRControllerTitleSceneMainVRCameraOriginCamera (eye)")
            {
                //PluginLog.Info($"Beginning rendering the camera: {camera.name}");

                // 最新の描画用カメラとして登録する
                _currentCamera = camera;
                Plugin.RenderMainCamera = _currentCamera;
            }

        }

        private void LateUpdate()
        {
            // 最後に描画したカメラを表示する
            //Debug.Log("_currentCamera : " + _currentCamera);

            if (_currentCamera != null && beforeCamera == null)
            {
                //PluginLog.Info(_currentCamera.gameObject.name);
                ////Without this there is no headtracking
                //_currentCamera.gameObject.AddComponent<SteamVR_TrackedObject>();
                //PluginLog.Info("SteamVR_TrackedObject ok started");

                //Plugin.SecondEye = new GameObject("SecondEye");
                //Plugin.SecondCam = Plugin.SecondEye.AddComponent<Camera>();
                //Plugin.SecondCam.gameObject.AddComponent<SteamVR_TrackedObject>();
                //Plugin.SecondCam.CopyFrom(_currentCamera);

                //// Without this the right eye gets stuck at a very far point in the map
                //Plugin.SecondCam.transform.parent = _currentCamera.transform.parent;

                //PluginLog.Info(_currentCamera.gameObject.name);


                beforeCamera = _currentCamera;
            }
            else if (_currentCamera != beforeCamera)
            {
                //Destroy(beforeCamera.gameObject.GetComponent<SteamVR_TrackedObject>());
                //_currentCamera.gameObject.AddComponent<SteamVR_TrackedObject>();
                //Plugin.SecondCam.transform.parent = _currentCamera.transform.parent;
                //Debug.Log("New Changed beforeCamera : " + beforeCamera);

                beforeCamera = _currentCamera;
            }

        }
        private void OnDestroy()
        {
            PluginLog.Info("GetActiveCamera OnDestroy");
        }
    }


}
