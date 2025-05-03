using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOver : MonoBehaviour, IUIToggleable
{
    [SerializeField] Button RestartButton;
    [SerializeField] Button QuitButton;
    public bool isOpen = false;

    private void Start()
    {
        RestartButton.onClick.AddListener(delegate { RestartGame(); });
        QuitButton.onClick.AddListener(delegate { QuitGame(); });
    }
    public void RestartGame()
    {
        DeathManager.Dead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public bool IsOpen => isOpen;
    public void Toggle()
    {
        isOpen = !isOpen;
        this.gameObject.SetActive(isOpen);
    }
}
