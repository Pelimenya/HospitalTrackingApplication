using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HospitalTrackingApplication.Models
{
    class PersonLocations
    {
        public Response[] response { get; set; }
    }

    public class Response
    {
        public int personCode { get; set; }
        public string personRole { get; set; }
        public int lastSecurityPointNumber { get; set; }
        public string lastSecurityPointDirection { get; set; }
        public string lastSecurityPointTime { get; set; }

        public SolidColorBrush EliceColor
        {
            get
            {
                var brush = Brushes.Transparent;

                if (personRole == "Клиент" && lastSecurityPointDirection == "in")
                {
                    brush = Brushes.Green;
                }
                else if (personRole == "Сотрудник" && lastSecurityPointDirection == "in")
                {
                    brush = Brushes.Blue;
                }
                return brush;
            }
        }
    }
}
