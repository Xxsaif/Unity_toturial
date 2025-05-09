using UnityEngine;


public static class ItemManager
{
    public static void SelectData(Item.ItemType itemType, ref GameObject gameObject, ref int stackLimit, ref string itemName)
    {
        switch (itemType)
        {
            case Item.ItemType.Sword:
                gameObject = GetGameObject("Sword");
                stackLimit = 1;
                itemName = "Sword";
                break;

            case Item.ItemType.Axe:
                gameObject = GetGameObject("Axe");
                stackLimit = 1;
                itemName = "Axe";
                break;

            case Item.ItemType.Medkit:
                gameObject = GetGameObject("Medkit_parent");
                stackLimit = 4;
                itemName = "Medkit";
                break;

            case Item.ItemType.Rock:
                gameObject = GetGameObject("Rock");
                stackLimit = 16;
                itemName = "Rock";
                break;

            case Item.ItemType.Stick:
                gameObject = GetGameObject("Stick");
                stackLimit = 16;
                itemName = "Stick";
                break;

            case Item.ItemType.String:
                gameObject = GetGameObject("String");
                stackLimit = 16;
                itemName = "String";
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
