namespace BeautifyMe.Areas.Payment.Data
{
    public class CardInfo
    {
        public string CardId { get; set; }  
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string Expiry { get; set; }
        public string SecurityCode { get; set; }
        public string MaskedCardNumber { get; set; }
    }
}
