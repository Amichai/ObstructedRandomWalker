// CPort.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

Heatmap::Heatmap(int ns, int nt, int magnification, int size) {
	Heatmap::stepsToAssess = ns* ns* nt *nt;
	Heatmap::nt = nt;
	Heatmap::ns = ns;
	Heatmap::magnification = magnification;
	Heatmap::size = size * magnification;
	for(int i=0; i < nt; i++){

	}
}


int _tmain(int argc, _TCHAR* argv[])
{
	Heatmap heatmap = Heatmap(5,20,5,10);
	return 0;
}

