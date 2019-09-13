using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers
{
    public static void SetLayerRecursively(GameObject gameObject, int newLayer)
    {
        if (gameObject == null)
        {
            return;
        }

        gameObject.layer = newLayer;
        foreach (Transform child in gameObject.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
