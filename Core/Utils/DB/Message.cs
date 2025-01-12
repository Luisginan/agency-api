﻿using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;

namespace Core.Utils.DB;

[ExcludeFromCodeCoverage]
public class Message<T>
{
    public Message()
    {
    }

    public Message(string? appId, string key, T? payLoad, int retry)
    {
            AppId = appId;
            Key = key;
            PayLoad = payLoad;
            Retry = retry;
        }
    
    public ObjectId Id { get; set; }
    public string Topic { get; init; } = "";
    public string GroupId { get; init; } = "";
    public string? AppId { get; init; } = "";
    public string Key { get; init; } = "";
    public T? PayLoad { get; init; }
    public int Retry { get; set; }
    public string? Error { get; set; }
    public string? Method { get; set; }
}