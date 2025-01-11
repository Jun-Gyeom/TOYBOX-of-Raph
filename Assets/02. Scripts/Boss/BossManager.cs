using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : Singleton<BossManager>
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

    [Header("Dialogue Systems")]
    [SerializeField] private List<DialogSystem> dialogueSystems;


    protected override void Awake()
    {
        StartPhase(phases[CurrentPhase]);
    }

    // ������ ���� �޼��� 
    public void StartPhase(Phase phase)
    {
        // �Ǵ� ���� 
        GameManager.Instance.Player.PlayerAbility = phase.ability;

        // ���� �б� ���� 
        StartCoroutine(ReadPatterns(phase.patterns));
    }

    public void NextPhase()
    {
        if (CurrentPhase < phases.Count - 1)
        {
            CurrentPhase++;
            StartPhase(phases[CurrentPhase]);
        }
    }

    // ���� �а� ���� �޼��� 
    private IEnumerator ReadPatterns(List<Pattern> patterns)
    {
        // ���� �������� ��� ���� �б�
        foreach (Pattern pattern in patterns)
        {
            // ���� ������ ��� Ÿ�� ���� ������ �б� 
            foreach (TileSetData tileset in pattern.tilesets)
            {
                // ��ٿ� ��� 
                yield return new WaitForSeconds(tileset.coolDownTime);

                // ���� ����
                if (tileset.useTrail)
                {
                    for (int i = 0; i < tileset.trails.Count; i++)
                    {
                        CreateThomas(tileset.trails[i]);
                    }
                }

                // Ÿ�� ���� ��ȯ
                if (tileset.useTile)
                {
                    TileManager.Instance.SetTileType(tileset.tilePositions, tileset.type, tileset.startupTime, tileset.holdingTime);
                }

                // ���� �ִϸ��̼� ���
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

            // ���� ���� ���� �ð� 
            yield return new WaitForSeconds(pattern.coolDownTime);
        }

        // if ��� ����� �������� Ȯ�� --> �׷��ٸ� ���� �Ŵ����� ���� Ŭ���� �Լ� ȣ�� 
        if (CurrentPhase >= phases.Count - 1)
        {
            // TODO - ���� Ŭ���� 
        }

        // ���� �߰� ��ȭ �Լ� ȣ�� 
        if (CurrentPhase < dialogueSystems.Count - 1)
        {
            dialogueSystems[CurrentPhase].StartDialogue();
        }
    }

    // ���� ���� �޼��� 
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
