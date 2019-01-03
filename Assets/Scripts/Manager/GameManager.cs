using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	private UIManager myUiManager;
	public UIManager MyUiManager => myUiManager;
	private PlayerController player;
	public PlayerController Player => player;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoadingScene;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoadingScene;
	}

	private void OnLevelFinishedLoadingScene(Scene scene, LoadSceneMode mode)
	{
		Setup();
	}

	private void Setup()
	{
		player = FindObjectOfType<PlayerController>();
		//player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		myUiManager.Setup();
	}
	
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

		myUiManager = GetComponent<UIManager>();
		Setup();
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