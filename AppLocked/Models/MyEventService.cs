using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLocked.Models
{
  internal class MyEventService
  {
    private Infrastructure.TimerService _timerService;
    /// <summary>
    /// События преобразованные для отображения.
    /// </summary>
    public ObservableCollection<MyWrapEvent> Events { get; }


    public MyEventService()
    {
      Events = new ObservableCollection<MyWrapEvent>();

      _timerService = new Infrastructure.TimerService(System.Threading.SynchronizationContext.Current);
      _timerService.GenerationInterval = 5;
      _timerService.AddingEvents += TimerServiceAddingEventsHandler;
    }

    private void TimerServiceAddingEventsHandler(object sender, EventArgs e)
    {
      foreach (MyEvent elEvent in _timerService.Events)
      {
        // создадим объект 'события' на основе новых данных
        MyWrapEvent _wrapEvent = new MyWrapEvent()
        {
          EventDate = elEvent.EventDate,
          EventName = elEvent.EventName,
          Description = elEvent.Description
        };
        Events.Add(_wrapEvent);
      }
    }
  }
}
