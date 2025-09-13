using Godot;
using System;
using System.Collections.Generic;

namespace starcraftbuildtrainer.scripts
{
    public partial class WorkerCommandCard : Container
    {
        public Vector2 ButtonSize { get; set; }

        private GridContainer _rootMenu;
        private GridContainer _basicBuildingMenu;
        private GridContainer _advancedBuildingMenu;
        private Dictionary<CommandCardType, GridContainer> _commandCards;

        private GridContainer _currentCommandCard;

        private const int GRID_COLUMNS = 3;

        public void Init(WorkerActivityControlData data)
        {
            _rootMenu = AddGridContainer();
            _basicBuildingMenu = AddGridContainer();
            _advancedBuildingMenu = AddGridContainer();
            _commandCards = new()
            {
                { CommandCardType.Root, _rootMenu },
                { CommandCardType.Basic, _basicBuildingMenu },
                { CommandCardType.Advanced, _advancedBuildingMenu },
            };

            _currentCommandCard = _rootMenu;
            _currentCommandCard.Show();

            foreach (var buttonData in data.CommandButtonData)
            {
                ProductionButton button = ProductionButton.Instantiate(buttonData, ButtonSize);
                button.Selected += OnCommandButtonSelected;
                _commandCards[buttonData.Menu].AddChild(button);
            }

            GridContainer AddGridContainer()
            {
                var grid = new GridContainer { Columns = GRID_COLUMNS, Visible = false };
                AddChild(grid);
                return grid;
            }
        }

        public void Open()
        {
            Show();
        }

        public void Close()
        {
            Hide();
            SelectCommandCardType(CommandCardType.Root);
        }

        private void OnCommandButtonSelected(ProductionButton button)
        {
            switch (button.Type)
            {
                case ProductionButtonType.GoToMinerals:
                    //TODO implement
                    break;
                case ProductionButtonType.GoToGas:
                    //TODO implement
                    break;
                case ProductionButtonType.BasicBuilding:
                    SelectCommandCardType(CommandCardType.Basic);
                    break;
                case ProductionButtonType.AdvancedBuilding:
                    SelectCommandCardType(CommandCardType.Advanced);
                    //TODO implement
                    break;
                case ProductionButtonType.SupplyDepot:
                    //TODO build supply depot
                    break;
                case ProductionButtonType.Cancel:
                    SelectCommandCardType(CommandCardType.Root);
                    break;
                case ProductionButtonType.None: 
                default:
                    break;
            }
        }

        private void SelectCommandCardType(CommandCardType type)
        {
            _currentCommandCard.Hide();
            _currentCommandCard = _commandCards[type];
            _currentCommandCard.Show();
        }
    }
}