using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Forever;

public class Player : MonoBehaviour
{
    Runner runner;

    public CounterManager counterManager;
    public Shoot shoot;
    public Menu GameOverScreen;

    public Vector2 input = Vector2.zero;
    public Vector2 lerpInput = Vector2.zero;
    public float sensitivity;
    public float sensitivityPowered;

    public float dodgeSpeed = 10f;

    public float startSpeed = 1f;
    public float maxSpeed = 8f;

    public float startAgility = 0.05f;
    public float maxAgility = 0.1f;

    public float acceleration = 0.02f;
    public float agilityAcceleration = 0.0003f;

    public float accMultiplier = 1f;
    public float reflexesModifier = 0.02f;

    public bool playing = false;
    public bool reflexesCoroutineRunning = false;

    public string difficulty = "Normal";


    private void Awake()
    {
        playing = true;
        runner = GetComponent<Runner>();
        runner.followSpeed = startSpeed;
        sensitivity = startAgility;

        Debug.Log("Current difficulty: " + PlayerPrefs.GetString("Difficulty", "Normal"));
        difficulty = PlayerPrefs.GetString("Difficulty", "Normal");

        switch (difficulty)
        {
            case ("Easy"):
                Debug.Log("Easy mode activated");
                accMultiplier = 0.5f;
                break;
            case ("Normal"):
                Debug.Log("Normal mode activated");
                accMultiplier = 1f;
                break;
            case ("Hard"):
                Debug.Log("Hard mode activated");
                accMultiplier = 1.5f;
                break;
        }
    }

    void FixedUpdate()
    {
        if (playing)
        {
            runner.followSpeed = Mathf.MoveTowards(runner.followSpeed, maxSpeed, Time.deltaTime * acceleration * accMultiplier);

            sensitivity = Mathf.MoveTowards(sensitivity, maxAgility, Time.deltaTime * agilityAcceleration * accMultiplier);

            // If reflexes power up is active
            if (reflexesCoroutineRunning)
            {
                sensitivityPowered = Mathf.MoveTowards(sensitivityPowered, maxAgility, Time.deltaTime * agilityAcceleration * accMultiplier);
                input.x += Input.GetAxis("Horizontal") * sensitivityPowered;
                input.y += Input.GetAxis("Vertical") * sensitivityPowered;
            }
            else
            {
                input.x += Input.GetAxis("Horizontal") * sensitivity;
                input.y += Input.GetAxis("Vertical") * sensitivity;
            }

            input.x = Mathf.Clamp01(input.x);
            input.y = Mathf.Clamp01(input.y);
            // For smooth movement
            lerpInput.x = Mathf.Lerp(lerpInput.x, input.x, Time.deltaTime * dodgeSpeed);
            lerpInput.y = Mathf.Lerp(lerpInput.y, input.y, Time.deltaTime * dodgeSpeed);

            runner.motion.offset = new Vector2(Mathf.Lerp(-0.52f, 0.52f, lerpInput.x), Mathf.Lerp(-0.5f, 0.3f, lerpInput.y));
        }

    }

    // An obstacle is hit
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision!");

        if (collision.collider.tag == "Obstacle")
        {
            Debug.Log("Obstacle!");
            GameOver();

        }
    }


    // A powerup is hit
    void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.name)
        {
            case "ShieldPowerUp":
                StartCoroutine(Shield());
                break;
            case "DoublePointsPowerUp":
                StartCoroutine(DoublePoints());
                break;
            case "FireRatePowerUp":
                StartCoroutine(FireRate());
                break;
            case "ReflexesPowerUp":
                StartCoroutine(Reflexes());
                break;
            case "SlowMoPowerUp":
                StartCoroutine(SlowMo());
                break;
        }

    }
    private void GameOver()
    {

        Debug.Log("Game over!");
        runner.follow = false;
        playing = false;
        GameOverScreen.Setup();
    }
    IEnumerator Shield()
    {
        Debug.Log("Shield powerup activated");
        Physics.IgnoreLayerCollision(8, 9, true);

        yield return new WaitForSeconds(10f);

        Physics.IgnoreLayerCollision(8, 9, false);
        Debug.Log("Shield wore off");
    }

    IEnumerator DoublePoints()
    {
        Debug.Log("Double points powerup activated");
        counterManager.pointsToAdd = 2;
        yield return new WaitForSeconds(10f);

        counterManager.pointsToAdd = 1;
        Debug.Log("Double points wore off");
    }

    IEnumerator FireRate()
    {
        Debug.Log("Shoot speed coroutine started");
        shoot.fireRate = 0.15f;
        yield return new WaitForSeconds(10f);

        shoot.fireRate = 0.3f;
        Debug.Log("Shoot speed wore off");
    }

    IEnumerator Reflexes()
    {
        reflexesCoroutineRunning = true;
        Debug.Log("Reflexes powerup activated");
        sensitivityPowered = sensitivity + reflexesModifier; // Grab current sensitivity and add modifier to increase reflexes
        yield return new WaitForSeconds(10f);

        Debug.Log("Reflexes wore off");
        reflexesCoroutineRunning = false;
    }

    IEnumerator SlowMo()
    {
        Debug.Log("Slow mo powerup activated");
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        yield return new WaitForSeconds(5f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        Debug.Log("Slow mo wore off");
    }

}