using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        var tileSorting = miniController.ActiveMiniTileSorting;
        
        tileSorting.selected.text = letter;
        tileSorting.SelectNextBox(true);
        
        //tileSorting.selected.onValueChanged.Invoke(letter); // Activate onValueChanged even if you write the same letter
    }

    public void OnBackspacePress()
    {
        var tileSorting = miniController.ActiveMiniTileSorting;
        
        // If there is a character there
        if (tileSorting.selected.text != "") 
            tileSorting.selected.text = ""; // Set selected to empty
        else // If not
        {
            tileSorting.SelectNextBox(false);
            tileSorting.selected.text = "";
        }
        
        
        
        
        //miniController.ActiveMiniTileSorting.selected.onValueChanged.Invoke("");
        
        //miniController.ActiveMiniTileSorting.SelectNextBox(false);
    }
}
