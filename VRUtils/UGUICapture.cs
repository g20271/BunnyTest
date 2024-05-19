//using System.Collections.Generic;
//using System.Reflection;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Collections;

//using BunnyTestVR.Logging;
//using System.Linq;
//using UnityEngine.SceneManagement;

//namespace BunnyTestVR.VRUtils
//{
//    /// <summary>
//    /// This class captures uGUI elements and displays them on a RenderTexture.
//    /// </summary>
//    public class UGUICapture : MonoBehaviour
//    {
//        /// <summary>
//        /// Creates and initializes a UGUICapture instance.
//        /// </summary>
//        /// <param name="parentGameObject">Parent game object.</param>
//        /// <param name="name">Name.</param>
//        /// <param name="layer">Working layer occupied for capture.</param>
//        /// <returns>A new instance of UGUICapture.</returns>
//        public static UGUICapture Create(GameObject parentGameObject, string name, int layer)
//        {
//            var gameObject = new GameObject($"{parentGameObject.name}{name}");
//            // Ensure the lifecycle of the GameObject is synchronized with its parent.
//            gameObject.transform.parent = parentGameObject.transform;
//            gameObject.SetActive(false);
//            var result = gameObject.AddComponent<UGUICapture>();
//            result.Layer = layer;
//            gameObject.SetActive(true);
//            return result;
//        }

//        private int Layer { get; set; }
//        public RenderTexture Texture { get; private set; }
//        //private Dictionary<Canvas, List<Graphic>> CanvasGraphics { get; set; }
//        private List<Canvas> Canvases { get; set; }
//        private ISet<Canvas> ProcessedCanvas { get; set; } = new HashSet<Canvas>();

//        void Start()
//        {
//            PluginLog.Debug($"Awake: {name}");

//            Texture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);

//            // Creates a camera to project the captured uGUI elements onto the RenderTexture.
//            var camera = gameObject.AddComponent<Camera>();
//            camera.cullingMask = 1 << Layer;
//            camera.depth = float.MaxValue;
//            camera.nearClipPlane = -1000f;
//            camera.farClipPlane = 1000f;
//            camera.targetTexture = Texture;
//            camera.backgroundColor = Color.clear;
//            camera.clearFlags = CameraClearFlags.Color;
//            camera.orthographic = true;
//            camera.useOcclusionCulling = false;
//            //var type = GraphicRegistry.instance.GetType();
//            //CanvasGraphics = (Dictionary<Canvas, List<Graphic>>)type.GetProperty("m_Graphics");
//            //CanvasGraphics = GraphicRegistry.instance.;

//            //FieldInfo field = typeof(GraphicRegistry).GetField("m_Graphics", BindingFlags.NonPublic | BindingFlags.Instance);
//            //if (field != null)
//            //{
//            //    CanvasGraphics = (Dictionary<Canvas, Graphic>)field.GetValue(null);
//            //}
//            //else
//            //{
//            //    PluginLog.Error("Failed to get m_Graphics field from GraphicRegistry");
//            //}
//            Canvases = new List<Canvas>();
//            for (int i = 0; i < SceneManager.sceneCount; i++)
//            {
//                var scene = SceneManager.GetSceneAt(i);
//                Canvases.AddRange(
//                    scene.GetRootGameObjects()
//                        .SelectMany(root => root.GetComponentsInChildren<Canvas>(true))
//                );
//            }
//            var go = new GameObject("TempDontDestroyObject");
//            DontDestroyOnLoad(go);

//            Canvases.AddRange(go.scene.GetRootGameObjects()
//                .SelectMany(root => root.GetComponentsInChildren<Canvas>(true))
//            );

//            DestroyImmediate(go);


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

//        void Update()
//        {
//            // Redirects the uGUI canvas output to a pre-prepared RenderTexture for capturing.
//            var camera = GetComponent<Camera>();
//            //foreach (var canvas in CanvasGraphics.Keys)
//            //{
//            //    if (canvas.enabled && (!ProcessedCanvas.Contains(canvas) || canvas.renderMode != RenderMode.ScreenSpaceCamera || canvas.worldCamera != camera))
//            //    {
//            //        PluginLog.Debug($"Add canvas to capture target: {canvas.name} in {canvas.gameObject.layer}:{LayerMask.LayerToName(canvas.gameObject.layer)}");
//            //        ProcessedCanvas.Add(canvas);
//            //        canvas.renderMode = RenderMode.ScreenSpaceCamera;
//            //        canvas.worldCamera = camera;
//            //        foreach (var i in canvas.gameObject.GetComponentsInChildren<Transform>()) i.gameObject.layer = Layer;
//            //    }
//            //}

//            foreach (var canvas in Canvases)
//            {
//                if (canvas.enabled && (!ProcessedCanvas.Contains(canvas) || canvas.renderMode != RenderMode.ScreenSpaceCamera || canvas.worldCamera != camera))
//                {
//                    PluginLog.Debug($"Add canvas to capture target: {canvas.name} in {canvas.gameObject.layer}:{LayerMask.LayerToName(canvas.gameObject.layer)}");
//                    ProcessedCanvas.Add(canvas);
//                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
//                    canvas.worldCamera = camera;
//                    foreach (var i in canvas.gameObject.GetComponentsInChildren<Transform>()) i.gameObject.layer = Layer;
//                }
//            }
//        }
//    }
//}
