using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// layerNumber：序号，从0开始
/// LayerMask：2的序号次方
/// 
/// 常见数值（layerNumber：LayerMask）：
/// -1：0 （Nothing）
/// 0：1 （Default）
/// 1：2（TransparentFX）
/// ：~0（Everything）
/// </summary>
public static class LayerMaskTool
{
    public static LayerMask layerMask_Nothing { get { return 0; } }
    public static LayerMask layerMask_Everything { get { return ~0; } }

    public static int GetLayerNumber(LayerMask layerMask)
    {
        return (int)Mathf.Log(layerMask.value, 2);
    }
    public static int GetLayerMask(int layerNumber)
    {
        return 1 << layerNumber;   // means take 1 and rotate it left by "layer" bit positions
    }

    public static bool IsGameObjectInMask(GameObject a, LayerMask m)
    {
        return ((1 << a.layer) & m) != 0;
    }
}
