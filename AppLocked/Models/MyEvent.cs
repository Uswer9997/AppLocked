using System;

namespace AppLocked.Models
{
  internal class MyEvent
  {
    public DateTime EventDate { get; set; }
    public string EventName { get; set; }
    public string Description { get; set; }

    public MyEvent()
    {
    }
  }
}
