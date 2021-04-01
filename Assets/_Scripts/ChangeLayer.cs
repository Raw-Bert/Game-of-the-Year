using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayer : MonoBehaviour
{
    public GameObject player;
    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > this.transform.position.y)
        {
            this.GetComponent<Renderer>().sortingOrder = player.GetComponent<Renderer>().sortingOrder + 1;
        }
        else
            this.GetComponent<Renderer>().sortingOrder = player.GetComponent<Renderer>().sortingOrder - 1;
    }
}
