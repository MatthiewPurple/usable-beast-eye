using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using usable_beast_eye;
using Il2Cppnewbattle_H;

[assembly: MelonInfo(typeof(UsableBeastEye), "Usable Beast Eye", "1.1.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace usable_beast_eye;
public class UsableBeastEye : MelonMod
{
    // After getting the affinities of a demon
    [HarmonyPatch(typeof(datSkillHelp_msg), nameof(datSkillHelp_msg.Get))]
    private class Patch
    {
        public static void Postfix(int id, ref string __result)
        {
            if (id == 219) __result = "Adds 2 blinking Press Turn Icons. \nUnusable when the left most \nicon is blinking.";
            if (id == 220) __result = "Adds 4 blinking Press Turn Icons. \nUnusable when the user is not \nfighting alone.";
        }
    }

    // After displaying the battle command panel
    [HarmonyPatch(typeof(nbCommSelProcess), nameof(nbCommSelProcess.DispCommandList2))]
    private class Patch3
    {
        public static void Postfix(ref nbCommSelProcessData_t s)
        {
            // If it's Demi-fiend's turn 
            if (s.my.formindex == 0)
            {
                // If he has Beast Eye
                if (s.commlist[0].Contains(219))
                {
                    // If the left most Press Turn Icon is blinking
                    if (nbMainProcess.nbGetMainProcessData().press4_ten != nbMainProcess.nbGetMainProcessData().press4_p)
                    {
                        // Disable Beast Eye
                        s.commdisable[0][s.commlist[0].IndexOf(219)] = 1;
                    }
                }

                // If he has Dragon Eye
                if (s.commlist[0].Contains(220))
                {
                    // If the left most Press Turn Icon is blinking
                    if ((nbMainProcess.nbGetMainProcessData().press4_ten != nbMainProcess.nbGetMainProcessData().press4_p)
                        || (nbMainProcess.nbGetMainProcessData().playerpcnt != 1))
                    {
                        // Disable Dragon Eye
                        s.commdisable[0][s.commlist[0].IndexOf(220)] = 1;
                    }
                }
            }
        }
    }

    // When launching the game
    public override void OnInitializeMelon()
    {
        // Replace Radiance with Beast Eye
        tblHearts.fclHeartsTbl[25].Skill[1].ID = 219;

        // Add Dragon Eye to Masakados
        tblHearts.fclHeartsTbl[25].Skill[7].ID = 220;
        tblHearts.fclHeartsTbl[25].Skill[7].TargetLevel = 1;

        // Raises Beast Eye's cost to 150 MP
        datNormalSkill.tbl[219].cost = 150;

        // Raises Dragon Eye's cost to 150 MP
        datNormalSkill.tbl[220].cost = 150;
    }
}
