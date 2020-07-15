using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

internal class ObjectPool<T> where T:new()
{
    private readonly Stack<T> m_stack = new Stack<T>();
    private readonly UnityAction<T> m_actionOnGet;
    private readonly UnityAction<T> m_actionOnRelease;

    public int countAll { get; private set; }
    public int countInactive { get { return m_stack.Count; } }
    private int countActive { get { return countAll - countInactive; } }

    public ObjectPool(UnityAction<T> actionOnGet,UnityAction<T> actionOnRelease)
    {
        m_actionOnGet = actionOnGet;
        m_actionOnRelease = actionOnRelease;
    }
    public T Get()
    {
        T element;
        if (m_stack.Count == 0)
        {
            element = new T();
            countAll++;
        }
        else
        {
            element = m_stack.Pop();
        }
        if (m_actionOnGet != null) m_actionOnGet(element);
        return element;
    }
    public void Release(T element)
    {
        if (m_stack.Count > 0 && ReferenceEquals(m_stack.Peek(), element))
            Debug.LogError("Internal error.Trying to destroy object that is already released to pool.");
        if (m_actionOnRelease != null)
            m_actionOnRelease(element);
        m_stack.Push(element);
    }
}
