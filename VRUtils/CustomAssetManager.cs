//using System.IO;

//using UnityEngine;

//using BunnyModTest2.Logging;

//namespace BunnyModTest2.VRUtils
//{
//    public class CustomAssetManager
//    {
//        private static AssetBundle Bundle { get; set; }
//        public static AssetBundle GetBundle()
//        {
//            if (Bundle == null)
//            {
//                Bundle = AssetBundle.LoadFromMemory(ReadAllBytes("C:/Users/hirot/BunnyModTest5/AssetBundles/custom_asset_bundle"));
//                foreach (var i in Bundle.GetAllAssetNames()) PluginLog.Debug($"Available custom_asset: {i}");
//            }
//            return Bundle;
//        }

//        //public static Shader UiUnlitTransparentShader { get => GetBundle().LoadAsset("assets/assetbundles/ui-unlit-transparent.shader").Cast<Shader>(); }

//        private static byte[] ReadAllBytes(string resourceName)
//        {
//            var assembly = typeof(CustomAssetManager).Assembly;
//            using (var stream = assembly.GetManifestResourceStream(resourceName))
//            using (var memoryStream = new MemoryStream())
//            {
//                stream.CopyTo(memoryStream);
//                return memoryStream.ToArray();
//            }
//        }
//    }
//}
