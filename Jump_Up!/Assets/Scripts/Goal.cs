using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager; // UIManager 참조

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 지정된 태그의 오브젝트와 충돌했을 때
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Game Clear!");
            uiManager.ShowGameClearUI();
        }
    }
}
