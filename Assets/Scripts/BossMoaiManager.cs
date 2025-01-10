using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoaiManager : MonoBehaviour
{
    [SerializeField] List<BossMoai> bossMoais = new List<BossMoai>();
    [SerializeField] GameObject explosionPrefab;   
    [SerializeField] private int pointsWhenDies = 500;

    private GameController gameController;
    public int hitCount;
    private void Awake()
    {
        hitCount = bossMoais.Count;
        DisableColliders();
    }
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        
    }
    public void MoaisStartShooting()
    {
        foreach(BossMoai boss in bossMoais)
        {
            boss.StartShooting();
        }
    }

    public void BossDies()
    {
        StartCoroutine(BossDiesEnum());
    }
    private IEnumerator BossDiesEnum()
    {
        DataManager.Instance.AddPoints(pointsWhenDies);
        var deadMoais = GameObject.FindGameObjectsWithTag("BossBody");

        foreach(GameObject deadMoai in deadMoais)
        {
            var bigExplosion = Instantiate(explosionPrefab, deadMoai.transform.position, Quaternion.identity);
            bigExplosion.transform.localScale = new Vector3(4, 4, 0);
            Destroy(deadMoai);
        }
        yield return new WaitForSeconds(2f);
        gameController.StageClear();       
        Destroy(gameObject);
    }

    public void DecreaseHitCount()
    {
        hitCount--;

        if (hitCount == 0)
        {
            Debug.Log("hit count decreased to 0.");
            BossDies();
        } 
    }

    public void EnableColliders() 
    {
        foreach(BossMoai bossMoai in bossMoais)
        {
            bossMoai.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }

    public void DisableColliders()
    {
        foreach (BossMoai bossMoai in bossMoais)
        {
            bossMoai.GetComponent<PolygonCollider2D>().enabled = false;
        }
    }
}
