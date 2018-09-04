  
  
// ReSharper disable UnusedMember.Global
using System;

namespace Lindholm.Bots{
	public enum AiHero {
	  
		Recommended, 
	  
		Ana, 
	  
		Bastion, 
	  
		Lucio, 
	  
		McCree, 
	  
		Mei, 
	  
		Reaper, 
	  
		Roadhog, 
	  
		Soldier76, 
	  
		Sombra, 
	  
		Torbjorn, 
	  
		Zarya, 
	  
		Zenyatta, 
	  
	}

	public static class AiHeroExtension
	{
		public static Deltin.CustomGameAutomation.AIHero ToDeltin(this AiHero lindholmHero)
		{
			Deltin.CustomGameAutomation.AIHero deltinHero = (Deltin.CustomGameAutomation.AIHero) Enum.Parse(typeof(Deltin.CustomGameAutomation.AIHero), lindholmHero.ToString());
			return deltinHero;
		}
	}
}



