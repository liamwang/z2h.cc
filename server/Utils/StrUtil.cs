namespace Server.Utils;

public class StrUtil
{
    /// <summary>
    /// 打乱字符串
    /// </summary>
    public static string Mixup(string text)
    {
        int sum = 0;
        foreach (char c in text)
        {
            sum += c;
        }
        int x = sum % text.Length;
        char[] arr = text.ToCharArray();
        char[] newArr = new char[arr.Length];
        Array.Copy(arr, x, newArr, 0, text.Length - x);
        Array.Copy(arr, 0, newArr, text.Length - x, x);
        return new string(newArr);
    }

    /// <summary>
    /// 恢复打乱的字符串
    /// </summary>
    public static string UnMixup(string cipher)
    {
        int sum = 0;
        foreach (char c in cipher)
        {
            sum += c;
        }
        int x = cipher.Length - sum % cipher.Length;
        char[] arr = cipher.ToCharArray();
        char[] newArr = new char[arr.Length];
        Array.Copy(arr, x, newArr, 0, cipher.Length - x);
        Array.Copy(arr, 0, newArr, cipher.Length - x, x);
        return new string(newArr);
    }
}
