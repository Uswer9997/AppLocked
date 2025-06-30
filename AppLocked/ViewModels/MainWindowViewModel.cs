using AppLocked.Models;
using System;
using System.Collections.ObjectModel;

namespace AppLocked.ViewModels
{
  internal class MainWindowViewModel
  {
    private MyEventService _eventService;

    /// <summary>
    /// События выбрасываемые из таймера, но уже преобразованные.
    /// </summary>
    public ObservableCollection<MyWrapEvent> Events { get => _eventService.Events; }

    #region Constructor
    public MainWindowViewModel()
    {
      _eventService = new  MyEventService();
    }
    #endregion

    private void OnAddingEvents(object sender, EventArgs e)
    {
      
    }
  }
}
