using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    // Velocidad del desplazamiento
    public float scrollSpeed = 2f;

    // Cantidad de instancias iniciales del fondo
    public int initialBackgroundCount = 3;

    private GameObject[] backgroundObjects;
    private float backgroundWidth;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null || spriteRenderer.sprite == null)
        {
            Debug.LogError("Por favor, asegúrate de que el objeto tiene un SpriteRenderer con un sprite asignado.");
            return;
        }

        // Obtener el ancho del sprite
        backgroundWidth = spriteRenderer.sprite.bounds.size.x;
        backgroundObjects = new GameObject[initialBackgroundCount];

        for (int i = 0; i < initialBackgroundCount; i++)
        {
            backgroundObjects[i] = CreateBackgroundSegment(i * backgroundWidth);
        }
    }

    void Update()
    {
        // Mover los segmentos del fondo
        for (int i = 0; i < backgroundObjects.Length; i++)
        {
            backgroundObjects[i].transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

            // Reubicar el fondo si sale de la pantalla
            if (backgroundObjects[i].transform.position.x < -backgroundWidth)
            {
                RepositionBackgroundSegment(i);
            }
        }
    }

    private GameObject CreateBackgroundSegment(float xPosition)
    {
        GameObject bgSegment = new GameObject("BackgroundSegment");
        SpriteRenderer renderer = bgSegment.AddComponent<SpriteRenderer>();
        renderer.sprite = spriteRenderer.sprite;
        renderer.sortingLayerID = spriteRenderer.sortingLayerID;
        renderer.sortingOrder = spriteRenderer.sortingOrder;

        bgSegment.transform.position = new Vector3(xPosition, 0, 0);

        return bgSegment;
    }

    private void RepositionBackgroundSegment(int index)
    {
        // Buscar el segmento más a la derecha
        float maxX = float.MinValue;
        for (int i = 0; i < backgroundObjects.Length; i++)
        {
            if (i != index && backgroundObjects[i].transform.position.x > maxX)
            {
                maxX = backgroundObjects[i].transform.position.x;
            }
        }

        // Reubicar este segmento a la derecha del más lejano
        backgroundObjects[index].transform.position = new Vector3(maxX + backgroundWidth, 0, 0);
    }
}
