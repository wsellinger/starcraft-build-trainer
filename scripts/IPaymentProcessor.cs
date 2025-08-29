namespace starcraftbuildtrainer.scripts
{
    public interface IPaymentProcessor
    {
        public bool MakePayment(ResourceCost cost);
    }
}
