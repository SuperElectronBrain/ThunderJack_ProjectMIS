using UnityEngine;

public abstract class State<T> : MonoBehaviour where T : class
{
    protected float elapsedTime;
    public abstract void Enter(T entity);
    public abstract void Execute(T entity);
    public abstract void Exit(T entity);
    public abstract void OnTransition(T entity);
}
