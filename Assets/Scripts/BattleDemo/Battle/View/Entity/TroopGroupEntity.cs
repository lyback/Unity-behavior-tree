
using Battle.Logic;
using Neatly.Timer;
using UnityEngine;

//部队实体基类
public class TroopGroupEntity : UnitEntityBase<TroopData>
{

    public override void Init(TroopData _data)
    {
        base.Init(_data);
    }
    #region 显示
    protected bool m_IsInit;

    protected void Show(bool value)
    {
        gameObject.SetActive(value);
    }

    #endregion

    #region 移动
    protected float m_LastPosX;
    protected float m_LastPosY;
    protected float m_CurPosX;
    protected float m_CurPosY;
    protected float m_TarPosX;
    protected float m_TarPosY;
    protected float m_DeltaTime;
    protected bool m_Move;

    protected void Start()
    {
        NeatlyTimer.AddFrame(this, FrameUpdate);
    }

    void FrameUpdate(float deltaTime)
    {
        m_DeltaTime = m_DeltaTime + deltaTime;
        DoMove();
        UnitUpdate();
    }

    protected virtual void UnitUpdate() { }

    protected void DoMove()
    {
        float radio = m_DeltaTime / MainBattleManager.Instance.m_LogicTime;
        if (m_Move)
        {
            m_CurPosX = Mathf.Lerp(m_LastPosX, m_TarPosX, radio);
            m_CurPosY = Mathf.Lerp(m_LastPosY, m_TarPosY, radio);
            SetLocalPosition(m_CurPosX, m_CurPosY);
            if (radio >= 1)
            {
                m_Move = false;
            }
        }
    }

    protected void OnCorrectMove()
    {
        m_CurPosX = m_TarPosX;
        m_CurPosY = m_TarPosY;
        m_LastPosX = m_TarPosX;
        m_LastPosY = m_TarPosY;
        SetLocalPosition(m_CurPosX, m_CurPosY);
        m_Move = false;
    }

    protected void SetTweenPosition(float x, float y)
    {
        m_DeltaTime = 0;
        m_LastPosX = m_CurPosX;
        m_LastPosY = m_CurPosY;
        m_TarPosX = x;
        m_TarPosY = y;
        m_Move = true;
    }
    #endregion

    #region Interface

    protected virtual void InitPosition(float posX, float posY)
    {
        m_TarPosX = posX;
        m_TarPosY = posY;
        OnCorrectMove();
    }
    #endregion
}
