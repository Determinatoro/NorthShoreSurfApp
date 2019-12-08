using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NorthShoreSurfApp.ViewModels
{
    public class SurfingConditionsViewModel
    {
        public WebViewSource OceanInfoUrl { get => "https://servlet.dmi.dk/byvejr/servlet/byvejr?by=9021&tabel=dag1&param=bolger"; }
        public WebViewSource WeatherInfoUrl { get => "https://servlet.dmi.dk/byvejr/servlet/byvejr_dag1?by=9021&tabel=dag1&mode=long"; }
        public static string VideoUrl { get => "rtsp://127.0.0.1:8080/video/h264";}
    }
}
