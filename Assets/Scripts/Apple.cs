using UnityEngine;

public class Apple : MonoBehaviour
{
    private Transform basketTransform;
    private Rigidbody rb;

    [Header("Difficulty")]
    public LevelDifficulty myLevel;

    [Header("Magnetic Settings")]
    public bool isMagnetic = false;
    public float magnetRange = 15f;
    public float magnetStrength = 20f;
    public static float bottomY = -20f;

    [Header("Color Settings")]
    public int appleColor = 0;
    public bool canChangeColor = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject basket = GameObject.FindWithTag("Player");
        if(basket != null) 
        {
            basketTransform = basket.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < bottomY)
        {
            Destroy(gameObject);
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
            apScript.AppleDestroyed();
        }
    }

    void FixedUpdate()
    {
        if (!isMagnetic) return;

        if(rb != null && basketTransform != null)
        {
            float distance = Vector3.Distance(transform.position, basketTransform.position);
            if(distance < magnetRange) 
            {
                rb.useGravity = false;
                Vector3 direction = (basketTransform.position - transform.position).normalized;
                rb.velocity = direction * magnetStrength;
            }
            else
            {
                rb.useGravity = true;
            }
        }
    }
}
