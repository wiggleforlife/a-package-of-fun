using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUppable : MonoBehaviour
{

    [SerializeField] GameObject player;

    public bool canBePickedUp = false;
    public bool isPickedUp = false;

    // Update is called once per frame
    void Update() {
        if (Math.Abs(player.transform.position.x - transform.position.x) < 2) {
            //tooltip.enabled = true;
            canBePickedUp = true;
        } else {
            //tooltip.enabled = false;
            canBePickedUp = false;
        }

        if (isPickedUp == true) {
            transform.position = player.transform.position + new Vector3(1, 0, 0);
        } 
    }

}
