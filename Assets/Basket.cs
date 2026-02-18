using UnityEngine;
using TMPro;

public class Basket : MonoBehaviour
{
    [Header("Set Dynamically")]
    public TextMeshProUGUI scoreGT;
    public TextMeshProUGUI ammoGT;

    [Header("Basket Color Settings")]
    public int currentColor = 0;
    public Color[] colors =
    {
        Color.red,
        Color.green,
        Color.blue
    };

    [Header("Hard Mode Settings")]
    public int appleAmmo = 0;
    public float lightRadiusMult = 2f;
    public GameObject appleProjectile;
    public float firingForce = 20f;

    [Header("Audio Settings")]
    public AudioClip catchSound;
    public AudioClip shootSound;
    private AudioSource basketAudio;

    private Light basketLight;
    private Renderer basketRenderer;
    private AppleTree treeScript;

    // Start is called before the first frame update
    void Start()
    {
        basketAudio = GetComponent<AudioSource>();
        basketRenderer = GetComponent<Renderer>();
        GameObject tree = GameObject.Find("Apple Tree");
        basketLight = GetComponentInChildren<Light>(); 
        treeScript = tree.GetComponent<AppleTree>();

        if(treeScript.levelType == LevelDifficulty.Medium)
        {
            UpdateColor();
        }
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        scoreGT = scoreGO.GetComponent<TextMeshProUGUI>();
        scoreGT.text = "0";

        GameObject ammoGO = GameObject.Find("AmmoCounter");
        if (ammoGO != null) 
        {
            ammoGT = ammoGO.GetComponent<TextMeshProUGUI>();
            ammoGT.text = "Apple Ammo: " + appleAmmo.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 pos = transform.position;
        pos.x = mousePos3D.x;
        transform.position = pos;

        if (treeScript.levelType == LevelDifficulty.Hard)
        {
            HardModeControls();
        }
        if (treeScript.levelType == LevelDifficulty.Medium)
        {
            ColorSwitching();
        }
    }

    void ColorSwitching()
    {
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

    // This handles the light control and shooting in the hard difficulty
    void HardModeControls()
    {
        if(basketLight != null)
        {
            basketLight.range = 10f + (appleAmmo * lightRadiusMult);
            if(appleAmmo < 2)
            {
                basketLight.intensity = 5.0f; 
            }
            else
            {
                basketLight.intensity = 2.0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && appleAmmo > 0)
        {
            FireApple();
        }
    }

    void FireApple()
    {
        basketAudio.PlayOneShot(shootSound);
        appleAmmo--;
        ammoGT.text = "Apple Ammo: " + appleAmmo.ToString();
        GameObject projectile = Instantiate(appleProjectile, transform.position + Vector3.up * 1f,  Quaternion.identity);

        Apple appleScript = projectile.GetComponent<Apple>();
        appleScript.isMagnetic = false;
        
        projectile.GetComponent<Renderer>().material.color = Color.yellow;
        projectile.tag = "Projectile";

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.AddForce(Vector3.up * firingForce, ForceMode.Impulse);
        Destroy(projectile, 3f);
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        if (collidedWith.tag == "Projectile")
        {
            return; 
        }
        if(collidedWith.tag == "Apple")
        {
            basketAudio.PlayOneShot(catchSound);
            if(treeScript.levelType == LevelDifficulty.Hard)
            {
                appleAmmo++;
                ammoGT.text = "Apple Ammo: " + appleAmmo.ToString();
            }
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