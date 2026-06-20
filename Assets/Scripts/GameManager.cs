using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Obstaculo Settings")]
    [SerializeField] private GameObject obstacle;
    public float timeSpawn = 2f;
    public bool gameOver = false;
    [Range(0f, 20f)]
    [SerializeField] private float xSpawn = 7f;
    [Range(0f, 20f)]
    [SerializeField] private float ySpawn = 11f;
    [Range(0f, 20f)]
    [SerializeField] private float speedFalling = 2f;//Velocidade de queda
    [Range(0f, 20f)]
    [SerializeField] private int numberSpawn = 4;//Quantidade de obstaculos a serem gerados
    [Range(0f, 20f)]
    [SerializeField] private float speedRotation = 0.5f;//Velocidade de rotação

    [Header("Pontuação")]
    public TextMeshProUGUI txtScore;
    public int score = 0;
    public float timeScore = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnObstacle());
    }

    private void Update()
    {
        if (gameOver == true) return;

        Score();
    }
    private IEnumerator SpawnObstacle()
    {
        while (!gameOver)
        {
            var obstacleToSpawn = Random.Range(0, numberSpawn);

            for (int i = 0; i < obstacleToSpawn; i++)
            {
                //Posição aleatória em X
                float x = Random.Range(-xSpawn, xSpawn);

                //Instanciar o Obstaculo
                GameObject objObstacle = Instantiate(
                    obstacle,
                    new Vector3(x, ySpawn, 0f),
                    Quaternion.identity);

                //Velocidade Aleatória de queda
                float damping = Random.Range(0f, speedFalling);

                Rigidbody rb =
                    objObstacle.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.linearDamping = damping;

                    rb.AddTorque(
                        new Vector3(
                        Random.Range(-speedRotation, speedRotation),
                        Random.Range(-speedRotation, speedRotation),
                        Random.Range(-speedRotation, speedRotation)
                        ),
                        ForceMode.Impulse
                        );
                }
            }
            yield return new WaitForSeconds(timeSpawn);
        }
    }

    public void GameOver()
    {
        gameOver = true;
        StopCoroutine(SpawnObstacle());
    }

    public void Score()
    {
        timeScore += Time.deltaTime;
        if(timeScore >= 1)
        {
            score++;
            txtScore.text = $"Score: {score}";

            timeScore = 0;

        }
    }
}