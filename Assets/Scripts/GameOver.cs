using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void MetodoSair()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void MetodoReiniciar()
    {
        this.gameObject.SetActive(false);
        GameManager.Instance.Enabled();
    }
}
