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
using Android.Views.Animations;

namespace DigitalSwitchClient
{
    [Activity(Label = "DigitalSwitchClient", MainLauncher = true)]
    public class MainActivity : Activity
    {
        List<ItemUI> itemUIList;
        TextView txtHome;
        FrameLayout flContainer;
        MqttConnection mqttConnection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            txtHome = FindViewById<TextView>(Resource.Id.txtHome);
            flContainer = FindViewById<FrameLayout>(Resource.Id.flContainer);

            Title = Resources.GetString(Resource.String.lbl_home);

            readItemsConfig();
            mqttConnection = new MqttConnection();
            
            Task.Factory.StartNew(() =>
            {
                Task.Delay(1000).Wait();
                RunOnUiThread(renderItemUIList);
                mqttConnection.Connect(null);
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

                float imgWidth = (0.109f * txtHome.Width);
                float imgHeight = (0.113f * txtHome.Height);
                imageV.LayoutParameters = new FrameLayout.LayoutParams((int)imgWidth, (int)imgHeight);

                float x = (item.X * txtHome.Width);
                float y = (item.Y * txtHome.Height);

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
            treatSetLedStatus(itemUI);
        }
        private void treatSetLedStatus(ItemUI itemUI)
        {
            int command;
            if (int.TryParse(itemUI.Command, out command))
            {
                mqttConnection.SetLedStatus(getLedEnum(command), itemUI.IsOn, (result) =>
                {
                    if (result)
                    {
                        Toast.MakeText(this, "Comando enviado!", ToastLength.Long).Show();
                        itemUI.IsOn = !itemUI.IsOn;
                    }
                    else
                    {
                        Toast.MakeText(this, "Ocorreu um erro ao enviar o comando!", ToastLength.Long).Show();
                        itemUI.IsOn = !itemUI.IsOn;
                    }
                });
            }
        }

        private EnumLed getLedEnum(int command)
        {
            switch (command)
            {
                case 1:
                    return EnumLed.Led1;
                case 2:
                    return EnumLed.Led2;
                case 3:
                    return EnumLed.Led3;
                case 4:
                    return EnumLed.Led4;
                default:
                    return EnumLed.Led1;
            }            
        }
    }
}

