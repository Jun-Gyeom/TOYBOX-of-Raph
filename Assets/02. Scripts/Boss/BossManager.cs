using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : Singleton<BossManager>
{
    [SerializeField] private List<Pattern> patterns;
    [SerializeField] private int pageNumber;
    [SerializeField] private Animator bossAnim;

    [Header("Thomas Prefabs")]
    [SerializeField] private GameObject HorizontalThomasPrefab;
    [SerializeField] private GameObject DownThomasPrefab;


    protected override void Awake()
    {
        StartPattern(patterns[0]);
    }

    // 패턴(페이지) 시작 메서드 
    public void StartPattern(Pattern pattern)
    {
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
            case HV.RIGHT:
                Instantiate(HorizontalThomasPrefab, ObjectPosition, Quaternion.identity).TryGetComponent(out trail);
                break;
            case HV.DOWN:
                Instantiate(DownThomasPrefab, ObjectPosition, Quaternion.identity).TryGetComponent(out trail);
                break;
        }
        trail?.Shot(traildata.speed, traildata.hv,traildata.isFast);
    }
}
