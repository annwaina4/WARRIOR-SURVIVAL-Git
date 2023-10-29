using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorPosition : MonoBehaviour
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
        transform.position
      = new Vector3(this.Player.transform.position.x, this.Player.transform.position.y, this.Player.transform.position.z);
    }
}
