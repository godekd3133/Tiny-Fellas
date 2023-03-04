using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


[RequireComponent(typeof(NetworkObject))]
public class TroopAdmin : NetworkBehaviour
{
    [ShowInInspector, ReadOnly] public List<Minion> minions { get; private set; }
    [ShowInInspector, ReadOnly] public Minion leaderMinion { get; private set; }


    [SerializeField] private bool isPlayer;
    [SerializeField] private bool isOwner;

    public bool IsPlayer { get { return isPlayer; } }
    public bool IsOwner { get { return isOwner; } }


    public UnityEvent<Minion> onPostMinionAdded;

    void Awake()
    {
        minions = new List<Minion>();
        leaderMinion = null;
    }

    void Start()
    {
        if (isPlayer)
            CameraManager.Instance.followingTarget = leaderMinion.transform;
    }

    private void Update()
    {
    }

    /// <summary>
    /// 리더 미니언 갱신
    /// </summary>
    /// <returns> Selected leader minion</returns>
    public Minion UpdateLeaderMinion()
    {
        //TODO 리더 미니언을 셀렉하는 방식을 고안한 후에 변경해야함.
        leaderMinion = minions.First();
        return leaderMinion;
    }

    /// <summary>
    /// 미니언 생성, 초기화 
    /// </summary>
    /// <returns> Generated minion</returns>
    public Minion AddMinion(Minion minion)
    {
        onPostMinionAdded.Invoke(minion);

        return minion;
    }


}
