﻿using System;

namespace AppLocked.Models
{
  internal class MyWrapEvent
  {
    public DateTime EventDate { get; set; }
    public string EventName { get; set; }
    public string Description { get; set; }

    public MyWrapEvent(MyEvent baseEvent)
    {
      EventDate = baseEvent.EventDate;
      EventName = baseEvent.EventName;
      Description = baseEvent.Description;
    }
  }
}
}
