namespace frontend.Extensions
{
    public static class EnumerableOfGuid
    {
        public static string ToQueryString(this IEnumerable<Guid> ids)
        {
            return "?" + string.Join("&", ids.Select(g => $"concertIds={g}"));
        }
    }
}
