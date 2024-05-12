using UnityEngine;
using UnityEngine.Rendering;

using BunnyModTest2.Logging;
using System.Collections.Generic;

namespace BunnyModTest2.VRUtils
{
    /// <summary>
    /// Hijack the camera's view to another camera.
    /// To minimize the impact, the original camera remains enabled and is virtually disabled by adjusting CullingMask before and after drawing.
    /// </summary>
    public class CameraHijacker : MonoBehaviour
    {

        /// <summary>
        /// Hijack the camera's view to another camera.
        /// </summary>
        /// <param name="source">The source camera.</param>
        /// <param name="destination">The destination camera. If null, the function only disables the source camera without redirecting its view.</param>
        /// <param name="useCopyFrom">If true, copies the camera settings using Camera.CopyFrom.</param>
        /// <param name="synchronization">If true, synchronizes some of the camera settings in real-time. Refer to CameraHijacker.Synchronize for detailed synchronization content.</param>
        public static void Hijack(Camera source, Camera destination = null, bool useCopyFrom = true, bool synchronization = true)
        {
            if (source != null)
            {
                PluginLog.Debug(destination ? $"Hijack {source.name} to {destination?.name}" : $"Hijack {source.name}");
                if (destination && useCopyFrom) destination.CopyFrom(source);
                var hijacker = source.GetComponent<CameraHijacker>();
                if (hijacker == null) hijacker = source.gameObject.AddComponent<CameraHijacker>();
                if (destination && synchronization) hijacker.Destination = destination;
            }
        }

        private Camera Destination { get; set; }
        private LayerMask LastCullingMask { get; set; }
        private CameraClearFlags LastClearFlags { get; set; }

        System.Action<ScriptableRenderContext, List<Camera>> onBeginContextRendering;
        System.Action<ScriptableRenderContext, List<Camera>> onEndContextRendering;

        void Awake()
        {
            onBeginContextRendering = OnBeginContextRendering;
            onEndContextRendering = OnEndContextRendering;
        }

        void OnEnable()
        {
            RenderPipelineManager.beginContextRendering += onBeginContextRendering;
            RenderPipelineManager.endContextRendering += onEndContextRendering;
        }

        /// <inheritdoc />
        void OnDisable()
        {
            RenderPipelineManager.beginContextRendering -= onBeginContextRendering;
            RenderPipelineManager.endContextRendering -= onEndContextRendering;
        }


        void OnBeginContextRendering(ScriptableRenderContext context, List<Camera> cameras)
        {
            var camera = GetComponent<Camera>();
            if (camera != null)
            {
                LastCullingMask = camera.cullingMask;
                LastClearFlags = camera.clearFlags;
                camera.cullingMask = 0;
                camera.clearFlags = CameraClearFlags.Nothing;
                if (Destination != null) Synchronize();
            }
        }


        void OnEndContextRendering(ScriptableRenderContext context, List<Camera> cameras)
        {
            var camera = GetComponent<Camera>();
            if (camera != null)
            {
                camera.cullingMask = LastCullingMask;
                camera.clearFlags = LastClearFlags;
            }
        }

        private void Synchronize()
        {
            Destination.cullingMask = LastCullingMask;
            Destination.clearFlags = LastClearFlags;
        }
    }
}
