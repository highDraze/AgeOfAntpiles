using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    
    private string Message;
    bool hovering = false;
    public UISave uiSave;

    private void Update() {
        if (Input.GetMouseButtonDown(0) == true && hovering) {
            Debug.Log("blabl");

           // uiSave.selectedObject2nd = this.gameObject;
        }
    }

    private void OnMouseEnter()
    {
        hovering = true;
        Message = this.gameObject.tag;
        
    }
    private void OnMouseExit()
    {
        hovering = false;
        Message = "";
    }
    void OnGUI()
    {
        GUI.Label(new Rect(100, 100, 400, 300), ("Was ist das?: " + Message));
    }
}