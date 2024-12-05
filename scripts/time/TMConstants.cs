using Godot;
using System;
using System.Collections.Generic;

public static class TMConstants {
  public enum Month {
    January = 1,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December
  }

  public enum Season {
    Summer,
    Autumn,
    Winter,
    Spring
  }

  public enum Hemisphere {
    North,
    South
  }

  public static readonly Dictionary<Month, string> MonthNames = new() {
    {Month.January, "January"},
    {Month.February, "February"},
    {Month.March, "March"},
    {Month.April, "April"},
    {Month.May, "May"},
    {Month.June, "June"},
    {Month.July, "July"},
    {Month.August, "August"},
    {Month.September, "September"},
    {Month.October, "October"},
    {Month.November, "November"},
    {Month.December, "December"},
  };

  public static readonly Dictionary<Season, string> SeasonNames = new() {
    {Season.Summer, "Summer"},
    {Season.Autumn, "Autumn"},
    {Season.Winter, "Winter"},
    {Season.Spring, "Spring"},
  };

  public static readonly Dictionary<Month, ulong> MonthLengths = new(){
    {Month.January, 31ul},
    {Month.February, 28ul},
    {Month.March, 31ul},
    {Month.April, 30ul},
    {Month.May, 31ul},
    {Month.June, 30ul},
    {Month.July, 31ul},
    {Month.August, 31ul},
    {Month.September, 30ul},
    {Month.October, 31ul},
    {Month.November, 30ul},
    {Month.December, 31ul},
  };

  public static readonly Dictionary<Month, Season[]> MonthToSeason = new() {
    {Month.January, new[]{Season.Winter, Season.Summer}},
    {Month.February, new[]{Season.Winter, Season.Summer}},
    {Month.March, new[]{Season.Spring, Season.Autumn}},
    {Month.April, new[]{Season.Spring, Season.Autumn}},
    {Month.May, new[]{Season.Spring, Season.Autumn}},
    {Month.June, new[]{Season.Summer, Season.Winter}},
    {Month.July, new[]{Season.Summer, Season.Winter}},
    {Month.August, new[]{Season.Summer, Season.Winter}},
    {Month.September, new[]{Season.Autumn, Season.Spring}},
    {Month.October, new[]{Season.Autumn, Season.Spring}},
    {Month.November, new[]{Season.Autumn, Season.Spring}},
    {Month.December, new[]{Season.Winter, Season.Summer}},
  };

  public static readonly Dictionary<Month, TMTime[]> MonthSunriseTime = new() {
    {Month.January, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 5, minute = 26}}},
    {Month.February, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 5, minute = 53}}},
    {Month.March, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 6, minute = 17}}},
    {Month.April, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 6, minute = 37}}},
    {Month.May, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 6, minute = 58}}},
    {Month.June, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 7, minute = 14}}},
    {Month.July, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 7, minute = 13}}},
    {Month.August, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 6, minute = 51}}},
    {Month.September, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 6, minute = 14}}},
    {Month.October, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 5, minute = 36}}},
    {Month.November, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 5, minute = 9}}},
    {Month.December, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 5, minute = 6}}},
  };

  public static readonly Dictionary<Month, TMTime[]> MonthSunsetTime = new() {
    {Month.January, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 19, minute = 24}}},
    {Month.February, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 19, minute = 6}}},
    {Month.March, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 18, minute = 32}}},
    {Month.April, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 17, minute = 55}}},
    {Month.May, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 17, minute = 28}}},
    {Month.June, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 17, minute = 20}}},
    {Month.July, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 17, minute = 31}}},
    {Month.August, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 17, minute = 50}}},
    {Month.September, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 18, minute = 9}}},
    {Month.October, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 18, minute = 29}}},
    {Month.November, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 18, minute = 54}}},
    {Month.December, new[]{new TMTime{hour = 0, minute = 0}, new TMTime{hour = 19, minute = 18}}},
  };
}

public struct TMDateTime {
  public ulong year;
  public ulong month;
  public ulong day;
  public ulong hour;
  public ulong minute;
  public ulong second;
}

public struct TMDate {
  public ulong year;
  public ulong month;
  public ulong day;
}

public struct TMTime {
  public ulong hour;
  public ulong minute;
  public ulong second;
}
