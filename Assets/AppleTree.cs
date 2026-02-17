using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelDifficulty
    {
        Easy,
        Medium,
        Hard
    }

public class AppleTree : MonoBehaviour
{
    [Header("Set in Inspector")]
    public LevelDifficulty levelType;
    public GameObject applePrefab;
    public Color[] appleColors = {
        Color.red,
        Color.green,
        Color.blue 
    };

    public float speed = 1f;
    public float leftAndRightEdge = 10f;
    public float chanceToChangeDirections = 0.05f;
    public float secondsBetweenAppleDrops = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DropApple", 2f);
    }

    void DropApple()
    {
        GameObject apple = Instantiate<GameObject>(applePrefab);
        apple.transform.position = transform.position;
    
        Apple appleScript = apple.GetComponent<Apple>();
        if (appleScript != null)
        {
            if(levelType == LevelDifficulty.Easy)
            {
                appleScript.appleColor = 0; 
                apple.GetComponent<Renderer>().material.color = appleColors[0];
                appleScript.isMagnetic = true;
            }
            else if(levelType == LevelDifficulty.Medium)
            {
                int randColor = Random.Range(0, appleColors.Length);
                appleScript.appleColor = randColor;
                apple.GetComponent<Renderer>().material.color = appleColors[randColor];
                appleScript.isMagnetic = false;
            }
            else if(levelType == LevelDifficulty.Hard)
            {
                appleScript.appleColor = 0;
                apple.GetComponent<Renderer>().material.color = appleColors[0];
                appleScript.isMagnetic = false;
            }
        }
        Invoke("DropApple", secondsBetweenAppleDrops);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        if(pos.x < -leftAndRightEdge)
        {
            speed = Mathf.Abs(speed);
        }
        else if(pos.x > leftAndRightEdge)
        {
            speed = -Mathf.Abs(speed);
        }
    }

    void FixedUpdate()
    {
        if(UnityEngine.Random.value < chanceToChangeDirections)
        {
            speed *= -1;
        }
    }
}
