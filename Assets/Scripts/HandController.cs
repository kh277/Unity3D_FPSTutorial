using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // 현재 장착된 Hand형 타입 무기
    [SerializeField]
    private Hand currentHand;

    // 공격 중
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;


    void Update()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        // 마우스 왼쪽 버튼을 누르면 코루틴 실행
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                // 딜레이를 활용해야 하는데 가장 적합한 것이 Coroutine임
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    // 공격 
    IEnumerator AttackCoroutine()
    {   
        // 마우스 좌클릭을 한 순간 코루틴 실행, 동시에 isAttack이 true가 되면서 중복실행 방지.
        isAttack = true;
        // 공격 애니메이션 실행
        currentHand.anim.SetTrigger("Attack");

        // 딜레이
        yield return new WaitForSeconds(currentHand.attackDelayA);


        // 공격 활성화 시점
        isSwing = true;

        // 물체 충돌 확인
        StartCoroutine(HitCoroutine());
        
        // 공격 적중 확인 Coroutine
        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);

        // 딜레이를 준 후 다시 isAttack false로 변경.
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        // Swing 상태에서 전방에 물체가 있는지 계속해서 확인
        while (isSwing)
        {
            if (CheckObject())
            {
                // 물체와 충돌한 경우 계속해서 충돌하는 것을 방지하기 위해 isSwing을 false로 설정.
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    // 전방에 물체가 있다면 true반환
    private bool CheckObject()
    {
        // transform.forward와 transform.TransformDirection(Vector3.forward)는 같은 의미임
        // Raycast(발사할 위치, 발사할 방향, 부딪힌 물체의 정보를 저장할 인스턴스, 범위)
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }
}
