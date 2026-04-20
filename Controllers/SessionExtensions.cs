using Microsoft.AspNetCore.Http;
using System.Text.Json;

public static class SessionExtensions
{
    // Hàm dùng để lưu một Object vào Session (Convert sang Json)
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    // Hàm dùng để lấy một Object từ Session (Parse từ Json ra)
    public static T Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}