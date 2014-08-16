using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Window;
using Geex.Run;

namespace Geex.Play.Rpg.Scene
{
    /// <summary>
    /// This class performs skill screen processing.
    /// </summary>
    public partial class SceneSkill : SceneBase
    {
        #region Variables

        /// <summary>
        /// Actor index
        /// </summary>
        int actorIndex=0;

        /// <summary>
        /// Equip window actor
        /// </summary>
        GameActor actor;

        /// <summary>
        /// Currently used skill
        /// </summary>
        Skill skill;

        /// <summary>
        /// Managed help window
        /// </summary>
        WindowHelp helpWindow;

        /// <summary>
        /// Managed status window
        /// </summary>
        WindowSkillStatus statusWindow;

        /// <summary>
        /// Managed skill window
        /// </summary>
        WindowSkill skillWindow;

        /// <summary>
        /// Managed target window
        /// </summary>
        WindowTarget targetWindow;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_index">actor index</param>
        public SceneSkill(int _index)
        {
            actorIndex = _index;
        }

        /// <summary>
        /// Initialize (default : actor_index = 0, equip_index = 0)
        /// </summary>
        public override void LoadSceneContent()
        {
            Initialize(actorIndex);
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="actor_index">actor index</param>
        public void Initialize(int actor_index)
        {
            this.actorIndex = actor_index;
            // Get actor
            actor = InGame.Party.Actors[actor_index];
            InitializeWindows();
        }

        /// <summary>
        /// Windows initialization
        /// </summary>
        void InitializeWindows()
        {
            // Make help window, status window, and skill window
            helpWindow = new WindowHelp();
            statusWindow = new WindowSkillStatus(actor);
            skillWindow = new WindowSkill(actor);
            // Associate help window
            skillWindow.HelpWindow = helpWindow;
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
            statusWindow.Dispose();
            skillWindow.Dispose();
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
            statusWindow.Update();
            skillWindow.Update();
            targetWindow.Update();
            // If skill window is active: call update_skill
            if (skillWindow.IsActive)
            {
                UpdateSkill();
                return;
            }
            // If skill target is active: call update_target
            else if (targetWindow.IsActive)
            {
                UpdateTarget();
                return;
            }
        }

        /// <summary>
        /// Frame Update (if skill window is active)
        /// </summary>
        void UpdateSkill()
        {
            // If B button was pressed
            if (Input.RMTrigger.B)
            {
                // Play cancel SE
                InGame.System.SoundPlay(Data.System.CancelSoundEffect);
                // Switch to menu screen
                Main.Scene = new SceneMenu(1);
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Get currently selected data on the skill window
                skill = skillWindow.Skill;
                // If unable to use
                if (skill == null || !actor.IsSkillCanUse(skill.Id))
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                // Play decision SE
                InGame.System.SoundPlay(Data.System.DecisionSoundEffect);
                // If effect scope is ally
                if (skill.Scope >= 3)
                {
                    // Activate target window
                    skillWindow.IsActive = false;
                    targetWindow.X = (skillWindow.Index + 1) % 2 * 304;
                    targetWindow.IsVisible = true;
                    targetWindow.IsActive = true;
                    // Set cursor position to effect scope (single / all)
                    if (skill.Scope == 4 || skill.Scope == 6)
                    {
                        targetWindow.Index = -1;
                    }
                    else if (skill.Scope == 7)
                    {
                        targetWindow.Index = actorIndex - 10;
                    }
                    else
                    {
                        targetWindow.Index = 0;
                    }
                }
                // If effect scope is other than ally
                else
                {
                    // If common event ID is valid
                    if (skill.CommonEventId > 0)
                    {
                        // Common event call reservation
                        InGame.Temp.CommonEventId = skill.CommonEventId;
                        // Play use skill SE
                        InGame.System.SoundPlay(skill.MenuSoundEffect);
                        // Use up SP
                        actor.Sp -= skill.SpCost;
                        // Remake each window content
                        statusWindow.Refresh();
                        skillWindow.Refresh();
                        targetWindow.Refresh();
                        // Switch to map screen
                        Main.Scene = new SceneMap();
                        return;
                    }
                }
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
                // Switch to different skill screen
                Main.Scene = new SceneSkill(actorIndex);
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
                // Switch to different skill screen
                Main.Scene = new SceneSkill(actorIndex);
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
                // Erase target window
                skillWindow.IsActive = true;
                targetWindow.IsVisible = false;
                targetWindow.IsActive = false;
                return;
            }
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // If unable to use because SP ran out
                if (!actor.IsSkillCanUse(skill.Id))
                {
                    // Play buzzer SE
                    InGame.System.SoundPlay(Data.System.BuzzerSoundEffect);
                    return;
                }
                // If target is all
                bool _used = false;
                if (targetWindow.Index == -1)
                {
                    // Apply skill use effects to entire party
                    _used = false;
                    foreach (GameActor party_actor in InGame.Party.Actors)
                    {
                        _used |= party_actor.SkillEffect(actor, skill);
                    }
                }
                // If target is user
                GameActor _target = null;
                if (targetWindow.Index <= -2)
                {
                    // Apply skill use effects to target actor
                    _target = InGame.Party.Actors[targetWindow.Index + 10];
                    _used = _target.SkillEffect(actor, skill);
                }
                // If single target
                if (targetWindow.Index >= 0)
                {
                    // Apply skill use effects to target actor
                    _target = InGame.Party.Actors[targetWindow.Index];
                    _used = _target.SkillEffect(actor, skill);
                }
                // If skill was used
                if (_used)
                {
                    // Play skill use SE
                    InGame.System.SoundPlay(skill.MenuSoundEffect);
                    // Use up SP
                    actor.Sp -= skill.SpCost;
                    // Remake each window content
                    statusWindow.Refresh();
                    skillWindow.Refresh();
                    targetWindow.Refresh();
                    // If entire party is dead
                    if (InGame.Party.IsAllDead)
                    {
                        // Switch to game over screen
                        Main.Scene = new SceneGameover();
                        return;
                    }
                    // If command event ID is valid
                    if (skill.CommonEventId > 0)
                    {
                        // Command event call reservation
                        InGame.Temp.CommonEventId = skill.CommonEventId;
                        // Switch to map screen
                        Main.Scene = new SceneMap();
                        return;
                    }
                }
                // If skill wasn't used
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
