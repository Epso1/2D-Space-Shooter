using UnityEngine;
using UnityEngine.UI;
public class ButtonEnablerTimer : MonoBehaviour
{
    Button button;
    [SerializeField] float timeToEnable = 2f;
   
    void Awake()
    {
        button = GetComponent<Button>();
        button.enabled = false;
        button.gameObject.SetActive(false);
        Invoke("EnableButton", timeToEnable);
    }

    void EnableButton()
    {
        button.gameObject.SetActive(true);
        button.enabled = true;
    }

}
