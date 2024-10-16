using System;
using System.Windows.Forms;

namespace DirecLayer
{
    public class EventHelper
    {
        public static void RaisedEvent(Object objectRaised,
            EventHandler eventHandlerRaised, EventArgs eventArgs)
        {
            if (eventHandlerRaised != null)
            {
                eventHandlerRaised.Invoke(objectRaised, eventArgs);
            }
        }

        public static void RaisedCellEvent(Object objectRaised, DataGridViewCellEventHandler eventHandlerRaised, DataGridViewCellEventArgs e)
        {
            if (eventHandlerRaised != null)
            {
                eventHandlerRaised.Invoke(objectRaised, e);
            }
        }

        public static void RaisedCellMouseEvent(Object objectRaised, DataGridViewCellMouseEventHandler eventHandlerRaised, DataGridViewCellMouseEventArgs e)
        {
            if (eventHandlerRaised != null)
            {
                eventHandlerRaised.Invoke(objectRaised, e);
            }
        }

        public static void RaiseFormCloseEvent(Object objectRaised, FormClosingEventHandler eventHandlerRaised, FormClosingEventArgs e)
        {
            if (eventHandlerRaised != null)
            {
                eventHandlerRaised.Invoke(objectRaised, e);
            }
        }

        public static void RaisedPreviewKeyDown(Object objectRaised, PreviewKeyDownEventHandler eventHandlerRaised, PreviewKeyDownEventArgs e)
        {
            if (eventHandlerRaised != null)
            {
                eventHandlerRaised.Invoke(objectRaised, e);
            }
        }

        public static void RaisedScroll(Object objectRaised, ScrollEventHandler eventHandlerRaised, ScrollEventArgs e)
        {
            if (eventHandlerRaised != null)
            {
                eventHandlerRaised.Invoke(objectRaised, e);
            }
        }

        public static void RaisedLeave(Object objectRaised, EventHandler eventHandlerRaised, EventArgs e)
        {
            if (eventHandlerRaised != null)
            {
                eventHandlerRaised.Invoke(objectRaised, e);
            }
        }
    }
}
