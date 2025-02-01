using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniController : MonoBehaviour
{
    public TileSorting ActiveMiniTileSorting; // Used on keyboard button to check which mini.selected is written in

    //private TileSorting[] allMinis;

    [SerializeField] private Color32 normal, highlighted;
    
    void Start()
    {
        var allActiveMinis = GetComponentsInChildren<TileSorting>();
        foreach (var mini in allActiveMinis)
        {
            if (mini.gameObject.activeSelf)
                ActiveMiniTileSorting = mini;
        }
    }

    public void ClearMinis() // Is called on buttons when selecting mini
    {
        //print("Disable all minis");
        
        var allActiveMinis = GetComponentsInChildren<TileSorting>();

        foreach (var mini in allActiveMinis)
        {
            //print(mini.name);
            mini.gameObject.SetActive(false);
        }
            
    }

    public void ActivateMini(int childNum) // Is used on mini selection buttons
    {
        var activatedMini = transform.GetChild(childNum).gameObject;

        activatedMini.SetActive(true);
        ActiveMiniTileSorting = activatedMini.GetComponent<TileSorting>();

        ActiveMiniTileSorting.normal = normal;
        ActiveMiniTileSorting.extra = highlighted;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
