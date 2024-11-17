using System.Collections.Generic;

namespace CapaEntidad.Paypal
{
    public class Response_checkout
    {
        public string id { get; set; }
        public string status { get; set; }
        public List<CC_Link> links { get; set; }

    }
    public class CC_Link
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }
}
