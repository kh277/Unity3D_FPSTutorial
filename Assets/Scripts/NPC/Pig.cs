using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName;     // 동물의 이름
    [SerializeField] private int hp;                // 동물의 체력
    [SerializeField] private float walkSpeed;       // 걷기 속도

    // 상태 변수
    private bool isAction;          // 행동 실행 중 여부 판별 
    private bool isWalking;         // 걷기 중 여부 판별

    [SerializeField] private float walkTime;        // 걷기 시간
    [SerializeField] private float waitTime;        // 대기 시간
    private float currentTime;

    // 필요한 컴포넌트
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;

    private Vector3 direction;


    void Start()
    {
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotation();
        ElapseTime();
    }

    private void Move()
    {
        if (isWalking)
            rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
    }

    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
                ResetStatus(); // 다음 랜덤 행동 개시
        }
    }

    private void ResetStatus()
    {
        isWalking = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
        int _random = Random.Range(0, 4);       // 대기, 풀뜯기, 두리번, 걷기

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
    }

    private void Wait()
    {
        currentTime = waitTime;
        
        Debug.Log("1");
    }

    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("2");
    }

    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("3");
    }

    private void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        Debug.Log("4");
    }
}
