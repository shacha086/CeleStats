using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Celeste.Mod.CeleStats;

public class RequestManager
{
    private WebClient WebClient;
    private bool _posting;
    private List<object[]> _post_queue = new();

    private void SetHeader()
    {
        WebClient.Headers.Add("User-Agent", "CodeStats.Client/0.5.8");
        WebClient.Headers.Add("Content-Type", "application/json");
    }
    
    public RequestManager(string url, string token)
    {
        WebClient = new WebClient();
        WebClient.BaseAddress = url;
        WebClient.Headers.Add("X-API-Token", token);
    }

    public string BaseAddress
    {
        get => WebClient.BaseAddress;
        set => WebClient.BaseAddress = value;
    }
    
    public string Token
    {
        get => WebClient.Headers.Get("X-API-Token");
        set => WebClient.Headers.Set("X-API-Token", value);
    }
    
    public void Post(CodeStatsPayload data, Action<string> onComplete = null, Action onCanceled = null,
        Action<AggregateException> onFault = null)
    {
        if (_posting)
        {
            _post_queue.Add(new object[] { data, onComplete, onCanceled, onFault });
        }
        else
        {
            _posting = true;
            SetHeader();
            WebClient.UploadStringTaskAsync("", JsonConvert.SerializeObject(data, Formatting.Indented)).ContinueWith(
                task =>
                {
                    _posting = false;
                    if (task.IsFaulted)
                    {
                        onFault?.Invoke(task.Exception);
                    }
                    else if (task.IsCanceled)
                    {
                        onCanceled?.Invoke();
                    }
                    else
                    {
                        onComplete?.Invoke(task.Result);
                    }

                    if (!_post_queue.Any()) return;
                    var post = _post_queue[0];
                    var _0 = (CodeStatsPayload)post[0];
                    var _1 = post[1] as Action<string>;
                    var _2 = post[2] as Action;
                    var _3 = post[3] as Action<AggregateException>;
                    Post(_0, _1, _2, _3);
                    _post_queue.RemoveAt(0);
                }
            );
        }
    }
}