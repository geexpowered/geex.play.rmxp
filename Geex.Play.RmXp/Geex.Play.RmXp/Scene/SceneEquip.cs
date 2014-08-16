using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Window;
using Geex.Run;

namespace Geex.Play.Rpg.Scene
{
    /// <summary>
    /// This window displays buyable goods on the shop screen.
    /// </summary>
    public partial class SceneEquip : SceneBase
    {
        #region Variables

        /// <summary>
        /// Equip index
        /// </summary>
        int equipIndex;

        /// <summary>
        /// Actor index
        /// </summary
        int actorIndex;

        /// <summary>
        /// Equip window actor
        /// </summary>
        GameActor actor;

        /// <summary>
        /// Help Window
        /// </summary>
        WindowHelp helpWindow;

        /// <summary>
        /// Parameters changes window
        /// </summary>
        WindowEquipLeft leftWindow;

        /// <summary>
        /// Equipment window
        /// </summary>
        WindowEquipRight rightWindow;

        /// <summary>
        /// Current item window
        /// </summary>
        WindowEquipItem itemWindow;

        /// <summary>
        /// Item window 1
        /// </summary>
        WindowEquipItem weaponWindow;

        /// <summary>
        /// Item window 2
        /// </summary>
        WindowEquipItem shieldWindow;

        /// <summary>
        /// Item window 3
        /// </summary>
        WindowEquipItem headWindow;

        /// <summary>
        /// Item window 4
        /// </summary>
        WindowEquipItem bodyWindow;

        /// <summary>
        /// Item window 5
        /// </summary>
        WindowEquipItem accessoryWindow;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor for update_right
        /// </summary>
        /// <param Name="actor_index">actor index</param>
        /// <param Name="equip_index">equip item index</param>
        public SceneEquip(int _index, int equipIndex)
            : base()
        {
            actorIndex = _index;
            equipIndex = equipIndex;
        }

        /// <summary>
        /// Constructor for SceneMenu
        /// </summary>
        /// <param Name="_index">actor index</param>
        public SceneEquip(int _index)
            : base()
        {
            actorIndex = _index;
        }

