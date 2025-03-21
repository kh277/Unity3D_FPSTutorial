using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle;       // 시야각
    [SerializeField] private float viewDistance;    // 시야 거리
    [SerializeField] private LayerMask targetMask;  // 타겟 마스크

    private Pig thePig;

    void Start()
    {
        thePig = GetComponent<Pig>();
    }

    void Update()
    {
        View();
    }

    private Vector3 BoundaryAngle(float _angle)
    {
        // 돼지의 Rotation.y 값이 바뀌는 것은 돼지가 필드에서 고개를 돌리는 것과 같음
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if (_targetTf.name == "Player")
            {
                // 플레이어가 돼지를 보는 각도
                Vector3 _direction = (_targetTf.position - transform.position).normalized;
                
                // 돼지의 시야각과 _direction 사이의 각도
                float _angle = Vector3.Angle(_direction, transform.forward);

                if (_angle < viewAngle * 0.5f)
                {
                    // 시야각 내에 있지만 장애물에 의해 못볼수도 있으므로 레이캐스트를 쏴서 확인
                    RaycastHit _hit;
                    if (Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if (_hit.transform.name == "Player")
                        {
                            Debug.Log("플레이어가 돼지 시야 내에 있습니다");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            thePig.Run(_hit.transform.position);
                        }
                    }
                }
            }
        }
    
    }
}
