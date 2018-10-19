using UnityEngine;


public static class GameStatistics{

    //Statistics
    public static string TotalNumberOfPlaysKey = "TotalNumberOfPlays";
    public static string TotalNumberOfDrivenAttractionsKey = "TotalNumberOfDrivenAttractions";
    public static string TotalNumberOfStoppedAttractionsKey = "TotalNumberOfStoppedAttractions";
    public static string TotalNumberOfVisitorsKey = "TotalNumberOfVisitors";
    public static string TotalNumberOfSatisfiedVisitorsKey = "TotalNumberOfSatisfiedVisitors";

    public static void AddTotalNumberOfPlays()
    {
        PlayerPrefs.SetInt(TotalNumberOfPlaysKey, PlayerPrefs.GetInt(TotalNumberOfPlaysKey) + 1);
    }

    public static int GetTotalNumberOfPlays()
    {
        return PlayerPrefs.GetInt(TotalNumberOfPlaysKey, 0);
    }

    public static void AddTotalNumberOfDrivenAttractions(int _amount)
    {
        PlayerPrefs.SetInt(TotalNumberOfDrivenAttractionsKey, PlayerPrefs.GetInt(TotalNumberOfDrivenAttractionsKey) + _amount);
    }

    public static int GetTotalNumberOfDrivenAttractions()
    {
        return PlayerPrefs.GetInt(TotalNumberOfDrivenAttractionsKey, 0);
    }

    public static void AddTotalNumberOfStoppedAttractions(int _amount)
    {
        PlayerPrefs.SetInt(TotalNumberOfStoppedAttractionsKey, PlayerPrefs.GetInt(TotalNumberOfStoppedAttractionsKey) + _amount);
    }

    public static int GetTotalNumberOfStoppedAttractions()
    {
        return PlayerPrefs.GetInt(TotalNumberOfStoppedAttractionsKey, 0);
    }

    public static void AddTotalNumberOfVisitors(int _amount)
    {
        PlayerPrefs.SetInt(TotalNumberOfVisitorsKey, PlayerPrefs.GetInt(TotalNumberOfVisitorsKey) + _amount);
    }

    public static int GetTotalNumberOfVisitors()
    {
        return PlayerPrefs.GetInt(TotalNumberOfVisitorsKey, 0);
    }

    public static void AddTotalNumberOfSatisfiedVisitors(int _amount)
    {
        PlayerPrefs.SetInt(TotalNumberOfSatisfiedVisitorsKey, PlayerPrefs.GetInt(TotalNumberOfSatisfiedVisitorsKey) + _amount);
    }

    public static int GetTotalNumberOfSatisfiedVisitors()
    {
        return PlayerPrefs.GetInt(TotalNumberOfSatisfiedVisitorsKey, 0);
    }
}
