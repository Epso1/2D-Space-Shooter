using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoai : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Color initialColor;
    [SerializeField] private Color hitColor;
    [SerializeField] private int hitsToDie = 20;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Sprite initialSprite;
    [SerializeField] private Sprite deadSprite;
    [SerializeField] private Sprite shootSprite;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private GameObject deadPrefab;
    private int pointsWhenHit = 50;
    private int hitCount = 0;
    

    private PolygonCollider2D polCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polCollider2D = GetComponent<PolygonCollider2D>();
        spriteRenderer.sprite = initialSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(GetHit());
        }
    }

    private IEnumerator GetHit()
    {
        hitCount++;
        if (hitCount >= hitsToDie)
        {
            StartCoroutine(BossMoaiDies());
        }
        DataManager.Instance.AddPoints(pointsWhenHit);
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = initialColor;        
    }
    private IEnumerator BossMoaiDies()
    {
        polCollider2D.enabled = false;
        var bigExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        bigExplosion.transform.localScale = new Vector3(2, 2, 0);        
        yield return new WaitForSeconds(0.2f);
        Instantiate(deadPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator Shoot()
    {
        yield return null;
    }

}
