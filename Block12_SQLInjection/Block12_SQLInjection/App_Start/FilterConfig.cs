﻿using System.Web;
using System.Web.Mvc;

namespace Block12_SQLInjection
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