        /// <summary>
        /// Initialize (default : actor_index = 0, equip_index = 0)
        /// </summary>
        public override void LoadSceneContent()
        {
            Initialize(actorIndex, equipIndex);
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param Name="actor_index">actor index</param>
        /// <param Name="equip_index">equip item index</param>
        public void Initialize(int actor_index, int equip_index)
        {
            this.actorIndex = actor_index;
            this.equipIndex = equip_index;
            // Get actor
            actor = InGame.Party.Actors[this.actorIndex];
            //Initialize windows
            InitializeWindows();
        }

        /// <summary>
        /// Windows initialization
        /// </summary>
        void InitializeWindows()
        {
            helpWindow = new WindowHelp();
            leftWindow = new WindowEquipLeft(actor);
            rightWindow = new WindowEquipRight(actor);
            weaponWindow = new WindowEquipItem(actor, 0);
            shieldWindow = new WindowEquipItem(actor, 1);
            headWindow = new WindowEquipItem(actor, 2);
            bodyWindow = new WindowEquipItem(actor, 3);
            accessoryWindow = new WindowEquipItem(actor, 4);
            // Associate help window
            rightWindow.HelpWindow = helpWindow;
            weaponWindow.HelpWindow = helpWindow;
            shieldWindow.HelpWindow = helpWindow;
            headWindow.HelpWindow = helpWindow;
            bodyWindow.HelpWindow = helpWindow;
            accessoryWindow.HelpWindow = helpWindow;
            // Set cursor position
            rightWindow.Index = equipIndex;
            Refresh();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            // Prepare for Transition
            //Graphics.Freeze();
            // Dispose of windows
            helpWindow.Dispose();
            leftWindow.Dispose();
            rightWindow.Dispose();
            weaponWindow.Dispose();
            shieldWindow.Dispose();
            headWindow.Dispose();
            bodyWindow.Dispose();
            accessoryWindow.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh
        /// </summary>
        public void Refresh()
        {
            // Set item window to visible
            weaponWindow.IsVisible = (rightWindow.Index == 0);
            shieldWindow.IsVisible = (rightWindow.Index == 1);
            headWindow.IsVisible = (rightWindow.Index == 2);
            bodyWindow.IsVisible = (rightWindow.Index == 3);
            accessoryWindow.IsVisible = (rightWindow.Index == 4);
            // Get currently equipped item
            Carriable _item1 = rightWindow.Item;
            // Set current item window to item_window
            switch (rightWindow.Index)
            {
                case 0:
                    itemWindow = weaponWindow;
                    break;
                case 1:
                    itemWindow = shieldWindow;
                    break;
                case 2:
                    itemWindow = headWindow;
                    break;
                case 3:
                    itemWindow = bodyWindow;
                    break;
                case 4:
                    itemWindow = accessoryWindow;
                    break;
            }
            // If right window is active
            if (rightWindow.IsActive)
            {
                // Erase parameters for after equipment change
                leftWindow.SetNewParameters(null, null, null);
            }
            // If item window is active
            if (itemWindow.IsActive)
            {
                // Get currently selected item
                Carriable _item2 = itemWindow.Item;
                // Change equipment
                int last_hp = actor.Hp;
                int last_sp = actor.Sp;
                actor.Equip(rightWindow.Index, _item2 == null ? 0 : _item2.Id);
                // Get parameters for after equipment change
                int _new_atk = actor.Atk;
                int _new_pdef = actor.Pdef;
                int _new_mdef = actor.Mdef;
                // Return equipment
                actor.Equip(rightWindow.Index, _item1 == null ? 0 : _item1.Id);
                actor.Hp = last_hp;
                actor.Sp = last_sp;
                // Draw in left window
                leftWindow.SetNewParameters(_new_atk, _new_pdef, _new_mdef);
            }
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        public override void Update()
        {
            // Update windows
            leftWindow.Update();
            rightWindow.Update();
            itemWindow.Update();
            Refresh();
            // If right window is active: call update_right
            if (rightWindow.IsActive)
            {
                UpdateRight();
                return;
            }
            // If item window is active: call update_item
            else if (itemWindow.IsActive)
            {
                UpdateItem();
                return;
            }
        }

        /// <summary>
        /// Frame Update (when right window is active)
        /// </summary>
        void UpdateRight()
          {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // Switch to menu screen
                Main.Scene = new SceneMenu(2);
              return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
              // If equipment is fixed
              if (actor.IsEquipFix(rightWindow.Index))
              {
                // Play buzzer SE
                InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                return;
                }
              // Play decision SE
              InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
              // Activate item window
              rightWindow.IsActive = false;
              itemWindow.IsActive = true;
              itemWindow.Index = 0;
              return;
              }
            // If R button was pressed
            if (Input.RMTrigger.R)
            {
              // Play cursor SE
              InGame.System.SoundPlay(Data.System.CursorSoundEffect);
              // To next actor
              actorIndex += 1;
              actorIndex %= InGame.Party.Actors.Count;
              // Switch to different equipment screen
              Main.Scene = new SceneEquip(actorIndex, rightWindow.Index);
              return;
              }
            // If L button was pressed
            if (Input.RMTrigger.L)
            {
              // Play cursor SE
              InGame.System.SoundPlay(Data.System.CursorSoundEffect);
              // To previous actor
              actorIndex += InGame.Party.Actors.Count - 1;
              actorIndex %= InGame.Party.Actors.Count;
              // Switch to different equipment screen
              Main.Scene = new SceneEquip(actorIndex, rightWindow.Index);
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
                // Activate right window
                rightWindow.IsActive = true;
                itemWindow.IsActive = false;
                itemWindow.Index = -1;
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Play equip SE
                InGame.System.SoundPlay(Data.System.EquipSoundEffect);
                // Get currently selected data on the item window
                Carriable _item = itemWindow.Item;
                // Change equipment
                actor.Equip(rightWindow.Index, _item == null ? 0 : _item.Id);
                // Activate right window
                rightWindow.IsActive = true;
                itemWindow.IsActive = false;
                itemWindow.Index = -1;
                // Remake right window and item window contents
                rightWindow.Refresh();
                itemWindow.Refresh();
                return;
            }
        }

        #endregion
    }
}
