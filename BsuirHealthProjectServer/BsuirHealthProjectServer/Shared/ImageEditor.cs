using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web;

namespace BsuirHealthProjectServer.Shared
{
    public class ImageEditor
    {
        public static byte[] GetResizedImage(HttpPostedFileBase image, int width, int height)
        {
            if (image == null)
                return null;
            Bitmap oldImage = new Bitmap(image.InputStream);
            Bitmap newImage = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(oldImage, new Rectangle(0, 0, width, height));
            }
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(newImage, typeof(byte[]));
        }
    }
}
