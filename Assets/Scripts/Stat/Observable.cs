using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observable
{
    private List<IObserver> observers = new List<IObserver>();

    // Observer�� ����ϴ� �޼���
    public void Attach(IObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    // Observer�� �����ϴ� �޼���
    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    // ��� Observer���� ���� ��ȭ�� �˸��� �޼���
    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.Update(this);
        }
    }

    // ���� ������ �߻��ϴ� �޼���
    // ���� ���� ���� �� Notify() ȣ�� �ʿ�
    //public abstract void ChangeState();
}
