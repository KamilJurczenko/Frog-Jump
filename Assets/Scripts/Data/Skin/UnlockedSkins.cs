using System.Collections.Generic;

[System.Serializable]
public class UnlockedSkins
{
    public List<string> unlockedSkins = new List<string>();

    public UnlockedSkins(List<string> unlockedSkinsList)
    {
        unlockedSkins = unlockedSkinsList;
    }
}
