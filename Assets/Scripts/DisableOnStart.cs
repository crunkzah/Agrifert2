using UnityEngine;

public class DisableOnStart : MonoBehaviour
{
    void Start()
    {
        this.gameObject.SetActive(false);
    }
}
