using System;
using System.Linq;

namespace HesssLaw {
	class MainClass {
		public static int Minimum;
		public static int Maximum;
		public static ChemicalEquation target;
		public static ChemicalEquation[] knowns;

		public static bool Iterate(ChemicalEquation eq, int i) {
			for ( int j = Minimum; j <= Maximum; ++j ) {
				ChemicalEquation e = eq + knowns[i] * j;
				if ( i < knowns.Length - 1 ) {
					if ( Iterate(e, i + 1) ) {
						Console.WriteLine("Equation #{0} was {1}multiplied by {2}", i + 1, j < 0 ? "flipped and " : "", Math.Abs(j));
						return true;
					}
				} else {
					e.Normalize();
					if ( e.Equals(target) ) {
						Console.WriteLine();
#if DEBUG	
						Console.WriteLine("{0} == {1}", target, e);
#endif
						Console.WriteLine("Enthalpy = {0}", e.Enthalpy * target.Products[0].Amount / e.Products.First(mol => mol.Name.Equals(target.Products[0].Name)).Amount);
						Console.WriteLine("Equation #{0} was {1}multiplied by {2}", i + 1, j < 0 ? "flipped and " : "", Math.Abs(j));
						return true;
					} else {
#if DEBUG
						Console.WriteLine("{0} != {1}", target, e);
#endif
					}
				}
				if ( i == 0 ) {
					Console.WriteLine("Top of tree is {0} / {1} done.", j - Minimum + 1, Maximum - Minimum);
				}
			}
			return false;
		}

		public static void Main(string[] args) {
			//*
			Console.Write("Search depth = ");
			Minimum = -(Maximum = int.Parse(Console.ReadLine()));
			Console.WriteLine();
			Console.Write("Target equation: ");
			target = ChemicalEquation.Parse(Console.ReadLine());
			Console.WriteLine();
			int num = 0;
			Console.Write("There are   known equations.\rThere are ");
			while ( true ) {
				ConsoleKeyInfo key = Console.ReadKey(true);
				if ( key.KeyChar >= '0' && key.KeyChar <= '9' ) {
					num *= 10;
					num += key.KeyChar - '0';
				} else if ( key.Key == ConsoleKey.Backspace ) {
					num /= 10;
				} else if ( key.Key == ConsoleKey.Enter ) {
					break;
				} else {
					continue;
				}
				Console.Write("\rThere are {0} known equations.\rThere are {0}", num);
			}
			Console.WriteLine();
			knowns = new ChemicalEquation[num];
			for ( int i = 0; i < knowns.Length; ++i ) {
				Console.Write("Equation #{0}: ", i + 1);
				string eq = Console.ReadLine();
				Console.Write("Equation #{0}'s ΔH: ", i + 1);
				knowns[i] = ChemicalEquation.Parse(eq, double.Parse(Console.ReadLine()));
			}
			Console.WriteLine();
			// */
			target.Normalize();
			if ( !Iterate(new ChemicalEquation(), 0) ) {
				Console.WriteLine("Unable to find equation... :(");
			}
		}
	}
}
