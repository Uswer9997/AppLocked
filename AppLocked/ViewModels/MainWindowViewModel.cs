using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLocked.ViewModels
{
  internal class MainWindowViewModel
  {
    /// <summary>
    /// События выбрасываемые из таймера
    /// </summary>
    public ObservableCollection<MyEvent> Events { get { return _orionDataProvider.ECEvents; } }
  }
}
