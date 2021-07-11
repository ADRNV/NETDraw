using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using NETDraw.Drawing.Artist;
using NETDraw.Drawing;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking;
using System.Collections.ObjectModel;

namespace NETDraw.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkPage : ContentPage
    {
        private static Artist _artist = new Artist();//Отрисовшик

   
        /// <summary>
        /// Эти поля должны быть в Artist
        /// </summary>
        private Dictionary<long, SKPath> Trace = new Dictionary<long, SKPath>();//Штрих в реальном времени

        /// <summary>
        /// Эти поля должны быть в Artist
        /// </summary>
        private List<SKPath> Hatch = new List<SKPath>();//Готовй штрих
        
        public WorkPage()
        {
            this.BindingContext = _artist;

            InitializeComponent();

            _artist.CurrentPaint = _artist.Brushes["Stroke"];


            foreach (var brush in _artist.Brushes.Keys)
            {
                BrushPicker.Items.Add(brush);
            }




        }

       




        /// <summary>
        /// Для этого нужно создать связующее звено c Artist и View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        #region HSLSlider
        private void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            ColorPreView.InvalidateSurface();

        }

        private void OnHslCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKColor currentcolor = SKColor.FromHsl( (float)hueSlider.Value,
                                            (float)saturationSlider.Value,
                                            (float)lightnessSlider.Value);
            
            args.Surface.Canvas.Clear(currentcolor);

            _artist.CurrentPaint.Color = currentcolor;
               
           
            
            
        }



        #endregion


        /// <summary>
        /// Это нужно перенести в другой класс возможно в Artist, т.к этот метод отвечает за отрисовку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TouchEffect_TouchAction(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!Trace.ContainsKey(args.Id))
                    {
                        SKPath path = new SKPath();
                        path.MoveTo(_artist.ConvertToPixel(args.Location,Canvas));
                        Trace.Add(args.Id, path);
                        Canvas.InvalidateSurface();
                        
                        
                    }
                    break;

                case TouchActionType.Moved:
                    if (Trace.ContainsKey(args.Id))
                    {
                        SKPath path = Trace[args.Id];
                        path.LineTo(_artist.ConvertToPixel(args.Location, Canvas));
                        Canvas.InvalidateSurface();
                        
                    }
                    break;

                case TouchActionType.Released:
                    if (Trace.ContainsKey(args.Id))
                    {
                        Hatch.Add(Trace[args.Id]);
                        Trace.Remove(args.Id);
                        Canvas.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (Trace.ContainsKey(args.Id))
                    {
                        Trace.Remove(args.Id);
                        Canvas.InvalidateSurface();
                    }
                    break;
            
            }
            

        }

     

        
        /// <summary>
        /// Это нужно перенести в Artist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            _artist.Draw(args, Hatch, Trace,_artist.CurrentPaint);
          
        }

      

        private void BrushPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            string brushName = BrushPicker.Items[BrushPicker.SelectedIndex];
           _artist.CurrentPaint = _artist.Brushes[brushName];

            
            
        }
    }
}
