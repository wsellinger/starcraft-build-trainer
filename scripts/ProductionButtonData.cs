namespace starcraftbuildtrainer.scripts
{
    public record ProductionButtonData(MenuType Menu, string TexturePath)
    {
        public static readonly ProductionButtonData[] BuildingMenu = [
            //Root
            MenuButtonData.BasicBuilding, MenuButtonData.AdvancedBuilding,
            //Basic
            BuildButtonData.SupplyDepot, MenuButtonData.CancelBasic,
            //Advanced
            BuildButtonData.Factory, MenuButtonData.CancelAdvaned,
        ];
    }

    public record MenuButtonData(MenuType Menu, string TexturePath, MenuType NextMenu) : ProductionButtonData(Menu, TexturePath)
    {
        //Statics

        public static readonly MenuButtonData BasicBuilding = new(MenuType.Root, BASIC_BUILDING_TEXTURE_PATH, MenuType.Basic);
        public static readonly MenuButtonData AdvancedBuilding = new(MenuType.Root, ADVANCED_BUILDING_TEXTURE_PATH, MenuType.Advanced);
        public static readonly MenuButtonData CancelBasic = new(MenuType.Basic, CANCEL_TEXTURE_PATH, MenuType.Root);
        public static readonly MenuButtonData CancelAdvaned = new(MenuType.Advanced, CANCEL_TEXTURE_PATH, MenuType.Root);

        //Paths

        private const string CANCEL_TEXTURE_PATH = "res://assets/art/shared/cancel_button.png";
        private const string BASIC_BUILDING_TEXTURE_PATH = "res://assets/art/terran/menu/basicBuilding.png";
        private const string ADVANCED_BUILDING_TEXTURE_PATH = "res://assets/art/terran/menu/advancedBuilding.png";
    }

    public record ActionButtonData(MenuType Menu, string TexturePath) : ProductionButtonData(Menu, TexturePath);

    public record GatherButtonData(MenuType Menu, string TexturePath, GatherType Type) : ActionButtonData(Menu, TexturePath)
    {
        //Statics

        public static readonly GatherButtonData Minerals = new(MenuType.Root, GATHER_MINERALS_TEXTURE_PATH, GatherType.Minerals);
        public static readonly GatherButtonData Gas = new(MenuType.Root, GATHER_GAS_TEXTURE_PATH, GatherType.Gas);

        //Paths

        private const string GATHER_MINERALS_TEXTURE_PATH = "res://assets/art/shared/gatherMinerals.png";
        private const string GATHER_GAS_TEXTURE_PATH = "res://assets/art/shared/gatherGas.png";

        public override string ToString() => $"Gather{Type}";
    }

    public record BuildButtonData(MenuType Menu, string TexturePath, BuildingType Type) : ActionButtonData(Menu, TexturePath)
    {
        //Statics

        public static readonly BuildButtonData SupplyDepot = new(MenuType.Basic, SUPPLY_DEPOT_TEXTURE_PATH, BuildingType.SupplyDepot);
        public static readonly BuildButtonData Factory = new(MenuType.Advanced, FACTORY_TEXTURE_PATH, BuildingType.Factory);
        
        //Paths
        
        private const string SUPPLY_DEPOT_TEXTURE_PATH = "res://assets/art/terran/buildings/supplyDepot.png";
        private const string FACTORY_TEXTURE_PATH = "res://assets/art/terran/buildings/factory.png";

        public override string ToString() => $"Build{Type}";
    }


    public enum MenuType
    {
        None,
        Root,
        Basic,
        Advanced,
    }

    public enum GatherType
    {
        None,
        Minerals,
        Gas,
    }

    public enum BuildingType
    {
        None,
        SupplyDepot,
        Factory,
    }
}
