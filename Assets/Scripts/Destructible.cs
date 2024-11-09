using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private int parentedQuantity = 2;
    // Contador de sistemas de hijos destruidos
    private int parentedDestroyed = 0;

    // Método que se llama cuando un hijo se destruye
    public void NotifyDestruction()
    {
        parentedDestroyed++;

        // Verifica si los hijos han sido destruidos
        if (parentedDestroyed >= parentedQuantity)
        {
            // Destruye el GameObject padre
            Destroy(gameObject);
        }
    }
}
