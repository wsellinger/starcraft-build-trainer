namespace starcraftbuildtrainer.scripts
{
    public record ProductionButtonData(ProductionButtonType Type, CommandCardType Menu, string TexturePath)
    {
        //Statics

        //Root

        public static readonly ProductionButtonData BasicBuilding = new(
            ProductionButtonType.BasicBuilding,
            CommandCardType.Root,
            BASIC_BUILDING_TEXTURE_PATH);

        public static readonly ProductionButtonData AdvancedBuilding = new(
            ProductionButtonType.AdvancedBuilding,
            CommandCardType.Root,
            ADVANCED_BUILDING_TEXTURE_PATH);

        //Basic

        public static readonly ProductionButtonData SupplyDepot = new(
            ProductionButtonType.SupplyDepot, 
            CommandCardType.Basic, 
            SUPPLY_DEPOT_TEXTURE_PATH);

        public static readonly ProductionButtonData CancelBasic = new(
            ProductionButtonType.Cancel,
            CommandCardType.Basic,
            CANCEL_TEXTURE_PATH);

        //Advanced

        public static readonly ProductionButtonData Factory = new(
            ProductionButtonType.Factory,
            CommandCardType.Advanced,
            FACTORY_TEXTURE_PATH);

        public static readonly ProductionButtonData CancelAdvaned = new(
            ProductionButtonType.Cancel,
            CommandCardType.Advanced,
            CANCEL_TEXTURE_PATH);

        public static readonly ProductionButtonData[] BuildingMenu = [
            //Root
            BasicBuilding, AdvancedBuilding,
            //Basic
            SupplyDepot, CancelBasic,
            //Advanced
            Factory, CancelAdvaned,
        ];

        //Paths

        private const string CANCEL_TEXTURE_PATH = "res://assets/art/shared/cancel_button.png";
        private const string BASIC_BUILDING_TEXTURE_PATH = "res://assets/art/terran/menu/basicBuilding.png";
        private const string ADVANCED_BUILDING_TEXTURE_PATH = "res://assets/art/terran/menu/advancedBuilding.png";
        private const string SUPPLY_DEPOT_TEXTURE_PATH = "res://assets/art/terran/buildings/supplyDepot.png";
        private const string FACTORY_TEXTURE_PATH = "res://assets/art/terran/buildings/factory.png";
    }

    public enum CommandCardType
    {
        None,
        Root,
        Basic,
        Advanced,
    }

    public enum ProductionButtonType
    {
        None,
        Cancel,

        //Root
        GoToMinerals,
        GoToGas,

        BasicBuilding,
        AdvancedBuilding,

        //Basic
        SupplyDepot,

        //Advanced
        Factory,
    }
}
