using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpponentMovement : MonoBehaviour
{
    //Määritetään muuttuja nopeus
    private float speed;

    //Muuttuja pelaaja
    public Transform player;

    //Muuttuja maa
    public Transform ground;

    //Nopeus jolla vastustaja voi hypätä
    public float jumpspeed;

    //Maksimimietäisyys jolla vastustaja lähtee seuraamaan
    private int maxDist;

    //Esteet
    public Transform obstacles;

    private Rigidbody rb;

    //Vastustajan säde
    //private float radius = 0.5f;

    //Pelaajan säde
    private float playerRadius = 0.5f;

    //Muuttuja johon tallennetaan vastustajan edellinen paikka
    Vector3 lastPos;

    //Vastustajan nykyinen paikka
    Vector3 currentPos;

    //Alkuperäinen paikka
    Vector3 originalPosition;

    //Määritetään alussa epätodeksi, muutetaan todeksi, jos on liian paikallaan
    private bool wait = false;


    void Start()
    {
        //Määritetään vastustajan nopeus tason mukaan
        if (SceneManager.GetActiveScene().buildIndex < 4)
        {
            speed = 5;
            maxDist = 20;
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 4 && SceneManager.GetActiveScene().buildIndex <= 6)
        {
            speed = 7;
            maxDist = 25;
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 7 && SceneManager.GetActiveScene().buildIndex <= 10)
        {
            speed = 9;
            maxDist = 30;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 11)
        {
            speed = 5;
            maxDist = 20;
        }

        //Asetetaan vastustajalle rigidbody
        rb = GetComponent<Rigidbody>();

        //Määritetään alussa edellinen paikka
        lastPos = transform.position;

        //Määritetään alkuperäinen paikka
        originalPosition = transform.position;
    }



    void FixedUpdate ()
    {
        //Määritetään nopeus jolla vastustaja menee pelaajaa kohti
        float step = speed * Time.deltaTime;

        //Jos pelaaja on tarpeeksi lähellä, vastustaja lähtee liikkeelle
		if(Vector3.Distance(player.position, transform.position) <= maxDist)
        {
            //Kun vastustaja ei ole liikkeessä esteestä poispäin
            if (wait == false)
            {
                //Vastustajan paikka muuttuu kohti pelaajaa
                transform.position = Vector3.MoveTowards(transform.position, player.position, step);

                //Jos vastustajan nopeus y-suunnassa on nolla ja pelaaja on korkeammalla kuin säde, vastustaja voi hypätä
                if (rb.velocity.y == 0 && player.position.y > playerRadius)
                {
                    Vector3 jump = new Vector3(0.0f, 20.0f, 0.0f);
                    rb.AddForce(jump * jumpspeed);
                }
            }
            //Määritetään nykyinen paikka
            currentPos = transform.position;

            //Jos vastustaja on liian paikallaan (törmännyt todennäköisesti esteeseen), se lähtee esteestä poispäin
            if (Mathf.Abs(lastPos.x - currentPos.x) < 0.003 && Mathf.Abs(lastPos.z - currentPos.z) < 0.003 && Mathf.Abs(currentPos.x - originalPosition.x) > 0.0f && Mathf.Abs(currentPos.z - originalPosition.z) > 2.0f)
            {
                //Muutetaan wait todeksi, jotta vastustaja ei lähde samantien uudestaan pelaajaa kohti
                wait = true;

                //Vastustajan paikka muuttuu esteestä poispäin
                transform.position = Vector3.MoveTowards(transform.position, obstacles.position, -step);
            }
            //Jos vastustajan etäisyys esteeseen on pienempi kuin määritetty arvo ja wait on muutettu todeksi, vastustaja jatkaa liikettä esteestä poispäin
            else if(Vector3.Distance(obstacles.position, transform.position) < 7 && wait == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, obstacles.position, -step);
            }
            //Jos vastustaja ei ole paikallaan tai on tarpeeksi kaukana esteestä, muutetaan wait epätodeksi
            else
            {
                wait = false;
            }
            //Tallennetaan nykyinen paikka edelliseksi paikaksi
            lastPos = currentPos;
        }
	}
}
