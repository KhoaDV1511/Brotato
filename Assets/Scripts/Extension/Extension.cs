using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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
    
    public static Vector3 MapLimited(this Vector3 posMove)
    {
        posMove.x = posMove.x > PotatoKey.MAP_MAX_X ? PotatoKey.MAP_MAX_X : posMove.x;
        posMove.x = posMove.x < PotatoKey.MAP_MIN_X ? PotatoKey.MAP_MIN_X : posMove.x;
        posMove.y = posMove.y > PotatoKey.MAP_MAX_Y ? PotatoKey.MAP_MAX_Y : posMove.y;
        posMove.y = posMove.y < PotatoKey.MAP_MIN_Y ? PotatoKey.MAP_MIN_Y : posMove.y;
        posMove.z = 0;
        return posMove;
    }
    public static T Cast<T>(this MonoBehaviour mono) where T : class
    {
        var t = mono as T;
        return t;
    }
    public static void DisableChildren(this Transform t)
    {
        foreach (var child in t)
        {
            ((Transform) child).Hide();
        }
    }
    
    public static void DestroyChildren(this Transform t)
    {
        foreach (var child in t)
        {
            Object.Destroy(((Transform) child).gameObject);
        }
    }
    
    public static void DestroyChildrenImmediate(this Transform t)
    {
        foreach (var child in t)
        {
            Object.DestroyImmediate(((Transform) child).gameObject);
        }
    }
}