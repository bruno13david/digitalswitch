using Android.App;
using Android.OS;
using System.Collections.Generic;
using Android.Widget;
using System.Linq;

namespace DigitalSwitchClient
{
    [Activity(Label = "DigitalSwitchClient", MainLauncher = true)]
    public class MainActivity : Activity
    {
        ImageView imgLed1;
        ImageView imgLed2;
        ImageView imgLed3;
        ImageView imgLed4;
        List<ItemUI> itemUIList;
        MqttConnection mqttConnection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);            
            Title = Resources.GetString(Resource.String.lbl_home);

            imgLed1 = FindViewById<ImageView>(Resource.Id.imgLed1);
            imgLed2 = FindViewById<ImageView>(Resource.Id.imgLed2);
            imgLed3 = FindViewById<ImageView>(Resource.Id.imgLed3);
            imgLed4 = FindViewById<ImageView>(Resource.Id.imgLed4);

            loadItemUIList();

            mqttConnection = new MqttConnection();
            mqttConnection.Connect();
        }
        
        private void loadItemUIList()
        {
            itemUIList = new List<ItemUI>();
            itemUIList.Add(new ItemUI("led", false, 1, EnumItemUIType.Led, imgLed1));
            itemUIList.Add(new ItemUI("led", false, 2, EnumItemUIType.Led, imgLed2));
            itemUIList.Add(new ItemUI("led", false, 3, EnumItemUIType.Led, imgLed3));
            itemUIList.Add(new ItemUI("led", false, 4, EnumItemUIType.Led, imgLed4));           

            foreach (var item in itemUIList)
            {
                treatItemUIState(item);
                item.Image.Click += ImageV_Click;
            }
        }
        private void treatItemUIState(ItemUI itemUI)
        {
            if (itemUI.Type == EnumItemUIType.Led)            
                treatItemUITypeLedState(itemUI.Image, itemUI);            
        }
        private static void treatItemUITypeLedState(ImageView img, ItemUI itemUI)
        {
            if (itemUI.IsOn)
                img.SetImageResource(Resource.Drawable.lamp_on);
            else
                img.SetImageResource(Resource.Drawable.lamp_off);
        }

        private void ImageV_Click(object sender, System.EventArgs e)
        {
            ImageView img = (ImageView)sender;
            var itemUI = itemUIList.FirstOrDefault(i => i.Id == img.Id);
            treatSetLedStatus(itemUI);
            treatItemUIState(itemUI);
        }
        private void treatSetLedStatus(ItemUI itemUI)
        {
            var result = mqttConnection.Publish(itemUI);

            if (result)
            {
                Toast.MakeText(this, "Comando enviado!", ToastLength.Long).Show();
                itemUI.IsOn = !itemUI.IsOn;
            }
            else
            {
                Toast.MakeText(this, "Ocorreu um erro ao enviar o comando!", ToastLength.Long).Show();
            }            
        }        
    }
}

