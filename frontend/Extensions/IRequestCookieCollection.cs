﻿using frontend.Models;

namespace frontend.Extensions
{
    public static class RequestCookieCollection
    {
        public static Guid GetCurrentBasketId(this IRequestCookieCollection cookies, Settings settings)
        {
            Guid.TryParse(cookies[settings.BasketIdCookieName], out var basketId);
            return basketId;
        }
    }
}
