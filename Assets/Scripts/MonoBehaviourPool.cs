using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourPool<T> where T : MonoBehaviour
{
    private T original;
    private List<T> availableBullets;
    private List<T> inUseBullets;

    public MonoBehaviourPool(T original, int size = 20)
    {
        this.original = original;
        availableBullets = new List<T>();
        inUseBullets = new List<T>();

        for (int i = 0; i < size; i++)
        {
            GameObject currentCopy = GameObject.Instantiate(original.gameObject);
            currentCopy.SetActive(false);
            availableBullets.Add(currentCopy.GetComponent<T>());
        }
    }

    public T GetObject()
    {
        int lastIndex = availableBullets.Count - 1;
        if (lastIndex < 0)
        {
            Debug.Log($"Added new object to the pool of type {typeof(T)}");
            GameObject currentCopy = GameObject.Instantiate(original.gameObject);
            currentCopy.SetActive(false);
            availableBullets.Add(currentCopy.GetComponent<T>());
            lastIndex = 0;
        }
        T lastBullet = availableBullets[lastIndex];
        inUseBullets.Add(lastBullet);
        availableBullets.RemoveAt(lastIndex);
        return lastBullet;
    }

    public void ReturnObject(T obj)
    {
        availableBullets.Add(obj);
        inUseBullets.Remove(obj);
    }
}
