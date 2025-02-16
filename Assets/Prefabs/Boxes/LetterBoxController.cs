using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LetterBoxController : MonoBehaviour
{
    public TMP_InputField tmpInput;
    private TMP_Text _tmpText;
    private Image _image;

    [SerializeField] private Color32 defaultTextColour, highlightedTextColour;
    [SerializeField] private Color32 defaultBoxColour, selectedBoxColour;
    [SerializeField] private Sprite defaultSprite, highlightedSprite;
    
    
    private TileSorting _tileSorting;
    private Button _button;

    //[SerializeField] private TMP_InputField northBox, southBox, eastBox, westBox;
    //[SerializeField] private TMP_InputField nextBox;


    public char correctChar; //{ get; private set; }

    
    #region Functions for Tile Sorting script

    public void AssignCorrectChar()
    {
        tmpInput = GetComponentInChildren<TMP_InputField>(); // Needed for validate

        correctChar = tmpInput.text.ToCharArray()[0];
    }
    
    public void ClearCharacter()
    {
        tmpInput = GetComponentInChildren<TMP_InputField>(); // Needed for validate
        ChangeInputText(null);
    }

    public void DisplayCorrectChar()
    {
        tmpInput = GetComponentInChildren<TMP_InputField>(); // Needed for validate
        ChangeInputText(correctChar.ToString());
    }
    
    public bool HasChar()
    {
        return !CompareInputText("");
    }
    
    public bool IsCorrectChar()
    {
        //return correctChar == _tmpInput.text.ToCharArray()[0];
        return CompareInputText(correctChar.ToString()); 
    }

    #endregion
    

    

    #region Button Functions

    private bool _hasBeenSelected = false; // Only check for change in input after being selected

    public void ChangeAppearance(int mode)
    {
        switch (mode)
        {
            case 0:
                _tmpText.color = defaultTextColour;
                _image.sprite = defaultSprite;
                _image.color = defaultBoxColour;
                break;
            case 1:
                _tmpText.color = highlightedTextColour;
                _image.sprite = highlightedSprite;
                _image.color = defaultBoxColour;
                break;
            case 2:
                ChangeAppearance(1);
                _image.color = selectedBoxColour;
                break;
            default: Debug.LogError("Wrong input for ChangeAppearance()!");
                break;
        }
        
    }
    
    public void Selected()
    {
        //ChangeAppearance(1);
        
        _hasBeenSelected = true;
        
        //print("Selected");
        //print(_tmpInput.text + " was selected");
        _tileSorting.selected = tmpInput; // Set tileSorting selected
        
        //var thisBox = GetComponentInChildren<TMP_InputField>();
        
        // Set selected in tile sorting to this box
        //_tileSorting.selected = _tmpInput;

        //_tmpInput.interactable = false;
        
        //_button.gameObject.SetActive(true);

        
    }

    public void Deselected()
    {
        
        //ChangeAppearance(0);
        
        //print("Deselected!");
        //var thisBox = GetComponentInChildren<TMP_InputField>();
        
        if (_tileSorting.selected == tmpInput)
        {
            //_tileSorting.selected = null;
            //print("Re-selected");
            //_tmpInput.Select();
        }
        
        //_tmpInput.interactable = true;
        
        //_button.gameObject.SetActive(false);
    }

    //private string _prevText = "";
    public void OnValueChanged() // changed the string attached to tmpInput
    {
        if (!_hasBeenSelected)
            return;
        
        // If there is no character(deleted current character), don't do anything
        
        // Skip to the next letterbox in the sequence/word
        //print(_tmpInput.text);
        
        //_tileSorting.SelectNextBox(!CompareInputText(""));


        // If no more boxes, just stay on the same one or select the next word
        
        //nextBox.Select();
        
        //_prevText = _tmpInput.text;
    }
    
    public void ButtonFunction() // Change between horizontal and vertical
    {
        // If button action happens twice
        if (_tileSorting.prevSelected == tmpInput)
        {
            _tileSorting.ChangeOrientation();
        }
        
        tmpInput.Select();
        //_tileSorting.selected = _tmpInput;
        //TouchScreenKeyboard.Open("");
    }

    #endregion



    public void ChangeInputText(string newText)
    {
        tmpInput.text = newText;
    }

    public bool CompareInputText(string text)
    {
        return tmpInput.text == text;
    }
    
    
    
    void Start()
    {
        //print("Start() in " + name);
        tmpInput = GetComponentInChildren<TMP_InputField>();
        _tmpText = GetComponentInChildren<TMP_Text>();
        _image = GetComponent<Image>();
        
        _tileSorting = GetComponentInParent<TileSorting>();
        
        _button = GetComponentInChildren<Button>();
        //_button.gameObject.SetActive(false);


        // Clear all letters and reset colours

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1);


    }

    private void OnEnable()
    {
        //print("Was enabled");
    }

    void Update()
    {
        
    }
    
    

    
    
      
}
