using Unity.Cinemachine;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public ParticleSystem particleDestruction;

    [Header("Cinemachine")]
    private CinemachineImpulseSource _impulseSource;
    private PlayerController player;

    private void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        player = FindAnyObjectByType<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Cria o efeito de destruição
        ParticleSystem particle = Instantiate(
            particleDestruction,
            transform.position,
            Quaternion.identity
        );

        // Calcula a distância até o jogador
        if (player != null && _impulseSource != null)
        {
            float distance = Vector3.Distance(
                transform.position,
                player.transform.position
            );

            // Evita divisão por zero
            distance = Mathf.Max(distance, 0.1f);

            float force = 1f / distance;

            // Aplica o Shake da câmera
            _impulseSource.GenerateImpulse(force);
        }

        // Se colidiu com o Player
        if (collision.gameObject.CompareTag("Player"))
        {
            Transform fire = particle.transform.Find("Fire");

            if (fire != null)
            {
                fire.gameObject.SetActive(false);
            }
        }

        // Destrói o obstáculo original
        Destroy(gameObject);
    }
}