using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace guiJson
{
    static class GUIController
    {
        /// <summary>
        /// create a scroll to any object 
        /// </summary>
        /// <param name="objectWithout"></param>
        /// <returns></returns>
        public static ScrollViewer addScrollToObject(UIElement objectWithout)
        {
            // Define a ScrollViewer
            ScrollViewer myScrollViewer = new ScrollViewer();
            myScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            // Add Layout control
            StackPanel myStackPanel = new StackPanel();
            myStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            myStackPanel.VerticalAlignment = VerticalAlignment.Top;

            myStackPanel.Children.Add(objectWithout);


            // Add the StackPanel as the lone Child of the Border
            myScrollViewer.Content = myStackPanel;
            return myScrollViewer;
        }
        /// <summary>
        /// create add menu
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="ListOfTextBoxAdd"></param>
        /// <returns></returns>
        public static Canvas createAddMenu(Type temp ,ref List<TextBox> ListOfTextBoxAdd)
        {
            Canvas canvas = new Canvas();
            int x = 0;
            int y = 0;
            foreach (var item in temp.GetFields())
            {
                x = 0;
                TextBlock textBlock = new TextBlock();
                textBlock.Text = item.Name;
                textBlock.Margin = new Thickness(x,y,0,0);
                canvas.Children.Add(textBlock);
                x += 50;
                TextBox textBox = new TextBox();
                textBox.Width = 100;
                textBox.Height = 30;
                textBox.Margin = new Thickness(x, y, 0, 0);
                textBox.Tag = item.Name;
                ListOfTextBoxAdd.Add(textBox);
                canvas.Children.Add(textBox);
                y += 50;
            }
          return canvas;
        }
        
        /// <summary>
        /// create the main objects of the jsons 
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <returns></returns>
        public static Canvas createMainObject(MainWindow mainWindow)
        {
            Canvas canvas = new Canvas();
            double widthWindows = System.Windows.SystemParameters.PrimaryScreenWidth;
            int x = 10;
            int y = 0;
            int sizeOfCanvas = 100;
            foreach (var item in MainWindow.listOfJsonModel)
            {
                if (x > widthWindows )
                {
                    y += sizeOfCanvas;
                    x = 0;
                }
                Canvas objectCanvas= createListOfFieldsForObject(item);
                objectCanvas.Margin = new Thickness(x,y,0,0);
                objectCanvas.Background = new SolidColorBrush(Colors.Cyan);
                objectCanvas.Width = sizeOfCanvas;
                objectCanvas.Height = sizeOfCanvas;
                objectCanvas.Tag = item.GetHashCode();
                objectCanvas.MouseDown += mainWindow.enterObjectCanvas;
                canvas.Children.Add(objectCanvas);
                x += sizeOfCanvas + 10;
              
            }
            return canvas;
        }
        
      
        /// <summary>
        /// create the fields ins strings to any objects
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static Canvas createListOfFieldsForObject(object temp)
        {
            Canvas canvas = new Canvas();
            int x = 0;
            int y = 0;
            foreach (var item in temp.GetType().GetFields())
            {
                x = 0;
                TextBlock textBlock = new TextBlock();
                textBlock.Text = item.Name+" : "+item.GetValue(temp);
                textBlock.Margin = new Thickness(x, y, 0, 0);
                canvas.Children.Add(textBlock);
                y += 10;
            }
            return canvas;
        }

    }
}
