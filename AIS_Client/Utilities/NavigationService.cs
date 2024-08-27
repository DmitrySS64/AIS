using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AIS_Client.Utilities
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<Type, Type> _viewModelPageMap = new Dictionary<Type, Type>();
        private readonly Frame _mainFrame;

        public NavigationService(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        public void Configure<TVievModel, TPage>() where TPage : Page
        {
            _viewModelPageMap[typeof(TVievModel)] = typeof(TPage);
        }
        public void NavigateTo(Type viewModelType)
        {
            if (_viewModelPageMap.TryGetValue(viewModelType, out Type pageType))
            {
                var page = (Page)Activator.CreateInstance(pageType);
                _mainFrame.Navigate(page);
            }
        }
        public void GoBack()
        {
            if (_mainFrame.CanGoBack)
            {
                _mainFrame.GoBack();
            }
        }
    }
}
