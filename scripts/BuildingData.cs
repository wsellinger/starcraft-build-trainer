using System.Collections.Generic;

namespace starcraftbuildtrainer.scripts
{
    public enum BuildingIdentity
    {
        None,
        SupplyDepot,
        Factory,
    }

    public record BuildingData(
        BuildingIdentity Identity,
        int BuildTime,
        ResourceCost Cost,
        uint Supply,
        string PendingTexturePath,
        string CompleteTexturePath)
    {
        //Statics

        public static readonly BuildingData SupplyDepot = new(
            Identity: BuildingIdentity.SupplyDepot, 
            BuildTime: 21,
            Cost: new(
                Minerals: 100, 
                Gas: 0, 
                Supply: 0),
            Supply: 8,
            PendingTexturePath: TERRAN_PENDING_BUILDING_TEXTURE_PATH, 
            CompleteTexturePath: "res://assets/art/terran/buildings/supplyDepot.png");

        public static readonly Dictionary<BuildingIdentity, BuildingData> Map = new()
        {
            { BuildingIdentity.SupplyDepot, SupplyDepot }
        };

        //Paths

        private const string TERRAN_PENDING_BUILDING_TEXTURE_PATH = "res://assets/art/terran/buildings/pendingBuildingTerran.png";
    }
}
