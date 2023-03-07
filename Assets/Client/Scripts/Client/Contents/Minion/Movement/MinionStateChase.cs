
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MinionStateChase : MinionState
{
    public MinionStateChase(Minion owner) : base(owner)
    {
    }

    public override MinionState CheckTransition()
    {
        if (MyInputManager.DragAxis.magnitude > 0)
            return new MinionStateMove(Owner);


        bool checkEnemyDetection = MyTroopAdmin.RecognizedEnemyMinionList.Count > 0;
        if (checkEnemyDetection == false)
            return new MinionStateIdle(Owner);

        if (Owner.chaseTarget != null)
            if (Owner.Stat.MyAttackBehaviour.IsInAttackRagne(Owner.chaseTarget.transform))
                return new MinionStateBattle(Owner);

        return this;
    }

    public override async UniTask EnterState()
    {


    }

    public override async UniTask ExitState()
    {
    }

    public override async UniTask UpdateState(CancellationToken cancellationToken)
    {
        while (true)
        {

            if (cancellationToken.IsCancellationRequested)
            {
                enabled = false;
                break;
            }            //owner.Stat.MyBattleAbility.StatMap[EStatName.ATTACK_RAGNE].CurrentValue ?? 

            foreach (var stat in Owner.Stat.MyBattleAbility.StatMap)
            {
                Debug.Log(stat.Key +" , "+stat.Value);
            }
            Owner.agent.stoppingDistance = Owner.Stat.MyBattleAbility[EStatName.ATTACK_RAGNE].CurrentValue - 0.25f;
            // 찾은 적이 있을경우 해당 적을향해, 없을경우 리더 미니언을 쫒아가면서 적을 찾음.
            if (MyTroopAdmin.RecognizedEnemyMinionList.Count > 0)
            {
                Owner.agent.SetDestination(MyTroopAdmin.RecognizedEnemyMinionList[0].transform.position);
                Owner.chaseTarget = MyTroopAdmin.RecognizedEnemyMinionList[0];
            }
            else
            {
                Owner.agent.SetDestination(MyTroopAdmin.RecognizedEnemyMinionList[0].transform.position);
                Owner.chaseTarget = MyTroopAdmin.RecognizedEnemyMinionList[0];
            }

            await UniTask.NextFrame();
        }
    }
}
