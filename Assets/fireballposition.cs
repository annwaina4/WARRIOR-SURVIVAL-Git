using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballposition : MonoBehaviour
{
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        this.Player = GameObject.Find("Player-Default");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(this.Player.transform.rotation.x, this.Player.transform.rotation.y, this.Player.transform.rotation.z, Space.Self);    
    }
}
