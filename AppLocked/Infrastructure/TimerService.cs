using AppLocked.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLocked.Infrastructure
{
  internal class TimerService : IDisposable
  {
    /// <summary>
    /// Таймер генерации новых событий
    /// </summary>
    private System.Timers.Timer checkTimer;

    private object locker;

    private int _generationInterval;
    /// <summary>
    /// Интервал генерации новых событий.
    /// </summary>
    public int GenerationInterval
    {
      get => this._generationInterval;
      set
      {
        _generationInterval = value;
        checkTimer.Interval = _generationInterval * 1000;
      }
    }


    private MyEvent[] _events;
    /// <summary>
    /// Генерируемые события
    /// </summary>
    public MyEvent[] Events
    {
      get => _events;
      private set
      {
        lock (locker)
        { _events = value; }
      }
    }

    /// <summary>
    /// Событие добавления новых 'событий'.
    /// </summary>
    public event EventHandler AddingEvents;
       
    /// <summary>
    /// Контекст синхронизации потока.
    /// </summary>
    private System.Threading.SynchronizationContext _synchronizationContext;



    #region Constructor
    public TimerService(System.Threading.SynchronizationContext synchronizationContext)
    {
      _synchronizationContext = synchronizationContext;
      locker = new object();

      checkTimer = new System.Timers.Timer();
      GenerationInterval = 10; // default value
      checkTimer.Elapsed += OnCheckEvents;
      checkTimer.Start();
    }
    #endregion

    private int counter;
    /// <summary>
    /// Создаёт новые события.
    /// </summary>
    protected void OnCheckEvents(object source, System.Timers.ElapsedEventArgs e)
    {
      // Останавливаем таймер на время выполнения метода
      checkTimer.Stop();

      try
      {
        MyEvent[] nEvents = new MyEvent[] { new MyEvent() { EventDate = DateTime.Now,
                                                            Description = "Тестовое событие",
                                                            EventName = (counter++).ToString() },
                                            new MyEvent()  { EventDate = DateTime.Now,
                                                            Description = "Тестовое событие",
                                                            EventName = (counter++).ToString() },
                                            new MyEvent() { EventDate = DateTime.Now,
                                                            Description = "Тестовое событие",
                                                            EventName = (counter++).ToString() } };

        if (System.Threading.Thread.CurrentThread.Name == null)
          System.Threading.Thread.CurrentThread.Name = "Timer thread";
        Console.WriteLine(System.Threading.Thread.CurrentThread.Name);

        Console.WriteLine("Вызываем метод Post");
        // Выполним метод в потоке синхронизации
        _synchronizationContext?.Post(AddEvents, nEvents);

        Console.WriteLine("Метод Post отработал");
      }
      catch
      {
        // тут выброс в поток синхронизации, а пока так
      }

      // Вновь запускаем таймер
      checkTimer.Start();
    }


    /// <summary>
    /// Этот метод выполнится в потоке синхронизации, т.е. там, 
    /// где был создан экземпляр данного сервиса.
    /// </summary>
    /// <param name="events"></param>
    private void AddEvents(object events)
    {
      MyEvent[] addingEvents = events as MyEvent[];

      if (AddingEvents != null)
      {
        if (System.Threading.Thread.CurrentThread.Name == null)
          System.Threading.Thread.CurrentThread.Name = "AddEvents metod thread";
        Console.WriteLine(System.Threading.Thread.CurrentThread.Name);

        Console.WriteLine("Начали записывать данные в свойство");
        Events = addingEvents;
        Console.WriteLine("Данные записали в свойство");

        Console.WriteLine("Начали вызывать событие через Invoke");
        AddingEvents.Invoke(this, EventArgs.Empty); // просто сигнал о налиции данных
        Console.WriteLine("Закончили вызывать событие через Invoke");
      }
    }

    #region Disposing
    // Flag: Has Dispose already been called?
    bool disposed = false;


    // Public implementation of Dispose pattern callable by consumers.
    public void Dispose()
    {
      Dispose(disposing: true);
    }

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
      if (disposed)
        return;

      if (disposing)
      {
        if (checkTimer.Enabled)
        {
          checkTimer.Stop();
          checkTimer.Elapsed -= OnCheckEvents;
          checkTimer.Dispose();
        }
      }

      disposed = true;
    }
    #endregion
  }
}
