using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    [SerializeField]
    private float camSpeed;

    [SerializeField]
    private float scrollSpeed;

    [SerializeField]
    private float smooth;

    [SerializeField]
    private float maxScrollOut;

    [SerializeField]
    private float maxScrollIn;

    private float camSize;

    public float capL;
    public float capR;
    public float capU;
    public float capD;

    
    void Start()
    {
        camSize = Camera.main.orthographicSize;
    }
    
    void Update()
    {
        TranslateCamera();
    }

    void LateUpdate()
    {
        ZoomCamera();
    }

    private void TranslateCamera()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            if (Input.mousePosition.x <= 10 && transform.position.x > capL)
            {
                //Debug.Log("Left");
                transform.Translate(Vector2.left * camSpeed * Time.deltaTime);
            }
            else if (Input.mousePosition.x >= Screen.width - 10 && transform.position.x < capR)
            {
                //Debug.Log("Right");
                transform.Translate(Vector2.right * camSpeed * Time.deltaTime);
            }

            if (Input.mousePosition.y <= 10 && transform.position.y > capD)
            {
                //Debug.Log("Down");
                transform.Translate(Vector2.down * camSpeed * Time.deltaTime);
            }
            else if (Input.mousePosition.y >= Screen.height && transform.position.y < capU)
            {
                //Debug.Log("Up");
                transform.Translate(Vector2.up * camSpeed * Time.deltaTime);
            }

            if (Input.GetAxisRaw("Vertical") == 1 && transform.position.y < capU)
            {
                transform.Translate(Vector2.up * camSpeed * Time.deltaTime);
            }
            else if (Input.GetAxisRaw("Vertical") == -1 && transform.position.y > capD)
            {
                transform.Translate(Vector2.down * camSpeed * Time.deltaTime);
            }
            if (Input.GetAxisRaw("Horizontal") == 1 && transform.position.x < capR)
            {
                transform.Translate(Vector2.right * camSpeed * Time.deltaTime);
            }
            else if (Input.GetAxisRaw("Horizontal") == -1 && transform.position.x > capL)
            {
                transform.Translate(Vector2.left * camSpeed * Time.deltaTime);
            }
        }
    }

    private void ZoomCamera()
    {
        float scr = Input.GetAxis("Mouse ScrollWheel");
        if(scr != 0.0f)
        {
            camSize -= scr * scrollSpeed;
            camSize = Mathf.Clamp(camSize, maxScrollIn, maxScrollOut);
            //Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, camSize, smooth * Time.deltaTime);
            Camera.main.orthographicSize = camSize;
        }
    }
}
