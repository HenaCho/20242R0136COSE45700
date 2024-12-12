using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpPlatform : MonoBehaviour
{
    [SerializeField]
    private float boostedMaxForce = 15f; // 플랫폼 위에서 강화된 최대 점프 힘
    private float originalMaxForce;     // 원래 최대 점프 힘
    [SerializeField]
    private Color boostedColor = Color.green; // 강화된 점프 시 색상
    private Color originalColor;              // 원래 색상
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어가 플랫폼에 닿았는지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // 기존 maxForce 저장 후 강화된 maxForce 적용
                originalMaxForce = playerController.maxForce;
                playerController.maxForce = boostedMaxForce;
            }
            spriteRenderer.color = boostedColor;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 플레이어가 플랫폼을 떠났을 때 maxForce 복원
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.maxForce = originalMaxForce;
            }
            spriteRenderer.color = originalColor;
        }
    }
}