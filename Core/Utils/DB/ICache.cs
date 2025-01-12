﻿namespace Core.Utils.DB;

public interface ICache
{
    Task<string?> GetStringAsync(string key);
    Task SetStringAsync(string key, string value);

    Task RemoveAsync(string key);
    void Remove(string key);
    string? GetString(string key);
    void SetString(string key, string value);
}