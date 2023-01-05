using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour{

    public GameObject player;
    private Vector3 offset;
    private bool third = true;
    private float rotationSpeed = 1.0f;


	void Start ()
    {
        offset = transform.position - player.transform.position;

        if (SceneManager.GetActiveScene().buildIndex == 11)
        {
            third = false;
        }

    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;

        if(third == false)
        {
            float rotation = Input.GetAxis("Vertical") * rotationSpeed;
            transform.Rotate(0, rotation, 0);
            //Quaternion target = Quaternion.Euler(0, rotation, 0);
            //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth)
        }
    }
}
