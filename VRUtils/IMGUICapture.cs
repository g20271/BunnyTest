﻿//using UnityEngine;

//using BunnyTestVR.Logging;

//namespace BunnyTestVR.VRUtils
//{
//    /// <summary>
//    /// This class captures IMGUI elements and displays them on a RenderTexture.
//    /// </summary>
//    public class IMGUICapture : MonoBehaviour
//    {

//        public static IMGUICapture Create(GameObject parentGameObject, string name)
//        {
//            var gameObject = new GameObject($"{parentGameObject.name}{name}");
//            // Ensure the lifecycle of the GameObject is synchronized with its parent.
//            gameObject.SetActive(false);
//            var result = gameObject.AddComponent<IMGUICapture>();
//            gameObject.AddComponent<FirstGUIEventProcessor>();
//            gameObject.AddComponent<LastGUIEventProcessor>();
//            gameObject.SetActive(true);
//            return result;
//        }

//        public RenderTexture LastTexture { get; private set; }
//        public RenderTexture Texture { get; private set; }

//        void Awake()
//        {
//            PluginLog.Debug($"Awake: {name}");

//            Texture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
//        }

//        void OnDestroy()
//        {
//            PluginLog.Debug($"OnDestroy: {name}");

//            if (Texture != null)
//            {
//                Texture.Release();
//                Texture = null;
//            }
//        }

//        /// <summary>
//        /// Captures the GUI elements by setting the RenderTexture at the beginning of the GUI rendering process.
//        /// </summary>
//        private class FirstGUIEventProcessor : MonoBehaviour
//        {
//            void OnGUI()
//            {
//                GUI.depth = int.MaxValue;
//                if (Event.current.type == EventType.Repaint)
//                {
//                    var capture = GetComponent<IMGUICapture>();
//                    if (capture)
//                    {
//                        capture.LastTexture = RenderTexture.active;
//                        RenderTexture.active = capture.Texture;
//                        GL.Clear(true, true, Color.clear);
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// Restores the original RenderTexture after capturing the GUI elements.
//        /// </summary>
//        private class LastGUIEventProcessor : MonoBehaviour
//        {
//            void OnGUI()
//            {
//                GUI.depth = int.MinValue;
//                if (Event.current.type == EventType.Repaint)
//                {
//                    var capture = GetComponent<IMGUICapture>();
//                    if (capture) RenderTexture.active = capture.LastTexture;
//                }
//            }
//        }
//    }
//}
