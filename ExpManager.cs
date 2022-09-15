using System;
using System.Collections.Generic;
using System.Globalization;

namespace Celeste.Mod.CeleStats;

public class ExpManager : Singleton<ExpManager>, IDisposable
{
    private bool _disposed;
    private int _restQuantity;
    private RequestManager _requestManager = new(CeleStatsModule.Settings.ApiUrl, CeleStatsModule.Settings.ApiKey);

    private int RestQuantity
    {
        get => _restQuantity;
        set
        {
            if (value < 1000)
            {
                _restQuantity = value;
            }
            else
            {
                CommitAll();
                _restQuantity = value - _restQuantity;
            }
        }
    }

    public ExpManager()
    {
        CeleStatsModule.Settings.ApiUrlChangedEvent += url => { _requestManager.BaseAddress = url; };
        CeleStatsModule.Settings.ApiKeyChangedEvent += key => { _requestManager.Token = key; };
    }

    public void AddExp(int quantity = 1)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException("");
        }

        Console.Out.WriteLine($"AddExp {quantity}");
        RestQuantity += quantity;
    }

    public void CommitAll()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException("");
        }

        if (RestQuantity < 1)
        {
            return;
        }

        Console.Out.WriteLine($"Commiting {RestQuantity} xps.");
        _requestManager.Post(new CodeStatsPayload
        {
            Time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture),
            Experiences = Experiences.Of(new[] { new KeyValuePair<string, int>("Celeste", RestQuantity) })
        }, null, null, exception => throw exception);
        
        RestQuantity = 0;
    }

    public void Dispose()
    {
        CommitAll();
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (_disposed)
        {
            return;
        }

        if (isDisposing)
        {
            _requestManager = null;
        }

        _disposed = true;
    }
}