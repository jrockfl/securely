namespace Securely.Application.Helpers;

public static class Argument
{
    public static void AssertNotNull<T>(T value, string name)
    {
        if (value is null)
        {
            throw new ArgumentNullException(name);
        }
    }

    public static void AssertNotNullOrEmpty(string value, string name)
    {
        if (value is null)
        {
            throw new ArgumentNullException(name);
        }

        if (value.Length == 0)
        {
            throw new ArgumentException("Value cannot be an empty string.", name);
        }
    }

    public static void AssertNotDefault<T>(T value, string name)
    {
        if (Equals(value, default(T)))
        {
            throw new ArgumentException("Value cannot be default.", name);
        }
    }
}
