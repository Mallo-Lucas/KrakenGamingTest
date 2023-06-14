using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStaticFunctions
{
    public static bool IsGoInLayerMask(GameObject go, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << go.layer));
    }
}
