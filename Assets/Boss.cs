using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Boss Settings")]
    public float health = 100f;
    public float maxHealth = 100f;
    public float speed = 4f;
    public float leftAndRightEdge = 15f;

    [Header("Boss Visuals")]
    public Color normalColor = Color.white;
    public Color hurtColor = Color.red;
    private Renderer bossRenderer;
    public Slider healthSlider;

    [Header("Audio Settings")]
    public AudioClip hurtSound;
    private AudioSource bossAudio;

    private bool inHalfPhase = false;
    // Start is called before the first frame update
    void Start()
    {
        bossAudio = GetComponent<AudioSource>();
        bossRenderer = GetComponent<Renderer>();
        bossRenderer.material.color = normalColor;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        if (pos.x < -leftAndRightEdge)
        {
            speed = Mathf.Abs(speed);
        }
        else if (pos.x > leftAndRightEdge)
        {
            speed = -Mathf.Abs(speed);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        if(collidedWith.tag == "Projectile")
        {
            TakeDamage(10f);
            Destroy(collidedWith);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthSlider.value = health;
        bossRenderer.material.color = hurtColor;
        Invoke("ResetColor", 0.1f);
        bossAudio.PlayOneShot(hurtSound);

        if((health <= maxHealth / 2) && !inHalfPhase)
        {
            inHalfPhase = true;
            speed *= 3f;
            leftAndRightEdge += 20f;
        }

        if(health <= 0)
        {
            Destroy(gameObject);
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
            apScript.DefeatBoss();
        }
    }

    void ResetColor()
    {
        bossRenderer.material.color = normalColor;
    }
}
