using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {
    
    public GameObject explosionAsteroid;
    public GameObject explosionPlayer;
    public int scoreValue = 10;

    private GameController gameControllerScript;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        
        if(gameControllerObject != null)
        {
            // Do something with the game object
            // Reference only the GameController script of the GameController object
            gameControllerScript = gameControllerObject.GetComponent<GameController>();
        }

        if(gameControllerScript == null)
        {
            Debug.Log("Cannot find Game Controller script on Object");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Boundary")
        {
            return;
        }

        if(other.tag == "Player")
        {
            Instantiate(explosionPlayer, other.transform.position, other.transform.rotation);

            gameControllerScript.GameOver();
        }
        else
        { 
            gameControllerScript.AddScore(scoreValue); // Passes score value to AddScore function in gameController
        }

        // Instantiate asteroid explosion
        Instantiate(explosionAsteroid, this.transform.position, this.transform.rotation);

        Destroy(other.gameObject);
        Destroy(this.gameObject);
    }
}
