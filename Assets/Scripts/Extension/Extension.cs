using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class Extension
{
    public static void Hide(this GameObject obj)
    {
        obj.SetActive(false);
    }

    public static void Hide(this Component component)
    {
        component.gameObject.SetActive(false);
    }

    public static void Show(this GameObject obj)
    {
        obj.SetActive(true);
    }

    public static void Show(this Component o)
    {
        o.gameObject.SetActive(true);
    }
    public static void ChangeAlpha(this SpriteRenderer s, float f)
    {
        var c = s.color;
        c.a = f;
        s.color = c;
    }
    
    public static Coroutine WaitNewFrame(this MonoBehaviour obj, Action cb)
    {
        if (obj && obj.gameObject.activeInHierarchy)
            return obj.StartCoroutine(IWaitNewFrame(cb));
        return null;
    }
    public static Coroutine WaitEndFrame(this MonoBehaviour obj, Action cb)
    {
        if (obj.gameObject.activeInHierarchy)
            return obj.StartCoroutine(IWaitEndFrame(cb));
        return null;
    }

    public static Coroutine WaitTimeout(this MonoBehaviour obj, Action cb, float delay)
    {
        if (obj != null && obj.gameObject.activeInHierarchy)
            return obj.StartCoroutine(IWaitTimeout(cb, delay));
        return null;
    }
    
    private static IEnumerator IWaitNewFrame(Action cb)
    {
        yield return null;
        cb.Invoke();
    }
    
    private static IEnumerator IWaitEndFrame(Action cb)
    {
        yield return new WaitForEndOfFrame();
        cb.Invoke();
    }
    private static IEnumerator IWaitTimeout(Action cb,float delay)
    {
        yield return new WaitForSeconds(delay);
        cb.Invoke();
    }
}