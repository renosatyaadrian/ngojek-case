using System;
using System.Globalization;

namespace OrderService.Helpers
{
    public static class MathHelper
    {
        public static float getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1);  // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
                ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return (float)d;
        }

        public static double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }

        public static string ToRupiah(double angka)
        {
            return String.Format(CultureInfo.CreateSpecificCulture("id-id"), "Rp. {0:N}", angka);
        }

        public static float DistanceRounding(float distance)
        {
            return (float)Math.Round(distance, MidpointRounding.AwayFromZero);
        }
    }
}
