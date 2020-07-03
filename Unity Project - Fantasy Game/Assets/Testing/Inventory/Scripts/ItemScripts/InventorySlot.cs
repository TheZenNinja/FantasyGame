using UnityEngine;
using System.Collections;

[System.Serializable]
public struct InventorySlot
{
    public Item item;
    public int count;

    public InventorySlot(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    /*public bool hasItem()
    {
        return item != null;
    }
    public void SetData(ItemSlot slot)
    {
        item = slot.GetItem();
        count = slot.GetCount();
    }

    public static InventorySlot operator +(InventorySlot a, int b)
    {
        a.count += b;
        return a;
    }
    public static InventorySlot operator -(InventorySlot a, int b)
    {
        a.count -= b;
        if (a.count <= 0)
            a.item = null;
        return a;

    }
    public static InventorySlot operator ++(InventorySlot a)
    {
        a.count++;
        if (a.count > 0)
        {
            return a;
        }
        else
            return null;
    }
    public static InventorySlot operator --(InventorySlot a)
    {
        a.count--;
        if (a.count <= 0)
            a.item = null;
        return a;
    }
*/
}
/*[CustomEditor(typeof(ItemInventoryCount))]
public class ItemInvGUI : Editor
{
    SerializedProperty item;
    SerializedProperty count;
    private void OnEnable()
    {
       item = serializedObject.FindProperty("item");
        count = serializedObject.FindProperty("count");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(count, GUILayout.Width(10));
        EditorGUILayout.PropertyField(item, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }
}*/
