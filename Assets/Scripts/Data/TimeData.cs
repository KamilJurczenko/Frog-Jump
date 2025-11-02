using System;

[System.Serializable]
public class TimeData
{
    public DateTime dateTimeOnExit;

    public TimeData(DateTime dateTime)
    {
        this.dateTimeOnExit = dateTime;
    }
}
