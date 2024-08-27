using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Client.Utilities
{
    public interface INavigationService
    {
        void NavigateTo(Type viewModelType);
        void GoBack();
    }
}
