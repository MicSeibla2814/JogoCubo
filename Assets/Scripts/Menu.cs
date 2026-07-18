using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameManager gameManager;

    private void Start()
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>()
            .gameObject.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setLoopPingPong();
    }
    private void Play()
    {
        GetComponent<CanvasGroup>().LeanAlpha(0, 0.2f).setOnComplete(IniciaGame);
    }
    public void IniciaGame()
    {
        gameManager.Enabled();
    }

   
       
    
}

