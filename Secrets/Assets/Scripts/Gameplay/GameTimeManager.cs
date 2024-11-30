using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameTimeManager : Singleton<GameTimeManager>
{
    [Serializable]
    public struct FactualTime
    {
        public int Hours;
        public int Minutes;
        public int Seconds;
        public float TotalSeconds;

        public static bool operator >(FactualTime lhs, FactualTime rhs)
        {
            return lhs.TotalSeconds > rhs.TotalSeconds;
        }

        public static bool operator <(FactualTime lhs, FactualTime rhs)
        {
            return lhs.TotalSeconds < rhs.TotalSeconds;
        }

        public static bool operator ==(FactualTime lhs, FactualTime rhs)
        {
            return Mathf.Approximately(lhs.TotalSeconds, rhs.TotalSeconds);
        }

        public static bool operator !=(FactualTime lhs, FactualTime rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (obj is FactualTime)
            {
                return this == (FactualTime)obj;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return TotalSeconds.GetHashCode();
        }

        public void SetTime(int hours, int minutes, int seconds)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            TotalSeconds = hours * 3600 + minutes * 60 + seconds;
        }

        public string GetTime1()
        {
            return $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";
        }

        public string GetTime2()
        {
            return $"{Hours:D2}:{Minutes:D2}";
        }

        public float ToGameTime()
        {
            return TotalSeconds / _timeScale;
        }
    }


    [FormerlySerializedAs("CurrentGameTime")]
    public FactualTime currentFactualTime = new FactualTime();

    [SerializeField] private float EachDayLasts = 300f;
    [SerializeField] private float record = 0f;
    [SerializeField] private Coroutine handle;
    private static float _timeScale;
    private bool isPaused = false;


    private void OnEnable()
    {
        _timeScale = 86400 / EachDayLasts;
        StartGameTime();
    }

    public void StopTime(bool pause)
    {
        isPaused = pause;
    }

    public void StartGameTime()
    {
        if (handle != null)
            StopGameTime();
        handle = StartCoroutine(UpdateTime());
    }

    public void StopGameTime()
    {
        StopCoroutine(handle);
    }

    void ProcessTime()
    {
        int totalSeconds = Convert.ToInt32(record * _timeScale);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;
        currentFactualTime.SetTime(hours, minutes, seconds);
    }

    private IEnumerator UpdateTime()
    {
        while (gameObject)
        {
            while (isPaused)
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }

            yield return new WaitForSecondsRealtime(0.1f);

            record += 0.1f;
            ProcessTime();
            if (record >= EachDayLasts)
            {
                record = 0;
            }
        }
    }
    
    public bool IsCurrentTimeGreaterThan(FactualTime targetTime)
    {
        return currentFactualTime > targetTime;
    }
}