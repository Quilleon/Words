using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterButton : MonoBehaviour
{
    private string letter;

    private MiniController miniController;

    private void Start()
    {
        letter = GetComponentInChildren<TMP_Text>().text;

        miniController = GetComponentInParent<AssignLetters>().transform.parent
            .GetComponentInChildren<MiniController>();
        
    }

    public void OnPress()
    {
        miniController.ActiveMiniTileSorting.selected.text = letter;
    }
}
