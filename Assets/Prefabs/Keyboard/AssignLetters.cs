using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AssignLetters : MonoBehaviour
{
    private string lettersInOrder = "qwertyuiopasdfghjklzxcvbnm";
    
    //[SerializeField] 
    private bool validate;
    private void OnValidate()
    {
        if (!validate)
            return;

        validate = false;

        var charArray = lettersInOrder.ToCharArray();
        int buttonNum = 0;

        for (int i = 0; i < transform.childCount; i++) // each row
        {
            var child = transform.GetChild(i);
            
            for (int j = 0; j < transform.GetChild(i).childCount; j++) // each button
            {
                var button = child.GetChild(j);
                button.GetComponentInChildren<TMP_Text>().text = charArray[buttonNum].ToString();
                buttonNum++;
            }
        }
        
        
    }
}
