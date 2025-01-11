using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : Singleton<BossManager>
{
    public int CurrentPhase { get; private set; } = 0;
    [SerializeField] private List<Pattern> patterns;
    [SerializeField] private Animator bossAnim;
    private float coolDownTime;

    [Header("Thomas Prefabs")]
    [SerializeField] private GameObject LeftThomasPrefab;
    [SerializeField] private GameObject RightThomasPrefab;
    [SerializeField] private GameObject DownThomasPrefab;


    protected override void Awake()
    {
        StartPattern(patterns[CurrentPhase]);
    }

    // 패턴(페이지) 시작 메서드 
    public void StartPattern(Pattern pattern)
    {
        coolDownTime = pattern.coolDownTime;

        // 패턴 읽기 시작 
        StartCoroutine(ReadTileSets(pattern.tilesets));
    }

    // 패턴 읽고 실행 메서드 
    private IEnumerator ReadTileSets(List<TileSetData> tilesets)
    {
        foreach (TileSetData tileset in tilesets)
        {
            // 쿨다운 대기 
            yield return new WaitForSeconds(tileset.coolDownTime);

            // 기차 출현
            if (tileset.useTrail)
            {
                for (int i = 0; i < tileset.trails.Count; i++)
                {
                    CreateThomas(tileset.trails[i]);
                }
            }

            // 타일 상태 변환
            if (tileset.useTile)
            {
                TileManager.Instance.SetTileType(tileset.tilePositions, tileset.type, tileset.startupTime, tileset.holdingTime);
            }

            // 보스 애니메이션 재생
            if (tileset.playAttackAnim)
            {
                bossAnim.Play("Attack");
            }
        }

        // 페이즈 쉬는 시간 
        yield return new WaitForSeconds(coolDownTime);

        // if 모든 페이즈가 끝났는지 확인 --> 그렇다면 게임 매니저의 게임 클리어 함수 호출 

        // 보스 중간 대화 함수 호출 


        // --- 삭제하거나 함수로 빼서 사용 
        //// 다음 페이즈로 이동
        //if (CurrentPhase < patterns.Count - 1)
        //{
        //    CurrentPhase++;
        //    StartPattern(patterns[CurrentPhase]);
        //}
    }

    // 광차 생성 메서드 
    public void CreateThomas(TrailData traildata)
    {
        Trail trail = null;
        Vector2 TilePos = Vector2.zero;
        switch (traildata.hv)
        {
            case HV.LEFT:
                TilePos = new Vector2(0,traildata.pos.y);
                break;
            case HV.RIGHT:
                TilePos = new Vector2(9, traildata.pos.y);
                break;
            case HV.UP:
                TilePos = new Vector2(traildata.pos.x, 4);
                break;
            case HV.DOWN:
                TilePos = new Vector2(traildata.pos.x, 0);
                break;
            default:
                break;
        }
        Vector3 ObjectPosition = TileManager.Instance.GetTileObejctPosition(TilePos);

        switch (traildata.hv)
        {
            case HV.LEFT:
                Instantiate(LeftThomasPrefab, ObjectPosition, Quaternion.identity).TryGetComponent(out trail);
                break;
            case HV.RIGHT:
                Instantiate(RightThomasPrefab, ObjectPosition, Quaternion.identity).TryGetComponent(out trail);
                break;
            case HV.DOWN:
                Instantiate(DownThomasPrefab, ObjectPosition, Quaternion.identity).TryGetComponent(out trail);
                break;
        }

        // Is Faster
        if (traildata.isFast) trail?.SetFastAnim();

        trail?.Shot(traildata.speed, traildata.hv,traildata.isFast);
    }
}
