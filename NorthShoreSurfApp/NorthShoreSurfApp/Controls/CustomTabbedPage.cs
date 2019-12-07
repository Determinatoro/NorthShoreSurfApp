using NorthShoreSurfApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace NorthShoreSurfApp
{
    public class CustomTabbedPage : TabbedPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            SetIcons();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            SetIcons();

            var navigationPage = CurrentPage as NavigationPage;
            if (navigationPage != null)
            {
                var rootPage = ((NavigationPage)CurrentPage).RootPage as ITabbedPageService;
                if (rootPage != null)
                    rootPage.OnPageSelected();
            }
        }

        private void SetIcons()
        {
            var list = Children.ToList();
            foreach (var page in list)
            {
                if (page is CustomNavigationPage)
                {
                    var navigationPage = page as CustomNavigationPage;

                    if (navigationPage.IconSelectedSource == null || navigationPage.IconUnselectedSource == null)
                        continue;
                    else if (page == CurrentPage)
                        page.IconImageSource = ((CustomNavigationPage)page).IconSelectedSource;
                    else
                        page.IconImageSource = ((CustomNavigationPage)page).IconUnselectedSource;
                }
            }
        }
    }
}
