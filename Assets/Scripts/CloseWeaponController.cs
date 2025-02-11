using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상 클래스
public abstract class CloseWeaponController : MonoBehaviour
{
    // 현재 장착된 Hand형 타입 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // 공격 중
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    protected void TryAttack()
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
    protected IEnumerator AttackCoroutine()
    {   
        // 마우스 좌클릭을 한 순간 코루틴 실행, 동시에 isAttack이 true가 되면서 중복실행 방지.
        isAttack = true;
        // 공격 애니메이션 실행
        currentCloseWeapon.anim.SetTrigger("Attack");

        // 딜레이
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);


        // 공격 활성화 시점
        isSwing = true;

        // 물체 충돌 확인
        StartCoroutine(HitCoroutine());
        
        // 공격 적중 확인 Coroutine
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);

        // 딜레이를 준 후 다시 isAttack false로 변경.
        isAttack = false;
    }

    // 추상 코루틴
    protected abstract IEnumerator HitCoroutine();

    // 전방에 물체가 있다면 true반환
    protected bool CheckObject()
    {
        // transform.forward와 transform.TransformDirection(Vector3.forward)는 같은 의미임
        // Raycast(발사할 위치, 발사할 방향, 부딪힌 물체의 정보를 저장할 인스턴스, 범위)
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }

    // 완성 함수이지만 추가 편집 가능
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}

