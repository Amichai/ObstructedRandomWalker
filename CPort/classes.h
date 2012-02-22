#pragma once

class Heatmap{
	public:
		Heatmap(int,int,int,int);
private:
	int stepsToAssess, magnification, size, nt,ns;
	Vector<Vector<Vector<Vector<int>>> test;
};

