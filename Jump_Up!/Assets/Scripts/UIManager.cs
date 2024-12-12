using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI heightText;    // 플레이어 높이 표시
    [SerializeField]
    private TextMeshProUGUI timerText;     // 소요 시간 표시
    [SerializeField]
    private LineRenderer trajectoryLine; // 점프 궤적 표시
    [SerializeField]
    private int trajectoryResolution = 30; // 궤적을 그릴 점의 수
    [SerializeField]
    private GameObject gameClearUI; // 게임 클리어 UI 오브젝트

    [Header("Player Settings")]
    [SerializeField]
    private Transform player;   // 플레이어 Transform
    [SerializeField]
    private Transform mapTop;   // 맵의 최상단 Transform
    [SerializeField]
    private Transform mapBottom; // 맵의 최하단 Transform

    private float startTime;

    void Start()
    {
        // 시작 시간 기록
        startTime = Time.time;
        trajectoryLine.positionCount = 0; // 초기에는 궤적을 그리지 않음
    }

    void Update()
    {
        UpdateHeight();
        UpdateTimer();
    }

    // 플레이어 높이를 계산하여 UI에 업데이트
    void UpdateHeight()
    {
        float playerHeight = player.position.y;
        float mapHeight = mapTop.position.y - mapBottom.position.y;
        float heightPercentage = Mathf.Clamp01((playerHeight - mapBottom.position.y) / mapHeight);
        heightText.text = $"Height: {(heightPercentage * 100f):F1}%";
    }

    // 소요 시간을 계산하여 UI에 업데이트
    void UpdateTimer()
    {
        float elapsedTime = Time.time - startTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"Time: {minutes:D2}:{seconds:D2}";
    }

    // 예상 점프 궤적을 계산하고 LineRenderer로 그리기
    public void UpdateTrajectory(Vector2 startPoint, Vector2 jumpDirection, float maxForce)
    {
        trajectoryLine.positionCount = trajectoryResolution;
        trajectoryLine.SetColors(new Color32(255, 80, 80, 255), new Color32(255, 80, 80, 255));

        // 점프 예상 궤적을 계산
        float timeStep = 0.1f;
        Vector2 velocity = jumpDirection * maxForce;
        Vector2[] points = new Vector2[trajectoryResolution];

        // 궤적 계산
        for (int i = 0; i < trajectoryResolution; i++)
        {
            float time = i * timeStep;
            Vector2 displacement = velocity * time + 0.5f * Physics2D.gravity * time * time;
            points[i] = startPoint + displacement / 100;
        }

        // 궤적 라인 그리기
        trajectoryLine.SetPositions(Array.ConvertAll(points, p => (Vector3)p));
    }


    public void ClearTrajectory()
    {
        trajectoryLine.positionCount = 0;
    }

    public void ShowGameClearUI()
    {
        // 게임 클리어 UI 활성화
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }

        // 게임 멈추기
        Time.timeScale = 0;
    }
}
