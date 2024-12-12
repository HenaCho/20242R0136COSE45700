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
    private Vector3 previousPosition; // 이전 프레임의 위치
    public Vector3 CurrentVelocity { get; private set; } // 현재 플랫폼의 속도

    void Start()
    {
        if (centerPoint == null)
        {
            GameObject centerObject = new GameObject("CenterPoint");
            centerObject.transform.position = transform.position;
            centerPoint = centerObject.transform;
        }
        previousPosition = transform.position;
    }

    void Update()
    {
        float targetX = movingRight ? centerPoint.position.x + distance : centerPoint.position.x - distance;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetX, transform.position.y), speed * Time.deltaTime);

        // 목표 지점에 도달하면 방향 변경
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            movingRight = !movingRight;
        }

        // 현재 플랫폼의 속도를 계산
        CurrentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;
    }
}