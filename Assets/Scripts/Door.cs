using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string scene;
    public void OnTriggerEnter2D(Collider2D doorCollider)
    {
        if (doorCollider.gameObject.name == "Player")
        {
            SceneManager.LoadScene(scene);
            Debug.Log("Scene loaded");
        }
    }
}