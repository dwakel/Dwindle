using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Converter;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Imaging;
using System.Diagnostics;


namespace ImageCompressor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataConverter dc = new DataConverter();
        private  string _retimg;
        Notifier notify = new Notifier();
        OpenFileDialog ofd = new OpenFileDialog();
        string[] files;
        string[] filename;
        class Imageclass : Notifier
        {
            string selectedimage;
           public string imagename { set; get; }
            public string selectedImage {
                get { return this.selectedimage; }
                set
                {
                    this.selectedimage = value;
                    OnPropertyChanged("selectedImage");
                }
            }
            
            
        }
        public string[] ImagePath { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();

            ListImages.SelectionMode = SelectionMode.Multiple;
            
            //ReturnImage = string.Empty;
                 
        }

        private void CompressImage_Click(object sender, RoutedEventArgs e)
        {
            ofd.Filter = "|*ico|*png|*jpg|*bmp|*gif";
            ofd.Multiselect = true;
            //ofd.ShowDialog()==

            if (ofd.ShowDialog() == true)
            {
                files = ofd.FileNames;
                filename = ofd.SafeFileNames;
            }

            var count = new int();
            List<Imageclass> finalImages = new List<Imageclass>();
                ImageArray ia = new ImageArray();
            //FileStream fr = new FileStream(ofd.FileName, FileMode.OpenOrCreate);
            foreach (var item in files)
            {


                FileStream fr = new FileStream(item, FileMode.OpenOrCreate);

                System.Drawing.Image img = System.Drawing.Image.FromStream(fr);

                var imageform = System.IO.Path.GetExtension(fr.Name);

               

                switch (imageform)
                {
                    case ".jpg":
                        img.Save(fr, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".png":
                        img.Save(fr, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".bmp":
                        img.Save(fr, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case "ico":
                        img.Save(fr, System.Drawing.Imaging.ImageFormat.Icon);
                        break;
                    case ".gif":
                        img.Save(fr, System.Drawing.Imaging.ImageFormat.Gif);
                        break;



                    default:
                        break;
                }

                

                System.IO.Directory.CreateDirectory("C:\\Users\\"+Environment.UserName.ToString()+"\\Pictures\\Dwindle\\");


                byte[] imgArray = dc.ImageToByteArray(img);
                ia.ImageArrayElements = imgArray;
                using (FileStream fs = File.Create("C:\\Users\\" + Environment.UserName.ToString() + "\\Pictures\\Dwindle\\ImageFile.bin"))
                {
                    var sf = new SoapFormatter();
                    var bf = new BinaryFormatter();
                    //bf.Serialize(fs, ia);
                    sf.Serialize(fs, ia);
                }




                SoapFormatter sff = new SoapFormatter();
                BinaryFormatter bff = new BinaryFormatter();

                using (FileStream fs = File.OpenRead("C:\\Users\\" + Environment.UserName.ToString() + "\\Pictures\\Dwindle\\ImageFile.bin"))
                {
                    var ia2 = (ImageArray)sff.Deserialize(fs);
                    byte[] imgArray2 = ia2.ImageArrayElements;
                    System.Drawing.Image finalActualImage = dc.ByteArrayToImage(imgArray2);

                   // var rand = new Random();
                    var imagePath = "C:\\Users\\" + Environment.UserName.ToString() + "\\Pictures\\Dwindle\\Compressed" + filename[count] +".jpg";

                    // var gg = Graphics.FromImage(finalActualImage);
                    var btm = new Bitmap(finalActualImage);

                    btm.Save(imagePath, ImageFormat.Jpeg);
                    finalImages.Add(new Imageclass { imagename = imagePath });

                }

                count++;
            }
            

            ListImages.ItemsSource = finalImages;


        }

       

        private void Btn_GoToPictures_Click(object sender, RoutedEventArgs e)
        {
            StartProcess("C:\\Users\\" + Environment.UserName.ToString() + "\\Pictures\\Dwindle\\");
        }

        private void StartProcess(string path)
        {
            ProcessStartInfo StartInformation = new ProcessStartInfo();
            StartInformation.FileName = path;
            Process process = Process.Start(StartInformation);
            //process.EnableRaisingEvents = true;
        }

        private void Btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            ListImages.SelectAll();
        }

        private void Btn_UnselectAll_Click(object sender, RoutedEventArgs e)
        {
            ListImages.UnselectAll();
        }

        private void CheckBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
