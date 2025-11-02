using System.Collections.Generic;

[System.Serializable]
public class Level 
{
    public float bestTime;

    public ushort stars;

    public List<int> flySmallIdList = new List<int>();

    public Level(float bestTime, ushort stars,List<int> flySmallIdList)
    {
        this.bestTime = bestTime;
        this.stars = stars;
        this.flySmallIdList = flySmallIdList;
    }

}

[System.Serializable]
public class CompletedLevels
{
    public List<Level> completedLevelList = new List<Level>();
    public CompletedLevels(List<Level> levelList)
    {
        completedLevelList = levelList;
    }
}
