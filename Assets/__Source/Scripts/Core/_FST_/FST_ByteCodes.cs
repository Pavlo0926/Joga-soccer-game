/////////////////////////////////////////////////////////////////////////////////
//
//  FST_ByteCodes.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	One globally accessible central class that contains all 
//                  bytecodes used in network events.
//
/////////////////////////////////////////////////////////////////////////////////

namespace FastSkillTeam {
    public static class FST_ByteCodes
    {
        public const byte AMBIENT_AUDIO_CODE = 102;
        public const byte AUDIO_CODE = 101;

        public const byte KICK = 1;
        public const byte MOVE_OBS = 2;
        public const byte TURN_CHANGE = 3;
        public const byte GOT_TURN_CHANGE = 4;
        public const byte GOAL_SCORED = 5;
        public const byte FORMATION_RESET = 6;
        public const byte GOT_OB_ID = 7;
        public const byte SELECTED_DISK = 8;
    }
}