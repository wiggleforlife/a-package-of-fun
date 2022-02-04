using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] Camera camera;

    void Start()
    {
        Debug.Log(camera.aspect);
        if (camera.aspect > 2.32 && camera.aspect < 2.34)
        {
            camera.transform.position = camera.transform.position + new Vector3(-1.56f - camera.transform.position.x, 0, 0);
        }
        else if (camera.aspect > 1.66 && camera.aspect < 1.67)
        {
            camera.transform.position = camera.transform.position + new Vector3(-4.26f - camera.transform.position.x, 0, 0);
        }
    }

    void Update()
    {
        if (player.transform.position.x > transform.position.x + 7)
        {
            transform.position += new Vector3(0.2f, 0, 0);
        }
        else if (player.transform.position.x < transform.position.x - 8 && transform.position.x > -1.5)
        {
            transform.position -= new Vector3(0.2f, 0, 0);
        }
    }
}
