namespace starcraftbuildtrainer.scripts
{
    public record ProductionButtonData(ProductionButtonType Type, string TexturePath)
    {
        //Statics

        public static readonly ProductionButtonData BasicBuilding = new(ProductionButtonType.BasicBuilding, BASIC_BUILDING_TEXTURE_PATH);

        //Paths

        private const string BASIC_BUILDING_TEXTURE_PATH = "res://assets/art/terran/menu/basicBuilding.png";
    }

    public enum ProductionButtonType
    {
        None,
        GoToMinerals,
        GoToGas,

        BasicBuilding,
        AdvancedBuilding,

        SupplyDepot
    }
}
