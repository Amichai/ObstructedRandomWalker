using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Common;

namespace ImageConvert {
	class Program {
		static void Main(string[] args) {
			string filename = @"C:\Users\Amichai\Downloads\sound.bmp";
			Image bitmap = Bitmap.FromFile(filename);
			Bitmap bitmap2 = new Bitmap(bitmap);
			int[][] doubleArray = bitmap2.BitmapToDoubleArray(".bmp", 0);
			StreamWriter sw = new StreamWriter("soundFile.txt");
			int width = doubleArray.GetLength(0);
			int height = doubleArray[0].GetLength(0);
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					sw.Write(doubleArray[i][j].ToString() + " ");
				}sw.Write("\n");
			} 
		}
	}
}
