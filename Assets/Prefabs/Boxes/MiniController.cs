using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniController : MonoBehaviour
{
    public TileSorting ActiveMiniTileSorting;
    
    
    void Start()
    {
        var allMinis = GetComponentsInChildren<TileSorting>();
        foreach (var mini in allMinis)
        {
            //var mini = miniScript.gameObject;
            
            if (mini.gameObject.activeSelf)
                ActiveMiniTileSorting = mini;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
