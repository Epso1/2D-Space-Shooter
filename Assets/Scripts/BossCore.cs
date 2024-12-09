using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCore : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Color initialColor;
    [SerializeField] private Color hitColor;
    [SerializeField] private int hitsToChange = 20;
    [SerializeField] private List<Sprite> sprites;
    private int currenSpriteIndex = 0;
    private BossBigCore boss;
    private int hitChangeCount = 0;
    private CircleCollider2D circleCollider;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        boss = GetComponentInParent<BossBigCore>();
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
        hitChangeCount++;
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.1f);       

        if (hitChangeCount >= hitsToChange)
        {
            hitChangeCount = 0;
            ChangeSprite();
        }
        else
        {
            spriteRenderer.color = initialColor;
        }
    }

    private void ChangeSprite()
    {
        currenSpriteIndex++;
        

        if (currenSpriteIndex >= sprites.Count)
        {
            circleCollider.enabled = false;
            boss.BossDies();
            StartCoroutine(BossCoreDies());
        }
        else
        {
            spriteRenderer.sprite = sprites[currenSpriteIndex];
        }
       
    }

    private IEnumerator BossCoreDies()
    {
        for (int i = 0; i < 10; i++)
        {
            spriteRenderer.color = Color.grey;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.grey;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
