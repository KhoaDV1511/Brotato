using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
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

    private static IEnumerator IWaitTimeout(Action cb, float delay)
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
            ((Transform)child).Hide();
        }
    }

    public static void DestroyChildren(this Transform t)
    {
        foreach (var child in t)
        {
            Object.Destroy(((Transform)child).gameObject);
        }
    }

    public static void DestroyChildrenImmediate(this Transform t)
    {
        foreach (var child in t)
        {
            Object.DestroyImmediate(((Transform)child).gameObject);
        }
    }

    public static Vector3 FindVectorADistanceVecTorB(this Vector3 direction, Vector2 origin, float distance)
    {
        var a = (Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));;
        var b = (-2 * direction.x * origin.x - 2 * direction.y * origin.y);
        var c = (Mathf.Pow(origin.x, 2) + Mathf.Pow(origin.x, 2) - Mathf.Pow(distance, 2));
        
        if (a == 0)
        {
            if (b == 0)
            {
                //Debug.Log("Phuong trinh vo nghiem!");
            }
            else
            {
                //Debug.Log($"Phuong trinh co mot nghiem: x = {(-c / b)}");
            }
            return direction;
        }
        // tinh delta
        float delta = b * b - 4 * a * c;
        float x1 = 0;
        float x2 = 0;
        // tinh nghiem
        if (delta > 0)
        {
            x1 = (float)((-b + Math.Sqrt(delta)) / (2 * a));
            x2 = (float)((-b - Math.Sqrt(delta)) / (2 * a));
            //Debug.Log($"Phuong trinh co 2 nghiem la: x1 = {x1} va x2 = {x2}");
        }
        else if (delta == 0)
        {
            x1 = (-b / (2 * a));
            //Debug.Log($"Phong trinh co nghiem kep: x1 = x2 = {x1}");
        }
        else
        {
            //Debug.Log("Phuong trinh vo nghiem delta > 0!");
        }

        //Debug.Log($"distance {Vector3.Distance(direction * value, origin)}");
        return direction * x1;
    }
    
    public static bool ValueInRange(this float value, float min, float max)
    {
        return value >= min && value < max;
    }
    
    private static SpriteAtlas _sprWeapon;
    private static SpriteAtlas SprWeapon
    {
        get
        {
            if (_sprWeapon == null) _sprWeapon = Resources.Load<SpriteAtlas>("SpriteAtlas/WeaponUI");
            return _sprWeapon;
        }
    }
    public static Sprite GetSpriteWeapon(this Sprite spr, int id)
    {
        return SprWeapon.GetSprite($"Weapon_{id}");
    }
}