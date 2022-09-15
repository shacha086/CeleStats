using System;

namespace Celeste.Mod.CeleStats;

public class Singleton<T> where T : new()
{
    private static T _instance;
    public static T Instance
    {
        get {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }
    }

    public static void Refresh()
    {
        if (_instance is IDisposable disposable)
        {
            disposable.Dispose();
            _instance = default;
        }
        else
        {
            throw new NotSupportedException("this is not disposable");
        }
    }
}