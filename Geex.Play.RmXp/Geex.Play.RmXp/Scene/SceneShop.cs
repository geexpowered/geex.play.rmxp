using System;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Window;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Scene
{
    /// <summary>
    /// This class performs shop screen processing.
    /// </summary>
    public partial class SceneShop : SceneBase
    {
        #region Variables

        /// <summary>
        /// Item which is beeing bought
        /// </summary>
        Carriable item;

        /// <summary>
        /// Managed help window
        /// </summary>
        WindowHelp helpWindow;

        /// <summary>
        /// Managed command window
        /// </summary>
        WindowShopCommand commandWindow;

        /// <summary>
        /// Managed gold window
        /// </summary>
        WindowGold goldWindow;

        /// <summary>
        /// Dummmy Window
        /// </summary>
        WindowBase dummyWindow;

        /// <summary>
        /// Managed buy window
        /// </summary>
        WindowShopBuy buyWindow;

        /// <summary>
        /// Managed sell window
        /// </summary>
        WindowShopSell sellWindow;

        /// <summary>
        /// Managed number window
        /// </summary>
        WindowShopNumber numberWindow;

        /// <summary>
        /// Managed status window
        /// </summary>
        WindowShopStatus statusWindow;

        #endregion

        #region Properties

        /// <summary>
        /// Disabled Main Command? Test
        /// </summary>
        bool IsDisabledMainCommand
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Initialize
        /// </summary>
        public override void LoadSceneContent()
        {
            InitializeWindows();
        }

        /// <summary>
        /// Initialize windows
        /// </summary>
        void InitializeWindows()
        {
            // Make help window
            helpWindow = new WindowHelp();
            // Make command window
            commandWindow = new WindowShopCommand();
            // Make gold window
            goldWindow = new WindowGold();
            goldWindow.X = 480 * GeexEdit.GameWindowWidth / 640;
            goldWindow.Y = 64;
            goldWindow.Width = goldWindow.Width * GeexEdit.GameWindowWidth / 640;
            // Make dummy window
            dummyWindow = new WindowBase(0, 128, GeexEdit.GameWindowWidth, GeexEdit.GameWindowHeight - 128);
            // Make buy window
            buyWindow = new WindowShopBuy(InGame.Temp.ShopGoods);
            buyWindow.IsActive = false;
            buyWindow.IsVisible = false;
            buyWindow.HelpWindow = helpWindow;
            // Make sell window
            sellWindow = new WindowShopSell();
            sellWindow.IsActive = false;
            sellWindow.IsVisible = false;
            sellWindow.HelpWindow = helpWindow;
            // Make quantity input window
            numberWindow = new WindowShopNumber();
            numberWindow.IsActive = false;
            numberWindow.IsVisible = false;
            // Make status window
            statusWindow = new WindowShopStatus();
            statusWindow.IsVisible = false;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            // Prepare for Transition
            Graphics.Freeze();
            // Dispose of windows
            helpWindow.Dispose();
            commandWindow.Dispose();
            goldWindow.Dispose();
            dummyWindow.Dispose();
            buyWindow.Dispose();
            sellWindow.Dispose();
            numberWindow.Dispose();
            statusWindow.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Frame Update
        /// </summary>
        public override void Update()
        {
            // Update windows
            helpWindow.Update();
            commandWindow.Update();
            goldWindow.Update();
            dummyWindow.Update();
            buyWindow.Update();
            sellWindow.Update();
            numberWindow.Update();
            statusWindow.Update();
            // If command window is active: call update_command
            if (commandWindow.IsActive)
            {
                UpdateCommand();
                return;
            }
            // If buy window is active: call update_buy
            else if (buyWindow.IsActive)
            {
                UpdateBuy();
                return;
            }
            // If sell window is active: call update_sell
            else if (sellWindow.IsActive)
            {
                UpdateSell();
                return;
            }
            // If quantity input window is active: call update_number
            else if (numberWindow.IsActive)
            {
                UpdateNumber();
                return;
            }
        }

        /// <summary>
        /// Frame Update (when command window is active)
        /// </summary>
        void UpdateCommand()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // Switch to map screen
                Main.Scene = new SceneMap();
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Return if Disabled Command
                if (IsDisabledMainCommand)
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                MainCommandInput();
                return;
            }
        }

        /// <summary>
        /// Frame Update (when buy window is active)
        /// </summary>
        void UpdateBuy()
        {
            // Set status window item
            statusWindow.Item = buyWindow.Item;
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // Change windows to initial mode
                commandWindow.IsActive = true;
                dummyWindow.IsVisible = true;
                buyWindow.IsActive = false;
                buyWindow.IsVisible = false;
                statusWindow.IsVisible = false;
                statusWindow.Item = null;
                // Erase help text
                helpWindow.SetText("");
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Get item
                item = buyWindow.Item;
                // If item is invalid, or price is higher than money possessed
                if (item == null || item.Price > InGame.Party.Gold)
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                // Get items in possession count
                int _number = 0;
                switch (item.GetType().Name)
                {
                    case "Item":
                        _number = InGame.Party.ItemNumber(item.Id);
                        break;
                    case "Weapon":
                        _number = InGame.Party.WeaponNumber(item.Id);
                        break;
                    case "Armor":
                        _number = InGame.Party.ArmorNumber(item.Id);
                        break;
                }
                // If 99 items are already in possession
                if (_number == 99)
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                // Play decision SE
                InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                // Calculate maximum amount possible to buy
                int _max = item.Price == 0 ? 99 : InGame.Party.Gold / item.Price;
                _max = Math.Min(_max, 99 - _number);
                // Change windows to quantity input mode
                buyWindow.IsActive = false;
                buyWindow.IsVisible = false;
                numberWindow.Set(item, _max, item.Price);
                numberWindow.IsActive = true;
                numberWindow.IsVisible = true;
            }
        }

        /// <summary>
        /// Frame Update (when sell window is active)
        /// </summary>
        void UpdateSell()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // Change windows to initial mode
                commandWindow.IsActive = true;
                dummyWindow.IsVisible = true;
                sellWindow.IsActive = false;
                sellWindow.IsVisible = false;
                statusWindow.Item = null;
                // Erase help text
                helpWindow.SetText("");
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Get item
                item = sellWindow.Item;
                // Set status window item
                statusWindow.Item = item;
                // If item is invalid, or item price is 0 (unable to sell)
                if (item == null || item.Price == 0)
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                // Play decision SE
                InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                // Get items in possession count
                int _number = 0;
                switch (item.GetType().Name)
                {
                    case "Item":
                        _number = InGame.Party.ItemNumber(item.Id);
                        break;
                    case "Weapon":
                        _number = InGame.Party.WeaponNumber(item.Id);
                        break;
                    case "Armor":
                        _number = InGame.Party.ArmorNumber(item.Id);
                        break;
                }
                // Maximum quanitity to sell = number of items in possession
                int _max = _number;
                // Change windows to quantity input mode
                sellWindow.IsActive = false;
                sellWindow.IsVisible = false;
                numberWindow.Set(item, _max, item.Price / 2);
                numberWindow.IsActive = true;
                numberWindow.IsVisible = true;
                statusWindow.IsVisible = true;
            }
        }

        /// <summary>
        /// Frame Update (when quantity input window is active)
        /// </summary>
        void UpdateNumber()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // Set quantity input window to inactive / invisible
                numberWindow.IsActive = false;
                numberWindow.IsVisible = false;
                // Number Cancel Command
                NumberCancelCommandInput();
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Play shop SE
                InGame.System.SoundPlay(Data.System.ShopSoundEffect);
                // Set quantity input window to inactive / invisible
                numberWindow.IsActive = false;
                numberWindow.IsVisible = false;
                // Number Command Input
                NumberCommandInput();
                return;
            }
        }

        /// <summary>
        /// Main Command Input
        /// </summary>
        void MainCommandInput()
        {
            // Branch by command window cursor position
            switch (commandWindow.Index)
            {
                case 0:
                    CommandMainBuy();
                    break;
                case 1:
                    CommandMainSell();
                    break;
                case 2:
                    CommandExit();
                    break;
            }
        }

        /// <summary>
        /// Update Number Cancel Command
        /// </summary>
        void NumberCancelCommandInput()
        {
            // Branch by command window cursor position
            switch (commandWindow.Index)
            {
                case 0:  // buy
                    // Change windows to buy mode
                    buyWindow.IsActive = true;
                    buyWindow.IsVisible = true;
                    break;
                case 1:  // sell
                    // Change windows to sell mode
                    sellWindow.IsActive = true;
                    sellWindow.IsVisible = true;
                    statusWindow.IsVisible = false;
                    break;
            }
        }

        /// <summary>
        /// Update Number Command
        /// </summary>
        void NumberCommandInput()
        {
            switch (commandWindow.Index)
            {
                case 0:  // buy
                    CommandNumberBuy();
                    break;
                case 1:  // sell
                    CommandNumberSell();
                    break;
            }
        }

        /// <summary>
        /// Command : Main Buy
        /// </summary>
        void CommandMainBuy()
        {
            // Change windows to buy mode
            commandWindow.IsActive = false;
            dummyWindow.IsVisible = false;
            buyWindow.IsActive = true;
            buyWindow.IsVisible = true;
            buyWindow.Refresh();
            statusWindow.IsVisible = true;
        }

        /// <summary>
        /// Command : Main Sell
        /// </summary>
        void CommandMainSell()
        {
            // Change windows to sell mode
            commandWindow.IsActive = false;
            dummyWindow.IsVisible = false;
            sellWindow.IsActive = true;
            sellWindow.IsVisible = true;
            sellWindow.Refresh();
        }

        /// <summary>
        /// Command : Exit
        /// </summary>
        void CommandExit()
        {
            // Switch to map screen
            Main.Scene = new SceneMap();
        }

        /// <summary>
        /// Command : Number Buy
        /// </summary>
        void CommandNumberBuy()
        {
            // Buy process
            InGame.Party.LoseGold(numberWindow.Number * item.Price);
            switch (item.GetType().Name)
            {
                case "Item":
                    InGame.Party.GainItem(item.Id, numberWindow.Number);
                    break;
                case "Weapon":
                    InGame.Party.GainWeapon(item.Id, numberWindow.Number);
                    break;
                case "Armor":
                    InGame.Party.GainArmor(item.Id, numberWindow.Number);
                    break;
            }
            // Refresh each window
            goldWindow.Refresh();
            buyWindow.Refresh();
            statusWindow.Refresh();
            // Change windows to buy mode
            buyWindow.IsActive = true;
            buyWindow.IsVisible = true;
        }

        /// <summary>
        /// Command : Number Sell
        /// </summary>
        void CommandNumberSell()
        {
            // Sell process
            InGame.Party.GainGold(numberWindow.Number * (item.Price / 2));
            switch (item.GetType().Name)
            {
                case "Item":
                    InGame.Party.LoseItem(item.Id, numberWindow.Number);
                    break;
                case "Weapon":
                    InGame.Party.LoseWeapon(item.Id, numberWindow.Number);
                    break;
                case "Armor":
                    InGame.Party.LoseArmor(item.Id, numberWindow.Number);
                    break;
            }
            // Refresh each window
            goldWindow.Refresh();
            sellWindow.Refresh();
            statusWindow.Refresh();
            // Change windows to sell mode
            sellWindow.IsActive = true;
            sellWindow.IsVisible = true;
            statusWindow.IsVisible = false;
        }

        #endregion
    }
}
