using System;
using System.Windows.Media.Imaging;

namespace UP_Markov.Helpers
{
    public static class ImageHelper
    {
        public static BitmapImage LoadImage(
            string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    return null;
                }

                return new BitmapImage(
                    new Uri(path,
                    UriKind.RelativeOrAbsolute));
            }

            catch
            {
                return null;
            }
        }
    }
}