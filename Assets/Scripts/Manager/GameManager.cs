using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	private UIManager myUIManager;
	public UIManager MyUiManager => myUIManager;
	
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		myUIManager = GetComponent<UIManager>();
	}

	public void LoadLevel(string nameLevel)
	{
		SceneManager.LoadScene(nameLevel);
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}