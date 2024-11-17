namespace CapaEntidad.Paypal
{
    public class Response_Paypal<T>
    {
        public bool Status { get; set; }
        public T Response { get; set; }

    }
}
