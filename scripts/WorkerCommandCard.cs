using Godot;
using System;
using System.Collections.Generic;

namespace starcraftbuildtrainer.scripts
{
    public partial class WorkerCommandCard : Container
    {
        //Events

        public event Action<ActionButtonData> ActionSelected;

        //Properties

        public Vector2 ButtonSize { get; set; }

        private GridContainer _rootMenu;
        private GridContainer _basicBuildingMenu;
        private GridContainer _advancedBuildingMenu;
        private Dictionary<MenuType, GridContainer> _commandCards;

        private GridContainer _currentCommandCard;

        private const int GRID_COLUMNS = 3;

        public void Init(WorkerActivityControlData data)
        {
            _rootMenu = AddGridContainer();
            _basicBuildingMenu = AddGridContainer();
            _advancedBuildingMenu = AddGridContainer();
            _commandCards = new()
            {
                { MenuType.Root, _rootMenu },
                { MenuType.Basic, _basicBuildingMenu },
                { MenuType.Advanced, _advancedBuildingMenu },
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
            SelectCommandCardType(MenuType.Root);
        }

        private void OnCommandButtonSelected(ProductionButton button)
        {
            switch (button.Data)
            {
                case MenuButtonData menuButtonData:
                    SelectCommandCardType(menuButtonData.NextMenu);
                    break;
                case ActionButtonData actionButtonData:
                    ActionSelected.Invoke(actionButtonData);
                    break;
                default:
                    break;
            }
        }

        private void SelectCommandCardType(MenuType type)
        {
            _currentCommandCard.Hide();
            _currentCommandCard = _commandCards[type];
            _currentCommandCard.Show();
        }
    }
}