using Android.App;
using Android.OS;
using DigitalSwitchClient.Models;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace DigitalSwitchClient
{
    [Activity(Label = "DigitalSwitchClient", MainLauncher = true)]
    public class MainActivity : Activity
    {
        List<ItemUI> itemUIList;
        ImageView imgHome;
        FrameLayout flContainer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            imgHome = FindViewById<ImageView>(Resource.Id.imgHome);
            flContainer = FindViewById<FrameLayout>(Resource.Id.flContainer);

            Title = Resources.GetString(Resource.String.lbl_home);

            readItemsConfig();

            Task.Factory.StartNew(() =>
            {
                Task.Delay(1000).Wait();
                RunOnUiThread(renderItemUIList);
            });
        }

        private void readItemsConfig()
        {
            Stream fileStream = Assets.Open("lam_coordinates.json");
            string fileText = new StreamReader(fileStream).ReadToEnd();
            itemUIList = JsonConvert.DeserializeObject<List<ItemUI>>(fileText);
        }

        private void renderItemUIList()
        {
            foreach (var item in itemUIList)
            {
                ImageView imageV = new ImageView(this);                
                imageV.SetScaleType(ImageView.ScaleType.FitCenter);                

                float imgWidth = (0.15f * imgHome.Width);
                float imgHeight = (0.15f * imgHome.Height);
                imageV.LayoutParameters = new FrameLayout.LayoutParams((int)imgWidth, (int)imgHeight);

                float x = (item.X * imgHome.Width);
                float y = (item.Y * imgHome.Height);

                imageV.PivotX = 0.5f;
                imageV.PivotY = 0.5f;
                imageV.SetX(x);
                imageV.SetY(y);
                imageV.Tag = item.Name;
                imageV.Click += ImageV_Click;

                flContainer.AddView(imageV);

                treatLampState(imageV, item);
            }
        }        
        private void treatLampState(ImageView img, ItemUI ui)
        {
            if (ui.IsOn)
                img.SetImageResource(Resource.Drawable.lamp_on);
            else
                img.SetImageResource(Resource.Drawable.lamp_off);
        }
        private void ImageV_Click(object sender, System.EventArgs e)
        {
            ImageView img = (ImageView)sender;
            var itemUI = itemUIList.FirstOrDefault(s => s.Name == img.Tag.ToString());
            itemUI.IsOn = !itemUI.IsOn;
            treatLampState(img, itemUI);
        }
    }
}

