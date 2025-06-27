using AppLocked.Models;
using System;
using System.Collections.ObjectModel;

namespace AppLocked.ViewModels
{
  internal class MainWindowViewModel
  {
    private Infrastructure.TimerService _timerService;

    /// <summary>
    /// События выбрасываемые из таймера, но уже преобразованные.
    /// </summary>
    public ObservableCollection<MyWrapEvent> Events { get; private set; }

    #region Constructor
    public MainWindowViewModel()
    {
      Events = new ObservableCollection<MyWrapEvent>();

      _timerService = new Infrastructure.TimerService(System.Threading.SynchronizationContext.Current);
      _timerService.GenerationInterval = 5;
      _timerService.AddingEvents += OnAddingEvents;
    }
    #endregion

    private object locker = new object();

    private void OnAddingEvents(object sender, EventArgs e)
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

        lock (locker)
        {
          Events.Add(_wrapEvent);
        }
      }
    }
  }
}
