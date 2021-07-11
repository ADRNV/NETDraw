using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using NETDraw.Drawing;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking;
using Xamarin.Forms;

namespace NETDraw.Drawing.Artist
{
    public class Artist : INotifyPropertyChanged
    {
        public SKPaint CurrentPaint;

        public Dictionary<string, SKPaint> Brushes = new Dictionary<string, SKPaint> 
        {
            {"Stroke",new SKPaint {StrokeCap = SKStrokeCap.Round,Style = SKPaintStyle.Stroke,StrokeWidth = 10,Color = CurrentColor}},
            {"Square",new SKPaint {StrokeCap = SKStrokeCap.Square,Style = SKPaintStyle.Stroke,StrokeWidth = 25,Color = CurrentColor}}


            
        };
        
       

           

        private int canvasstate;
        public int CanvasState
        {
            get => canvasstate;
        }

        private SKCanvas canvas;
        public SKCanvas Canvas
        {
            get => canvas;

            set => canvas = value;
        }

        private static SKColor _currentcolor;
       
        public static SKColor CurrentColor
        {
            get => _currentcolor;

            set 
            {
                _currentcolor = value;
               

            
            }
        }

        /// <summary>
        /// Этот метод конвертирует TouchTrackingPoint
        /// в SKPoint
        /// </summary>
        /// <param name="point">Кордината</param>
        /// <param name="source">К какому полотну это принадлежит</param>
        /// <returns>SKPoint</returns>
        public SKPoint ConvertToPixel(TouchTrackingPoint point,SKCanvasView source)
        {
            return new SKPoint((float)(source.CanvasSize.Width * point.X / source.Width),
                               (float)(source.CanvasSize.Height * point.Y / source.Height));
        }

        ///<summary>
        ///Этот метод отрисовывает заданные готовые и ресующиеся пути
        /// hatch - готовые пути
        /// trace - рисующиеся
        /// </summary>
        public void Draw(SKPaintSurfaceEventArgs args,List<SKPath> hatch,Dictionary<long,SKPath> trace,SKPaint paint)
        {
           

            canvas = args.Surface.Canvas;
            

            foreach(var path in hatch)//Обход всех готовых путей
            {
                canvas.DrawPath(path, CurrentPaint);
            }

            foreach(var path in trace.Values)//Обход еще рисующихся путей
            {
                canvas.DrawPath(path,CurrentPaint);
                
            }
          

            hatch.Clear();

            canvasstate = canvas.SaveCount;
            canvas.Save();
           
    
    
        }



     


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)//Реализация интерфейса INotifyPropertyChanged
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
