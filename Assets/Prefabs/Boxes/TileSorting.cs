using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class WordHints
{
    public string[] hints;
}

public class TileSorting : MonoBehaviour // TileMaster???
{
    //private GameObject[] _tempArray;
    //[SerializeField] private string[] horizontalWordHints;
    [SerializeField] [Range(0, 1)] private static int wordHintList; // Chooses which hint list to take hints from
    [SerializeField] public WordHints[] horizontalWordHintArray;
    //[SerializeField] private string[] verticalWordHints;
    [SerializeField] private WordHints[] verticalWordHintArray;
    
    private string activeWordHint;
    private TMP_Text _wordHintDisplay;

    private bool _shouldSelectSelected;
    private void OnEnable() // Happens too damn fast
    {
        //print("Enabled: " + name);

        if (selected)
        {
            _shouldSelectSelected = true;
            //selected.Select();
            //print("Selected " + selected.text + " in OnEnable()");
        }
        
        // Select a box to start with
        //selected = transform.GetChild(0).GetComponentInChildren<TMP_InputField>();
        //selected.Select();
        //print(selected.GetComponent<LetterBoxController>().correctChar + " was selected");
    }
    
    void Start()
    {
        //print("Start function in " + name);
        _wordHintDisplay = GameObject.Find("Word Hint").GetComponent<TMP_Text>();
        
        
        assignCharacters = false;
        //clearCharacters = true;
        doSorting = true;
        
        Sorting();
        
        
        // Select a box to start with
        selected = transform.GetChild(0).GetComponentInChildren<TMP_InputField>();
        selected.Select();
        //print(selected.text + " was selected");
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
        var nextBox = horizontal ? _crossWord[nextX, selectedYValue] : _crossWord[selectedXValue, nextY];
        
        //print(nextX + ", " + selectedYValue);

        if (nextBox) // if there is a box there
        {
            selected = nextBox.tmpInput;
            selected.Select();
            
        }
        else // if not
        {
            //selected = currentBox;
            currentBox.tmpInput.Select();
        }
        
        
        
        
        //(horizontal ? _crossWord[nextX, selectedYValue] : _crossWord[selectedXValue, nextY]).GetComponentInParent<LetterBoxController>().ButtonFunction();
    }

    #endregion
    
    
    

    public TMP_InputField prevSelected;
    private bool _prevHorizontalValue;
    
    [HideInInspector] public Color normal, extra;

    private int _numWordInRowColumn;

    private bool activated;
    
    // Update is called once per frame
    void Update()
    {
        if (_shouldSelectSelected)
        {
            _shouldSelectSelected = false;
            selected.Select(); 
        }
        
        if (prevSelected && selected != prevSelected)
        {
            prevSelected.GetComponentInParent<LetterBoxController>().ChangeAppearance(0);
        }
        
        // Should be called through checks instead of update()
        ActivateWord();

        

        _prevHorizontalValue = horizontal;
        prevSelected = selected;
    }

    
    private List<LetterBoxController> letterBoxesInCurrentWord;
    
    private bool _hasResetThisFrame;
    
    //TODO: This should not happen every frame!
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
                    letterBox.ChangeAppearance(0);
                    
                    /*
                    var colours = letterBox.tmpInput.colors;
                    colours.normalColor = normal;
                    letterBox.tmpInput.colors = colours;
                    */
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
            
            selectedXValue = ParentPositionValue('x', selected.transform.parent) - lowX;
            selectedYValue = ParentPositionValue('y', selected.transform.parent) - lowY;
            
            //print(selected.transform.parent.position);
            //print("(" + selectedXValue + "," + selectedYValue + ")");
            

            //activeWordHint = horizontal ? horizontalWordHints[highY - selectedYValue +1] : verticalWordHints[selectedXValue];
            //print(activeWordHint);
            //_wordHintDisplay.text = activeWordHint;
            
            //int numberOfLettersInWord = 0;
            
            letterBoxesInCurrentWord = new List<LetterBoxController>();
            
            
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
                ChangeOrientation();
                
                prevSelected = selected;
                
