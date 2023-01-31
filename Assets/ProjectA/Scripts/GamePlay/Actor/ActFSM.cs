using System.Collections;
using UnityEngine;

public class ActFSM : MonoBehaviour
{
    public class Settings
    {
        // FSM 머신을 사용하는 객체의 상태
        public eACT_FSM State;
        
        // FSM 머신이 다음 동작할 때 넘어가는 시간
        public float ThinkTime;
    }
    public Settings FSM = new Settings() { State = eACT_FSM.Default , ThinkTime = 0 };

    public eACT_FSM FS => FSM.State;

    /// <summary>
    /// FSM 시작 점
    /// </summary>
    public void OnFSM()
    {
        StartCoroutine(FSM.State.ToString());
    }

    /// <summary>
    /// 상태 교체하는 함수
    /// </summary>
    public void OnChangeState()
    {
        if (FSM.State != eACT_FSM.Dead)
            StartCoroutine(FSM.State.ToString());
        else
            OnDead();
    }

    public virtual void OnIdle()
    {
    }

    public virtual void OnAttack()
    {
    }

    public virtual void OnDefense()
    {
    }

    public virtual void OnDead()
    {
    }

    private IEnumerator Idle()
    {
        while (FS == eACT_FSM.Idle)
        {
            OnIdle();
            yield return new WaitForSeconds(FSM.ThinkTime);
        }
        OnChangeState();
    }

    private IEnumerator Attack()
    {
        while (FS == eACT_FSM.Attack)
        {
            OnAttack();
            yield return new WaitForSeconds(FSM.ThinkTime);
        }
        OnChangeState();
    }

    private IEnumerator Defense()
    {
        while (FS == eACT_FSM.Defense)
        {
            OnDefense();
            yield return new WaitForSeconds(FSM.ThinkTime);
        }
        OnChangeState();
    }
}
