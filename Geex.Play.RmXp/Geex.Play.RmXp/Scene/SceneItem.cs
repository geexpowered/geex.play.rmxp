using System;
using System.Collections.Generic;
using System.Text;
using Geex.Run;
using Geex.Play.Rpg.Window;
using Geex.Play.Rpg.Game;
using Geex.Edit;

namespace Geex.Play.Rpg.Scene
{
    /// <summary>
    /// This class performs item screen processing.
    /// </summary>
    public partial class SceneItem : SceneBase
    {
        #region Variables

        /// <summary>
        /// Managed help window
        /// </summary>
        WindowHelp helpWindow;

        /// <summary>
        /// Manages item window
        /// </summary>
        WindowItem itemWindow;

        /// <summary>
        /// Managed target window
        /// </summary>
        WindowTarget targetWindow;

        /// <summary>
        /// Current item
        /// </summary>
        Item item;

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
        /// Windows initialization
        /// </summary>
        void InitializeWindows()
        {
            // Make help window, item window
            helpWindow = new WindowHelp();
            itemWindow = new WindowItem();
            // Associate help window
            itemWindow.HelpWindow = helpWindow;
            // Make target window (set to invisible / inactive)
            targetWindow = new WindowTarget();
            targetWindow.IsVisible = false;
            targetWindow.IsActive = false;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            // Prepare for transition
            //Graphics.Freeze();
            // Dispose of windows
            helpWindow.Dispose();
            itemWindow.Dispose();
            targetWindow.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Frame update
        /// </summary>
        public override void Update()
        {
            // Update windows
            helpWindow.Update();
            itemWindow.Update();
            targetWindow.Update();
            // If item window is active: call update_item
            if (itemWindow.IsActive)
            {
                UpdateItem();
                return;
            }
            // If target window is active: call update_target
            else if (targetWindow.IsActive)
            {
                UpdateTarget();
                return;
            }
        }

        /// <summary>
        /// Frame Update (when item window is active)
        /// </summary>
        void UpdateItem()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // Switch to menu screen
                Main.Scene = new SceneMenu(0);
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Get currently selected data on the item window
                Carriable selected_item = itemWindow.Item;
                // If not a use item
                if (selected_item!=null && !(selected_item.GetType().Name == "Item"))
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                // Cast the Carriable into Item type
                this.item = (Item)selected_item;
                if (item==null) return;
                // If it can't be used
                if (!InGame.Party.IsItemCanUse(item.Id))
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                // Play decision SE
                InGame.System.SoundPlay(Data.System.DecisionSoundEffect);

                // If effect scope is an ally
                if (item.Scope >= 3)
                {
                    // Activate target window
                    itemWindow.IsActive = false;
                    targetWindow.X = (itemWindow.Index + 1) % 2 * 304 * GeexEdit.GameWindowWidth / 640;
                    targetWindow.IsVisible = true;
                    targetWindow.IsActive = true;
                    // Set cursor position to effect scope (single / all)
                    if (item.Scope == 4 || item.Scope == 6)
                    {
                        targetWindow.Index = -1;
                    }
                    else
                    {
                        targetWindow.Index = 0;
                    }
                }
                // If effect scope is other than an ally
                else
                {
                    // If command event ID is valid
                    if (item.CommonEventId > 0)
                    {
                        // Command event call reservation
                        InGame.Temp.CommonEventId = item.CommonEventId;
                        // Play item use SE
                        InGame.System.SoundPlay(item.MenuSoundEffect);
                        // If consumable
                        if (item.Consumable)
                        {
                            // Decrease used items by 1
                            InGame.Party.LoseItem(item.Id, 1);
                            // Draw item window item
                            itemWindow.DrawItem(itemWindow.Index);
                        }
                        // Switch to map screen
                        Main.Scene = new SceneMap();
                        return;
                    }
                }
                return;
            }
        }

        /// <summary>
        /// Frame Update (when target window is active)
        /// </summary>
        void UpdateTarget()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // If unable to use because items ran out
                if (!InGame.Party.IsItemCanUse(item.Id))
                {
                    // Remake item window contents
                    itemWindow.Refresh();
                }
                // Erase target window
                itemWindow.IsActive = true;
                targetWindow.IsVisible = false;
                targetWindow.IsActive = false;
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // If items are used up
                if (InGame.Party.ItemNumber(item.Id) == 0)
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                bool _used = false;
                // If target is all
                if (targetWindow.Index == -1)
                {
                    // Apply item effects to entire party
                    foreach (GameActor actor in InGame.Party.Actors)
                    {
                        _used |= actor.ItemEffect(item);
                    }
                }
                // If single target
                if (targetWindow.Index >= 0)
                {
                    // Apply item use effects to target actor
                    GameActor target = InGame.Party.Actors[targetWindow.Index];
                    _used = target.ItemEffect(item);
                }
                // If an item was used
                if (_used)
                {
                    // Play item use SE
                    InGame.System.SoundPlay(item.MenuSoundEffect);
                    // If consumable
                    if (item.Consumable)
                    {
                        // Decrease used items by 1
                        InGame.Party.LoseItem(item.Id, 1);
                        // Redraw item window item
                        itemWindow.DrawItem(itemWindow.Index);
                    }
                    // Remake target window contents
                    targetWindow.Refresh();
                    // If all party members are dead
                    if (InGame.Party.IsAllDead)
                    {
                        // Switch to game over screen
                        Main.Scene = new SceneGameover();
                        return;
                    }
                    // If common event ID is valid
                    if (item.CommonEventId > 0)
                    {
                        // Common event call reservation
                        InGame.Temp.CommonEventId = item.CommonEventId;
                        // Switch to map screen
                        Main.Scene = new SceneMap();
                        return;
                    }
                }
                // If item wasn't used
                if (!_used)
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                }
                return;
            }
        }

        #endregion
    }
}
