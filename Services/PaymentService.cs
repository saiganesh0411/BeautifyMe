using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Services.Interfaces;

namespace BeautifyMe.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly BeautifyMeContext _context;
        public PaymentService(BeautifyMeContext context)
        {
            _context = context;
        }
        public Card AddCard(Card card)
        {
            _context.Cards.Add(card);
            _context.SaveChanges();
            return card;
        }

        public List<Card> GetSavedCards(string emailId)
        {
            return _context.Cards.Where(card => card.EmailId == emailId && card.IsAcive == true).ToList();
        }

        public void RemoveCard(Card card)
        {
            _context.Cards.Remove(card);
            _context.SaveChanges();
        }

        public bool ValidateCard(int cardId, string encryptedSecurityCode)
        {
            return _context.Cards.Where(c => c.Id == cardId && c.IsAcive == true && c.EncryptedSecurityCode == encryptedSecurityCode).Any();
        }
    }
}
