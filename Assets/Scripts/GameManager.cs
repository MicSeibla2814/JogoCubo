using System.Collections;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Menu Pause")]
    public GameObject menuPause;

    [Header("Menu Iniciar")]
    public GameObject menuIniciar;
    bool gameStarted = false;

    [Header("Menu GameOver")]
    public GameObject menuGameOver;
    public CinemachineCamera cam;
    public CinemachineCamera camZoom;
    public GameObject player;

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
        //StartCoroutine(SpawnObstacle());
    }

    private void Update()
    {
        if (gameOver == true) return;

        Score();

        //Pause
        MetodoPause();
    }

    public void MetodoPause()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Time.timeScale == 0f)
            {


                StartCoroutine(ScaleTime(0f, 1f, 0.5f));
                menuPause.SetActive(false);
            }
            else if (Time.timeScale == 1f)
            {

                StartCoroutine(ScaleTime(1f, 0f, 0.5f));
                menuPause.SetActive(true);
            }
        }
    }

    private IEnumerator SpawnObstacle()
    {
        while (!gameOver || gameStarted == true)
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
        //abrir o menu de game Over
        menuGameOver.SetActive(true);
        
        //Animação de cameras
        cam.gameObject.SetActive(false);
        camZoom.gameObject.SetActive(true);

        //Desativar o game Manager
        gameObject.SetActive(false);
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

    public void Enabled()
    {
        player.SetActive(true);
        gameObject.SetActive(true);
        menuIniciar.SetActive(false);

        gameOver = false;
        score = 0;
        timeScore = 0;

        cam.gameObject.SetActive(true);
        camZoom.gameObject.SetActive(false);

        StartCoroutine(SpawnObstacle());
    }


//===========================Pause Menu=================================
public IEnumerator ScaleTime(float start, float end, float duration)
    {
        //Armazena o tempo inicial
        float lastTime = Time.realtimeSinceStartup;
        float timer = 0.0f;

        while(timer < duration)
        {
            //Ajusta o tempo de escala
            Time.timeScale = Mathf.Lerp(start, end, timer / duration);

            //Ajustar o deltaTime para o tempo real
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            //Compensar a taxa de quadros
            timer += (Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;

            yield return null;
        }
        Time.timeScale = end;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

 
}