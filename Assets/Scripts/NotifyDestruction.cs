
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NotifyDestruction : MonoBehaviour
{
    private void OnDestroy()
    {
        // Busca el componente Destructible en el padre y notifica la destrucción
        Destructible destructible = GetComponentInParent<Destructible>();
        if (destructible != null)
        {
            destructible.NotifyDestruction();
        }
    }
}

