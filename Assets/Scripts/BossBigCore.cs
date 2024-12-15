using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBigCore : MonoBehaviour
{
    [SerializeField] private List<Transform> shotOrigins;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject bossBullet;
    private int pointsWhenDies = 500;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BossDies()
    {
       StartCoroutine(BossDiesEnum());
    }
    private IEnumerator BossDiesEnum() 
    {
        DataManager.Instance.AddPoints(pointsWhenDies);
        animator.SetTrigger("Death");

        for (int i = 0; i < 10; i++)
        {            
            Vector3 randomPosition = transform.position + new Vector3(Random.Range(-1.6f, 1.6f), Random.Range(-0.8f, 0.8f));
            Instantiate(explosionPrefab, randomPosition, Quaternion.identity);
            spriteRenderer.color = Color.grey;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.grey;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
        }
        var bigExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        bigExplosion.transform.localScale = new Vector3(2, 2, 0);
        gameController.StageClear();
        Destroy(gameObject);

    }

    public void BossShoots()
    {
        foreach (Transform shotOrigin in shotOrigins)
        {
            Instantiate(bossBullet, shotOrigin);
        }
    }
}
