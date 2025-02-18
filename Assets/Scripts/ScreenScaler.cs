using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScaler : MonoBehaviour
{
    [SerializeField] private bool phoneBuild;
    [SerializeField] private float phoneScale = .91f, windowsScale = .25f;

    [SerializeField] private bool validate;
    private void OnValidate()
    {
        if (!validate) return;

        validate = false;

        transform.localScale = new Vector3(1, 1, 1) * (phoneBuild ? phoneScale : windowsScale);
    }
}