                ActivateWord();
                return;
            }
            if (letterBoxesInCurrentWord.Count <= 1 && _hasResetThisFrame) Debug.LogError("Lonely letterbox in both directions!!!");
        }
        
        // Reveal word hint
        //print(gridY + " - " + selectedYValue + " -1");

        
        
        int yValueReversed = gridY - 1 - selectedYValue;

        if (horizontal && _horizontalWords[yValueReversed] != 0)
        {
            activeWordHint = horizontalWordHintArray[wordHintList].hints[WhichHint()];
            
            // If hint is empty, resort to default
            if (activeWordHint == "")
                activeWordHint = horizontalWordHintArray[0].hints[WhichHint()];
            
        }
        else if (!horizontal && _verticalWords[selectedXValue] != 0)
        {
            activeWordHint = verticalWordHintArray[wordHintList].hints[WhichHint()];
            
            // If hint is empty, resort to default
            if (activeWordHint == "")
                activeWordHint = verticalWordHintArray[0].hints[WhichHint()];
        }
        else
        {
            activeWordHint = "";
        }
        
        
        //activeWordHint = horizontal ? horizontalWordHints[hint-1] : verticalWordHints[selectedXValue];
        
        //print(activeWordHint);
        _wordHintDisplay.text = activeWordHint;
        
        
        _hasResetThisFrame = false;
        
        _prevHorizontalValue = horizontal;
        prevSelected = selected;
    }

    int WhichHint() // Hint based on how many words in row + the number of the word in the row
    {
        int hint = -1;
        
        int yValueReversed = gridY - 1 - selectedYValue;

        
        for (int i = 0; i < (horizontal ? yValueReversed : selectedXValue); i++)
        {
            //print("Added " + _horizontalWords[i]);
            hint += horizontal ? _horizontalWords[i] : _verticalWords[i]; // Add in all previous rows before your row
        }
        hint += _numWordInRowColumn; // Add which word in row this is
        
        return hint;
    }

    void HighlightWord(bool tempHorizontal)
    {
        // Reset when looking at column
        _numWordInRowColumn = 0;

        var iExtremeValue = (tempHorizontal ? gridX : gridY) -1;

        //TODO: Make a function out of this
        if (tempHorizontal)
        {
            for (int i = 0; i <= iExtremeValue; i++)
            {
                var letterBox_i = tempHorizontal ? _crossWord[i, selectedYValue] : _crossWord[selectedXValue, i];
                
                if (letterBox_i != null)
                {
                    letterBoxesInCurrentWord.Add(letterBox_i);
                    
                    //numberOfLettersInWord++;
                }
                
                if ((letterBox_i == null && letterBoxesInCurrentWord.Count > 0) || i == iExtremeValue) // When you meet an empty box after starting a word or it is the last box in row/column
                {
                    //print("Empty space or end reached");
                    
                    if ((tempHorizontal ? selectedXValue : selectedYValue) > i) // Selected box has not been reached
                    {
                        //print("Clear boxes!");
                        letterBoxesInCurrentWord.Clear();
                        _numWordInRowColumn++; // Word hint +1
                    }
                    else // Selected box is in the word
                    {
                        //print("Correct Word");
                        _numWordInRowColumn++;
                        break;
                    }
                }
            }
        }
        else // vertical
        {
            for (int i = iExtremeValue; i >= 0; i--)
            {
                var letterBox_i = tempHorizontal ? _crossWord[i, selectedYValue] : _crossWord[selectedXValue, i];
                
                if (letterBox_i != null)
                {
                    letterBoxesInCurrentWord.Add(letterBox_i);
                    
                    //numberOfLettersInWord++;
                }
                
                if ((letterBox_i == null && letterBoxesInCurrentWord.Count > 0) || i == 0) // When you meet an empty box after starting a word or it is the last box in row/column
                {
                    //print("Empty space or end reached");
                    
                    if ((tempHorizontal ? selectedXValue > i : selectedYValue < i) ) // Selected box has not been reached
                    {
                        //print("Clear boxes!");
                        letterBoxesInCurrentWord.Clear();
                        _numWordInRowColumn++; // Word hint +1
                    }
                    else // Selected box is in the word
                    {
                        //print("Correct Word");
                        _numWordInRowColumn++;
                        break;
                    }
                }
            }
        }
        
        //print("Num of word in row is " + _numWordInRowColumn);

        
        // Change all box colours in the word
        foreach (var box in letterBoxesInCurrentWord)
        {
            box.ChangeAppearance(1);
            
            if (box.tmpInput == selected)
                box.ChangeAppearance(2);
            
            /*
            var colours = box.tmpInput.colors;
            colours.normalColor = extra;
            box.tmpInput.colors = colours;
            */
                    
            //print("Changed horizontal colours");
        }
    }
    
    
    
    //[SerializeField] private float distanceBetweenTiles = 1f;
    
    private LetterBoxController[,] _crossWord; // Matrix of the letterBoxes
    private int gridX, gridY; // Number of rows and columns in a matrix (NB! starts at 1)
    private int highX, lowX, highY, lowY; // x and y extreme positions
    
    // Bools for tasks
    [SerializeField] private bool clearCharacters, displayCorrectChar;
    private bool assignCharacters, doSorting;
    [SerializeField] private bool validate;

    private int[] _horizontalWords, _verticalWords;
    private int _wordCount;

    int ParentPositionValue(char xy, Transform obj)
    {
        //var pos = obj.parent.localPosition;
        var pos = obj.localPosition;
        
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
        List<LetterBoxController> childList = new List<LetterBoxController>();
        childList.AddRange(transform.GetComponentsInChildren<LetterBoxController>());
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
        
        _crossWord = new LetterBoxController[gridX,gridY]; //TMP_InputField[,] letterGrid = new TMP_InputField[gridX,gridY]; // 2D array
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
        _horizontalWords = new int[gridY];
        _verticalWords = new int[gridX];
        
        List<LetterBoxController> charList = new List<LetterBoxController>();
        Vector2 prevPos = new Vector2();
        int randomInt = 0;

        // Horizontal Words
        for (int y = gridY-1; y >= 0; y--) // Iterate y-axis
        {
            for (int x = 0; x <= gridX-1; x++) // Iterate x-axis
            {
                var box = _crossWord[x, y];
                
                //print("Checked: (" + x + ", " + y + ")" );
                
                if (x==gridX-1 && y==0) // Also check the last iteration of the for loops
                {
                    if (box)
                        charList.Add(box);
                    
                    if (charList.Count > 1)
                    {
                        _wordCount++;
                        //print("There is a word on the last row. (" + x + ", " + y + ")" );
                        _horizontalWords[y]++;
                    }
                    
                    continue;
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
                        _horizontalWords[(int)prevPos.y]++; // Add word count to the row of the last checked box
                    }
                    //print(x + ", " + y + " is the start of something new");
                    
                    charList.Clear();
                }

                charList.Add(box);
                prevPos = new Vector2(x, y);
                randomInt++;
            }
        }
        
        charList.Clear();
        //charList = new List<TMP_InputField>();
        prevPos = new Vector2();
        randomInt = 0;
        
        // Vertical Words
        for (int x = 0; x <= gridX-1; x++) // Iterate x-axis
        {
            for (int y = gridY-1; y >= 0; y--) // Iterate y-axis
            {
                var box = _crossWord[x, y];

                
                if (x==gridX-1 && y==0) // Also check the last iteration of the for-loops
                {
                    if (box)
                        charList.Add(box);
                    
                    if (charList.Count > 1)
                    {
                        //_wordCount++;
                        //print("There is a word on the last column. (" + x + ", " + y + ")" );
                        _verticalWords[x]++;
                    }
                    continue;
                }
                
                
                if (!box) continue; // If there is no box, skip this iteration
                
                //print(x + ", " + y); // Registered boxes
                
                if (randomInt == 0) // If first box
                { }
                else if (prevPos.y -1 != y || prevPos.x != x) // If there is a gap between letters, there is a new line, or it is the last iteration
                {
                    if (charList.Count > 1)
                    {
                        //_wordCount++;
                        //print(x + ", " + y + " is the start of another word");
                        
                        //print(prevPos.y);
                        _verticalWords[(int)prevPos.x]++; // Add word count to the row of the last checked box
                    }
                    //print(x + ", " + y + " is the start of something new");
                    
                    charList.Clear();
                }

                charList.Add(box);
                prevPos = new Vector2(x, y);
                randomInt++;
            }
        }

        
        #region Prints

        // Print number of horizontal words
        //var numHorizontalWords = 0; foreach (var row in _horizontalWords) numHorizontalWords += row; print("Horizontal words in crossword: " + numHorizontalWords);
        
        //foreach (var num in _horizontalWords) print(num);
        
        // Print number of vertical words
        //var numVerticalWords = 0; foreach (var column in _verticalWords) numVerticalWords += column; print("Vertical words in crossword: " + numVerticalWords);
        
        //foreach (var num in _verticalWords) print(num);
        

        //print(_horizontalWords[3]);
        //foreach (var num in _horizontalWords) print(num);

        #endregion
    }

    // TODO: This should be done on every box when it is changed
    public bool FilledMini(bool wantCorrectChar)
    {
        bool isMiniFilled = true;
        
        foreach (var box in _crossWord)
        {
            if (isMiniFilled && box) // If there is an item there
            {
                isMiniFilled = wantCorrectChar ? box.IsCorrectChar() : box.HasChar();
            }
        }

        return isMiniFilled;
    }
    
    public void ChangeHint()
    {
        //if (hintList is 0 or 1)
            wordHintList = wordHintList == 0 ? 1 : 0;
        //else
          //  Debug.LogError("Wrong hint input");
    }
}
