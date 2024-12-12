using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 재시작을 위한 네임스페이스

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startPoint; // 드래그 시작 위치 (항상 플레이어 위치)
    private Vector2 endPoint;   // 드래그 끝 위치
    private bool isDragging = false;    // 드래그 중인지 여부
    public float maxForce = 10f;   // 최대 점프 힘 (거리로 계산)
    private float gravityScale = 1f;    // 기본 중력 스케일
    private float fallMultiplier = 2.5f;    // 가속도 증가 비율
    private bool isGrounded = false;    // 착지 여부를 나타내는 변수
    [SerializeField]
    private float deathHeight = -10f;   // 플레이어가 죽는 높이 (y 좌표)

    [SerializeField]
    private UIManager uiManager;  // UIController 참조 추가
    private Animator animator;
    private SoundManager soundManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale; // 초기 중력 스케일 설정
        animator = GetComponent<Animator>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    void Update()
    {
        // 마우스 버튼을 눌렀을 때 드래그 시작
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = transform.position; // 드래그 시작점을 항상 플레이어 위치로 설정
            isDragging = true;
        }

        // 드래그 중일 때 점프 궤적을 업데이트
        if (isDragging & isGrounded)
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 드래그 끝점 설정
            uiManager.UpdateTrajectory(startPoint, startPoint - endPoint, maxForce); // 예상 점프 궤적 업데이트
        }

        // 드래그를 놓으면 점프
        if (Input.GetMouseButtonUp(0) && isDragging && isGrounded)
        {
            Jump();
            isDragging = false;
            uiManager.ClearTrajectory(); // 점프 후 궤적 제거
        }

        ApplyAcceleration();
        UpdateAnimation();

        // 플레이어가 특정 높이 이하로 떨어지면 죽음 처리
        if (transform.position.y < deathHeight)
        {
            Die();
        }
    }

    void Jump()
    {
        // 드래그 시작 지점(플레이어 위치)과 끝 지점의 차이로 점프 방향과 힘을 계산
        Vector2 jumpDirection = (startPoint - endPoint).normalized;
        float jumpDistance = Vector2.Distance(startPoint, endPoint);
        float jumpForce = Mathf.Clamp(jumpDistance, 0, maxForce); // 최대 힘을 초과하지 않도록 제한

        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse); // 힘을 이용해 점프

        // 점프 방향에 따라 플레이어의 좌우 반전
        if (jumpDirection.x < 0)
        {
            transform.localScale = new Vector3(-2.5f, 2.5f, 1); // 왼쪽으로 점프하면 반전
        }
        else if (jumpDirection.x > 0)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 1); // 오른쪽으로 점프하면 원래 상태
        }

        isGrounded = false;

        soundManager.PlaySFX(0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 플랫폼에 착지하면 다시 점프 가능
        if (collision.gameObject.CompareTag("Platform"))
        {
            rb.velocity = Vector2.zero;  // 착지 시 속도 초기화
            isGrounded = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            MovingPlatform platform = collision.gameObject.GetComponent<MovingPlatform>();
            if (platform != null)
            {
            // 플랫폼의 이동 속도를 플레이어에 추가
                transform.position += platform.CurrentVelocity * Time.deltaTime;
            }
        }
    }


    void ApplyAcceleration()
    {
        // 플레이어가 아래로 떨어지는 중일 때
        if (rb.velocity.y < 0)
        {
            // 중력 가속도를 더하여 더 빠르게 떨어지도록 설정
            rb.gravityScale = gravityScale * fallMultiplier;
        }
        else
        {
            // 점프 시나 상승 중일 때는 기본 중력만 적용
            rb.gravityScale = gravityScale;
        }
    }

    void UpdateAnimation()
    {
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isGrounded", true);
        }
        else
        {
            if (rb.velocity.y > 0)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
                animator.SetBool("isGrounded", false);
            }
            else if (rb.velocity.y < 0)
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
                animator.SetBool("isGrounded", false);
            }
        }
    }

    void Die()
    {
        // 현재 씬을 다시 로드하여 게임 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}