using UnityEngine;


public static class ItemManager
{
    public static void SelectData(Item.ItemType itemType, ref GameObject gameObject, ref int stackLimit, ref string itemName)
    {
        switch (itemType)
        {
            case Item.ItemType.Sword:
                gameObject = GetGameObject("Sword");
                stackLimit = 32;
                itemName = "Sword";
                break;

            case Item.ItemType.Axe:
                gameObject = GetGameObject("Axe");
                stackLimit = 32;
                itemName = "Axe";
                break;

            case Item.ItemType.Medkit:
                gameObject = GetGameObject("Medkit");
                stackLimit = 32;
                itemName = "Medkit";
                Debug.Log(gameObject.name);
                break;
        }
    }

    private static GameObject GetGameObject(string name)
    {
        for (int i = 0; i < GameObject.Find("hand.r").transform.childCount; i++)
        {
            if (GameObject.Find("hand.r").transform.GetChild(i).gameObject.name == name)
            {
                return GameObject.Find("hand.r").transform.GetChild(i).gameObject;
            }
        }
        return null;
    }
}
