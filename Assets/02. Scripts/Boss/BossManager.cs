using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public int CurrentPhase { get; private set; } = 0;
    [SerializeField] private List<Phase> phases;
    [SerializeField] private Animator bossAnim;

    [Header("Thomas Prefabs")]
    [SerializeField] private GameObject LeftThomasPrefab;
    [SerializeField] private GameObject RightThomasPrefab;
    [SerializeField] private GameObject DownThomasPrefab;

    [Header("Warning Line")]
    [SerializeField] private TrailLine LinePrefab;

    [Header("Dialogue System")]
    [SerializeField] public List<DialogSystem> DialogSystems;

    private void Awake()
    {
        DialogSystems[0].StartDialogue();
    }

    // 페이즈 시작 메서드 
    public void StartPhase(int phaseIndex)
    {
        // 권능 적용 
        GameManager.Instance.Player.PlayerAbility = phases[phaseIndex].ability;

        // 패턴 읽기 시작 
        StartCoroutine(ReadPatterns(phases[phaseIndex].patterns));
    }

    public void NextPhase()
    {
        if (CurrentPhase < phases.Count - 1)
        {
            StartPhase(CurrentPhase);
        }
    }

    // 패턴 읽고 실행 메서드 
    private IEnumerator ReadPatterns(List<Pattern> patterns)
    {
        // 현제 페이즈의 모든 패턴 읽기
        foreach (Pattern pattern in patterns)
        {
            // 현재 패턴의 모든 타일 변경 데이터 읽기 
            foreach (TileSetData tileset in pattern.tilesets)
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
                    int index = 0;
                    if(tileset.useTile)
                    {
                        switch (tileset.type)
                        {
                            case TileType.VOID:
                                index = 0;
                                break;
                            case TileType.SPIKE:
                                index = 1;
                                break;
                            case TileType.FALL:
                                index = 2;
                                break;
                            case TileType.TADDYBEAR:
                                index = 3; 
                                break;
                            default:
                                break;
                        }
                    }
                    else if(tileset.useTrail)
                    {
                        index = 0;
                    }
                    bossAnim.SetFloat("INDEX",index);
                    bossAnim.SetTrigger("ATTACK");
                }
            }

            // 패턴 이후 쉬는 시간 
            yield return new WaitForSeconds(pattern.coolDownTime);
        }

        // if 모든 페이즈가 끝났는지 확인 --> 그렇다면 게임 매니저의 게임 클리어 함수 호출 
        if (CurrentPhase >= phases.Count - 1)
        {
            // TODO - 게임 클리어 
            // 1. 클리어 대화 
            DialogSystems[CurrentPhase + 1].StartDialogue();

            // 2. 클리어 화면 


            yield break;
        }

        // 보스 중간 대화 함수 호출 
        if (CurrentPhase < phases.Count)
        {
            DialogSystems[CurrentPhase + 1].StartDialogue();
            CurrentPhase++;
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
                TilePos = new Vector2(9,traildata.pos.y);
                break;
            case HV.RIGHT:
                TilePos = new Vector2(0, traildata.pos.y);
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

        TrailLine Tline = Instantiate(LinePrefab);
        Tline.Init(ObjectPosition, traildata.hv);

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
