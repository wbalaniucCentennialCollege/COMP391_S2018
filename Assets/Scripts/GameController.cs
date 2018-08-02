using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// NEW USING STATEMENTS
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    [Header("Wave Settings")]
    public GameObject hazard;   // What are we spawning?
    public Vector2 spawnValue;  // Where do we spawn our hazards?
    public int hazardCount;     // How many hazards per wave?
    public float startWait;     // How long until the first wave?
    public float spawnWait;     // How long between each hazard in each wave? 
    public float waveWait;      // How long between each wave of hazards?

    [Header("Text Options")]
    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    [Header("Audio Options")]
    public AudioClip gameOverClip;

    private bool gameOver;
    private bool restart;
    private int score;
    private AudioSource aSource;

	// Use this for initialization
	void Start () {
        score = 0;
        gameOver = false;
        restart = false;
        aSource = GetComponent<AudioSource>();

        scoreText.text = "";

        UpdateScore();

        StartCoroutine(SpawnWaves()); // Runs a function separate from the rest of the code (in it's own thread)
	}
	
	// Update is called once per frame
	void Update () {
		// Check whether you are restarting
        if(restart)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                // THE OLD WAY (DON'T DO THIS) of reloading a scene
                // Application.LoadLevel(Application.loadedLevel);
                // SceneManager.LoadScene("Game"); // <-- This is fine, but is prone to errors
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // <-- Better, but more complicated
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

    // IEnumerator return type required for Coroutines
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait); // Pause. This will "wait" for "startWait" seconds
        while(true)
        {
            for(int i = 0; i < hazardCount; i++)
            {
                Vector2 spawnPosition = new Vector2(spawnValue.x, Random.Range(-spawnValue.y, spawnValue.y));
                //                                      11                          -4              4
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait); // Wait time between spawning each asteroid
            }
            yield return new WaitForSeconds(waveWait); // Delay between each wave of enemies.

            if(gameOver)
            {
                restartText.gameObject.SetActive(true);
                restartText.text = "Press R for Restart";
                restart = true;
                break;
            }
        }
    }

    // Update the text of the score
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    // Accepts score values and calls UpdateScore
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        // Optional and overkill
        // gameOverText.text = "Game Over";
        gameOver = true;

        // Access the AudioSource component
        // Change the clip from the background music to the game over music
        aSource.clip = gameOverClip;
        aSource.loop = false;
        // Play the clip
        aSource.Play();
    }
}
