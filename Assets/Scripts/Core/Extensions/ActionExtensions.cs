
public static class ActionExtensions
{
    public static void SafeInvoke(this System.Action @this)
    {
        if (@this != null)
        {
            @this.Invoke();
        }
    }

    public static void SafeInvoke<T>(this System.Action<T> @this, T arg)
    {
        if (@this != null)
        {
            @this.Invoke(arg);
        }
    }
}
