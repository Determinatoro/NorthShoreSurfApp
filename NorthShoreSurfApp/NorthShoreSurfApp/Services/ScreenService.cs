using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShoreSurfApp
{
    public interface IScreenService
    {
        void Landscape();
        void Portrait();
        void Unspecified();
        void HideStatusBar();
        void ShowStatusBar();
    }
}
