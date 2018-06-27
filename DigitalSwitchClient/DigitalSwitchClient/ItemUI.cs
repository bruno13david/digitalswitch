using Android.Widget;

namespace DigitalSwitchClient
{
    public enum EnumItemUIType
    {
        Led = 1
    }

    public class ItemUI
    {
        public ItemUI(string topic, bool isOn, int portNumber, EnumItemUIType type, ImageView image)
        {
            Id = image.Id;
            Topic = topic;
            IsOn = isOn;
            PortNumber = portNumber;
            Type = type;
            Image = image;
        }

        public int Id { get; set; }
        public string Topic { get; set; }
        public bool IsOn { get; set; }
        public int PortNumber { get; set; }
        public EnumItemUIType Type { get; set; }
        public ImageView Image { get; set; }

        public string GetCommand()
        {
            switch (Type)
            {
                case EnumItemUIType.Led:
                    return $"{PortNumber}:{IsOn}";
                default:
                    return string.Empty;
            }
        }
    }
}