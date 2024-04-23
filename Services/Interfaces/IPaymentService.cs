using BeautifyMe.BeautifyMeDbModels;

namespace BeautifyMe.Services.Interfaces
{
    public interface IPaymentService
    {
        public Card AddCard(Card card);
        public List<Card> GetSavedCards(string emailId);
        public void RemoveCard(Card card);
        public bool ValidateCard(int cardId, string encryptedSecurityCode);
    }
}
