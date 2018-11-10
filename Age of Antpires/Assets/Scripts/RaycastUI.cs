using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RaycastUI : NetworkBehaviour {
    bool hovering = false;
    public UISave uiSave;

	void Update () {
        if (!isLocalPlayer)
            return;
        CdRaycast();
    }

    struct InputTypes
    {
        public int MouseButtonClicked;
        public bool shiftclicked;
        public bool buildDirt;
        public bool buildWall;

        public InputTypes(int m, bool s = false, bool d = false, bool w = false)
        {
            MouseButtonClicked = m;
            shiftclicked = s;
            buildDirt = d;
            buildWall = w;
        }
    }
    
    void CdRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (GameObject.Find("Canvas") != null)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "Ant" || hit.transform.gameObject.tag == "Queen" ||
                        hit.transform.gameObject.tag == "Upgrade" || hit.transform.gameObject.tag == "Egg" ||
                        hit.transform.gameObject.tag == "Storage")
                        if (hit.transform.gameObject.GetComponent<Ant>() == null || ((uiSave.player == 0) == (hit.transform.gameObject.GetComponent<Ant>().ID > 0)))
                            GameObject.Find("Canvas").GetComponentInChildren<InGame_UI_Manager>().mainselected = hit.transform.gameObject;
                    else if (hit.transform.gameObject.tag != "UI") GameObject.Find("Canvas").GetComponentInChildren<InGame_UI_Manager>().mainselected = null;
                }
            }

            CmdAquireGameObject(ray.origin, ray.direction, new InputTypes(0));
        }
        if (Input.GetMouseButtonDown(1))
        {
            CmdAquireGameObject(ray.origin, ray.direction, new InputTypes(1, (Input.GetKey(KeyCode.LeftShift)), (Input.GetKey(KeyCode.E)), (Input.GetKey(KeyCode.R))));
        }
    }

    [Command]
    void CmdAquireGameObject(Vector3 origin, Vector3 direction, InputTypes input)
    {
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (input.MouseButtonClicked == 1)
            {
                if (hit.transform.gameObject.tag == "Air" || hit.transform.gameObject.tag == "Dirt" 
                    || hit.transform.gameObject.tag == "Stone" || hit.transform.gameObject.tag == "Food" ||
                    hit.transform.gameObject.tag == "Ant" || hit.transform.gameObject.tag == "Queen")
                {
                    if (uiSave.selectedObject1st != null)
                    {
                        uiSave.shiftClicked = input.shiftclicked;
                        uiSave.dirtBuildClicked = input.buildDirt;
                        uiSave.wallBuildClicked = input.buildWall;

                        uiSave.selectedObject2nd = hit.transform.gameObject;
                        //print(hit.transform.gameObject.tag);
                        //print("Air");
                    }
                }
            }
            if (input.MouseButtonClicked == 0)
            {
                //GameData.Instance.selectedObjectUI = hit.transform.gameObject;
                if (hit.transform.gameObject.tag == "Ant" || hit.transform.gameObject.tag == "Queen")
                {
                    uiSave.selectedObject1st = hit.transform.gameObject;
                    uiSave.selectedObject2nd = null;
                    //print("Ant");
                }
                else uiSave.selectedObject1st = null;
            }
        }
    }
}
