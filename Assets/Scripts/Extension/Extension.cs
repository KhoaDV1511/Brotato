using UnityEngine;

public static class Extension
{
    public static void Show(this Component obj)
    {
        obj.gameObject.SetActive(true);
    }
    public static void Hide(this Component obj)
    {
        obj.gameObject.SetActive(false);
    }
}