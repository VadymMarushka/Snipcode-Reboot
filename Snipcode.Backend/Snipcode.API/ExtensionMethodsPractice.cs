public static class EnumerableExtensions
{
    public static IEnumerable<T> MyWhere<T>(
        this IEnumerable<T> source,
        Func<T, bool> predicate)
    {
        if (source == null || predicate == null)
            throw new ArgumentNullException();

        foreach (var item in source)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<TResult> MySelect<TSource, TResult>(
    this IEnumerable<TSource> source,
    Func<TSource, TResult> func)
    {
        if (source == null || func == null)
            throw new ArgumentNullException();

        foreach (var item in source)
        {
             yield return func(item);
        }
    }
}