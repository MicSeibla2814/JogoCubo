using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Header("Auto Destroy Settings")]
    [Range(0f, 10f)]
    [SerializeField] private float destroyTime = 3f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }


















}
