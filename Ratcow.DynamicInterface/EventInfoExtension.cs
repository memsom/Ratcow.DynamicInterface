﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ratcow.DynamicInterface
{
    public static class EventInfoExtension
    {
        static IEnumerable<EventInfo> GetEventsImpl(Type type)
        {
            if (!type.IsInterface)
                return type.GetEvents();

            return (new Type[] { type })
                   .Concat(type.GetInterfaces())
                   .SelectMany(i => i.GetEvents());
        }

        public static IEnumerable<EventInfo> GetPublicEvents(this Type type)
        {
            return GetEventsImpl(type);
        }
    }
}
