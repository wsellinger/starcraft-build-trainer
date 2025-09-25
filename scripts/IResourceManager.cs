namespace starcraftbuildtrainer.scripts
{
    public interface IResourceManager
    {
        public bool MakePayment(ResourceCost cost);
        public void AddSupplyTotal(uint value);
    }
}
