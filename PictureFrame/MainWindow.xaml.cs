using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Threading;

namespace PictureFrame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<BitmapImage> imgList = new List<BitmapImage>();// Where the list of images will be stored
        BitmapImage dispimg = new BitmapImage();
        int breaktime;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Add images to the list.
            OpenFileDialog o = new OpenFileDialog();
            bool success = true;
            o.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            o.Multiselect = true;
            o.ShowDialog();
            for(int z = 0;z<o.FileNames.Length;z++)
            {
                BitmapImage bi = new BitmapImage();
                try
                {
                    bi = new BitmapImage(new Uri(o.FileNames[z]));
                    imgList.Add(bi);
                }
                catch
                {
                    System.Windows.MessageBox.Show("Please choose a valid image.");
                    success = false;
                    ClearAllImages();
                    break;
                }
            }
            if (success)
            {
                tgStart.IsEnabled = true;
                btnClearAll.IsEnabled = true;
            }
          }

        void ClearAllImages()
        {
            //Clear all stored images
            imgList.Clear();
            tgStart.IsEnabled = false;
            btnClearAll.IsEnabled = false;
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            ClearAllImages();
        }
        
        private async void Start(object sender, RoutedEventArgs e)
        {            
            Random r = new Random();
            bool StringIsValid,CharIsValid;
            if (imgList.Count == 0)
                image.Source = null;
            else
            {   if (Tim.Text.Equals(""))
                    breaktime = 5;
                else
                {
                    StringIsValid = true;                    
                    string InputBreakTime = Tim.Text;
                    //To check if the input is an integer
                    char[] PossibleChar= new char[]{'0','1','2','3', '4', '5','6','7','8','9' };
                    foreach (char x in InputBreakTime)
                    {
                        CharIsValid = false;
                        foreach (char y in PossibleChar)
                        {
                            if (x.Equals(y))
                            {
                                CharIsValid = true;
                                break;
                            }
                        }
                        if (!CharIsValid)
                        {
                            StringIsValid = false;
                            break;
                        }                            
                    }

                    if (StringIsValid)
                    {
                        //To avoid a crash due to null error or having a delay time above 5 min
                        if ((int.Parse(InputBreakTime) > 0) && (int.Parse(InputBreakTime) < 300))
                            breaktime = int.Parse(InputBreakTime);
                        else
                            breaktime = 5;//Default value
                    }
                    else
                        breaktime = 5;//Default value
                }
                btnClearAll.IsEnabled = false;
                btnAdd.IsEnabled = false;
                tgStart.Content = "Stop";
                while (tgStart.IsChecked == true)
                {
                    
                    dispimg = imgList[r.Next(0, imgList.Count)];
                    image.Source = dispimg;                    
                    await Task.Delay(breaktime*1000);//Causes the delay till the change in Image                    
                }
            }
        }

        //Method to stop the slide show
        private  void Stop(object sender, RoutedEventArgs e)
        {
            image.Source = null;//removes the image
            btnClearAll.IsEnabled = true;
            btnAdd.IsEnabled = true;
            tgStart.Content = "Start";
        }
        
    }
}
