using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers
{
    public static void SetLayerRecursively(GameObject gameObject, int newLayer, int? oldLayer = null)
    {
        if (gameObject == null)
        {
            return;
        }

        if (!oldLayer.HasValue || gameObject.layer == oldLayer.Value)
        {
            gameObject.layer = newLayer;
        }
        foreach (Transform child in gameObject.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
