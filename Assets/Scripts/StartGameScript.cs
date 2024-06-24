using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    public GameObject EnemySpawner;
    public GameObject StartMessage;
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelfDestruct()
    {
        Player.GetComponent<PlayerScript>().EndStartState();
        Player.GetComponent<PlayerScript>().AddSoul(1);
        EnemySpawner.SetActive(true);
        Destroy(StartMessage);
        Destroy(this.gameObject);
    }
}
