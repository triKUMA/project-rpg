using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static TMConstants;

public partial class TimeManager : Node3D {
  [Export] public Hemisphere Hemisphere { get; private set; } = Hemisphere.South;

  // Day length in minutes
  [Export] private double dayLength = 30d;
  [Export, Range(1ul, 24ul)] private ulong startHour = 9ul;

  public static TimeManager Instance { get; private set; }

  public Season Season => MonthToSeason[(Month)Month][(int)Hemisphere];

  public event Action<ulong> OnSecondUpdated;
  public event Action<ulong> OnMinuteUpdated;
  public event Action<ulong> OnHourUpdated;
  public event Action<ulong> OnDayUpdated;
  public event Action<ulong> OnMonthUpdated;
  public event Action<ulong> OnYearUpdated;

  public ulong Year = 1ul;
  public ulong Month = 1ul;
  public ulong Day = 1ul;
  public ulong Hour = 0ul;
  public ulong Minute = 0ul;
  public ulong Second = 0ul;
  public double Millisecond = 0d;

  private double timeSpeed => (24 * 60) / dayLength;

  public override void _Ready() {
    if (Instance != null) {
      QueueFree();
      return;
    }

    Instance = this;

    Hour = startHour;
  }

  public override void _Process(double delta) {
    Millisecond += delta * 1000d * timeSpeed;
    if (Millisecond < 1000) return;

    Second += (ulong)(Millisecond / 1000);
    Millisecond %= 1000;
    OnSecondUpdated?.Invoke(Second);

    CascadeTime(ref Second, ref Minute, 60ul, OnMinuteUpdated);
    CascadeTime(ref Minute, ref Hour, 60ul, OnHourUpdated);
    CascadeTime(ref Hour, ref Day, 24ul, OnDayUpdated);
    CascadeDay(ref Day, ref Month, OnMonthUpdated);
    CascadeTime(ref Month, ref Year, 12ul, OnYearUpdated, true);
  }

  public TMDateTime GetDateTime() {
    return new TMDateTime {
      year = Year,
      month = Month,
      day = Day,
      hour = Hour,
      minute = Minute,
      second = Second,
    };
  }

  private void CascadeTime(ref ulong startVal, ref ulong overflowVal, ulong capacity, Action<ulong> overflowCallback, bool startsAtOne = false) {
    var adjStartVal = startsAtOne ? startVal - 1ul : startVal;

    if (adjStartVal < capacity) return;

    overflowVal += adjStartVal / capacity;
    startVal = adjStartVal % capacity + (startsAtOne ? 1ul : 0ul);

    overflowCallback?.Invoke(overflowVal);
  }

  private void CascadeDay(ref ulong day, ref ulong month, Action<ulong> overflowCallback) {
    bool overflow, monthUpdated = false;
    do {
      overflow = false;
      var leapYear = IsLeapyear(Year) ? 29ul : 28ul;
      var monthValue = (month - 1) % 12 + 1;
      var monthAsd = MonthLengths[(Month)monthValue];
      ulong monthLength = month == 2ul ? leapYear : monthAsd;
      if (day > monthLength) {
        overflow = true;
        day -= monthLength;
        month++;
        monthUpdated = true;
      }
    } while (overflow);

    if (monthUpdated) overflowCallback?.Invoke(month);
  }

  private bool IsLeapyear(ulong year) {
    return (
      year % 4ul == 0ul && (
      year % 100ul != 0ul ||
      year % 400ul == 0ul
      )
    );
  }
}
