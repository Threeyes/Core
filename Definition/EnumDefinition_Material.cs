using UnityEngine;

namespace Threeyes.Core
{
    /// <summary>
    /// 提供常用ShaderProperty的ID，避免设置材质时多余的调用
    /// Ref：UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Rendering.ColorMaterialPropertyAffordanceReceiver.ShaderPropertyLookup
    /// 
    /// </summary>
    public readonly struct ShaderPropertyCollection
    {
        public static readonly int BaseColorID = Shader.PropertyToID(
#if UNITY_PIPELINE_URP || UNITY_PIPELINE_HDRP
       "_BaseColor"
#else
       "_Color"
#endif
            );
        public static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
    }

    /// <summary>
    /// Material Float Type
    /// </summary>
    public enum MaterialFloatType
    {
        Custom = 1,
        [MaterialPropertyName("_Cutoff", "_Cutoff", "_Cutoff")]
        AlphaCutoff,//Set the alpha value limit which determine whether it should render each pixel.
        [MaterialPropertyName("_Metallic", "_Metallic", "_Metallic")]
        Metallic,//determines how "metal-like" the surface is
        [MaterialPropertyName("_Glossiness", "_Smoothness", "_Smoothness")]
        Smoothness,//control the “microsurface detail” or smoothness across a surface   
        [MaterialPropertyName("_BumpScale", "_BumpScale", "_NormalScale")]
        NormalScale,//Scale for NormalMap
        [MaterialPropertyName("_Parallax", "_Parallax", "_HeightOffset")]
        HeightScale,//Scale for HeightMap
        [MaterialPropertyName("_OcclusionStrength", "_OcclusionStrength", null)]
        OcclusionStrength//Strength for OcclusionMap
    }

    /// <summary>
    /// Material Color Type
    /// </summary>
    public enum MaterialColorType
    {
        Custom = 1,
        [MaterialPropertyName("_Color", "_BaseColor", "_BaseColor")]
        BaseColor,

        ///PS:
        ///1. [URP] has HDR keywork
        [MaterialPropertyName("_EmissionColor", "_EmissionColor", "_EmissionColor")]
        EmissionColor,
    }

    /// <summary>
    /// 贴图名称（用于Offset/Tilling等）
    /// </summary>
    public enum MaterialTextureType
    {
        Custom = 1,
        [MaterialPropertyName("_MainTex", "_BaseMap", "_BaseColorMap")]
        BaseMap,
        [MaterialPropertyName("_BumpMap", "_BumpMap", "_NormalMap")]
        NormalMap,
        [MaterialPropertyName("_ParallaxMap", "_ParallaxMap", "_HeightMap")]
        HeightMap,

        ///PS:
        ///1. [HDRP]: uses the green channel of this map to calculate ambient occlusion.
        [MaterialPropertyName("_OcclusionMap", "_OcclusionMap", "_MaskMap")]
        OcclusionMap,
        [MaterialPropertyName("_EmissionMap", "_EmissionMap", "_EmissiveColorMap")]
        EmissionMap,
    }

    /// <summary>
    /// Material Vector2 Type, mainly use for offset and tiling
    /// </summary>
    public enum MaterialVector2Type
    {
        Custom = 1,
        Offset,
        Tiling
    }

    /// <summary>
    /// Material Vector4 Type
    /// </summary>
    public enum MaterialVector4Type
    {
        Custom = 1,
    }
}