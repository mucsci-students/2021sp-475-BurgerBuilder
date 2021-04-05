using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseController : MonoBehaviour
{
    public float plateDistance = 10f;
    public float keysSpeed;
    public float clampX;
    public float clampYMin;
    public float clampYMax;

    public GameObject mouseUI;
    public GameObject spaceUI;

    public Rigidbody plateRB;

    private Vector3 worldPosition;

    private bool wasdMode;

    void Start()
    {
        // When the game begins we want the cursor to vanish
        if(DifficultyStatic.playfabScoreboard != "NONE")
            Cursor.visible = false;

        wasdMode = DifficultyStatic.trackpadMode;
        if(DifficultyStatic.trackpadMode)
        {
            spaceUI.SetActive(true);
        }
        else
        {
            mouseUI.SetActive(true);
        }
        worldPosition = transform.position;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Main");
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

        if(!wasdMode)
        {
            Vector3 mousePos = Input.mousePosition;
            //Offset plate by an adjustable distance so it's far from the camera.
            //Keep plate within clampX and Y
            mousePos.z += plateDistance;
            //Get the world coordinates of the mouse position
            worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        }
        else if (wasdMode)
        {
            Vector3 pos = transform.position;
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
            worldPosition = pos + moveDirection * keysSpeed * Time.fixedDeltaTime;
        }

        worldPosition = new Vector3(
            Mathf.Clamp(worldPosition.x, -clampX, clampX),
            Mathf.Clamp(worldPosition.y, clampYMin, clampYMax),
            worldPosition.z
        );
        
        if(!wasdMode)
        {
            worldPosition = Vector3.Lerp(transform.position, worldPosition, Time.fixedDeltaTime * 25);
        }
        
    }

    void FixedUpdate()
    {
        //Use movePosition because otherwise physics will not be enacted on other objects
        plateRB.MovePosition(worldPosition);
    }
}
