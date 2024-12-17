using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    // 필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    // 필요하면 HUD 호출
    [SerializeField]
    private GameObject go_BulletHUD;

    // 총알 개수 반영
    [SerializeField]
    private TextMeshProUGUI[] text_Bullet;

    void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }
}
