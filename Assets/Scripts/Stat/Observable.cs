using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observable
{
    private List<IObserver> observers = new List<IObserver>();

    // Observer를 등록하는 메서드
    public void Attach(IObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    // Observer를 제거하는 메서드
    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    // 모든 Observer에게 상태 변화를 알리는 메서드
    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.Update(this);
        }
    }

    // 상태 변경이 발생하는 메서드
    // 상태 변경 로직 후 Notify() 호출 필요
    //public abstract void ChangeState();
}
