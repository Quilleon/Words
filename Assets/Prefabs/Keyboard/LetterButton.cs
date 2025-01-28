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

        // Get active mini controller
        miniController = GetComponentInParent<AssignLetters>().transform.parent
            .GetComponentInChildren<MiniController>();
        
    }

    public void OnPress()
    {
        miniController.ActiveMiniTileSorting.selected.text = letter;
        miniController.ActiveMiniTileSorting.selected.onValueChanged.Invoke(letter); // Activate onValueChanged even if you write the same letter
        //TODO: Should fix for keyboard also
    }
}
