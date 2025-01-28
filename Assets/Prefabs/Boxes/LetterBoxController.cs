using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LetterBoxController : MonoBehaviour
{
    private TMP_InputField _tmpInput;
    private TileSorting _tileSorting;
    private Button _button;

    //[SerializeField] private TMP_InputField northBox, southBox, eastBox, westBox;
    //[SerializeField] private TMP_InputField nextBox;


    public char correctChar; //{ get; private set; }

    #region Functions for Tile Sorting script

    public void AssignCorrectChar()
    {
        _tmpInput = GetComponentInChildren<TMP_InputField>(); // Needed for validate

        correctChar = _tmpInput.text.ToCharArray()[0];
    }
    
    public void ClearCharacter()
    {
        _tmpInput = GetComponentInChildren<TMP_InputField>(); // Needed for validate
        _tmpInput.text = null;
    }

    public void DisplayCorrectChar()
    {
        _tmpInput = GetComponentInChildren<TMP_InputField>(); // Needed for validate
        _tmpInput.text = correctChar.ToString();
    }
    
    public bool IsCorrectChar()
    {
        return correctChar == _tmpInput.text.ToCharArray()[0];
    }
    
    

    #endregion
    

    

    #region Button Functions

    private bool _hasBeenSelected = false;
    
    public void Selected()
    {
        
        _hasBeenSelected = true;
        
        //print("Selected");
        //print(correctChar + " was selected");
        _tileSorting.selected = _tmpInput; // TODO: Isn't initialized
        
        //var thisBox = GetComponentInChildren<TMP_InputField>();
        
        // Set selected in tile sorting to this box
        //_tileSorting.selected = _tmpInput;

        //_tmpInput.interactable = false;
        
        //_button.gameObject.SetActive(true);
    }

    public void Deselected()
    {
        //print("Deselected!");
        //var thisBox = GetComponentInChildren<TMP_InputField>();
        
        if (_tileSorting.selected == _tmpInput )
        {
            //_tileSorting.selected = null;
            print("Re-selected");
            _tmpInput.Select();
        }
        
        //_tmpInput.interactable = true;
        
        //_button.gameObject.SetActive(false);
    }

    private string _prevText = "";
    public void OnValueChanged() // changed the string attached to tmpInput
    {
        if (!_hasBeenSelected)
            return;
        
        // If there is no character(deleted current character), don't do anything
        
        if (_tmpInput.text == "")
            _tileSorting.SelectNextBox(false);
        
        
        // Skip to the next letterbox in the sequence/word
        _tileSorting.SelectNextBox(true);
        
        
        // If no more boxes, just stay on the same one or select the next word
        
        //nextBox.Select();
        
        _prevText = _tmpInput.text;
    }
    
    public void ButtonFunction() // Change between horizontal and vertical
    {
        // If button action happens twice
        if (_tileSorting.prevSelected == _tmpInput)
        {
            _tileSorting.ChangeOrientation();
        }
        
        _tmpInput.Select();
        //_tileSorting.selected = _tmpInput;
        //TouchScreenKeyboard.Open("");
    }

    #endregion



    public void ChangeText(string newText)
    {
        _tmpInput.text = newText;
    }
    
    
    
    void Start()
    {
        _tmpInput = GetComponentInChildren<TMP_InputField>();
        _tileSorting = GetComponentInParent<TileSorting>();
        
        _button = GetComponentInChildren<Button>();
        //_button.gameObject.SetActive(false);


        // Clear all letters and reset colours

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1);


    }
    
    
    
    void Update()
    {
        
    }
    
    

    
    
      
}
