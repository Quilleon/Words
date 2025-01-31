using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterButton : MonoBehaviour
{
    [SerializeField] private bool hasLetter = true;
    private string letter;

    private MiniController miniController;

    private void Start()
    {
        if (hasLetter)
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

    public void OnBackspacePress()
    {
        // If there is a character there
        if (miniController.ActiveMiniTileSorting.selected.text != "") miniController.ActiveMiniTileSorting.selected.text = ""; // Set selected to empty
        else miniController.ActiveMiniTileSorting.SelectNextBox(false);
        
        
        
        
        //miniController.ActiveMiniTileSorting.selected.onValueChanged.Invoke("");
        
        //miniController.ActiveMiniTileSorting.SelectNextBox(false);
    }
}
