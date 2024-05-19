using BunnyTestVR;
using HarmonyLib;
using UnityEngine.Rendering.Universal;

namespace ForceTaa;

[HarmonyPatch]
[HarmonyPatch(
    typeof(UniversalRenderPipelineAsset),
    nameof(UniversalRenderPipelineAsset.msaaSampleCount),
    MethodType.Setter
)]
public static class CameraSetPatch
{
    [HarmonyPostfix]
    static void Prefix(ref MsaaQuality value)
    {
        Logs.WriteInfo(
            "intercepted UnityEngine.Rendering.Universal.UniversalAdditionalCameraData antialiasing setter"
        );
        value = MsaaQuality._8x;
    }
}