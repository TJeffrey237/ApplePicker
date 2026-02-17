using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Basket : MonoBehaviour
{
    [Header("Set Dynamically")]
    public TextMeshProUGUI scoreGT;

    public int currentColor = 0;
    public Color[] colors =
    {
        Color.red,
        Color.green,
        Color.blue
    };

    private Renderer basketRenderer;
    private AppleTree treeScript;

    // Start is called before the first frame update
    void Start()
    {
        basketRenderer = GetComponent<Renderer>();
        GameObject tree = GameObject.Find("Apple Tree"); 
        treeScript = tree.GetComponent<AppleTree>();

        if(treeScript.levelType == LevelDifficulty.Medium)
        {
            UpdateColor();
        }
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        scoreGT = scoreGO.GetComponent<TextMeshProUGUI>();
        scoreGT.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;

        if (treeScript.levelType != LevelDifficulty.Medium)
        {
            return; 
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentColor = 0;
            UpdateColor();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentColor = 1;
            UpdateColor();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentColor = 2;
            UpdateColor();
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        if(collidedWith.tag == "Apple")
        {
            Apple appleScript = collidedWith.GetComponent<Apple>();
            if(appleScript.appleColor == this.currentColor)
            {
                int score = int.Parse(scoreGT.text);
                score += 100;
                scoreGT.text = score.ToString();
                if(score > HighScore.score)
                {
                    HighScore.score = score;
                }
            }
            else
            {
                ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
                apScript.AppleDestroyed();
            }

            Destroy(collidedWith);
        }
    }

    void UpdateColor()
    {
        basketRenderer.material.color = colors[currentColor];
    }
}