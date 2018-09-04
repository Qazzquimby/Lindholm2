  
  
// ReSharper disable UnusedMember.Global
using System;
using Deltin.CustomGameAutomation;

namespace Lindholm.Bots{
	public enum Difficulty {
	  
		Easy, 
	  
		Medium, 
	  
		Hard, 
	  
	}


	public static class DifficultyExtension
	{
		public static Deltin.CustomGameAutomation.Difficulty ToDeltin(this Difficulty lindholmDifficulty)
		{
			Deltin.CustomGameAutomation.Difficulty deltinDifficulty = (Deltin.CustomGameAutomation.Difficulty) Enum.Parse(typeof(Deltin.CustomGameAutomation.Difficulty), lindholmDifficulty.ToString());
			return deltinDifficulty;
		}
	}
}



