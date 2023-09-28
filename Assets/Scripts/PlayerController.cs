using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float shootingFrequency = 1.0f;
    [SerializeField]
    private GameObject beam = null;

    private bool isInitialized = false;
    private Vector3 screenBounds = Vector3.zero;
    private float objectWidth = 0.0f;

    private void Awake()
    {
        //we calculate the screen bounds so we can clamp the player movement to be inside the screen area
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        GameManager.OnInitialized += Initialize;
        GameManager.OnGameOver += OnGameOver;
    }

    private void Initialize()
    {
        isInitialized = true;
        InvokeRepeating("Shoot", 0, shootingFrequency);//Sart repeated and constat shooting
    }

    void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        //we chech for mouse or touch inputs
        if (Input.mousePresent)
        {
            MouseMouvement();
        }

        else if (Input.touchSupported)
        {
            TouchMouvement();
        }
        
    }

    private void TouchMouvement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);//first finguer/touch on screen
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);//we calculate the pos relative to the screen
            float limitX = Mathf.Clamp(touchPosition.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);//Clamp the x position to be inside the screen bounds regarding the player and screen width
            Vector2 fixedPosition = new Vector2(limitX, transform.position.y);// new vector with x clamped position
            transform.position = Vector3.Lerp(transform.position, fixedPosition, speed * Time.deltaTime);
        }
    }

    private void MouseMouvement()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//we calculate the pos relative to the screen
        float limitX = Mathf.Clamp(mousePosition.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);//Clamp the x position to be inside the screen bounds regarding the player and screen width
        Vector2 fixedPosition = new Vector2(limitX, transform.position.y);// new vector with x clamped position
        transform.position = Vector3.Lerp(transform.position, fixedPosition, speed * Time.deltaTime);

    }

    private void Shoot()
    {
        if (beam!= null)
        {
            Instantiate(beam, transform.position, Quaternion.identity);
        }
    }

    private void OnGameOver()
    {
        isInitialized = false;
        CancelInvoke("Shoot");
    }

    private void OnDestroy()
    {
        GameManager.OnInitialized -= Initialize;
        GameManager.OnGameOver -= OnGameOver;
    }
}
