using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform centerPoint; // 플랫폼 이동의 중심 지점
    [SerializeField]
    private float distance = 2f; // 좌우 이동 거리
    [SerializeField]
    private float speed = 2f; // 플랫폼 이동 속도

    private bool movingRight = true; // 플랫폼이 오른쪽으로 이동 중인지 여부

    void Start()
    {
        // centerPoint가 설정되지 않았다면 현재 오브젝트의 위치를 중심으로 설정
        if (centerPoint == null)
        {
            GameObject centerObject = new GameObject("CenterPoint");
            centerObject.transform.position = transform.position;
            centerPoint = centerObject.transform;
        }
    }

    void Update()
    {
        // 이동 목표 지점 계산
        float targetX = movingRight ? centerPoint.position.x + distance : centerPoint.position.x - distance;

        // 플랫폼 이동
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetX, transform.position.y), speed * Time.deltaTime);

        // 목표 지점에 도달하면 방향 변경
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            movingRight = !movingRight;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어를 플랫폼의 자식으로 설정
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 플레이어가 플랫폼에서 떨어지면 부모 설정 해제
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}