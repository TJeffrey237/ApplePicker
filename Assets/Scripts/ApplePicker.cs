using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject basketPrefab;
    public int numBaskets = 1;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;    
    public List<GameObject> basketList;

    public AudioClip basketHitSound;
    private AudioSource basketAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        basketAudio = GetComponent<AudioSource>();
        basketList = new List<GameObject>();
        for(int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBasketGO.transform.position = pos;
            basketList.Add(tBasketGO);
        }
    }

    public void AppleDestroyed()
    {
        basketAudio.PlayOneShot(basketHitSound);
        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach(GameObject tGO in tAppleArray)
        {
            Destroy(tGO);
        }

        int basketIndex = basketList.Count - 1;
        GameObject tBasketGO = basketList[basketIndex];
        basketList.RemoveAt(basketIndex);
        Destroy(tBasketGO);

        if(basketList.Count == 0)
        {
            SceneManager.LoadSceneAsync(0);
        }
    }

    public void DefeatBoss()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
