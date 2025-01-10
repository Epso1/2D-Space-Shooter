using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private float shotCoolDownTime = 1f;
    private int pointsWhenHit = 50;
    private int hitCount = 0;
    private Transform player;
    private BossMoaiManager bossMoaiManager;
    

    private PolygonCollider2D polCollider2D;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polCollider2D = GetComponent<PolygonCollider2D>();
        spriteRenderer.sprite = initialSprite;     
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        bossMoaiManager = GetComponentInParent<BossMoaiManager>();
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
        var deadMoai = Instantiate(deadPrefab, transform.position, Quaternion.identity);
        deadMoai.transform.localScale = this.transform.localScale;
        bossMoaiManager.DecreaseHitCount();
        Destroy(gameObject);
    }

    private IEnumerator StartShootingEnum()
    {
        while (true)
        {
            if (player != null)
            {
                Vector2 shotDirection = (player.position - transform.position).normalized;
                spriteRenderer.sprite = shootSprite;
                var bulletObject = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
                bulletObject.GetComponent<EnemyBullet>().direction = shotDirection;
                yield return new WaitForSeconds(0.2f);
                spriteRenderer.sprite = initialSprite;
            }            
            yield return new WaitForSeconds(shotCoolDownTime);
        }       
    }

    public void StartShooting()
    {
        StartCoroutine(StartShootingEnum());
    }

}
