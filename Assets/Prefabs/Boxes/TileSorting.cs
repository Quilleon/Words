using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TileSorting : MonoBehaviour // TileMaster???
{
    //private GameObject[] _tempArray;
    [SerializeField] private string[] horizontalWordHints;
    [SerializeField] private string[] verticalWordHints;
    private string activeWordHint;
    private TMP_Text _wordHintDisplay;
    
    private void OnEnable() // Happens too damn fast
    {
        print("Enabled: " + name);
        
        if (selected)
            selected.Select();
        
        // Select a box to start with
        //selected = transform.GetChild(0).GetComponentInChildren<TMP_InputField>();
        //selected.Select();
        //print(selected.GetComponent<LetterBoxController>().correctChar + " was selected");
    }
    
    void Start()
    {
        //print("Start");
        _wordHintDisplay = GameObject.Find("Word Hint").GetComponent<TMP_Text>();
        
        
        assignCharacters = false;
        //clearCharacters = true;
        doSorting = true;
        
        Sorting();
        
        
        // Select a box to start with
        selected = transform.GetChild(0).GetComponentInChildren<TMP_InputField>();
        selected.Select();
        print(selected.text + " was selected");
        //print(selected.GetComponentInParent<LetterBoxController>().correctChar + " was selected");
    }
    

    public TMP_InputField selected; //{private get; set;}
    private int selectedXValue, selectedYValue; // Value starting from 0
    
    public bool horizontal { get; private set; } = true; // Changed externally

    #region Crossword Functions

    public void ChangeOrientation()
    {
        print("Change orientation");
        horizontal = !horizontal;
    }

    public void SelectNextBox(bool forward)
    {
        //TODO: skip to next word when at the end of current word
        
        // Go to next x/y- value
        var nextX = selectedXValue + (forward ? 1 : -1);
        var nextY = selectedYValue + (forward ? -1 : 1);

        nextX = Mathf.Clamp(nextX, 0, gridX -1);
        nextY = Mathf.Clamp(nextY, 0, gridY -1);
        
        // Select next box
        var currentBox = _crossWord[selectedXValue, selectedYValue];
        var nextBox = (horizontal ? _crossWord[nextX, selectedYValue] : _crossWord[selectedXValue, nextY]);
        
        if (nextBox) // if there is a box there
            nextBox.Select();
        else // if not
            currentBox.Select();
        
        
        //(horizontal ? _crossWord[nextX, selectedYValue] : _crossWord[selectedXValue, nextY]).GetComponentInParent<LetterBoxController>().ButtonFunction();
    }

    #endregion
    
    
    

    public TMP_InputField prevSelected;
    private bool _prevHorizontalValue;
    
    [SerializeField] private Color normal, extra;

    private int _numWordInRowColumn;

    private bool activated;
    
    // Update is called once per frame
    void Update()
    {
        ActivateWord();   

        _prevHorizontalValue = horizontal;
        prevSelected = selected;
    }


    private List<TMP_InputField> letterBoxesInCurrentWord;
    
    private bool _hasResetThisFrame;
    
    void ActivateWord()
    {
        if (selected != prevSelected || horizontal != _prevHorizontalValue)
        {
            //print("Colour function");
            
            // Reset all colours
            foreach (var letterBox in _crossWord)
            {
                if (letterBox != null)
                {
                    var colours = letterBox.colors;
                    colours.normalColor = normal;
                    letterBox.colors = colours;
                }
            }
            
            // Re-select box
            if (selected == null) //&& _prevSelected != null
            {
                print("Re-selected");
                selected = prevSelected;
                selected.Select();
                
                //_prevSelected.Select();
                //_prevSelected = selected;
                //return;
            }
            
            //print("Change word colour");
            
            selectedXValue = ParentPositionValue('x', selected.transform) - lowX;
            selectedYValue = ParentPositionValue('y', selected.transform) - lowY;
            
            //print(selected.transform.parent.position);
            //print("(" + selectedXValue + "," + selectedYValue + ")");
            

            //activeWordHint = horizontal ? horizontalWordHints[highY - selectedYValue +1] : verticalWordHints[selectedXValue];
            //print(activeWordHint);
            //_wordHintDisplay.text = activeWordHint;
            
// TODO: Remove this
            //int numberOfLettersInWord = 0;
            
            letterBoxesInCurrentWord = new List<TMP_InputField>();
            
            
            HighlightWord(horizontal);
            
            
            /*
            if (horizontal)
            {
                //List<TMP_InputField> letterBoxesInWord = new List<TMP_InputField>();

                
            }
            else // if vertical
            {
                for (int i = 0; i < gridY; i++)
                {
                    //print("Changed vertical colours");
                    var letterBox = _crossWord[selectedXValue, i];
                    if (letterBox != null)
                    {
                        var colours = letterBox.colors;
                        colours.normalColor = extra;
                        letterBox.colors = colours;

                        //numberOfLettersInWord++;
                    }
                }
            }*/
            
            if (letterBoxesInCurrentWord.Count <= 1 && !_hasResetThisFrame)
            {
                print("RESET!!!");
                _hasResetThisFrame = true;
                
                _prevHorizontalValue = horizontal;
                //ChangeOrientation();
                
                prevSelected = selected;
                
                ActivateWord();
                return;
            }
            if (letterBoxesInCurrentWord.Count <= 1 && _hasResetThisFrame) Debug.LogError("Lonely letterbox in both directions!!!");
        }
        
        // Reveal word hint
        //print(gridY + " - " + selectedYValue + " -1");

        // hint based on how many words in row + number of word in the row
        int hint = 0;
        for (int i = 0; i < gridY-1 - selectedYValue; i++)
            hint += _horizontalWords[i];
        hint += _numWordInRowColumn;
        
        
        activeWordHint = horizontal ? horizontalWordHints[hint] : verticalWordHints[selectedXValue];
        //activeWordHint = horizontal ? horizontalWordHints[hint-1] : verticalWordHints[selectedXValue];
        
        //print(activeWordHint);
        _wordHintDisplay.text = activeWordHint;
        
        
        _hasResetThisFrame = false;
        
        _prevHorizontalValue = horizontal;
        prevSelected = selected;
    }

    void HighlightWord(bool tempHorizontal)
    {
        // Reset when looking at column
        _numWordInRowColumn = 0;
                
        for (int i = 0; i < (tempHorizontal ? gridX : gridY); i++)
        {
            var letterBox = tempHorizontal ? _crossWord[i, selectedYValue] : _crossWord[selectedXValue, i];
            if (letterBox != null)
            {
                letterBoxesInCurrentWord.Add(letterBox);
                        
                //numberOfLettersInWord++;
            }
            else if (letterBox == null && letterBoxesInCurrentWord.Count > 0) // When you meet an empty box after starting a word 
            {
                if ((tempHorizontal ? selectedXValue : selectedYValue)> i) // Selected box has not been reached
                {
                    letterBoxesInCurrentWord.Clear();
                    _numWordInRowColumn++; // Word hint +1
                }
                else // Selected box is in the word
                {
                    _numWordInRowColumn++;
                    break;
                }
            }
        }

        // Change all box colours in the word
        foreach (var box in letterBoxesInCurrentWord)
        {
            var colours = box.colors;
            colours.normalColor = extra;
            box.colors = colours;
                    
            //print("Changed horizontal colours");
        }
    }
    
    
    
    //[SerializeField] private float distanceBetweenTiles = 1f;
    
    private TMP_InputField[,] _crossWord; // Matrix of the letterBoxes
    private int gridX, gridY; // Number of rows and columns in a matrix (NB! starts at 1)
    private int highX, lowX, highY, lowY; // x and y extreme positions
    
    // Bools for tasks
    [SerializeField] private bool clearCharacters, displayCorrectChar;
    private bool assignCharacters, doSorting;
    [SerializeField] private bool validate;

    private int[] _horizontalWords;
    private int _wordCount;

    int ParentPositionValue(char xy, Transform obj)
    {
        var pos = obj.parent.localPosition;
        
        switch (xy)
        {
            case 'x':
                return Mathf.CeilToInt(pos.x); //(int)Math.Ceiling
            case 'y':
                return Mathf.CeilToInt(pos.y); 
            default:
                Debug.LogError("XY value typo!");
                return 0;
        }
    }
    
    private void OnValidate()
    {
        if (!validate)
            return;
        
        validate = false;
        
        Sorting();
    }

    private void Sorting()
    {
        // Get all children
        List<LetterBoxController> controllerList = new List<LetterBoxController>();
        controllerList.AddRange(transform.GetComponentsInChildren<LetterBoxController>());

        foreach (var controller in controllerList)
        {
            if (assignCharacters) // Assign Current Char as Correct Char
                controller.AssignCorrectChar();
            if (clearCharacters)
                controller.ClearCharacter();
            else if (displayCorrectChar)
                controller.DisplayCorrectChar();
        }
        


        #region Sort Into Matrix

        if (!doSorting)
            return;
        
        // Get all children
        List<TMP_InputField> childList = new List<TMP_InputField>();
        childList.AddRange(transform.GetComponentsInChildren<TMP_InputField>());
        //print("Number of boxes under crossword: " + childList.Count);
        
        
        // Find number of rows and columns 
        // Highest - Lowest (x/y-value)
        lowX = ParentPositionValue('x', childList[0].transform); //int lowX = (int)childList[0].transform.parent.position.x;
        highX = lowX;
        
        lowY = ParentPositionValue('y', childList[0].transform);
        highY = lowY;
        
        foreach (var child in childList)
        {
            int thisX = ParentPositionValue('x', child.transform);
            
            if (lowX > thisX) lowX = thisX;
            if (highX < thisX) highX = thisX;
            
            int thisY = ParentPositionValue('y', child.transform);
            
            if (lowY > thisY) lowY = thisY;
            if (highY < thisY) highY = thisY;
        }

        // Rounding the negative value down instead of up
        //if (lowX <= 0) lowX--; //+ highX <
        //if (lowY <= 0) lowY--;
        
        //print("x = " + lowX + ", " + highX);
        //print("y = " + lowY + ", " + highY);

        // Get difference +1 for grid size
        gridX = highX - lowX + 1;
        gridY = highY - lowY + 1;
        
        _crossWord = new TMP_InputField[gridX,gridY]; //TMP_InputField[,] letterGrid = new TMP_InputField[gridX,gridY]; // 2D array
        //print("Grid = " + gridX + ", " + gridY); //Console.WriteLine("Grid is {1}, {2}", gridX, gridY);

        // Sort all squares into a 2D array (matrix)
        foreach (var child in childList)
        {
            var x = ParentPositionValue('x', child.transform) - lowX;
            var y = ParentPositionValue('y', child.transform) - lowY;

            _crossWord[x, y] = child;
        }

        /*
        var temp = _crossWord[0, 0].colors;
        //temp.normalColor = Color.blue;
        _crossWord[0, 4].colors = temp;
        */
        
        #endregion


        // Counting words in crossword
        _horizontalWords = new int[gridX];
        
        List<TMP_InputField> charList = new List<TMP_InputField>();
        Vector2 prevPos = new Vector2();
        int randomInt = 0;

        for (int y = gridY-1; y >= 0; y--)
        {
            for (int x = 0; x <= gridX-1; x++)
            {
                var box = _crossWord[x, y];

                
                if (x==gridX-1 && y==0) // Also check the last iteration
                {
                    if (box)
                        charList.Add(box);
                    
                    if (charList.Count > 1)
                    {
                        _wordCount++;
                        //print("There is a word on the last row. (" + x + ", " + y + ")" );
                    }
                }
                
                
                if (!box)
                    continue;
                
                //print(x + ", " + y); // Registered boxes
                
                if (randomInt == 0) // If first box
                { }
                else if (prevPos.x +1 != x || prevPos.y != y) // If there is a gap between letters, there is a new line, or it is the last iteration
                {
                    if (charList.Count > 1)
                    {
                        _wordCount++;
                        //print(x + ", " + y + " is the start of another word");

                        //print(prevPos.y);
                        _horizontalWords[(int)prevPos.y]++;
                    }
                    //print(x + ", " + y + " is the start of something new");
                    
                    charList.Clear();
                }

                charList.Add(box);
                prevPos = new Vector2(x, y);
                randomInt++;
            }
        }
        /*
        foreach (var box in _crossWord)
        {
            if (!box)
                continue;
            
            var boxPos = new Vector2(PositionValue('x', box.transform), PositionValue('y', box.transform));
            
            if (randomInt == 0)
            {
                
            }
            else if (!(prevPos.x +1 == boxPos.x)) //|| prevPos.y == boxPos.y
            {
                charList.Clear();
                charList.Add(box);
                _wordCount++;
            }
            
            prevPos = boxPos;
            randomInt++;
        }*/
        
        //print("Words in crossword: " + _wordCount);

        //print(_horizontalWords[3]);
        //foreach (var num in _horizontalWords) print(num);
    }
}
