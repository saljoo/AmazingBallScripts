using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    //Pelaajan nopeus
    private float speed;

    //Pelaajan hyppynopeus
    private float jumpspeed = 14f;

    //Yksityinen muuttuja rb
    private Rigidbody rb;

    //Yksityinen muuttuja säde
    private float radius = 0.5f;

    //Yksityinen muuttuja vastustajan säde
    private float opponentRadius = 0.5f;

    //Yksityinen muuttuja pelaajan ja vastustajan välinen etäisyys
    private float dist;

    //Muuttuja jolla tarkistetaan mitä kameraa käytetään
    private bool third;

    //Kääntymisnopeus
    private float rotationSpeed = 1.0f;

    //Teksti johon merkitään pelaajan keräämät kerättävät
    public Text countText;

    //Kerätyt kerättävät
    private int count;

    public GameObject nextLevelMenu;
    public GameObject restartLevelMenu;
    public GameObject[] opponents;

    public GameObject thirdPersonCamera;
    public GameObject firstPersonCamera;


    void Start ()
    {
        Array.Clear(opponents, 0, opponents.Length);
        opponents = GameObject.FindGameObjectsWithTag("Opponent");
        rb = GetComponent<Rigidbody>();

        //Määritetään pelaajan nopeus tason mukaan
        if (SceneManager.GetActiveScene().buildIndex < 4)
        {
            speed = 10;
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 4 && SceneManager.GetActiveScene().buildIndex <= 6)
        {
            speed = 12;
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 7 && SceneManager.GetActiveScene().buildIndex <= 10)
        {
            speed = 14;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 11)
        {
            speed = 10;
        }

        if (SceneManager.GetActiveScene().buildIndex == 11)
        {
            third = false;
            thirdPersonCamera.SetActive(false);
            firstPersonCamera.SetActive(true);
        }
        else
        {
            third = true;
        }

        //Asetetaan alussa nollaksi
        count = 0;
        SetCountText();
    }

    void FixedUpdate ()
    {
        //Jos ensimmäisen persoonan peli
        if(third == false)
        {
            //Määritetään kääntymisnopeus
            float rotation = Input.GetAxis("Vertical") * rotationSpeed;
            transform.Rotate(0, rotation, 0);

            if (rb.velocity.y == 0)
            {
                if (Input.GetKey(key: KeyCode.UpArrow))
                {
                    transform.position += transform.forward * Time.deltaTime * speed;
                }
                if (Input.GetKey(key: KeyCode.DownArrow))
                {
                    transform.position += transform.forward * Time.deltaTime * -speed;
                }
            }
        }
        else
        {
            float verticalMovement = Input.GetAxis("Vertical");
            float horizontalMovement = Input.GetAxis("Horizontal");

            Vector3 movement = new Vector3(verticalMovement, 0.0f, horizontalMovement);
            if (rb.velocity.y == 0)
            {
                rb.AddForce(movement * speed);
            }
        }
        if (rb.velocity.y == 0)
        {
            if (Input.GetKey(key: KeyCode.Space))
            {
                Vector3 jump = new Vector3(0.0f, 20.0f, 0.0f);
                rb.AddForce(jump * jumpspeed);
            }
        }
    }

    void LateUpdate()
    {
        Touching();

        //Tarkistetaan onko pelaaja alustalla
        if (transform.position.y < -3.0f)
        {
            restartLevelMenu.SetActive(true);
            transform.position = Vector3.zero;
            foreach (GameObject opponent in opponents)
            {
                opponent.SetActive(false);
            }
            speed = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Collected: " + count.ToString() + "/10";
        if (count >= 10)
        {
            speed = 0;
            nextLevelMenu.SetActive(true);
            foreach (GameObject opponent in opponents)
            {
                opponent.SetActive(false);
            }
        }
    }

    void Touching()
    {
        //Tarkistetaan koskevatko pelaaja ja vastustaja
        foreach (GameObject opponent in opponents)
        {
            dist = Vector3.Distance(opponent.transform.position, transform.position);

            if (dist <= radius + opponentRadius)
            {
                speed = 0;
                restartLevelMenu.SetActive(true);
                foreach (GameObject oppounent in opponents)
                {
                    oppounent.SetActive(false);
                }
            }
        }
    }
}