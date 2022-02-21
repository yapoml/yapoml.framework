﻿namespace Yapoml.Playwright.Events
{
    public class EventSource : IEventSource
    {
        public IPageEventSource PageEventSource { get; } = new PageEventSource();

        public IComponentEventSource ComponentEventSource { get; } = new ComponentEventSource();
    }
}
