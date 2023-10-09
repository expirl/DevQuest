using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum LaserType
{
    DamageLaser,
    DebuffLaser,
    None
}

public class ShootRailGun : MonoBehaviour
{
    public GameObject[] laserPrefab; // 레이저 Prefab

    [SerializeField] private GameObject laserSpawnPoint; // 레이저 발사 위치
    [SerializeField] private PlayerState playerState;


    private void Start()
    {
        playerState = transform.parent.GetComponent<PlayerState>();
    }

    void Update()
    {
     
        // 마우스 왼쪽 클릭 시 그리고 공격 부여 물약을 획득 시에 데미지 부여 레이저 발사
        if (Input.GetMouseButtonDown(0) && playerState.IsAttack == true)
        {
            // 레이저 생성
            // ShootLaser();
             CreateLaser(LaserType.DamageLaser); // == laserPrefab[0]
        }

        // 마우스 오른쪽 클릭 시 속도 너프 레이저 발사
        if(Input.GetMouseButtonDown(1))
        {
            CreateLaser(LaserType.DebuffLaser); // == laserPrefab[1]
        }
    }



    // 레이캐스트 충돌 체크
    void ShootLaser()
    {
        // 메인 카메라에서 마우스 포인터 방향으로 레이 발사
        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Ray ray = new Ray(laserSpawnPoint.transform.position, laserSpawnPoint.transform.forward);
        RaycastHit hit;

        int enemyLayMask = LayerMask.GetMask("Enemy");

        // 레이케스트 충돌 확인
        if (Physics.Raycast(ray, out hit, 1000f, enemyLayMask))
        {
            Debug.Log("적 감지");
        }
    }

   // Projectile(발사체) 충돌 체크
    private void CreateLaser(LaserType laserType)
    {
        StartCoroutine(ShowLaser(laserSpawnPoint.transform.position, laserType));
    }


    // 레이저 생성 코루틴
    // 데미지 레이저 - laserPrefab[0] , 속도 디버프 레이저 - laserPrefab[1]
    IEnumerator ShowLaser(Vector3 startPosition, LaserType laserType)
    {
        // 레이저 생성 및 설정
        GameObject laser = Instantiate(laserPrefab[(int)laserType], startPosition, Quaternion.Euler(90, 0, 0));
        laser.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);

        // 일정 시간 후 레이저 제거
        yield return new WaitForSeconds(1f);

        Destroy(laser);
    }
}
