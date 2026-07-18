using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public ParticleSystem particleDestruction;

    [Header("Cinemachine")]
    private CinemachineImpulseSource _impulseSource;//Variavel
    private PlayerController player;

    private void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();//Referencia
        player = FindAnyObjectByType<PlayerController>();//Busca o player na cena

    }
   private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Obstacle"))
        {
            ParticleSystem particle = Instantiate(
                particleDestruction,
                transform.position,
                Quaternion.identity);

            if (player != null)
            {
                var distance =
                    Vector3.Distance(transform.position, player.transform.position);

                var force = 1 / distance;//Quanto maior a distancia menor a força

                _impulseSource.GenerateImpulse(force);//realiza Shake
            }
            if(collision.gameObject.CompareTag("Player"))
            {
                Transform fire = particle.transform.Find("Fire");

                if (fire != null)
                    fire.gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }
}}
