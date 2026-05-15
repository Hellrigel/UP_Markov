using System;
using System.IO;
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
                    path =
                        "Resources/Covers/default.jpg";
                }

                string fullPath =
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        path);

                return new BitmapImage(
                    new Uri(fullPath));
            }

            catch
            {
                return new BitmapImage(
                    new Uri(
                        Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            "Resources/Covers/default.jpg")));
            }
        }
    }
}