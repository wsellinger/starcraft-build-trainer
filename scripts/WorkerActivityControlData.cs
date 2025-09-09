namespace starcraftbuildtrainer.scripts
{
    public record WorkerActivityControlData(string ActivityTexturePath, ProductionButtonData[] CommandButtonData)
    {
        //Statics

        public static readonly WorkerActivityControlData Construction = new(CONSTRUCTION_TEXTURE_PATH, []);
        public static readonly WorkerActivityControlData Minerals = new(MINERAL_TEXTURE_PATH, [ProductionButtonData.BasicBuilding]);
        public static readonly WorkerActivityControlData Gas = new(GAS_TEXTURE_PATH, [ProductionButtonData.BasicBuilding]);

        //Paths

        private const string CONSTRUCTION_TEXTURE_PATH = "res://assets/art/terran/menu/workerBuildingIcon.png";
        private const string MINERAL_TEXTURE_PATH = "res://assets/art/shared/mineralFields.png";
        private const string GAS_TEXTURE_PATH = "res://assets/art/shared/vespeneGeyser.png";
    }
}